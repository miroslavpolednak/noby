using CIS.Infrastructure.Caching;
using DomainServices.CaseService.Api.SharedDto;
using DomainServices.CaseService.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace DomainServices.CaseService.Api.Endpoints.NotifyStarbuild;

internal sealed class NotifyStarbuildHandler
    : IRequestHandler<NotifyStarbuildRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(NotifyStarbuildRequest request, CancellationToken cancellationToken)
    {
        // bez prihlaseneho uzivatele to nema cenu
        if (!_userAccessor.IsAuthenticated)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AuthenticatedUserNotFound);
        }

        // instance Case
        var caseInstance = await _dbContext
            .Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        var productType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == caseInstance.ProductTypeId);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(t => t.Id == caseInstance.State);

        // get case owner
        var ownerInstance = await _userService.GetUser(caseInstance.OwnerUserId, cancellationToken);

        // vytahnout povolena SATypeId pro tento ProductTypeId
        var allowedSaTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.ProductTypeId == caseInstance.ProductTypeId)
            .Select(t => t.Id)
            .ToList();

        // get rbcid
        // zkus se kouknout, jestli to rbcid nahodou fakt neexistuje na SA - protoze kdyby jo, tak ho musime poslat do SB
        if (string.IsNullOrEmpty(request.RiskBusinessCaseId))
        {
            var saList = await _salesArrangementService.GetSalesArrangementList(caseInstance.CaseId, cancellationToken: cancellationToken);
            
            request.RiskBusinessCaseId = saList
                .SalesArrangements
                .FirstOrDefault(t => allowedSaTypeId.Contains(t.SalesArrangementTypeId))?
                .RiskBusinessCaseId;
        }

        //TODO login
        var sbRequest = new ExternalServices.SbWebApi.Dto.CaseStateChanged.CaseStateChangedRequest
        {
            CaseId = caseInstance.CaseId,
            ContractNumber = caseInstance.ContractNumber,
            ClientFullName = $"{caseInstance.FirstNameNaturalPerson} {caseInstance.Name}",
            CaseStateName = caseState.Name,
            ProductTypeId = caseInstance.ProductTypeId,
            OwnerUserCpm = ownerInstance.CPM,
            OwnerUserIcp = ownerInstance.ICP,
            Mandant = (CIS.Foms.Enums.Mandants)productType.MandantId.GetValueOrDefault(),
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

    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly Database.CaseServiceDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;

    public NotifyStarbuildHandler(
        IDistributedCache distributedCache,
        Database.CaseServiceDbContext dbContext,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        UserService.Clients.IUserServiceClient userService,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApiClient,
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _distributedCache = distributedCache;
        _dbContext = dbContext;
        _userAccessor = userAccessor;
        _codebookService = codebookService;
        _userService = userService;
        _sbWebApiClient = sbWebApiClient;
        _salesArrangementService = salesArrangementService;
    }
}
