using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StarWarsInfo.Data;

namespace StarWarsInfo.Services;

public class StarWarsImportService : IStarWarsImportService
{
    private readonly ILogger<StarWarsImportService> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public StarWarsImportService(
        ILogger<StarWarsImportService> logger,
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

        // Import commonly used resources; extend as needed.
        await ImportResourceAsync(client, "people", cancellationToken);
        await ImportResourceAsync(client, "planets", cancellationToken);
        await ImportResourceAsync(client, "starships", cancellationToken);
        await ImportResourceAsync(client, "species", cancellationToken);
        await ImportResourceAsync(client, "films", cancellationToken);
        await ImportResourceAsync(client, "vehicles", cancellationToken);

        _logger.LogInformation("Star Wars import finished.");
    }

    private async Task ImportResourceAsync(HttpClient client, string resource, CancellationToken ct)
    {
        string? next = $"{resource}/";
        int importedCount = 0;

        while (!ct.IsCancellationRequested && next is not null)
        {
            using var response = await client.GetAsync(next, ct);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var jsonDocument = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            var results = jsonDocument.RootElement;

            if (results.ValueKind == JsonValueKind.Array)
            {
                // TODO: Map "results" JSON elements to your domain entities and upsert via _dbContext
                // Example outline:
                // foreach (var item in results.EnumerateArray())
                // {
                //     var entity = MapToYourEntity(resource, item);
                //     UpsertEntity(entity);
                // }
                // await _dbContext.SaveChangesAsync(ct);

                importedCount += results.GetArrayLength();
            }

            next = TryGetNextRelative(jsonDocument);
        }

        _logger.LogInformation("Imported {Count} items from '{Resource}'.", importedCount, resource);
    }

    private static string? TryGetNextRelative(JsonDocument document)
    {
        if (!document.RootElement.TryGetProperty("next", out var nextProp) ||
            nextProp.ValueKind != JsonValueKind.String)
        {
            return null;
        }

        var nextUrl = nextProp.GetString();
        if (string.IsNullOrWhiteSpace(nextUrl))
            return null;

        // Convert absolute SWAPI URL to relative so it works with HttpClient.BaseAddress
        if (Uri.TryCreate(nextUrl, UriKind.Absolute, out var uri))
        {
            return uri.PathAndQuery.TrimStart('/');
        }

        // Already relative
        return nextUrl;
    }

    // Example mappers/upserts would go here, tailored to your DbContext and entities.
}