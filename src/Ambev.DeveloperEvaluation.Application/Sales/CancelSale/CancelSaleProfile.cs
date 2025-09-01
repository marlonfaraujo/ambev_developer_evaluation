using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleProfile : Profile
    {
        public CancelSaleProfile()
        {
            CreateMap<CancelSaleCommand, Sale>();
            CreateMap<Sale, CancelSaleResult>()
                .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
                .ForCtorParam("SaleNumber", opt => opt.MapFrom(x => x.SaleNumber))
                .ForCtorParam("TotalSalePrice", opt => opt.MapFrom(x => (decimal)x.TotalSalePrice.Value))
                .ForCtorParam("SaleStatus", opt => opt.MapFrom(x => x.SaleStatus))
                .ForCtorParam("SaleItems", opt => opt.MapFrom(x => x.SaleItems));

            CreateMap<SaleItem, CancelSaleItemResult>()
                .ForCtorParam("Id", opt => opt.MapFrom(x => x.Id))
                .ForCtorParam("ProductId", opt => opt.MapFrom(x => x.ProductId))
                .ForCtorParam("ProductItemQuantity", opt => opt.MapFrom(x => x.ProductItemQuantity))
                .ForCtorParam("UnitProductItemPrice", opt => opt.MapFrom(x => (decimal)x.UnitProductItemPrice.Value))
                .ForCtorParam("DiscountAmount", opt => opt.MapFrom(x => (decimal)x.DiscountAmount.Value))
                .ForCtorParam("TotalSaleItemPrice", opt => opt.MapFrom(x => (decimal)x.TotalSaleItemPrice.Value))
                .ForCtorParam("TotalWithoutDiscount", opt => opt.MapFrom(x => (decimal)x.TotalWithoutDiscount.Value))
                .ForCtorParam("SaleItemStatus", opt => opt.MapFrom(x => x.SaleItemStatus));
        }
    }
}
