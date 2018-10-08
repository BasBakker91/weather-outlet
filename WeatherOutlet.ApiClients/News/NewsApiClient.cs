using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.News;

namespace WeatherOutlet.ApiClients.News
{
    /// <summary>
    /// An Api Client that interacts with <a href="https://newsapi.org/docs/endpoints/everything">NewsAPI.org's Api</a>
    /// </summary>
    public class NewsApiClient : JsonApiClientBase, INewsApiClient
    {
        private readonly NewsApiClientConfig newsApiClientConfig;

        /// <summary>
        /// Creates an instance of <see cref="NewsApiClient"/>
        /// </summary>
        public NewsApiClient(HttpClient httpClient, NewsApiClientConfig newsApiClientConfig) : base(httpClient)
        {
            this.newsApiClientConfig = newsApiClientConfig;
        }

        /// <summary>
        /// Gets the top 20 news articles containing the keyword
        /// </summary>
        /// <param name="keyword">A keyword that the news articles must contain</param>
        /// <returns>List of found news articles</returns>
        public async Task<ApiResponse<NewsResult>> GetNewsAsync(string keyword)
        {
            try
            {
                var uri = $"everything?q={keyword}&sortBy=publishedAt&language=nl&apiKey={newsApiClientConfig.ApiKey}";

                return await GetAsync<NewsResult>(uri);
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
