using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace CIS.Security.InternalServices
{
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
                    _logger.LogDebug("Authorization header is not Basic");
                    return new ParseResult(1, "Authorization header is not set to Basic authentication");
                }
                else
                {
                    // login a heslo jsou v Base64 ve formatu {login}:{heslo}
                    string usernamePwd = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter ?? throw new Exception("AuthenticationHeader parameter not set")));
                    int separatorIndex = usernamePwd.IndexOf(":");

                    // string neobsahuje :
                    if (separatorIndex <= 0)
                    {
                        _logger.LogDebug("Missing ':' in Base authentication");
                        return new ParseResult(2, "Missing ':' in Base authentication");
                    }
                    else
                    {
                        string login = usernamePwd.Substring(0, separatorIndex);

                        _logger.LogDebug("Parsed as {Login}", login);
                        return new ParseResult(login, usernamePwd.Substring(separatorIndex + 1));
                    }
                }
            }
            catch
            {
                _logger.LogDebug("Incorrect Authorization header");
                return new ParseResult(3, "Incorrect Authorization header");
            }
        }
    }
}
