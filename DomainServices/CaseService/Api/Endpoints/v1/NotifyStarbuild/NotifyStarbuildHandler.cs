using CIS.Infrastructure.Caching;
using DomainServices.CaseService.Api.SharedDto;
using DomainServices.CaseService.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace DomainServices.CaseService.Api.Endpoints.v1.NotifyStarbuild;

internal sealed class NotifyStarbuildHandler(
    IDistributedCache _distributedCache,
    Database.CaseServiceDbContext _dbContext,
    CodebookService.Clients.ICodebookServiceClient _codebookService,
    UserService.Clients.IUserServiceClient _userService,
    ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient,
    SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService)
        : IRequestHandler<NotifyStarbuildRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(NotifyStarbuildRequest request, CancellationToken cancellationToken)
    {
        // instance Case
        var caseInstance = await _dbContext
            .Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        var productType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == caseInstance.ProductTypeId);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(t => t.Id == caseInstance.State);

        // get case owner
        var ownerInstance = await _userService.GetUser(caseInstance.OwnerUserId, cancellationToken);

        // get rbcid
        // zkus se kouknout, jestli to rbcid nahodou fakt neexistuje na SA - protoze kdyby jo, tak ho musime poslat do SB
        if (string.IsNullOrEmpty(request.RiskBusinessCaseId) && !request.SkipRiskBusinessCaseId)
        {
            var productSaId = (await _salesArrangementService.GetProductSalesArrangements(caseInstance.CaseId, cancellationToken)).First().SalesArrangementId;
            var productSaInstance = await _salesArrangementService.GetSalesArrangement(productSaId, cancellationToken);

            request.RiskBusinessCaseId = productSaInstance.RiskBusinessCaseId;
        }

        var sbRequest = new ExternalServices.SbWebApi.Dto.CaseStateChanged.CaseStateChangedRequest
        {
            CaseId = caseInstance.CaseId,
            ContractNumber = caseInstance.ContractNumber,
            ClientFullName = $"{caseInstance.FirstNameNaturalPerson} {caseInstance.Name}",
            CaseStateId = caseState.Id,
            CaseStateName = caseState.Name,
            ProductTypeId = caseInstance.ProductTypeId,
            OwnerUserCpm = ownerInstance.UserInfo.Cpm,
            OwnerUserIcp = ownerInstance.UserInfo.Icp,
            Mandant = (Mandants)productType.MandantId!,
            RiskBusinessCaseId = request.RiskBusinessCaseId,
            IsEmployeeBonusRequested = caseInstance.IsEmployeeBonusRequested
        };
        var result = await _sbWebApiClient.CaseStateChanged(sbRequest, cancellationToken);

        // ulozit request id
        if (result.RequestId.HasValue)
        {
            await _distributedCache.SetObjectAsync($"CaseStateChanged_{result.RequestId.Value}", new CaseStateChangeRequestId
            {
                RequestId = result.RequestId.Value,
                CaseId = caseInstance.CaseId,
                CreatedTime = DateTime.Now
            }, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1),
            }, cancellationToken);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
