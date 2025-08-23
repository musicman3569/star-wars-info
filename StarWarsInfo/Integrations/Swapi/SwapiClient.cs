using System.Text.Json;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi.Mappers;
using StarWarsInfo.Models;

namespace StarWarsInfo.Integrations.Swapi;

/// <summary>
/// Provides methods for interacting with the Star Wars API (SWAPI) to fetch information
/// related to various Star Wars entities, such as starships.
/// </summary>
public class SwapiClient : ISwapiClient
{
    private readonly ILogger<SwapiClient> _logger;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initialize an instance of the Star Wars API HTTP client.
    /// </summary>
    /// <param name="logger"></param> Logging interface to use.
    /// <param name="httpClientFactory"></param> Factory for creating the HTTP client to be used for the REST API calls.
    public SwapiClient(
        ILogger<SwapiClient> logger,
        AppDbContext dbContext,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("swapi");
    }

    /// <summary>
    /// Fetches a specified resource from the Star Wars API (SWAPI) as a JSON document asynchronously.
    /// </summary>
    /// <param name="resource">The URI of the resource to fetch from the SWAPI.</param>
    /// <param name="ct">A cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task whose value contains a <see cref="JsonDocument"/> representing the fetched resource.</returns>
    private async Task<JsonDocument> FetchResourceAsync(string resource, CancellationToken ct)
    {
        using var response = await _httpClient.GetAsync(resource, ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        return await JsonDocument.ParseAsync(stream, cancellationToken: ct);
    }

    /// <summary>
    /// Fetches a list of Starship objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Starship model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Starships retrieved from the API, or an empty list if no data is available.</returns>
    public async Task<List<Starship>> FetchStarshipsAsync(CancellationToken ct)
    {
        var jsonDocument = await FetchResourceAsync("starships", ct);
        return jsonDocument?
            .RootElement
            .EnumerateArray()
            .Select(StarshipMapper.FromJson)
            .ToList() ?? [];
    }
    
    /// <summary>
    /// Fetches a list of Film objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Film model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Films retrieved from the API, or an empty list if no data is available.</returns>
    public async Task<List<Film>> FetchFilmsAsync(CancellationToken ct)
    {
        var jsonDocument = await FetchResourceAsync("films", ct);
        return jsonDocument?
            .RootElement
            .EnumerateArray()
            .Select(FilmMapper.FromJson)
            .ToList() ?? [];
    }
}