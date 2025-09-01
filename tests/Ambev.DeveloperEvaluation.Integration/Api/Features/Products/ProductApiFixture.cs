using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using System.Net.Http.Json;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Products
{
    public class ProductApiFixture : IDisposable
    {
        public HttpClient Client { get; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string JwtToken { get; }

        public ProductApiFixture()
        {
            var factory = new CustomWebApplicationFactory();
            Client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            JwtToken = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            var response = Client.PostAsJsonAsync("/api/products", new { Name = "Name Initial", Description = "Initial", Price = 10.00m }).Result;
            var product = response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>().Result;
            ProductId = (Guid)(product!.Data!.Id);
            ProductName = product!.Data!.Name;
        }

        public Guid GetNewProductId()
        {
            var response = Client.PostAsJsonAsync("/api/products", new { Name = "Initial", Description = "Initial", Price = 10.00m }).Result;
            var product = response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>().Result;
            return (Guid)(product!.Data!.Id);
        }

        public void Dispose()
        {
            Client.DeleteAsync($"/api/products/{ProductId}").Wait();
        }
    }
}
