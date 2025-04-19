using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace T1PR2_Client.Pages
{
    /// <summary>
    /// Represents the model for the Logout page.
    /// </summary>
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// Handles GET requests to log out the user by clearing the session and redirecting to the Index page.
        /// </summary>
        /// <returns>A redirection to the Index page.</returns>
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}