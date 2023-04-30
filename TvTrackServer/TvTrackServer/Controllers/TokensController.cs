using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Services;

namespace TvTrackServer.Controllers
{
    [Route("tokens")]
    [ApiController]
    public class TokensController : CustomControllerBase
    {
        private readonly TvTrackServerDbContext _dbContext;
        private readonly TVGoogleCalendarService _calendarService;

        public TokensController(TvTrackServerDbContext dbContext,
            TVGoogleCalendarService calendarService) : base(dbContext)
        {
            _dbContext = dbContext;
            _calendarService = calendarService;
        }

        // PUT: tokens/fcm/{username}
        [HttpPut("fcm/{username}")]
        public async Task<IActionResult> PutFCMDeviceToken(string username, string fcmDeviceToken)
        {
            var user = await FindByUsernameWithTokensAsync(username);
            if (user == null)
            {
                return NotFound($"User {username} does not exist");
            }

            user.Tokens.FCMDeviceToken = fcmDeviceToken;
            _dbContext.Entry(user).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // PUT: tokens/gc/{username}
        [HttpPut("gc/{username}")]
        public async Task<IActionResult> PutGoogleCalendarTokens(string username, string gcApiToken, string gcRefreshToken)
        {
            var user = await FindByUsernameWithTokensAsync(username);
            if (user == null)
            {
                return NotFound($"User {username} does not exist");
            }

            user.Tokens.GoogleCalendarToken = gcApiToken;
            user.Tokens.GoogleCalendarRefreshToken = gcRefreshToken;
            _dbContext.Entry(user).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> DebugCalendar()
        {
            await _calendarService.Debug();

            return Ok();
        }
    }
}
