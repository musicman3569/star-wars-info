using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Controller responsible for managing person-related operations in the Star Wars information system.
/// </summary>
[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class PersonController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="dbContext">The database context for data operations.</param>
    public PersonController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Retrieves all people from the database.
    /// </summary>
    /// <returns>A list of all people.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var people = _dbContext.People.ToList();
        return Ok(people);
    }

    /// <summary>
    /// Retrieves a specific person by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the person.</param>
    /// <returns>The person if found; otherwise, returns NotFound result.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var person = _dbContext.People.Find(id);
        if (person == null)
            return NotFound();
        return Ok(person);
    }

    /// <summary>
    /// Creates a new person in the database.
    /// </summary>
    /// <param name="person">The person object to create.</param>
    /// <returns>A CreatedAtAction result containing the newly created person.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Person person)
    {
        person.Created = DateTime.UtcNow;
        person.Edited = DateTime.UtcNow;
        _dbContext.People.Add(person);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = person.PersonId }, person);
    }

    /// <summary>
    /// Updates an existing person in the database.
    /// </summary>
    /// <param name="person">The updated person object.</param>
    /// <returns>OK with the updated person if successful; otherwise, BadRequest.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] Person person)
    {
        person.Edited = DateTime.UtcNow;        
        _dbContext.Entry(person).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return Ok(person);
    }

    /// <summary>
    /// Deletes a specific person from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the person to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var person = _dbContext.People.Find(id);
        if (person == null)
            return NotFound();

        _dbContext.People.Remove(person);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}