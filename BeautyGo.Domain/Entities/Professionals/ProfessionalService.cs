using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Persons;

namespace BeautyGo.Domain.Entities.Professionals;

public class ProfessionalService : BaseEntity
{
    public Guid ProfessionalId { get; set; }
    public Professional Professional { get; set; }

    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
}
