using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;

public static class SmsMappers
{
    public static SendApi.v1.Phone Map(this Phone phone)
    {
        return new SendApi.v1.Phone
        {
            countryCode = phone.CountryCode,
            nationalPhoneNumber = phone.NationalNumber
        };
    }
}