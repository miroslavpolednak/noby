namespace CIS.Core.Types;

/// <summary>
/// Obecný resopnse model pro Mediator request podporující stránkování.
/// </summary>
public interface IPaginableResponse
{
    /// <summary>
    /// Aktuální velikost stránky - počet záznamů v tomto response.
    /// </summary>
    /// <example>10</example>
    int PageSize { get; }

    /// <summary>
    /// Offset prvního záznamu vytaženého ze zdrojových dat - zero based.
    /// </summary>
    /// <example>0</example>
    int RecordOffset { get; }

    /// <summary>
    /// Celkový počet záznamů nalezených bez ohledu na stránkování.
    /// </summary>
    int RecordsTotalSize { get; }
}