using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartQueryValidator : AbstractValidator<ListCartQuery>
    {
        public ListCartQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
