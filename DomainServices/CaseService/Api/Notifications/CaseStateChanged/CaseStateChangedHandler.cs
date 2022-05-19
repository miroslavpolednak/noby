namespace DomainServices.CaseService.Api.Notifications.Handlers;

internal class CaseStateChangedHandler
    : INotificationHandler<CaseStateChangedNotification>
{
    public async Task Handle(CaseStateChangedNotification notification, CancellationToken cancellationToken)
    {
        var productType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == notification.ProductTypeId);
        var caseState = (await _codebookService.CaseStates(cancellationToken)).First(t => t.Id == notification.CaseStateId);

        // get case owner
        var ownerInstance = CIS.Core.Results.ServiceCallResult.ResolveAndThrowIfError<UserService.Contracts.User>(await _userService.GetUser(notification.CaseOwnerUserId, cancellationToken));

        var request = new ExternalServices.SbWebApi.Shared.CaseStateChangedRequest(
            notification.CaseId,
            notification.ContractNumber, 
            notification.ClientName ?? "", 
            caseState.Name, 
            notification.ProductTypeId,
            ownerInstance.CPM,
            ownerInstance.ICP, 
            productType.Mandant,
            "");

        await _sbWebApiClient.CaseStateChanged(request, cancellationToken);
    }

    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;
    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CaseStateChangedHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, 
        UserService.Abstraction.IUserServiceAbstraction userService,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApiClient)
    {
        _codebookService = codebookService;
        _userService = userService;
        _sbWebApiClient = sbWebApiClient;
    }
}
