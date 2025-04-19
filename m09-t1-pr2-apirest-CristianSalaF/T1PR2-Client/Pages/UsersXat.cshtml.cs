using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T1PR2_Client.Tools;

namespace T1PR2_Client.Pages
{
    public class UsersXatModel : PageModel
    {
        public string DisplayName { get; set; } = "Usuari desconegut";
        
        public IActionResult OnGet()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login");
            }

            // Check if token is expired
            if (TokenHelper.IsTokenExpired(token))
            {
                HttpContext.Session.Remove("AuthToken");
                return RedirectToPage("/Login");
            }

            // Get claims from token
            var principal = TokenHelper.GetPrincipalFromToken(token);
            if (principal != null)
            {
                // Extract display name from claims
                DisplayName = principal.Claims
                                  .FirstOrDefault(c => c.Type == "DisplayName")?.Value 
                              ?? principal.Identity?.Name 
                              ?? "Usuari desconegut";
            }

            return Page();
        }
    }
}
