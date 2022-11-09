namespace CIS.Infrastructure.MediatR.Rollback;

/// <summary>
/// Uloziste umoznujici prenos vybranych dat z Meditr handleru do rollback handleru.
/// </summary>
public interface IRollbackBag
{
    object? this[string key] { get; }

    /// <summary>
    /// Pocet polozek vlozenych do bagu
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Prida dalsi polozku do bagu. Jednotlive polozky jsou nasledne readonly dostupne jako dictionary.
    /// </summary>
    /// <param name="key">Unikatni klic polozky</param>
    /// <param name="value">Hodnota polozky - napr. int32 (id)</param>
    void Add(string key, object value);
}
