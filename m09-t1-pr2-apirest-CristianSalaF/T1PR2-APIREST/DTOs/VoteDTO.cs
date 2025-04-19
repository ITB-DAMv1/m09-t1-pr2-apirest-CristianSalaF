using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    /// <summary>
    /// Represents a data transfer object for a vote.
    /// </summary>
    public class VoteDTO
    {
        /// <summary>
        /// Gets or sets the ID of the game being voted on.
        /// </summary>
        [Required]
        public int GameId { get; set; }
    }
}
