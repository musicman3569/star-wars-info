using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;
using Microsoft.AspNetCore.Http;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Controller responsible for managing starship-related operations in the Star Wars information system.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class StarshipController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StarshipController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="dbContext">The database context for data operations.</param>
    public StarshipController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Retrieves all starships from the database.
    /// </summary>
    /// <returns>A list of all starships.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var starships = _dbContext.Starships.ToList();
        return Ok(starships);
    }

    /// <summary>
    /// Retrieves a specific starship by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the starship.</param>
    /// <returns>The starship if found; otherwise, returns NotFound result.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var starship = _dbContext.Starships.Find(id);
        if (starship == null)
            return NotFound();
        return Ok(starship);
    }

    /// <summary>
    /// Creates a new starship in the database.
    /// </summary>
    /// <param name="starship">The starship object to create.</param>
    /// <returns>A CreatedAtAction result containing the newly created starship.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Starship starship)
    {
        starship.Created = DateTime.UtcNow;
        starship.Edited = DateTime.UtcNow;
        _dbContext.Starships.Add(starship);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = starship.StarshipId }, starship);
    }

    /// <summary>
    /// Updates an existing starship in the database.
    /// </summary>
    /// <param name="starship">The updated starship object.</param>
    /// <returns>OK with the updated starship if successful; otherwise, BadRequest.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] Starship starship)
    {
        starship.Edited = DateTime.UtcNow;        
        _dbContext.Entry(starship).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return Ok(starship);
    }

    /// <summary>
    /// Deletes a specific starship from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the starship to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var starship = _dbContext.Starships.Find(id);
        if (starship == null)
            return NotFound();

        _dbContext.Starships.Remove(starship);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}