#### [CIS.Core](index.md 'index')
### [CIS.Core.Configuration](CIS.Core.Configuration.md 'CIS.Core.Configuration')

## KestrelConfiguration Class

Nastavení Kestrel serveru v gRPC službách.

```csharp
public sealed class KestrelConfiguration
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; KestrelConfiguration

### Remarks
Nepoužíváme standardní Kestrel konfigurační soubor, protože nám neumožňuje zadávat např. SSL certifikáty.
### Properties

<a name='CIS.Core.Configuration.KestrelConfiguration.Certificate'></a>

## KestrelConfiguration.Certificate Property

SSL certifikát použitý pro vytvoření HTTPS tunelu

```csharp
public CIS.Core.Configuration.KestrelConfiguration.CertificateInfo? Certificate { get; set; }
```

#### Property Value
[CertificateInfo](CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.md 'CIS.Core.Configuration.KestrelConfiguration.CertificateInfo')

<a name='CIS.Core.Configuration.KestrelConfiguration.Endpoints'></a>

## KestrelConfiguration.Endpoints Property

Nastavené endpointy pro danou službu

```csharp
public System.Collections.Generic.List<CIS.Core.Configuration.KestrelConfiguration.EndpointInfo>? Endpoints { get; set; }
```

#### Property Value
[System.Collections.Generic.List&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')[EndpointInfo](CIS.Core.Configuration.KestrelConfiguration.EndpointInfo.md 'CIS.Core.Configuration.KestrelConfiguration.EndpointInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')