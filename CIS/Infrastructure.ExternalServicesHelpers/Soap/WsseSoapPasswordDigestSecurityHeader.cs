using System.Globalization;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace CIS.Infrastructure.ExternalServicesHelpers.Soap;

public class WsseSoapPasswordDigestSecurityHeader(string username, string password) : MessageHeader
{
    private const string _wsuNamespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

    private readonly string _nonce = GenerateNonce();
    private readonly string _created = GenerateTimestamp();

    public override string Name => "Security";
    public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

    protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", Name, Namespace);
        writer.WriteAttributeString("soapenv", "mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/", "1");
        writer.WriteXmlnsAttribute("wsse", Namespace);
        writer.WriteXmlnsAttribute("wsu", _wsuNamespace);
    }

    protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", "UsernameToken", Namespace);
        writer.WriteAttributeString("wsu", "Id", _wsuNamespace, $"UsernameToken-{Guid.NewGuid():N}");

        // Username
        writer.WriteElementString("wsse", "Username", Namespace, username);

        // Password
        writer.WriteStartElement("wsse", "Password", Namespace);
        writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
        writer.WriteValue(ComputePasswordDigest());
        writer.WriteEndElement();

        // Nonce
        writer.WriteStartElement("wsse", "Nonce", Namespace);
        writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        writer.WriteValue(_nonce);
        writer.WriteEndElement();

        // Created
        writer.WriteElementString("wsu", "Created", _wsuNamespace, _created);

        writer.WriteEndElement(); // UsernameToken
    }

    private string ComputePasswordDigest()
    {

#pragma warning disable CA5350

        // Decode the Base64 nonce
        var nonceBytes = Convert.FromBase64String(_nonce);

        // Convert the created timestamp to bytes
        var createdBytes = Encoding.UTF8.GetBytes(_created);

        // Convert the password to bytes
        var passwordBytes = SHA384.HashData(Encoding.UTF8.GetBytes(password));

        // Combine nonce, created, and password
        var combinedBytes = new byte[nonceBytes.Length + createdBytes.Length + passwordBytes.Length];
        Buffer.BlockCopy(nonceBytes, 0, combinedBytes, 0, nonceBytes.Length);
        Buffer.BlockCopy(createdBytes, 0, combinedBytes, nonceBytes.Length, createdBytes.Length);
        Buffer.BlockCopy(passwordBytes, 0, combinedBytes, nonceBytes.Length + createdBytes.Length, passwordBytes.Length);

        // Compute SHA-1 hash
        var hashBytes = SHA1.HashData(combinedBytes);
#pragma warning restore CA5350

        return Convert.ToBase64String(hashBytes);
    }

    private static string GenerateNonce()
    {
        var nonceBytes = RandomNumberGenerator.GetBytes(16);

        return Convert.ToBase64String(nonceBytes);
    }

    private static string GenerateTimestamp()
    {
        return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}