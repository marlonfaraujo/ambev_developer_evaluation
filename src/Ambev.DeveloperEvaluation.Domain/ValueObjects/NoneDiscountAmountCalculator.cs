namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class NoneDiscountAmountCalculator : DiscountAmountCalculator
    {
        public override decimal Calculate(decimal totalProductItemPrice)
        {
            return 0;
        }
    }
}
