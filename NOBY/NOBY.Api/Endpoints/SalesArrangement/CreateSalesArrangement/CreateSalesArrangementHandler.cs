namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler(
    CreateSalesArrangementParametersFactory _createService,
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService,
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService,
    NOBY.Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization)
        : IRequestHandler<SalesArrangementCreateSalesArrangementRequest, SalesArrangementCreateSalesArrangementResponse>
{
    public async Task<SalesArrangementCreateSalesArrangementResponse> Handle(SalesArrangementCreateSalesArrangementRequest request, CancellationToken cancellationToken)
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

        return new SalesArrangementCreateSalesArrangementResponse
        {
            SalesArrangementId = newSaId,
            ProcessId = createRequest.ProcessId
        };
    }

    private async Task validatePermissions(SalesArrangementCreateSalesArrangementRequest request, CancellationToken cancellationToken)
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
}
