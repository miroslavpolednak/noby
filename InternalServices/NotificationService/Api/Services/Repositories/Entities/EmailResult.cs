using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;

public class EmailResult : Result
{
    public override Contracts.Result.Dto.Result ToDto() => this.Map();
}