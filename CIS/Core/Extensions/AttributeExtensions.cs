using System.Reflection;

namespace CIS.Core
{
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
        
        public static bool HasAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>() is not null;
        }
    }
}
