using _123Vendas.Domain.Events;
using Serilog;

public class EventPublisher(ILogger logger) : IEventPublisher
{
    private readonly ILogger _logger = logger;

    public void Publish(IEvent eventToPublish)
    {
        _logger.Information("Event Published: {EventType}, SaleId: {SaleId}, OccurredOn: {OccurredOn}",
            eventToPublish.EventType,
            eventToPublish is CompraCriada compraCriada ? compraCriada.SaleId :
            eventToPublish is CompraAlterada compraAlterada ? compraAlterada.SaleId :
            eventToPublish is CompraCancelada compraCancelada ? compraCancelada.SaleId :
            eventToPublish is ItemCancelado itemCancelado ? itemCancelado.SaleId : Guid.Empty,
            eventToPublish.OccurredOn);
    }
}