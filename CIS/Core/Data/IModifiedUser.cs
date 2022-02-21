namespace CIS.Core.Data;

public interface IModifiedUser
{
    int? ModifiedUserId { get; set; }
    string? ModifiedUserName { get; set; }
}