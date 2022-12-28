using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

internal class SendToCmpHandler
    : IRequestHandler<SendToCmpRequest, IActionResult>
{

    #region Construction

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public SendToCmpHandler(
        IHttpContextAccessor httpContextAccessor,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _httpContextAccessor = httpContextAccessor;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }

    #endregion

    public async Task<IActionResult> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // provolat validaci SA
        var validationResult = await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken);
        bool validationContainErrors = validationResult
            ?.ValidationMessages
            ?.Any(t => t.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error) ?? false;

        if (validationResult?.ValidationMessages?.Any() ?? false 
            && (validationContainErrors || !request.IgnoreWarnings))
        {
            return new BadRequestObjectResult(new SendToCmpResponse
            {
                Categories = validationResult.ValidationMessages
                    .GroupBy(t => t.NobyMessageDetail.Category)
                    .Select(t => new ValidateCategory
                    {
                        CategoryName = t.Key,
                        ValidationMessages = t.Select(t2 => new ValidateMessage
                        {
                            Message = t2.NobyMessageDetail.Message,
                            Parameter = t2.NobyMessageDetail.ParameterName,
                            Severity = t2.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error ? MessageSeverity.Error : MessageSeverity.Warning
                        }).ToList()
                    }).ToList()
            });
        }

        // odeslat do SB
        await _salesArrangementService.SendToCmp(request.SalesArrangementId, cancellationToken);

        // update case state
        await _caseService.UpdateCaseState(saInstance.CaseId, 2, cancellationToken);

        return new OkResult();
    }
}
