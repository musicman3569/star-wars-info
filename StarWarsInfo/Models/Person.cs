namespace StarWarsInfo.Models;

public class Person
{
    public int PersonId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public int? Height { get; set; } // unknown
    public int? Mass { get; set; } // unknown
    public string HairColor { get; set; } = string.Empty;
    public string SkinColor { get; set; } = string.Empty; // unknown
    public string EyeColor { get; set; } = string.Empty; // unknown
    public string BirthYear { get; set; } = string.Empty; // unknown
    public string Gender { get; set; } = string.Empty; // male, female, n/a
    public int HomeworldId { get; set; } = 0; // foreign key
    public Planet Homeworld { get; set; } = null!;
    public ICollection<Film> Films { get; set; } = [];
    public ICollection<Species> Species { get; set; } = [];
    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Starship> Starships { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;
    
    public Person()
    {
    }
}