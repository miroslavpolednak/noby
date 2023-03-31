using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.SignatureStatesNoby;


namespace DomainServices.CodebookService.Endpoints.SignatureStatesNoby;

public class SignatureStatesNobyHandler
    : IRequestHandler<SignatureStatesNobyRequest, List<GenericCodebookItem>>
{
    public Task<List<GenericCodebookItem>> Handle(SignatureStatesNobyRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<GenericCodebookItem>
        {
            new GenericCodebookItem() { Id = 1, Name ="připraveno", IsValid = true},
            new GenericCodebookItem() { Id = 2, Name ="v procesu", IsValid = true},
            new GenericCodebookItem() { Id = 3, Name ="čeká na sken", IsValid = true},
            new GenericCodebookItem() { Id = 4, Name ="podepsáno", IsValid = true},
            new GenericCodebookItem() { Id = 5, Name ="zrušeno", IsValid = true},
        });
    }

    private readonly ILogger<SignatureStatesNobyHandler> _logger;

    public SignatureStatesNobyHandler(ILogger<SignatureStatesNobyHandler> logger)
    {
        _logger = logger;
    }
}
