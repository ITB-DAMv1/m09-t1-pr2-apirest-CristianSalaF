using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T1PR2_Client.Model;

namespace T1PR2_Client.Pages;

/// <summary>
/// Represents the model for the Index page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Gets or sets the list of games retrieved from the API.
    /// </summary>
    public List<GameDTO>? Games { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    /// <param name="clientFactory">The HTTP client factory used to create HTTP clients.</param>
    /// <param name="logger">The logger used for logging information and errors.</param>
    public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests to the Index page and retrieves the list of games from the API.
    /// </summary>
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
