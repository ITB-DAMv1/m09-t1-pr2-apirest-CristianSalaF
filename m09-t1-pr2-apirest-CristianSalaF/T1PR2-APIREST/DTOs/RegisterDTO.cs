using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    /// <summary>
    /// Data Transfer Object for user registration.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The username of the user. Must be at least 4 characters long.
        /// </summary>
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        /// <summary>
        /// The password of the user. Must be at least 6 characters long.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
