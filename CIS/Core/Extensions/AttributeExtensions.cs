using System.Reflection;

namespace CIS.Core;

/// <summary>
/// Extension metody na C# atributy
/// </summary>
public static class AttributeExtensions
{
    /// <summary>
    /// A generic extension method that aids in reflecting and retrieving any attribute that is applied to an `Enum`.
    /// </summary>
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
    }

    /// <summary>
    /// Vrací informaci o tom, zda daná enum value obsahuje požadovaný atribut.
    /// </summary>
    /// <typeparam name="TAttribute">Typ hledaného atributu</typeparam>
    /// <param name="enumValue">Hodnota enumu</param>
    /// <returns>True, pokud daná hodnota enumu obsahuje TAttribut</returns>
    public static bool HasAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>() is not null;
    }
}