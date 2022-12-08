namespace CIS.Core.Attributes;

/// <summary>
/// Marker for DependencyInjection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class TransientServiceAttribute : Attribute
{
}