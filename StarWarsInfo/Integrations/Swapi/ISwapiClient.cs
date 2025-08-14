using System.Threading;
using System.Threading.Tasks;

namespace StarWarsInfo.Integrations.Swapi;

public interface ISwapiClient
{
    Task ImportAllDataAsync(CancellationToken cancellationToken = default);
}