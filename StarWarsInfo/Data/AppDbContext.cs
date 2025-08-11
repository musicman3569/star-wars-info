using Microsoft.EntityFrameworkCore;
using StarWarsInfo.Models;

namespace StarWarsInfo.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Person> People { get; set; } = default!;
    public DbSet<Planet> Planets { get; set; } = default!;
    public DbSet<Species> Species { get; set; } = default!;
    public DbSet<Starship> Starships { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    public DbSet<Film> Films { get; set; } = default!;
}