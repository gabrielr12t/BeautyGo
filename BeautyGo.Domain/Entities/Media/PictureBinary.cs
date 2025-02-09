namespace BeautyGo.Domain.Entities.Media;

public partial class PictureBinary : BaseEntity
{
    public byte[] BinaryData { get; set; }

    public Guid PictureId { get; set; }

    public Picture Picture { get; set; }
}
