using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional
{
    public class BranchsFunctionalTests : IClassFixture<CustomWebApplicationDbTestFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationDbTestFactory _factory;
        private string JwtToken { get; }

        public BranchsFunctionalTests(CustomWebApplicationDbTestFactory factory)
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

        [Fact(DisplayName = "Cart flow with branch change")]
        public async Task Cart_With_Branch_Change_Should_Work()
        {
            var branch = await SeedBranchAsync(new Branch { Name = "Branch name", Description = "Branch description" });
            var product = await SeedProductAsync(new Product { Name = "Product name", Description = "Product description", Price = 100 });

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

            var updateBranchRequest = new
            {
                Id = branch.Id,
                Name = "Updated Branch Name",
                Description = "Branch after update"
            };

            var updateBranchResponse = await _client.PutAsJsonAsync($"/api/branchs/{branch.Id}", updateBranchRequest);
            updateBranchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getBranchResponse = await _client.GetAsync($"/api/branchs/{branch.Id}");
            getBranchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getBranch = await getBranchResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetBranchResponse>>();
            getBranch.Data.Name.Should().Be(updateBranchRequest.Name);

            var getCartResponse = await _client.GetAsync($"/api/carts/{created.Data.Id}");
            getCartResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getCart = await getCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetCartResponse>>();
            getCart.Data.BranchName.Should().Be(getBranch.Data.Name);

        }
    }
}
