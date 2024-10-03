namespace _123Vendas.Domain.Events;

public interface IEvent
{
    string EventType { get; }
    DateTime OccurredOn { get; }
}
