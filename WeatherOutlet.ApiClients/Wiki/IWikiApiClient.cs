using System.Threading.Tasks;
using WeatherOutlet.Shared.Wiki;

namespace WeatherOutlet.ApiClients.Wiki
{
    public interface IWikiApiClient
    {
        Task<ApiResponse<WikiResult>> GetWikiDetailsAsync(string city);
    }
}