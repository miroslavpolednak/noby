﻿using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace DomainServices.CustomerService.Abstraction
{
    internal class ExceptionInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var response = continuation(request, context);
            var task = catcher(response, context.Method.FullName);
            return new AsyncUnaryCall<TResponse>(task, response.ResponseHeadersAsync, response.GetStatus, response.GetTrailers, response.Dispose);
        }

        private async Task<TResponse> catcher<TResponse>(AsyncUnaryCall<TResponse> responseAsync, string methodFullName)
        {
            try
            {
                return await responseAsync;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable) // nedostupna sluzba
            {
                _logger.LogError(ex, $"CustomerService unavailable: {ex.Message}");
                throw new ServiceUnavailableException(methodFullName, ex.Message);
            }
            catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument)
            {
                throw new CisArgumentException(ex.GetExceptionCodeFromTrailers(), ex.GetErrorMessageFromRpcException(), ex.GetArgumentFromTrailers());
            }
            catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.Aborted) // EAS/sim nebylo mozne zavolat
            {
                throw; // new Exceptions.SimulationServiceFatalException(ex.GetExceptionCodeFromTrailers(), ex.GetErrorMessageFromRpcException());
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, $"Uncought RpcException: {ex.Message}");
                throw;
            }
            catch (Exception ex) when (ex is not RpcException)
            {
                _logger.LogError(ex, $"[{methodFullName}] {ex.Message}");
                throw;
            }
        }
    }
}
