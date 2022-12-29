using CIS.InternalServices.NotificationService.Contracts.Common;
using entity = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using dto = CIS.InternalServices.NotificationService.Contracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;

public static class ResultMapper
{
    public static IEnumerable<dto.Abstraction.Result> Map(this IEnumerable<entity.Abstraction.Result> results)
    {
        return results.Select(r => r.ToDto());
    }

    public static dto.Abstraction.Result Map(this entity.SmsResult smsResult)
    {
        return new dto.SmsResult
        {
            NotificationId = smsResult.Id,
            State = smsResult.State,
            Channel = smsResult.Channel,
            Errors = smsResult.ErrorSet.ToList(),
            Identifier = new Identifier
            {
                Identity = smsResult.Identity ?? "",
                IdentityScheme = smsResult.IdentityScheme ?? ""
            },
            CustomId = smsResult.CustomId ?? "",
            DocumentId = smsResult.DocumentId ?? "",
            RequestTimestamp = smsResult.RequestTimestamp ?? default,
            HandoverToMcsTimestamp = smsResult.HandoverToMcsTimestamp ?? default,
            Phone = new Phone
            {
                CountryCode = smsResult.CountryCode,
                NationalNumber = smsResult.PhoneNumber
            },
            Text = smsResult.Text,
        };
    }

    public static dto.Abstraction.Result Map(this entity.EmailResult emailResult)
    {
        return new dto.EmailResult
        {
            NotificationId = emailResult.Id,
            State = emailResult.State,
            Channel = emailResult.Channel,
            Errors = emailResult.ErrorSet.ToList(),
            Identifier = new Identifier
            {
                Identity = emailResult.Identity ?? "",
                IdentityScheme = emailResult.IdentityScheme ?? ""
            },
            CustomId = emailResult.CustomId ?? "",
            DocumentId = emailResult.DocumentId ?? "",
            RequestTimestamp = emailResult.RequestTimestamp ?? default,
            HandoverToMcsTimestamp = emailResult.HandoverToMcsTimestamp ?? default
        };
    }
}