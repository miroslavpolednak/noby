namespace FOMS.Api.Endpoints.Codebooks.Handlers;

internal class GetAllHandler
    : IRequestHandler<Dto.GetAllRequest, Dto.GetAllResponse>
{
    public async Task<Dto.GetAllResponse> Handle(Dto.GetAllRequest request, CancellationToken cancellationToken)
    {
        Dto.GetAllResponse model = new();

        foreach (string code in request.CodebookCodes)
            model.Codebooks.Add(await fillCodebook(code));

        return model;
    }

    private async Task<Dto.GetAllResponse.GetAllResponseItem> fillCodebook(string code)
        => code switch
        {
            "productinstancetypes" => new(code, await _codebooks.ProductInstanceTypes()),
            "salesarrangementtypes" => new(code, await _codebooks.SalesArrangementTypes()),

            _ => throw new NotImplementedException($"Codebook code '{code}' is not implemented")
        };

    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebooks;

    public GetAllHandler(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebooks)
    {
        _codebooks = codebooks;
    }
}
