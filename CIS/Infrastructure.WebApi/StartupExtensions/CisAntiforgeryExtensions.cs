﻿using Microsoft.AspNetCore.Antiforgery;

namespace CIS.Infrastructure.WebApi;

public static class CisAntiforgeryExtensions
{
    public static IServiceCollection AddCisAntiforgery(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.Cookie = new CookieBuilder
            {
                Name = "XSRF",
                SameSite = SameSiteMode.Strict,
                SecurePolicy = CookieSecurePolicy.Always,
                HttpOnly = true
            };
            options.HeaderName = "X-XSRF-TOKEN";
        });
        services.AddScoped<AntiforgeryMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseCisAntiforgery(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AntiforgeryMiddleware>();
    }

    internal sealed class AntiforgeryMiddleware 
        : IMiddleware
    {
        private static string[] _restrictedMethods = new[] { "POST", "DELETE", "PUT" };
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryMiddleware(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_restrictedMethods.Contains(context.Request.Method))
            {
                await _antiforgery.ValidateRequestAsync(context);
            }

            // create new csrf tokens
            var tokens = _antiforgery.GetAndStoreTokens(context);
            var headerToken = tokens.RequestToken;
            if (!string.IsNullOrEmpty(headerToken))
            {
                context.Response.Cookies.Append("XSRF-TOKEN", headerToken, new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = false
                });
            }

            await next(context);
        }
    }
}
