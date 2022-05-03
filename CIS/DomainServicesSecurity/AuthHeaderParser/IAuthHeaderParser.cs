namespace CIS.DomainServicesSecurity;

internal interface IAuthHeaderParser
{
    ParseResult Parse(string authHeaderContent);
}
