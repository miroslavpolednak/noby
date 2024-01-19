﻿using CIS.InternalServices.NotificationService.LegacyContracts.Common;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Mappers;

public static class McsSmsMappers
{
    public static McsSendApi.v4.Phone Map(this Phone phone)
    {
        return new McsSendApi.v4.Phone
        {
            countryCode = phone.CountryCode,
            nationalPhoneNumber = phone.NationalNumber
        };
    }
    
    public static McsSendApi.v4.NotificationConsumer MapToMcs(string consumerId)
    {
        return new()
        {
            consumerId = consumerId
        };
    }
}