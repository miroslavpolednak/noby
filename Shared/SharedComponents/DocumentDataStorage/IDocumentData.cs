namespace SharedComponents.DocumentDataStorage;

/// <summary>
/// Interface, který musí implementovat každá entita, která se má ukládat do databáze
/// </summary>
public interface IDocumentData
{
    /// <summary>
    /// Verze kontraktu, jedná se o manuálně udržované číslo začínající 1
    /// </summary>
    int Version { get; }
}
