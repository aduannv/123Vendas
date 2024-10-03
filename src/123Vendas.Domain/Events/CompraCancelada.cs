namespace _123Vendas.Domain.Events;

public class CompraCancelada : IEvent
{
    public Guid SaleId { get; set; }
    public string EventType => nameof(CompraCancelada);
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
