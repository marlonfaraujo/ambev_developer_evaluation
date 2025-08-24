using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features
{
    public class HelperControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly string _jwtToken;

        public HelperControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            _jwtToken = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);
        }

        public string GetJwtToken()
        {
            return _jwtToken;
        }

        private static readonly Faker<User> UserFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Status, f => UserStatus.Active)
            .RuleFor(u => u.Role, f => UserRole.Admin);

        public async Task<ResponseData> GetTestData()
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

            var authRequest = new
            {
                Email = userRequest.Email,
                Password = userRequest.Password
            };

            var userResponse = _client.PostAsJsonAsync($"/api/users", userRequest);
            var productResponse = _client.PostAsJsonAsync($"/api/products", productRequest);
            var branchResponse = _client.PostAsJsonAsync($"/api/branchs", branchRequest);

            await Task.WhenAll(userResponse, productResponse, branchResponse, authResponse);

            var userResult = await userResponse;
            var productResult = await productResponse;
            var branchResult = await branchResponse;

            var authResponse = await _client.PostAsJsonAsync($"/api/auth", authRequest);

            var product = productResult.Content.ReadFromJsonAsync<ApiResponseWithData<Product>>().Result.Data;
            var branch = branchResult.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>().Result.Data;
            var user = userResult.Content.ReadFromJsonAsync<ApiResponseWithData<User>>().Result.Data;
            var authUser = authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<AuthenticateUserResponse>>().Result.Data;

            return new ResponseData { Product = product, Branch = branch, User = user, AuthUser = authUser };
        }
    }
}
