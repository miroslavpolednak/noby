namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal abstract class BaseValidator
{
    protected readonly long _caseId;
    protected readonly ILogger<CreateSalesArrangementParametersFactory> _logger;

    public BaseValidator(ILogger<CreateSalesArrangementParametersFactory> logger, long caseId)
    {
        _logger = logger;
        _caseId = caseId;
    }
}
