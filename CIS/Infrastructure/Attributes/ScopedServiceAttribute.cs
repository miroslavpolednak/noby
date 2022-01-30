namespace CIS.Infrastructure.Attributes;

/// <summary>
/// Marker for DependencyInjection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ScopedServiceAttribute : Attribute
{
}
