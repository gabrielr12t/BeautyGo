using BeautyGo.Application.Core.Abstractions.Security;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Settings;
using System.Security.Cryptography;
using System.Text;

namespace BeautyGo.Infrasctructure.Services.Security;

public class EncryptionService : IEncryptionService
{
    protected readonly AppSettings _appSettings; 

    public EncryptionService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    #region Utilities

    protected static byte[] EncryptTextToMemory(string data, SymmetricAlgorithm provider)
    {
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
        {
            var toEncrypt = Encoding.Unicode.GetBytes(data);
            cs.Write(toEncrypt, 0, toEncrypt.Length);
            cs.FlushFinalBlock();
        }

        return ms.ToArray();
    }

    protected static string DecryptTextFromMemory(byte[] data, SymmetricAlgorithm provider)
    {
        using var ms = new MemoryStream(data);
        using var cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.Unicode);

        return sr.ReadToEnd();
    }

    protected virtual SymmetricAlgorithm GetEncryptionAlgorithm(string encryptionKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(encryptionKey);

        SymmetricAlgorithm provider = _appSettings.Get<SecuritySettings>().UseAesEncryptionAlgorithm ? Aes.Create() : TripleDES.Create();

        var vectorBlockSize = provider.BlockSize / 8;

        provider.Key = Encoding.ASCII.GetBytes(encryptionKey[0..16]);
        provider.IV = Encoding.ASCII.GetBytes(encryptionKey[^vectorBlockSize..]);

        return provider;
    }

    #endregion

    #region Methods

    public virtual string CreateSaltKey(int size)
    {
        using var provider = RandomNumberGenerator.Create();
        var buff = new byte[size];
        provider.GetBytes(buff);

        return Convert.ToBase64String(buff);
    }

    public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat)
    {
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltkey)), passwordFormat);
    }

    public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
    {
        if (string.IsNullOrEmpty(plainText))
            return plainText;

        if (string.IsNullOrEmpty(encryptionPrivateKey))
            encryptionPrivateKey = _appSettings.Get<SecuritySettings>().EncryptionKey;

        using var provider = GetEncryptionAlgorithm(encryptionPrivateKey);
        var encryptedBinary = EncryptTextToMemory(plainText, provider);

        return Convert.ToBase64String(encryptedBinary);
    }

    public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
    {
        if (string.IsNullOrEmpty(cipherText))
            return cipherText;

        if (string.IsNullOrEmpty(encryptionPrivateKey))
            encryptionPrivateKey = _appSettings.Get<SecuritySettings>().EncryptionKey;

        using var provider = GetEncryptionAlgorithm(encryptionPrivateKey);

        var buffer = Convert.FromBase64String(cipherText);
        return DecryptTextFromMemory(buffer, provider);
    }

    #endregion
}
