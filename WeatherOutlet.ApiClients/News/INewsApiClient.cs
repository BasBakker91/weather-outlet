using System.Threading.Tasks;
using WeatherOutlet.Shared.News;

namespace WeatherOutlet.ApiClients.News
{
    public interface INewsApiClient
    {
        Task<ApiResponse<NewsResult>> GetNewsAsync(string keyword);
    }
}