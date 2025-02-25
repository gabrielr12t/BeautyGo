using BeautyGo.Domain.Entities.Professionals;

namespace BeautyGo.Domain.Entities.Businesses;

public class Service : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public TimeSpan Duration { get; set; }

    public Guid BeautyBusinessId { get; set; }
    public Business BeautyBusiness { get; set; }

    public ICollection<ServicePicture> Pictures { get; set; }
    public ICollection<ProfessionalService> Profissionals { get; set; }
}
