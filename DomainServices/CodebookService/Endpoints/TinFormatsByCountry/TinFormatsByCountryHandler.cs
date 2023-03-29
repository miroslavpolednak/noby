using DomainServices.CodebookService.Contracts.Endpoints.TinFormatsByCountry;
using System.ComponentModel.DataAnnotations;

namespace DomainServices.CodebookService.Endpoints.TinFormatsByCountry;

public class TinFormatsByCountryHandler
    : IRequestHandler<TinFormatsByCountryRequest, List<TinFormatItem>>
{
    public Task<List<TinFormatItem>> Handle(TinFormatsByCountryRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<TinFormatItem>
        {
            new TinFormatItem() { Id = 100, CountryCode = "AD", RegularExpression = "^[EF][-]\\d{6}[-][A-Z]$", IsForFo = true, Tooltip = "8 znaků (1 písmeno+ 6 číslic + 1 kontrolní písmeno), první písmeno vždy F nebo E, písmena jsou od čísel oddělené pomlčkou", IsValid = true},
            new TinFormatItem() { Id = 101, CountryCode = "AD", RegularExpression = "^[ALECDGOPU][-]\\d{6}[-][A-Z]$", IsForFo = false, Tooltip = "8 znaků (1 písmeno+ 6 číslic + 1 kontrolní písmeno), písmena jsou od čísel oddělené pomlčkou", IsValid = true},
        });
    }

    private readonly ILogger<TinFormatsByCountryHandler> _logger;

    public TinFormatsByCountryHandler(
        ILogger<TinFormatsByCountryHandler> logger)
    {
        _logger = logger;
    }
}