namespace FOMS.Api.Endpoints.Codebooks.Handlers;

internal class GetAllHandler
    : IRequestHandler<Dto.GetAllRequest, List<Dto.GetAllResponseItem>>
{
    public async Task<List<Dto.GetAllResponseItem>> Handle(Dto.GetAllRequest request, CancellationToken cancellationToken)
    {
        List<Dto.GetAllResponseItem> model = new();

        _logger.LogDebug("Getting {codebooks}", request.CodebookCodes);

        foreach (var code in request.CodebookCodes)
            model.Add(await fillCodebook(code.Key, code.Original));

        _logger.LogDebug("{count} codebooks resolved", model.Count());

        return model;
    }

    //TODO nejak automatizovat? Zase to nechci zpomalovat reflexi.... code generators?
    private async Task<Dto.GetAllResponseItem> fillCodebook(string code, string original)
        => code switch
        {
            "actioncodessavings" => new(original, (await _codebooks.ActionCodesSavings()).Where(t => t.IsValid)),
            "actioncodessavingsloan" => new(original, (await _codebooks.ActionCodesSavingsLoan()).Where(t => t.IsValid)),
            "casestates" => new(original, await _codebooks.CaseStates()),
            "countries" => new(original, await _codebooks.Countries()),
            "fixedlengthperiods" => new(original, await _codebooks.FixedLengthPeriods()),
            "genders" => new(original, await _codebooks.Genders()),
            "identificationdocumenttypes" => new(original, await _codebooks.IdentificationDocumentTypes()),
            //"nationalities" => new(original, await _codebooks.Nationalities()),//!!!
            //"persondegreeafter" => new(original, await _codebooks.PersonDegreeAfter()),//!!!
            //"persondegreebefore" => new(original, await _codebooks.PersonDegreeBefore()),//!!!
            "producttypes" => new(original, await _codebooks.ProductTypes()),
            "productloanpurposes" => new(original, await _codebooks.ProductLoanPurposes()),
            "productloankinds" => new(original, await _codebooks.ProductLoanKinds()),
            //"residencytypes" => new(original, await _codebooks.ResidencyTypes()),//!!!
            "salesarrangementtypes" => new(original, await _codebooks.SalesArrangementTypes()),
            //"mktactioncodessavings" => new(original, (await _codebooks.MktActionCodesSavings())),//!!!

            _ => throw new NotImplementedException($"Codebook code '{original}' is not implemented")
        };

    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebooks;
    private readonly ILogger<GetAllHandler> _logger;

    public GetAllHandler(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebooks, ILogger<GetAllHandler> logger)
    {
        _logger = logger;
        _codebooks = codebooks;
    }
}
