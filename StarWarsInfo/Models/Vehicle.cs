namespace StarWarsInfo.Models;

public class Vehicle
{
    public int VehicleId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Manufacturer { get; set; } // unknown
    public decimal? CostInCredits { get; set; } // unknown
    public decimal? Length { get; set; } // unknown
    public int? MaxAtmospheringSpeed { get; set; } // unknown
    public int? Crew { get; set; } // unknown
    public int? Passengers { get; set; } // unknown
    public string? CargoCapacity { get; set; } // unknown
    public string? Consumables { get; set; } // unknown
    public string VehicleClass { get; set; } = string.Empty; // unknown
    public ICollection<Person> Pilots { get; set; } = [];
    public ICollection<Film> Films { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;
    
    public Vehicle()
    {
    }
}