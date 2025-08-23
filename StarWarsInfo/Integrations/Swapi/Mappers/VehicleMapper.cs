using StarWarsInfo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

/// <summary>
/// The VehicleMapper class is responsible for mapping vehicle-related data
/// from external sources, particularly the Star Wars API (SWAPI), into the
/// internal Vehicle model used within the system.
/// </summary>
public class VehicleMapper
{
    public string Name { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public string Manufacturer { get; set; } = String.Empty;
    public string CostInCredits { get; set; } = String.Empty;
    public string Length { get; set; } = String.Empty;
    public string MaxAtmospheringSpeed { get; set; } = String.Empty;
    public string Crew { get; set; } = String.Empty;
    public string Passengers { get; set; } = String.Empty;
    public string CargoCapacity { get; set; } = String.Empty;
    public string Consumables { get; set; } = String.Empty;
    public string VehicleClass { get; set; } = String.Empty;
    public ICollection<string> Pilots { get; set; } = [];
    public ICollection<string> Films { get; set; } = [];
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; } = String.Empty;

    /// <summary>
    /// Converts the current instance of <see cref="VehicleMapper"/> into an instance of <see cref="Vehicle"/>.
    /// </summary>
    /// <returns>A <see cref="Vehicle"/> object populated from the current <see cref="VehicleMapper"/> instance.</returns>
    public Vehicle ToVehicle() => new Vehicle
    {
        VehicleId = SwapiFieldParser.RawUrlToId(Url),
        Name = SwapiFieldParser.RawTextToTitleCase(Name),
        Model = SwapiFieldParser.RawTextToTitleCase(Model),
        Manufacturer = SwapiFieldParser.RawTextToTitleCase(Manufacturer),
        CostInCredits = SwapiFieldParser.RawTextToDecimalNullable(CostInCredits),
        Length = SwapiFieldParser.RawTextToDecimalNullable(Length),
        MaxAtmospheringSpeed = SwapiFieldParser.RawTextToIntNullable(MaxAtmospheringSpeed),
        Crew = SwapiFieldParser.RawTextToIntNullable(Crew),
        Passengers = SwapiFieldParser.RawTextToIntNullable(Passengers),
        CargoCapacity = CargoCapacity,
        Consumables = Consumables,
        VehicleClass = SwapiFieldParser.RawTextToTitleCase(VehicleClass),
        Created = Created,
        Edited = Edited
    };

    /// <summary>
    /// Creates an instance of <see cref="Vehicle"/> by deserializing the provided <see cref="JsonElement"/>
    /// using the mapping rules defined in <see cref="VehicleMapper"/>.
    /// </summary>
    /// <param name="jsonElement">The JSON representation of a vehicle object as a <see cref="JsonElement"/>.</param>
    /// <returns>A <see cref="Vehicle"/> object created from the provided JSON data.</returns>
    public static Vehicle FromJson(JsonElement jsonElement)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var vehicleMapper = jsonElement.Deserialize<VehicleMapper>(options) ?? new();
        return vehicleMapper.ToVehicle();
    }
}