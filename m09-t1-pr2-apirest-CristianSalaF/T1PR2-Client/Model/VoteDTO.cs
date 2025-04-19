using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    /// <summary>
    /// Represents a vote for a specific game.
    /// </summary>
    public class VoteDTO
    {
        /// <summary>
        /// Gets or sets the ID of the game being voted for.
        /// </summary>
        public int GameId { get; set; }
    }
}
