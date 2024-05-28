using CIS.InternalServices.NotificationService.Contracts.v2;
using Google.Protobuf.Collections;
using SharedComponents.DocumentDataStorage;
using System.Text.Encodings.Web;
using System.Text.Json;

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
            CustomId = notification.CustomId,
            DocumentId = notification.DocumentId,
            RequestTimestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(notification.CreatedTime.ToUniversalTime()),
            CreatedBy = notification.CreatedUserName
        };

        if (!string.IsNullOrEmpty(notification.ProductId) && notification.ProductType != null)
        {
            result.Product = new Product
            {
                ProductId = notification.ProductId,
                ProductType = notification.ProductType ?? default
            };
        }

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
        if (!string.IsNullOrEmpty(notification.Identity) && notification.IdentityScheme != null)
        {
            result.Identifier = new SharedTypes.GrpcTypes.UserIdentity { Identity = notification.Identity, IdentityScheme = notification.IdentityScheme.Value };
        }

        // kolekce hashes
        if (notification.DocumentHashes is not null)
        {
            result.DocumentHashes.AddRange(notification.DocumentHashes.Select(t => new Contracts.v2.DocumentHash
            {
                Hash = t.Hash,
                HashAlgorithm = t.HashAlgorithm
            }));
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

    internal static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}
