using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

internal class SendToCmpHandler
    : IRequestHandler<SendToCmpRequest, SendToCmpResponse>
{

    #region Construction

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ILogger<SendToCmpHandler> _logger;

    public SendToCmpHandler(
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        ILogger<SendToCmpHandler> logger)
    {
        _caseService = caseService;
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }

    #endregion

    public async Task<SendToCmpResponse> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // provolat validaci SA
        var validationResult = await callSaValidation(request.SalesArrangementId, cancellationToken);
        if (validationResult?.ValidationMessages?.Any() ?? false && validationResult.ValidationMessages.Any(t => t.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error))
        {
            return new SendToCmpResponse
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
            };
        }

        // odeslat do SB
        await _salesArrangementService.SendToCmp(request.SalesArrangementId, cancellationToken);

        // update case state
        await _caseService.UpdateCaseState(saInstance.CaseId, 2, cancellationToken);

        return new SendToCmpResponse();
    }

    private async Task<_SA.ValidateSalesArrangementResponse> callSaValidation(int salesArrangementId, CancellationToken cancellationToken)
    {
        try
        {
            return ServiceCallResult.ResolveAndThrowIfError<_SA.ValidateSalesArrangementResponse>(await _salesArrangementService.ValidateSalesArrangement(salesArrangementId, cancellationToken));
        }
        catch (CisArgumentException ex)
        {
            // rethrow to be catched by validation middleware
            throw new CisValidationException(ex.ExceptionCode, ex.Message);
        }
    }
}
