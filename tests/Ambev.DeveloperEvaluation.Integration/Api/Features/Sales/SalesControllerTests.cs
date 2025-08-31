using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features.Sales
{
    public class SalesControllerTests : IClassFixture<SaleApiFixture>
    {
        private readonly SaleApiFixture _saleApiFixture;

        public SalesControllerTests(SaleApiFixture saleApiFixture)
        {
            _saleApiFixture = saleApiFixture;
            
        }

        /// <summary>
        /// Verifies that creating a new sale via POST returns HTTP 201 Created.
        /// </summary>
        [Fact(DisplayName = "POST /api/sales should return Created when sale is valid")]
        public async Task Post_Sale_ReturnsCreated()
        {
            _saleApiFixture.NewCartId();
            var saleRequest = new
            {
                CartId = _saleApiFixture.CartId
            };
            // Act
            var response = await _saleApiFixture.Client.PostAsJsonAsync("/api/sales", saleRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        /// <summary>
        /// Verifies that retrieving a sale by ID via GET returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "GET /api/sales/{id} should return Ok when sale exists")]
        public async Task Get_Sale_By_Id_ReturnsOk()
        {
            // Act
            var getResponse = await _saleApiFixture.Client.GetAsync($"/api/sales/{_saleApiFixture.SaleId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        /// <summary>
        /// Verifies that updating a sale via PUT returns HTTP 204 No Content.
        /// </summary>
        [Fact(DisplayName = "PUT /api/sales/{id} should return NoContent when update is successful")]
        public async Task Put_Sale_ReturnsNoContent()
        {
            _saleApiFixture.NewSaleId();
            var saleItems = _saleApiFixture.SaleItems;
            foreach (var item in saleItems)
            {
                item.ProductItemQuantity = new Faker().Random.Number(1, 20);
                item.UnitProductItemPrice = new Faker().Random.Decimal(50, 100);
            }

            var updateRequest = new
            {
                Id = _saleApiFixture.SaleId,
                BranchSaleId = _saleApiFixture.BranchId,
                SaleItems = saleItems,
                SaleStatus = SaleStatusEnum.Modified.ToString()
            };

            // Act
            var putResponse = await _saleApiFixture.Client.PutAsJsonAsync($"/api/sales/{_saleApiFixture.SaleId}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }

        /// <summary>
        /// Verifies that deleting a sale via DELETE returns HTTP 200 OK.
        /// </summary>
        [Fact(DisplayName = "DELETE /api/sales/{id} should return OK when delete is successful")]
        public async Task Delete_Sale_ReturnsNoContent()
        {
            _saleApiFixture.NewSaleId();
            var putResponse = await _saleApiFixture.Client.PostAsJsonAsync($"/api/sales/{_saleApiFixture.SaleId}/cancel", new { Id = _saleApiFixture.SaleId });

            // Act
            var deleteResponse = await _saleApiFixture.Client.DeleteAsync($"/api/sales/{_saleApiFixture.SaleId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        /// <summary>
        /// Verifies that listing sales via GET returns HTTP 200 OK and a valid response.
        /// </summary>
        [Fact(DisplayName = "GET /api/sales should return Ok and sales list when query is valid")]
        public async Task Get_Sales_ReturnsOkAndSalesList()
        {
            // Arrange: create query params for ListSalesRequest (using fixture values)
            var query = $"?PageNumber=1&PageSize=5";
            // Act
            var response = await _saleApiFixture.Client.GetAsync($"/api/sales{query}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(content));
        }


        /// <summary>
        /// Verifies that listing sales via GET returns HTTP 200 OK and a valid response.
        /// </summary>
        [Fact(DisplayName = "GET /api/sales should return Ok and sales list when query by sale id")]
        public async Task Get_Sales_ReturnsOkAndSalesListBySaleId()
        {
            _saleApiFixture.NewSaleId();
            // Arrange: create query params for ListSalesRequest (using fixture values)
            var query = $"?PageNumber=1&PageSize=5&SaleId={_saleApiFixture.SaleId}";
            // Act
            var response = await _saleApiFixture.Client.GetAsync($"/api/sales{query}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(content));
        }
    }
}
