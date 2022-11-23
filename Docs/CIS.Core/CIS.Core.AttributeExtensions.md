#### [CIS.Core](index.md 'index')
### [CIS.Core](CIS.Core.md 'CIS.Core')

## AttributeExtensions Class

Extension metody na C# atributy

```csharp
public static class AttributeExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; AttributeExtensions
### Methods

<a name='CIS.Core.AttributeExtensions.GetAttribute_TAttribute_(thisSystem.Enum)'></a>

## AttributeExtensions.GetAttribute<TAttribute>(this Enum) Method

A generic extension method that aids in reflecting and retrieving any attribute that is applied to an `Enum`.

```csharp
public static TAttribute? GetAttribute<TAttribute>(this System.Enum enumValue)
    where TAttribute : System.Attribute;
```
#### Type parameters

<a name='CIS.Core.AttributeExtensions.GetAttribute_TAttribute_(thisSystem.Enum).TAttribute'></a>

`TAttribute`
#### Parameters

<a name='CIS.Core.AttributeExtensions.GetAttribute_TAttribute_(thisSystem.Enum).enumValue'></a>

`enumValue` [System.Enum](https://docs.microsoft.com/en-us/dotnet/api/System.Enum 'System.Enum')

#### Returns
[TAttribute](CIS.Core.AttributeExtensions.md#CIS.Core.AttributeExtensions.GetAttribute_TAttribute_(thisSystem.Enum).TAttribute 'CIS.Core.AttributeExtensions.GetAttribute<TAttribute>(this System.Enum).TAttribute')

<a name='CIS.Core.AttributeExtensions.HasAttribute_TAttribute_(thisSystem.Enum)'></a>

## AttributeExtensions.HasAttribute<TAttribute>(this Enum) Method

Vrací informaci o tom, zda daná enum value obsahuje požadovaný atribut.

```csharp
public static bool HasAttribute<TAttribute>(this System.Enum enumValue)
    where TAttribute : System.Attribute;
```
#### Type parameters

<a name='CIS.Core.AttributeExtensions.HasAttribute_TAttribute_(thisSystem.Enum).TAttribute'></a>

`TAttribute`

Typ hledaného atributu
#### Parameters

<a name='CIS.Core.AttributeExtensions.HasAttribute_TAttribute_(thisSystem.Enum).enumValue'></a>

`enumValue` [System.Enum](https://docs.microsoft.com/en-us/dotnet/api/System.Enum 'System.Enum')

Hodnota enumu

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')  
True, pokud daná hodnota enumu obsahuje TAttribut