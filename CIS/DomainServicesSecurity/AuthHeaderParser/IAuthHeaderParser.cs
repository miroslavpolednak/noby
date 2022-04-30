namespace CIS.DomainServicesSecurity;

public interface IAuthHeaderParser
{
    ParseResult Parse(string authHeaderContent);
}
