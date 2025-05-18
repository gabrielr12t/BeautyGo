using BeautyGo.Domain.Entities.Appointments;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Appointments;

public class AppointmentByProfessionalIdSpecification : Specification<Appointment>
{
    private readonly Guid _professionalId;

    public AppointmentByProfessionalIdSpecification(Guid professionalId)
    {
        _professionalId = professionalId;
    }

    public override Expression<Func<Appointment, bool>> ToExpression() => 
        p => p.ProfessionalId == _professionalId;
}
