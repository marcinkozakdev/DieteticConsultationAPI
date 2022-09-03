using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Hosting;

namespace DieticianConsultationAPI.IntegrationTest
{
    public class DieticianControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DieticianControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=10&pageNumber=2")]
        [InlineData("pageSize=50&pageNumber=3")]
        public async Task GetAllDieticians_WithQueryParameters_ReturnOkResult(string queryParams)
        {
            // arange

          

            // act

            var response = await _client.GetAsync("/api/dietician?pageSize=5&pageNumber=1");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }

        [Theory]
        [InlineData("pageSize=100&pageNumber=1")]
        [InlineData("pageSize=11&pageNumber=2")]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAllInvalidQueryParams_ReturnBadRequest(string queryParams)
        {
            // arange

         

            // act

            var response = await _client.GetAsync("/api/dietician?pageSize=5&pageNumber=1");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
