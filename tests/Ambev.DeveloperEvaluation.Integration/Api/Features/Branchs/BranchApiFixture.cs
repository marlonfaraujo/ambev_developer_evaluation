using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Api.Security;
using Ambev.DeveloperEvaluation.WebApi.Common;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Branchs
{
    public class BranchApiFixture : IDisposable
    {
        public HttpClient Client { get; }
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }  
        public string JwtToken { get; }

        public BranchApiFixture()
        {
            var factory = new CustomWebApplicationFactory();
            Client = factory.CreateClient();

            var key = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";
            JwtToken = FakeJwtTokenGenerator.GenerateToken(key, string.Empty, string.Empty);

            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            var response = Client.PostAsJsonAsync("/api/branchs", new Branch { Name = "Name Initial", Description = "Initial" }).Result;
            var branch = response.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>().Result;
            BranchId = (Guid)(branch.Data?.Id);
            BranchName = branch.Data?.Name;
        }

        public Guid GetNewBranchId()
        {
            var response = Client.PostAsJsonAsync("/api/branchs", new Branch { Name = "Initial", Description = "Initial" }).Result;
            var branch = response.Content.ReadFromJsonAsync<ApiResponseWithData<Branch>>().Result;
            return (Guid)(branch.Data?.Id);
        }

        public void Dispose()
        {
            Client.DeleteAsync($"/api/branchs/{BranchId}").Wait();
        }
    }
}
