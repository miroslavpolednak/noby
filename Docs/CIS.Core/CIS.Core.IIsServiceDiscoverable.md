#### [CIS.Core](index.md 'index')
### [CIS.Core](CIS.Core.md 'CIS.Core')

## IIsServiceDiscoverable Interface

Rozhraní reprezentující konfigurační objekt, který má možnost pomocí ServiceDiscovery zjistit adresu vlastní služby.

```csharp
public interface IIsServiceDiscoverable
```

### Remarks
Pokud je v projektu přidána reference na ServiceDiscovery (builder.AddCisServiceDiscovery a app.UseServiceDiscovery) a UseServiceDiscovery=true, pokusí se ServiceDiscovery Client načíst URL služby ServiceName.
### Properties

<a name='CIS.Core.IIsServiceDiscoverable.ServiceName'></a>

## IIsServiceDiscoverable.ServiceName Property

Nazev sluzby v ServiceDiscovery

```csharp
string? ServiceName { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Je povinné pokud se má dočítat URL služby ze ServiceDiscovery.

<a name='CIS.Core.IIsServiceDiscoverable.ServiceType'></a>

## IIsServiceDiscoverable.ServiceType Property

Typ služby: 1=gRPC, 2=REST, 3=proprietary

```csharp
int ServiceType { get; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

<a name='CIS.Core.IIsServiceDiscoverable.ServiceUrl'></a>

## IIsServiceDiscoverable.ServiceUrl Property

Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.

```csharp
System.Uri? ServiceUrl { get; set; }
```

#### Property Value
[System.Uri](https://docs.microsoft.com/en-us/dotnet/api/System.Uri 'System.Uri')

<a name='CIS.Core.IIsServiceDiscoverable.UseServiceDiscovery'></a>

## IIsServiceDiscoverable.UseServiceDiscovery Property

If True, then library will try to obtain all needed service URL's from ServiceDiscovery.

```csharp
bool UseServiceDiscovery { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

### Remarks
Default is set to True