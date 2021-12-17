namespace CIS.Core.Data;

public interface IModifiedUserId
{
    int ModifiedUserId { get; set; }
}

public class BaseModifiedUserId : IModifiedUserId
{
    public int ModifiedUserId { get; set; }
}