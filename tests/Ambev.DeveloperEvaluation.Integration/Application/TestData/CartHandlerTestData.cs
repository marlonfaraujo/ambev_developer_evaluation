using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Application.TestData
{
    public static class CartHandlerTestData
    {
        public static Cart GetCart(Guid userId, Branch branch, Product product, decimal totalSalePrice)
        {
            using var context = new DatabaseInMemory().Context;
            var repository = new CartRepository(context);

            var cart = new Cart(userId, new Money(totalSalePrice), branch.Id, "Branch");
            cart.CartItems.Add(
                new CartItem(
                    product.Id, 
                    product.Name, 
                    10, 
                    product.Price, 
                    new Money(0), 
                    new Money((product.Price.Value * 10)), 
                    new Money((product.Price.Value * 10))
                )
            );

            var result = repository.CreateAsync(cart).Result;
            return result;
        }
    }
}
