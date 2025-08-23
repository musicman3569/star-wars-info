using StarWarsInfo.Models;

namespace StarWarsInfo.Integrations.Swapi;

/// <summary>
/// Represents a client interface for interacting with the Star Wars API (SWAPI).
/// Provides methods to retrieve information about various Star Wars entities.
/// </summary>
public interface ISwapiClient
{
    /// <summary>
    /// Fetches a list of Starship objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Starship model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Starships retrieved from the API, or an empty list if no data is available.</returns>
    public Task<List<Starship>> FetchStarshipsAsync(CancellationToken ct);
    
    public Task<List<Film>> FetchFilmsAsync(CancellationToken ct);
}