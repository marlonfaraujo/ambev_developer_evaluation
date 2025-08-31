using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.NoSql;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleItemCancelledNotificationHandler : INotificationHandler<MediatRDomainNotification<SaleItemCancelledEvent>>
    {
        private readonly ILogger<SaleItemCancelledNotificationHandler> _logger;
        private readonly IMongoDbService<ISaleModel> _mongoDbService;
        private readonly IQueryDatabaseService _queryDbService;

        public SaleItemCancelledNotificationHandler(ILogger<SaleItemCancelledNotificationHandler> logger, IMongoDbService<ISaleModel> mongoDbService, IQueryDatabaseService queryDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;   
            _queryDbService = queryDbService;
        }

        public async Task Handle(MediatRDomainNotification<SaleItemCancelledEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleItemCancelledEvent Id: {notification.Notification.SaleItemId}");
            var result = await _queryDbService.GetSaleQueryBySaleItemId<ListSalesQueryResult>(notification.Notification.SaleItemId);
            if (result == null || !result.Any())
            {
                return;
            }
            var document = SaleModel.WithSaleItems(result);
            var filter = new Dictionary<string, object>
            {
                { "SaleId", document.SaleId }
            };
            await _mongoDbService.UpdateByParamsAsync(filter, document);
        }
    }
}
