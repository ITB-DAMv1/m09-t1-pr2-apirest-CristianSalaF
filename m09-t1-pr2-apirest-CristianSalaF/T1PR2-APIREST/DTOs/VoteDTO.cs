using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    public class VoteDTO
    {
        [Required]
        public int GameId { get; set; }
    }
}
