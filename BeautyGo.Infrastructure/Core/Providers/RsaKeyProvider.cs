using BeautyGo.Application.Core.Providers;
using BeautyGo.Domain.Settings;
using System.Security.Cryptography;

namespace BeautyGo.Infrastructure.Core.Providers;

public class RsaKeyProvider : IRsaKeyProvider, IDisposable
{
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;

    public RsaKeyProvider(SecuritySettings securitySettings, IBeautyGoFileProvider fileProvider)
    {
        var privateKey = fileProvider.ReadAllBytesAsync(securitySettings.PrivateKeyFilePath).GetAwaiter().GetResult();
        _privateRsa = RSA.Create();
        _privateRsa.ImportRSAPrivateKey(privateKey, out _);

        var publicKey = fileProvider.ReadAllBytesAsync(securitySettings.PublicKeyFilePath).GetAwaiter().GetResult();
        _publicRsa = RSA.Create();
        _publicRsa.ImportRSAPublicKey(publicKey, out _);
    }

    public RSA GetPrivateKey() => _privateRsa;
    public RSA GetPublicKey() => _publicRsa;

    public void Dispose()
    {
        _privateRsa.Dispose();
        _publicRsa.Dispose();
    }
}
