using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.News;

namespace WeatherOutlet.ApiClients.News
{
    public class NewsApiClient : JsonApiClientBase, INewsApiClient
    {
        private readonly NewsApiClientConfig newsApiClientConfig;

        public NewsApiClient(HttpClient httpClient, NewsApiClientConfig newsApiClientConfig) : base(httpClient)
        {
            this.newsApiClientConfig = newsApiClientConfig;
        }

        public async Task<ApiResponse<NewsResult>> GetNewsAsync(string keyword) => await GetNewsAsync(keyword, CancellationToken.None);
        public async Task<ApiResponse<NewsResult>> GetNewsAsync(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                return await GetAsync<NewsResult>(string.Format("everything?q={0}&sortBy=publishedAt&language=nl&apiKey={1}", keyword, newsApiClientConfig.ApiKey), cancellationToken);
            }
            catch (ApiException apiException)
            {
                if (apiException.StatusCode == HttpStatusCode.NotFound)
                {
                    return new ApiResponse<NewsResult>(HttpStatusCode.NotFound, null);
                }

                throw;
            }
        }
    }
}
