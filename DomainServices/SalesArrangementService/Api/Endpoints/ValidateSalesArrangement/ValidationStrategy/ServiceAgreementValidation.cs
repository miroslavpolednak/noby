using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement.ValidationStrategy;

internal class ServiceAgreementValidation : ISalesArrangementValidationStrategy
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public ServiceAgreementValidation(ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerOnSAService = customerOnSAService;
    }

    public async Task<ValidateSalesArrangementResponse> Validate(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var customers = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);

        if (customers.Any(c => c.CustomerIdentifiers.All(i => i.IdentityScheme != Identity.Types.IdentitySchemes.Kb)))
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