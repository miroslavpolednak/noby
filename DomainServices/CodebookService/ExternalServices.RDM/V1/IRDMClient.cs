using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CodebookService.ExternalServices.RDM.V1;

public interface IRDMClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<List<Contracts.GetCodebookResponse_CodebookEntry>> GetCodebookItems(string codebookCode, CancellationToken cancellationToken = default);

    Task<List<T>> GetCustomCodebookItems<T>(string codebookCode, CancellationToken cancellationToken = default)
        where T : class, new();
}
