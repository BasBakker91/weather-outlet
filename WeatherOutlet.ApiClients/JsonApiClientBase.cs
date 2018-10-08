using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherOutlet.ApiClients
{
    /// <summary>
    /// A Json Api base class that provides save requests
    /// It also fixes the issue of having thousands of sockets open. (Using HttpClient issue)
    /// </summary>
    public class JsonApiClientBase
    {
        private readonly HttpClient client;

        /// <summary>
        /// Creates an instance of <see cref="JsonApiClientBase"/>
        /// </summary>
        protected JsonApiClientBase(HttpClient httpClient)
        {
            client = httpClient;
        }

        /// <summary>
        /// Performs a GET request to the given uri
        /// </summary>
        /// <typeparam name="T">The return object's type</typeparam>
        /// <param name="uri">The endpoint to request to</param>
        /// <returns>An <see cref="ApiResponse{T}>"/> with the <see cref="System.Net.HttpStatusCode"/> and the object returned by the server</returns>
        /// <exception cref="ApiException">Thrown when the server responds without a success status code</exception>
        protected async Task<ApiResponse<T>> GetAsync<T>(string uri)
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

        /// <summary>
        /// Performs a POST requst to the given uri, without an expected return object
        /// </summary>
        /// <typeparam name="TRequestObject">The type of the POST content</typeparam>
        /// <param name="uri">The endpoint to request to</param>
        /// <param name="data">The data for the POST content</param>
        /// <returns>An <see cref="ApiResponse{T}>"/> with the <see cref="System.Net.HttpStatusCode"/> returned by the server</returns>
        /// <exception cref="ApiException">Thrown when the server responds without a success status code</exception>
        protected async Task<ApiResponse<object>> PostAsync<TRequestObject>(string uri, TRequestObject data)
        {
            using (var httpContent = new StringContent(JsonConvert.SerializeObject(data)))
            using (var response = await client.PostAsync(uri, httpContent))
            {
                var stringContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException(response.StatusCode, stringContent);
                }

                return new ApiResponse<object>(response.StatusCode, null);
            }
        }

        /// <summary>
        /// Performs a POST requst to the given uri
        /// </summary>
        /// <typeparam name="TRequestObject">The type of result content</typeparam>
        /// <typeparam name="TRequestObject">The type of the POST content</typeparam>
        /// <param name="uri">The endpoint to request to</param>
        /// <param name="data">The data for the POST content</param>
        /// <returns>An <see cref="ApiResponse{T}>"/> with the <see cref="System.Net.HttpStatusCode"/> and the object returned by the server</returns>
        /// <exception cref="ApiException">Thrown when the server responds without a success status code</exception>
        protected async Task<ApiResponse<TResultObject>> PostAsync<TResultObject, TRequestObject>(string uri, TRequestObject data)
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
