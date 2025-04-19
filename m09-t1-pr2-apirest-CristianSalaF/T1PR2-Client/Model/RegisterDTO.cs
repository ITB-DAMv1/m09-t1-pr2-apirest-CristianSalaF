using System.ComponentModel.DataAnnotations;

namespace T1PR2_Client.Model
{
    /// <summary>
    /// Data Transfer Object for user registration.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
