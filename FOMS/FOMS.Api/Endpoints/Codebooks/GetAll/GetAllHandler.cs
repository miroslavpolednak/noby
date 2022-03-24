using DomainServices.CodebookService.Abstraction;

namespace FOMS.Api.Endpoints.Codebooks.GetAll;

internal class GetAllHandler
    : IRequestHandler<GetAllRequest, List<GetAllResponseItem>>
{
    public async Task<List<GetAllResponseItem>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        List<GetAllResponseItem> model = new();

        _logger.CodebooksGetAllStarted(request.CodebookCodes);

        foreach (var code in request.CodebookCodes)
            model.Add(await fillCodebook(code.Key, code.Original, cancellationToken));
        
        return model;
    }

    //TODO nejak automatizovat? Zase to nechci zpomalovat reflexi.... code generators?
    private async Task<GetAllResponseItem> fillCodebook(string code, string original, CancellationToken cancellationToken)
        => code switch
        {
            "actioncodessavings" => new(original, (await _codebooks.ActionCodesSavings(cancellationToken)).Where(t => t.IsValid)),
            "actioncodessavingsloan" => new(original, (await _codebooks.ActionCodesSavingsLoan(cancellationToken)).Where(t => t.IsValid)),
            "casestates" => new(original, await _codebooks.CaseStates(cancellationToken)),
            "countries" => new(original, await _codebooks.Countries(cancellationToken)),
            "currencies" => new(original, await _codebooks.Currencies(cancellationToken)),
            "customerroles" => new(original, await _codebooks.CustomerRoles(cancellationToken)),
            "fixedrateperiods" => new(original, await _codebooks.FixedRatePeriods(cancellationToken)),
            "genders" => new(original, await _codebooks.Genders(cancellationToken)),
            "identificationdocumenttypes" => new(original, await _codebooks.IdentificationDocumentTypes(cancellationToken)),
            "mandants" => new(original, await _codebooks.Mandants(cancellationToken)),
            "maritalstatuses" => new(original, (await _codebooks.MaritalStatuses(cancellationToken)).Where(t => t.IsValid)),
            "obligationtypes" => new(original, await _codebooks.ObligationTypes(cancellationToken)),
            "producttypes" => new(original, await _codebooks.ProductTypes(cancellationToken)),
            "loanpurposes" => new(original, await _codebooks.LoanPurposes(cancellationToken)),
            "loankinds" => new(original, await _codebooks.LoanKinds(cancellationToken)),
            "salesarrangementtypes" => new(original, await _codebooks.SalesArrangementTypes(cancellationToken)),
            "signaturetypes" => new(original, await _codebooks.SignatureTypes(cancellationToken)),
            "realestatetypes" => new(original, await _codebooks.RealEstateTypes(cancellationToken)),
            "realestatepurchasetypes" => new(original, await _codebooks.RealEstatePurchaseTypes(cancellationToken)),
            
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
