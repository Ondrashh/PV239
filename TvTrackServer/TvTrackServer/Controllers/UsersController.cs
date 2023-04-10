﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models;
using TvTrackServer.Models.Database;

namespace TvTrackServer.Controllers;

[Route("users")]
[ApiController]
public class UsersController : CustomControllerBase
{
    private readonly TvTrackServerDbContext _context;

    public UsersController(TvTrackServerDbContext context) : base(context)
    {
        _context = context;
    }

    // GET: /users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        if (_context.Users == null)
        {
            return NotFound();
        }
        return await _context.Users.ToListAsync();
    }

    // GET: /users/user123
    [HttpGet("{username}")]
    public async Task<ActionResult<User>> GetUser(string username)
    {
        if (_context.Users == null)
        {
            return NotFound();
        }
        var user = await FindByUsernameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST /users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(string username)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'TVTrackServerDbContext.Users'  is null.");
        }

        if (await FindByUsernameAsync(username) != null)
        {
            return BadRequest($"User with username {username} already exists.");
        }

        var user = new User() { Username = username };
        var defaultList = new ShowList()
        {
            Name = "Watch Next",
            Description = "",
            Default = true,
            User = user
        };
        user.ShowLists.Add(defaultList);
        _context.Users.Add(user);
        _context.ShowLists.Add(defaultList);
        await _context.SaveChangesAsync();

        return Created(nameof(GetUser), user.Username);
    }

    // DELETE: users/user123
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        if (_context.Users == null)
        {
            return NotFound();
        }
        User? user = await FindByUsernameAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}