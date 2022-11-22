namespace CIS.Core.Data;

/// <summary>
/// EF entita obsahuje sloupce s informací o uživateli, který ji vytvořil.
/// </summary>
public interface ICreated
{
    /// <summary>
    /// Jméno a příjmení uživatele
    /// </summary>
    string? CreatedUserName { get; set; }

    /// <summary>
    /// v33id uživatele
    /// </summary>
    int? CreatedUserId { get; set; }

    /// <summary>
    /// Datum vytvoření entity
    /// </summary>
    DateTime CreatedTime { get; set; }
}