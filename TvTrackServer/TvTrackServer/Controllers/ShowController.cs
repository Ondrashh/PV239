using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using TVTrack.Models.Database;
using TVTrack.Models.TvMaze;
using TvTrackServer.Models.Dto;
using TvTrackServer.TvMazeConnector;
using TvTrackServer.Helpers;

namespace TvTrackServer.Controllers;

[Route("shows")]
[ApiController]
public class ShowController : CustomControllerBase
{
    private readonly TvTrackServerDbContext _context;
    private readonly TvMazeClient _tvMazeClient;
    public ShowController(TvMazeClient client, TvTrackServerDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
        _tvMazeClient = client;
    }

    [SwaggerOperation(Summary = "Search for show")]
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string search)
    {
        var result = await _tvMazeClient.Search(search);
        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get show details, including user's activity regarding it")]
    [HttpGet("{tvMazeId}")]
    public async Task<IActionResult> GetShow(int tvMazeId, [FromQuery] string? username)
    {
        var user = await FindByUsernameWithShowListsAsync(username);
        var showDetails = await _tvMazeClient.GetShowDetailsWithSeasonsAndEpisodes(tvMazeId);

        if (user == null) return Ok(showDetails);

        await LoadUserActivityInfo(tvMazeId, user, showDetails);
        await LoadTvTrackRatingInfo(tvMazeId, showDetails);

        showDetails.InUsersDefaultList = ShowInUsersDefaultList(tvMazeId, user);

        return Ok(showDetails);
    }

    private bool ShowInUsersDefaultList(int tvMazeId, User user)
    {
        var usersDefaultList = user.ShowLists.FirstOrDefault(e => e.Default);
        if (usersDefaultList == null) return false;
        var showItemInDefaultList = usersDefaultList.Shows.FirstOrDefault(s => s.TvMazeId == tvMazeId);
        return showItemInDefaultList != null;
    }

    private async Task LoadUserActivityInfo(int tvMazeId, User user, Show showDetails)
    {
        var usersShowActivity = await _context.ShowActivities.Include(s => s.EpisodeActivities).FirstOrDefaultAsync(s => s.UserId == user.Id && s.TvMazeId == tvMazeId);
        if (usersShowActivity != null)
        {
            showDetails.UserRated = usersShowActivity.UserRated;
            showDetails.UserRating = usersShowActivity.UserRating;
            showDetails.Notifications = usersShowActivity.Notifications;
            showDetails.Calendar = usersShowActivity.Calendar;

            foreach(var userEpisodeActivity in usersShowActivity.EpisodeActivities)
            {
                showDetails.Embedded.Episodes.First(e => e.Id == userEpisodeActivity.TvMazeId).UserWatched = userEpisodeActivity.Watched;
            }
        }
    }

    private async Task LoadTvTrackRatingInfo(int tvMazeId, Show showDetails)
    {
        var userRatedShow = await _context.UserRatedShows.FirstOrDefaultAsync(s => s.TvMazeId == tvMazeId);
        if (userRatedShow != null)
        {
            showDetails.Rating ??= new Rating();
            showDetails.Rating.TvTrackRatingCount = userRatedShow.UserRatingCount;
            showDetails.Rating.TvTrackRating = userRatedShow.UserRating;
        }
    }

    [SwaggerOperation(Summary = "Turn on or off notifications for this show")]
    [HttpPatch("{tvMazeId}/notifications")]
    public async Task<IActionResult> ToggleNotifications(int tvMazeId, [FromQuery] string username, [FromBody] EnabledDto enabledDto)
    {
        var user = await FindByUsernameWithShowActivitiesAsync(username);
        if (user == null) return BadRequest("Invalid username.");

        var userShowActivity = user.ShowActivities.FirstOrDefault(s=> s.TvMazeId == tvMazeId);
        if (userShowActivity == null)
        {
            userShowActivity = new ShowActivity()
            {
                Notifications = enabledDto.Enabled,
                UserId = user.Id,
                TvMazeId = tvMazeId,
                NextNotifyDate = DateTime.Today
            };

            _context.ShowActivities.Add(userShowActivity);
        }
        else
        {
            userShowActivity.Notifications = enabledDto.Enabled;
            _context.Entry(userShowActivity).State = EntityState.Modified;
        }

        var show = await _tvMazeClient.GetShowDetails(tvMazeId);

        var nextDate = show.GetNextEpisodeDate();
        if (!string.IsNullOrEmpty(show.Schedule?.Days.FirstOrDefault())
            && nextDate > userShowActivity.NextNotifyDate)
        {
            userShowActivity.NextNotifyDate = nextDate;
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [SwaggerOperation(Summary = "Turn on or off showing show in user's calendar")]
    [HttpPatch("{tvMazeId}/calendar")]
    public async Task<IActionResult> ToggleCalendar(int tvMazeId, [FromQuery] string username, [FromBody] EnabledDto enabledDto)
    {
        var user = await FindByUsernameWithShowActivitiesAsync(username);
        if (user == null) return BadRequest("Invalid username.");

        var userShowActivity = user.ShowActivities.FirstOrDefault(s => s.TvMazeId == tvMazeId);
        if (userShowActivity == null)
        {
            userShowActivity = new ShowActivity()
            {
                Calendar = enabledDto.Enabled,
                UserId = user.Id,
                TvMazeId = tvMazeId,
                NextNotifyDate = DateTime.Today
            };

            _context.ShowActivities.Add(userShowActivity);
        }
        else
        {
            userShowActivity.Calendar = enabledDto.Enabled;
            _context.Entry(userShowActivity).State = EntityState.Modified;
        }

        var show = await _tvMazeClient.GetShowDetails(tvMazeId);

        var nextDate = show.GetNextEpisodeDate();
        if (!string.IsNullOrEmpty(show.Schedule?.Days.FirstOrDefault())
            && nextDate > userShowActivity.NextNotifyDate)
        {
            userShowActivity.NextNotifyDate = nextDate;
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [SwaggerOperation(Summary = "Set whether the user has watched the entire series or not.")]
    [HttpPatch("{showTvMazeId}")]
    public async Task<IActionResult> SetWatchStatusOfShow(int showTvMazeId, [FromQuery] string username, [FromBody] WatchedDto watchedDto)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest("No user with given username.");

        Show showInfo = await _tvMazeClient.GetShowDetailsWithSeasonsAndEpisodes(showTvMazeId);
   
        foreach(var episode in showInfo.Embedded.Episodes)
        {
            await SetOneEpisodeWatchedStatus(showTvMazeId, episode.Id, user, watchedDto.Watched);
        }

        return Ok();
    }

    [SwaggerOperation(Summary = "Set whether the user has watched the whole season or not.")]
    [HttpPatch("{showTvMazeId}/seasons/{seasonNumber}")]
    public async Task<IActionResult> SetWatchStatusOfShow(int showTvMazeId, int seasonNumber, [FromQuery] string username, [FromBody] WatchedDto watchedDto)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest("No user with given username.");

        Show showInfo = await _tvMazeClient.GetShowDetailsWithSeasonsAndEpisodes(showTvMazeId);

        foreach (var episode in showInfo.Embedded.Episodes)
        {
            if(episode.Season == seasonNumber)
                await SetOneEpisodeWatchedStatus(showTvMazeId, episode.Id, user, watchedDto.Watched);
        }

        return Ok();
    }

    [SwaggerOperation(Summary = "Set whether the user has watched this episode or not")]
    [HttpPatch("{showTvMazeId}/episodes/{episodeTvMazeId}")]
    public async Task<IActionResult> ToggleEpisodeWatchedStatus(int showTvMazeId, int episodeTvMazeId, [FromQuery] string username, [FromBody] WatchedDto watchedDto)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest("No user with given username.");

        await SetOneEpisodeWatchedStatus(showTvMazeId, episodeTvMazeId, user, watchedDto.Watched);

        return Ok();
    }

    private async Task SetOneEpisodeWatchedStatus(int showTvMazeId, int episodeTvMazeId, User user, bool watched)
    {
        var userShowActivity = await FetchOrCreateUserShowActivityAsync(showTvMazeId, user);
        var episodeShowActivity = await FetchOrCreateUserEpisodeActivityAsync(userShowActivity.Id, episodeTvMazeId);

        if (watched)
        {
            episodeShowActivity.Watched = true;
        }
        else
        {
            _context.EpisodeActivities.Remove(episodeShowActivity);
        }
        await _context.SaveChangesAsync();
    }

    [SwaggerOperation(Summary = "Post user's show rating")]
    [HttpPost("{tvMazeId}/ratings")]
    public async Task<IActionResult> PostRating(int tvMazeId, [FromQuery] string username, int rating)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest($"No user wíth username {username}.");
        if (rating < 0 || rating > 5) return BadRequest("Rating has to be between 0 and 5, included.");

        var userShowActivity = await FetchOrCreateUserShowActivityAsync(tvMazeId, user);

        if (userShowActivity.UserRated == false)
        {
            userShowActivity.UserRated = true;
            userShowActivity.UserRating = rating;
            await NoteUserRating(tvMazeId, rating, false);
        }
        else
        {
            var lastRating = userShowActivity.UserRating;
            userShowActivity.UserRating = rating;
            await NoteUserRating(tvMazeId, rating, true, lastRating);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    private async Task<ShowActivity> FetchOrCreateUserShowActivityAsync(int tvMazeId, User user)
    {
        var userShowActivity = await _context.ShowActivities.Where(a => a.UserId == user.Id && a.TvMazeId == tvMazeId).FirstOrDefaultAsync();
        if (userShowActivity == null)
        {
            userShowActivity = new ShowActivity
            {
                TvMazeId = tvMazeId,
                User = user
            };
            _context.ShowActivities.Add(userShowActivity);
            await _context.SaveChangesAsync();
        }

        return userShowActivity;
    }

    private async Task<EpisodeActivity> FetchOrCreateUserEpisodeActivityAsync(int showActivityId, int episodeTvMazeId)
    {
        var episodeActivity = await _context.EpisodeActivities.Where(e => e.TvMazeId == episodeTvMazeId && e.ShowActivityId == showActivityId).FirstOrDefaultAsync();
        if (episodeActivity == null)
        {
            episodeActivity = new EpisodeActivity
            {
                TvMazeId = episodeTvMazeId,
                ShowActivityId = showActivityId
            };
            _context.EpisodeActivities.Add(episodeActivity);
            await _context.SaveChangesAsync();
        }
        return episodeActivity;
    }

    private async Task NoteUserRating(int tvMazeId, int newRating, bool ratedBefore, int oldRating = 0)
    {
        var userRatedShow = await _context.UserRatedShows.Where(s => s.TvMazeId == tvMazeId).FirstOrDefaultAsync();
        if (userRatedShow == null)
        {
            var newUserRatedShow = new UserRatedShow()
            {
                TvMazeId = tvMazeId,
                UserRatingCount = 1,
                UserRating = newRating
            };
            _context.UserRatedShows.Add(newUserRatedShow);
            await _context.SaveChangesAsync();
            return;
        }

        if (ratedBefore)
        {
            float newUserRating = (userRatedShow.UserRating * userRatedShow.UserRatingCount - oldRating + newRating) / userRatedShow.UserRatingCount;
            if (newUserRating < 0) newUserRating = 0;
            if (newUserRating > 5) newUserRating = 5;
            userRatedShow.UserRating = newUserRating;
        }
        else
        {
            float newUserRating = (userRatedShow.UserRating * userRatedShow.UserRatingCount + newRating) / (userRatedShow.UserRatingCount + 1);
            if (newUserRating < 0) newUserRating = 0;
            if (newUserRating > 5) newUserRating = 5;
            userRatedShow.UserRating = newUserRating;
            userRatedShow.UserRatingCount += 1;
        }
    }
}
