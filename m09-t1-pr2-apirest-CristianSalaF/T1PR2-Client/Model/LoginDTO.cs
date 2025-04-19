using System.ComponentModel.DataAnnotations;

namespace T1PR2_Client.Model
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for login credentials.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}
