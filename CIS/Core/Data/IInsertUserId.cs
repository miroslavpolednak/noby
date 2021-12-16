namespace CIS.Core.Data;

public interface IInsertUserId
{
    int InsertUserId { get; set; }
}

public class BaseInsertUserId : IInsertUserId
{
    public int InsertUserId { get; set; }
}