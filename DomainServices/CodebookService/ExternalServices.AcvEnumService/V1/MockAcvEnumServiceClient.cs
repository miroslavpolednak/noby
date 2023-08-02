using DomainServices.CodebookService.ExternalServices.AcvEnumService.V1.Contracts;

namespace DomainServices.CodebookService.ExternalServices.AcvEnumService.V1;

internal sealed class MockAcvEnumServiceClient
    : IAcvEnumServiceClient
{
    public Task<List<EnumItemDTO>> GetCategory(Categories category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
