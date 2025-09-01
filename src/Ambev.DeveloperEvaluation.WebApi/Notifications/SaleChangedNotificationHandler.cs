using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.NoSql;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Notifications
{
    public class SaleChangedNotificationHandler : INotificationHandler<MediatRDomainNotification<SaleChangedEvent>>
    {
        private readonly ILogger<SaleChangedNotificationHandler> _logger;
        private readonly IMongoDbService<ISaleModel> _mongoDbService;
        private readonly IQueryDatabaseService _queryDbService;

        public SaleChangedNotificationHandler(ILogger<SaleChangedNotificationHandler> logger, IMongoDbService<ISaleModel> mongoDbService, IQueryDatabaseService queryDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;   
            _queryDbService = queryDbService;
        }

        public async Task Handle(MediatRDomainNotification<SaleChangedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Event log SaleChangedEvent Id: {notification.Notification.SaleId}");
            var result = await _queryDbService.GetSaleQueryById<ListSalesQueryResult>(notification.Notification.SaleId);
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
