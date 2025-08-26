using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Products
{
    public class ProductControllerTests : IClassFixture<ProductApiFixture>
    {
        private readonly ProductApiFixture _productApiFixture;

        public ProductControllerTests(ProductApiFixture productApiFixture)
        {
            _productApiFixture = productApiFixture;
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

            var response = await _productApiFixture.Client.PostAsJsonAsync("/api/products", productRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that updating a product via PUT returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "PUT /api/products should return Created when product is updated")]
        public async Task UpdateProduct_ReturnsCreated()
        {
            var updateRequest = new
            {
                Id = _productApiFixture.ProductId,
                Name = "Updated Product",
                Description = "Product after update",
                Price = 75.00m
            };

            var response = await _productApiFixture.Client.PutAsJsonAsync($"/api/products/{_productApiFixture.GetNewProductId()}", updateRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a product by ID via GET returns HTTP 200 OK when the product exists.
        /// </summary>
        [Fact(DisplayName = "GET /api/products/{id} should return Ok when product exists")]
        public async Task GetProduct_ReturnsOk()
        {

            var response = await _productApiFixture.Client.GetAsync($"/api/products/{_productApiFixture.GetNewProductId()}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that listing products via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/products should return Ok with product list")]
        public async Task ListProducts_ReturnsOk()
        {
            var listRequest = "?Category=Test Category"; // Example query string, adjust as needed
            var response = await _productApiFixture.Client.GetAsync($"/api/products{listRequest}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting a product via DELETE returns HTTP 200 OK when the product exists.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/products/{id} should return Ok when product is deleted")]
        public async Task DeleteProduct_ReturnsOk()
        {
            var response = await _productApiFixture.Client.DeleteAsync($"/api/products/{_productApiFixture.ProductId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

