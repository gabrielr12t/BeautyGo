using BeautyGo.Domain.Entities.Media;

namespace BeautyGo.Domain.Entities.Business;

public class ServicePicture : BaseEntity
{
    public int Order { get; set; }

    public Guid ServiceId { get; set; }
    public Service Service { get; set; }

    public Guid PictureId { get; set; }
    public Picture Picture { get; set; }
}
