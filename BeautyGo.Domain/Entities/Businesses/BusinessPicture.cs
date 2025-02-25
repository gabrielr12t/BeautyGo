using BeautyGo.Domain.Entities.Media;

namespace BeautyGo.Domain.Entities.Businesses;

public class BusinessPicture : BaseEntity
{
    public int Order { get; set; }

    public Guid BeautyBusinessId { get; set; }
    public Business BeautyBusiness { get; set; }

    public Guid PictureId { get; set; }
    public Picture Picture { get; set; }
}
