using C4M = Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness;
using Refit;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RipService.CreditWorthiness;

internal class CreditWorthinessHandler
    : IRequestHandler<Contracts.CreditWorthinessRequest, Contracts.CreditWorthinessResponse>
{
    private readonly Mpss.Rip.Infrastructure.RemoteServices.IC4M.ICreditWorthinessServices _c4MService;
    private readonly ILogger<CreditWorthinessHandler> _logger;
    private readonly ICreditWorthinessComputeRequestTransformation _requestTransformation;
    private readonly ICreditWorthinessComputeResponseTransformation _responseTransformation;

    public CreditWorthinessHandler(Mpss.Rip.Infrastructure.RemoteServices.IC4M.ICreditWorthinessServices c4MService, ILogger<CreditWorthinessHandler> logger, ICreditWorthinessComputeRequestTransformation requestTransformation, ICreditWorthinessComputeResponseTransformation responseTransformation)
    {
        _c4MService = c4MService;
        _logger = logger;
        _requestTransformation = requestTransformation;
        _responseTransformation = responseTransformation;
    }

    public async Task<Contracts.CreditWorthinessResponse> Handle(Contracts.CreditWorthinessRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request před transformací = {@Request}", request);
        var c4mRequest = await _requestTransformation.Transform(request);
        _logger.LogInformation("Volání C4M start. Request = {@Request}", c4mRequest);

        try
        {
            ApiResponse<C4M.CreditWorthinessCalculation> apiResponse = await _c4MService.CreditWorthinessCalculation(c4mRequest);
            if (!apiResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Volání C4M end. ERR. Chyba={apiResponse.Error} Reason={apiResponse.ReasonPhrase} Headers={apiResponse.ContentHeaders}");
                throw new Exception($"Chyba={apiResponse.Error} Reason={apiResponse.ReasonPhrase}");
            }
            _logger.LogInformation("Volání C4M end. OK. Response = {@Response}", apiResponse.Content);
            var ripResponse = _responseTransformation.Transform(apiResponse.Content, request);
            _logger.LogInformation("Response po transformaci = {@Response}", ripResponse);

            return ripResponse;
        }
        catch (ApiException ex)
        {
            // Extract the details of the error
            var errors = await ex.GetContentAsAsync<Dictionary<string, string>>();
            // Combine the errors into a string
            var message = string.Join("; ", errors.Values);
            // Throw a normal exception
            throw new Exception(message);
        }
    }
}
