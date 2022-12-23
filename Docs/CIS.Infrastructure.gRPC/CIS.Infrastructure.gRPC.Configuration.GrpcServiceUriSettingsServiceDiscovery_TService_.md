#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC.Configuration](CIS.Infrastructure.gRPC.Configuration.md 'CIS.Infrastructure.gRPC.Configuration')

## GrpcServiceUriSettingsServiceDiscovery<TService> Class

Implementace podporující ServiceDiscovery.

```csharp
public sealed class GrpcServiceUriSettingsServiceDiscovery<TService> :
CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>,
CIS.Core.IIsServiceDiscoverable
    where TService : class
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery_TService_.TService'></a>

`TService`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcServiceUriSettingsServiceDiscovery<TService>

Implements [CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings&lt;](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>')[TService](CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery_TService_.md#CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery_TService_.TService 'CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery<TService>.TService')[&gt;](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>'), [CIS.Core.IIsServiceDiscoverable](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable 'CIS.Core.IIsServiceDiscoverable')
### Properties

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery_TService_.ServiceType'></a>

## GrpcServiceUriSettingsServiceDiscovery<TService>.ServiceType Property

Always gRPC (=1)

```csharp
public int ServiceType { get; }
```

Implements [ServiceType](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable.ServiceType 'CIS.Core.IIsServiceDiscoverable.ServiceType')

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery_TService_.ServiceUrl'></a>

## GrpcServiceUriSettingsServiceDiscovery<TService>.ServiceUrl Property

Adresa služby.

```csharp
public System.Uri? ServiceUrl { get; set; }
```

Implements [ServiceUrl](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.IIsServiceDiscoverable.ServiceUrl 'CIS.Core.IIsServiceDiscoverable.ServiceUrl'), [ServiceUrl](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md#CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.ServiceUrl 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>.ServiceUrl')

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')