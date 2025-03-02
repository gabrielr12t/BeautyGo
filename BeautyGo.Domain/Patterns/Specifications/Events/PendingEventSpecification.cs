using BeautyGo.Domain.Entities.Events;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Events;

public class PendingEventSpecification : Specification<Event>
{ 
    private readonly int _maxAttemps;

    public PendingEventSpecification(int maxAttemps)
    {
        _maxAttemps = maxAttemps;   
    }

    public override Expression<Func<Event, bool>> ToExpression() =>
        @event => (@event.Status == EventStatus.Pendind && @event.Schedule < DateTime.Now && !@event.Executed.HasValue) &&
                  !@event.Deleted.HasValue &&
                  @event.Attempts <= _maxAttemps;
}
