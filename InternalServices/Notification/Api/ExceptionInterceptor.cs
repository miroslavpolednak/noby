using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CIS.InternalServices.Notification.Api
{
    public class ExceptionInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (RpcException e)
            {
                _logger.LogInformation(e, e.Message);
                throw;
            }
            catch (Exception e) // neosetrena vyjimka
            {
                _logger.LogError(e, e.Message);
                throw new RpcException(new Status(StatusCode.Internal, e.Message, e));
            }
        }
    }
}
