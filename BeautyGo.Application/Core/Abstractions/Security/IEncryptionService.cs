using BeautyGo.Domain.Common.Defaults;

namespace BeautyGo.Application.Core.Abstractions.Security;

public interface IEncryptionService
{
    string CreateSaltKey(int size = 5);

    string CreatePasswordHash(string password, string saltKey = "salt1", string passwordFormat = BeautyGoUserDefaults.DefaultHashedPasswordFormat);

    string EncryptText(string plainText, string encryptionPrivateKey = "");

    string DecryptText(string cipherText, string encryptionPrivateKey = "");
}
