namespace StarWarsInfo.Integrations.Swapi;

public interface ISwapiImportService
{
    Task<int> ImportFilmsAsync(CancellationToken ct = default);
    // Task<int> ImportPeopleAsync(CancellationToken ct = default);
    // Task<int> ImportPlanetsAsync(CancellationToken ct = default);
    // Task<int> ImportSpeciesAsync(CancellationToken ct = default);
    Task<int> ImportStarshipsAsync(CancellationToken ct = default);
    // Task<int> ImportVehiclesAsync(CancellationToken ct = default);
}
