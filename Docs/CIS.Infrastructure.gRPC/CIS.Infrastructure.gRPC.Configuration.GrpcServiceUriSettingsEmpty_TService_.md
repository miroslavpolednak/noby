#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC.Configuration](CIS.Infrastructure.gRPC.Configuration.md 'CIS.Infrastructure.gRPC.Configuration')

## GrpcServiceUriSettingsEmpty<TService> Class

```csharp
public sealed class GrpcServiceUriSettingsEmpty<TService> :
CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>
    where TService : class
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsEmpty_TService_.TService'></a>

`TService`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcServiceUriSettingsEmpty<TService>

Implements [CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings&lt;](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>')[TService](CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsEmpty_TService_.md#CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsEmpty_TService_.TService 'CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsEmpty<TService>.TService')[&gt;](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>')
### Properties

<a name='CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsEmpty_TService_.ServiceUrl'></a>

## GrpcServiceUriSettingsEmpty<TService>.ServiceUrl Property

Adresa služby.

```csharp
public System.Uri? ServiceUrl { get; set; }
```

Implements [ServiceUrl](CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.md#CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.ServiceUrl 'CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings<TService>.ServiceUrl')

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')