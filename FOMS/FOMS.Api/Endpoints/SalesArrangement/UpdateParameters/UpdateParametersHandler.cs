using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.UpdateParameters;

internal class UpdateParametersHandler
    : AsyncRequestHandler<UpdateParametersRequest>
{
    protected override async Task Handle(UpdateParametersRequest request, CancellationToken cancellationToken)
    {
        var model = new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            Mortgage = new _SA.SalesArrangementParametersMortgage
            {
                ContractSignatureTypeId = request.Parameters?.ContractSignatureTypeId,
                SalesArrangementSignatureTypeId = request.Parameters?.SalesArrangementSignatureTypeId,
                ExpectedDateOfDrawing = request.Parameters?.ExpectedDateOfDrawing,
                IncomeCurrencyCode = request.Parameters?.IncomeCurrencyCode,
                ResidencyCurrencyCode = request.Parameters?.ResidencyCurrencyCode,
                Agent = request.Parameters?.Agent,
                AgentConsentWithElCom = request.Parameters?.AgentConsentWithElCom,
            }
        };

        if (request.Parameters?.LoanRealEstates is not null)
            model.Mortgage.LoanRealEstates.AddRange(request.Parameters!.LoanRealEstates.Select(x => new _SA.SalesArrangementParametersMortgage.Types.LoanRealEstate
            {
                IsCollateral = x.IsCollateral,
                RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                RealEstateTypeId = x.RealEstateTypeId,
            }));

        //TODO ma smysl tady resit ruzne objekty, kdyz ani nevim jak to bude za mesic vypadat?
        await _salesArrangementService.UpdateSalesArrangementParameters(model, cancellationToken);
    }

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<UpdateParametersHandler> _logger;

    public UpdateParametersHandler(
        ISalesArrangementServiceAbstraction salesArrangementService,
        ILogger<UpdateParametersHandler> logger)
    {
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
