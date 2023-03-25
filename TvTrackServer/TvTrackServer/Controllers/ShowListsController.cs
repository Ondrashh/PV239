using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvTrackServer.Models.Database;
using TvTrackServer.Models.Dto;

namespace TvTrackServer.Controllers
{
    [Route("showlists")]
    [ApiController]
    public class ShowListsController : CustomControllerBase
    {
        private readonly TvTrackServerDbContext _context;
        private readonly IMapper _mapper;

        public ShowListsController(TvTrackServerDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /showlists?username=user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowList>>> GetShowLists([FromQuery] string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return BadRequest($"User with username '{username}' doesn't exist.");
            }

            if (_context.ShowLists == null)
            {
                return NotFound();
            }

            var lists = await _context.ShowLists.Where(l => l.UserId == user.Id).ToListAsync();
            var listsDtos = _mapper.Map<List<ShowList>, List<ShowListPreviewDto>>(lists);
            return Ok(listsDtos);
        }

        // GET: /showlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowList>> GetShowList(int id)
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

            // TODO here will be the extra linking stuff
            // return showList;
            return Ok("This endpoint is under construction.");
        }

        // PUT: showlists/5
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

        [HttpPost("default/shows")]
        public async Task<IActionResult> AddShowsToDefaultList(int tvMazeId, [FromQuery] string username)
        {
            if (_context.ShowLists == null) return BadRequest("No lists in the database.");
            var user = await FindByUsernameAsync(username);
            if (user == null) return BadRequest($"No user wíth username {username}.");
            var defaultList = _context.ShowLists.FirstOrDefault(sl => sl.UserId == user.Id && sl.Default);
            if (defaultList == null) return Problem("User has no default list.");

            return await AddShowToList(tvMazeId, defaultList);
        }

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

        [HttpDelete("default/shows/{tvMazeShowId}")]
        public async Task<IActionResult> DeleteShowFromDefaultList(int tvMazeShowId, [FromQuery] string username)
        {
            var user = await FindByUsernameAsync(username);
            if (user == null) return NotFound("No user with given username.");
            var usersDefaultList = await _context.ShowLists.FirstOrDefaultAsync(l => l.UserId == user.Id && l.Default);
            if (usersDefaultList == null) return Problem("User has no default list, even though every user should.");
            return await DeleteShowFromList(usersDefaultList, tvMazeShowId);
        }



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
}
