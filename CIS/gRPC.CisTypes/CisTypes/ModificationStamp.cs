namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class ModificationStamp
{
    public ModificationStamp(int userId, DateTime dateTime)
    {
        UserId = userId;
        DateTime = dateTime;
    }
}
