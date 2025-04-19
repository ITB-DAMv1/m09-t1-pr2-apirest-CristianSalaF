using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using T1PR2_APIREST.Context;
using T1PR2_APIREST.Models;
using System.Security.Claims;
using T1PR2_APIREST.DTOs;

namespace T1PR2_APIREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideogameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VideogameController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Videogame
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReadGameDTO>>> GetAll()
        {
            var games = await _context.Games
                .Include(g => g.Votes)
                .OrderByDescending(g => g.Votes.Count)
                .ToListAsync();

            var gameDtos = games.Select(game => new ReadGameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                DeveloperTeamName = game.DeveloperTeamName,
                ImageUrl = game.ImageUrl,
                VoteCount = game.Votes.Count
            }).ToList();

            return Ok(gameDtos);
        }


        // GET: api/Videogame/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ReadGameDTO>> GetById(int id)
        {
            var game = await _context.Games
                .Include(g => g.Votes)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) return NotFound();

            var gameDto = new ReadGameDTO()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                DeveloperTeamName = game.DeveloperTeamName,
                ImageUrl = game.ImageUrl,
                VoteCount = game.Votes.Count
            };

            return Ok(gameDto);
        }

        // POST: api/Videogame
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        // PUT: api/Videogame/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditGame(int id, Game updated)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            game.Title = updated.Title;
            game.Description = updated.Description;
            game.DeveloperTeamName = updated.DeveloperTeamName;
            game.ImageUrl = updated.ImageUrl;

            await _context.SaveChangesAsync();
            return Ok(game);
        }

        // DELETE: api/Videogame/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Videogame/vote/5
        [HttpPost("vote/{gameId}")]
        [Authorize]
        public async Task<IActionResult> Vote(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var gameExists = await _context.Games.AnyAsync(g => g.Id == gameId);
            if (!gameExists) return NotFound("Game not found.");

            bool alreadyVoted = await _context.Votes.AnyAsync(v => v.UserId == userId && v.GameId == gameId);
            if (alreadyVoted) return BadRequest("You have already voted for this game.");

            var vote = new Vote
            {
                UserId = userId,
                GameId = gameId
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return Ok("Vote recorded");
        }
    }
}