namespace StarWarsInfo.Models;

public class Planet
{
    public int PlanetId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public int? RotationPeriod { get; set; } // unknown
    public int? OrbitalPeriod { get; set; } // unknown
    public int? Diameter { get; set; } // unknown
    public string Climate { get; set; } = string.Empty;
    public decimal? Gravity { get; set; } // unknown, " standard" suffix
    public string Terrain { get; set; } = string.Empty;
    public decimal? SurfaceWater { get; set; } // unknown
    public ulong? Population { get; set; } // unknown
    public ICollection<Person> Residents { get; set; } = [];
    public ICollection<Film> Films { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;
    
    public Planet()
    {
    }
}