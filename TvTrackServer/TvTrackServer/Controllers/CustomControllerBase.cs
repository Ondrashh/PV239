using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models;
using TvTrackServer.Models.Database;

namespace TvTrackServer.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        private readonly TvTrackServerDbContext _context;

        public CustomControllerBase(TvTrackServerDbContext dbContext) {
            _context = dbContext;
        }

        protected Task<User?> FindByUsernameAsync(string username)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
