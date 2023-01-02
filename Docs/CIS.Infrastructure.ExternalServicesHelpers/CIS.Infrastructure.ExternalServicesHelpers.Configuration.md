#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')

## CIS.Infrastructure.ExternalServicesHelpers.Configuration Namespace

Implementace konfigurace konzumované služby v appsettings.json.

| Classes | |
| :--- | :--- |
| [ExternalServiceConfiguration&lt;TClient&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>') | Výchozí implementace IExternalServiceConfiguration. |

| Interfaces | |
| :--- | :--- |
| [IExternalServiceConfiguration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration') | Základní konfigurace externí služby (služby třetí strany). |
| [IExternalServiceConfiguration&lt;TClient&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>') | Generická verze konfigurace. |
### Enums

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes'></a>

## ExternalServicesAuthenticationTypes Enum

Možné typy autentizace na službu třetí strany.

```csharp
public enum ExternalServicesAuthenticationTypes
```
### Fields

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes.Basic'></a>

`Basic` 2

HTTP Basic Authentication

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServicesAuthenticationTypes.None'></a>

`None` 1

Bez autentizace