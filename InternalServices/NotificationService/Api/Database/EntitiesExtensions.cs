using CIS.InternalServices.NotificationService.Contracts.v2;
using SharedComponents.DocumentDataStorage;
using System.Threading;

namespace CIS.InternalServices.NotificationService.Api.Database;

internal static class EntitiesExtensions
{
    public static Contracts.v2.ResultData MapToResultDataV2(this Entities.Notification notification)
    {
        var result = new Contracts.v2.ResultData
        {
            NotificationId = notification.Id.ToString(),
            State = notification.State,
            Channel = notification.Channel,
            CaseId = notification.CaseId,
            CustomId = notification.CustomId,
            DocumentId = notification.DocumentId,
            RequestTimestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(notification.CreatedTime.ToUniversalTime()),
            CreatedBy = notification.CreatedUserName
        };

        // cas ziskani resultu
        if (notification.ResultTime.HasValue)
        {
            result.ResultTimestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(notification.ResultTime.Value.ToUniversalTime());
        }

        // kolekce chyb
        if (notification.Errors is not null)
        {
            result.Errors.AddRange(notification.Errors.Select(t => new Contracts.v2.ResultData.Types.ResultError
            {
                Code = t.Code,
                Message = t.Message
            }));
        }

        // klient
        if (!string.IsNullOrEmpty(notification.Identity) && !string.IsNullOrEmpty(notification.IdentityScheme))
        {
            result.Identifier = new SharedTypes.GrpcTypes.UserIdentity(notification.Identity, Enum.Parse<UserIdentitySchemes>(notification.IdentityScheme));
        }

        // doc hash
        if (!string.IsNullOrEmpty(notification.HashAlgorithm) && !string.IsNullOrEmpty(notification.DocumentHash))
        {
            result.DocumentHash = new Contracts.v2.DocumentHash
            {
                Hash = notification.DocumentHash,
                HashAlgorithm = Enum.Parse<Contracts.v2.DocumentHash.Types.HashAlgorithms>(notification.HashAlgorithm)
            };
        }

        return result;
    }

    public static ResultData.Types.SmsRequestData? MapToSmsResult(this DocumentDataItem<Database.DocumentDataEntities.SmsData, string>? documentEntity)
    {
        if (documentEntity?.Data is null)
        {
            return null;
        }
        else
        {
            return new ResultData.Types.SmsRequestData
            {
                SmsType = documentEntity.Data.SmsType,
                Phone = $"{documentEntity.Data.CountryCode}{documentEntity.Data.NationalNumber}"
            };
        }
    }
}
