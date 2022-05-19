using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacities;

namespace DomainServices.CodebookService.Endpoints.LegalCapacities;

internal class LegalCapacitiesHandler
    : IRequestHandler<LegalCapacitiesRequest, List<GenericCodebookItemWithCode>>
{
    public Task<List<GenericCodebookItemWithCode>> Handle(LegalCapacitiesRequest request, CancellationToken cancellationToken)
    {
        //TODO dodelat na ciselnik od Asseca az bude
        return Task.FromResult(new List<GenericCodebookItemWithCode>
        {
            new GenericCodebookItemWithCode { Id = 1, Name = "Omezená svéprávnost", Code = "D", IsValid = true },
            new GenericCodebookItemWithCode { Id = 2, Name = "Bez omezení", Code = "N", IsValid = true },
            new GenericCodebookItemWithCode { Id = 3, Name = "Jiné omezení", Code = "O", IsValid = true }
        });
    }
}
