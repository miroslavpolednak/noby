using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using System;

namespace CIS.Infrastructure.Security
{
    public class CisCurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContext;

        public CisCurrentUserProvider(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ICisUser Get()
        {
            return new CisUser(2, "John Doe");
        }
    }
}
