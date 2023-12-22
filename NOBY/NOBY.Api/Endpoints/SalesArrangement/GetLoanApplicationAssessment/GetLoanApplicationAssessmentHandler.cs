using CIS.Core.Security;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using DomainServices.UserService.Clients;
using System.Collections.Generic;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{
    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
        // 
        if (request.NewAssessmentRequired && !_currentUser.HasPermission(UserPermissions.SCORING_Perform))
        {
            throw new CisAuthorizationException("SCORING_Perform permission missing");
        }

        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        // instance of Offer
        var offer = await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken);

        // neexistuje RBC -> vytvorit novy a ulozit
        if (string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
        {
            await createRBC(saInstance, cancellationToken);
        }
        // nemame ulozenou DataVersion na SA nebo chceme vytvorit novy assessment
        else if (string.IsNullOrEmpty(saInstance.LoanApplicationDataVersion) || request.NewAssessmentRequired)
        {
            await updateLoanAssesment(saInstance, !request.NewAssessmentRequired, cancellationToken);
        }

        // ma se vytvorit novy assesment
        if (request.NewAssessmentRequired)
        {
            await createNewAssessment(saInstance, offer, cancellationToken);
        }
        // ma se pouzit jiz vytvoreny assessment, ale nemame ulozene jeho ID
        else if (string.IsNullOrEmpty(saInstance.LoanApplicationAssessmentId))
        {
            throw new NobyValidationException("LoanApplicationAssessmentId is empty");
        }

        // load assesment by ID
        var assessment = await getAssessment(saInstance.LoanApplicationAssessmentId, cancellationToken);

        // get exposure
        var exposure = await getExposure(saInstance, cancellationToken);

        return await createResult(saInstance.SalesArrangementId, assessment, exposure, offer, cancellationToken);
    }

    private async Task<GetLoanApplicationAssessmentResponse> createResult(
        int salesArrangementId,
        LoanApplicationAssessmentResponse assessment,
        DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2.CustomerExposureCalculateResponse exposure,
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
        Dictionary<int, List<DomainServices.HouseholdService.Contracts.Obligation>> obligations = new();
        customers.ForEach(async customer =>
        {
            var list = await _customerOnSAService.GetObligationList(customer.CustomerOnSAId, cancellationToken);
            if (list.Count > 0) 
            {
                obligations[customer.CustomerOnSAId] = list!;
            }
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
                CustomerObligations = new List<Dto.HouseholdCustomerObligations>()
            };

            var customer = customers.First(t => t.CustomerOnSAId == household.CustomerOnSAId1);
            var customerExposure = exposure.Customers?.FirstOrDefault(t => t.InternalCustomerId == customer.CustomerOnSAId);

            Dto.HouseholdCustomerObligations obligationCustomer = new()
            {
                DateOfBirth = customer.DateOfBirthNaturalPerson,
                FirstName = customer.FirstNameNaturalPerson,
                LastName = customer.Name,
                RoleId = (CustomerRoles)customer.CustomerRoleId,
                ExistingObligations = getExistingObligations(customerExposure, obligationTypes),
                RequestedObligations = getRequestedObligations(customerExposure, obligationTypes)
            };

            householdResponse.CustomerObligations.Add(obligationCustomer);

            response.Households.Add(householdResponse);
        }
        
        return response;
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

        return obligations;
    }

    private static List<Dto.HouseholdObligationItem> getExistingObligations(CustomerExposureCustomer? customerExposure, List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes)
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

        return obligations;
    }

    private async Task<LoanApplicationAssessmentResponse> getAssessment(string loanApplicationAssessmentId, CancellationToken cancellationToken)
    {
        var assessmentRequest = new RiskBusinessCaseGetAssessmentRequest
        {
            LoanApplicationAssessmentId = loanApplicationAssessmentId,
            RequestedDetails = _assessmentRequestDetails
        };
        return await _riskBusinessCaseService.GetAssessment(assessmentRequest, cancellationToken);
    }

    private async Task<DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2.CustomerExposureCalculateResponse> getExposure(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(_currentUser.User!.Id, cancellationToken);
        
        return await _customerExposureService.Calculate(new DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2.CustomerExposureCalculateRequest
        {
            LoanApplicationDataVersion = saInstance.LoanApplicationDataVersion,
            SalesArrangementId = saInstance.SalesArrangementId,
            RiskBusinessCaseId = saInstance.RiskBusinessCaseId,
            UserIdentity = new DomainServices.RiskIntegrationService.Contracts.Shared.Identity
            {
                IdentityId = user.UserIdentifiers[0].Identity,
                IdentityScheme = user.UserIdentifiers[0].IdentityScheme.ToString()
            }
        }, cancellationToken);
    }

    private async Task createNewAssessment(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, GetMortgageOfferResponse offer, CancellationToken cancellationToken)
    {
        // create assesment
        var createAssessmentRequest = new RiskBusinessCaseCreateAssessmentRequest
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            RiskBusinessCaseId = saInstance.RiskBusinessCaseId,
            LoanApplicationDataVersion = saInstance.LoanApplicationDataVersion,
            AssessmentMode = RiskBusinessCaseAssessmentModes.SC,
            GrantingProcedureCode = offer.SimulationInputs.IsEmployeeBonusRequested == true ? RiskBusinessCaseGrantingProcedureCodes.EMP : RiskBusinessCaseGrantingProcedureCodes.STD,
        };
        var createAssessmentResponse = await _riskBusinessCaseService.CreateAssessment(createAssessmentRequest, cancellationToken);

        // update SA
        await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            LoanApplicationAssessmentId = createAssessmentResponse.LoanApplicationAssessmentId,
            RiskBusinessCaseExpirationDate = createAssessmentResponse.RiskBusinessCaseExpirationDate,
            LoanApplicationDataVersion = saInstance.LoanApplicationDataVersion,
            RiskSegment = saInstance.RiskSegment
        }, cancellationToken);
        saInstance.LoanApplicationAssessmentId = createAssessmentResponse.LoanApplicationAssessmentId;

        // set flow switch
        await _salesArrangementService.SetFlowSwitch(saInstance.SalesArrangementId, FlowSwitches.ScoringPerformedAtleastOnce, true, cancellationToken);
    }

    private async Task createRBC(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        var createResult = await _riskCaseProcessor.CreateOrUpdateRiskCase(saInstance.SalesArrangementId, cancellationToken);

        // update
        await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            RiskBusinessCaseId = createResult.RiskBusinessCaseId,
            RiskSegment = createResult.RiskSegment,
            LoanApplicationDataVersion = createResult.LoanApplicationDataVersion
        }, cancellationToken);
    }

    private async Task updateLoanAssesment(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, bool updateSalesArrangement, CancellationToken cancellationToken)
    {
        var laResult = await _riskCaseProcessor.SaveLoanApplication(saInstance.SalesArrangementId, saInstance.CaseId, saInstance.OfferId!.Value, cancellationToken);
        saInstance.LoanApplicationDataVersion = laResult.LoanApplicationDataVersion;

        if (updateSalesArrangement)
        {
            await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
            {
                SalesArrangementId = saInstance.SalesArrangementId,
                RiskSegment = laResult.RiskSegment,
                LoanApplicationDataVersion = laResult.LoanApplicationDataVersion
            }, cancellationToken);
        }
    }

    static List<RiskBusinessCaseRequestedDetails> _assessmentRequestDetails = new()
    {
        RiskBusinessCaseRequestedDetails.assessmentDetail,
        RiskBusinessCaseRequestedDetails.householdAssessmentDetail,
        RiskBusinessCaseRequestedDetails.counterpartyAssessmentDetail,
        RiskBusinessCaseRequestedDetails.collateralRiskCharacteristics
    };

    private readonly ICurrentUserAccessor _currentUser;
    private readonly IUserServiceClient _userService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.HouseholdService.Clients.IHouseholdServiceClient _householdService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly NOBY.Services.RiskCaseProcessor.RiskCaseProcessorService _riskCaseProcessor;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient _customerExposureService;

    public GetLoanApplicationAssessmentHandler(
        IUserServiceClient userService,
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUser,
        DomainServices.HouseholdService.Clients.IHouseholdServiceClient householdService,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        Services.RiskCaseProcessor.RiskCaseProcessorService riskCaseProcessor,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient customerExposureService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _offerService = offerService;
        _riskBusinessCaseService = riskBusinessCaseService;
        _currentUser = currentUser;
        _userService = userService;
        _salesArrangementService = salesArrangementService;
        _riskCaseProcessor = riskCaseProcessor;
        _customerExposureService = customerExposureService;
    }
}
