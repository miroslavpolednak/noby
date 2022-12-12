namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class UserInfo
{
    public UserInfo(int userId, string? userName)
    {
        UserId = userId;
        UserName = userName ?? "";
    }
}
