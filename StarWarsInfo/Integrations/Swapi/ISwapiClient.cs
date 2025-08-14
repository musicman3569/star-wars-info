using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StarWarsInfo.Integrations.Swapi;

public interface ISwapiClient
{
    Task<OkObjectResult> ImportAllDataAsync(CancellationToken cancellationToken = default);
}