using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal sealed class CalculateHandler
    : IRequestHandler<_V2.CreditWorthinessCalculateRequest, _V2.CreditWorthinessCalculateResponse>
{
    public async Task<_V2.CreditWorthinessCalculateResponse> Handle(_V2.CreditWorthinessCalculateRequest request, CancellationToken cancellation)
    {
        // request pro hlavni bonita sluzbu
        var requestModel = await _requestMapper.MapToC4m(request, cancellation);
        
        // volani c4m hlavni bonita sluzby
        var response = await _client.Calculate(requestModel, cancellation);

        // get DTI
        var dtiRequestModel = await _dtiRequestMapper.MapToC4m(request, cancellation);
        var dtiResponse = await _riskCharacteristicsClient.CalculateDti(dtiRequestModel, cancellation);

        // get DSTI
        //var dstiRequestModel = await _dstiRequestMapper.MapToC4m(request, cancellation);
        //var dstiResponse = await _riskCharacteristicsClient.CalculateDsti(dstiRequestModel, cancellation);

        return response.ToServiceResponse();
    }

    private readonly Clients.CreditWorthiness.V1.ICreditWorthinessClient _client;
    private readonly Clients.RiskCharakteristics.V1.IRiskCharakteristicsClient _riskCharacteristicsClient;
    private readonly Mappers.CalculateRequestMapper _requestMapper;
    private readonly Mappers.DtiRequestMapper _dtiRequestMapper;

    public CalculateHandler(
        Mappers.CalculateRequestMapper requestMapper,
        Mappers.DtiRequestMapper dtiRequestMapper,
        Clients.RiskCharakteristics.V1.IRiskCharakteristicsClient riskCharacteristicsClient,
        Clients.CreditWorthiness.V1.ICreditWorthinessClient client)
    {
        _riskCharacteristicsClient = riskCharacteristicsClient;
        _dtiRequestMapper = dtiRequestMapper;
        _requestMapper = requestMapper;
        _client = client;
    }
}
