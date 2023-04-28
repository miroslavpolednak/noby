using CIS.Core;

namespace CIS.Infrastructure;

/// <summary>
/// Vrací aktuální lokální čas
/// </summary>
public sealed class LocalDateTime : IDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}