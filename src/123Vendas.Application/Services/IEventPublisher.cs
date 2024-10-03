using _123Vendas.Domain.Events;

public interface IEventPublisher
{
    void Publish(IEvent eventToPublish);
}