namespace CinemaTicketingSystem.Application.Contracts
{
    public interface IIntegrationEventBus
    {

        Task PublishAsync<T>(T integrationEvent) where T : class;
    }
}
