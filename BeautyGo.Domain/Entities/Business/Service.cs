using BeautyGo.Domain.Entities.Professionals;

namespace BeautyGo.Domain.Entities.Business;

public class Service : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public TimeSpan Duration { get; set; }
    
    public Guid BeautyBusinessId { get; set; }
    public BeautyBusiness BeautyBusiness { get; set; }

    public ICollection<ServicePicture> Pictures { get; set; }
    public ICollection<ProfessionalService> Profissionals { get; set; }
}
