using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate;

internal sealed class SimpleCalculateHandler
    : IRequestHandler<_V2.CreditWorthinessSimpleCalculateRequest, _V2.CreditWorthinessSimpleCalculateResponse>
{
    public async Task<CreditWorthinessSimpleCalculateResponse> Handle(CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken)
    {
        // appl type pro aktualni produkt
        var riskApplicationType = await getRiskApplicationType(request.Product!.ProductTypeId, cancellationToken);

        // request pro hlavni bonita sluzbu
        var requestModel = await _requestMapper.MapToC4m(request.ToFullRequest(), riskApplicationType, cancellationToken);

        // volani c4m hlavni bonita sluzby
        var response = await _client.Calculate(requestModel, cancellationToken);

        return response.ToServiceResponse(request.Product.LoanPaymentAmount);
    }

    private async Task<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> getRiskApplicationType(int productTypeId, CancellationToken cancellationToken)
        => (await _codebookService.RiskApplicationTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw new CisValidationException(17006, $"ProductTypeId={productTypeId} is missing in RiskApplicationTypes codebook");

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Clients.CreditWorthiness.V1.ICreditWorthinessClient _client;
    private readonly Calculate.Mappers.CalculateRequestMapper _requestMapper;

    public SimpleCalculateHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Clients.CreditWorthiness.V1.ICreditWorthinessClient client,
        Calculate.Mappers.CalculateRequestMapper requestMapper)
    {
        _codebookService = codebookService;
        _client = client;
        _requestMapper = requestMapper;
    }
}
