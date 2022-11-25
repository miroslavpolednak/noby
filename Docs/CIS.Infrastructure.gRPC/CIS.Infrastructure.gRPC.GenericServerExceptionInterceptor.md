#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## GenericServerExceptionInterceptor Class

Server Interceptor který odchytává vyjímky v doménové službě a vytváří z nich RpcException, které dokáže Clients projekt zase přetavit na původní CIS exception.

```csharp
public sealed class GenericServerExceptionInterceptor : Grpc.Core.Interceptors.Interceptor
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [Grpc.Core.Interceptors.Interceptor](https://docs.microsoft.com/en-us/dotnet/api/Grpc.Core.Interceptors.Interceptor 'Grpc.Core.Interceptors.Interceptor') &#129106; GenericServerExceptionInterceptor