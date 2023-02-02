using DomainServices.CodebookService.Contracts.Endpoints.TinNoFillReasonsByCountry;

namespace DomainServices.CodebookService.Endpoints.TinNoFillReasonsByCountry;

public class TinNoFillReasonsByCountryHandler
    : IRequestHandler<TinNoFillReasonsByCountryRequest, List<TinNoFillReasonItem>>
{
    public Task<List<TinNoFillReasonItem>> Handle(TinNoFillReasonsByCountryRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<TinNoFillReasonItem>
        {
            new TinNoFillReasonItem() { Id = "AD", IsTinMandatory = true, ReasonForBlankTin = "TIN není možné získat", IsValid = true},
            new TinNoFillReasonItem() { Id = "AE", IsTinMandatory = false, ReasonForBlankTin = "Země TIN nevydává", IsValid = true},
        });
    }

    private readonly ILogger<TinNoFillReasonsByCountryHandler> _logger;

    public TinNoFillReasonsByCountryHandler(
        ILogger<TinNoFillReasonsByCountryHandler> logger)
    {
        _logger = logger;
    }
}
