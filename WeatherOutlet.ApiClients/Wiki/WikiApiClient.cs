using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.Wiki;

namespace WeatherOutlet.ApiClients.Wiki
{
    public class WikiApiClient : JsonApiClientBase, IWikiApiClient
    {
        public WikiApiClient(HttpClient httpClient) : base(httpClient)
        {

        }

        public async Task<ApiResponse<WikiResult>> GetWikiDetailsAsync(string city) => await GetWikiDetailsAsync(city, CancellationToken.None);
        public async Task<ApiResponse<WikiResult>> GetWikiDetailsAsync(string city, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetAsync<JArray>(string.Format("w/api.php?action=opensearch&search={0}&limit=1&format=json", city), cancellationToken);

                if (result == null || !result.Content.Any())
                    return null;

                var contentResult = result.Content;

                var names = contentResult[1].Select(j => j.Value<string>()).ToArray();

                if (!names.Any())
                    return null;

                var desciptions = contentResult[2].Select(j => j.Value<string>()).ToArray();
                var urls = contentResult[3].Select(j => j.Value<string>()).ToArray();


                var name = names.FirstOrDefault();
                var desciption = desciptions.FirstOrDefault();
                var url = urls.FirstOrDefault();

                return new ApiResponse<WikiResult>(result.StatusCode, new WikiResult()
                {
                    Name = name,
                    Description = desciption,
                    Uri = url,
                });
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
    }
}
