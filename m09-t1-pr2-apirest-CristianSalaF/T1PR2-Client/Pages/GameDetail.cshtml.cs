using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using T1PR2_Client.Model;

namespace T1PR2_Client.Pages
{
    /// <summary>
    /// Represents the model for the Game Detail page.
    /// </summary>
    public class GameDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameDetailModel"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client factory used to create HTTP clients.</param>
        public GameDetailModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets or sets the ID of the game.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the game details.
        /// </summary>
        public GameDTO? game { get; set; }

        /// <summary>
        /// Gets or sets the message to display to the user.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Handles GET requests to retrieve game details.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClient.CreateClient("ApiGameJam");
            game = await client.GetFromJsonAsync<GameDTO>($"api/Videogame/{Id}");
            return Page();
        }

        /// <summary>
        /// Handles POST requests to vote for a game.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClient.CreateClient("ApiGameJam");

            // Get JWT from session and add to header
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                Message = "Has d'estar autenticat per votar.";
                return await OnGetAsync();
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"api/Videogame/vote/{Id}", null);
            if (response.IsSuccessStatusCode)
            {
                Message = "Vot enregistrat!";
            }
            else
            {
                Message = await response.Content.ReadAsStringAsync();
            }

            return await OnGetAsync();
        }
    }
}