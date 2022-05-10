namespace Mpss.Rip.Infrastructure.Services.PersonDealer;

public interface IPersonDealerService
{
    Task<PersonDealerExtension> GetUserData(string identity, string identityScheme);
}
