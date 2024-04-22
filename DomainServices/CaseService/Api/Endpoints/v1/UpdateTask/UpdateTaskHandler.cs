using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.V1;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateTask;

internal sealed class UpdateTaskHandler(ISbWebApiClient _sbWebApi) 
    : IRequestHandler<UpdateTaskRequest, Empty>
{
    public async Task<Empty> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        Dictionary<string, string?> metadata = [];

        switch (request.AmendmentsCase)
        {
            case UpdateTaskRequest.AmendmentsOneofCase.Retention:
                metadata.Add("ukol_retence_sazba_dat_od", ((DateOnly)request.Retention.InterestRateValidFrom).ToSbFormat());
                metadata.Add("ukol_retence_sazba_kalk", request.Retention.LoanInterestRate.ToSbFormat());
                metadata.Add("ukol_retence_sazba_vysl", request.Retention.LoanInterestRateProvided.ToSbFormat());
                metadata.Add("ukol_retence_splatka_kalk", request.Retention.LoanPaymentAmount.ToSbFormat());
                metadata.Add("ukol_retence_splatka_vysl", request.Retention.LoanPaymentAmountFinal.ToSbFormat());
                metadata.Add("ukol_retence_popl_kalk", request.Retention.FeeSum.ToSbFormat());
                metadata.Add("ukol_retence_popl_vysl", request.Retention.FeeFinalSum.ToSbFormat());
                break;

            case UpdateTaskRequest.AmendmentsOneofCase.Refixation:
                metadata.Add("ukol_retence_sazba_dat_od", ((DateOnly)request.Refixation.InterestRateValidFrom).ToSbFormat());
                metadata.Add("ukol_retence_sazba_kalk", request.Refixation.LoanInterestRate.ToSbFormat());
                metadata.Add("ukol_retence_sazba_vysl", request.Refixation.LoanInterestRateProvided.ToSbFormat());
                metadata.Add("ukol_retence_splatka_kalk", request.Refixation.LoanPaymentAmount.ToSbFormat());
                metadata.Add("ukol_retence_splatka_vysl", request.Refixation.LoanPaymentAmountFinal.ToSbFormat());
                break;
        }
        
        await _sbWebApi.UpdateTask(new ExternalServices.SbWebApi.Dto.UpdateTask.UpdateTaskRequest 
        { 
            TaskIdSb = request.TaskIdSb, 
            Metadata = metadata 
        }, cancellationToken);

        return new Empty();
    }
}
