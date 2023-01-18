using CIS.InternalServices.NotificationService.Contracts.Common;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;

public static class SmsMappers
{
    public static SendApi.v4.Phone Map(this Phone phone)
    {
        return new SendApi.v4.Phone
        {
            countryCode = phone.CountryCode,
            nationalPhoneNumber = phone.NationalNumber
        };
    }
}