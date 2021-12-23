namespace CIS.Core.Data;

public interface IModifiedUser
{
    int? ModifiedUserId { get; set; }
    string? ModifiedUserName { get; set; }
}

public class BaseModifiedUser : IModifiedUser
{
    public int? ModifiedUserId { get; set; }
    public string? ModifiedUserName { get; set; }
}