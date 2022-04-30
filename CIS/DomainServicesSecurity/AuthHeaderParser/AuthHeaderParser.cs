using System.Net.Http.Headers;
using System.Text;

namespace CIS.DomainServicesSecurity;

/// <summary>
/// Parsovat login a heslo z Authorization headeru
/// </summary>
internal class AuthHeaderParser : IAuthHeaderParser
{
    private readonly ILogger<IAuthHeaderParser> _logger;

    public AuthHeaderParser(ILogger<IAuthHeaderParser> logger) 
    {
        _logger = logger;
    }

    /// <param name="authHeaderContent">Basic [{login}:{heslo}]{Base64}</param>
    public ParseResult Parse(string authHeaderContent)
    {
        try
        {
            var header = AuthenticationHeaderValue.Parse(authHeaderContent);

            // Je pouzita jina nez Basic autentizace
            if (header.Scheme != "Basic")
            {
                _logger.AuthIsNotBasic();
                return new ParseResult(1, "Authorization header is not set to Basic authentication");
            }
            else
            {
                // login a heslo jsou v Base64 ve formatu {login}:{heslo}
                string usernamePwd = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter ?? throw new System.Security.Authentication.AuthenticationException("AuthenticationHeader parameter not set")));
                int separatorIndex = usernamePwd.IndexOf(":", StringComparison.OrdinalIgnoreCase);

                // string neobsahuje :
                if (separatorIndex <= 0)
                {
                    _logger.AuthMissingColon();
                    return new ParseResult(2, "Missing ':' in Base authentication");
                }
                else
                {
                    string login = usernamePwd.Substring(0, separatorIndex);

                    _logger.AuthParsedLogin(login);
                    return new ParseResult(login, usernamePwd.Substring(separatorIndex + 1));
                }
            }
        }
        catch
        {
            _logger.AuthIncorrectAuthHeader();
            return new ParseResult(3, "Incorrect Authorization header");
        }
    }
}
