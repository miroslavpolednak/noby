using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients;

public partial interface ICodebookServiceClient
{
    Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> AcademicDegreesAfter(CancellationToken cancellationToken = default(CancellationToken));
}
