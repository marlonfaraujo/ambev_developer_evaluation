using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SimulateSaleService
    {
        private readonly Sale _sale;
        private readonly IEnumerable<Product> _products;

        public SimulateSaleService(Sale sale, IEnumerable<Product> products)
        {
            _sale = sale;
            _products = products;
        }

        public Sale MakePriceSimulation()
        {
            _sale.AddSaleItems(joinProductsWithSaleItems());
            ValidateMaxQuantityProductItems(_sale.SaleItems);

            _sale.TotalSalePrice = new Money(TotalSalePrice());
            return _sale;
        }

        public decimal TotalSalePrice()
        {
            decimal totalSalePrice = _sale.SaleItems.Sum(x => x.CalculateTotalSaleItemPrice());
            return totalSalePrice;
        }

        private List<SaleItem> joinProductsWithSaleItems()
        {
            var result = _sale.SaleItems.Join(_products,
                                        saleItem => saleItem.ProductId,
                                        product => product.Id,
                                        (saleItem, product) => new SaleItem
                                        {
                                            Id = saleItem.Id,
                                            ProductId = product.Id,
                                            UnitProductItemPrice = new Money(product.Price),
                                            ProductItemQuantity = saleItem.ProductItemQuantity
                                        })
                                        .GroupBy(x => x.ProductId)
                                        .Select(g => new SaleItem
                                        {
                                            Id = g.FirstOrDefault().Id,
                                            ProductId = g.Key,
                                            ProductItemQuantity = g.Sum(x => x.ProductItemQuantity),
                                            UnitProductItemPrice = g.FirstOrDefault().UnitProductItemPrice
                                        })
                                        .ToList();

            return result;
        }

        public bool ValidateMaxQuantityProductItems(IEnumerable<SaleItem> saleItems)
        {
            var spec = new MaxQuantityProductItemsSpecification();
            if (!spec.IsSatisfiedBy(saleItems))
            {
                throw new MaxQuantityProductItemsException($"The maximum quantity of product items is {MaxQuantityProductItemsSpecification.MAX_ITEMS_PER_PRODUCT}.");
            }

            return false;
        }
    }
}
