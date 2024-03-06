using CIS.Core.Security;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using DomainServices.UserService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2;

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
        var offer = await _offerService.GetOffer(saInstance.OfferId!.Value, cancellationToken);

        // neexistuje RBC -> vytvorit novy a ulozit
        await _createProductTrain.RunCreateRiskBusinessCaseOnly(saInstance.SalesArrangementId, cancellationToken);

        // nemame ulozenou DataVersion na SA nebo chceme vytvorit novy assessment
        if (!string.IsNullOrEmpty(saInstance.RiskBusinessCaseId) && (string.IsNullOrEmpty(saInstance.LoanApplicationDataVersion) || request.NewAssessmentRequired))
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

        return await _resultService.CreateResult(saInstance.SalesArrangementId, assessment, exposure, offer, cancellationToken);
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

    private async Task<CustomerExposureCalculateResponse?> getExposure(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(_currentUser.User!.Id, cancellationToken);

        // exposure muze selhat...
        try
        {
            return await _customerExposureService.Calculate(new CustomerExposureCalculateRequest
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
        catch
        {
            return null;
        }
    }

    private async Task createNewAssessment(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        // create assesment
        var createAssessmentRequest = new RiskBusinessCaseCreateAssessmentRequest
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            RiskBusinessCaseId = saInstance.RiskBusinessCaseId,
            LoanApplicationDataVersion = saInstance.LoanApplicationDataVersion,
            AssessmentMode = RiskBusinessCaseAssessmentModes.SC,
            GrantingProcedureCode = offer.MortgageOffer.SimulationInputs.IsEmployeeBonusRequested == true ? RiskBusinessCaseGrantingProcedureCodes.EMP : RiskBusinessCaseGrantingProcedureCodes.STD,
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

    private readonly GetLoanApplicationAssessmentResultService _resultService;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly IUserServiceClient _userService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly NOBY.Services.RiskCaseProcessor.RiskCaseProcessorService _riskCaseProcessor;
    private readonly Services.CreateProductTrain.ICreateProductTrainService _createProductTrain;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient _customerExposureService;

    public GetLoanApplicationAssessmentHandler(
        GetLoanApplicationAssessmentResultService resultService,
        IUserServiceClient userService,
        ICurrentUserAccessor currentUser,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        Services.RiskCaseProcessor.RiskCaseProcessorService riskCaseProcessor,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        DomainServices.RiskIntegrationService.Clients.CustomerExposure.V2.ICustomerExposureServiceClient customerExposureService,
        Services.CreateProductTrain.ICreateProductTrainService createProductTrain)
    {
        _resultService = resultService;
        _offerService = offerService;
        _riskBusinessCaseService = riskBusinessCaseService;
        _currentUser = currentUser;
        _userService = userService;
        _salesArrangementService = salesArrangementService;
        _riskCaseProcessor = riskCaseProcessor;
        _customerExposureService = customerExposureService;
        _createProductTrain = createProductTrain;
    }
}
