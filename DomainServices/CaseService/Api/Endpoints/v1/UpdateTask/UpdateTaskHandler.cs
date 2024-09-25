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
            case UpdateTaskRequest.AmendmentsOneofCase.MortgageRetention:
                metadata.Add("ukol_retence_sazba_dat_od", ((DateOnly)request.MortgageRetention.InterestRateValidFrom).ToSbFormat());
                metadata.Add("ukol_retence_sazba_kalk", request.MortgageRetention.LoanInterestRate.ToSbFormat());
                metadata.Add("ukol_retence_sazba_vysl", request.MortgageRetention.LoanInterestRateProvided.ToSbFormat());
                metadata.Add("ukol_retence_splatka_kalk", request.MortgageRetention.LoanPaymentAmount.ToSbFormat());
                metadata.Add("ukol_retence_splatka_vysl", request.MortgageRetention.LoanPaymentAmountFinal.ToSbFormat());
                metadata.Add("ukol_retence_popl_kalk", request.MortgageRetention.FeeSum.ToSbFormat());
                metadata.Add("ukol_retence_popl_vysl", request.MortgageRetention.FeeFinalSum.ToSbFormat());
                break;

            case UpdateTaskRequest.AmendmentsOneofCase.MortgageRefixation:
                metadata.Add("ukol_retence_sazba_kalk", request.MortgageRefixation.LoanInterestRate.ToSbFormat());
                metadata.Add("ukol_retence_sazba_vysl", request.MortgageRefixation.LoanInterestRateProvided.ToSbFormat());
                metadata.Add("ukol_retence_splatka_kalk", request.MortgageRefixation.LoanPaymentAmount.ToSbFormat());
                metadata.Add("ukol_retence_splatka_vysl", request.MortgageRefixation.LoanPaymentAmountFinal.ToSbFormat());
                metadata.Add("ukol_retence_perioda_fixace", request.MortgageRefixation.FixedRatePeriod.ToString(CultureInfo.InvariantCulture));
                break;

            case UpdateTaskRequest.AmendmentsOneofCase.MortgageExtraPayment:
                metadata.Add("ukol_mspl_dat_spl", ((DateOnly)request.MortgageExtraPayment.ExtraPaymentDate).ToSbFormat());
                metadata.Add("ukol_mspl_suma", request.MortgageExtraPayment.Principal.ToSbFormat());
                metadata.Add("ukol_mspl_suma_celkem", request.MortgageExtraPayment.ExtraPaymentAmountIncludingFee.ToSbFormat());
                metadata.Add("ukol_mspl_typ", request.MortgageExtraPayment.IsFinalExtraPayment.ToSbFormat());
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
