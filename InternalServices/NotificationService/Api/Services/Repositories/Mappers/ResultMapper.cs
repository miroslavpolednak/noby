using CIS.InternalServices.NotificationService.Contracts.Common;
using Entity = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using Dto = CIS.InternalServices.NotificationService.Contracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;

public static class ResultMapper
{
    public static IEnumerable<Dto.Result> Map(this IEnumerable<Entity.Abstraction.Result> results)
    {
        return results.Select(r => r.ToDto());
    }

    public static Identifier? Map(string? identity, string? identityScheme)
    {
        return string.IsNullOrEmpty(identity) && string.IsNullOrEmpty(identityScheme)
            ? null : new Identifier { Identity = identity, IdentityScheme = identityScheme };
    }
    
    public static Dto.Result Map(this Entity.SmsResult smsResult)
    {
        return new Dto.Result
        {
            NotificationId = smsResult.Id,
            State = smsResult.State,
            Channel = smsResult.Channel,
            Errors = smsResult.ErrorSet.ToList(),
            Identifier = Map(smsResult.Identity, smsResult.IdentityScheme),
            CustomId = smsResult.CustomId,
            DocumentId = smsResult.DocumentId,
            RequestTimestamp = smsResult.RequestTimestamp,
            RequestData = new Dto.RequestData
            {
                SmsData = new Dto.SmsData
                {
                    Phone = new Phone
                    {
                        CountryCode = smsResult.CountryCode,
                        NationalNumber = smsResult.PhoneNumber
                    },
                    Type = smsResult.Type,
                    Text = smsResult.Text
                }
            },
            ResultTimestamp = smsResult.ResultTimestamp,
            CreatedBy = smsResult.CreatedBy
        };
    }

    public static Dto.Result Map(this Entity.EmailResult emailResult)
    {
        return new Dto.Result
        {
            NotificationId = emailResult.Id,
            State = emailResult.State,
            Channel = emailResult.Channel,
            Errors = emailResult.ErrorSet.ToList(),
            Identifier = Map(emailResult.Identity, emailResult.IdentityScheme),
            CustomId = emailResult.CustomId,
            DocumentId = emailResult.DocumentId,
            RequestTimestamp = emailResult.RequestTimestamp,
            RequestData = new Dto.RequestData
            {
                EmailData = new Dto.EmailData()
            },
            ResultTimestamp = emailResult.ResultTimestamp,
            CreatedBy = emailResult.CreatedBy
        };
    }
}