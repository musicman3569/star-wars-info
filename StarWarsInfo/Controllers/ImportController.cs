using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using StarWarsInfo.Services;

namespace StarWarsInfo.Controllers;

public class ImportController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IStarWarsImportService _importService;

    public ImportController(ILogger<ImportController> logger, AppDbContext dbContext, IStarWarsImportService importService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _importService = importService;
    }
    
    // GET: All
    public async Task<IActionResult> All(CancellationToken cancellationToken = default)
    {
        try
        {
            await _importService.ImportAllDataAsync(cancellationToken);
            return Ok(new
            {
                status = "import-started"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data import");
            return StatusCode(500, new { status = "import-failed", message = ex.Message });
        }
    }
    
}