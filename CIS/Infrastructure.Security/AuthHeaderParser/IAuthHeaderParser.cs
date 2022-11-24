namespace CIS.Infrastructure.Security;

internal interface IAuthHeaderParser
{
    ParseResult Parse(string authHeaderContent);
}
