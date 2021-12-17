namespace CIS.Core.Data;

public interface ICreated
{
    int CreatedUserId { get; set; }
    DateTime CreatedTime { get; set; }
}

public class BaseCreated : ICreated
{
    public int CreatedUserId { get; set; }
    public DateTime CreatedTime { get; set; }
}

public class BaseCreatedWithModifiedUserId : BaseCreated, IModifiedUserId
{
    public int ModifiedUserId { get; set; }
}