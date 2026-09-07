using MediatR;

namespace JobTracker.SharedKernel.Domain;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}
