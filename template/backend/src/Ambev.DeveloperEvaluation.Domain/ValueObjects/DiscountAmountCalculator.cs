namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public abstract class DiscountAmountCalculator
    {
        public abstract decimal Calculate(decimal totalProductItemPrice);
    }
}
