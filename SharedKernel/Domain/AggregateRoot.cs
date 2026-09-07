namespace JobTracker.SharedKernel.Domain;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();
        return events;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
