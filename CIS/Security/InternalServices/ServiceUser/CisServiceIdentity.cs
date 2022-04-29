﻿using System.Security.Claims;

namespace CIS.Security.InternalServices.ServiceUser;

/// <summary>
/// Identita technickeho uzivatele pod kterym je volana interni sluzba
/// </summary>
internal sealed class CisServiceIdentity 
    : ClaimsIdentity, CIS.Core.Security.IServiceUser
{
    public CisServiceIdentity(string username) 
        : base(InternalServicesAuthentication.DefaultSchemeName, ClaimTypes.NameIdentifier, ClaimTypes.Role)
    {
        this.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
    }
}
