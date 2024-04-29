using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;

internal sealed class GetMortgageExtraPaymentListHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService)
    : IRequestHandler<GetMortgageExtraPaymentListRequest, List<GetMortgageExtraPaymentListResponse>>
{
    public async Task<List<GetMortgageExtraPaymentListResponse>> Handle(GetMortgageExtraPaymentListRequest request, CancellationToken cancellationToken)
    {
        // vsechny SA
        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        // vsechny procesy
        var allProcesses = await _caseService.GetProcessList(request.CaseId, cancellationToken);

        // vyber extra paymenty, ktere nejsou zrusene
        var extraPayments = allProcesses
            .Where(t => t.ProcessTypeId == (int)RefinancingTypes.MortgageExtraPayment && !t.Cancelled && t.AmendmentsCase == DomainServices.CaseService.Contracts.ProcessTask.AmendmentsOneofCase.MortgageExtraPayment)
            .ToList();

        return extraPayments.Select(process =>
        {
            var currentProcessSA = saList.SalesArrangements.FirstOrDefault(t => t.ProcessId == process.ProcessId);
            RefinancingHelper.GetRefinancingState((SalesArrangementStates)currentProcessSA.State);
            RefinancingHelper.GetRefinancingState(false, currentProcessSA?.ProcessId, process);
            return new GetMortgageExtraPaymentListResponse
            {
                CreatedOn = DateTime.Now,//????
                ExtraPaymentAmount = process.MortgageExtraPayment.ExtraPaymentAmountIncludingFee,
                PrincipalAmount = process.MortgageExtraPayment.ExtraPaymentAmount,
                ExtraPaymentDate = DateOnly.FromDateTime(process.MortgageExtraPayment.ExtraPaymentDate),
                PaymentState = process.MortgageExtraPayment.PaymentState,
                IsExtraPaymentFullyRepaid = process.MortgageExtraPayment.IsFinalExtraPayment,
                RefinancingStateId = 1
            };
        })
        .ToList();
    }
}
