using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.Wiki;

namespace WeatherOutlet.ApiClients.Wiki
{
    /// <summary>
    /// An Api Client that interacts with <a href="https://www.mediawiki.org/wiki/API:Search">Wikipedia's Api</a>
    /// </summary>
    public class WikiApiClient : JsonApiClientBase, IWikiApiClient
    {
        /// <summary>
        /// Creates an instance of <see cref="WikiApiClient"/>
        /// </summary>
        public WikiApiClient(HttpClient httpClient) : base(httpClient)
        {
            
        }

        /// <summary>
        /// Returns the first wikipedia article that matches the keyword
        /// </summary>
        public async Task<ApiResponse<WikiResult>> GetWikiDetailsAsync(string keyword)
        {
            try
            {
                if (keyword == null)
                    throw new ArgumentNullException(nameof(keyword));

                if (string.IsNullOrWhiteSpace(keyword))
                    throw new InvalidOperationException("Can not request wiki page without a keyword");

                var uri = $"w/api.php?action=opensearch&search={keyword}&limit=1&format=json";

                var result = await GetAsync<JArray>(uri);

                if (result == null || !result.Content.Any())
                    return null;

                var wikiResult = ExtractWikiResult(result.Content);

                return new ApiResponse<WikiResult>(result.StatusCode, wikiResult);
            }
            catch(ApiException apiException)
            {
                if(apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        /// <summary>
        /// Extracts the wiki result from json provided by the Wiki Api
        /// </summary>
        private WikiResult ExtractWikiResult(JArray jArray)
        {
            var names = jArray[1].Select(j => j.Value<string>()).ToArray();

            if (!names.Any())
                return null;

            var desciptions = jArray[2].Select(j => j.Value<string>()).ToArray();
            var urls = jArray[3].Select(j => j.Value<string>()).ToArray();

            return new WikiResult()
            {
                Name = names.FirstOrDefault(),
                Description = desciptions.FirstOrDefault(),
                Uri = urls.FirstOrDefault(),
            };
        }
    }
}
