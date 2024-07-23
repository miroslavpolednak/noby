namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail;

internal static class Mappers
{
    public static McsSendApi.v4.EmailAddress MapToMcs(this Contracts.v2.SendEmailRequest.Types.EmailAddress emailAddress)
    {
        return new()
        {
            party = emailAddress.Party?.LegalPerson is null && emailAddress.Party?.NaturalPerson is null ? null : new()
            {
                legalPerson = emailAddress.Party?.LegalPerson is null ? null : new()
                {
                    name = emailAddress.Party.LegalPerson.Name
                },
                naturalPerson = emailAddress.Party?.NaturalPerson is null ? null : new()
                {
                    surname = emailAddress.Party.NaturalPerson.Surname,
                    firstName = emailAddress.Party.NaturalPerson.FirstName,
                    middleName = emailAddress.Party.NaturalPerson.MiddleName
                }
            },
            value = emailAddress.Value
        };
    }
}
