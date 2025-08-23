using System.Linq.Expressions;
using StarWarsInfo.Data;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace StarWarsInfo.Integrations.Swapi;

public class SwapiImportService : ISwapiImportService
{
    private readonly ISwapiClient _swapiClient;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<SwapiImportService> _logger;
    
    public SwapiImportService(ISwapiClient swapiClient, AppDbContext dbContext, ILogger<SwapiImportService> logger)
    {
        _swapiClient = swapiClient;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public Task<int> ImportFilmsAsync(CancellationToken ct = default) =>
        ImportAsync(
            _swapiClient.FetchFilmsAsync,
            _dbContext.Films,
            f => f.FilmId,
            "starwarsinfo.\"Films_FilmId_seq\"",
            ct
        );
    
    // public Task<int> ImportPeopleAsync(CancellationToken ct = default) => 
    // 
    //
    // public Task<int> ImportPlanetsAsync(CancellationToken ct = default) => 
    // 
    //
    // public Task<int> ImportSpeciesAsync(CancellationToken ct = default) => 
    // 
    
    public Task<int> ImportStarshipsAsync(CancellationToken ct = default) =>
        ImportAsync(
            _swapiClient.FetchStarshipsAsync,
            _dbContext.Starships,
            s => s.StarshipId,
            "starwarsinfo.\"Starships_StarshipId_seq\"",
            ct
        );

    
    // public Task<int> ImportVehiclesAsync(CancellationToken ct = default) => 
    // 
    
    private async Task<int> ImportAsync<TEntity>(
        Func<CancellationToken, Task<List<TEntity>>> fetchAsync,
        DbSet<TEntity> dbSet,
        Expression<Func<TEntity, int>> idSelector,
        string sequenceName,
        CancellationToken ct
    )
        where TEntity : class
    {
        // Call the API fetch function to get the entities to be imported
        var entities = await fetchAsync(ct);

        await _dbContext.BulkInsertOrUpdateAsync(
            entities,
            cancellationToken: ct
        );

        // Get the next available id for the imported entities.
        var maxId = await dbSet.MaxAsync(idSelector, ct);
        var nextId = maxId + 1;

        // The imported date already has id values, so the auto-increment sequence
        // will be out of sync. This will reset it to the next highest available id.
        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT setval({sequenceName}, {nextId}, false);",
            ct
        );

        return entities.Count;
    }
}