using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Products
{
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            var token = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Verifies that creating a new product via POST returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/products should return Created when product is valid")]
        public async Task CreateProduct_ReturnsCreated()
        {
            var productRequest = new
            {
                Name = "Fake Product",
                Description = "A fake product for testing",
                Price = 99.99m
            };

            var response = await _client.PostAsJsonAsync("/api/products", productRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that updating a product via PUT returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "PUT /api/products should return Created when product is updated")]
        public async Task UpdateProduct_ReturnsCreated()
        {
            // First, create a product to update
            var createRequest = new
            {
                Name = "Product To Update",
                Description = "Product before update",
                Price = 50.00m,
                Stock = 10,
                Category = "Update Category"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            Guid productId = created.data.id;

            var updateRequest = new
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Product after update",
                Price = 75.00m,
                Stock = 20,
                Category = "Updated Category"
            };

            var response = await _client.PutAsJsonAsync("/api/products", updateRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a product by ID via GET returns HTTP 200 OK when the product exists.
        /// </summary>
        [Fact(DisplayName = "GET /api/products/{id} should return Ok when product exists")]
        public async Task GetProduct_ReturnsOk()
        {
            // First, create a product to retrieve
            var createRequest = new
            {
                Name = "Product To Get",
                Description = "Product for get test",
                Price = 10.00m,
                Stock = 5,
                Category = "Get Category"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            Guid productId = created.data.id;

            var response = await _client.GetAsync($"/api/products/{productId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that listing products via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/products should return Ok with product list")]
        public async Task ListProducts_ReturnsOk()
        {
            var listRequest = "?Category=Test Category"; // Example query string, adjust as needed
            var response = await _client.GetAsync($"/api/products{listRequest}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting a product via DELETE returns HTTP 200 OK when the product exists.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/products/{id} should return Ok when product is deleted")]
        public async Task DeleteProduct_ReturnsOk()
        {
            // First, create a product to delete
            var createRequest = new
            {
                Name = "Product To Delete",
                Description = "Product for delete test",
                Price = 20.00m,
                Stock = 2,
                Category = "Delete Category"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            Guid productId = created.data.id;

            var response = await _client.DeleteAsync($"/api/products/{productId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

