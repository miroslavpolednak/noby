#### [CIS.Infrastructure.Telemetry](index.md 'index')
### [CIS.Infrastructure.Telemetry.Configuration](CIS.Infrastructure.Telemetry.Configuration.md 'CIS.Infrastructure.Telemetry.Configuration')

## LogConfiguration Class

```csharp
public sealed class LogConfiguration
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; LogConfiguration
### Properties

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.Database'></a>

## LogConfiguration.Database Property

Logovani do databaze

```csharp
public CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.MsSqlLogger? Database { get; set; }
```

#### Property Value
[CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.MsSqlLogger](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.MsSqlLogger 'CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.MsSqlLogger')

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.File'></a>

## LogConfiguration.File Property

Logovani do souboru

```csharp
public CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.FileLogger? File { get; set; }
```

#### Property Value
[CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.FileLogger](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.FileLogger 'CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.FileLogger')

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.Seq'></a>

## LogConfiguration.Seq Property

Logovani do Sequ

```csharp
public CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.SeqLogger? Seq { get; set; }
```

#### Property Value
[CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.SeqLogger](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.SeqLogger 'CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.SeqLogger')

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.UseConsole'></a>

## LogConfiguration.UseConsole Property

Logovat output do console

```csharp
public bool UseConsole { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')