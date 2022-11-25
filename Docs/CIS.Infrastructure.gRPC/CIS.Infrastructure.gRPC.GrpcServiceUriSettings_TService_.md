#### [CIS.Infrastructure.gRPC](index.md 'index')
### [CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC.md 'CIS.Infrastructure.gRPC')

## GrpcServiceUriSettings<TService> Class

Reprezentuje konfiguraci připojení na doménovou službu.

```csharp
public sealed class GrpcServiceUriSettings<TService>
    where TService : class
```
#### Type parameters

<a name='CIS.Infrastructure.gRPC.GrpcServiceUriSettings_TService_.TService'></a>

`TService`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; GrpcServiceUriSettings<TService>

### Remarks
Jedná se o třídu s generickým parametrem, protože mohu mít v projektu napojených více gRPC služeb. Pak je TService typem klienta pro každou službu.
### Properties

<a name='CIS.Infrastructure.gRPC.GrpcServiceUriSettings_TService_.ServiceType'></a>

## GrpcServiceUriSettings<TService>.ServiceType Property

Typ klienta (Clients projekt) pro danou konfiguraci.

```csharp
public System.Type ServiceType { get; set; }
```

#### Property Value
[System.Type](https://docs.microsoft.com/en-us/dotnet/api/System.Type 'System.Type')

<a name='CIS.Infrastructure.gRPC.GrpcServiceUriSettings_TService_.Url'></a>

## GrpcServiceUriSettings<TService>.Url Property

Adresa služby.

```csharp
public System.Uri Url { get; set; }
```

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')