using CIS.InternalServices.NotificationService.Contracts.Email.Dto;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;

public static class EmailMappers
{
    public static IEnumerable<SendApi.v4.EmailAddress> Map(this IEnumerable<EmailAddress> emailAddresses)
    {
        return emailAddresses.Select(Map);
    }

    public static SendApi.v4.EmailAddress Map(this EmailAddress emailAddress)
    {
        return new SendApi.v4.EmailAddress
        {
            party = emailAddress.Party.Map(),
            value = emailAddress.Value
        };
    }

    public static SendApi.v4.Party Map(this Party party)
    {
        return new SendApi.v4.Party
        {
            legalPerson = party.LegalPerson?.Map(),
            naturalPerson = party.NaturalPerson?.Map()
        };
    }

    public static SendApi.v4.LegalPerson Map(this LegalPerson legalPerson)
    {
        return new SendApi.v4.LegalPerson
        {
            name = legalPerson.Name 
        };
    }

    public static SendApi.v4.NaturalPerson Map(this NaturalPerson naturalPerson)
    {
        return new SendApi.v4.NaturalPerson
        {
            surname = naturalPerson.Surname,
            firstName = naturalPerson.FirstName,
            middleName = naturalPerson.MiddleName
        };
    }

    public static SendApi.v4.Content Map(this EmailContent emailContent)
    {
        return new SendApi.v4.Content
        {
            charset = "UTF-8",
            format = emailContent.Format,
            language = emailContent.Language,
            text = emailContent.Text
        };
    }
    
    public static SendApi.v4.Attachment Map(string filename, string objectKey)
    {
        return new SendApi.v4.Attachment
        {
            s3Content = new SendApi.v4.S3Content
            {
                filename = filename,
                objectKey = objectKey
            }
        };
    }
}