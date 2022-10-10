using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.Validate;

internal sealed class ValidateHandler
    : IRequestHandler<ValidateRequest, ValidateResponse>
{
    public async Task<ValidateResponse> Handle(ValidateRequest request, CancellationToken cancellationToken)
    {
        var response = ServiceCallResult.ResolveAndThrowIfError<_SA.ValidateSalesArrangementResponse>(await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken));
        
        return new ValidateResponse(response);
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public ValidateHandler(DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
