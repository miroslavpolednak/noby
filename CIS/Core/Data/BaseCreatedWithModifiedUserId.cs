namespace CIS.Core.Data;

public class BaseCreatedWithModifiedUserId : BaseCreated, IModifiedUser
{
    public int? ModifiedUserId { get; set; }
    public string? ModifiedUserName { get; set; }
}