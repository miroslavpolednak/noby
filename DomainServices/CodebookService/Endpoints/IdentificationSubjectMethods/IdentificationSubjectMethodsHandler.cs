using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IdentificationSubjectMethods;

namespace DomainServices.CodebookService.Endpoints.IdentificationSubjectMethods;

public class IdentificationSubjectMethodsHandler
    : IRequestHandler<IdentificationSubjectMethodsRequest, List<GenericCodebookItem>>
{
    public Task<List<GenericCodebookItem>> Handle(IdentificationSubjectMethodsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<GenericCodebookItem>
        {
            new GenericCodebookItem() { Id = 1, Name = "za fyzické přítomnosti", IsValid = true },
            new GenericCodebookItem() { Id = 3, Name = "ověření notářem, krajským nebo obecním úřadem", IsValid = true },
            new GenericCodebookItem() { Id = 8, Name = "zástupce MPSS", IsValid = true },
        });
    }

    private readonly ILogger<IdentificationSubjectMethodsHandler> _logger;

    public IdentificationSubjectMethodsHandler(
        ILogger<IdentificationSubjectMethodsHandler> logger)
    {
        _logger = logger;
    }
}
