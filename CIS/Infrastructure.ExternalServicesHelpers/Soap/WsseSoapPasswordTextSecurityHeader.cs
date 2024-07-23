using System.Globalization;
using System.ServiceModel.Channels;
using System.Xml;

namespace CIS.Infrastructure.ExternalServicesHelpers.Soap;

public class WsseSoapPasswordTextSecurityHeader(string username, string password, string nonce, DateTime created) : MessageHeader
{
    private const string _swuNamespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
    private readonly string _username = username;
    private readonly string _password = password;
    private readonly string _nonce = nonce;
    private readonly DateTime _created = created;

    public override string Name => "Security";
    public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

    protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", Name, Namespace);
        writer.WriteAttributeString("soapenv", "mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/", "1");
        writer.WriteXmlnsAttribute("wsse", Namespace);
        writer.WriteXmlnsAttribute("swu", _swuNamespace);
    }

    protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", "UsernameToken", Namespace);
        writer.WriteAttributeString("wsu", "Id", _swuNamespace, $"UsernameToken-{Guid.NewGuid().ToString("N")}");

        // Username
        writer.WriteElementString("wsse", "Username", Namespace, _username);

        // Password
        writer.WriteStartElement("wsse", "Password", Namespace);
        writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
        writer.WriteValue(_password);
        writer.WriteEndElement();

        // Nonce
        writer.WriteStartElement("wsse", "Nonce", Namespace);
        writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        writer.WriteValue(_nonce);
        writer.WriteEndElement();

        // Created
        writer.WriteElementString("wsu", "Created", Namespace, _created.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));

        writer.WriteEndElement(); // UsernameToken
    }
}