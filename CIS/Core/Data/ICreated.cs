namespace CIS.Core.Data;

public interface ICreated
{
    string? CreatedUserName { get; set; }
    int CreatedUserId { get; set; }
    DateTime CreatedTime { get; set; }
}

public class BaseCreated : ICreated
{
    public string? CreatedUserName { get; set; }
    public int CreatedUserId { get; set; }
    public DateTime CreatedTime { get; set; }
}

public class BaseCreatedWithModifiedUserId : BaseCreated, IModifiedUser
{
    public int? ModifiedUserId { get; set; }
    public string? ModifiedUserName { get; set; }
}