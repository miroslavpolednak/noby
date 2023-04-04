#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## ContextUserForwardingClientInterceptor Class

Client Interceptor který automaticky přidává hlavičku "noby-user-id" (tj. ID kontextového uživatele) do každého requestu na doménovou službu.

```csharp
public sealed class ContextUserForwardingClientInterceptor : Grpc.Core.Interceptors.Interceptor
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [Grpc.Core.Interceptors.Interceptor](https://docs.microsoft.com/en-us/dotnet/api/Grpc.Core.Interceptors.Interceptor 'Grpc.Core.Interceptors.Interceptor') &#129106; ContextUserForwardingClientInterceptor

### Remarks
TODO toto neni uplne pekna implementace, ale neprisel jsem na jiny zpusob jak v grpc pipeline vyklepat scoped instanci ICurrentUserAccessor a vrazit ji do headeru