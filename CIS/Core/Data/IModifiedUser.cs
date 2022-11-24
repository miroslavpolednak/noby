namespace CIS.Core.Data;

/// <summary>
/// EF entita obsahuje sloupce s informací o uživateli, který ji naposledy updatoval.
/// </summary>
public interface IModifiedUser
{
    /// <summary>
    /// v33id uživatele
    /// </summary>
    int? ModifiedUserId { get; set; }

    /// <summary>
    /// Jméno a příjmení uživatele
    /// </summary>
    string? ModifiedUserName { get; set; }
}