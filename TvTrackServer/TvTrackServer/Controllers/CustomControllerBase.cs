using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models;
using TvTrackServer.Models.Database;

namespace TvTrackServer.Controllers;

public class CustomControllerBase : ControllerBase
{
    private readonly TvTrackServerDbContext _context;

    public CustomControllerBase(TvTrackServerDbContext dbContext) {
        _context = dbContext;
    }

    protected async Task<User?> FindByUsernameAsync(string? username, bool includeShowActivity = false, bool includeShowLists = false)
    {
        if (username == null) return null;
        var usersQuery = _context.Users;
        if (includeShowActivity) usersQuery.Include(u => u.ShowActivities);
        if (includeShowLists) usersQuery.Include(u => u.ShowLists);
        return await usersQuery.FirstOrDefaultAsync(u => u.Username == username);
    }
}
