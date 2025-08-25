using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Branchs
{
    public class BranchControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly HelperControllerTests _helperControllerTests;

        public BranchControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            _helperControllerTests = new HelperControllerTests(factory);
            var token = _helperControllerTests.GetFakeJwtToken();

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Verifies that creating a new branch via POST returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/branchs should return Created when branch is valid")]
        public async Task CreateBranch_ReturnsCreated()
        {
            var branchRequest = new
            {
                Name = "Fake Branch",
                Description = "123 Fake Street"
            };

            var response = await _client.PostAsJsonAsync("/api/branchs", branchRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that updating a branch via PUT returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "PUT /api/branchs should return Created when branch is updated")]
        public async Task UpdateBranch_ReturnsCreated()
        {
            // First, create a branch to update
            var createRequest = new
            {
                Name = "Branch To Update",
                Description = "456 Update Ave"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/branchs", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>();
            Guid branchId = created.Data.Id;

            var updateRequest = new
            {
                Id = branchId,
                Name = "Updated Branch",
                Description = "789 Updated Blvd"
            };

            var response = await _client.PutAsJsonAsync($"/api/branchs/{branchId}", updateRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a branch by ID via GET returns HTTP 200 OK when the branch exists.
        /// </summary>
        [Fact(DisplayName = "GET /api/branchs/{id} should return Ok when branch exists")]
        public async Task GetBranch_ReturnsOk()
        {
            // First, create a branch to retrieve
            var createRequest = new
            {
                Name = "Branch To Get",
                Description = "101 Get St"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/branchs", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>();
            Guid branchId = created.Data.Id;

            var response = await _client.GetAsync($"/api/branchs/{branchId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that listing branches via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/branchs should return Ok with branch list")]
        public async Task ListBranchs_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/branchs");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting a branch via DELETE returns HTTP 200 OK when the branch exists.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/branchs/{id} should return Ok when branch is deleted")]
        public async Task DeleteBranch_ReturnsOk()
        {
            // First, create a branch to delete
            var createRequest = new
            {
                Name = "Branch To Delete",
                Description = "202 Delete Rd"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/branchs", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>();
            Guid branchId = created.Data.Id;

            var response = await _client.DeleteAsync($"/api/branchs/{branchId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
