using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.CaseService.Api.Notifications.Handlers;

internal class CaseStateChangedHandler
    : INotificationHandler<CaseStateChangedNotification>
{
    public async Task Handle(CaseStateChangedNotification notification, CancellationToken cancellationToken)
    {
        var productType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == notification.ProductTypeId);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(t => t.Id == notification.CaseStateId);

        // get current user's login
        var userInstance = ServiceCallResult.ResolveAndThrowIfError<UserService.Contracts.User>(await _userService.GetUser(_userAccessor.User!.Id, cancellationToken));

        // get case owner
        var ownerInstance = ServiceCallResult.ResolveAndThrowIfError<UserService.Contracts.User>(await _userService.GetUser(notification.CaseOwnerUserId, cancellationToken));

        // vytahnout povolena SATypeId pro tento ProductTypeId
        var allowedSaTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.ProductTypeId == notification.ProductTypeId)
            .Select(t => t.Id)
            .ToList();

        // get rbcid
        var saList = ServiceCallResult.ResolveAndThrowIfError<_SA.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(notification.CaseId, cancellationToken: cancellationToken));
        string? rbcId = saList.SalesArrangements.FirstOrDefault(t => allowedSaTypeId.Contains(t.SalesArrangementTypeId))?.RiskBusinessCaseId;

        //TODO login
        var request = new ExternalServices.SbWebApi.Shared.CaseStateChangedModel(
            userInstance.UserIdentifiers.First().Identity,
            notification.CaseId,
            notification.ContractNumber, 
            notification.ClientName ?? "", 
            caseState.Name, 
            notification.ProductTypeId,
            ownerInstance.CPM,
            ownerInstance.ICP, 
            productType.Mandant,
            rbcId);

        await _sbWebApiClient.CaseStateChanged(request, cancellationToken);
    }

    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;
    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CaseStateChangedHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, 
        UserService.Abstraction.IUserServiceAbstraction userService,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApiClient,
        SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _userAccessor = userAccessor;
        _codebookService = codebookService;
        _userService = userService;
        _sbWebApiClient = sbWebApiClient;
        _salesArrangementService = salesArrangementService;
    }
}
