using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;
using Microsoft.AspNetCore.Authorization;


namespace StarWarsInfo.Controllers;

/// <summary>
/// Handles the import of Star Wars data into the application database.
/// Provides endpoints for batch data retrieval and insertion/updating operations.
/// </summary>
[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class ImportController : Controller
{
    private readonly ISwapiImportService _swapiImportService;

    /// <summary>
    /// Handles the import of Star Wars data into the application's database.
    /// Provides mechanisms to interact with third-party APIs and persist the retrieved data.
    /// </summary>
    public ImportController(ISwapiImportService swapiImportService)
    {
        _swapiImportService = swapiImportService;
    }

    /// <summary>
    /// Imports all starship data from the Star Wars API into the application's database.
    /// Handles the data retrieval, processing, and batch insertion or update operations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe while performing the asynchronous operation.</param>
    /// <returns>
    /// An IActionResult containing the status of the import operation. On success, the response includes
    /// the count of imported starships. On failure, an error status and message are returned.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> SyncAll(CancellationToken cancellationToken = default)
    {
        var starshipImportCount = 0;
        var filmImportCount = 0;
        var planetImportCount = 0;
        var peopleImportCount = 0;
        var speciesImportCount = 0;
        var vehicleImportCount = 0;
        var currentModel = "";
        
        try
        {
            currentModel = "starships";
            starshipImportCount = await _swapiImportService.ImportStarshipsAsync(cancellationToken);
            currentModel = "films";
            filmImportCount = await _swapiImportService.ImportFilmsAsync(cancellationToken);
            currentModel = "planets";
            planetImportCount = await _swapiImportService.ImportPlanetsAsync(cancellationToken);
            currentModel = "people";
            peopleImportCount = await _swapiImportService.ImportPeopleAsync(cancellationToken);
            currentModel = "species";
            speciesImportCount = await _swapiImportService.ImportSpeciesAsync(cancellationToken);
            currentModel = "vehicles";           
            vehicleImportCount = await _swapiImportService.ImportVehiclesAsync(cancellationToken);
            
            
            return new OkObjectResult(new 
            {
                status = "complete",
                message = "Import successful.",
                starship_import_count = starshipImportCount,
                film_import_count = filmImportCount,
                planet_import_count = planetImportCount,
                people_import_count = peopleImportCount,
                species_import_count = speciesImportCount,
                vehicle_import_count = vehicleImportCount,
                current_model = currentModel           
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "failed", 
                message = ex.Message,
                starship_import_count = starshipImportCount,
                film_import_count = filmImportCount,
                planet_import_count = planetImportCount,
                people_import_count = peopleImportCount,
                species_import_count = speciesImportCount,
                vehicle_import_count = vehicleImportCount,
                current_model = currentModel
            });
        }
    }
    
}