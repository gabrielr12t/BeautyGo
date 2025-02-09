using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace BeautyGo.Domain.Helpers;

public class RsaHelper
{
    private static RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048);
    private RSAParameters _privateKey;
    private RSAParameters _publicKey;

    public RsaHelper()
    {
        _privateKey = rsaProvider.ExportParameters(true);
        _publicKey = rsaProvider.ExportParameters(false);
    }

    public string Ecrypt(string plainText)
    {
        rsaProvider = new RSACryptoServiceProvider();
        rsaProvider.ImportParameters(_publicKey);

        var data = Encoding.Unicode.GetBytes(plainText);
        var cypher = rsaProvider.Encrypt(data, false);

        return Convert.ToBase64String(cypher);
    }

    public string Decrypt(string cypherText)
    {
        var dataBytes = Convert.FromBase64String(cypherText);

        rsaProvider.ImportParameters(_privateKey);

        var plainText = rsaProvider.Decrypt(dataBytes, false);

        return Encoding.Unicode.GetString(plainText);
    }

    public string GetPublicKey()
    {
        var writer = new StringWriter();
        var serializer = new XmlSerializer(typeof(RSAParameters));

        serializer.Serialize(writer, _publicKey);

        return writer.ToString();
    }
}
