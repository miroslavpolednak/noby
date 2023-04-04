#### [CIS.Infrastructure.gRPC](index.md 'index')

## CIS.Infrastructure.gRPC Namespace

| Classes | |
| :--- | :--- |
| [ByteStringExtensions](CIS.Infrastructure.gRPC.ByteStringExtensions.md 'CIS.Infrastructure.gRPC.ByteStringExtensions') | |
| [CisGrpcHealthChecks](CIS.Infrastructure.gRPC.CisGrpcHealthChecks.md 'CIS.Infrastructure.gRPC.CisGrpcHealthChecks') | |
| [ContextUserForwardingClientInterceptor](CIS.Infrastructure.gRPC.ContextUserForwardingClientInterceptor.md 'CIS.Infrastructure.gRPC.ContextUserForwardingClientInterceptor') | Client Interceptor který automaticky přidává hlavičku "noby-user-id" (tj. ID kontextového uživatele) do každého requestu na doménovou službu. |
| [GenericClientExceptionInterceptor](CIS.Infrastructure.gRPC.GenericClientExceptionInterceptor.md 'CIS.Infrastructure.gRPC.GenericClientExceptionInterceptor') | Client Interceptor pro konverzi RpcException na CIS vyjímky. |
| [GenericServerExceptionInterceptor](CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor.md 'CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor') | Server Interceptor který odchytává vyjímky v doménové službě a vytváří z nich RpcException, které dokáže Clients projekt zase přetavit na původní CIS exception. |
| [GrpcExceptionHelpers](CIS.Infrastructure.gRPC.GrpcExceptionHelpers.md 'CIS.Infrastructure.gRPC.GrpcExceptionHelpers') | Helpery pro vytváření RpcException. |
| [KestrelExtensions](CIS.Infrastructure.gRPC.KestrelExtensions.md 'CIS.Infrastructure.gRPC.KestrelExtensions') | Nastavení Kestrel serveru pro gRPC služby. |
| [StartupExtensions](CIS.Infrastructure.gRPC.StartupExtensions.md 'CIS.Infrastructure.gRPC.StartupExtensions') | Extension metody do startupu aplikace pro registraci gRPC služeb. |
