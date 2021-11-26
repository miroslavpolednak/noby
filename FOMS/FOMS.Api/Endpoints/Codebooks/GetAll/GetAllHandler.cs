namespace FOMS.Api.Endpoints.Codebooks.Handlers;

internal class GetAllHandler
    : IRequestHandler<Dto.GetAllRequest, List<Dto.GetAllResponseItem>>
{
    public async Task<List<Dto.GetAllResponseItem>> Handle(Dto.GetAllRequest request, CancellationToken cancellationToken)
    {
        List<Dto.GetAllResponseItem> model = new();

        foreach (var code in request.CodebookCodes)
            model.Add(await fillCodebook(code.Key, code.Original));

        return model;
    }

    private async Task<Dto.GetAllResponseItem> fillCodebook(string code, string original)
        => code switch
        {
            "identificationdocumenttypes" => new(original, await _codebooks.IdentificationDocumentTypes()),
            "persondegreeafter" => new(original, await _codebooks.PersonDegreeAfter()),
            "persondegreebefore" => new(original, await _codebooks.PersonDegreeBefore()),
            "productinstancetypes" => new(original, await _codebooks.ProductInstanceTypes()),
            "salesarrangementtypes" => new(original, await _codebooks.SalesArrangementTypes()),
            "actioncodessavings" => new(original, (await _codebooks.ActionCodesSavings()).Where(t => t.IsActual)),
            "actioncodessavingsloan" => new(original, (await _codebooks.ActionCodesSavingsLoan()).Where(t => t.IsActual)),
            "savingstarif" => new(original, new List<DomainServices.CodebookService.Contracts.GenericCodebookItem> { new DomainServices.CodebookService.Contracts.GenericCodebookItem { Id = 61, Name = "Alfa 1" } }),

            _ => throw new NotImplementedException($"Codebook code '{original}' is not implemented")
        };

    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebooks;

    public GetAllHandler(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebooks)
    {
        _codebooks = codebooks;
    }
}
