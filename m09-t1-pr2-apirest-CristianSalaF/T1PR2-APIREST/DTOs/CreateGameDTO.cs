using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating a new game.
    /// </summary>
    public class CreateGameDTO
    {
        /// <summary>
        /// Title of the game.
        /// This property is required and must be at least 2 characters long.
        /// </summary>
        [Required]
        [MinLength(2)]
        public string Title { get; set; }

        /// <summary>
        /// Description of the game.
        /// This property is required.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Name of the developer team.
        /// This property is required.
        /// </summary>
        [Required]
        public string DeveloperTeamName { get; set; }

        /// <summary>
        /// URL of the game's image.
        /// Optional property.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
