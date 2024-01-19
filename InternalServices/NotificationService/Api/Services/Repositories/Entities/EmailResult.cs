using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;
using CIS.InternalServices.NotificationService.LegacyContracts.Statistics.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;

public class EmailResult : Abstraction.Result
{
    public SenderType SenderType { get; set; }

    public bool Resend { get; set; }

    public override LegacyContracts.Result.Dto.Result ToDto() => this.Map();
}