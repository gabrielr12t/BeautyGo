using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Media;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Entities.Media;
using BeautyGo.Domain.Extensions;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace BeautyGo.Infrastructure.Services.Media;

public partial class PictureService : IPictureService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBeautyGoFileProvider _fileProvider;
    private readonly IEFBaseRepository<Picture> _pictureRepository;
    private readonly IEFBaseRepository<PictureBinary> _pictureBinaryRepository;
    private readonly IWebHelper _webHelper;
    private readonly MediaSettings _mediaSettings;

    #endregion

    #region Ctor

    public PictureService(
        IHttpContextAccessor httpContextAccessor,
        IBeautyGoFileProvider fileProvider,
        IEFBaseRepository<Picture> pictureRepository,
        IEFBaseRepository<PictureBinary> pictureBinaryRepository,
        IWebHelper webHelper,
        AppSettings appSettings,
        IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _fileProvider = fileProvider;
        _pictureRepository = pictureRepository;
        _pictureBinaryRepository = pictureBinaryRepository;
        _webHelper = webHelper;
        _mediaSettings = appSettings.Get<MediaSettings>();
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Utilities 

    protected virtual async Task<byte[]> LoadPictureFromFileAsync(Guid pictureId, string mimeType)
    {
        var lastPart = await GetFileExtensionFromMimeTypeAsync(mimeType);
        var fileName = $"{pictureId:0000000}_0.{lastPart}";
        var filePath = await GetPictureLocalPathAsync(fileName);

        return await _fileProvider.ReadAllBytesAsync(filePath);
    }

    protected virtual async Task SavePictureInFileAsync(Guid pictureId, byte[] pictureBinary, string mimeType)
    {
        var lastPart = await GetFileExtensionFromMimeTypeAsync(mimeType);
        var fileName = $"{pictureId:0000000}_0.{lastPart}";
        await _fileProvider.WriteAllBytesAsync(await GetPictureLocalPathAsync(fileName), pictureBinary);
    }

    protected virtual async Task DeletePictureOnFileSystemAsync(Picture picture)
    {
        if (picture == null)
            throw new ArgumentNullException(nameof(picture));

        var lastPart = await GetFileExtensionFromMimeTypeAsync(picture.MimeType);
        var fileName = $"{picture.Id.ToString():0000000}_0.{lastPart}";
        var filePath = await GetPictureLocalPathAsync(fileName);
        _fileProvider.DeleteFile(filePath);
    }

    protected virtual async Task DeletePictureThumbsAsync(Picture picture)
    {
        var filter = $"{picture.Id.ToString():0000000}*.*";
        var currentFiles = _fileProvider.GetFiles(_fileProvider.GetAbsolutePath(BeautyGoMediaDefaults.ImageThumbsPath), filter, false);
        foreach (var currentFileName in currentFiles)
        {
            var thumbFilePath = await GetThumbLocalPathAsync(currentFileName);
            _fileProvider.DeleteFile(thumbFilePath);
        }
    }

    protected virtual Task<string> GetThumbLocalPathAsync(string thumbFileName)
    {
        var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(BeautyGoMediaDefaults.ImageThumbsPath);

        if (_mediaSettings.MultipleThumbDirectories)
        {
            //get the first two letters of the file name
            var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
            if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > BeautyGoMediaDefaults.MultipleThumbDirectoriesLength)
            {
                var subDirectoryName = fileNameWithoutExtension[0..BeautyGoMediaDefaults.MultipleThumbDirectoriesLength];
                thumbsDirectoryPath = _fileProvider.GetAbsolutePath(BeautyGoMediaDefaults.ImageThumbsPath, subDirectoryName);
                _fileProvider.CreateDirectory(thumbsDirectoryPath);
            }
        }

        var thumbFilePath = _fileProvider.Combine(thumbsDirectoryPath, thumbFileName);
        return Task.FromResult(thumbFilePath);
    }

    protected virtual Task<string> GetImagesPathUrlAsync(string businessLocation = null)
    {
        var pathBase = _httpContextAccessor.HttpContext.Request?.PathBase.Value ?? string.Empty;
        var imagesPathUrl = _mediaSettings.UseAbsoluteImagePath ? businessLocation : $"{pathBase}/";
        imagesPathUrl = string.IsNullOrEmpty(imagesPathUrl) ? _webHelper.GetBusinessLocation() : imagesPathUrl;
        imagesPathUrl += "images/";

        return Task.FromResult(imagesPathUrl);
    }

    protected virtual async Task<string> GetThumbUrlAsync(string thumbFileName, string businessLocation = null)
    {
        var url = await GetImagesPathUrlAsync(businessLocation) + "thumbs/";

        if (_mediaSettings.MultipleThumbDirectories)
        {
            //get the first two letters of the file name
            var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
            if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > BeautyGoMediaDefaults.MultipleThumbDirectoriesLength)
            {
                var subDirectoryName = fileNameWithoutExtension[0..BeautyGoMediaDefaults.MultipleThumbDirectoriesLength];
                url = url + subDirectoryName + "/";
            }
        }

        url += thumbFileName;
        return url;
    }

    protected virtual Task<string> GetPictureLocalPathAsync(string fileName)
    {
        return Task.FromResult(_fileProvider.GetAbsolutePath("images", fileName));
    }

    protected virtual async Task<byte[]> LoadPictureBinaryAsync(Picture picture, bool fromDb)
    {
        if (picture == null)
            throw new ArgumentNullException(nameof(picture));

        var result = fromDb
            ? (await GetPictureBinaryByPictureIdAsync(picture.Id))?.BinaryData ?? Array.Empty<byte>()
            : await LoadPictureFromFileAsync(picture.Id, picture.MimeType);

        return result;
    }

    protected virtual Task<bool> GeneratedThumbExistsAsync(string thumbFilePath, string thumbFileName)
    {
        return Task.FromResult(_fileProvider.FileExists(thumbFilePath));
    }

    protected virtual async Task SaveThumbAsync(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
    {
        //ensure \thumb directory exists
        var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(BeautyGoMediaDefaults.ImageThumbsPath);
        _fileProvider.CreateDirectory(thumbsDirectoryPath);

        //save
        await _fileProvider.WriteAllBytesAsync(thumbFilePath, binary);
    }

    protected virtual async Task<PictureBinary> UpdatePictureBinaryAsync(Picture picture, byte[] binaryData, bool publishEvent = true)
    {
        if (picture == null)
            throw new ArgumentNullException(nameof(picture));

        var pictureBinary = await GetPictureBinaryByPictureIdAsync(picture.Id);

        var isNew = pictureBinary == null;

        if (isNew)
            pictureBinary = new PictureBinary
            {
                PictureId = picture.Id
            };

        pictureBinary.BinaryData = binaryData;

        if (isNew)
            await _pictureBinaryRepository.InsertAsync(pictureBinary);
        else
            await _pictureBinaryRepository.UpdateAsync(pictureBinary);

        return pictureBinary;
    }

    protected virtual SKEncodedImageFormat GetImageFormatByMimeType(string mimeType)
    {
        var format = SKEncodedImageFormat.Jpeg;
        if (string.IsNullOrEmpty(mimeType))
            return format;

        var parts = mimeType.ToLower().Split('/');
        var lastPart = parts[^1];

        switch (lastPart)
        {
            case "webp":
                format = SKEncodedImageFormat.Webp;
                break;
            case "png":
            case "gif":
            case "bmp":
            case "x-icon":
                format = SKEncodedImageFormat.Png;
                break;
            default:
                break;
        }

        return format;
    }

    protected virtual string GetMimeTypeFromFileName(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        if (!provider.TryGetContentType(fileName, out contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    protected virtual byte[] ImageResize(SKBitmap image, SKEncodedImageFormat format, int targetSize)
    {
        if (image == null)
            throw new ArgumentNullException("Image is null");

        float width, height;
        if (image.Height > image.Width)
        {
            // portrait
            width = image.Width * (targetSize / (float)image.Height);
            height = targetSize;
        }
        else
        {
            // landscape or square
            width = targetSize;
            height = image.Height * (targetSize / (float)image.Width);
        }

        if ((int)width == 0 || (int)height == 0)
        {
            width = image.Width;
            height = image.Height;
        }
        try
        {
            using var resizedBitmap = image.Resize(new SKImageInfo((int)width, (int)height), SKFilterQuality.Medium);
            using var cropImage = SKImage.FromBitmap(resizedBitmap);

            //In order to exclude saving pictures in low quality at the time of installation, we will set the value of this parameter to 80 (as by default)
            return cropImage.Encode(format, _mediaSettings.DefaultImageQuality > 0 ? _mediaSettings.DefaultImageQuality : 80).ToArray();
        }
        catch
        {
            return image.Bytes;
        }

    }

    #endregion

    #region Getting picture local path/URL methods

    public async Task<byte[]> GetDownloadBitsAsync(IFormFile file)
    {
        await using var fileStream = file.OpenReadStream();
        await using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        var fileBytes = ms.ToArray();

        return fileBytes;
    }

    public virtual Task<string> GetFileExtensionFromMimeTypeAsync(string mimeType)
    {
        if (mimeType == null)
            return Task.FromResult<string>(null);

        var parts = mimeType.Split('/');
        var lastPart = parts[^1];
        switch (lastPart)
        {
            case "pjpeg":
                lastPart = "jpg";
                break;
            case "x-png":
                lastPart = "png";
                break;
            case "x-icon":
                lastPart = "ico";
                break;
            default:
                break;
        }

        return Task.FromResult(lastPart);
    }

    public virtual async Task<byte[]> LoadPictureBinaryAsync(Picture picture)
    {
        return await LoadPictureBinaryAsync(picture, true);
    }

    public virtual async Task<string> GetDefaultPictureUrlAsync(int targetSize = 0,
        PictureType defaultPictureType = PictureType.Entity,
        string businessLocation = null)
    {
        var defaultImageFileName = defaultPictureType switch
        {
            PictureType.Avatar => BeautyGoMediaDefaults.DefaultAvatarFileName,
            _ => BeautyGoMediaDefaults.DefaultImageFileName,
        };
        var filePath = await GetPictureLocalPathAsync(defaultImageFileName);
        if (!_fileProvider.FileExists(filePath))
            return string.Empty;

        if (targetSize == 0)
            return await GetImagesPathUrlAsync(businessLocation) + defaultImageFileName;

        var fileExtension = _fileProvider.GetFileExtension(filePath);
        var thumbFileName = $"{_fileProvider.GetFileNameWithoutExtension(filePath)}_{targetSize}{fileExtension}";
        var thumbFilePath = await GetThumbLocalPathAsync(thumbFileName);
        if (!await GeneratedThumbExistsAsync(thumbFilePath, thumbFileName))
        {
            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            //you should be very careful, mutexes cannot be used in with the await operation
            //we can't use semaphore here, because it produces PlatformNotSupportedException exception on UNIX based systems
            using var mutex = new Mutex(false, thumbFileName);
            mutex.WaitOne();
            try
            {
                using var image = SKBitmap.Decode(filePath);
                var codec = SKCodec.Create(filePath);
                var format = codec.EncodedFormat;
                var pictureBinary = ImageResize(image, format, targetSize);
                var mimeType = GetMimeTypeFromFileName(thumbFileName);
                SaveThumbAsync(thumbFilePath, thumbFileName, mimeType, pictureBinary).Wait();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        return await GetThumbUrlAsync(thumbFileName, businessLocation);
    }

    public virtual async Task<string> GetDefaultPicturePathAsync(int targetSize = 0,
       PictureType defaultPictureType = PictureType.Entity,
       string businessLocation = null)
    {
        var defaultImageFileName = defaultPictureType switch
        {
            PictureType.Avatar => BeautyGoMediaDefaults.DefaultAvatarFileName,
            _ => BeautyGoMediaDefaults.DefaultImageFileName,
        };

        return await GetPictureLocalPathAsync(defaultImageFileName);
    }

    public virtual async Task<string> GetPictureUrlAsync(Guid pictureId,
        int targetSize = 0,
        bool showDefaultPicture = true,
        string businessLocation = null,
        PictureType defaultPictureType = PictureType.Entity)
    {
        var picture = await GetPictureByIdAsync(pictureId);
        return (await GetPictureUrlAsync(picture, targetSize, showDefaultPicture, businessLocation, defaultPictureType)).Url;
    }

    public virtual async Task<(string Url, Picture Picture)> GetPictureUrlAsync(Picture picture,
        int targetSize = 0,
        bool showDefaultPicture = true,
        string businessLocation = null,
        PictureType defaultPictureType = PictureType.Entity)
    {
        if (picture == null)
            return showDefaultPicture ? (await GetDefaultPictureUrlAsync(targetSize, defaultPictureType, businessLocation), null) : (string.Empty, (Picture)null);

        byte[] pictureBinary = null;
        if (picture.IsNew)
        {
            await DeletePictureThumbsAsync(picture);
            pictureBinary = await LoadPictureBinaryAsync(picture);

            if ((pictureBinary?.Length ?? 0) == 0)
                return showDefaultPicture ? (await GetDefaultPictureUrlAsync(targetSize, defaultPictureType, businessLocation), picture) : (string.Empty, picture);

            //we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown
            picture = await UpdatePictureAsync(picture.Id,
                pictureBinary,
                picture.MimeType,
                picture.SeoFilename,
                picture.AltAttribute,
                picture.TitleAttribute,
                false,
                false);
        }

        var seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

        var lastPart = await GetFileExtensionFromMimeTypeAsync(picture.MimeType);
        string thumbFileName;
        if (targetSize == 0)
        {
            thumbFileName = !string.IsNullOrEmpty(seoFileName)
                ? $"{picture.Id:0000000}_{seoFileName}.{lastPart}"
                : $"{picture.Id:0000000}.{lastPart}";

            var thumbFilePath = await GetThumbLocalPathAsync(thumbFileName);
            if (await GeneratedThumbExistsAsync(thumbFilePath, thumbFileName))
                return (await GetThumbUrlAsync(thumbFileName, businessLocation), picture);

            pictureBinary ??= await LoadPictureBinaryAsync(picture);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            //you should be very careful, mutexes cannot be used in with the await operation
            //we can't use semaphore here, because it produces PlatformNotSupportedException exception on UNIX based systems
            using var mutex = new Mutex(false, thumbFileName);
            mutex.WaitOne();
            try
            {
                SaveThumbAsync(thumbFilePath, thumbFileName, picture.MimeType, pictureBinary).Wait();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        else
        {
            thumbFileName = !string.IsNullOrEmpty(seoFileName)
                ? $"{picture.Id.ToString():0000000}_{seoFileName}_{targetSize}.{lastPart}"
                : $"{picture.Id.ToString():0000000}_{targetSize}.{lastPart}";

            var thumbFilePath = await GetThumbLocalPathAsync(thumbFileName);
            if (await GeneratedThumbExistsAsync(thumbFilePath, thumbFileName))
                return (await GetThumbUrlAsync(thumbFileName, businessLocation), picture);

            pictureBinary ??= await LoadPictureBinaryAsync(picture);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            //you should be very careful, mutexes cannot be used in with the await operation
            //we can't use semaphore here, because it produces PlatformNotSupportedException exception on UNIX based systems
            using var mutex = new Mutex(false, thumbFileName);
            mutex.WaitOne();
            try
            {
                if (pictureBinary != null)
                {
                    try
                    {
                        using var image = SKBitmap.Decode(pictureBinary);
                        var format = GetImageFormatByMimeType(picture.MimeType);
                        pictureBinary = ImageResize(image, format, targetSize);
                    }
                    catch
                    {
                    }
                }

                SaveThumbAsync(thumbFilePath, thumbFileName, picture.MimeType, pictureBinary).Wait();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        return (await GetThumbUrlAsync(thumbFileName, businessLocation), picture);
    }

    public virtual async Task<string> GetThumbLocalPathAsync(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
    {
        var (url, _) = await GetPictureUrlAsync(picture, targetSize, showDefaultPicture);
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        return await GetThumbLocalPathAsync(_fileProvider.GetFileName(url));
    }

    #endregion

    #region CRUD methods

    public virtual async Task<Picture> GetPictureByIdAsync(Guid pictureId)
    {
        return await _pictureRepository.GetByIdAsync(pictureId);
    }

    public virtual async Task DeletePictureAsync(Picture picture)
    {
        if (picture == null)
            throw new ArgumentNullException(nameof(picture));

        //delete thumbs
        await DeletePictureThumbsAsync(picture);

        await DeletePictureOnFileSystemAsync(picture);

        //delete from database
        await _pictureRepository.RemoveAsync(picture);

        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task<IPagedList<Picture>> GetPicturesAsync(string virtualPath = "", int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var query = _pictureRepository.QueryNoTracking();

        if (!string.IsNullOrEmpty(virtualPath))
            query = virtualPath.EndsWith('/') ? query.Where(p => p.VirtualPath.StartsWith(virtualPath) || p.VirtualPath == virtualPath.TrimEnd('/')) : query.Where(p => p.VirtualPath == virtualPath);

        query = query.OrderByDescending(p => p.Id);

        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    public virtual async Task<Picture> InsertPictureAsync(byte[] pictureBinary, string mimeType, string seoFilename,
        string altAttribute = null, string titleAttribute = null,
        bool isNew = true, bool validateBinary = true, bool publishEvent = true)
    {
        mimeType = CommonHelper.EnsureNotNull(mimeType);
        mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

        seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

        if (validateBinary)
            pictureBinary = await ValidatePictureAsync(pictureBinary, mimeType);

        var picture = new Picture
        {
            MimeType = mimeType,
            SeoFilename = seoFilename,
            AltAttribute = altAttribute,
            TitleAttribute = titleAttribute,
            IsNew = isNew
        };

        await _pictureRepository.InsertAsync(picture);
        await _unitOfWork.SaveChangesAsync();

        await UpdatePictureBinaryAsync(picture, true ? pictureBinary : Array.Empty<byte>(), publishEvent: publishEvent);

        await SavePictureInFileAsync(picture.Id, pictureBinary, mimeType);

        return picture;
    }

    public virtual async Task<Picture> InsertPictureAsync(IFormFile formFile, string defaultFileName = "", string virtualPath = "")
    {
        var imgExt = new List<string>
            {
                ".bmp",
                ".gif",
                ".webp",
                ".jpeg",
                ".jpg",
                ".jpe",
                ".jfif",
                ".pjpeg",
                ".pjp",
                ".png",
                ".tiff",
                ".tif"
            } as IReadOnlyCollection<string>;

        var fileName = formFile.FileName;
        if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(defaultFileName))
            fileName = defaultFileName;

        //remove path (passed in IE)
        fileName = _fileProvider.GetFileName(fileName);

        var contentType = formFile.ContentType;

        var fileExtension = _fileProvider.GetFileExtension(fileName);
        if (!string.IsNullOrEmpty(fileExtension))
            fileExtension = fileExtension.ToLowerInvariant();

        if (imgExt.All(ext => !ext.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase)))
            return null;

        //contentType is not always available 
        //that's why we manually update it here
        //http://www.sfsu.edu/training/mimetype.htm
        if (string.IsNullOrEmpty(contentType))
        {
            switch (fileExtension)
            {
                case ".bmp":
                    contentType = MimeTypes.ImageBmp;
                    break;
                case ".gif":
                    contentType = MimeTypes.ImageGif;
                    break;
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                case ".jfif":
                case ".pjpeg":
                case ".pjp":
                    contentType = MimeTypes.ImageJpeg;
                    break;
                case ".webp":
                    contentType = MimeTypes.ImageWebp;
                    break;
                case ".png":
                    contentType = MimeTypes.ImagePng;
                    break;
                case ".tiff":
                case ".tif":
                    contentType = MimeTypes.ImageTiff;
                    break;
                default:
                    break;
            }
        }

        var picture = await InsertPictureAsync(await GetDownloadBitsAsync(formFile), contentType, _fileProvider.GetFileNameWithoutExtension(fileName));

        if (string.IsNullOrEmpty(virtualPath))
            return picture;

        picture.VirtualPath = _fileProvider.GetVirtualPath(virtualPath);
        await UpdatePictureAsync(picture);

        return picture;
    }

    public virtual async Task<Picture> UpdatePictureAsync(Guid pictureId, byte[] pictureBinary, string mimeType,
        string seoFilename, string altAttribute = null, string titleAttribute = null,
        bool isNew = true, bool validateBinary = true)
    {
        mimeType = CommonHelper.EnsureNotNull(mimeType);
        mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

        seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

        if (validateBinary)
            pictureBinary = await ValidatePictureAsync(pictureBinary, mimeType);

        var picture = await GetPictureByIdAsync(pictureId);
        if (picture == null)
            return null;

        //delete old thumbs if a picture has been changed
        if (seoFilename != picture.SeoFilename)
            await DeletePictureThumbsAsync(picture);

        picture.MimeType = mimeType;
        picture.SeoFilename = seoFilename;
        picture.AltAttribute = altAttribute;
        picture.TitleAttribute = titleAttribute;
        picture.IsNew = isNew;

        await _pictureRepository.UpdateAsync(picture);

        await _unitOfWork.SaveChangesAsync();

        await UpdatePictureBinaryAsync(picture, true ? pictureBinary : Array.Empty<byte>());

        await SavePictureInFileAsync(picture.Id, pictureBinary, mimeType);

        return picture;
    }

    public virtual async Task<Picture> UpdatePictureAsync(Picture picture)
    {
        if (picture == null)
            return null;

        var seoFilename = CommonHelper.EnsureMaximumLength(picture.SeoFilename, 100);

        //delete old thumbs if a picture has been changed
        if (seoFilename != picture.SeoFilename)
            await DeletePictureThumbsAsync(picture);

        picture.SeoFilename = seoFilename;

        _pictureRepository.UpdateAsync(picture);
        await _unitOfWork.SaveChangesAsync();

        await UpdatePictureBinaryAsync(picture, true ? (await GetPictureBinaryByPictureIdAsync(picture.Id)).BinaryData : Array.Empty<byte>());

        await SavePictureInFileAsync(picture.Id, (await GetPictureBinaryByPictureIdAsync(picture.Id)).BinaryData, picture.MimeType);

        return picture;
    }

    public virtual async Task<PictureBinary> GetPictureBinaryByPictureIdAsync(Guid pictureId)
    {
        return await _pictureBinaryRepository.QueryNoTracking()
            .FirstOrDefaultAsync(pb => pb.PictureId == pictureId);
    }

    public virtual async Task<Picture> SetSeoFilenameAsync(Guid pictureId, string seoFilename)
    {
        var picture = await GetPictureByIdAsync(pictureId);
        if (picture == null)
            throw new ArgumentException("No picture found with the specified id");

        //update if it has been changed
        if (seoFilename != picture.SeoFilename)
        {
            //update picture
            picture = await UpdatePictureAsync(picture.Id,
                await LoadPictureBinaryAsync(picture),
                picture.MimeType,
                seoFilename,
                picture.AltAttribute,
                picture.TitleAttribute,
                true,
                false);
        }

        return picture;
    }

    public virtual Task<byte[]> ValidatePictureAsync(byte[] pictureBinary, string mimeType)
    {
        try
        {
            using var image = SKBitmap.Decode(pictureBinary);

            //resize the image in accordance with the maximum size
            if (Math.Max(image.Height, image.Width) > _mediaSettings.MaximumImageSize)
            {
                var format = GetImageFormatByMimeType(mimeType);
                pictureBinary = ImageResize(image, format, _mediaSettings.MaximumImageSize);
            }
            return Task.FromResult(pictureBinary);
        }
        catch
        {
            return Task.FromResult(pictureBinary);
        }
    }

    #endregion
}
