using StarWarsInfo.Models;
using System.Text.Json;
using Serilog;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

/// <summary>
/// The PlanetMapper class is responsible for mapping planet-related data
/// from external sources, particularly the Star Wars API (SWAPI), into the
/// internal Planet model used within the system.
/// </summary>
public class PlanetMapper
{
    public string Name { get; set; } = String.Empty;
    public string RotationPeriod { get; set; } = String.Empty;
    public string OrbitalPeriod { get; set; } = String.Empty;
    public string Diameter { get; set; } = String.Empty;
    public string Climate { get; set; } = String.Empty;
    public string Gravity { get; set; } = String.Empty;
    public string Terrain { get; set; } = String.Empty;
    public string SurfaceWater { get; set; } = String.Empty;
    public string Population { get; set; } = String.Empty;
    public ICollection<string> Residents { get; set; } = [];
    public ICollection<string> Films { get; set; } = [];
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; } = String.Empty;

    /// <summary>
    /// Converts the current instance of <see cref="PlanetMapper"/> into an instance of <see cref="Planet"/>.
    /// </summary>
    /// <returns>A <see cref="Planet"/> object populated from the current <see cref="PlanetMapper"/> instance.</returns>
    public Planet ToPlanet() => new Planet
    {
        PlanetId = SwapiFieldParser.RawUrlToId(Url),
        Name = SwapiFieldParser.RawTextToTitleCase(Name),
        RotationPeriod = SwapiFieldParser.RawTextToIntNullable(RotationPeriod),
        OrbitalPeriod = SwapiFieldParser.RawTextToIntNullable(OrbitalPeriod),
        Diameter = SwapiFieldParser.RawTextToIntNullable(Diameter),
        Climate = Climate,
        Gravity = SwapiFieldParser.RawTextToDecimalNullable(Gravity),
        Terrain = Terrain,
        SurfaceWater = SwapiFieldParser.RawTextToDecimalNullable(SurfaceWater),
        Population = SwapiFieldParser.RawTextToUlongNullable(Population),
        Created = Created,
        Edited = Edited
    };


    /// <summary>
    /// Creates an instance of <see cref="Planet"/> by deserializing the provided <see cref="JsonElement"/>
    /// using the mapping rules defined in <see cref="PlanetMapper"/>.
    /// </summary>
    /// <param name="jsonElement">The JSON representation of a planet object as a <see cref="JsonElement"/>.</param>
    /// <returns>A <see cref="Planet"/> object created from the provided JSON data.</returns>
    public static Planet FromJson(JsonElement jsonElement)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        
        var planetMapper = jsonElement.Deserialize<PlanetMapper>(options) ?? new PlanetMapper();
        Log.Information("Mapping Planet: {name}", planetMapper.Name);
        return planetMapper.ToPlanet();
    }
}