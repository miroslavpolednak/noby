﻿using FOMS.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;

namespace FOMS.Api.StartupExtensions;

public static class FomsAuthentication
{
    public static AuthenticationBuilder AddFomsAuthentication(this WebApplicationBuilder builder, Infrastructure.Configuration.AppConfiguration configuration)
    {
        // its mandatory to have auth scheme
        if (string.IsNullOrEmpty(configuration.AuthenticationScheme))
            throw new ArgumentException($"Authentication scheme is not specified. Please add correct NOBY.AuthenticationScheme in appsettings.json");

        switch (configuration.AuthenticationScheme)
        {
            case AuthenticationConstants.CaasAuthScheme:
                return builder.Services.AddFomsCaasAuthentication();

            // fake authentication
            case AuthenticationConstants.MockAuthScheme:
                return builder.Services.AddFomsMockAuthentication();

            // simple login
            case AuthenticationConstants.SimpleLoginAuthScheme:
                return builder.Services.AddFomsSimpleLoginAuthentication();

            // not existing auth scheme
            default:
                throw new NotImplementedException($"Authentication scheme '{configuration.AuthenticationScheme}' not implemented");
        }
    }
}
