using Microsoft.AspNetCore.Identity;

namespace T1PR2_APIREST.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
