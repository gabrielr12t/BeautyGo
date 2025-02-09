using BeautyGo.Domain.Entities.Media;

namespace BeautyGo.Domain.Entities.Stores;

public class StorePicture : BaseEntity
{
    public int Order { get; set; }

    public Guid StoreId { get; set; }
    public Store Store { get; set; }

    public Guid PictureId { get; set; }
    public Picture Picture { get; set; }
}
