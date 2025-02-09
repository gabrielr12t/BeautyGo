using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Business;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Domain.Entities.Professionals;

public class Professional : User
{
    public Professional()
    {
        Services = new List<ProfessionalService>();
        Appointments = new List<Appointment>();
        ProfessionalAvailabilities = new List<ProfessionalAvailability>();
    }

    public Guid BusinessId { get; set; }
    public BeautyBusiness Business { get; set; }

    public ICollection<ProfessionalService> Services { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }
}
