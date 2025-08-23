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
    
    /// <summary>
    /// Fetches a list of Film objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Film model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Films retrieved from the API, or an empty list if no data is available.</returns>
    public Task<List<Film>> FetchFilmsAsync(CancellationToken ct);
    
    /// <summary>
    /// Fetches a list of Planet objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Planet model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Planets retrieved from the API, or an empty list if no data is available.</returns>
    public Task<List<Planet>> FetchPlanetsAsync(CancellationToken ct);
    
    /// <summary>
    /// Fetches a list of Person objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Person model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of People retrieved from the API, or an empty list if no data is available.</returns>
    public Task<List<Person>> FetchPeopleAsync(CancellationToken ct);
    
    /// <summary>
    /// Fetches a list of Species objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Species model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Species retrieved from the API, or an empty list if no data is available.</returns>
    public Task<List<Species>> FetchSpeciesAsync(CancellationToken ct);
    
    /// <summary>
    /// Fetches a list of Vehicle objects by retrieving the corresponding data
    /// from the Star Wars API (SWAPI) and mapping/cleaning the raw values to a Vehicle model.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of Vehicles retrieved from the API, or an empty list if no data is available.</returns>
    public Task<List<Vehicle>> FetchVehiclesAsync(CancellationToken ct);
}