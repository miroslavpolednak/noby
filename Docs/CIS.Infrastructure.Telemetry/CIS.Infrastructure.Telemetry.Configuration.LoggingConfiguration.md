#### [CIS.Infrastructure.Telemetry](index.md 'index')
### [CIS.Infrastructure.Telemetry.Configuration](CIS.Infrastructure.Telemetry.Configuration.md 'CIS.Infrastructure.Telemetry.Configuration')

## LoggingConfiguration Class

```csharp
public sealed class LoggingConfiguration
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; LoggingConfiguration
### Properties

<a name='CIS.Infrastructure.Telemetry.Configuration.LoggingConfiguration.Application'></a>

## LoggingConfiguration.Application Property

Jak se má logovat - nastavení sinků

```csharp
public CIS.Infrastructure.Telemetry.Configuration.LogConfiguration? Application { get; set; }
```

#### Property Value
[LogConfiguration](CIS.Infrastructure.Telemetry.Configuration.LogConfiguration.md 'CIS.Infrastructure.Telemetry.Configuration.LogConfiguration')

<a name='CIS.Infrastructure.Telemetry.Configuration.LoggingConfiguration.IncludeOnlyPaths'></a>

## LoggingConfiguration.IncludeOnlyPaths Property

Pokud je nastaveno, omezuje logování pouze na zadané RequestUrl

```csharp
public System.Collections.Generic.List<string>? IncludeOnlyPaths { get; set; }
```

#### Property Value
[System.Collections.Generic.List&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.List-1 'System.Collections.Generic.List`1')

<a name='CIS.Infrastructure.Telemetry.Configuration.LoggingConfiguration.LogType'></a>

## LoggingConfiguration.LogType Property

Typ logu - gRPC nebo WebApi

```csharp
public CIS.Infrastructure.Telemetry.Configuration.LogBehaviourTypes LogType { get; set; }
```

#### Property Value
[CIS.Infrastructure.Telemetry.Configuration.LogBehaviourTypes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Infrastructure.Telemetry.Configuration.LogBehaviourTypes 'CIS.Infrastructure.Telemetry.Configuration.LogBehaviourTypes')