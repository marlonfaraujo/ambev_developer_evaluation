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
        private readonly ResponseData _responseData;

        public HelperControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            _jwtToken = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);

            _responseData = new ResponseData();
        }

        public string GetFakeJwtToken()
        {
            return _jwtToken;
        }

        public ResponseData GetResponseData()
        {
            return _responseData;
        }

        public async Task<AuthenticateUserResponse> GetAuthToken(User userRequest)
        {            
            if (!string.IsNullOrWhiteSpace(userRequest.Email))
                return _responseData.AuthUser;

            var authRequest = new
            {
                Email = userRequest.Email,
                Password = userRequest.Password
            };

            var authResponse = await _client.PostAsJsonAsync($"/api/auth", authRequest);
            var authUser = await authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<AuthenticateUserResponse>>();

            if (authUser != null && authUser.Data != null && !string.IsNullOrWhiteSpace(authUser.Data.Token))
                _responseData.AuthUser = authUser.Data;

            return _responseData.AuthUser;
        }

        public async Task<User> GetUser() 
        {
            if (_responseData.User != null 
                && !string.IsNullOrWhiteSpace(_responseData.User.Email))
                return _responseData.User;

            var userRequest = UserFaker.Generate();
            userRequest.Password = "ev@luAt10n";

            var userResponse = await _client.PostAsJsonAsync($"/api/users", userRequest);
            var user = await userResponse.Content.ReadFromJsonAsync<ApiResponseWithData<User>>();
            
            if (user != null && user.Data != null && user.Data.Id != Guid.Empty)
            {
                _responseData.User = user.Data;
                userRequest.Id = user.Data.Id;
                await GetAuthToken(userRequest);
            }

            return _responseData.User;
        }

        public async Task<Product> GetProduct()
        {
            if (_responseData.Product != null)
                return _responseData.Product;

            var productRequest = new
            {
                Name = "Product",
                Description = "Product for delete test",
                Price = 10.00m
            };

            var productResponse = await _client.PostAsJsonAsync($"/api/products", productRequest);
            var product = await productResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Product>>();

            if (product != null && product.Data != null)
            {
                _responseData.Product = product.Data;
            }

            return _responseData.Product;
        }

        public async Task<Branch> GetBranch()
        {
            if (_responseData.Branch != null)
                return _responseData.Branch;

            var branchRequest = new
            {
                Name = "Branch",
                Description = "Branch for delete test"
            };

            var branchResponse = await _client.PostAsJsonAsync($"/api/branchs", branchRequest);
            var branch = await branchResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>();

            if (branch != null && branch.Data != null)
            {
                _responseData.Branch = branch.Data;
            }

            return _responseData.Branch;
        }


        private static readonly Faker<User> UserFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
            .RuleFor(u => u.Status, f => UserStatus.Active)
            .RuleFor(u => u.Role, f => UserRole.Admin);

        public async Task<ResponseData> GetWithTestData()
        {
            await GetUser();

            if (_responseData.User == null 
                || _responseData.User.Id == Guid.Empty)
            {
                _responseData.User = await GetUser();
            }

            await GetProduct();

            await GetBranch();

            return _responseData;
        }
    }
}
