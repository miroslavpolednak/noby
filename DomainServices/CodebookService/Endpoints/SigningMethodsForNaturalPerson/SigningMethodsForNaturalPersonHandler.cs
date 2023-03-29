using DomainServices.CodebookService.Contracts.Endpoints.SigningMethodsForNaturalPerson;

namespace DomainServices.CodebookService.Endpoints.SigningMethodsForNaturalPerson;

public class SigningMethodsForNaturalPersonHandler
    : IRequestHandler<SigningMethodsForNaturalPersonRequest, List<SigningMethodsForNaturalPersonItem>>
{
    public Task<List<SigningMethodsForNaturalPersonItem>> Handle(SigningMethodsForNaturalPersonRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<SigningMethodsForNaturalPersonItem>
        {
            new SigningMethodsForNaturalPersonItem() { Code = "OFFERED", Order = 4, Name = "Delegovaná metoda podpisu", Description = "deprecated", IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "PHYSICAL", Order = 1, Name = "Ruční podpis", Description = "Fyzický/ruční podpis dokumentu.", IsValid = true, StarbuildEnumId = 1 },
            new SigningMethodsForNaturalPersonItem() { Code = "DELEGATE", Order = 1, Name = "Přímé bankovnictví", Description = "Přímé bankovnictví - Delegovaná metoda podpisu", IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "PAAT", Order = 1, Name = "KB klíč", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "INT_CERT_FILE", Order = 2, Name = "Interní certifikát v souboru", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "APOC", Order = 3, Name = "Automatizovaný Podpis Osobním Certifikátem", Description = null, IsValid = true, StarbuildEnumId = 2 },
        });
    }


    private readonly ILogger<SigningMethodsForNaturalPersonHandler> _logger;

    public SigningMethodsForNaturalPersonHandler(
        ILogger<SigningMethodsForNaturalPersonHandler> logger)
    {
        _logger = logger;
    }
}
