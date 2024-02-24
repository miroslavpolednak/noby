#region legacy code
using CIS.InternalServices.NotificationService.Api.Endpoints.v1;
using CIS.InternalServices.NotificationService.LegacyContracts.Statistics.Dto;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

public class EmailResult : Result
{
    public SenderType SenderType { get; set; }

    public bool Resend { get; set; }

    public override LegacyContracts.Result.Dto.Result ToDto() => this.Map();
}
#endregion legacy code