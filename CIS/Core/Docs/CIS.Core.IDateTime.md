#### [CIS.Core](index.md 'index')
### [CIS.Core](CIS.Core.md 'CIS.Core')

## IDateTime Interface

Interface pro získání instance aktuálního času z DI.

```csharp
public interface IDateTime
```

### Remarks
Bylo by užitečné, kdyby servery běželi v UTC, nicméně v aktuálním stavu asi není potřeba.
### Properties

<a name='CIS.Core.IDateTime.Now'></a>

## IDateTime.Now Property

Current time

```csharp
System.DateTime Now { get; }
```

#### Property Value
[System.DateTime](https://docs.microsoft.com/en-us/dotnet/api/System.DateTime 'System.DateTime')

<a name='CIS.Core.IDateTime.OffsetNow'></a>

## IDateTime.OffsetNow Property

Current time

```csharp
System.DateTimeOffset OffsetNow { get; }
```

#### Property Value
[System.DateTimeOffset](https://docs.microsoft.com/en-us/dotnet/api/System.DateTimeOffset 'System.DateTimeOffset')