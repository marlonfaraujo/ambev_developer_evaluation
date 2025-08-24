using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.SimulateSale;
using Ambev.DeveloperEvaluation.ORM.Services;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CheckoutCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly RedisDatabaseService _redisService;

        public CartsController(IMediator mediator, IMapper mapper, RedisDatabaseService redisService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _redisService = redisService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateCartRequestValidator();
            
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var query = _mapper.Map<SimulateSaleQuery>(request);
            var response = await _mediator.Send(query, cancellationToken);

            await _redisService.SetAsync(GetCurrentUserGuid().ToString(), _mapper.Map<CreateCartResponse>(response), TimeSpan.FromHours(1));

            return Created(string.Empty, new ApiResponseWithData<CreateCartResponse>
            {
                Success = true,
                Message = "Cart created successfully",
                Data = _mapper.Map<CreateCartResponse>(response)
            });
        }


        [HttpPost("Checkout")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
        {
            var cartCache = await _redisService.GetAsync<CreateCartResponse>(GetCurrentUserGuid().ToString());

            var validator = new CheckoutCartValidator();
            var validationResult = await validator.ValidateAsync(cartCache, cancellationToken);
            
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var command = _mapper.Map<CreateSaleCommand>(cartCache);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Checkout successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateCartResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateCartRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _redisService.RemoverAsync(GetCurrentUserGuid().ToString());

            var query = _mapper.Map<SimulateSaleQuery>(request);
            var response = await _mediator.Send(query, cancellationToken);

            await _redisService.SetAsync(GetCurrentUserGuid().ToString(), _mapper.Map<UpdateCartResponse>(response), TimeSpan.FromHours(1));

            return Created(string.Empty, new ApiResponseWithData<UpdateCartResponse>
            {
                Success = true,
                Message = "Cart updated successfully",
                Data = _mapper.Map<UpdateCartResponse>(response)
            });
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
        {
            var result = await _redisService.GetAsync<CreateCartResponse>(GetCurrentUserGuid().ToString());

            if (result == null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "cart not found"
                });
            }

            return Ok(new ApiResponseWithData<CreateCartResponse>
            {
                Success = true,
                Message = "cart retrieved successfully",
                Data = _mapper.Map<CreateCartResponse>(result)
            });
        }

        [HttpDelete("")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCart(CancellationToken cancellationToken)
        {
            var result = await _redisService.RemoverAsync(GetCurrentUserGuid().ToString());

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cart deleted successfully"
            });
        }
    }
}
