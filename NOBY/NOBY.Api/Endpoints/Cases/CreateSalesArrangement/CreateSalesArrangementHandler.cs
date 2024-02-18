using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // validace prav
        await validatePermissions(request, cancellationToken);

        // pokud neprojde validace, primo ve Validate() se vyhodi exception
        var builder = await _createService
            .CreateBuilder(request.CaseId, request.SalesArrangementTypeId)
            .Validate(cancellationToken);
        var createRequest = await builder.UpdateParameters(cancellationToken);

        // vytvorit SA
        var newSaId = await _salesArrangementService.CreateSalesArrangement(createRequest, cancellationToken);

        // post procesing
        await builder.PostCreateProcessing(newSaId, cancellationToken);

        return new CreateSalesArrangementResponse
        {
            SalesArrangementId = newSaId
        };
    }

    private async Task validatePermissions(CreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // kontrola na kategorii
        var saCategory = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == request.SalesArrangementTypeId)
            ?.SalesArrangementCategory;
        if (saCategory != (int)SalesArrangementCategories.ServiceRequest)
        {
            throw new CisValidationException($"SalesArrangement type not supported");
        }

        // retence
        if (_retentionSATypes.Contains(request.SalesArrangementTypeId) && !_currentUser.HasPermission(UserPermissions.CHANGE_REQUESTS_RefinancingAccess))
        {
            throw new CisAuthorizationException("Missing permission for Refinancing SA type");
        }
        // ostatni typy SA
        else if (_otherSATypes.Contains(request.SalesArrangementTypeId) && !_currentUser.HasPermission(UserPermissions.CHANGE_REQUESTS_Access))
        {
            throw new CisAuthorizationException("Missing permission for non-refinancing SA type");
        }
    }

    private static int[] _retentionSATypes = 
        [
            (int)SalesArrangementTypes.Refixation,
            (int)SalesArrangementTypes.Retention,
            (int)SalesArrangementTypes.MimoradnaSplatka
        ];

    private static int[] _otherSATypes =
        [
            (int)SalesArrangementTypes.Drawing,
            (int)SalesArrangementTypes.GeneralChange,
            (int)SalesArrangementTypes.HUBN,
            (int)SalesArrangementTypes.CustomerChange,
            (int)SalesArrangementTypes.CustomerChange3602A,
            (int)SalesArrangementTypes.CustomerChange3602B,
            (int)SalesArrangementTypes.CustomerChange3602C
        ];

    private readonly ICurrentUserAccessor _currentUser;
    private readonly CreateSalesArrangementParametersFactory _createService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;

    public CreateSalesArrangementHandler(
        CreateSalesArrangementParametersFactory createService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        ICurrentUserAccessor currentUser)
    {
        _createService = createService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _currentUser = currentUser;
    }
}
