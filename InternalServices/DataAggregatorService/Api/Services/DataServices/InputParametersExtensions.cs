namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

public static class InputParametersExtensions
{
    private const string _inputParameterNullMessage = "The parameter has not been set on the input or through the configuration of dynamic parameters.";

    public static void ValidateSalesArrangementId(this InputParameters inputParameters)
    {
        if (inputParameters.SalesArrangementId.HasValue)
            return;

        throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId));
    }

    public static void ValidateOfferId(this InputParameters inputParameters)
    {
        if (inputParameters.OfferId.HasValue)
            return;

        throw new ArgumentNullException(nameof(inputParameters.OfferId), _inputParameterNullMessage);
    }

    public static void ValidateCaseId(this InputParameters inputParameters)
    {
        if (inputParameters.CaseId.HasValue)
            return;

        throw new ArgumentNullException(nameof(InputParameters.CaseId), _inputParameterNullMessage);
    }

    public static void ValidateCustomerIdentity(this InputParameters inputParameters)
    {
        if (inputParameters.CustomerIdentity is null)
            return;

        throw new ArgumentNullException(nameof(InputParameters.CustomerIdentity));
    }

    public static void ValidateUserId(this InputParameters inputParameters)
    {
        if (inputParameters.UserId.HasValue)
            return;

        throw new ArgumentNullException(nameof(InputParameters.UserId));
    }
}