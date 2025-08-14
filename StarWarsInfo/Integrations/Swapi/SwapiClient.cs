using System.Text.Json;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi.Mappers;

namespace StarWarsInfo.Integrations.Swapi;

public class SwapiClient : ISwapiClient
{
    private readonly ILogger<SwapiClient> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _logPrefix = "[SwapiClient]";

    public SwapiClient(
        ILogger<SwapiClient> logger,
        AppDbContext dbContext,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OkObjectResult> ImportAllDataAsync(CancellationToken cancellationToken = default)
    {
        LogWithPrefix("IMPORT STARTED");

        var client = _httpClientFactory.CreateClient("swapi");
        var starshipCount = await ImportStarshipsAsync(client, cancellationToken);

        LogWithPrefix("IMPORT COMPLETE");
        LogWithPrefix("Processed {Count} Starship records.");
        
        return new OkObjectResult(new 
        {
            status = "complete",
            starship_import_count = starshipCount
        });
    }

    private async Task<JsonDocument> FetchResourceAsync(HttpClient client, string resource, CancellationToken ct)
    {
        using var response = await client.GetAsync(resource, ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        return await JsonDocument.ParseAsync(stream, cancellationToken: ct);
    }

    private async Task<int> ImportStarshipsAsync(HttpClient client, CancellationToken ct)
    {
        var jsonDocument = await FetchResourceAsync(client, "starships", ct);
        var starships = jsonDocument?
            .RootElement
            .EnumerateArray()
            .Select(StarshipMapper.FromJson)
            .ToList() ?? [];
        
        await _dbContext.BulkInsertOrUpdateAsync(starships, cancellationToken:ct);
        return starships.Count;
    }
    
    private void LogWithPrefix(string message)
    {
        _logger.LogInformation("{LogPrefix} {Message}", _logPrefix, message);
    }
}