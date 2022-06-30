using CIS.Foms.Enums;
using DomainServices.CaseService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _CA = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_CA.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));

        // data o SA
        object detailData = await _dataFactory
            .GetService()
            .GetData(saInstance.CaseId, saInstance.OfferId, (SalesArrangementStates)saInstance.State, cancellationToken);

        return new GetDetailResponse()
        {
            ProductTypeId = caseInstance.Data.ProductTypeId,
            SalesArrangementId = saInstance.SalesArrangementId,
            SalesArrangementTypeId = saInstance.SalesArrangementTypeId,
            LoanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId,
            CreatedBy = saInstance.Created.UserName,
            CreatedTime = saInstance.Created.DateTime,
            Data = detailData,
            Parameters = getParameters(saInstance)
        };
    }

    static SalesArrangement.Dto.ParametersMortgage? getParameters(_SA.SalesArrangement saInstance)
        => saInstance.ParametersCase switch
        {
            _SA.SalesArrangement.ParametersOneofCase.Mortgage => new SalesArrangement.Dto.ParametersMortgage
            {
                SignatureTypeId = saInstance.Mortgage.SignatureTypeId,
                ExpectedDateOfDrawing = saInstance.Mortgage.ExpectedDateOfDrawing,
                IncomeCurrencyCode = saInstance.Mortgage.IncomeCurrencyCode,
                ResidencyCurrencyCode = saInstance.Mortgage.ResidencyCurrencyCode,
                Agent = saInstance.Mortgage.Agent,
                LoanRealEstates = saInstance.Mortgage.LoanRealEstates is null ? null : saInstance.Mortgage.LoanRealEstates.Select(x => new SalesArrangement.Dto.LoanRealEstateDto
                {
                    IsCollateral = x.IsCollateral,
                    RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                    RealEstateTypeId = x.RealEstateTypeId
                }).ToList()
            },
            _SA.SalesArrangement.ParametersOneofCase.None => null,
            _ => throw new NotImplementedException("Api/SalesArrangement/GetDetailHandler/getParameters")
        };

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly Services.SalesArrangementDataFactory _dataFactory;
    
    public GetDetailHandler(
        ICaseServiceAbstraction caseService,
        Services.SalesArrangementDataFactory dataFactory,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _caseService = caseService;
        _dataFactory = dataFactory;
        _salesArrangementService = salesArrangementService;
    }
}