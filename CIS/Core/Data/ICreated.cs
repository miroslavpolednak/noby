namespace CIS.Core.Data;

public interface ICreated
{
    string? CreatedUserName { get; set; }
    int? CreatedUserId { get; set; }
    DateTime CreatedTime { get; set; }
}