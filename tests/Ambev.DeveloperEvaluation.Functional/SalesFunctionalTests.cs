using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace Ambev.DeveloperEvaluation.Functional
{
    public class SalesFunctionalTests : IClassFixture<CustomWebApplicationDbTestFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationDbTestFactory _factory;
        private string JwtToken { get; }
        private readonly ITestOutputHelper _output;

        public SalesFunctionalTests(CustomWebApplicationDbTestFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
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

        private async Task<Cart> SeedCartAsync(Cart cart)
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Carts.Add(cart);
            await db.SaveChangesAsync();
            return cart;
        }

        private static readonly Faker<User> userFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Status, UserStatus.Active)
            .RuleFor(u => u.Role, UserRole.Admin);

        [Fact(DisplayName = "Sale flow")]
        public async Task Sale_Crud_Flow_Should_Work()
        {
            var userRequest = userFaker.Generate();
            userRequest.Password = "t3stC@rtUs#r";
            var userResponse = await _client.PostAsJsonAsync("/api/users", userRequest);
            var user = await userResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateUserResponse>>();

            var authResponse = await _client.PostAsJsonAsync("/api/auth", new { Email = userRequest.Email, Password = userRequest.Password });
            var auth = await authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<AuthenticateUserResponse>>();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.Data?.Token);

            var branch = await SeedBranchAsync(new Branch { Name = "Branch name", Description = "Branch description" });
            var product = await SeedProductAsync(new Product { Name = "Product name", Description = "Product description", Price = new Money(100) });
            var cart = await SeedCartAsync(
                new Cart(
                    user.Data.Id, 
                    new Money(product.Price.Value * 2), 
                    branch.Id, 
                    branch.Name, 
                    new List<CartItem> {
                        new CartItem(
                            product.Id, 
                            product.Name, 
                            2, 
                            product.Price, 
                            new Money(0), 
                            new Money(product.Price.Value * 2), 
                            new Money(product.Price.Value * 2)
                        )
                    }
                ));

            var saleRequest = new
            {
                CartId = cart.Id
            };
            var postResponse = await _client.PostAsJsonAsync("/api/sales", saleRequest);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
            created.Should().NotBeNull();
            created!.Data.SaleStatus.Should().Be(SaleStatusEnum.Created.ToString());

            var getResponse = await _client.GetAsync($"/api/sales/{created.Data.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedRequest = new
            {
                Id = created.Data.Id,
                BranchSaleId = created.Data.BranchSaleId,
                SaleItems = new[] { new { Id = created.Data.SaleItems.First().Id, ProductId = created.Data.SaleItems.First().ProductId, ProductItemQuantity = 4 } },
                SaleStatus = SaleStatusEnum.Modified.ToString()
            };
            var putResponse = await _client.PutAsJsonAsync($"/api/sales/{created.Data.Id}", updatedRequest);
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var check = await _client.GetFromJsonAsync<ApiResponseWithData<GetSaleResponse>>($"/api/sales/{created.Data.Id}");
            check!.Data.SaleStatus.Should().Be(SaleStatusEnum.Modified.ToString());

            var cancelResponse = await _client.PostAsJsonAsync($"/api/sales/{created.Data.Id}/cancel", new { Id = created.Data.Id });
            cancelResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleteResponse = await _client.DeleteAsync($"/api/sales/{created.Data.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var finalCheck = await _client.GetAsync($"/api/sales/{created.Data.Id}");
            finalCheck.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        [Fact(DisplayName = "Sales update and synchronization with mongodb")]
        public async Task Sales_Update_And_Synchronization_Should_Work()
        {
            var userRequest = userFaker.Generate();
            userRequest.Password = "t3stC@rtUs#r";
            var userResponse = await _client.PostAsJsonAsync("/api/users", userRequest);
            var user = await userResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateUserResponse>>();

            var authResponse = await _client.PostAsJsonAsync("/api/auth", new { Email = userRequest.Email, Password = userRequest.Password });
            var auth = await authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<AuthenticateUserResponse>>();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.Data?.Token);

            var branch = await SeedBranchAsync(new Branch { Name = "Branch name", Description = "Branch description" });
            var product = await SeedProductAsync(new Product { Name = "Product name", Description = "Product description", Price = new Money(100) });
            var cart = await SeedCartAsync(
                new Cart(
                    user.Data.Id,
                    new Money(product.Price.Value * 2),
                    branch.Id,
                    branch.Name,
                    new List<CartItem> {
                        new CartItem(
                            product.Id, 
                            product.Name, 
                            2, 
                            product.Price, 
                            new Money(0), 
                            new Money(product.Price.Value * 2), 
                            new Money(product.Price.Value * 2)
                        )
                    }
                ));

            var saleRequest = new
            {
                CartId = cart.Id
            };
            var postResponse = await _client.PostAsJsonAsync("/api/sales", saleRequest);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
            created.Should().NotBeNull();
            created!.Data.SaleStatus.Should().Be(SaleStatusEnum.Created.ToString());

            var getResponse = await _client.GetAsync($"/api/sales/{created.Data.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedRequest = new
            {
                Id = created.Data.Id,
                BranchSaleId = created.Data.BranchSaleId,
                SaleItems = new[] { new { Id = created.Data.SaleItems.First().Id, ProductId = created.Data.SaleItems.First().ProductId, ProductItemQuantity = 4 } },
                SaleStatus = SaleStatusEnum.Modified.ToString()
            };
            var putResponse = await _client.PutAsJsonAsync($"/api/sales/{created.Data.Id}", updatedRequest);
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var check = await _client.GetFromJsonAsync<ApiResponseWithData<GetSaleResponse>>($"/api/sales/{created.Data.Id}");
            check!.Data.SaleStatus.Should().Be(SaleStatusEnum.Modified.ToString());

            var query = $"?PageNumber=1&PageSize=5&SaleId={created.Data.Id}";
            var listResponse = await _client.GetAsync($"/api/sales{query}");
            var listResult = await listResponse.Content.ReadFromJsonAsync<ApiResponseWithData<IEnumerable<ListSalesResponse>>>();
            listResult.Data.First()
                .SaleItems.Where(x => x.ProductId == created.Data.SaleItems.First().ProductId)
                .First().ProductItemQuantity.Should().Be(4);

        }
    }
}
