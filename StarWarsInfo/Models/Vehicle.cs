namespace StarWarsInfo.Models;

public class Vehicle
{
    public int VehicleId { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string? Manufacturer { get; set; } // unknown
    public decimal? CostInCredits { get; set; } // unknown
    public decimal? Length { get; set; } // unknown
    public int? MaxAtmospheringSpeed { get; set; } // unknown
    public int? Crew { get; set; } // unknown
    public int? Passengers { get; set; } // unknown
    public string? CargoCapacity { get; set; } // unknown
    public string? Consumables { get; set; } // unknown
    public string VehicleClass { get; set; } // unknown
    public ICollection<Person> Pilots { get; set; }
    public ICollection<Film> Films { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
}