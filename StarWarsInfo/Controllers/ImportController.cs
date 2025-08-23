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
        
        try
        {
            starshipImportCount = await _swapiImportService.ImportStarshipsAsync(cancellationToken);
            filmImportCount = await _swapiImportService.ImportFilmsAsync(cancellationToken);
            
            return new OkObjectResult(new 
            {
                status = "complete",
                message = "Import successful.",
                starship_import_count = starshipImportCount,
                film_import_count = filmImportCount
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "failed", 
                message = ex.Message,
                starship_import_count = starshipImportCount,
                film_import_count = filmImportCount
            });
        }
    }
    
}