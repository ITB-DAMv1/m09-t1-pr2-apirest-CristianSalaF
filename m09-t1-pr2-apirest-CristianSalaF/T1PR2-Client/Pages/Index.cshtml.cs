using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T1PR2_Client.Model;

namespace T1PR2_Client.Pages;

public class IndexModel : PageModel
{

    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IndexModel> _logger;

    public List<GameDTO>? Games { get; set; }

    public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient("ApiGameJam");

        try
        {
            var result = await client.GetFromJsonAsync<List<GameDTO>>("api/Videogame");
            Games = result?.Select(g => new GameDTO
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                DeveloperTeamName = g.DeveloperTeamName,
                ImageUrl = g.ImageUrl,
                VoteCount = g.VoteCount
            }).ToList() ?? new List<GameDTO>();
        }
        catch
        {
            _logger.LogError("Error fetching games from API.");
            Games = new List<GameDTO>();
        }
    }
}
