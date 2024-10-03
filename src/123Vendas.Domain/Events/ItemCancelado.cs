namespace _123Vendas.Domain.Events;

public class ItemCancelado : IEvent
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public string EventType => nameof(ItemCancelado);
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
