using StarWarsInfo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

/// <summary>
/// The StarshipMapper class is responsible for mapping starship-related data
/// from external sources, particularly the Star Wars API (SWAPI), into the
/// internal Starship model used within the system.
/// </summary>
public class StarshipMapper
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
    public string HyperdriveRating { get; set; } = String.Empty;
    [JsonPropertyName("MGLT")]
    public string Mglt { get; set; } = String.Empty;
    public string StarshipClass { get; set; } = String.Empty;
    public ICollection<string> Pilots { get; set; } = [];
    public ICollection<string> Films { get; set; } = [];
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; } = String.Empty;

    /// <summary>
    /// Converts the current instance of <see cref="StarshipMapper"/> into an instance of <see cref="Starship"/>.
    /// </summary>
    /// <returns>A <see cref="Starship"/> object populated from the current <see cref="StarshipMapper"/> instance.</returns>
    public Starship ToStarship() =>  new Starship
    {
        StarshipId = SwapiFieldParser.RawUrlToId(Url),
        Name = SwapiFieldParser.RawTextToTitleCase(Name),
        Model = SwapiFieldParser.RawTextToTitleCase(Model),
        Manufacturer = SwapiFieldParser.RawTextToTitleCase(Manufacturer),
        CostInCredits = SwapiFieldParser.RawTextToDecimalNullable(CostInCredits),
        Length = SwapiFieldParser.RawTextToDecimal(Length),
        MaxAtmospheringSpeed = SwapiFieldParser.RawTextToDecimalNullable(MaxAtmospheringSpeed),
        Crew = SwapiFieldParser.RawTextToIntNullable(Crew),
        Passengers = SwapiFieldParser.RawTextToIntNullable(Passengers),
        CargoCapacity = SwapiFieldParser.RawTextToUlongNullable(CargoCapacity),
        Consumables = Consumables,
        HyperdriveRating = SwapiFieldParser.RawTextToDecimalNullable(HyperdriveRating),
        Mglt = SwapiFieldParser.RawTextToIntNullable(Mglt),
        StarshipClass = SwapiFieldParser.RawTextToTitleCase(StarshipClass),
        // Pilots = Pilots,
        // Films = Films,
        Created = Created,
        Edited = Edited
    };

    /// <summary>
    /// Creates an instance of <see cref="Starship"/> by deserializing the provided <see cref="JsonElement"/>
    /// using the mapping rules defined in <see cref="StarshipMapper"/>.
    /// </summary>
    /// <param name="jsonElement">The JSON representation of a starship object as a <see cref="JsonElement"/>.</param>
    /// <returns>A <see cref="Starship"/> object created from the provided JSON data.</returns>
    public static Starship FromJson(JsonElement jsonElement)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var starshipMapper = jsonElement.Deserialize<StarshipMapper>(options) ?? new();
        return starshipMapper.ToStarship();
    }
}
