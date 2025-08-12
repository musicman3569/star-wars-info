namespace StarWarsInfo.Models;

public class Species
{
    public int SpeciesId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Classification { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public int? AverageHeight { get; set; } // n/a
    public string SkinColors { get; set; } = string.Empty; // comma separated
    public string HairColors { get; set; } = string.Empty; // comma separated
    public string EyeColors { get; set; } = string.Empty; // comma separated
    public int? AverageLifespan { get; set; } // n/a, indefinite, unknown
    public int HomeworldId { get; set; } = 0;
    public ICollection<Planet> Homeworld { get; set; } = new List<Planet>();
    public string Language { get; set; } = string.Empty;
    public ICollection<Person> People { get; set; } = new List<Person>();
    public ICollection<Film> Films { get; set; } = new List<Film>();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    
    public Species()
    {
    }
}