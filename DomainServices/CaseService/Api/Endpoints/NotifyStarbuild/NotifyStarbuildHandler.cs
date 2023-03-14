using DomainServices.CaseService.Api.Notifications.Handlers;
using DomainServices.CaseService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.CaseService.Api.Endpoints.NotifyStarbuild;

internal sealed class NotifyStarbuildHandler
    : IRequestHandler<NotifyStarbuildRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(NotifyStarbuildRequest request, CancellationToken cancellationToken)
    {
        // instance Case
        var caseInstance = await _dbContext
            .Cases
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        var productType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == caseInstance.ProductTypeId);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(t => t.Id == caseInstance.State);

        // get current user's login
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        // get case owner
        var ownerInstance = await _userService.GetUser(caseInstance.OwnerUserId, cancellationToken);

        // vytahnout povolena SATypeId pro tento ProductTypeId
        var allowedSaTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.ProductTypeId == caseInstance.ProductTypeId)
            .Select(t => t.Id)
            .ToList();

        // get rbcid
        var saList = await _salesArrangementService.GetSalesArrangementList(caseInstance.CaseId, cancellationToken: cancellationToken);
        string? rbcId = saList.SalesArrangements.FirstOrDefault(t => allowedSaTypeId.Contains(t.SalesArrangementTypeId))?.RiskBusinessCaseId;

        //TODO login
        var sbRequest = new ExternalServices.SbWebApi.Dto.CaseStateChangedRequest
        {
            Login = userInstance.UserIdentifiers.FirstOrDefault()?.Identity ?? "anonymous",
            CaseId = caseInstance.CaseId,
            ContractNumber = caseInstance.ContractNumber,
            ClientFullName = caseInstance.ClientName ?? "",
            CaseStateName = caseState.Name,
            ProductTypeId = caseInstance.ProductTypeId,
            OwnerUserCpm = ownerInstance.CPM,
            OwnerUserIcp = ownerInstance.ICP,
            Mandant = (CIS.Foms.Enums.Mandants)productType.MandantId.GetValueOrDefault(),
            RiskBusinessCaseId = rbcId,
            IsEmployeeBonusRequested = caseInstance.IsEmployeeBonusRequested
        };
        var result = await _sbWebApiClient.CaseStateChanged(sbRequest, cancellationToken);

        // ulozit request id
        if (result.RequestId.HasValue)
        {
            _dbContext.QueueRequestIds.Add(new Database.Entities.QueueRequestId
            {
                RequestId = result.RequestId.Value,
                CaseId = caseInstance.CaseId,
                CreatedTime = DateTime.Now
            });
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.QueueRequestIdSaved(result.RequestId.Value, request.CaseId);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly ILogger<CaseStateChangedHandler> _logger;
    private readonly Database.CaseServiceDbContext _dbContext;

    public NotifyStarbuildHandler(
        Database.CaseServiceDbContext dbContext,
        ILogger<CaseStateChangedHandler> logger,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        UserService.Clients.IUserServiceClient userService,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApiClient,
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userAccessor = userAccessor;
        _codebookService = codebookService;
        _userService = userService;
        _sbWebApiClient = sbWebApiClient;
        _salesArrangementService = salesArrangementService;
    }
}
