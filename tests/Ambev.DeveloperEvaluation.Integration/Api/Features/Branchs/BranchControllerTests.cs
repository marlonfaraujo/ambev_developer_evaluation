using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Branchs
{
    public class BranchControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BranchControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            var token = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);
            
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
                Address = "123 Fake Street",
                City = "Faketown",
                State = "FS",
                ZipCode = "12345-678"
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
                Address = "456 Update Ave",
                City = "Updatetown",
                State = "UP",
                ZipCode = "98765-432"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/branchs", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            Guid branchId = created.data.id;

            var updateRequest = new
            {
                Id = branchId,
                Name = "Updated Branch",
                Address = "789 Updated Blvd",
                City = "Updated City",
                State = "UC",
                ZipCode = "11111-222"
            };

            var response = await _client.PutAsJsonAsync("/api/branchs", updateRequest);

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
                Address = "101 Get St",
                City = "Getville",
                State = "GT",
                ZipCode = "22222-333"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/branchs", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            Guid branchId = created.data.id;

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
                Address = "202 Delete Rd",
                City = "Deleteburg",
                State = "DL",
                ZipCode = "44444-555"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/branchs", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            Guid branchId = created.data.id;

            var response = await _client.DeleteAsync($"/api/branchs/{branchId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
