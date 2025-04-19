using System.ComponentModel.DataAnnotations;

namespace T1PR2_APIREST.DTOs
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// Contains the user's email and password.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The password of the user.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
