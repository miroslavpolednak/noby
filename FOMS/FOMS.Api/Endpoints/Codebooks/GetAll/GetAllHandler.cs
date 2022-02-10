using DomainServices.CodebookService.Abstraction;

namespace FOMS.Api.Endpoints.Codebooks.Handlers;

internal class GetAllHandler
    : IRequestHandler<Dto.GetAllRequest, List<Dto.GetAllResponseItem>>
{
    public async Task<List<Dto.GetAllResponseItem>> Handle(Dto.GetAllRequest request, CancellationToken cancellationToken)
    {
        List<Dto.GetAllResponseItem> model = new();

        _logger.CodebooksGetAllStarted(request.CodebookCodes);

        foreach (var code in request.CodebookCodes)
            model.Add(await fillCodebook(code.Key, code.Original, cancellationToken));
        
        return model;
    }

    //TODO nejak automatizovat? Zase to nechci zpomalovat reflexi.... code generators?
    private async Task<Dto.GetAllResponseItem> fillCodebook(string code, string original, CancellationToken cancellationToken)
        => code switch
        {
            "actioncodessavings" => new(original, (await _codebooks.ActionCodesSavings(cancellationToken)).Where(t => t.IsValid)),
            "actioncodessavingsloan" => new(original, (await _codebooks.ActionCodesSavingsLoan(cancellationToken)).Where(t => t.IsValid)),
            "casestates" => new(original, await _codebooks.CaseStates(cancellationToken)),
            "countries" => new(original, await _codebooks.Countries(cancellationToken)),
            "fixedlengthperiods" => new(original, await _codebooks.FixedLengthPeriods(cancellationToken)),
            "genders" => new(original, await _codebooks.Genders(cancellationToken)),
            "identificationdocumenttypes" => new(original, await _codebooks.IdentificationDocumentTypes(cancellationToken)),
            "mandants" => new(original, await _codebooks.Mandants(cancellationToken)),
            "producttypes" => new(original, await _codebooks.ProductTypes(cancellationToken)),
            "productloanpurposes" => new(original, await _codebooks.ProductLoanPurposes(cancellationToken)),
            "productloankinds" => new(original, await _codebooks.ProductLoanKinds(cancellationToken)),
            "salesarrangementtypes" => new(original, await _codebooks.SalesArrangementTypes(cancellationToken)),
            "signaturetypes" => new(original, await _codebooks.SignatureTypes(cancellationToken)),
            
            //"residencytypes" => new(original, await _codebooks.ResidencyTypes()),//!!!
            //"mktactioncodessavings" => new(original, (await _codebooks.MktActionCodesSavings())),//!!!
            //"nationalities" => new(original, await _codebooks.Nationalities()),//!!!
            //"persondegreeafter" => new(original, await _codebooks.PersonDegreeAfter()),//!!!
            //"persondegreebefore" => new(original, await _codebooks.PersonDegreeBefore()),//!!!

            _ => throw new NotImplementedException($"Codebook code '{original}' is not implemented")
        };

    private readonly ICodebookServiceAbstraction _codebooks;
    private readonly ILogger<GetAllHandler> _logger;

    public GetAllHandler(ICodebookServiceAbstraction codebooks, ILogger<GetAllHandler> logger)
    {
        _logger = logger;
        _codebooks = codebooks;
    }
}
