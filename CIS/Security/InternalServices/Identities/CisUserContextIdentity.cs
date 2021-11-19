using System.Security.Claims;

namespace CIS.Security.InternalServices.Identities
{
    public class CisUserContextIdentity : ClaimsIdentity
    {
        public CisUserContextIdentity(string username)
            : base(InternalServicesAuthentication.ContextUserSchemeName)
        {
            //this.AddClaim(new Claim(CIS.Core.SecurityConstants.ContextUserClaimTypeUserId, username));
        }
    }
}
