using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Sales
{
    public class SalesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SalesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            var token = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Verifies that creating a new sale via POST returns HTTP 201 Created.
        /// </summary>
        [Fact(DisplayName = "POST /api/sales should return Created when sale is valid")]
        public async Task Post_Sale_ReturnsCreated()
        {
            var responseProducts = await _client.GetAsync($"/api/products");
            var responseBranch = await _client.GetAsync($"/api/branchs");

            // Arrange
            var saleRequest = new
            {
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                BranchSaleId = 1,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    }
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/sales", saleRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a sale by ID via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/sales/{id} should return Ok when sale exists")]
        public async Task Get_Sale_By_Id_ReturnsOk()
        {
            // Arrange
            var saleRequest = new
            {
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                BranchSaleId = 1,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    } 
                }
            };
            var postResponse = await _client.PostAsJsonAsync("/api/sales", saleRequest);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<dynamic>();
            int saleId = created.id;

            // Act
            var getResponse = await _client.GetAsync($"/api/sales/{saleId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        /// <summary>
        /// Verifies that updating a sale via PUT returns HTTP 204 No Content.
        /// </summary>
        [Fact(DisplayName = "PUT /api/sales/{id} should return NoContent when update is successful")]
        public async Task Put_Sale_ReturnsNoContent()
        {
            // Arrange
            var saleRequest = new
            {
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                BranchSaleId = 1,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    }
                }
            };
            var postResponse = await _client.PostAsJsonAsync("/api/sales", saleRequest);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<dynamic>();
            int saleId = created.id;

            var updateRequest = new
            {
                Id = saleId,
                SaleDate = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                BranchSaleId = 1,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        ProductItemQuantity = 3,
                        UnitProductItemPrice = 10.0m
                    }
                }
            };

            // Act
            var putResponse = await _client.PutAsJsonAsync($"/api/sales/{saleId}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting a sale via DELETE returns HTTP 204 No Content.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/sales/{id} should return NoContent when delete is successful")]
        public async Task Delete_Sale_ReturnsNoContent()
        {
            // Arrange
            var saleRequest = new
            {
                UserId = Guid.NewGuid(),
                BranchSaleId = Guid.NewGuid(),
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = Guid.NewGuid(),
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    }
                }
            };
            var postResponse = await _client.PostAsJsonAsync("/api/sales", saleRequest);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<dynamic>();
            int saleId = created.id;

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/sales/{saleId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
