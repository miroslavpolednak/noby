#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC.Configuration](CIS.Infrastructure.gRPC.Configuration.md 'CIS.Infrastructure.gRPC.Configuration')

## GrpcServiceUriSettingsDirect<TService> Class

Implementace bez napojení na ServiceDiscovery.

```csharp
public sealed class GrpcServiceUriSettingsDirect<TService> :
CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>
    where TService : class
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect_TService_.TService'></a>

`TService`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcServiceUriSettingsDirect<TService>

Implements [CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings&lt;](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>')[TService](CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect_TService_.md#CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect_TService_.TService 'CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect<TService>.TService')[&gt;](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>')
### Properties

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect_TService_.ServiceUrl'></a>

## GrpcServiceUriSettingsDirect<TService>.ServiceUrl Property

Adresa služby.

```csharp
public System.Uri? ServiceUrl { get; set; }
```

Implements [ServiceUrl](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md#CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.ServiceUrl 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>.ServiceUrl')

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')