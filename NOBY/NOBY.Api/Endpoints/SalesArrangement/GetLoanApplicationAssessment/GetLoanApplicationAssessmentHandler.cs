using CIS.Core.Security;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using DomainServices.UserService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal sealed class GetLoanApplicationAssessmentHandler
    : IRequestHandler<GetLoanApplicationAssessmentRequest, GetLoanApplicationAssessmentResponse>
{
    public async Task<GetLoanApplicationAssessmentResponse> Handle(GetLoanApplicationAssessmentRequest request, CancellationToken cancellationToken)
    {
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
        else if (string.IsNullOrEmpty(saInstance.LoanApplicationDataVersion) || request.NewAssessmentRequired)
        {
            await updateLoanAssesment(saInstance, !request.NewAssessmentRequired, cancellationToken);
        }

        if (request.NewAssessmentRequired)
        {
            await createNewAssessment(saInstance, offer, cancellationToken);
        }
        else if (string.IsNullOrEmpty(saInstance.LoanApplicationAssessmentId))
        {
            throw new NobyValidationException("LoanApplicationAssessmentId is empty");
        }

        // load assesment by ID
        var assessmentRequest = new RiskBusinessCaseGetAssessmentRequest
        {
            LoanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId,
            RequestedDetails = new()
            {
                RiskBusinessCaseRequestedDetails.assessmentDetail,
                RiskBusinessCaseRequestedDetails.householdAssessmentDetail,
                RiskBusinessCaseRequestedDetails.counterpartyAssessmentDetail,
                RiskBusinessCaseRequestedDetails.collateralRiskCharacteristics
            }
        };
        var assessment = await _riskBusinessCaseService.GetAssessment(assessmentRequest, cancellationToken);

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
        var response = assessment.ToApiResponse(offer);

        if (response.AssessmentResult == 502 && (response.Reasons?.Any(t => t.Code == "060009") ?? false))
        {
            // customers
            var customers = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

            foreach (var customer in customers)
            {
                var obligations = await _customerOnSAService.GetObligationList(customer.CustomerOnSAId, cancellationToken);
                if (obligations.Any(t => ((t.Creditor is not null && !t.Creditor.IsExternal.GetValueOrDefault()) && (t.Correction is not null && t.Correction.CorrectionTypeId.GetValueOrDefault() != 1))))
                {
                    response.DisplayAssessmentResultInfoText = true;
                    break;
                }
            }
        }

        return response;
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

    private readonly ICurrentUserAccessor _currentUser;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly NOBY.Services.RiskCaseProcessor.RiskCaseProcessorService _riskCaseProcessor;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient _customerExposureService;

    public GetLoanApplicationAssessmentHandler(
        IUserServiceClient userService,
        ICustomerOnSAServiceClient customerOnSAService,
        ICurrentUserAccessor currentUser,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        Services.RiskCaseProcessor.RiskCaseProcessorService riskCaseProcessor,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient customerExposureService)
    {
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
