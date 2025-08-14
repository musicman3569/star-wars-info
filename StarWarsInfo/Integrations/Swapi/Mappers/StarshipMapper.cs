using StarWarsInfo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

public class StarshipMapper {
    public string Name { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string CostInCredits { get; set; }
    public string Length { get; set; }
    public string MaxAtmospheringSpeed { get; set; }
    public string Crew { get; set; }
    public string Passengers { get; set; }
    public string CargoCapacity { get; set; }
    public string Consumables { get; set; }
    public string HyperdriveRating { get; set; }
    [JsonPropertyName("MGLT")]
    public string Mglt { get; set; }
    public string StarshipClass { get; set; }
    public ICollection<string> Pilots { get; set; }
    public ICollection<string> Films { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
    
    public Starship ToStarship()
    {
        return new Starship
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
    }
    
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
