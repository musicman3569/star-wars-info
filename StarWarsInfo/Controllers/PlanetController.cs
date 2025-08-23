using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Controller responsible for managing planet-related operations in the Star Wars information system.
/// </summary>
[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class PlanetController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PlanetController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="dbContext">The database context for data operations.</param>
    public PlanetController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Retrieves all planets from the database.
    /// </summary>
    /// <returns>A list of all planets.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var planets = _dbContext.Planets.ToList();
        return Ok(planets);
    }

    /// <summary>
    /// Retrieves a specific planet by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the planet.</param>
    /// <returns>The planet if found; otherwise, returns NotFound result.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var planet = _dbContext.Planets.Find(id);
        if (planet == null)
            return NotFound();
        return Ok(planet);
    }

    /// <summary>
    /// Creates a new planet in the database.
    /// </summary>
    /// <param name="planet">The planet object to create.</param>
    /// <returns>A CreatedAtAction result containing the newly created planet.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Planet planet)
    {
        planet.Created = DateTime.UtcNow;
        planet.Edited = DateTime.UtcNow;
        _dbContext.Planets.Add(planet);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = planet.PlanetId }, planet);
    }

    /// <summary>
    /// Updates an existing planet in the database.
    /// </summary>
    /// <param name="planet">The updated planet object.</param>
    /// <returns>OK with the updated planet if successful; otherwise, BadRequest.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] Planet planet)
    {
        planet.Edited = DateTime.UtcNow;        
        _dbContext.Entry(planet).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return Ok(planet);
    }

    /// <summary>
    /// Deletes a specific planet from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the planet to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var planet = _dbContext.Planets.Find(id);
        if (planet == null)
            return NotFound();

        _dbContext.Planets.Remove(planet);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}