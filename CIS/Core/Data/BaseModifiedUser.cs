namespace CIS.Core.Data;

/// <summary>
/// Implementace <see cref="IModifiedUser"/>
/// </summary>
public class BaseModifiedUser
    : IModifiedUser
{
    public int? ModifiedUserId { get; set; }

    public string? ModifiedUserName { get; set; }
}