using Microsoft.AspNetCore.Mvc;
using StarWarsInfo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Controllers;

/// <summary>
/// Controller responsible for managing vehicle-related operations in the Star Wars information system.
/// </summary>
[Authorize]
[ApiController]
[Route("v1/[controller]")]
public class VehicleController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly AppDbContext _dbContext;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="dbContext">The database context for data operations.</param>
    public VehicleController(ILogger<ImportController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Retrieves all vehicles from the database.
    /// </summary>
    /// <returns>A list of all vehicles.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var vehicles = _dbContext.Vehicles.ToList();
        return Ok(vehicles);
    }

    /// <summary>
    /// Retrieves a specific vehicle by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle.</param>
    /// <returns>The vehicle if found; otherwise, returns NotFound result.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var vehicle = _dbContext.Vehicles.Find(id);
        if (vehicle == null)
            return NotFound();
        return Ok(vehicle);
    }

    /// <summary>
    /// Creates a new vehicle in the database.
    /// </summary>
    /// <param name="vehicle">The vehicle object to create.</param>
    /// <returns>A CreatedAtAction result containing the newly created vehicle.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Vehicle vehicle)
    {
        vehicle.Created = DateTime.UtcNow;
        vehicle.Edited = DateTime.UtcNow;
        _dbContext.Vehicles.Add(vehicle);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = vehicle.VehicleId }, vehicle);
    }

    /// <summary>
    /// Updates an existing vehicle in the database.
    /// </summary>
    /// <param name="vehicle">The updated vehicle object.</param>
    /// <returns>OK with the updated vehicle if successful; otherwise, BadRequest.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] Vehicle vehicle)
    {
        vehicle.Edited = DateTime.UtcNow;        
        _dbContext.Entry(vehicle).State = EntityState.Modified;
        _dbContext.SaveChanges();
        return Ok(vehicle);
    }

    /// <summary>
    /// Deletes a specific vehicle from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the vehicle to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var vehicle = _dbContext.Vehicles.Find(id);
        if (vehicle == null)
            return NotFound();

        _dbContext.Vehicles.Remove(vehicle);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}