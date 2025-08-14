using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;

namespace StarWarsInfo.Controllers;

public class ImportController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly ISwapiClient _swapiClient;

    public ImportController(ILogger<ImportController> logger, AppDbContext dbContext, ISwapiClient swapiClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _swapiClient = swapiClient;
    }
    
    // GET: All
    public async Task<IActionResult> All(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _swapiClient.ImportAllDataAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data import");
            return StatusCode(500, new { status = "failed", message = ex.Message });
        }
    }
    
}