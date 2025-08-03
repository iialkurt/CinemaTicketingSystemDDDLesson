using MediatR;

namespace CinemaTicketingSystem.SharedKernel;

public interface IDomainEvent : INotification;

public interface IIntegrationEvent : INotification;