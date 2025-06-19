using BeautyGo.Application.Core.Abstractions.Security;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Domain.Settings;
using System.Security.Cryptography;

namespace BeautyGo.Infrastructure.Services.Installation;

internal class InstallationService : IInstallationService
{
    private readonly IEncryptionService _encryptionService;
    private readonly IBeautyGoFileProvider _fileProvider;
    private readonly SecuritySettings _securitySettings;

    public InstallationService(
        IEncryptionService encryptionService,
        IBeautyGoFileProvider fileProvider)
    {
        _securitySettings = Singleton<AppSettings>.Instance.Get<SecuritySettings>();
        _encryptionService = encryptionService;
        _fileProvider = fileProvider;
    }

    #region Utilities

    private async Task ConfigureRSA()
    {
        var fileNamePublicKey = _fileProvider.GetFileName(_securitySettings.PublicKeyFilePath);
        var fileNamePrivateKey = _fileProvider.GetFileName(_securitySettings.PrivateKeyFilePath);
        var directoryName = _fileProvider.GetDirectoryName(_securitySettings.PublicKeyFilePath);

        _fileProvider.CreateDirectory(directoryName);
        _fileProvider.CreateFile(_fileProvider.Combine(directoryName, fileNamePublicKey));
        _fileProvider.CreateFile(_fileProvider.Combine(directoryName, fileNamePrivateKey));

        var rsa = RSA.Create(2048);

        var privateKey = rsa.ExportRSAPrivateKey();
        await _fileProvider.WriteAllBytesAsync(_securitySettings.PrivateKeyFilePath, privateKey);

        var publicKey = rsa.ExportRSAPublicKey();
        await _fileProvider.WriteAllBytesAsync(_securitySettings.PublicKeyFilePath, publicKey);
    }

    #endregion

    public Task<bool> IsInstalledAsync()
    {
        return Task.FromResult(_fileProvider.FileExists(_securitySettings.PublicKeyFilePath));
    }

    public async Task InstallAsync()
    {
        await ConfigureRSA();
    }
}
