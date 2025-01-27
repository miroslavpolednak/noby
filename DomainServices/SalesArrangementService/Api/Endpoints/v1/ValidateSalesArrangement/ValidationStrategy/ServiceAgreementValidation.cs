﻿using CIS.Core.Attributes;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Contracts;
using SharedTypes.Extensions;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

[ScopedService, SelfService]
internal sealed class ServiceAgreementValidation(ICustomerOnSAServiceClient _customerOnSAService) 
    : ISalesArrangementValidationStrategy
{
    public async Task<ValidateSalesArrangementResponse> Validate(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var customers = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);

        if (customers.Any(c => !c.CustomerIdentifiers.HasKbIdentity()))
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.NotAllCustomersOnSaAreIdentified);

        CheckIfApplicantIsSet(salesArrangement);

        return new ValidateSalesArrangementResponse();
    }

    private static void CheckIfApplicantIsSet(SalesArrangement salesArrangement)
    {
        if (salesArrangement.ParametersCase is not (SalesArrangement.ParametersOneofCase.GeneralChange or SalesArrangement.ParametersOneofCase.HUBN))
            return;

        _ = salesArrangement.ParametersCase switch
        {
            SalesArrangement.ParametersOneofCase.GeneralChange => salesArrangement.GeneralChange.Applicant,
            SalesArrangement.ParametersOneofCase.HUBN => salesArrangement.HUBN.Applicant,
            _ => throw new NotImplementedException()
        } ?? throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.ApplicantIsNotSet);
    }
}