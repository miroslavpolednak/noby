using System.Reflection;

namespace DomainServices.CodebookService.Endpoints;

internal static class Extensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>()!;
    }
}