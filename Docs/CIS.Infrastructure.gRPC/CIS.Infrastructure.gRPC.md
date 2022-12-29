#### [CIS.Infrastructure.gRPC](index.md 'index')

## CIS.Infrastructure.gRPC Namespace

| Classes | |
| :--- | :--- |
| [ContextUserForwardingClientInterceptor](CIS.Infrastructure.gRPC.ContextUserForwardingClientInterceptor.md 'CIS.Infrastructure.gRPC.ContextUserForwardingClientInterceptor') | Client Interceptor který automaticky přidává hlavičku "mp-user-id" (tj. ID kontextového uživatele) do každého requestu na doménovou službu. |
| [GenericClientExceptionInterceptor](CIS.Infrastructure.gRPC.GenericClientExceptionInterceptor.md 'CIS.Infrastructure.gRPC.GenericClientExceptionInterceptor') | Client Interceptor pro konverzi RpcException na CIS vyjímky. |
| [GenericServerExceptionInterceptor](CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor.md 'CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor') | Server Interceptor který odchytává vyjímky v doménové službě a vytváří z nich RpcException, které dokáže Clients projekt zase přetavit na původní CIS exception. |
| [GrpcErrorCollection](CIS.Infrastructure.gRPC.GrpcErrorCollection.md 'CIS.Infrastructure.gRPC.GrpcErrorCollection') | Kolekce chyb uložená v Trailers grpc response (ve chvíli, kdy se vrací RpcException) |
| [GrpcExceptionHelpers](CIS.Infrastructure.gRPC.GrpcExceptionHelpers.md 'CIS.Infrastructure.gRPC.GrpcExceptionHelpers') | Helpery pro vytváření RpcException. |
| [KestrelExtensions](CIS.Infrastructure.gRPC.KestrelExtensions.md 'CIS.Infrastructure.gRPC.KestrelExtensions') | Nastavení Kestrel serveru pro gRPC služby. |
| [StartupExtensions](CIS.Infrastructure.gRPC.StartupExtensions.md 'CIS.Infrastructure.gRPC.StartupExtensions') | Extension metody do startupu aplikace pro registraci gRPC služeb. |
