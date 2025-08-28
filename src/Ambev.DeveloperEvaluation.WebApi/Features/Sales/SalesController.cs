using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.NoSql;
using Ambev.DeveloperEvaluation.WebApi.Adapters;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly RedisDatabaseService _redisService;

        public SalesController(IMediator mediator, IMapper mapper, RedisDatabaseService redisService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _redisService = redisService;
        }

        [HttpPost("", Name=nameof(CreateSale))]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale(CancellationToken cancellationToken)
        {
            var cartCache = await _redisService.GetAsync<CreateCartResponse>(GetCurrentUserGuid().ToString());
            if (cartCache == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Cart not found"
                });
            }

            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(cartCache, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var command = _mapper.Map<CreateSaleCommand>(cartCache);
            var response = await _mediator.Send(new MediatRRequestAdapter<CreateSaleCommand, CreateSaleResult>(command), cancellationToken);

            await _redisService.RemoverAsync(GetCurrentUserGuid().ToString());

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var command = _mapper.Map<UpdateSaleCommand>(request);
            var response = await _mediator.Send(new MediatRRequestAdapter<UpdateSaleCommand, UpdateSaleResult>(command), cancellationToken);

            return Ok(new ApiResponseWithData<UpdateSaleResponse>
            {
                Success = true,
                Message = "Sale updated successfully",
                Data = _mapper.Map<UpdateSaleResponse>(response)
            });
        }

        [HttpPost("{id}/Cancel")]
        [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelSale([FromBody] CancelSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CancelSaleCommand>(request);
            var response = await _mediator.Send(new MediatRRequestAdapter<CancelSaleCommand, CancelSaleResult>(command), cancellationToken);

            return Ok(new ApiResponseWithData<CancelSaleResponse>
            {
                Success = true,
                Message = "Sale canceled successfully",
                Data = _mapper.Map<CancelSaleResponse>(response)
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var query = _mapper.Map<GetSaleQuery>(request.Id);
            var response = await _mediator.Send(new MediatRRequestAdapter<GetSaleQuery, GetSaleResult>(query), cancellationToken);

            return Ok(new ApiResponseWithData<GetSaleResponse>
            {
                Success = true,
                Message = "sale retrieved successfully",
                Data = _mapper.Map<GetSaleResponse>(response)
            });
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ApiResponseWithData<ListSalesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSales([FromQuery] ListSalesRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListSalesRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);            
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var query = _mapper.Map<ListSaleQuery>(request);
            var response = await _mediator.Send(new MediatRRequestAdapter<ListSaleQuery, ListSaleResult>(query), cancellationToken);

            return Ok(new ApiResponseWithData<IEnumerable<ListSalesResponse>>
            {
                Success = true,
                Message = "sale retrieved successfully",
                Data = _mapper.Map<IEnumerable<ListSalesResponse>>(response.Items)
            });

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new DeleteSaleRequest { Id = id };
            var validator = new DeleteSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var command = _mapper.Map<DeleteSaleCommand>(request.Id);
            await _mediator.Send(new MediatRRequestAdapter<DeleteSaleCommand, DeleteSaleResult>(command), cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Sale deleted successfully"
            });
        }
    }
}
