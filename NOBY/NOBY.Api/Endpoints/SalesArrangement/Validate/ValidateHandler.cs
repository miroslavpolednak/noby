using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.Validate;

internal sealed class ValidateHandler
    : IRequestHandler<ValidateRequest, ValidateResponse>
{
    public async Task<ValidateResponse> Handle(ValidateRequest request, CancellationToken cancellationToken)
    {
        var response = ServiceCallResult.ResolveAndThrowIfError<_SA.ValidateSalesArrangementResponse>(await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken));

        return new ValidateResponse
        {
            Categories = response.ValidationMessages?
                .GroupBy(t => t.NobyMessageDetail.Category)
                .Select(t => new Dto.ValidateCategory
                {
                    CategoryName = t.Key,
                    ValidationMessages = t.Select(t2 => new Dto.ValidateMessage
                    {
                        Message = t2.NobyMessageDetail.Message,
                        Parameter = t2.NobyMessageDetail.ParameterName,
                        Severity = t2.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error ? Dto.ValidateMessage.MessageSeverity.Error : Dto.ValidateMessage.MessageSeverity.Warning
                    }).ToList()
                }).ToList()
        };   
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClients _salesArrangementService;

    public ValidateHandler(DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClients salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
