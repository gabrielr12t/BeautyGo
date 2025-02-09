using BeautyGo.Domain.Entities.Media;

namespace BeautyGo.Domain.Entities.Business;

public class BeautyBusinessPicture : BaseEntity
{
    public int Order { get; set; }

    public Guid BeautyBusinessId { get; set; }
    public BeautyBusiness BeautyBusiness { get; set; }

    public Guid PictureId { get; set; }
    public Picture Picture { get; set; }
}
