using Microsoft.AspNetCore.Identity;

namespace T1PR2_APIREST.Models
{
    /// <summary>
    /// Represents a user in the system, inheriting from IdentityUser.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the collection of votes associated with the user.
        /// </summary>
        public ICollection<Vote> Votes { get; set; }
    }
}
