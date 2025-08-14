using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi.Mappers;

namespace StarWarsInfo.Integrations.Swapi;

public class SwapiClient : ISwapiClient
{
    private readonly ILogger<SwapiClient> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private int _addedCount = 0;
    private int _updatedCount = 0;

    public SwapiClient(
        ILogger<SwapiClient> logger,
        AppDbContext dbContext,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    public async Task ImportAllDataAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Star Wars import...");

        var client = _httpClientFactory.CreateClient("swapi");

        await ImportStarshipsAsync(client, cancellationToken);
        // Import commonly used resources; extend as needed.
        // await FetchResourceAsync(client, "people", cancellationToken);
        // await FetchResourceAsync(client, "planets", cancellationToken);
        // await FetchResourceAsync(client, "species", cancellationToken);
        // await FetchResourceAsync(client, "films", cancellationToken);
        // await FetchResourceAsync(client, "vehicles", cancellationToken);

        _logger.LogInformation("Star Wars import finished.");
        _logger.LogInformation("Added {Count} items.", _addedCount);
        _logger.LogInformation("Updated {Count} items.", _updatedCount);
    }

    private async Task<JsonDocument?> FetchResourceAsync(HttpClient client, string resource, CancellationToken ct)
    {
        JsonDocument? jsonDocument = null;
        
        if (!ct.IsCancellationRequested)
        {
            using var response = await client.GetAsync(resource, ct);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            jsonDocument = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        }

        return jsonDocument;
    }

    private async Task ImportStarshipsAsync(HttpClient client, CancellationToken ct)
    {
        var jsonDocument = await FetchResourceAsync(client, "starships", ct);
        foreach (JsonElement starshipJson in jsonDocument.RootElement.EnumerateArray())
        {
            var starship = StarshipMapper.FromJson(starshipJson);
            var oldStarship = await _dbContext.Starships.FirstOrDefaultAsync(s => s.StarshipId == starship.StarshipId);
            
            if (oldStarship == null)
            {
                _dbContext.Starships.Add(starship);
                _addedCount++;
                _logger.LogInformation("Added {StarshipName} ({StarshipId})", starship.Name, starship.StarshipId);
            }
            else
            {
                _dbContext.Entry(oldStarship).CurrentValues.SetValues(starship);
                _updatedCount++;
                _logger.LogInformation("Updated {StarshipName} ({StarshipId})", starship.Name, starship.StarshipId);
            }
            
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}