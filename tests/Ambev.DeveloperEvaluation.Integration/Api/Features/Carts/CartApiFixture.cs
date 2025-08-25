using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Bogus;
using System.Net.Http.Json;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Carts
{
    public class CartApiFixture : IDisposable
    {
        public HttpClient Client { get; }
        public Guid CartUserId { get; private set; }
        public string JwtToken { get; }
        public Guid BranchId { get; private set; }
        public Guid ProductId { get; private set; }

        private static readonly Faker<User> userFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Status, UserStatus.Active)
            .RuleFor(u => u.Role, UserRole.Admin);

        public CartApiFixture()
        {
            var factory = new CustomWebApplicationFactory();
            Client = factory.CreateClient();

            var userRequest = userFaker.Generate();
            userRequest.Password = "t3stC@rtUs#r";

            var userResponse = Client.PostAsJsonAsync("/api/users", userRequest).Result;
            var user = userResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateUserResponse>>().Result;
            CartUserId = (Guid)(user.Data?.Id);

            var authResponse = Client.PostAsJsonAsync("/api/auth", new { Email = userRequest.Email, Password = userRequest.Password }).Result;
            var auth = authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<ApiResponseWithData<AuthenticateUserResponse>>>().Result;
            JwtToken = auth.Data.Data.Token;
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            var branchResponse = Client.PostAsJsonAsync("/api/branchs", new Branch { Name = "Initial", Description = "Initial" }).Result;
            var branch = branchResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>().Result;
            BranchId = (Guid)(branch.Data?.Id);

            var productResponse = Client.PostAsJsonAsync("/api/products", new Product { Name = "Initial", Description = "Initial", Price = 10.00m }).Result;
            var product = productResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Product>>().Result;
            ProductId = (Guid)(product.Data?.Id);

            var cartRequest = new
            {
                BranchSaleId = BranchId,
                SaleItems = new[]
                {
                    new { ProductId = ProductId, ProductItemQuantity = 2 }
                }
            };

            var response = Client.PostAsJsonAsync("/api/carts", cartRequest).Result;
            var cart = response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>().Result;
            CartUserId = (Guid)(cart.Data?.UserId);
        }

        public void NewCartUserId()
        {
            var cartRequest = new
            {
                BranchSaleId = BranchId,
                SaleItems = new[]
                {
                    new { ProductId = ProductId, ProductItemQuantity = 2 }
                }
            };

            var response = Client.PostAsJsonAsync("/api/carts", cartRequest).Result;
            var cart = response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>().Result!;
            CartUserId = (Guid)(cart.Data?.UserId);
        }

        public void Dispose()
        {
            Client.DeleteAsync($"/api/carts").Wait();
        }
    }
}
