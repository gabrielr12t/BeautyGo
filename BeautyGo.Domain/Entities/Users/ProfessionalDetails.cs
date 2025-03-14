using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Professionals;

namespace BeautyGo.Domain.Entities.Users;

public class ProfessionalDetails : BaseEntity
{
    public Guid BusinessId { get; set; }
    public Business Business { get; set; }

    public ICollection<ProfessionalService> Services { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }
}
