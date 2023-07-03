using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3;
using _C4MRiskCharacteristics = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2;
using DomainServices.CodebookService.Contracts.v1;

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
        decimal? dti = null;
        decimal? dsti = null;
        try
        {
            var dtiRequestModel = await _dtiRequestMapper.MapToC4m(request, riskApplicationType, cancellation);
            dti = (await _riskCharacteristicsClient.CalculateDti(dtiRequestModel, cancellation)).Dti;
        }
        catch { }

        // get DSTI
        try
        {
            var dstiRequestModel = await _dstiRequestMapper.MapToC4m(request, riskApplicationType, cancellation);
            dsti = (await _riskCharacteristicsClient.CalculateDsti(dstiRequestModel, cancellation)).Dsti;
        }
        catch { }

        return response.ToServiceResponse(dti, dsti, request.Product.LoanPaymentAmount);
    }

    private async Task<RiskApplicationTypesResponse.Types.RiskApplicationTypeItem> getRiskApplicationType(int productTypeId, CancellationToken cancellationToken)
        => (await _codebookService.RiskApplicationTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId is not null && t.ProductTypeId.Contains(productTypeId))
        ?? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ProductTypeIdNotFound, productTypeId);

    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly _C4M.ICreditWorthinessClient _client;
    private readonly _C4MRiskCharacteristics.IRiskCharacteristicsClient _riskCharacteristicsClient;
    private readonly Mappers.CalculateRequestMapper _requestMapper;
    private readonly Mappers.DtiRequestMapper _dtiRequestMapper;
    private readonly Mappers.DstiRequestMapper _dstiRequestMapper;
    
    public CalculateHandler(
        Mappers.CalculateRequestMapper requestMapper,
        Mappers.DtiRequestMapper dtiRequestMapper,
        Mappers.DstiRequestMapper dstiRequestMapper,
        CodebookService.Clients.ICodebookServiceClient codebookService,
        _C4MRiskCharacteristics.IRiskCharacteristicsClient riskCharacteristicsClient,
        _C4M.ICreditWorthinessClient client)
    {
        _codebookService = codebookService;
        _riskCharacteristicsClient = riskCharacteristicsClient;
        _dtiRequestMapper = dtiRequestMapper;
        _dstiRequestMapper = dstiRequestMapper;
        _requestMapper = requestMapper;
        _client = client;
    }
}
