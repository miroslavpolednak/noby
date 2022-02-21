namespace CIS.Core.Data;

public class BaseModifiedUser : IModifiedUser
{
    public int? ModifiedUserId { get; set; }
    public string? ModifiedUserName { get; set; }
}