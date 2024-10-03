namespace _123Vendas.Domain.Events;

public class CompraCriada : IEvent
{
    public Guid SaleId { get; set; }
    public string EventType => nameof(CompraCriada);
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
