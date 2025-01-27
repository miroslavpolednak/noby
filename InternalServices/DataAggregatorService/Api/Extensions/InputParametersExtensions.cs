﻿namespace CIS.InternalServices.DataAggregatorService.Api.Extensions;

public static class InputParametersExtensions
{
    private const string InputParameterNullMessage = "The parameter has not been set on the input or through the configuration of dynamic parameters.";

    public static void ValidateSalesArrangementId(this InputParameters inputParameters)
    {
        if (inputParameters.SalesArrangementId.HasValue)
            return;

        throw new ArgumentNullException(nameof(InputParameters.SalesArrangementId), InputParameterNullMessage);
    }

    public static void ValidateOfferId(this InputParameters inputParameters)
    {
        if (inputParameters.OfferId.HasValue)
            return;

        throw new ArgumentNullException(nameof(inputParameters.OfferId), InputParameterNullMessage);
    }

    public static void ValidateCaseId(this InputParameters inputParameters)
    {
        if (inputParameters.CaseId.HasValue)
            return;

        throw new ArgumentNullException(nameof(InputParameters.CaseId), InputParameterNullMessage);
    }

    public static void ValidateCustomerIdentities(this InputParameters inputParameters)
    {
        if (inputParameters.CustomerIdentities.Any())
            return;

        throw new InvalidOperationException($"{nameof(inputParameters.CustomerIdentities)} - Sequence contains no elements");
    }

    public static void ValidateUserId(this InputParameters inputParameters)
    {
        if (inputParameters.UserId.HasValue)
            return;

        throw new ArgumentNullException(nameof(InputParameters.UserId), InputParameterNullMessage);
    }
}