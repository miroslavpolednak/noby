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
[FileLogger](CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.FileLogger.md 'CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.FileLogger')

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.LogRequestPayload'></a>

## LogConfiguration.LogRequestPayload Property

True = do logu se ulozi plny request payload sluzby

```csharp
public bool LogRequestPayload { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.LogResponsePayload'></a>

## LogConfiguration.LogResponsePayload Property

True = do logu se ulozi plny response payload sluzby

```csharp
public bool LogResponsePayload { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.MaxPayloadLength'></a>

## LogConfiguration.MaxPayloadLength Property

Maximální velikost obsahu vlastnosti Payload

```csharp
public System.Nullable<int> MaxPayloadLength { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

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