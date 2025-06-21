using System.Security.Cryptography;

namespace BeautyGo.Application.Core.Providers;

public interface IRsaKeyProvider
{
    RSA GetPrivateKey();
    RSA GetPublicKey();
}
