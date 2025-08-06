#region

using MediatR;

#endregion

namespace CinemaTicketingSystem.SharedKernel;

public interface IDomainEvent : INotification;

public interface IIntegrationEvent : INotification;