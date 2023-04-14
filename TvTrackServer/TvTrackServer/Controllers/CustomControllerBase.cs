using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TVTrack.Models.Database;

namespace TvTrackServer.Controllers;

public class CustomControllerBase : ControllerBase
{
    private readonly TvTrackServerDbContext _context;

    public CustomControllerBase(TvTrackServerDbContext dbContext) {
        _context = dbContext;
    }

    protected async Task<User?> FindByUsernameAsync(string? username)
    {
        if (username == null) return null;
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    protected async Task<User?> FindByUsernameWithShowListsAsync(string? username)
    {
        if (username == null) return null;
        return await _context.Users.Include(u => u.ShowLists).ThenInclude(l => l.Shows).FirstOrDefaultAsync(u => u.Username == username);
    }

    protected async Task<User?> FindByUsernameWithShowActivitiesAsync(string? username)
    {
        if (username == null) return null;
        return await _context.Users.Include(u => u.ShowActivities).ThenInclude(s => s.EpisodeActivities).FirstOrDefaultAsync(u => u.Username == username);
    }
}
