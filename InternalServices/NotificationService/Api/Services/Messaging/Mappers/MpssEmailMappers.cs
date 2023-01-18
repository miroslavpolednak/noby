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
        return new MpssSendApi.v1.EmailAddress
        {
            party = emailAddress.Party.MapToMpss(),
            value = emailAddress.Value
        };
    }

    public static MpssSendApi.v1.Party MapToMpss(this Party party)
    {
        return new MpssSendApi.v1.Party
        {
            legalPerson = party.LegalPerson?.MapToMpss(),
            naturalPerson = party.NaturalPerson?.MapToMpss()
        };
    }

    public static MpssSendApi.v1.LegalPerson MapToMpss(this LegalPerson legalPerson)
    {
        return new MpssSendApi.v1.LegalPerson
        {
            name = legalPerson.Name 
        };
    }

    public static MpssSendApi.v1.NaturalPerson MapToMpss(this NaturalPerson naturalPerson)
    {
        return new MpssSendApi.v1.NaturalPerson
        {
            surname = naturalPerson.Surname,
            firstName = naturalPerson.FirstName,
            middleName = naturalPerson.MiddleName
        };
    }

    public static MpssSendApi.v1.Content MapToMpss(this EmailContent emailContent)
    {
        return new MpssSendApi.v1.Content
        {
            charset = "UTF-8",
            format = emailContent.Format,
            language = emailContent.Language,
            text = emailContent.Text
        };
    }
    
    public static MpssSendApi.v1.Attachment MapToMpss(string objectKey, string filename)
    {
        return new MpssSendApi.v1.Attachment
        {
            s3Content = new MpssSendApi.v1.S3Content
            {
                filename = filename,
                objectKey = objectKey
            }
        };
    }
}