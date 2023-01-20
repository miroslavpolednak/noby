using CIS.InternalServices.NotificationService.Contracts.Email.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;

public static class MpssEmailMappers
{
    public static IEnumerable<MpssSendApi.v1.EmailAddress> MapToMpss(this IEnumerable<EmailAddress> emailAddresses)
    {
        return emailAddresses.Select(MapToMpss);
    }

    public static MpssSendApi.v1.EmailAddress MapToMpss(this EmailAddress emailAddress)
    {
        return new ()
        {
            party = emailAddress.Party.MapToMpss(),
            value = emailAddress.Value
        };
    }

    public static MpssSendApi.v1.Party MapToMpss(this Party party)
    {
        return new ()
        {
            legalPerson = party.LegalPerson?.MapToMpss(),
            naturalPerson = party.NaturalPerson?.MapToMpss()
        };
    }

    public static MpssSendApi.v1.LegalPerson MapToMpss(this LegalPerson legalPerson)
    {
        return new ()
        {
            name = legalPerson.Name 
        };
    }

    public static MpssSendApi.v1.NaturalPerson MapToMpss(this NaturalPerson naturalPerson)
    {
        return new ()
        {
            surname = naturalPerson.Surname,
            firstName = naturalPerson.FirstName,
            middleName = naturalPerson.MiddleName
        };
    }

    public static MpssSendApi.v1.Content MapToMpss(this EmailContent emailContent)
    {
        return new ()
        {
            charset = "UTF-8",
            format = emailContent.Format,
            language = emailContent.Language,
            text = emailContent.Text
        };
    }

    public static MpssSendApi.v1.NotificationConsumer MapToMpss(string consumerId)
    {
        return new()
        {
            consumerId = consumerId
        };
    }
    
    public static MpssSendApi.v1.Attachment MapToMpss(string objectKey, string filename)
    {
        return new ()
        {
            s3Content = new ()
            {
                filename = filename,
                objectKey = objectKey
            }
        };
    }
}