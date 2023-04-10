﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models.Database;
using TvTrackServer.Models.TvMaze;
using TvTrackServer.TvMazeConnector;

namespace TvTrackServer.Controllers;

[Route("shows")]
[ApiController]
public class ShowController : CustomControllerBase
{
    private readonly TvTrackServerDbContext _context;
    private readonly TvMazeClient _tvMazeClient;
    public ShowController(TvTrackServerDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
        _tvMazeClient = new TvMazeClient();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string search)
    {
        var result = await _tvMazeClient.Search(search);
        return Ok(result);
    }

    [HttpGet("{tvMazeId}")]
    public async Task<IActionResult> GetShow(int tvMazeId, [FromQuery] string? username)
    {
        var user = await FindByUsernameAsync(username);
        var showDetails = await _tvMazeClient.GetShowDetails(tvMazeId);

        if (user == null) return Ok(showDetails);

        await LoadUserActivityInfo(tvMazeId, user, showDetails);
        await LoadTvTrackRatingInfo(tvMazeId, showDetails);
        return Ok(showDetails);
    }

    private async Task LoadUserActivityInfo(int tvMazeId, User user, Show showDetails)
    {
        // TODO include here but defs need to be updated for it to work
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

    [HttpPatch("{tvMazeId}/notifications")]
    public async Task<IActionResult> ToggleNotifications(int tvMazeId, [FromQuery] string username, [FromBody] bool enabled)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest("Invalid username.");

        var userShowActivity = await _context.ShowActivities.FirstOrDefaultAsync(s => s.UserId == user.Id && s.TvMazeId == tvMazeId);
        if (userShowActivity == null)
        {
            var newShowActivity = new ShowActivity()
            {
                Notifications = enabled,
                UserId = user.Id,
                TvMazeId = tvMazeId
            };
            _context.ShowActivities.Add(newShowActivity);
            await _context.SaveChangesAsync();
        }
        else
        {
            userShowActivity.Notifications = enabled;
            _context.Entry(userShowActivity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpPatch("{tvMazeId}/calendar")]
    public async Task<IActionResult> ToggleCalendar(int tvMazeId, [FromQuery] string username, [FromBody] bool enabled)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest("Invalid username.");

        var userShowActivity = await _context.ShowActivities.FirstOrDefaultAsync(s => s.UserId == user.Id && s.TvMazeId == tvMazeId);
        if (userShowActivity == null)
        {
            var newShowActivity = new ShowActivity()
            {
                Calendar = enabled,
                UserId = user.Id,
                TvMazeId = tvMazeId
            };
            _context.ShowActivities.Add(newShowActivity);
            await _context.SaveChangesAsync();
        }
        else
        {
            userShowActivity.Calendar = enabled;
            _context.Entry(userShowActivity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    // TODO mark all show as watched
    // TODO mark as watched up to episode X
    [HttpPatch("{showTvMazeId}/episodes/{episodeTvMazeId}")]
    public async Task<IActionResult> ToggleEpisodeWatchedStatus(int showTvMazeId, int episodeTvMazeId, [FromQuery] string username, [FromBody] bool watched)
    {
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest("No user with given username.");

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

        return Ok();
    }

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
