using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherOutlet.ApiClients
{
    public class JsonApiClientBase
    {
        private readonly HttpClient client;

        protected JsonApiClientBase(HttpClient httpClient)
        {
            client = httpClient;
        }

        protected async Task<ApiResponse<T>> GetAsync<T>(string uri, CancellationToken cancellationToken)
        {
            using (var response = await client.GetAsync(uri))
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException(response.StatusCode, stringContent);
                }

                var deserializedObject = JsonConvert.DeserializeObject<T>(stringContent);

                var apiReponse = new ApiResponse<T>(response.StatusCode, deserializedObject);

                return apiReponse;
            }
        }

        protected async Task PostAsync<TRequestObject>(string uri, TRequestObject data, CancellationToken cancellationToken)
        {
            using (var httpContent = new StringContent(JsonConvert.SerializeObject(data)))
            using (var response = await client.PostAsync(uri, httpContent))
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException(response.StatusCode, stringContent);
                }
            }
        }

        protected async Task<ApiResponse<TResultObject>> PostAsync<TResultObject, TRequestObject>(string uri, TRequestObject data, CancellationToken cancellationToken)
        {
            using (var httpContent = new StringContent(JsonConvert.SerializeObject(data)))
            using (var response = await client.PostAsync(uri, httpContent))
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException(response.StatusCode, stringContent);
                }

                TResultObject deserializedObject = JsonConvert.DeserializeObject<TResultObject>(stringContent);
                var apiResponse = new ApiResponse<TResultObject>(response.StatusCode, deserializedObject);

                return apiResponse;
            }
        }
    }
}
