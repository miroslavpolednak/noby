using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V2.CreditWorthinessCalculateRequest, _V2.CreditWorthinessCalculateResponse>
{
    public async Task<_V2.CreditWorthinessCalculateResponse> Handle(_V2.CreditWorthinessCalculateRequest request, CancellationToken cancellation)
    {
        // appl type pro aktualni produkt
        var riskApplicationType = await getRiskApplicationType(request.Product!.ProductTypeId, cancellation);

        // request pro hlavni bonita sluzbu
        var requestModel = await _requestMapper.MapToC4m(request, riskApplicationType, cancellation);
        
        // volani c4m hlavni bonita sluzby
        var response = await _client.Calculate(requestModel, cancellation);

        // get DTI
        var dtiRequestModel = await _dtiRequestMapper.MapToC4m(request, riskApplicationType, cancellation);
        var dtiResponse = await _riskCharacteristicsClient.CalculateDti(dtiRequestModel, cancellation);

        // get DSTI
        var dstiRequestModel = await _dstiRequestMapper.MapToC4m(request, riskApplicationType, cancellation);
        var dstiResponse = await _riskCharacteristicsClient.CalculateDsti(dstiRequestModel, cancellation);

        return response.ToServiceResponse(dtiResponse.Dti, dstiResponse.Dsti, request.Product.LoanPaymentAmount);
    }

    private async Task<CodebookService.Contracts.Endpoints.RiskApplicationTypes.RiskApplicationTypeItem> getRiskApplicationType(int productTypeId, CancellationToken cancellationToken)
        => (await _codebookService.RiskApplicationTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw new CisValidationException(17006, $"ProductTypeId={productTypeId} is missing in RiskApplicationTypes codebook");

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Clients.CreditWorthiness.V1.ICreditWorthinessClient _client;
    private readonly Clients.RiskCharakteristics.V1.IRiskCharakteristicsClient _riskCharacteristicsClient;
    private readonly Mappers.CalculateRequestMapper _requestMapper;
    private readonly Mappers.DtiRequestMapper _dtiRequestMapper;
    private readonly Mappers.DstiRequestMapper _dstiRequestMapper;
    private readonly ILogger<CalculateHandler> _logger;

    public CalculateHandler(
        ILogger<CalculateHandler> logger,
        Mappers.CalculateRequestMapper requestMapper,
        Mappers.DtiRequestMapper dtiRequestMapper,
        Mappers.DstiRequestMapper dstiRequestMapper,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Clients.RiskCharakteristics.V1.IRiskCharakteristicsClient riskCharacteristicsClient,
        Clients.CreditWorthiness.V1.ICreditWorthinessClient client)
    {
        _logger = logger;
        _codebookService = codebookService;
        _riskCharacteristicsClient = riskCharacteristicsClient;
        _dtiRequestMapper = dtiRequestMapper;
        _dstiRequestMapper = dstiRequestMapper;
        _requestMapper = requestMapper;
        _client = client;
    }
}
