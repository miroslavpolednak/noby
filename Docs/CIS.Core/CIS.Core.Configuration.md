#### [CIS.Core](index.md 'index')

## CIS.Core.Configuration Namespace

Interfaces pro obecné konfigurační objekty ekosystému - appsettings.json, kestrel.json.

| Classes | |
| :--- | :--- |
| [KestrelConfiguration](CIS.Core.Configuration.KestrelConfiguration.md 'CIS.Core.Configuration.KestrelConfiguration') | Nastavení Kestrel serveru v gRPC službách. |
| [KestrelConfiguration.CertificateInfo](CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.md 'CIS.Core.Configuration.KestrelConfiguration.CertificateInfo') | Nastavení SSL certifikátu |
| [KestrelConfiguration.EndpointInfo](CIS.Core.Configuration.KestrelConfiguration.EndpointInfo.md 'CIS.Core.Configuration.KestrelConfiguration.EndpointInfo') | Nastavení endpointu |

| Interfaces | |
| :--- | :--- |
| [ICisEnvironmentConfiguration](CIS.Core.Configuration.ICisEnvironmentConfiguration.md 'CIS.Core.Configuration.ICisEnvironmentConfiguration') | Konfigurace služby v prostředí NOBY. Binduje se z elementu *CisEnvironmentConfiguration* v appsettings.json. |
| [ICorsConfiguration](CIS.Core.Configuration.ICorsConfiguration.md 'CIS.Core.Configuration.ICorsConfiguration') | Nastavení CORS pro REST/Webapi projekty. |
### Enums

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes'></a>

## KestrelConfiguration.CertificateInfo.LocationTypes Enum

Možné způsoby

```csharp
public enum KestrelConfiguration.CertificateInfo.LocationTypes
```
### Fields

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes.CertStore'></a>

`CertStore` 2

Certifikát je uložený ve Windows Certificate store

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes.FileSystem'></a>

`FileSystem` 1

Certifikát je uložený na filesystému