using DomainServices.CodebookService.ExternalServices.RDM.V1.Contracts;

namespace DomainServices.CodebookService.ExternalServices.RDM.V1;

internal sealed class MockRDMClient
    : IRDMClient
{
    public Task<List<GetCodebookResponse_CodebookEntry>> GetCodebookItems(string codebookCode, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Contracts.GetCodebookMappingResponse_CodebookEntryMapping>> GetMappingItems(string codebookCode, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
