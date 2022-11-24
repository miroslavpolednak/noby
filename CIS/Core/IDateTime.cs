namespace CIS.Core;

/// <summary>
/// Interface pro získání instance aktuálního času z DI.
/// </summary>
/// <remarks>
/// Bylo by užitečné, kdyby servery běželi v UTC, nicméně v aktuálním stavu asi není potřeba.
/// </remarks>
public interface IDateTime
{
    /// <summary>
    /// Current time
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Current time
    /// </summary>
    DateTimeOffset OffsetNow { get; }
}