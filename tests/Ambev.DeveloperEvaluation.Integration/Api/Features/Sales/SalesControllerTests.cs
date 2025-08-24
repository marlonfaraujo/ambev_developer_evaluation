using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
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

        private static readonly Faker<User> UserFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
            .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin));

        public async Task<ResponseData> GetData()
        {
            var productRequest = new
            {
                Name = "Product",
                Description = "Product for delete test",
                Price = 10.00m
            };
            var branchRequest = new
            {
                Name = "Branch",
                Description = "Branch for delete test"
            };
            var userRequest = UserFaker.Generate();
            userRequest.Password = "ev@luAt10n";

            var productResponse = _client.PostAsJsonAsync($"/api/products", productRequest);
            var branchResponse = _client.PostAsJsonAsync($"/api/branchs", branchRequest);
            var userResponse = _client.PostAsJsonAsync($"/api/users", userRequest);

            await Task.WhenAll(productResponse, branchResponse, userResponse);

            var productResult = await productResponse;
            var branchResult = await branchResponse;
            var userResult = await userResponse;

            var product = productResult.Content.ReadFromJsonAsync<ApiResponseWithData<Product>>().Result.Data;
            var branch = branchResult.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>().Result.Data;
            var user = userResult.Content.ReadFromJsonAsync<ApiResponseWithData<User>>().Result.Data;

            return new ResponseData { Product = product, Branch = branch, User = user };
        }

        /// <summary>
        /// Verifies that creating a new sale via POST returns HTTP 201 Created.
        /// </summary>
        [Fact(DisplayName = "POST /api/sales should return Created when sale is valid")]
        public async Task Post_Sale_ReturnsCreated()
        {
            var responseData = await GetData();
            // Arrange
            var saleRequest = new
            {
                SaleDate = DateTime.UtcNow,
                UserId = responseData.User.Id,
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = responseData.Product.Id,
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    }
                }
            };

            // Act
            var response = await _client.PostAsync("/api/sales", null);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a sale by ID via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/sales/{id} should return Ok when sale exists")]
        public async Task Get_Sale_By_Id_ReturnsOk()
        {
            var responseData = await GetData();

            // Arrange
            var saleRequest = new
            {
                SaleDate = DateTime.Now,
                UserId = responseData.User.Id,
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = responseData.Product.Id,
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    } 
                }
            };
            var postResponse = await _client.PostAsync("/api/sales", null);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Sale>>();
            Guid saleId = created.Data.Id;

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
            var responseData = await GetData();
            // Arrange
            var saleRequest = new
            {
                SaleDate = DateTime.Now,
                UserId = responseData.User.Id,
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = responseData.Product.Id,
                        ProductItemQuantity = 2,
                        UnitProductItemPrice = 10.0m
                    }
                }
            };
            var postResponse = await _client.PostAsync("/api/sales", null);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Sale>>();
            Guid saleId = created.Data.Id;

            var updateRequest = new
            {
                Id = saleId,
                SaleDate = DateTime.UtcNow,
                UserId = responseData.User.Id,
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem
                    {
                        ProductId = responseData.Product.Id,
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
            var postResponse = await _client.PostAsync("/api/sales", null);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Sale>>();
            Guid saleId = created.Data.Id;

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/sales/{saleId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
