namespace StarWarsInfo.Models;

public class Starship
{
    public int StarshipId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public decimal? CostInCredits { get; set; } // unknown
    public decimal Length { get; set; } = 0;
    public decimal? MaxAtmospheringSpeed { get; set; } // unknown, n/a, km suffix
    public int? Crew { get; set; } // unknown, n/a
    public int? Passengers { get; set; } // unknown, n/a
    public ulong? CargoCapacity { get; set; } // unkown
    public string? Consumables { get; set; } // time duration: X months, year(s), week, days, hours, unknown
    public decimal? HyperdriveRating { get; set; } // unknown
    public int? Mglt { get; set; }
    public string StarshipClass { get; set; } = string.Empty;
    public ICollection<Person> Pilots { get; set; } = new List<Person>();
    public ICollection<Film> Films { get; set; } = new List<Film>();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    
    public Starship()
    {
    }
}