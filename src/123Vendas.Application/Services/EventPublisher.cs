using _123Vendas.Domain.Events;
using Microsoft.Extensions.Logging;

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ILogger<EventPublisher> logger)
    {
        _logger = logger;
    }

    public void Publish(IEvent eventToPublish)
    {
        _logger.LogInformation("Event Published: {EventType}, SaleId: {SaleId}, OccurredOn: {OccurredOn}",
            eventToPublish.EventType,
            eventToPublish is CompraCriada compraCriada ? compraCriada.SaleId :
            eventToPublish is CompraAlterada compraAlterada ? compraAlterada.SaleId :
            eventToPublish is CompraCancelada compraCancelada ? compraCancelada.SaleId :
            eventToPublish is ItemCancelado itemCancelado ? itemCancelado.SaleId : Guid.Empty,
            eventToPublish.OccurredOn);
    }
}