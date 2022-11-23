#### [CIS.Core](index.md 'index')
### [CIS.Core.Configuration](CIS.Core.Configuration.md 'CIS.Core.Configuration').[KestrelConfiguration](CIS.Core.Configuration.KestrelConfiguration.md 'CIS.Core.Configuration.KestrelConfiguration')

## KestrelConfiguration.EndpointInfo Class

Nastavení endpointu

```csharp
public sealed class KestrelConfiguration.EndpointInfo
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; EndpointInfo
### Properties

<a name='CIS.Core.Configuration.KestrelConfiguration.EndpointInfo.Port'></a>

## KestrelConfiguration.EndpointInfo.Port Property

Port na kterém endpoint poslouchá

```csharp
public int Port { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Example
30000

<a name='CIS.Core.Configuration.KestrelConfiguration.EndpointInfo.Protocol'></a>

## KestrelConfiguration.EndpointInfo.Protocol Property

Druh protokolu použitý pro daný endpoint

```csharp
public int Protocol { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

### Remarks
1 = HTTP 1.1  
2 = HTTP 2