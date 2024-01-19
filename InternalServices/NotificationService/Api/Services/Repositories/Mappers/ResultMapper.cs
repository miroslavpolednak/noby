using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using Entity = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using Dto = CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Mappers;

public static class ResultMapper
{
    public static IEnumerable<Dto.Result> Map(this IEnumerable<Entity.Abstraction.Result> results)
    {
        return results.Select(r => r.ToDto());
    }

    public static Identifier? MapIdentifier(string? identity, string? identityScheme)
    {
        return string.IsNullOrEmpty(identity) && string.IsNullOrEmpty(identityScheme)
            ? null : new Identifier { Identity = identity ?? string.Empty, IdentityScheme = identityScheme ?? string.Empty };
    }

    public static DocumentHash? MapDocumentHash(string? hash, string? hashAlgorithm)
    {
        return string.IsNullOrEmpty(hash) && string.IsNullOrEmpty(hashAlgorithm)
            ? null : new DocumentHash { Hash = hash ?? string.Empty, HashAlgorithm = hashAlgorithm ?? string.Empty };
    }
    
    public static Dto.Result Map(this Entity.SmsResult smsResult)
    {
        return new Dto.Result
        {
            NotificationId = smsResult.Id,
            State = smsResult.State,
            Channel = smsResult.Channel,
            Errors = smsResult.ErrorSet.ToList(),
            Identifier = MapIdentifier(smsResult.Identity, smsResult.IdentityScheme),
            CaseId = smsResult.CaseId,
            CustomId = smsResult.CustomId,
            DocumentId = smsResult.DocumentId,
            DocumentHash = MapDocumentHash(smsResult.DocumentHash, smsResult.HashAlgorithm),
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
                    Type = smsResult.Type
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
            Identifier = MapIdentifier(emailResult.Identity, emailResult.IdentityScheme),
            CaseId = emailResult.CaseId,
            CustomId = emailResult.CustomId,
            DocumentId = emailResult.DocumentId,
            DocumentHash = MapDocumentHash(emailResult.DocumentHash, emailResult.HashAlgorithm),
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