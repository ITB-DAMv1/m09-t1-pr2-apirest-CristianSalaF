using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string DeveloperTeamName { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
