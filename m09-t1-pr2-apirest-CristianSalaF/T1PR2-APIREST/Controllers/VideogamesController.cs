using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using T1PR2_APIREST.Context;
using T1PR2_APIREST.Models;
using System.Security.Claims;
using T1PR2_APIREST.DTOs;

namespace T1PR2_APIREST.Controllers
{
    /// <summary>
    /// Controller for managing video games.
    /// This controller handles CRUD operations for video games, as well as voting functionality.
    /// It uses Entity Framework Core for database operations.
    /// It provides endpoints for creating, reading, updating, and deleting video games.
    /// It also allows users to vote for their favorite games.
    /// This controller is secured with authentication and authorization mechanisms.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VideogameController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for the VideogameController class.
        /// This constructor takes AppDbContext as a parameter.
        /// It is used to initialize the controller with the database context.
        /// This allows the controller to interact with the database and perform CRUD operations on video games.
        /// </summary>
        /// <param name="context"></param>
        public VideogameController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all video games.
        /// This method retrieves a list of all video games from the database.
        /// It includes the number of votes for each game.
        /// It is accessible to all users (anonymous).
        /// Example call: GET /api/videogame
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get a video game by ID.
        /// This method retrieves a specific video game from the database using its ID.
        /// It includes the number of votes for the game.
        /// It is accessible to all users (anonymous).
        /// Example call: GET /api/videogame/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a new video game.
        /// This method adds a new video game to the database.
        /// It requires admin authorization.
        /// It is accessible only to users with the "Admin" role.
        /// Example call: POST /api/videogame
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        /// <summary>
        /// Update an existing video game.
        /// This method updates the details of a specific video game in the database using its ID.
        /// It requires admin authorization.
        /// It is accessible only to users with the "Admin" role.
        /// /// Example call: PUT /api/videogame/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updated"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete a video game.
        /// This method removes a specific video game from the database using its ID.
        /// It requires admin authorization.
        /// It is accessible only to users with the "Admin" role.
        /// Example call: DELETE /api/videogame/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Vote for a video game.
        /// This method allows a user to vote for their favorite video game.
        /// It requires user authentication.
        /// It is accessible only to authenticated users.
        /// Example call: POST /api/videogame/vote/5
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpPost("vote/{gameId}")]
        [Authorize]
        public async Task<IActionResult> Vote(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var gameExists = await _context.Games.AnyAsync(g => g.Id == gameId);
            if (!gameExists) return NotFound("No s'ha trobat el joc.");

            bool alreadyVoted = await _context.Votes.AnyAsync(v => v.UserId == userId && v.GameId == gameId);
            if (alreadyVoted) return BadRequest("Ja has votat per aquest joc.");

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