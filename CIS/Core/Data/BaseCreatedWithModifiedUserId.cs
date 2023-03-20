namespace CIS.Core.Data;

/// <summary>
/// Implementace <see cref="IModifiedUser"/> a <see cref="ICreated"/>
/// </summary>
public class BaseCreatedWithModifiedUserId
    : BaseCreated, IModifiedUser
{
    public int? ModifiedUserId { get; set; }

    public string? ModifiedUserName { get; set; }
}