using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.V1;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CaseService.Api.Endpoints.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskRequest, Empty>
{
    private readonly ISbWebApiClient _sbWebApi;

    public UpdateTaskHandler(ISbWebApiClient sbWebApi)
    {
        _sbWebApi = sbWebApi;
    }

    public async Task<Empty> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> metadata = new()
        {
            { "ukol_retence_sazba_dat_od", ((DateOnly)request.Retention.InterestRateValidFrom!).ToSbFormat() },
            { "ukol_retence_sazba_kalk", request.Retention.LoanInterestRate!.ToSbFormat() },
            { "ukol_retence_sazba_vysl", request.Retention.LoanInterestRateProvided!.ToSbFormat() },
            { "ukol_retence_splatka_kalk", request.Retention.LoanPaymentAmount!.Value.ToString(CultureInfo.InvariantCulture) },
            { "ukol_retence_splatka_vysl", request.Retention.LoanPaymentAmountFinal!.Value.ToString(CultureInfo.InvariantCulture) },
            { "ukol_retence_popl_kalk", request.Retention.FeeSum!.Value.ToString(CultureInfo.InvariantCulture)},
            { "ukol_retence_popl_vysl", request.Retention.FeeFinalSum !.Value.ToString(CultureInfo.InvariantCulture)}
        };

        await _sbWebApi.UpdateTask(new() { TaskIdSb = request.TaskIdSb, Metadata = metadata }, cancellationToken);

        return new();
    }
}
