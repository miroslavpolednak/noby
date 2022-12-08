#### [CIS.Core](index.md 'index')
### [CIS.Core.Configuration](CIS.Core.Configuration.md 'CIS.Core.Configuration')

## ICorsConfiguration Interface

Nastavení CORS pro REST/Webapi projekty.

```csharp
public interface ICorsConfiguration
```
### Properties

<a name='CIS.Core.Configuration.ICorsConfiguration.AllowedOrigins'></a>

## ICorsConfiguration.AllowedOrigins Property

Seznam povolených origins

```csharp
string[]? AllowedOrigins { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[[]](https://docs.microsoft.com/en-us/dotnet/api/System.Array 'System.Array')

### Example
https://localhost:8080, https://dev.noby.cz

<a name='CIS.Core.Configuration.ICorsConfiguration.EnableCors'></a>

## ICorsConfiguration.EnableCors Property

true = zapne CORS middleware v Webapi projektu

```csharp
bool EnableCors { get; set; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')