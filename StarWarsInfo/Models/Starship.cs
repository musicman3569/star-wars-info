namespace StarWarsInfo.Models;

public class Starship
{
    public int StarshipId { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public decimal CostInCredits { get; set; } // unknown
    public decimal Length { get; set; }
    public decimal? MaxAtmospheringSpeed { get; set; } // unknown, n/a, km suffix
    public int? Crew { get; set; } // unknown, n/a
    public int? Passengers { get; set; } // unknown, n/a
    public string? CargoCapacity { get; set; } // unkwown
    public string? Consumables { get; set; } // time duration: X months, year(s), week, days, hours, unknown
    public string? HyperdriveRating { get; set; } // unknown
    public int? MGLT { get; set; }
    public string StarshipClass { get; set; }
    public ICollection<Person> Pilots { get; set; }
    public ICollection<Film> Films { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
    public string Notes { get; set; }
}