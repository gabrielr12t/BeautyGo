namespace BeautyGo.Domain.Entities.Media;

public class Picture : BaseEntity
{
    public string MimeType { get; set; }

    public string SeoFilename { get; set; }

    public string AltAttribute { get; set; }

    public string TitleAttribute { get; set; }

    public bool IsNew { get; set; }

    public string VirtualPath { get; set; }

    private ICollection<PictureBinary> _pictureBinaries;
    public ICollection<PictureBinary> PictureBinaries
    {
        get { return _pictureBinaries ?? new List<PictureBinary>(); }
        set { _pictureBinaries = value; }
    }
}
