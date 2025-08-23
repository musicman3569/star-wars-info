using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Controller responsible for managing species-related operations in the Star Wars information system.
/// </summary>
[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class SpeciesController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="dbContext">The database context for data operations.</param>
    public SpeciesController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Retrieves all speciess from the database.
    /// </summary>
    /// <returns>A list of all speciess.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var speciess = _dbContext.Species.ToList();
        return Ok(speciess);
    }

    /// <summary>
    /// Retrieves a specific species by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the species.</param>
    /// <returns>The species if found; otherwise, returns NotFound result.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var species = _dbContext.Species.Find(id);
        if (species == null)
            return NotFound();
        return Ok(species);
    }

    /// <summary>
    /// Creates a new species in the database.
    /// </summary>
    /// <param name="species">The species object to create.</param>
    /// <returns>A CreatedAtAction result containing the newly created species.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Species species)
    {
        species.Created = DateTime.UtcNow;
        species.Edited = DateTime.UtcNow;
        _dbContext.Species.Add(species);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = species.SpeciesId }, species);
    }

    /// <summary>
    /// Updates an existing species in the database.
    /// </summary>
    /// <param name="species">The updated species object.</param>
    /// <returns>OK with the updated species if successful; otherwise, BadRequest.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] Species species)
    {
        species.Edited = DateTime.UtcNow;        
        _dbContext.Entry(species).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return Ok(species);
    }

    /// <summary>
    /// Deletes a specific species from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the species to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var species = _dbContext.Species.Find(id);
        if (species == null)
            return NotFound();

        _dbContext.Species.Remove(species);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}