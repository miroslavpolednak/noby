#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## GenericClientExceptionInterceptor Class

Client Interceptor pro konverzi RpcException na CIS vyjímky.

```csharp
public sealed class GenericClientExceptionInterceptor : Grpc.Core.Interceptors.Interceptor
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [Grpc.Core.Interceptors.Interceptor](https://docs.microsoft.com/en-us/dotnet/api/Grpc.Core.Interceptors.Interceptor 'Grpc.Core.Interceptors.Interceptor') &#129106; GenericClientExceptionInterceptor

### Remarks
Používáme, abychom chyby z doménových služeb přetavili z generické RpcException na konkrétní vyjímky, které vyhodila daná doménová služba.