namespace DomainServices.CaseService.Api.Notifications.Handlers;

internal sealed class CaseStateChangedHandler
    : INotificationHandler<CaseStateChangedNotification>
{
    public async Task Handle(CaseStateChangedNotification notification, CancellationToken cancellationToken)
    {
        var productType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == notification.ProductTypeId);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(t => t.Id == notification.CaseStateId);

        // get current user's login
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        // get case owner
        var ownerInstance = await _userService.GetUser(notification.CaseOwnerUserId, cancellationToken);

        // vytahnout povolena SATypeId pro tento ProductTypeId
        var allowedSaTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.ProductTypeId == notification.ProductTypeId)
            .Select(t => t.Id)
            .ToList();

        // get rbcid
        var saList = await _salesArrangementService.GetSalesArrangementList(notification.CaseId, cancellationToken: cancellationToken);
        string? rbcId = saList.SalesArrangements.FirstOrDefault(t => allowedSaTypeId.Contains(t.SalesArrangementTypeId))?.RiskBusinessCaseId;

        //TODO login
        var request = new ExternalServices.SbWebApi.Dto.CaseStateChangedRequest
        {
            Login = userInstance.UserIdentifiers.FirstOrDefault()?.Identity ?? "anonymous",
            CaseId = notification.CaseId,
            ContractNumber = notification.ContractNumber,
            ClientFullName = notification.ClientName ?? "",
            CaseStateName = caseState.Name,
            ProductTypeId = notification.ProductTypeId,
            OwnerUserCpm = ownerInstance.CPM,
            OwnerUserIcp = ownerInstance.ICP,
            Mandant = (CIS.Foms.Enums.Mandants)productType.MandantId.GetValueOrDefault(),
            RiskBusinessCaseId = rbcId
        };
        await _sbWebApiClient.CaseStateChanged(request, cancellationToken);
    }

    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CaseStateChangedHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        CodebookService.Clients.ICodebookServiceClients codebookService, 
        UserService.Clients.IUserServiceClient userService,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApiClient,
        SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _userAccessor = userAccessor;
        _codebookService = codebookService;
        _userService = userService;
        _sbWebApiClient = sbWebApiClient;
        _salesArrangementService = salesArrangementService;
    }
}
