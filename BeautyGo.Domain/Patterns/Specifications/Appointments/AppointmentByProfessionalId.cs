using BeautyGo.Domain.Entities.Appointments;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Appointments;

public class AppointmentByProfessionalId : Specification<Appointment>
{
    private readonly Guid _professionalId;

    public AppointmentByProfessionalId(Guid professionalId)
    {
        _professionalId = professionalId;
    }

    public override Expression<Func<Appointment, bool>> ToExpression() => 
        p => p.ProfessionalId == _professionalId;
}
