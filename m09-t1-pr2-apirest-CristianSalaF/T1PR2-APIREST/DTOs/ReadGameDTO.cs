using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    public class ReadGameDTO
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string DeveloperTeamName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
