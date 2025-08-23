using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using StarWarsInfo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWarsInfo.Integrations.Swapi.Mappers;

public class FilmMapper
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int EpisodeId { get; set; } = 0;
    public string OpeningCrawl { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public DateOnly ReleaseDate { get; set; } = default!;
    public ICollection<string> Characters { get; set; } = [];
    public ICollection<string> Planets { get; set; } = [];
    public ICollection<string> Starships { get; set; } = [];
    public ICollection<string> Vehicles { get; set; } = [];
    public ICollection<string> Species { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;

    public Film ToFilm() => new Film
    {
        FilmId = SwapiFieldParser.RawUrlToId(Url),
        Title = Title,
        EpisodeId = EpisodeId,
        OpeningCrawl = OpeningCrawl,
        Director = SwapiFieldParser.RawTextToTitleCase(Director),
        Producer = SwapiFieldParser.RawTextToTitleCase(Producer),
        ReleaseDate = ReleaseDate,
        // Characters = Characters,
        // Planets = Planets,
        // Starships = Starships,
        // Vehicles = Vehicles,
        // Species = Species,
        Created = Created,
        Edited = Edited       
    };
    
    public static Film FromJson(JsonElement jsonElement)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        var filmMapper = jsonElement.Deserialize<FilmMapper>(options) ?? new();
        return filmMapper.ToFilm();
    }
}