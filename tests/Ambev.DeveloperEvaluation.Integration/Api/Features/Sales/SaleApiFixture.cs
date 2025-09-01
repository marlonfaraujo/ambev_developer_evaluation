using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Bogus;
using System.Net.Http.Json;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Sales
{
    public class SaleApiFixture : IDisposable
    {
        public HttpClient Client { get; }
        public Guid SaleId { get; private set; }
        public string JwtToken { get; private set; }
        public Guid BranchId { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid CartId { get; private set; }
        public Guid UserId { get; private set; }
        public List<SaleItem> SaleItems { get; private set; }

        private static readonly Faker<User> userFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Status, UserStatus.Active)
            .RuleFor(u => u.Role, UserRole.Admin);

        public SaleApiFixture()
        {
            var factory = new CustomWebApplicationFactory();
            Client = factory.CreateClient();

            SetSaleParams();
        }

        private void SetSaleParams()
        {
            var userRequest = userFaker.Generate();
            userRequest.Password = "t3stC@rtUs#r";

            var userResponse = Client.PostAsJsonAsync("/api/users", userRequest).Result;
            var user = userResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateUserResponse>>().Result;
            UserId = (Guid)(user.Data?.Id);

            var authResponse = Client.PostAsJsonAsync("/api/auth", new { Email = userRequest.Email, Password = userRequest.Password }).Result;
            var auth = authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<AuthenticateUserResponse>>().Result;
            JwtToken = auth.Data?.Token;
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            var branchResponse = Client.PostAsJsonAsync("/api/branchs", new { Name = "Initial", Description = "Initial" }).Result;
            var branch = branchResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateBranchResponse>>().Result;
            BranchId = (Guid)(branch.Data?.Id);

            var productResponse = Client.PostAsJsonAsync("/api/products", new { Name = "Initial", Description = "Initial", Price = 10.00m }).Result;
            var product = productResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>().Result;
            ProductId = (Guid)(product.Data?.Id);

            var cartRequest = new
            {
                BranchSaleId = BranchId,
                CartItems = new[]
                {
                    new { ProductId = ProductId, ProductItemQuantity = 2 }
                }
            };

            var cartResponse = Client.PostAsJsonAsync("/api/carts", cartRequest).Result;
            var cart = cartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>().Result;
            CartId = (Guid)(cart.Data?.Id);


            var saleRequest = new
            {
                CartId = CartId
            };
            var saleResponse = Client.PostAsJsonAsync("/api/sales", saleRequest).Result;
            var sale = saleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>().Result;
            SaleId = (Guid)(sale.Data?.Id);
            SaleItems = sale.Data?.SaleItems.Select(x => 
                new SaleItem(
                    x.Id, 
                    x.ProductId, 
                    x.ProductItemQuantity, 
                    x.UnitProductItemPrice, 
                    x.DiscountAmount, 
                    x.TotalSaleItemPrice, 
                    x.TotalWithoutDiscount, 
                    x.SaleItemStatus)).ToList();
        }

        public void NewCartId()
        {
            var cartRequest = new
            {
                BranchSaleId = BranchId,
                CartItems = new[]
                {
                    new { ProductId = ProductId, ProductItemQuantity = 2 }
                }
            };

            var response = Client.PostAsJsonAsync("/api/carts", cartRequest).Result;
            var cart = response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>().Result!;
            CartId = (Guid)(cart.Data?.Id);
            UserId = (Guid)(cart.Data?.UserId);
        }

        public void NewSaleId()
        {
            SetSaleParams();
        }

        public void Dispose()
        {
            Client.PostAsJsonAsync($"/api/sales/{SaleId}/cancel", new { Id = SaleId }).Wait();
            Client.DeleteAsync($"/api/sales/{SaleId}").Wait();
        }
    }
}
