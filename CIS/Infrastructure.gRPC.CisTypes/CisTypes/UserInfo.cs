namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class UserInfo
{
    public UserInfo(int userId, string? userName)
    {
        UserId = userId;
        UserName = userName ?? "";
    }
}
