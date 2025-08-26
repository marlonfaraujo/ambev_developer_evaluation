using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Branchs
{
    public class BranchControllerTests : IClassFixture<BranchApiFixture>
    {
        private readonly BranchApiFixture _branchApiFixture;

        public BranchControllerTests(BranchApiFixture branchApiFixture)
        {
            _branchApiFixture = branchApiFixture;
        }

        /// <summary>
        /// Verifies that creating a new branch via POST returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/branchs should return Created when branch is valid") ]
        public async Task CreateBranch_ReturnsCreated()
        {
            var branchRequest = new
            {
                Name = "Fake Branch",
                Description = "123 Fake Street"
            };

            var response = await _branchApiFixture.Client.PostAsJsonAsync("/api/branchs", branchRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that updating a branch via PUT returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "PUT /api/branchs should return Created when branch is updated")]
        public async Task UpdateBranch_ReturnsCreated()
        {

            var updateRequest = new
            {
                Id = _branchApiFixture.BranchId,
                Name = "Updated Branch",
                Description = "789 Updated Blvd"
            };

            var response = await _branchApiFixture.Client.PutAsJsonAsync($"/api/branchs/{_branchApiFixture.GetNewBranchId()}", updateRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a branch by ID via GET returns HTTP 200 OK when the branch exists.
        /// </summary>
        [Fact(DisplayName = "GET /api/branchs/{id} should return Ok when branch exists")]
        public async Task GetBranch_ReturnsOk()
        {
            var response = await _branchApiFixture.Client.GetAsync($"/api/branchs/{_branchApiFixture.GetNewBranchId()}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that listing branches via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/branchs should return Ok with branch list")]
        public async Task ListBranchs_ReturnsOk()
        {
            var response = await _branchApiFixture.Client.GetAsync("/api/branchs");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting a branch via DELETE returns HTTP 200 OK when the branch exists.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/branchs/{id} should return Ok when branch is deleted")]
        public async Task DeleteBranch_ReturnsOk()
        {
            var response = await _branchApiFixture.Client.DeleteAsync($"/api/branchs/{_branchApiFixture.BranchId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
