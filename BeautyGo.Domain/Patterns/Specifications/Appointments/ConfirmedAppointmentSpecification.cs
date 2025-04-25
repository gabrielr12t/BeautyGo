using BeautyGo.Domain.Entities.Appointments;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Appointments;

public class ConfirmedAppointmentSpecification : Specification<Appointment>
{
    public override Expression<Func<Appointment, bool>> ToExpression() =>
        p => p.IsConfirmed();
}
