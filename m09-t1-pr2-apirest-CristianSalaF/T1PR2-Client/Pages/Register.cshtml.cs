using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T1PR2_Client.Model;

namespace T1PR2_Client.Pages
{
    /// <summary>
    /// Represents the model for the Register page.
    /// Handles user registration functionality.
    /// </summary>
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger _logger;

        /// <summary>
        /// Gets or sets the registration data transfer object.
        /// </summary>
        [BindProperty]
        public RegisterDTO Register { get; set; } = new();

        /// <summary>
        /// Gets or sets the error message to display in case of registration failure.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModel"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client factory for making API requests.</param>
        /// <param name="logger">The logger for logging information and errors.</param>
        public RegisterModel(IHttpClientFactory httpClient, ILogger<RegisterModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to the Register page.
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// Handles POST requests for user registration.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var client = _httpClient.CreateClient("ApiGameJam");
                var response = await client.PostAsJsonAsync("api/Auth/register", Register);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Register successful");
                    return RedirectToPage("/Login");
                }
                else
                {
                    _logger.LogWarning("Register failed");
                    ErrorMessage = "No s'ha pogut completar el registre. Revisa els camps.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant el registre");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }

            return Page();
        }
    }
}