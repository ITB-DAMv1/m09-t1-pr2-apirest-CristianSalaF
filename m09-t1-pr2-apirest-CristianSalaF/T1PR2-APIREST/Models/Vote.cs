using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace T1PR2_APIREST.Models
{
    /// <summary>
    /// Represents a vote in the system.
    /// </summary>
    public class Vote
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vote.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who cast the vote.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who cast the vote.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the game associated with the vote.
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Gets or sets the game associated with the vote.
        /// </summary>
        public Game Game { get; set; }
    }
}
