using System.Security.Claims;

namespace CIS.Security.InternalServices.Identities
{
    public class CisServiceIdentity : ClaimsIdentity
    {
        public CisServiceIdentity(string username) 
            : base(InternalServicesAuthentication.DefaultSchemeName, ClaimTypes.NameIdentifier, ClaimTypes.Role)
        {
            this.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
        }
    }
}
