namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class MinDiscountAmountCalculator : DiscountAmountCalculator
    {
        private const decimal DISCOUNT_AMOUNT = 0.1m;

        public override decimal Calculate(decimal totalProductItemPrice)
        {
            return totalProductItemPrice * DISCOUNT_AMOUNT;
        }
    
    }
}
