using BeautyGo.Application.Core.Abstractions.Security;
using BeautyGo.Domain.Common.Defaults;
using System.Security.Cryptography;

namespace BeautyGo.Infrasctructure.Services.Installation;

internal class InstallationService : IInstallationService
{ 
    private IEncryptionService _encryptionService;

    public InstallationService(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    private string GenerateSecureKeyString(int length)
    {
        byte[] randomBytes = new byte[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    private void CreateSecurityPrivateKey()
    {
        var securityPrivateKey = _encryptionService.EncryptText(
            GenerateSecureKeyString(32),
            BeautyGoCommonDefault.EnvironmentVariablePrivateKey);

        Environment.SetEnvironmentVariable(BeautyGoCommonDefault.PrivateKeyName, securityPrivateKey, EnvironmentVariableTarget.User);
    }

    public async Task InstallAsync()
    {
        var environmentVariable = Environment.GetEnvironmentVariable(BeautyGoCommonDefault.PrivateKeyName, EnvironmentVariableTarget.User);
        if (environmentVariable == null)
        {
            CreateSecurityPrivateKey();
        }

        await Task.CompletedTask;
    }
}
