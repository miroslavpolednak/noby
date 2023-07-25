using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationTypes;

internal sealed class GetRealEstateValuationTypesHandler
    : IRequestHandler<GetRealEstateValuationTypesRequest, List<int>>
{
    public async Task<List<int>> Handle(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, false, cancellationToken);

        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.GetRealEstateValuationTypesRequest
        {
            LoanAmount = 1,
            DealType = "HYPO",
            RealEstateValuationId = request.RealEstateValuationId
        };
        //dsRequest.LoanPurposes.AddRange();

        var result = await _realEstateValuationService.GetRealEstateValuationTypes(dsRequest, cancellationToken);

        return result.Select(t => (int)t).ToList();
    }

    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetRealEstateValuationTypesHandler(IRealEstateValuationServiceClient realEstateValuationService, ICaseServiceClient caseService)
    {
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
    }
}

