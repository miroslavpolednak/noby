using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

internal sealed class SendToCmpHandler
    : AsyncRequestHandler<SendToCmpRequest>
{

    #region Construction

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public SendToCmpHandler(
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }

    #endregion

    protected override async Task Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // provolat validaci SA
        var validationResult = await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken);

        bool validationContainErrors = validationResult
            ?.ValidationMessages
            ?.Any(t => t.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error) ?? false;
        bool validationContainWarnings = validationResult
            ?.ValidationMessages
            ?.Any(t => t.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Warning) ?? false;

        if (validationContainErrors || (validationContainWarnings && !request.IgnoreWarnings))
        {
            throw new CisValidationException("SA neni validni, nelze odeslat do SB. Provolej Validate endpoint.");
        }

        // odeslat do SB
        await _salesArrangementService.SendToCmp(request.SalesArrangementId, cancellationToken);

        // update case state
        await _caseService.UpdateCaseState(saInstance.CaseId, (int)CaseStates.InApproval, cancellationToken);
    }
}
