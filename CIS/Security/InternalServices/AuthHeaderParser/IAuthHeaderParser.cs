namespace CIS.Security.InternalServices
{
    public interface IAuthHeaderParser
    {
        ParseResult Parse(string authHeaderContent);
    }
}
