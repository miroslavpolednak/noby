using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients.Services;

internal partial class CodebookService
    : ICodebookServiceClient
{
    public async Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> AcademicDegreesAfter(CancellationToken cancellationToken = default(CancellationToken))
        => await _cache.GetOrCreate(async () => (await _service.AcademicDegreesAfterAsync(new Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: cancellationToken)).Items.ToList());

    private readonly ClientsMemoryCache _cache;
    private readonly Contracts.v1.CodebookService.CodebookServiceClient _service;

    public CodebookService(Contracts.v1.CodebookService.CodebookServiceClient service, ClientsMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }
        
}
