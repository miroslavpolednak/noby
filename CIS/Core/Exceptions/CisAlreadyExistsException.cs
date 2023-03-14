namespace CIS.Core.Exceptions;

/// <summary>
/// Objekt již existuje.
/// </summary>
/// <remarks>Např. pokud vytvářím entitu v databázi, ale toto ID již existuje. Nebo pokud přidávám do kolekce již existující klíč.</remarks>
public sealed class CisAlreadyExistsException
    : BaseCisException
{
    /// <summary>
    /// Název entity
    /// </summary>
    /// <example>DomainServices.Api.TestClass</example>
    public string? EntityName { get; private set; }

    /// <summary>
    /// Id entity, která vyvolala vyjímku
    /// </summary>
    /// <example>111</example>
    public object? EntityId { get; private set; }

    /// <param name="exceptionCode">CIS kód chyby</param>
    /// <param name="message">Text chyby</param>
    public CisAlreadyExistsException(int exceptionCode, string message)
        : base(exceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS kód chyby</param>
    /// <param name="entityName">Název entity, která chybu vyvolala</param>
    /// <param name="entityId">ID entity, která chybu vyvolala</param>
    public CisAlreadyExistsException(int exceptionCode, string entityName, object entityId)
        : base(exceptionCode, $"{entityName} {entityId} already exists.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    public object? GetId() => EntityId;
}