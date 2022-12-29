#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC.Configuration](CIS.Infrastructure.gRPC.Configuration.md 'CIS.Infrastructure.gRPC.Configuration')

## IGrpcServiceUriSettings<TService> Interface

Reprezentuje konfiguraci připojení na doménovou službu.

```csharp
public interface IGrpcServiceUriSettings<TService>
    where TService : class
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.TService'></a>

`TService`

Derived  
&#8627; [GrpcServiceUriSettingsDirect&lt;TService&gt;](CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect_TService_.md 'CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect<TService>')  
&#8627; [GrpcServiceUriSettingsServiceDiscovery&lt;TService&gt;](CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery_TService_.md 'CIS.Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsServiceDiscovery<TService>')

### Remarks
Jedná se o třídu s generickým parametrem, protože mohu mít v projektu napojených více gRPC služeb. Pak je TService typem klienta pro každou službu.
### Properties

<a name='CIS.Infrastructure.gRPC.Configuration.IGrpcServiceUriSettings_TService_.ServiceUrl'></a>

## IGrpcServiceUriSettings<TService>.ServiceUrl Property

Adresa služby.

```csharp
System.Uri? ServiceUrl { get; }
```

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')