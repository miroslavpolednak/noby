namespace CIS.Core.Data;

/// <summary>
/// EF entita obsahuje sloupec IsActual.
/// </summary>
/// <remarks>
/// IsActual se používá pro soft-delete entit. False znamená smazanou entitu.
/// </remarks>
public interface IIsActual
{
    bool IsActual { get; set; }
}