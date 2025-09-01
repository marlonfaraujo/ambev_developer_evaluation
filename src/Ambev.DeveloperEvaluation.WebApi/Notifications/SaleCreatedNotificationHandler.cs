using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Services;
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
            var document = SaleModel.Create(result);
            await _mongoDbService.InsertAsync(document);
        }
    }
}
