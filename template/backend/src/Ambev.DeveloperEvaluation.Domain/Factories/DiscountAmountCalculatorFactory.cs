using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Factories
{
    public class DiscountAmountCalculatorFactory
    {
        public static DiscountAmountCalculator Create(int productItemQuantity)
        {
            if (productItemQuantity > 10 && productItemQuantity <= 20) return new MaxDiscountAmountCalculator();
            if (productItemQuantity >= 4) return new MinDiscountAmountCalculator();

            return new NoneDiscountAmountCalculator();   
        }
    }
}
