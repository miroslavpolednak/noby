﻿using CIS.Core.Security;
using Microsoft.AspNetCore.Http;

namespace FOMS.Infrastructure.Security;

public class FomsCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor? _httpContext;

    public FomsCurrentUserAccessor(IHttpContextAccessor? httpContext)
    {
        _httpContext = httpContext;
    }

    public ICurrentUser? User
    {
        get
        {
            return _httpContext?.HttpContext?.User as ICurrentUser;
        }
    }
}
