using StarWarsInfo.Models;
using System.Text.Json;
using Serilog;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

/// <summary>
/// The SpeciesMapper class is responsible for mapping species-related data
/// from external sources, particularly the Star Wars API (SWAPI), into the
/// internal Species model used within the system.
/// </summary>
public class SpeciesMapper
{
    public string Name { get; set; } = String.Empty;
    public string Classification { get; set; } = String.Empty;
    public string Designation { get; set; } = String.Empty;
    public string AverageHeight { get; set; } = String.Empty;
    public string SkinColors { get; set; } = String.Empty;
    public string HairColors { get; set; } = String.Empty;
    public string EyeColors { get; set; } = String.Empty;
    public string AverageLifespan { get; set; } = String.Empty;
    public string? Homeworld { get; set; } = String.Empty;
    public string Language { get; set; } = String.Empty;
    public ICollection<string> People { get; set; } = [];
    public ICollection<string> Films { get; set; } = [];
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; } = String.Empty;

    /// <summary>
    /// Converts the current instance of <see cref="SpeciesMapper"/> into an instance of <see cref="Species"/>.
    /// </summary>
    /// <returns>A <see cref="Species"/> object populated from the current <see cref="SpeciesMapper"/> instance.</returns>
    public Species ToSpecies() => new Species
    {
        SpeciesId = SwapiFieldParser.RawUrlToId(Url),
        Name = SwapiFieldParser.RawTextToTitleCase(Name),
        Classification = SwapiFieldParser.RawTextToTitleCase(Classification),
        Designation = SwapiFieldParser.RawTextToTitleCase(Designation),
        AverageHeight = SwapiFieldParser.RawTextToIntNullable(AverageHeight),
        SkinColors = SkinColors,
        HairColors = HairColors,
        EyeColors = EyeColors,
        AverageLifespan = SwapiFieldParser.RawTextToIntNullable(AverageLifespan),
        HomeworldId = SwapiFieldParser.RawUrlToIdNullable(Homeworld),
        Language = SwapiFieldParser.RawTextToTitleCase(Language),
        Created = Created,
        Edited = Edited
    };

    /// <summary>
    /// Creates an instance of <see cref="Species"/> by deserializing the provided <see cref="JsonElement"/>
    /// using the mapping rules defined in <see cref="SpeciesMapper"/>.
    /// </summary>
    /// <param name="jsonElement">The JSON representation of a species object as a <see cref="JsonElement"/>.</param>
    /// <returns>A <see cref="Species"/> object created from the provided JSON data.</returns>
    public static Species FromJson(JsonElement jsonElement)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var speciesMapper = jsonElement.Deserialize<SpeciesMapper>(options) ?? new();
        Log.Information($"Mapping Species: {speciesMapper.Name}");
        return speciesMapper.ToSpecies();
    }
}