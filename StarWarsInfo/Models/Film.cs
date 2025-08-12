namespace StarWarsInfo.Models;

public class Film
{
    public int FilmId { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public int EpisodeId { get; set; } = 0;
    public string OpeningCrawl { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Producer { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; } = DateTime.MinValue;
    public ICollection<Person> Characters { get; set; } = new List<Person>();
    public ICollection<Planet> Planets { get; set; } = new List<Planet>();
    public ICollection<Starship> Starships { get; set; } = new List<Starship>();
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<Species> Species { get; set; } = new List<Species>();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Edited { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    
    public Film()
    {
    }
}