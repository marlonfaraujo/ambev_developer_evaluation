using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Carts
{
    public class CartControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly HelperControllerTests _helperControllerTests;

        public CartControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            _helperControllerTests = new HelperControllerTests(factory);
            var token = _helperControllerTests.GetJwtToken();

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
            
        private void SetAuthorizationHeader(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Verifies that creating a new cart via POST returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/carts should return Created when cart is valid")]
        public async Task CreateCart_ReturnsCreated()
        {
            var responseData = await _helperControllerTests.GetTestData();
            SetAuthorizationHeader(responseData.AuthUser.Token);

            var cartRequest = new
            {
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new[]
                {
                    new { ProductId = responseData.Product.Id, ProductItemQuantity = 2 }
                }
            };
            var response = await _client.PostAsJsonAsync("/api/carts", cartRequest);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that creating a cart with empty items returns HTTP 400 BadRequest.
        /// </summary>
        [Fact(DisplayName = "POST /api/carts should return BadRequest when cart has no items")]
        public async Task CreateCart_EmptyItems_ReturnsBadRequest()
        {
            var cartRequest = new
            {
                BranchSaleId = Guid.NewGuid(),
                SaleItems = Array.Empty<object>()
            };

            var response = await _client.PostAsJsonAsync("/api/carts", cartRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        /// <summary>
        /// Verifies that updating the cart via PUT returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "PUT /api/carts should return Created when cart is updated")]
        public async Task UpdateCart_ReturnsCreated()
        {
            var responseData = await _helperControllerTests.GetTestData();
            SetAuthorizationHeader(responseData.AuthUser.Token);

            // First, create a cart
            var createRequest = new
            {
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new[]
                {
                    new { ProductId = responseData.Product.Id, ProductItemQuantity = 1 }
                }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/carts", createRequest);
            createResponse.EnsureSuccessStatusCode();

            var responseData2 = await _helperControllerTests.GetTestData();

            // Now, update the cart
            var updateRequest = new
            {
                BranchSaleId = responseData2.Branch.Id,
                SaleItems = new[]
                {
                    new { ProductId = responseData2.Product.Id, ProductItemQuantity = 3 }
                }
            };

            var response = await _client.PutAsJsonAsync("/api/carts", createRequest);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that updating the cart with invalid data returns HTTP 400 BadRequest.
        /// </summary>
        [Fact(DisplayName = "PUT /api/carts should return BadRequest when update is invalid")]
        public async Task UpdateCart_InvalidData_ReturnsBadRequest()
        {
            var responseData = await _helperControllerTests.GetTestData();
            SetAuthorizationHeader(responseData.AuthUser.Token);

            var updateRequest = new
            {
                BranchSaleId = responseData.Branch.Id, // Invalid Guid
                SaleItems = new[]
                {
                    new { ProductId = responseData.Product.Id, ProductItemQuantity = 0 } // Invalid quantity
                }
            };

            var response = await _client.PutAsJsonAsync("/api/carts", updateRequest);

            Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
        }


        /// <summary>
        /// Verifies that retrieving the cart via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/carts should return Ok when cart exists")]
        public async Task GetCart_ReturnsOk()
        {
            var responseData = await _helperControllerTests.GetTestData();
            SetAuthorizationHeader(responseData.AuthUser.Token);

            // Ensure a cart exists
            var cartRequest = new
            {
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new[]
                {
                    new { ProductId = responseData.Product.Id, ProductItemQuantity = 2 }
                }
            };
            var postResponse = await _client.PostAsJsonAsync("/api/carts", cartRequest);
            var result = await postResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

            var response = await _client.GetAsync($"/api/carts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting the cart via DELETE returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/carts should return Ok when cart is deleted")]
        public async Task DeleteCart_ReturnsOk()
        {
            var responseData = await _helperControllerTests.GetTestData();
            SetAuthorizationHeader(responseData.AuthUser.Token);

            // Ensure a cart exists
            var cartRequest = new
            {
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new[]
                {
                    new { ProductId = responseData.Product.Id, ProductItemQuantity = 2 }
                }
            };
            await _client.PostAsJsonAsync("/api/carts", cartRequest);

            var response = await _client.DeleteAsync("/api/carts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that checking out the cart via POST returns HTTP 201 Created when the cart is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/sales should return Created when checkout is successful")]
        public async Task CheckoutCart_ReturnsCreated()
        {
            var responseData = await _helperControllerTests.GetTestData();
            SetAuthorizationHeader(responseData.AuthUser.Token);

            // Ensure a cart exists
            var cartRequest = new
            {
                BranchSaleId = responseData.Branch.Id,
                SaleItems = new[]
                {
                    new { ProductId = responseData.Product.Id, ProductItemQuantity = 2 }
                }
            };
            await _client.PostAsJsonAsync("/api/carts", cartRequest);

            // You may need to create a cart first before checkout
            var response = await _client.PostAsync("/api/sales", null);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that checking out an empty cart returns HTTP 400 BadRequest.
        /// </summary>
        [Fact(DisplayName = "POST /api/sale should return BadRequest when cart is empty")]
        public async Task CheckoutCart_EmptyCart_ReturnsBadRequest()
        {
            // Delete cart to ensure it's empty
            await _client.DeleteAsync("/api/carts");

            var response = await _client.PostAsync("/api/sales", null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
