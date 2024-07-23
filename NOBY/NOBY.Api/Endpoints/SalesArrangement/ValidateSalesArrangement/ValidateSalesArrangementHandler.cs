using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement;

internal sealed class ValidateSalesArrangementHandler(
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService, 
    Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization)
        : IRequestHandler<ValidateSalesArrangementRequest, SalesArrangementValidateSalesArrangementResponse>
{
    public async Task<SalesArrangementValidateSalesArrangementResponse> Handle(ValidateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // validace opravneni
        await _salesArrangementAuthorization.ValidateSaAccessBySaType213And248BySAId(request.SalesArrangementId, cancellationToken);

        var response = await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken);

        return new SalesArrangementValidateSalesArrangementResponse
        {
            Categories = response
                .ValidationMessages
                ?.Where(t => t.NobyMessageDetail.Severity != _SA.ValidationMessageNoby.Types.NobySeverity.None)
                .GroupBy(t => t.NobyMessageDetail.Category)
                .OrderBy(t => t.Min(x => x.NobyMessageDetail.CategoryOrder))
                .Select(t => new SalesArrangementSharedValidateCategory
                {
                    CategoryName = t.Key,
                    ValidationMessages = t.Select(t2 => new SalesArrangementSharedValidateMessage
                    {
                        Message = t2.NobyMessageDetail.Message,
                        Parameter = t2.NobyMessageDetail.ParameterName,
                        Severity = t2.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error ? SalesArrangementSharedValidateMessageSeverity.Error : SalesArrangementSharedValidateMessageSeverity.Warning
                    }).ToList()
                }).ToList()
        };   
    }
}
