﻿using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Client Interceptor který automaticky přidává hlavičku "noby-user-id" (tj. ID kontextového uživatele) do každého requestu na doménovou službu.
/// </summary>
/// <remarks>
/// TODO toto neni uplne pekna implementace, ale neprisel jsem na jiny zpusob jak v grpc pipeline vyklepat scoped instanci ICurrentUserAccessor a vrazit ji do headeru
/// </remarks>
public sealed class ContextUserForwardingClientInterceptor 
    : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public ContextUserForwardingClientInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var userAccessor = serviceScope.ServiceProvider.GetService<Core.Security.ICurrentUserAccessor>();

            if (userAccessor?.IsAuthenticated ?? false)
            {
                Metadata metadata = new();
                if (context.Options.Headers is not null && context.Options.Headers.Count != 0)
                    foreach (var m in context.Options.Headers)
                        metadata.Add(m);
                
                // ID prihlaseneho uzivatele
                metadata.Add(Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdKey, userAccessor!.User!.Id.ToString(System.Globalization.CultureInfo.InvariantCulture));
                
                // login prihlaseneho uzivatele
                if (!string.IsNullOrEmpty(userAccessor.User.Login))
                {
                    metadata.Add(Core.Security.SecurityConstants.ContextUserHttpHeaderUserIdentKey, userAccessor.User.Login);
                }

                var newOptions = context.Options.WithHeaders(metadata);
                var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, newOptions);

                return base.AsyncUnaryCall(request, newContext, continuation);
            }
            else if (userAccessor is null)
            {
                serviceScope.ServiceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger<ContextUserForwardingClientInterceptor>()
                    .UserAccessorNotFound();
            }
            else if (context.Method.Name != "GetUserByLogin") //ignorujeme chyby v callu do UserService
            {
                serviceScope.ServiceProvider
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger<ContextUserForwardingClientInterceptor>()
                    .UserAccessorAnonymous();
            }

            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
