using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Carts
{
    public class CartControllerTests : IClassFixture<CartApiFixture>
    {
        private readonly HelperControllerTests _helperControllerTests;
        private readonly CartApiFixture _cartApiFixture;

        public CartControllerTests(CartApiFixture cartApiFixture)
        {
            _cartApiFixture = cartApiFixture;
        }
            
        /// <summary>
        /// Verifies that creating a new cart via POST returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/carts should return Created when cart is valid")]
        public async Task CreateCart_ReturnsCreated()
        {
            var cartRequest = new
            {
                BranchSaleId = _cartApiFixture.BranchId,
                SaleItems = new[]
                {
                    new { ProductId = _cartApiFixture.ProductId, ProductItemQuantity = 2 }
                }
            };
            var response = await _cartApiFixture.Client.PostAsJsonAsync("/api/carts", cartRequest);
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

            var response = await _cartApiFixture.Client.PostAsJsonAsync("/api/carts", cartRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        /// <summary>
        /// Verifies that updating the cart via PUT returns HTTP 201 Created when the request is valid.
        /// </summary>
        [Fact(DisplayName = "PUT /api/carts should return Created when cart is updated")]
        public async Task UpdateCart_ReturnsCreated()
        {
            //_cartApiFixture.NewCartUserId();

            // Now, update the cart
            var updateRequest = new
            {
                BranchSaleId = _cartApiFixture.BranchId,
                SaleItems = new[]
                {
                    new { ProductId = _cartApiFixture.ProductId, ProductItemQuantity = 3 }
                },
                UserId = _cartApiFixture.CartUserId
            };

            var response = await _cartApiFixture.Client.PutAsJsonAsync("/api/carts", updateRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that updating the cart with invalid data returns HTTP 400 BadRequest.
        /// </summary>
        [Fact(DisplayName = "PUT /api/carts should return BadRequest when update is invalid")]
        public async Task UpdateCart_InvalidData_ReturnsBadRequest()
        {
            var updateRequest = new
            {
                BranchSaleId = _cartApiFixture.BranchId, // Invalid Guid
                SaleItems = new[]
                {
                    new { ProductId = _cartApiFixture.ProductId, ProductItemQuantity = 0 } // Invalid quantity
                }
            };

            var response = await _cartApiFixture.Client.PutAsJsonAsync("/api/carts", updateRequest);

            Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
        }


        /// <summary>
        /// Verifies that retrieving the cart via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/carts should return Ok when cart exists")]
        public async Task GetCart_ReturnsOk()
        {
            var response = await _cartApiFixture.Client.GetAsync($"/api/carts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting the cart via DELETE returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/carts should return Ok when cart is deleted")]
        public async Task DeleteCart_ReturnsOk()
        {
            var response = await _cartApiFixture.Client.DeleteAsync("/api/carts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Verifies that checking out the cart via POST returns HTTP 201 Created when the cart is valid.
        /// </summary>
        [Fact(DisplayName = "POST /api/sales should return Created when checkout is successful")]
        public async Task CheckoutCart_ReturnsCreated()
        {
            _cartApiFixture.NewCartUserId();
            // You may need to create a cart first before checkout
            var response = await _cartApiFixture.Client.PostAsync("/api/sales", null);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that checking out an empty cart returns HTTP 400 BadRequest.
        /// </summary>
        [Fact(DisplayName = "POST /api/sale should return BadRequest when cart is empty")]
        public async Task CheckoutCart_EmptyCart_ReturnsBadRequest()
        {
            // Delete cart to ensure it's empty
            await _cartApiFixture.Client.DeleteAsync("/api/carts");

            var response = await _cartApiFixture.Client.PostAsync("/api/sales", null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
