namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class ModificationStamp
{
    public ModificationStamp(Core.Data.ICreated entity)
    {
        UserName = entity.CreatedUserName ?? "";
        UserId = entity.CreatedUserId;
        DateTime = entity.CreatedTime;
    }

    public ModificationStamp(int? userId, DateTime dateTime)
    {
        UserId = userId;
        DateTime = dateTime;
    }

    public ModificationStamp(int? userId, string? userName, DateTime dateTime)
    {
        UserName = userName ?? "";
        UserId = userId;
        DateTime = dateTime;
    }
}
