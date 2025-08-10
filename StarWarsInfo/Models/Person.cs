namespace StarWarsInfo.Models;

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public int? Height { get; set; } // unknown
    public int? Mass { get; set; } // unknown
    public string HairColor { get; set; }
    public string SkinColor { get; set; }
    public string EyeColor { get; set; }
    public string BirthYear { get; set; }
    public string Gender { get; set; } // male, female, n/a
    public int PlanetId_Homeworld { get; set; }
    public Planet Homeworld { get; set; }
    public ICollection<Film> Films { get; set; }
    public ICollection<Species> Species { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; }
    public ICollection<Starship> Starships { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
}