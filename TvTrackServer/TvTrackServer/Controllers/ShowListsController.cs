﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using TVTrack.Models.Database;
using TVTrack.Models.TvMaze;
using TvTrackServer.Models.Dto;
using TvTrackServer.TvMazeConnector;

namespace TvTrackServer.Controllers;

[Route("showlists")]
[ApiController]
public class ShowListsController : CustomControllerBase
{
    private readonly TvTrackServerDbContext _context;
    private readonly IMapper _mapper;
    private readonly TvMazeClient _tvMazeClient;

    public ShowListsController(TvMazeClient client, TvTrackServerDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
        _tvMazeClient = client;
    }

    // GET: /showlists?username=user
    [SwaggerOperation(Summary = "Get user's show lists overview")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowList>>> GetShowLists([FromQuery] string username)
    {
        var user = await FindByUsernameWithShowListsAsync(username);

        if (user == null)
        {
            return BadRequest($"User with username '{username}' doesn't exist.");
        }

        var listsDtos = _mapper.Map<List<ShowList>, List<ShowListPreviewDto>>(user.ShowLists);
        return Ok(listsDtos);
    }

    // GET: /showlists/5
    [SwaggerOperation(Summary = "Get show list detail including detail of shows in it")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShowList(int id, [FromQuery]string username)
    {
        var showList = await _context.ShowLists.AsNoTracking().Include(e => e.Shows).FirstOrDefaultAsync(e => e.Id == id);
        if (showList == null) return BadRequest("No show list with given id exists.");
        var newShowList = new List<ShowPreviewDto>();
        var user = await FindByUsernameAsync(username);
        foreach (var showListItem in showList.Shows)
        {
            var show = await _tvMazeClient.GetShowDetails(showListItem.TvMazeId);
            if (!string.IsNullOrEmpty(username))
            {
                await LoadUserActivityInfo(show.Id, user, show);
            }

            newShowList.Add(_mapper.Map<ShowPreviewDto>(show));
        }
        var showListDto = _mapper.Map<ShowListDetailDto>(showList);
        showListDto.Shows = newShowList;
        return Ok(showListDto);
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
        }
    }

    // GET: /showlists/available/5/2
    [SwaggerOperation(Summary = "Get shows for adding new show to user list")]
    [HttpGet("available/{username}/{showId}")]
    public async Task<IActionResult> GetAvailableForAddToShowList(string username, int showId)
    {
        var userShowLists = await _context.ShowLists.AsNoTracking().Where(e => e.User.Username == username).Include(x => x.Shows).ToListAsync();
        var resultList = new List<ShowListAvailableDto>();
        foreach(var showList in userShowLists)
        {
            if (!showList.Shows.Any(x => x.TvMazeId == showId))
            {
                resultList.Add(_mapper.Map<ShowListAvailableDto>(showList));
            }
        }
        return Ok(resultList);
    }

    // PUT: showlists/5
    [SwaggerOperation(Summary = "Edits show list")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShowList(int id, CreateShowListDto showListDto)
    {
        var showList = await _context.ShowLists.FindAsync(id);
        if (showList == null) return NotFound();
        if (showList.Default) return BadRequest("Default list cannot be renamed.");

        showList.Name = showListDto.Name;
        showList.Description = showListDto.Description;

        _context.Entry(showList).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShowListExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: /showlists
    [SwaggerOperation(Summary = "Creates user's show list")]
    [HttpPost]
    public async Task<ActionResult<ShowList>> PostShowList(CreateShowListDto showListDto, [FromQuery] string username)
    {
        if (_context.ShowLists == null)
        {
            return Problem("Entity set 'TVTrackServerDbContext.ShowLists'  is null.");
        }
        var user = await FindByUsernameAsync(username);
        if (user == null) return BadRequest($"User {username} doesn't exist.");

        var showList = _mapper.Map<ShowList>(showListDto);
        showList.User = user;
        _context.ShowLists.Add(showList);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetShowList", new { id = showList.Id }, showList);
    }

    // DELETE: /showlists/5
    [SwaggerOperation(Summary = "Deletes user's show list, cannot delete default")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShowList(int id)
    {
        if (_context.ShowLists == null)
        {
            return NotFound();
        }
        var showList = await _context.ShowLists.FindAsync(id);
        if (showList == null)
        {
            return NotFound();
        }
        if (showList.Default)
        {
            return BadRequest("Default show list cannot be deleted.");
        }

        _context.ShowLists.Remove(showList);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [SwaggerOperation(Summary = "Add show to user's default list")]
    [HttpPost("default/shows")]
    public async Task<IActionResult> AddShowsToDefaultList(int tvMazeId, [FromQuery] string username)
    {
        if (_context.ShowLists == null) return BadRequest("No lists in the database.");
        var user = await FindByUsernameWithShowListsAsync(username);
        if (user == null) return BadRequest($"No user wíth username {username}.");
        var defaultList = user.ShowLists.FirstOrDefault(l => l.Default);
        if (defaultList == null) return Problem("User has no default list.");

        return await AddShowToList(tvMazeId, defaultList);
    }

    [SwaggerOperation(Summary = "Add show to given show list")]
    [HttpPost("{listId}/shows")]
    public async Task<IActionResult> AddShowToList(int listId, int tvMazeId)
    {
        if (_context.ShowLists == null) return BadRequest("No lists in the database.");
        var showList = await _context.ShowLists.FindAsync(listId);
        if (showList == null) return BadRequest("No such list exists.");

        return await AddShowToList(tvMazeId, showList);
    }

    private async Task<IActionResult> AddShowToList(int tvMazeId, ShowList showList)
    {
        var showAlreadyInList = _context.ShowListItems.FirstOrDefault(i => i.TvMazeId == tvMazeId && i.ShowListId == showList.Id);
        if (showAlreadyInList != null) return Ok("Already in list.");

        var show = new ShowListItem() { TvMazeId = tvMazeId };
        showList.Shows.Add(show);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [SwaggerOperation(Summary = "Remove show from user's default list")]
    [HttpDelete("default/shows/{tvMazeShowId}")]
    public async Task<IActionResult> DeleteShowFromDefaultList(int tvMazeShowId, [FromQuery] string username)
    {
        var user = await FindByUsernameWithShowListsAsync(username);
        if (user == null) return NotFound("No user with given username.");
        var usersDefaultList = user.ShowLists.FirstOrDefault(l => l.Default);
        if (usersDefaultList == null) return Problem("User has no default list, even though every user should.");
        return await DeleteShowFromList(usersDefaultList, tvMazeShowId);
    }

    [SwaggerOperation(Summary = "Remove show from given show list")]
    [HttpDelete("{listId}/shows/{tvMazeShowId}")]
    public async Task<IActionResult> DeleteShowFromList(int listId, int tvMazeShowId)
    {
        var listToRemoveFrom = _context.ShowLists.Find(listId);
        if (listToRemoveFrom == null) return NotFound("List does not exist.");
        return await DeleteShowFromList(listToRemoveFrom, tvMazeShowId);
    }

    private async Task<IActionResult> DeleteShowFromList(ShowList listToRemoveFrom, int tvMazeShowId)
    {
        var showToRemove = await _context.ShowListItems.FirstOrDefaultAsync(s => s.TvMazeId == tvMazeShowId && s.ShowListId == listToRemoveFrom.Id);
        if (showToRemove == null) return Ok("Already not present.");
        _context.ShowListItems.Remove(showToRemove);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private bool ShowListExists(int id)
    {
        return (_context.ShowLists?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
