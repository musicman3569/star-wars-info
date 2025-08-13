using System.Threading;
using System.Threading.Tasks;

namespace StarWarsInfo.Services;

public interface IStarWarsImportService
{
    Task ImportAllDataAsync(CancellationToken cancellationToken = default);
}