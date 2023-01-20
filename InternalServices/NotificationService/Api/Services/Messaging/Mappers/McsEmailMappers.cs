using CIS.InternalServices.NotificationService.Contracts.Email.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;

public static class McsEmailMappers
{
    public static IEnumerable<McsSendApi.v4.EmailAddress> MapToMcs(this IEnumerable<EmailAddress> emailAddresses)
    {
        return emailAddresses.Select(MapToMcs);
    }

    public static McsSendApi.v4.EmailAddress MapToMcs(this EmailAddress emailAddress)
    {
        return new ()
        {
            party = emailAddress.Party.MapToMcs(),
            value = emailAddress.Value
        };
    }

    public static McsSendApi.v4.Party MapToMcs(this Party party)
    {
        return new ()
        {
            legalPerson = party.LegalPerson?.MapToMcs(),
            naturalPerson = party.NaturalPerson?.MapToMcs()
        };
    }

    public static McsSendApi.v4.LegalPerson MapToMcs(this LegalPerson legalPerson)
    {
        return new ()
        {
            name = legalPerson.Name 
        };
    }

    public static McsSendApi.v4.NaturalPerson MapToMcs(this NaturalPerson naturalPerson)
    {
        return new ()
        {
            surname = naturalPerson.Surname,
            firstName = naturalPerson.FirstName,
            middleName = naturalPerson.MiddleName
        };
    }

    public static McsSendApi.v4.Content MapToMcs(this EmailContent emailContent)
    {
        return new ()
        {
            charset = "UTF-8",
            format = emailContent.Format,
            language = emailContent.Language,
            text = emailContent.Text
        };
    }

    public static McsSendApi.v4.NotificationConsumer MapToMcs(string consumerId)
    {
        return new()
        {
            consumerId = consumerId
        };
    }
    
    public static McsSendApi.v4.Attachment MapToMcs(string objectKey, string filename)
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