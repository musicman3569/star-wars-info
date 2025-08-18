using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Controllers;

public class StarshipController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    public StarshipController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var starships = _dbContext.Starships.ToList();
        return Ok(starships);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var starship = _dbContext.Starships.Find(id);
        if (starship == null)
            return NotFound();
        return Ok(starship);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Starship starship)
    {
        _dbContext.Starships.Add(starship);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = starship.StarshipId }, starship);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Starship starship)
    {
        if (id != starship.StarshipId)
            return BadRequest();

        _dbContext.Entry(starship).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return NoContent();
    }

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