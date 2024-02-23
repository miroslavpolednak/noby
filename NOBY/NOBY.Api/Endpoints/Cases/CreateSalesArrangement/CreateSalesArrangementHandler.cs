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

        _salesArrangementAuthorization.ValidateRefinancingPermissions(request.SalesArrangementTypeId, UserPermissions.CHANGE_REQUESTS_RefinancingAccess, UserPermissions.CHANGE_REQUESTS_Access);
    }

    private readonly NOBY.Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly CreateSalesArrangementParametersFactory _createService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;

    public CreateSalesArrangementHandler(
        CreateSalesArrangementParametersFactory createService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        NOBY.Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _createService = createService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }
}
