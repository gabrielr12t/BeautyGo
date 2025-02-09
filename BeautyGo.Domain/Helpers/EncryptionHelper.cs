using BeautyGo.Domain.Common.Defaults;
using System.Security.Cryptography;
using System.Text;

namespace BeautyGo.Domain.Helpers;

public class EncryptionHelper
{
    #region Utilities

    private static byte[] GenerateKey256(string key)
    {
        using SHA256 sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
    }

    public static async Task<string> EncryptAsync(string plainText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GenerateKey256(key);
            aes.GenerateIV(); // Generate a new IV for each encryption
            byte[] iv = aes.IV;

            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
            using (MemoryStream ms = new MemoryStream())
            {
                // Write the IV to the beginning of the stream
                ms.Write(iv, 0, iv.Length);

                // Encrypt the data
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    await sw.WriteAsync(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static async Task<string> DecryptAsync(string cipherText, string key)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = GenerateKey256(key);

            // Extract the IV from the beginning of the encrypted data
            byte[] iv = new byte[16]; // AES IV size
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            // Decrypt the data
            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (MemoryStream ms = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }

    #endregion

    #region Methods

    public static string CreateSaltKey(int size = 5)
    {
        //generate a cryptographic random number
        using var provider = new RNGCryptoServiceProvider();
        var buff = new byte[size];
        provider.GetBytes(buff);

        // Return a Base64 string representation of the random number
        return Convert.ToBase64String(buff);
    }

    public static string CreatePasswordHash(string password, string saltkey = "salt1", string passwordFormat = BeautyGoServicesDefaults.DefaultHashedPasswordFormat)
    {
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltkey)), passwordFormat);
    }

    #endregion
}
