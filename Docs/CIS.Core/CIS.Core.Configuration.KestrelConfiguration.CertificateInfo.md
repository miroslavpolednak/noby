#### [CIS.Core](index.md 'index')
### [CIS.Core.Configuration](CIS.Core.Configuration.md 'CIS.Core.Configuration').[KestrelConfiguration](CIS.Core.Configuration.KestrelConfiguration.md 'CIS.Core.Configuration.KestrelConfiguration')

## KestrelConfiguration.CertificateInfo Class

Nastavení SSL certifikátu

```csharp
public sealed class KestrelConfiguration.CertificateInfo
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CertificateInfo
### Properties

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.CertStoreLocation'></a>

## KestrelConfiguration.CertificateInfo.CertStoreLocation Property

Typ Windows Certificate store

```csharp
public System.Security.Cryptography.X509Certificates.StoreLocation CertStoreLocation { get; set; }
```

#### Property Value
[System.Security.Cryptography.X509Certificates.StoreLocation](https://docs.microsoft.com/en-us/dotnet/api/System.Security.Cryptography.X509Certificates.StoreLocation 'System.Security.Cryptography.X509Certificates.StoreLocation')

### Example
LocalMachine, CurrenUser

### Remarks
Pouze pokud Location=CertStore

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.CertStoreName'></a>

## KestrelConfiguration.CertificateInfo.CertStoreName Property

Název složky ve Windows Certificate store

```csharp
public System.Security.Cryptography.X509Certificates.StoreName CertStoreName { get; set; }
```

#### Property Value
[System.Security.Cryptography.X509Certificates.StoreName](https://docs.microsoft.com/en-us/dotnet/api/System.Security.Cryptography.X509Certificates.StoreName 'System.Security.Cryptography.X509Certificates.StoreName')

### Example
My, Root

### Remarks
Pouze pokud Location=CertStore

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.Location'></a>

## KestrelConfiguration.CertificateInfo.Location Property

Druh úložiště certifkátu

```csharp
public CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes Location { get; set; }
```

#### Property Value
[LocationTypes](CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.md#CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes 'CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.LocationTypes')

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.Password'></a>

## KestrelConfiguration.CertificateInfo.Password Property

Heslo certifikátu uloženého na filesystému

```csharp
public string? Password { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Pouze pokud Location=FileSystem

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.Path'></a>

## KestrelConfiguration.CertificateInfo.Path Property

URI souboru s certifikátem na filesystému

```csharp
public string? Path { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Pouze pokud Location=FileSystem

<a name='CIS.Core.Configuration.KestrelConfiguration.CertificateInfo.Thumbprint'></a>

## KestrelConfiguration.CertificateInfo.Thumbprint Property

Thumbprint certifikátu

```csharp
public string? Thumbprint { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Pouze pokud Location=CertStore