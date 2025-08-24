using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsRequestValidator : AbstractValidator<ListProductsRequest>
    {
        public ListProductsRequestValidator()
        {
        }
    }
}
