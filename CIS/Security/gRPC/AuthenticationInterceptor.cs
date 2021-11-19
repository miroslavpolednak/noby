using Grpc.Core;
using Grpc.Core.Interceptors;
using System;

namespace CIS.Security.gRPC
{
    public class AuthenticationInterceptor : Interceptor
    {
        private readonly Core.Configuration.ICisEnvironmentConfiguration _configuration;

        public AuthenticationInterceptor(Core.Configuration.ICisEnvironmentConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var headerValue = new Metadata.Entry("authorization", "Basic " + (_authorizationHeader ?? getAuthorizationHeader()));

            if (context.Options.Headers == null)
            {
                var headers = new Metadata();
                headers.Add(headerValue);

                var newOptions = context.Options.WithHeaders(headers);
                var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, newOptions);

                return base.AsyncUnaryCall(request, newContext, continuation);
            }
            else if (context.Options.Headers.Get("authorization") is null)
            {
                context.Options.Headers.Add(headerValue);
            }

            return continuation(request, context);
        }

        private string getAuthorizationHeader()
        {
            if (string.IsNullOrEmpty(_configuration.InternalServicesLogin) || string.IsNullOrEmpty(_configuration.InternalServicePassword))
                throw new System.ArgumentNullException("InternalServicesLogin or InternalServicePassword is empty");

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_configuration.InternalServicesLogin}:{_configuration.InternalServicePassword}");
            _authorizationHeader = Convert.ToBase64String(plainTextBytes);

            return _authorizationHeader;
        }

        protected static string? _authorizationHeader = null;
    }
}
