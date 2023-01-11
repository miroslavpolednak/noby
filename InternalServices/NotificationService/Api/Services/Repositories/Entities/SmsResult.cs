using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;

public class SmsResult : Result
{
    public string Text { get; set; } = null!;
    
    public string CountryCode { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public override Contracts.Result.Dto.Result ToDto() => this.Map();
}