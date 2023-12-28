using CIS.Core.Attributes;
using CIS.Core.Security;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.HouseholdService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using DomainServices.UserService.Clients;
using System.Collections.Generic;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

[ScopedService, SelfService]
internal sealed class GetLoanApplicationAssessmentResultService
{
    public async Task<GetLoanApplicationAssessmentResponse> CreateResult(
        int salesArrangementId,
        LoanApplicationAssessmentResponse assessment,
        CustomerExposureCalculateResponse exposure,
        GetMortgageOfferResponse offer,
        CancellationToken cancellationToken)
    {
        // customers
        var customers = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);
        // households
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        // codebooks
        var obligationTypes = await _codebookService.ObligationTypes(cancellationToken);

        // get obligations for each customer
        //TODO new DS endpoint for all obligations on SA?
        Dictionary<int, List<Obligation>> obligations = new();
        customers.ForEach(async customer =>
        {
            var list = await _customerOnSAService.GetObligationList(customer.CustomerOnSAId, cancellationToken);
            obligations[customer.CustomerOnSAId] = list ?? new List<Obligation>(0);
        });

        // vytvoreni response
        GetLoanApplicationAssessmentResponse response = new()
        {
            RiskBusinesscaseExpirationDate = assessment!.RiskBusinessCaseExpirationDate,
            AssessmentResult = assessment.AssessmentResult,
            Application = assessment.ToLoanApplicationApiResponse(offer),
            Reasons = assessment.ToReasonsApiResponse(),
            Households = new(households.Count),
            DisplayAssessmentResultInfoText = assessment.GetDisplayAssessmentResultInfoTextToReasonsApiResponse(customers, obligations)
        };

        // vytvoreni response - households
        foreach (var household in households)
        {
            Dto.Household householdResponse = new()
            {
                HouseholdId = household.HouseholdId,
                HouseholdTypeId = household.HouseholdTypeId,
                Risk = assessment.HouseholdsDetails?.FirstOrDefault(t => t.HouseholdId.GetValueOrDefault() == household.HouseholdId)?.ToHouseholdRiskApiResponse(),
                Customers = new List<Dto.HouseholdCustomerObligations>()
            };

            if (household.CustomerOnSAId1.HasValue)
            {
                var c = customers.First(t => t.CustomerOnSAId == household.CustomerOnSAId1.Value);
                householdResponse.Customers.Add(getCustomer(c, obligations[household.CustomerOnSAId1.Value], exposure, obligationTypes));
            }

            if (household.CustomerOnSAId2.HasValue)
            {
                var c = customers.First(t => t.CustomerOnSAId == household.CustomerOnSAId2.Value);
                householdResponse.Customers.Add(getCustomer(c, obligations[household.CustomerOnSAId2.Value], exposure, obligationTypes));
            }

            response.Households.Add(householdResponse);
        }

        return response;
    }

    private Dto.HouseholdCustomerObligations getCustomer(
        CustomerOnSA customer,
        List<Obligation> obligations,
        CustomerExposureCalculateResponse exposure, 
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes)
    {
        var customerExposure = exposure.Customers?.FirstOrDefault(t => t.InternalCustomerId == customer.CustomerOnSAId);

        Dto.HouseholdCustomerObligations obligationCustomer = new()
        {
            DateOfBirth = customer.DateOfBirthNaturalPerson,
            FirstName = customer.FirstNameNaturalPerson,
            LastName = customer.Name,
            RoleId = (CustomerRoles)customer.CustomerRoleId,
            ExistingObligations = getExistingObligations(customerExposure, obligationTypes, obligations)
        };

        if (_currentUser.HasPermission(UserPermissions.CLIENT_EXPOSURE_DisplayRequestedExposure))
        {
            obligationCustomer.RequestedObligations = getRequestedObligations(customerExposure, obligationTypes);
        }

        return obligationCustomer;
    }

    private static List<Dto.HouseholdObligationItem> getRequestedObligations(CustomerExposureCustomer? customerExposure, List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes)
    {
        var obligations = new List<Dto.HouseholdObligationItem>();

        if (customerExposure?.RequestedCBCBNaturalPersonExposureItem is not null)
        {
            obligations.AddRange(customerExposure.RequestedCBCBNaturalPersonExposureItem.CreateHouseholdObligations(obligationTypes));
        }

        if (customerExposure?.RequestedKBGroupNaturalPersonExposures is not null)
        {
            obligations.AddRange(customerExposure.RequestedKBGroupNaturalPersonExposures.CreateHouseholdObligations(obligationTypes));
        }

        if (customerExposure?.RequestedCBCBJuridicalPersonExposureItem is not null)
        {
            obligations.AddRange(customerExposure.RequestedCBCBJuridicalPersonExposureItem.CreateHouseholdObligations(obligationTypes));
        }

        if (customerExposure?.RequestedKBGroupJuridicalPersonExposures is not null)
        {
            obligations.AddRange(customerExposure.RequestedKBGroupJuridicalPersonExposures.CreateHouseholdObligations(obligationTypes));
        }

        return obligations.OrderBy(t => t.ObligationTypeOrder).ToList();
    }

    private static List<Dto.HouseholdObligationItem> getExistingObligations(CustomerExposureCustomer? customerExposure, List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes, List<Obligation> nobyObligations)
    {
        var obligations = new List<Dto.HouseholdObligationItem>();

        if (customerExposure?.ExistingCBCBNaturalPersonExposureItem is not null)
        {
            obligations.AddRange(customerExposure.ExistingCBCBNaturalPersonExposureItem.CreateHouseholdObligations(obligationTypes));
        }

        if (customerExposure?.ExistingKBGroupNaturalPersonExposures is not null)
        {
            obligations.AddRange(customerExposure.ExistingKBGroupNaturalPersonExposures.CreateHouseholdObligations(obligationTypes));
        }

        if (customerExposure?.ExistingCBCBJuridicalPersonExposureItem is not null)
        {
            obligations.AddRange(customerExposure.ExistingCBCBJuridicalPersonExposureItem.CreateHouseholdObligations(obligationTypes));
        }

        if (customerExposure?.ExistingKBGroupJuridicalPersonExposures is not null)
        {
            obligations.AddRange(customerExposure.ExistingKBGroupJuridicalPersonExposures.CreateHouseholdObligations(obligationTypes));
        }

        // zavazky NOBY
        obligations.AddRange(nobyObligations.CreateHouseholdObligations(obligationTypes));

        return obligations.OrderBy(t => t.ObligationTypeOrder).ToList();
    }

    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.HouseholdService.Clients.IHouseholdServiceClient _householdService;

    public GetLoanApplicationAssessmentResultService(
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUser,
        DomainServices.HouseholdService.Clients.IHouseholdServiceClient householdService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _currentUser = currentUser;
    }
}
