using CIS.Core.Security;

namespace CIS.Infrastructure.Security
{
    public class CisCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContext;

        public CisCurrentUserAccessor(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ICurrentUser User
        {
            get
            {
                return new CisUser(2, "John Doe", "990614w");
            }
        }
    }
}
