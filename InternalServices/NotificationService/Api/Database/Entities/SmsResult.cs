using CIS.InternalServices.NotificationService.Api.Endpoints.v1;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

public class SmsResult : Result
{
    public string Type { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public override LegacyContracts.Result.Dto.Result ToDto() => this.Map();
}