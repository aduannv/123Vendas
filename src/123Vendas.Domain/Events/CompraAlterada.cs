namespace _123Vendas.Domain.Events;

public class CompraAlterada : IEvent
{
    public Guid SaleId { get; set; }
    public string EventType => nameof(CompraAlterada);
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
