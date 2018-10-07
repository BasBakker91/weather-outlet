using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.Wiki;

namespace WeatherOutlet.ApiClients.Wiki
{
    public interface IWikiApiClient
    {
        Task<ApiResponse<WikiResult>> GetWikiDetailsAsync(string city);
        Task<ApiResponse<WikiResult>> GetWikiDetailsAsync(string city, CancellationToken cancellationToken);
    }
}