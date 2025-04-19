using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.Models
{
    /// <summary>
    /// Represents a game entity with details such as title, description, developer team name, and associated votes.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets or sets the unique identifier for the game.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        /// Gets or sets the collection of votes associated with the game.
        /// </summary>
        public ICollection<Vote> Votes { get; set; }
    }
}
