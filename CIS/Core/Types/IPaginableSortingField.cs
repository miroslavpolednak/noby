namespace CIS.Core.Types;

/// <summary>
/// Nastavení jednoho pole pro řazení stránkovacích requestů.
/// </summary>
public interface IPaginableSortingField
{
    /// <summary>
    /// Název pole.
    /// </summary>
    /// <remarks>
    /// Nemusí se shodovat s názvem databázového pole.
    /// </remarks>
    string Field { get; }

    /// <summary>
    /// Nastavení řazení sestupně nebo vzestupně.
    /// </summary>
    bool Descending { get; }
}