using StarWarsInfo.Models;
using System.Text.Json;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

public class PersonMapper
{
    public string Name { get; set; } = string.Empty;
    public string Height { get; set; } = string.Empty;
    public string Mass { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string SkinColor { get; set; } = string.Empty;
    public string EyeColor { get; set; } = string.Empty;
    public string BirthYear { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Homeworld { get; set; } = string.Empty;
    public ICollection<string> Films { get; set; } = [];
    public ICollection<string> Species { get; set; } = [];
    public ICollection<string> Vehicles { get; set; } = [];
    public ICollection<string> Starships { get; set; } = [];
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; } = string.Empty;

    public Person ToPerson()
    {
        return new Person
        {
            PersonId = SwapiFieldParser.RawUrlToId(Url),
            Name = SwapiFieldParser.RawTextToTitleCase(Name),
            Height = SwapiFieldParser.RawTextToIntNullable(Height),
            Mass = SwapiFieldParser.RawTextToDecimalNullable(Mass),
            HairColor = SwapiFieldParser.RawTextToTitleCase(HairColor),
            SkinColor = SwapiFieldParser.RawTextToTitleCase(SkinColor),
            EyeColor = SwapiFieldParser.RawTextToTitleCase(EyeColor),
            BirthYear = BirthYear,
            Gender = SwapiFieldParser.RawTextToTitleCase(Gender),
            HomeworldId = SwapiFieldParser.RawUrlToId(Homeworld),
            Created = Created,
            Edited = Edited
        };
    }

    public static Person FromJson(JsonElement jsonElement)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var personMapper = jsonElement.Deserialize<PersonMapper>(options) ?? new();
        return personMapper.ToPerson();
    }
}