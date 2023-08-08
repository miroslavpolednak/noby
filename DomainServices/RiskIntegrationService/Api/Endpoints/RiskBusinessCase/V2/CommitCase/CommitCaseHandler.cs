﻿using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3.Contracts;
using _cl = DomainServices.RiskIntegrationService.ExternalServices.RiskBusinessCase.V3;
using CIS.Core.Configuration;
using CIS.Core.Exceptions;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CommitCase;

internal sealed class CommitCaseHandler
    : IRequestHandler<_V2.RiskBusinessCaseCommitCaseRequest, _V2.RiskBusinessCaseCommitCaseResponse>
{
    public async Task<_V2.RiskBusinessCaseCommitCaseResponse> Handle(_V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);
        var riskApplicationType = Helpers.GetRiskApplicationType(await _codebookService.RiskApplicationTypes(cancellationToken), request.ProductTypeId);
        var channels = await _codebookService.Channels(cancellationToken);

        var requestModel = new _C4M.RiskBusinessCaseCommitCreate
        {
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId.ToEnvironmentId(_cisEnvironment.EnvironmentName!), chanel).ToC4M(),
            ItChannel = FastEnum.Parse<_C4M.ItSubChannelType>(chanel, true),
            LoanApplicationProduct = new _C4M.LoanApplicationProduct()
            {
                ProductClusterCode = riskApplicationType!.C4MAplCode
            },
            LoanSoldProduct = request.SoldProduct != null ? new _C4M.LoanSoldProduct
            {
                Id = _C4M.ResourceIdentifier.CreateLoanSoldProduct(request.SoldProduct.Id, request.SoldProduct.Company)?.ToC4M()
            } : null,
            RiskBusinessCaseFinalResult = FastEnum.Parse<_C4M.RiskBusinessCaseFinalResultType>(request.FinalResult.ToString(), true),
            ApprovalLevel = request.ApprovalLevel,
            ApprovalDate = request.ApprovalDate,
            LoanAgreement = request.LoanAgreement != null ? new _C4M.LoanAgreement
            {
                DistributionChannel = channels.FirstOrDefault(t => t.Id == request.LoanAgreement.DistributionChannelId)?.Code,
                SignatureType = request.LoanAgreement.SignatureType.ToString()
            } : null,
            CollateralAgreements = request.CollateralAgreementsId?.Select(t => new _C4M.CollateralAgreement
            {
                Id = _C4M.ResourceIdentifier.CreateCollateralAgreement(t).ToC4M()
            }).ToList()
        };

        // human user instance
        if (request.UserIdentity != null)
        {
            try
            {
                var userInstance = await _userService.GetUserRIPAttributes(request.UserIdentity.IdentityId ?? "", request.UserIdentity.IdentityScheme ?? "", cancellationToken);
                if (userInstance != null)
                {
                    if (Helpers.IsDealerSchema(userInstance.DealerCompanyId))
                        requestModel.LoanApplicationDealer = userInstance.ToC4mDealer(request.UserIdentity);
                    else
                        requestModel.Creator = userInstance.ToC4mPerson(request.UserIdentity);
                }
            }
            catch (CisNotFoundException) { }
            catch (Exception) {
                throw;
            }
        }

        // approver
        if (request.Approver != null)
        {
            try
            {
                var approverInstance = await _userService.GetUserRIPAttributes(request.Approver.IdentityId ?? "", request.Approver.IdentityScheme ?? "", cancellationToken);
                if (approverInstance != null)
                {
                    if (Helpers.IsDealerSchema(approverInstance.DealerCompanyId))
                        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ApproverEqualDealer);
                    else
                        requestModel.Approver = approverInstance.ToC4mPerson(request.Approver);
                }
            }
            catch (CisNotFoundException) { }
            catch (Exception)
            {
                throw;
            }
        }

        //C4M
        var response = await _client.CommitCase(request.RiskBusinessCaseId, requestModel, cancellationToken);

        return new _V2.RiskBusinessCaseCommitCaseResponse
        {
            //TODO C4M
            RiskBusinessCaseId = response.RiskBusinessCaseId,
            Timestamp = response.Timestamp.DateTime,
            OperationResult = response.OperationResult,
            FinalState = response.RiskBusinessCaseFinalState,
            ResultReasons = response.ResultReasons?.Select(t => new Contracts.Shared.ResultReasonDetail
            {
                Code = t.Code,
                Description = t.Desc
            }).ToList()
        };
    }

    private readonly _cl.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;
    private readonly UserService.Clients.IUserServiceClient _userService;

    public CommitCaseHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        _cl.IRiskBusinessCaseClient client,
        CodebookService.Clients.ICodebookServiceClient codebookService,
        ICisEnvironmentConfiguration cisEnvironment,
        UserService.Clients.IUserServiceClient userService)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
        _codebookService = codebookService;
        _cisEnvironment = cisEnvironment;
        _userService = userService;
    }
}
