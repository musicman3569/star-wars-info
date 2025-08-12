namespace StarWarsInfo.Models;

public class Species
{
    public int SpeciesId { get; set; }
    public string Name { get; set; }
    public string Classification { get; set; }
    public string Designation { get; set; }
    public int? AverageHeight { get; set; } // n/a
    public string SkinColors { get; set; } // comma separated
    public string HairColors { get; set; } // comma separated
    public string EyeColors { get; set; } // comma separated
    public int? AverageLifespan { get; set; } // n/a, indefinite, unknown
    public int HomeworldId { get; set; }
    public ICollection<Planet> Homeworld { get; set; }
    public string Language { get; set; }
    public ICollection<Person> People { get; set; }
    public ICollection<Film> Films { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
}