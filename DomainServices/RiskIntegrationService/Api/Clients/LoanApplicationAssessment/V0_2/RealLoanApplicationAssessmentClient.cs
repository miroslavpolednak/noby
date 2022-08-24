using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2.Contracts;
using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2;

internal sealed class RealLoanApplicationAssessmentClient
    : ILoanApplicationAssessmentClient
{
    public async Task<Identified> GetAssesment(string loanApplicationAssessmentId, List<string>? requestedDetails, CancellationToken cancellationToken)
    {
        string path = requestedDetails != null && requestedDetails.Any()
            ? string.Format(default, _getUrlWithExpand, loanApplicationAssessmentId, string.Join("&expand=", requestedDetails))
            : string.Format(default, _getUrl, loanApplicationAssessmentId);

        var response = await _httpClient
            .GetAsync(_httpClient.BaseAddress + path, cancellationToken)
            .ConfigureAwait(false); ;

        var result = await response.Content.ReadFromJsonAsync<Identified>(_jsonOptions, cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(GetAssesment), nameof(Identified));

        return result;
    }

    private readonly HttpClient _httpClient;

    const string _getUrl = "/loan-application-assessment/{0}";
    const string _getUrlWithExpand = "/loan-application-assessment/{0}?expand={1}";

    public RealLoanApplicationAssessmentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private static System.Text.Json.JsonSerializerOptions _jsonOptions = new()
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString //TODO odstranit az c4m opravi format cisel
    };

    static RealLoanApplicationAssessmentClient()
    {
        _jsonOptions.Converters.Add(new CIS.Infrastructure.Json.DateTimeOffsetConverterUsingDateTimeParse()); //TODO odstranit az c4m opravi format cisel
    }
}
