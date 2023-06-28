namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // kontrola na kategorii
        if ((await _codebookService.SalesArrangementTypes(cancellationToken)).FirstOrDefault(t => t.Id == request.SalesArrangementTypeId)?.SalesArrangementCategory != 2)
            throw new CisValidationException($"SalesArrangement type not supported");

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

    private readonly CreateSalesArrangementParametersFactory _createService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    
    public CreateSalesArrangementHandler(
        CreateSalesArrangementParametersFactory createService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService, 
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _createService = createService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}
