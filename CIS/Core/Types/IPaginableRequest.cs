namespace CIS.Core.Types;

/// <summary>
/// Obecný request model pro Mediator request podporující stránkování.
/// </summary>
/// <remarks>
/// Používá se pro gRPC i Webapi aplikace.
/// </remarks>
public interface IPaginableRequest
{
    /// <summary>
    /// Velikost jedné stránky - počet záznamů.
    /// </summary>
    /// <example>10</example>
    int PageSize { get; }

    /// <summary>
    /// Offset prvního záznamu vytaženého ze zdrojových dat - zero based.
    /// </summary>
    /// <example>0</example>
    int RecordOffset { get; }

    /// <summary>
    /// Informace o tom, zda aktuální instance requestu obsahuje nastavení řazení záznamů.
    /// </summary>
    bool HasSorting { get; }

    /// <summary>
    /// Typ IPaginableSortingField, který je v aktuální instanci použit.
    /// </summary>
    /// <remarks>Je zde pro překlady IPaginableRequest z/do Webapi vs. gRPC služba.</remarks>
    Type TypeOfSortingField { get; }

    /// <summary>
    /// Vrací seznam polí s nastaveným řazením.
    /// </summary>
    /// <remarks>
    /// Implementace IPaginableSortingField je různá pro gRPC a Webapi služby.
    /// </remarks>
    IEnumerable<IPaginableSortingField>? GetSorting();
}