namespace SharedTypes.GrpcTypes;

public partial class UserInfo
{
    public UserInfo(int userId, string? userName)
    {
        UserId = userId;
        UserName = userName ?? "";
    }
}
