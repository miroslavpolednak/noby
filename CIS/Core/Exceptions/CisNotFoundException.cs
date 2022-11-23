namespace CIS.Core.Exceptions;

/// <summary>
/// Objekt nebyl nalezen.
/// </summary>
/// <remarks>
/// Např. při dotazu do databáze dané ID neexistuje.
/// </remarks>
public sealed class CisNotFoundException 
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
    public CisNotFoundException(int exceptionCode, string message) 
        : base(exceptionCode, message) 
    { }

    /// <param name="exceptionCode">CIS kód chyby</param>
    /// <param name="entityName">Název entity, která chybu vyvolala</param>
    /// <param name="entityId">ID entity, která chybu vyvolala</param>
    public CisNotFoundException(int exceptionCode, string entityName, object entityId)
        : base(exceptionCode, $"{entityName} {entityId} not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    public object? GetId() => EntityId;
}
