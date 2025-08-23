using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Controller responsible for managing film-related operations in the Star Wars information system.
/// </summary>
[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class FilmController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FilmController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="dbContext">The database context for data operations.</param>
    public FilmController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Retrieves all films from the database.
    /// </summary>
    /// <returns>A list of all films.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var films = _dbContext.Films.ToList();
        return Ok(films);
    }

    /// <summary>
    /// Retrieves a specific film by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the film.</param>
    /// <returns>The film if found; otherwise, returns NotFound result.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var film = _dbContext.Films.Find(id);
        if (film == null)
            return NotFound();
        return Ok(film);
    }

    /// <summary>
    /// Creates a new film in the database.
    /// </summary>
    /// <param name="film">The film object to create.</param>
    /// <returns>A CreatedAtAction result containing the newly created film.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Film film)
    {
        film.Created = DateTime.UtcNow;
        film.Edited = DateTime.UtcNow;
        _dbContext.Films.Add(film);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = film.FilmId }, film);
    }

    /// <summary>
    /// Updates an existing film in the database.
    /// </summary>
    /// <param name="film">The updated film object.</param>
    /// <returns>OK with the updated film if successful; otherwise, BadRequest.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] Film film)
    {
        film.Edited = DateTime.UtcNow;        
        _dbContext.Entry(film).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return Ok(film);
    }

    /// <summary>
    /// Deletes a specific film from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the film to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var film = _dbContext.Films.Find(id);
        if (film == null)
            return NotFound();

        _dbContext.Films.Remove(film);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}