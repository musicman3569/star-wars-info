namespace StarWarsInfo.Models;

public class Planet
{
    public int PlantetId { get; set; }
    public string Name { get; set; }
    public int? RotationPeriod { get; set; } // unknown
    public int? OrbitalPeriod { get; set; } // unknown
    public int? Diameter { get; set; } // unknown
    public string Climate { get; set; }
    public decimal? Gravity { get; set; } // unknown, " standard" suffix
    public string Terrain { get; set; }
    public int? SurfaceWater { get; set; } // unknown
    public long? Population { get; set; } // unknown
    public ICollection<Person> Residents { get; set; }
    public ICollection<Film> Films { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
}