#### [CIS.Core](index.md 'index')
### [CIS.Core.Configuration](CIS.Core.Configuration.md 'CIS.Core.Configuration')

## ICisEnvironmentConfiguration Interface

Konfigurace služby v prostředí NOBY. Binduje se z elementu *CisEnvironmentConfiguration* v appsettings.json.

```csharp
public interface ICisEnvironmentConfiguration
```
### Properties

<a name='CIS.Core.Configuration.ICisEnvironmentConfiguration.DefaultApplicationKey'></a>

## ICisEnvironmentConfiguration.DefaultApplicationKey Property

Název služby v systému NOBY

```csharp
string? DefaultApplicationKey { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
DS:CustomerService

### Remarks
[typ_sluzby]:[nazev_sluzby] - DS (Doménová služba), CIS (Infrastrukturní služba)

<a name='CIS.Core.Configuration.ICisEnvironmentConfiguration.EnvironmentName'></a>

## ICisEnvironmentConfiguration.EnvironmentName Property

Název aplikačního prostředí

```csharp
string? EnvironmentName { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
DEV

### Remarks
Povolené hodnoty: DEV, FAT, UAT, SIT, PROD

<a name='CIS.Core.Configuration.ICisEnvironmentConfiguration.InternalServicePassword'></a>

## ICisEnvironmentConfiguration.InternalServicePassword Property

Heslo servisního uživatele

```csharp
string? InternalServicePassword { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
passw0rd

<a name='CIS.Core.Configuration.ICisEnvironmentConfiguration.InternalServicesLogin'></a>

## ICisEnvironmentConfiguration.InternalServicesLogin Property

Login uživatele pod kterým se daná služba autentizuje do ostatních služeb v rámci NOBY

```csharp
string? InternalServicesLogin { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
user_a

### Remarks
Je nutné zadat pokud má fungovat automatické dohledávání adres služeb v Clients projektech.

<a name='CIS.Core.Configuration.ICisEnvironmentConfiguration.ServiceDiscoveryUrl'></a>

## ICisEnvironmentConfiguration.ServiceDiscoveryUrl Property

Adresa Service discovery služby pro dané prostředí

```csharp
string? ServiceDiscoveryUrl { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Example
https://177.0.0.55:30000

### Remarks
Pokud není zadáno, nebude fungovat automatické dohledávání adres služeb v Clients projektech.