using DomainServices.CaseService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Services.CreateRiskBusinessCase;

[TransientService, SelfService]
public sealed class CreateRiskBusinessCaseService
{
    public async Task<string> Create(long caseId, int salesArrangementId, int customerOnSAId, IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers, CancellationToken cancellationToken)
    {
        long? mpId = customerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp)?.IdentityId;
        long? kbId = customerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId;
        if (!mpId.HasValue || !kbId.HasValue)
        {
            // nema modre ID, nezajima me
            throw new CisValidationException(400001, $"CreateRiskBusinessCaseService for CaseId #{caseId} not proceeding / missing MP ID");
        }

        //SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
        {
            throw new CisValidationException(400002, "CreateRiskBusinessCaseService: SA does not have Offer bound to it");
        }

        // case
        var caseInstance = await _caseService.GetCaseDetail(caseId, cancellationToken);

        // offer
        var offerInstance = await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken);

        // household
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        if (households.Count == 0)
        {
            throw new CisValidationException(400003, "CreateRiskBusinessCaseService: household does not exist");
        }

        // ziskat segment
        string riskSegment = await getRiskSegment();

        await _salesArrangementService.UpdateLoanAssessmentParameters(salesArrangementId, null, riskSegment, null, saInstance.RiskBusinessCaseExpirationDate, cancellationToken);

        // get rbcId
        var createRBCResponse = await _riskBusinessCaseService.CreateCase(salesArrangementId, offerInstance.ResourceProcessId, cancellationToken);

        // ulozit na SA
        await _salesArrangementService.UpdateSalesArrangement(new()
        {
            SalesArrangementId = salesArrangementId,
            ContractNumber = saInstance.ContractNumber ?? "",
            RiskBusinessCaseId = createRBCResponse.RiskBusinessCaseId ?? ""
        }, cancellationToken);

        return createRBCResponse.RiskBusinessCaseId;

        #region local fce
        async Task<string> getRiskSegment()
        {
            var loanApplicationRequest = new DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.LoanApplicationSaveRequest
            {
                SalesArrangementId = salesArrangementId,
                LoanApplicationDataVersion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                Households = new()
                {
                    new()
                    {
                        HouseholdId = households.First(t => t.HouseholdTypeId == (int)HouseholdTypes.Main).HouseholdId,
                        Customers = new()
                        {
                            new DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.LoanApplicationCustomer
                            {
                                InternalCustomerId = customerOnSAId,
                                PrimaryCustomerId = kbId.Value.ToString(CultureInfo.InvariantCulture),
                                CustomerRoleId = (int)CustomerRoles.Debtor
                            }
                        }
                    },
                },
                Product = new()
                {
                    ProductTypeId = caseInstance.Data.ProductTypeId,
                    LoanKindId = offerInstance.SimulationInputs.LoanKindId
                }
            };

            return await _loanApplicationService.Save(loanApplicationRequest, cancellationToken);
        }
        #endregion local fce
    }

    private readonly IHouseholdServiceClient _householdService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceClient _loanApplicationService;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;

    public CreateRiskBusinessCaseService(
        DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceClient loanApplicationService,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        IHouseholdServiceClient householdService,
        IOfferServiceClient offerService,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
        _householdService = householdService;
        _offerService = offerService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
