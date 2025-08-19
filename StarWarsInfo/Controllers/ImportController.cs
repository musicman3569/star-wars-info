using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;
using EFCore.BulkExtensions;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Handles the import of Star Wars data into the application database.
/// Provides endpoints for batch data retrieval and insertion/updating operations.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class ImportController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly ISwapiClient _swapiClient;

    /// <summary>
    /// Handles the import of Star Wars data into the application's database.
    /// Provides mechanisms to interact with third-party APIs and persist the retrieved data.
    /// </summary>
    public ImportController(ILogger<ImportController> logger, AppDbContext dbContext, ISwapiClient swapiClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _swapiClient = swapiClient;
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
        try
        {
            var starships = await _swapiClient.FetchStarshipsAsync(cancellationToken);
            await _dbContext.BulkInsertOrUpdateAsync(starships, cancellationToken:cancellationToken);
            return new OkObjectResult(new 
            {
                status = "complete",
                starship_import_count = starships.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data import");
            return StatusCode(500, new
            {
                status = "failed", 
                message = ex.Message
            });
        }
    }
    
}