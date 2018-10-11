using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherOutlet.ApiClients.Wiki;
using Xunit;

namespace WeatherOutlet.ApiClients.IntegrationTests
{
    public class WikiApiClientIntegrationTests
    {
        private readonly WikiApiClient wikiApiClient; 

        public WikiApiClientIntegrationTests()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://en.wikipedia.org/"),
                Timeout = TimeSpan.FromSeconds(30),
            };

            wikiApiClient = new WikiApiClient(httpClient);
        }


        [Theory]
        [InlineData("Nieuwveen")]
        [InlineData("Amsterdam")]
        [InlineData("New York")]
        public async Task GetWikiDetailsAsync_SearchForKeyword(string keyword)
        {
            // Arrange
            var expected = HttpStatusCode.OK;

            // Act
            var result = await wikiApiClient.GetWikiDetailsAsync(keyword);

            // Assert
            Assert.Equal(expected, result.StatusCode);
            Assert.Equal(keyword, result.Content.Name);
        }

        [Fact]
        public async Task GetWikiDetailsAsync_NullKeywordCheck()
        {
            // Arrange
            string keyword = null;

            // Act


            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => wikiApiClient.GetWikiDetailsAsync(keyword));
        }

        [Fact]
        public async Task GetWikiDetailsAsync_StringEmptyKeywordCheck()
        {
            // Arrange
            string keyword = string.Empty;

            // Act


            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => wikiApiClient.GetWikiDetailsAsync(keyword));
        }

        [Fact]
        public async Task GetWikiDetailsAsync_InvalidKeywordCheck()
        {
            // Arrange
            string keyword = "thiisaninvalidkeywordthatwikidoesnotunderstand";
            var expected = HttpStatusCode.NotFound;

            // Act
            var result = await wikiApiClient.GetWikiDetailsAsync(keyword);
            var actual = result.StatusCode;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
