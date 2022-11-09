using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.Validate;

internal sealed class ValidateHandler
    : IRequestHandler<ValidateRequest, ValidateResponse>
{
    public async Task<ValidateResponse> Handle(ValidateRequest request, CancellationToken cancellationToken)
    {
        var response = ServiceCallResult.ResolveAndThrowIfError<_SA.ValidateSalesArrangementResponse>(await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken));
        
        return new ValidateResponse(response);
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClients _salesArrangementService;

    public ValidateHandler(DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClients salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
