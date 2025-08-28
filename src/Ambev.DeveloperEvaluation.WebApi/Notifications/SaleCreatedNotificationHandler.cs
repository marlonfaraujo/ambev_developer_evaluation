using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.NoSql;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleCreatedNotificationHandler : INotificationHandler<MediatRDomainNotification<SaleCreatedEvent>>
    {
        private readonly ILogger<SaleCreatedNotificationHandler> _logger;
        private readonly IMongoDbService<ISaleModel> _mongoDbService;
        private readonly IQueryDatabaseService _queryDbService;

        public SaleCreatedNotificationHandler(ILogger<SaleCreatedNotificationHandler> logger, IMongoDbService<ISaleModel> mongoDbService, IQueryDatabaseService queryDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _queryDbService = queryDbService;
        }

        public async Task Handle(MediatRDomainNotification<SaleCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleCreatedEvent Id: {notification.Notification.SaleId}");
            var result = await _queryDbService.GetSaleQueryById<ListSalesQueryResult>(notification.Notification.SaleId);
            if (result == null || !result.Any()) {
                return;
            }
            var document = WithSaleItems(result);
            await _mongoDbService.InsertAsync(document);

            SaleModel WithSaleItems(IEnumerable<ListSalesQueryResult> queryResult)
            {
                if (!queryResult.Any()) return new SaleModel();

                var saleItems = queryResult.Select(x => new SaleItemModel(
                    x.SaleItemId,
                    x.ProductId,
                    x.ProductItemQuantity,
                    x.UnitProductItemPrice,
                    x.DiscountAmount,
                    x.TotalSaleItemPrice,
                    x.TotalWithoutDiscount,
                    x.SaleItemStatus,
                    x.ProductName,
                    x.ProductDescription
                )).ToList();

                return new SaleModel
                {
                    Id = Guid.NewGuid(),
                    SaleId = queryResult.First().SaleId,
                    SaleNumber = queryResult.First().SaleNumber,
                    SaleDate = queryResult.First().SaleDate ?? DateTime.Now,
                    TotalSalePrice = queryResult.First().TotalSalePrice,
                    SaleStatus = queryResult.First().SaleStatus,
                    UserId = queryResult.First().UserId,
                    UserName = queryResult.First().UserName,
                    BranchId = queryResult.First().BranchId,
                    BranchName = queryResult.First().BranchName,
                    BranchDescription = queryResult.First().BranchDescription,
                    SaleItems = saleItems
                };
            }
        }
    }
}
