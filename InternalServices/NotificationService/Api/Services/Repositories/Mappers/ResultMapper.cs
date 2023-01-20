using CIS.InternalServices.NotificationService.Contracts.Common;
using entity = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using dto = CIS.InternalServices.NotificationService.Contracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;

public static class ResultMapper
{
    public static IEnumerable<dto.Result> Map(this IEnumerable<entity.Abstraction.Result> results)
    {
        return results.Select(r => r.ToDto());
    }

    public static Identifier? Map(string? identity, string? identityScheme)
    {
        return string.IsNullOrEmpty(identity) && string.IsNullOrEmpty(identityScheme)
            ? null : new Identifier { Identity = identity, IdentityScheme = identityScheme };
    }
    
    public static dto.Result Map(this entity.SmsResult smsResult)
    {
        return new dto.Result
        {
            NotificationId = smsResult.Id,
            State = smsResult.State,
            Channel = smsResult.Channel,
            Errors = smsResult.ErrorSet.ToList(),
            Identifier = Map(smsResult.Identity, smsResult.IdentityScheme),
            CustomId = smsResult.CustomId,
            DocumentId = smsResult.DocumentId,
            RequestTimestamp = smsResult.RequestTimestamp,
            RequestData = new dto.RequestData
            {
                SmsData = new dto.SmsData
                {
                    Phone = new Phone
                    {
                        CountryCode = smsResult.CountryCode,
                        NationalNumber = smsResult.PhoneNumber
                    },
                    Text = smsResult.Text
                }
            },
            HandoverToMcsTimestamp = smsResult.HandoverToMcsTimestamp,
        };
    }

    public static dto.Result Map(this entity.EmailResult emailResult)
    {
        return new dto.Result
        {
            NotificationId = emailResult.Id,
            State = emailResult.State,
            Channel = emailResult.Channel,
            Errors = emailResult.ErrorSet.ToList(),
            Identifier = Map(emailResult.Identity, emailResult.IdentityScheme),
            CustomId = emailResult.CustomId,
            DocumentId = emailResult.DocumentId,
            RequestTimestamp = emailResult.RequestTimestamp,
            RequestData = new dto.RequestData
            {
                EmailData = new dto.EmailData()
            },
            HandoverToMcsTimestamp = emailResult.HandoverToMcsTimestamp,
        };
    }
}