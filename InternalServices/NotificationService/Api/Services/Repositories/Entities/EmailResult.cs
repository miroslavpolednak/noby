using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;
using CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;

public class EmailResult : Result
{
    public SenderType SenderType { get; set; }

    public bool Resent { get; set; }

    public override Contracts.Result.Dto.Result ToDto() => this.Map();
}