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
        Dictionary<string, string?> metadata = new()
        {
            { "ukol_retence_sazba_dat_od", ((DateOnly)request.Retention.InterestRateValidFrom!).ToSbFormat() },
            { "ukol_retence_sazba_kalk", request.Retention.LoanInterestRate!.ToSbFormat() },
            { "ukol_retence_sazba_vysl", request.Retention.LoanInterestRateProvided!.ToSbFormat() },
            { "ukol_retence_splatka_kalk", request.Retention.LoanPaymentAmount.ToSbFormat() },
            { "ukol_retence_splatka_vysl", request.Retention.LoanPaymentAmountFinal.ToSbFormat() },
            { "ukol_retence_popl_kalk", request.Retention.FeeSum?.ToSbFormat()},
            { "ukol_retence_popl_vysl", request.Retention.FeeFinalSum?.ToSbFormat()},
            //{ "ukol_refixace_TBD", request.Retention.FixedRatePeriod?.ToString(CultureInfo.InvariantCulture) } HACH-10693 SB neumí refixaci
        };

        await _sbWebApi.UpdateTask(new ExternalServices.SbWebApi.Dto.UpdateTask.UpdateTaskRequest { TaskIdSb = request.TaskIdSb, Metadata = metadata }, cancellationToken);

        return new Empty();
    }
}
