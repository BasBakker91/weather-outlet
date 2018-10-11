using Moq;
using System;
using WeatherOutlet.ApiClients.OpenWeather;
using WeatherOutlet.Server.Controllers;
using WeatherOutlet.Shared.Weather;
using Xunit;
using System.Threading.Tasks;
using WeatherOutlet.ApiClients;
using WeatherOutlet.ApiClients.News;
using WeatherOutlet.Shared.News;
using WeatherOutlet.ApiClients.Wiki;
using WeatherOutlet.Shared.Wiki;
using System.Net;

namespace WeatherOutlet.Server.Tests
{
    public class PlacesControllerTests
    {
        private readonly OpenWeatherApiClient openWeatherApiClient;
        private readonly NewsApiClient newsApiClient;
        private readonly WikiApiClient wikiApiClient;

        public PlacesControllerTests()
        {
            openWeatherApiClient = CreateMockWeatherApiClient();
            newsApiClient = CreateMockNewsApiClient();
            wikiApiClient = CreateMockWikiApiClient();
        }

        [Theory]
        [InlineData("Nieuwveen")]
        [InlineData("Amsterdam")]
        public async Task SearchAsync_SimpleConcurrencyCheck(string keyword)
        {
            // Arrange
            var placesController = new PlacesController(openWeatherApiClient, wikiApiClient, newsApiClient);

            // Act
            var result = await placesController.SearchAsync(keyword);

            // Assert
            Assert.NotNull(result.Articles);
            Assert.NotNull(result.WeatherForecast);
            Assert.NotNull(result.PlaceDetails);
        }

        [Fact]
        public async Task SearchAsync_ResultWithWeatherResultsNotFound()
        {
            // Arrange
            var keyword = "WeatherCheck";
            var placesController = new PlacesController(openWeatherApiClient, wikiApiClient, newsApiClient);

            // Act
            var result = await placesController.SearchAsync(keyword);

            // Assert
            Assert.NotNull(result.Articles);
            Assert.Null(result.WeatherForecast);
            Assert.NotNull(result.PlaceDetails);
        }

        [Theory]
        [InlineData("Nieuwveen")]
        [InlineData("Amsterdam")]
        public async Task SearchAsync_PlaceIsSameAsKeyword(string expected)
        {
            // Arrange
            var placesController = new PlacesController(openWeatherApiClient, wikiApiClient, newsApiClient);

            // Act
            var result = await placesController.SearchAsync(expected);

            // Assert
            Assert.Equal(expected, result.Place);
        }

        [Fact]
        public async Task SearchAsync_ResultWithWikiResultsNotFound()
        {
            // Arrange
            var keyword = "WikiCheck";
            var placesController = new PlacesController(openWeatherApiClient, wikiApiClient, newsApiClient);

            // Act
            var result = await placesController.SearchAsync(keyword);

            // Assert
            Assert.NotNull(result.Articles);
            Assert.NotNull(result.WeatherForecast);
            Assert.Null(result.PlaceDetails);
        }

        [Fact]
        public async Task SearchAsync_ResultWithNewsResultsNotFound()
        {
            // Arrange
            var keyword = "NewsCheck";
            var placesController = new PlacesController(openWeatherApiClient, wikiApiClient, newsApiClient);

            // Act
            var result = await placesController.SearchAsync(keyword);

            // Assert
            Assert.Null(result.Articles);
            Assert.NotNull(result.WeatherForecast);
            Assert.NotNull(result.PlaceDetails);
        }



        [Fact]
        public async Task SearchAsync_CheckExceptionThrownWithoutWeatherApi()
        {
            // Arrange
            string keyword = "Amsterdam";
            var placesController = new PlacesController(null, wikiApiClient, newsApiClient);

            // Act

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => placesController.SearchAsync(keyword));
        }

        [Fact]
        public async Task SearchAsync_CheckExceptionThrownWithoutWikiApi()
        {
            // Arrange
            string keyword = "Amsterdam";
            var placesController = new PlacesController(openWeatherApiClient, null, newsApiClient);

            // Act

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => placesController.SearchAsync(keyword));
        }

        [Fact]
        public async Task SearchAsync_CheckExceptionThrownWithoutNewsApi()
        {
            // Arrange
            string keyword = "Amsterdam";
            var placesController = new PlacesController(openWeatherApiClient, wikiApiClient, null);

            // Act

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => placesController.SearchAsync(keyword));
        }

        private ApiResponse<WeatherForecast> ReturnWeatherForecastResultFromKeyword(string keyword)
        {
            switch (keyword)
            {
                case "Nieuwveen":
                case "Amsterdam":
                case "NewsCheck":
                case "WikiCheck":
                    return new ApiResponse<WeatherForecast>(HttpStatusCode.OK, new WeatherForecast()
                    {
                        City = new City()
                        {
                            Name = keyword
                        },
                    });
                default:
                    return new ApiResponse<WeatherForecast>(HttpStatusCode.NotFound, null);
            }
        }

        private ApiResponse<NewsResult> ReturnNewsResultFromKeyword(string keyword)
        {
            switch (keyword)
            {
                case "Nieuwveen":
                case "Amsterdam":
                case "WikiCheck":
                case "WeatherCheck":
                    return new ApiResponse<NewsResult>(HttpStatusCode.OK, new NewsResult()
                    {
                        Articles = new Article[]
                        {
                            new Article()
                            {
                                Description = keyword,
                                Title = keyword,
                            }
                        },
                    });
                default:
                    return new ApiResponse<NewsResult>(HttpStatusCode.NotFound, null);
            }
        }

        private ApiResponse<WikiResult> ReturnWikiResultFromKeyword(string keyword)
        {
            switch (keyword)
            {
                case "Nieuwveen":
                case "Amsterdam":
                case "WeatherCheck":
                case "NewsCheck":
                    return new ApiResponse<WikiResult>(HttpStatusCode.OK, new WikiResult()
                    {
                        Name = keyword,
                    });
                default:
                    return new ApiResponse<WikiResult>(HttpStatusCode.NotFound, null);
            }
        }

        private OpenWeatherApiClient CreateMockWeatherApiClient()
        {
            var mockWeatherApiClient = new Mock<OpenWeatherApiClient>();
            mockWeatherApiClient.Setup(api => api.GetWeatherForecastAsync(It.IsAny<string>()))
                .Returns((string keyword) => Task.FromResult(ReturnWeatherForecastResultFromKeyword(keyword)));

            return mockWeatherApiClient.Object;
        }

        private NewsApiClient CreateMockNewsApiClient()
        {
            var mockNewsApiClient = new Mock<NewsApiClient>();
            mockNewsApiClient.Setup(api => api.GetNewsAsync(It.IsAny<string>()))
                .Returns((string keyword) => Task.FromResult(ReturnNewsResultFromKeyword(keyword)));

            return mockNewsApiClient.Object;
        }

        private WikiApiClient CreateMockWikiApiClient()
        {
            var mockWikiApiClient = new Mock<WikiApiClient>();
            mockWikiApiClient.Setup(api => api.GetWikiDetailsAsync(It.IsAny<string>()))
                .Returns((string keyword) => Task.FromResult(ReturnWikiResultFromKeyword(keyword)));

            return mockWikiApiClient.Object;
        }
    }
}
