using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models.Database;

namespace TvTrackServer.Controllers
{
    [Route("shows")]
    [ApiController]
    public class ShowController : CustomControllerBase
    {
        private readonly TvTrackServerDbContext _context;
        public ShowController(TvTrackServerDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("{tvMazeId}")]
        public async Task<IActionResult> GetShow(int tvMazeId, [FromQuery] string? username)
        {
            // TODO this needs to be routed properly throught tvmaze
            var user = await FindByUsernameAsync(username);
            var showActivity = await _context.ShowActivities.FirstOrDefaultAsync(s => s.UserId == user.Id && s.TvMazeId == tvMazeId);
            return Ok(showActivity);
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
        // TODO mark one episode as watched

        [HttpPost("{tvMazeId}/ratings")]
        public async Task<IActionResult> PostRating(int tvMazeId, [FromQuery] string username, int rating)
        {
            var user = await FindByUsernameAsync(username);
            if (user == null) return BadRequest($"No user wíth username {username}.");
            if (rating < 0 || rating > 5) return BadRequest("Rating has to be between 0 and 5, included.");

            var userShowActivity = await _context.ShowActivities.Where(a => a.UserId == user.Id && a.TvMazeId == tvMazeId).FirstOrDefaultAsync();

            if (userShowActivity == null)
            {
                var newShowActivity = new ShowActivity
                {
                    TvMazeId = tvMazeId,
                    User = user,
                    UserRating = rating,
                    UserRated = true
                };
                _context.ShowActivities.Add(newShowActivity);
                await NoteUserRating(tvMazeId, rating, false);
            }
            else if (userShowActivity.UserRated == false)
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
}
