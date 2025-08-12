namespace StarWarsInfo.Models;

public class Film
{
    public int FilmId { get; set; }
    public string Title { get; set; }
    public string EpisodeId { get; set; }
    public string OpeningCrawl { get; set; }
    public string Director { get; set; }
    public string Producer { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ICollection<Person> Characters { get; set; }
    public ICollection<Planet> Planets { get; set; }
    public ICollection<Starship> Starships { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; }
    public ICollection<Species> Species { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }
    public string Url { get; set; }
}