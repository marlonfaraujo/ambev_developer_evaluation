using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional
{
    public class CartsFunctionalTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private string JwtToken { get; }

        public CartsFunctionalTests(CustomWebApplicationFactory factory)
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

        [Fact(DisplayName = "Cart flow")]
        public async Task Cart_Crud_Flow_Should_Work()
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
            created!.Data!.BranchSaleId.Should().Be(branch.Id);
            
            var getResponse = await _client.GetAsync($"/api/carts/{created.Data.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedBranch = await SeedBranchAsync(new Branch { Name = "Branch", Description = "Branch" });
            var updatedRequest = new
            {
                BranchSaleId = updatedBranch.Id,
                CartItems = new[]
                {
                    new { ProductId = product.Id, ProductItemQuantity = 4 }
                }
            };
            var putResponse = await _client.PutAsJsonAsync($"/api/carts/{created.Data.Id}", updatedRequest);
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var check = await _client.GetFromJsonAsync<ApiResponseWithData<GetCartResponse>>($"/api/carts/{created.Data.Id}");
            check!.Data!.BranchName.Should().Be("Branch");

            var deleteResponse = await _client.DeleteAsync($"/api/carts/{created.Data.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var finalCheck = await _client.GetAsync($"/api/carts/{created.Data.Id}");
            finalCheck.StatusCode.Should().Be(HttpStatusCode.NotFound);
            
        }
    }
}
