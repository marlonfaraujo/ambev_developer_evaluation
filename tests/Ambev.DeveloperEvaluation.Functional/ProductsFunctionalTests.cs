using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional
{
    public class ProductsFunctionalTests : IClassFixture<CustomWebApplicationDbTestFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationDbTestFactory _factory;
        private string JwtToken { get; }

        public ProductsFunctionalTests(CustomWebApplicationDbTestFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            JwtToken = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);
        }

        private async Task<Branch> SeedBranchAsync(Branch branch)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Branchs.Add(branch);
            await db.SaveChangesAsync();
            return branch;
        }

        private async Task<Product> SeedProductAsync(Product product)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return product;
        }

        [Fact(DisplayName = "Cart flow with product change")]
        public async Task Cart_With_Product_Change_Should_Work()
        {
            var branch = await SeedBranchAsync(new Branch { Name = "Branch name", Description = "Branch description" });
            var product = await SeedProductAsync(new Product { Name = "Product name", Description = "Product description", Price = new Money(100) });

            var cartRequest = new
            {
                BranchSaleId = branch.Id,
                CartItems = new[]
                {
                    new { ProductId = product.Id, ProductItemQuantity = 2 }
                }
            };
            var postResponse = await _client.PostAsJsonAsync("/api/carts", cartRequest);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();
            created.Should().NotBeNull();

            var updateProductRequest = new
            {
                Id = product.Id,
                Name = "Updated Product Name",
                Description = "Product after update",
                Price = product.Price.Value
            };

            var updateProductResponse = await _client.PutAsJsonAsync($"/api/products/{product.Id}", updateProductRequest);
            updateProductResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProductResponse = await _client.GetAsync($"/api/products/{product.Id}");
            getProductResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getProduct = await getProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetProductResponse>>();
            getProduct.Data.Name.Should().Be(updateProductRequest.Name);

            var getCartResponse = await _client.GetAsync($"/api/carts/{created.Data.Id}");
            getCartResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getCart = await getCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetCartResponse>>();
            getCart.Data.CartItems.Select(x => x.ProductId == product.Id).Should().NotBeNull();
            getCart.Data.CartItems.Where(x => x.ProductId == product.Id).FirstOrDefault().ProductName.Should().Be(getProduct.Data.Name);

        }
    }
}
