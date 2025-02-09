using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Media;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Application.Core.Abstractions.Media;

public interface IPictureService
{
    Task<byte[]> GetDownloadBitsAsync(IFormFile file);

    Task<string> GetFileExtensionFromMimeTypeAsync(string mimeType);

    Task<byte[]> LoadPictureBinaryAsync(Picture picture);

    Task<string> GetDefaultPictureUrlAsync(int targetSize = 0,
        PictureType defaultPictureType = PictureType.Entity,
        string storeLocation = null);

    Task<string> GetDefaultPicturePathAsync(int targetSize = 0,
        PictureType defaultPictureType = PictureType.Entity,
        string storeLocation = null);

    Task<string> GetPictureUrlAsync(Guid pictureId,
        int targetSize = 0,
        bool showDefaultPicture = true,
        string storeLocation = null,
        PictureType defaultPictureType = PictureType.Entity);

    Task<(string Url, Picture Picture)> GetPictureUrlAsync(Picture picture,
        int targetSize = 0,
        bool showDefaultPicture = true,
        string storeLocation = null,
        PictureType defaultPictureType = PictureType.Entity);

    Task<string> GetThumbLocalPathAsync(Picture picture, int targetSize = 0, bool showDefaultPicture = true);

    Task<Picture> GetPictureByIdAsync(Guid pictureId);

    Task DeletePictureAsync(Picture picture);

    Task<Picture> InsertPictureAsync(byte[] pictureBinary, string mimeType, string seoFilename,
        string altAttribute = null, string titleAttribute = null,
        bool isNew = true, bool validateBinary = true, bool publishEvent = true);

    Task<Picture> InsertPictureAsync(IFormFile formFile, string defaultFileName = "", string virtualPath = "");

    Task<Picture> UpdatePictureAsync(Guid pictureId, byte[] pictureBinary, string mimeType,
        string seoFilename, string altAttribute = null, string titleAttribute = null,
        bool isNew = true, bool validateBinary = true);

    Task<Picture> UpdatePictureAsync(Picture picture);

    Task<Picture> SetSeoFilenameAsync(Guid pictureId, string seoFilename);

    Task<byte[]> ValidatePictureAsync(byte[] pictureBinary, string mimeType);

    Task<PictureBinary> GetPictureBinaryByPictureIdAsync(Guid pictureId);
}
