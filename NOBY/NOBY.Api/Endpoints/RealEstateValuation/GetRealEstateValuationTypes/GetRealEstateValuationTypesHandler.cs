namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationTypes;

internal sealed class GetRealEstateValuationTypesHandler
    : IRequestHandler<GetRealEstateValuationTypesRequest, List<int>>
{
    public async Task<List<int>> Handle(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken)
    {
        var response = await _estateValuationTypeService.GetAllowedTypes(request.RealEstateValuationId, request.CaseId, cancellationToken);
        return response.AllowedTypes;
    }

    private readonly Services.RealEstateValuationType.RealEstateValuationTypeService _estateValuationTypeService;

    public GetRealEstateValuationTypesHandler(Services.RealEstateValuationType.RealEstateValuationTypeService estateValuationTypeService)
    {
        _estateValuationTypeService = estateValuationTypeService;
    }
}

