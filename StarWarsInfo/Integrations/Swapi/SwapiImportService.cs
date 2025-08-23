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

    // public async Task<int> ImportStarshipsAsync(CancellationToken cancellationToken = default)
    // {
    //     // Retrieve all starship data from the Star Wars API and bulk insert/update into the database
    //     var starships = await _swapiClient.FetchStarshipsAsync(cancellationToken);
    //     await _dbContext.BulkInsertOrUpdateAsync(
    //         starships,
    //         cancellationToken:cancellationToken
    //     );
    //         
    //     // With the new data inserted, reset the sequence to the next available ID.
    //     // The syntax for this is specific to PostgreSQL.
    //     var maxId = await _dbContext.Starships.MaxAsync(s => s.StarshipId, cancellationToken);
    //     var nextId = maxId + 1;
    //     await _dbContext.Database.ExecuteSqlInterpolatedAsync(
    //         $"SELECT setval('starwarsinfo.\"Starships_StarshipId_seq\"', {nextId}, false);",
    //         cancellationToken
    //     );
    //     
    //     return starships.Count;
    // }
    
    // public async Task<int> ImportFilmsAsync(CancellationToken ct = default) 
    // {
    //     
    // }
    //
    // public async Task<int> ImportPeopleAsync(CancellationToken ct = default) 
    // {
    //     
    // }
    //
    // public async Task<int> ImportPlanetsAsync(CancellationToken ct = default) 
    // {
    //     
    // }
    //
    // public async Task<int> ImportSpeciesAsync(CancellationToken ct = default) 
    // {
    //     
    // }
    
    public Task<int> ImportStarshipsAsync(CancellationToken ct = default) =>
        ImportAsync(
            _swapiClient.FetchStarshipsAsync,
            _dbContext.Starships,
            s => s.StarshipId,
            "starwarsinfo.\"Starships_StarshipId_seq\"",
            ct
        );

    
    // public async Task<int> ImportVehiclesAsync(CancellationToken ct = default) 
    // {
    //     
    // }
    
    private async Task<int> ImportAsync<TEntity>(
        Func<CancellationToken, Task<List<TEntity>>> fetchAsync,
        DbSet<TEntity> dbSet,
        Expression<Func<TEntity, int>> idSelector,
        string sequenceName,
        CancellationToken ct)
        where TEntity : class
    {
        var entities = await fetchAsync(ct);

        await _dbContext.BulkInsertOrUpdateAsync(
            entities,
            cancellationToken: ct
        );

        var maxId = await dbSet.MaxAsync(idSelector, ct);
        var nextId = maxId + 1;

        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT setval({sequenceName}, {nextId}, false);",
            ct
        );

        return entities.Count;
    }
}