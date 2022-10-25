using CIS.InternalServices.NotificationService.Contracts.Email.Dto;

namespace CIS.InternalServices.NotificationService.Api.Mappers;

public static class EmailMappers
{
    public static IEnumerable<SendApi.v2.EmailAddress> Map(this IEnumerable<EmailAddress> emailAddresses)
    {
        return emailAddresses.Select(Map);
    }

    public static SendApi.v2.EmailAddress Map(this EmailAddress emailAddress)
    {
        return new SendApi.v2.EmailAddress
        {
            party = emailAddress.Party.Map(),
            value = emailAddress.Value
        };
    }

    public static SendApi.v2.Party Map(this Party party)
    {
        return new SendApi.v2.Party
        {
            legalPerson = party.LegalPerson?.Map(),
            naturalPerson = party.NaturalPerson?.Map()
        };
    }

    public static SendApi.v2.LegalPerson Map(this LegalPerson legalPerson)
    {
        return new SendApi.v2.LegalPerson
        {
            name = legalPerson.Name 
        };
    }

    public static SendApi.v2.NaturalPerson Map(this NaturalPerson naturalPerson)
    {
        return new SendApi.v2.NaturalPerson
        {
            surname = naturalPerson.Surname,
            firstName = naturalPerson.FirstName,
            middleName = naturalPerson.MiddleName
        };
    }

    public static SendApi.v2.Content Map(this EmailContent emailContent)
    {
        return new SendApi.v2.Content
        {
            charset = "UTF-8",
            format = emailContent.Format,
            language = emailContent.Language,
            text = emailContent.Text
        };
    }

    // todo:
    public static SendApi.v2.Attachment Map(this EmailAttachment emailAttachment)
    {
        return new SendApi.v2.Attachment
        {
            s3Content = new SendApi.v2.S3Content
            {
                filename = "",
                objectKey = ""
            }
        };
    }
}