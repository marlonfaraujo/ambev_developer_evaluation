namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class MaxDiscountAmountCalculator : DiscountAmountCalculator
    {
        private const decimal DISCOUNT_AMOUNT = 0.2m;

        public override decimal Calculate(decimal totalProductItemPrice)
        {
            return totalProductItemPrice * DISCOUNT_AMOUNT;
        }
    }
}
