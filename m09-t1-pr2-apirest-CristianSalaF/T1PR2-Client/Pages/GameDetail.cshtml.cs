using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using T1PR2_Client.Model;

namespace T1PR2_Client.Pages
{
    public class GameDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public GameDetailModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public GameDTO? game { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClient.CreateClient("ApiGameJam");
            game = await client.GetFromJsonAsync<GameDTO>($"api/Videogame/{Id}");
            return Page();
        }

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