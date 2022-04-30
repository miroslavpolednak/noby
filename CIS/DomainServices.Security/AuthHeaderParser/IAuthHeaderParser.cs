namespace CIS.DomainServices.Security;

public interface IAuthHeaderParser
{
    ParseResult Parse(string authHeaderContent);
}
