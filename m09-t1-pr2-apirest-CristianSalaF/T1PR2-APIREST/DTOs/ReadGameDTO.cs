using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    /// <summary>
    /// Represents a data transfer object for reading game information.
    /// </summary>
    public class ReadGameDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the game.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the game.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the game.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the developer team.
        /// </summary>
        public string DeveloperTeamName { get; set; }

        /// <summary>
        /// Gets or sets the URL of the game's image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the vote count for the game.
        /// </summary>
        public int VoteCount { get; set; }
    }
}
