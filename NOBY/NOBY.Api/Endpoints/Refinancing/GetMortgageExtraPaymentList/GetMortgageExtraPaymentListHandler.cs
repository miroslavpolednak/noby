﻿using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;

internal sealed class GetMortgageExtraPaymentListHandler(
    ICodebookServiceClient _codebookService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService)
    : IRequestHandler<GetMortgageExtraPaymentListRequest, List<RefinancingGetMortgageExtraPaymentListResponse>>
{
    public async Task<List<RefinancingGetMortgageExtraPaymentListResponse>> Handle(GetMortgageExtraPaymentListRequest request, CancellationToken cancellationToken)
    {
        // vsechny procesy
        var allProcesses = await _caseService.GetProcessList(request.CaseId, cancellationToken);

        // vsechny SA
        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        
        // vyber extra paymenty, ktere nejsou zrusene
        var extraPayments = allProcesses
            .Where(t => t.ProcessTypeId == (int)EnumRefinancingTypes.MortgageExtraPayment && !t.Cancelled && t.AmendmentsCase == DomainServices.CaseService.Contracts.ProcessTask.AmendmentsOneofCase.MortgageExtraPayment)
            .ToList();

        var refinancingStates = await _codebookService.RefinancingStates(cancellationToken);

        return extraPayments.Select(process =>
        {
            var currentProcessSA = saList.SalesArrangements.FirstOrDefault(t => t.ProcessId == process.ProcessId);
            
            //TODO: co s timhle?
            var refinancingState = RefinancingHelper.GetRefinancingState((SharedTypes.Enums.EnumSalesArrangementStates)(currentProcessSA?.State ?? 0), false, process);
            var state = refinancingStates.First(t => t.Id == (int)refinancingState);

			return new RefinancingGetMortgageExtraPaymentListResponse
            {
                ProcessId = process.ProcessId,
                SalesArrangementId = currentProcessSA?.SalesArrangementId,
                CreatedOn = process.CreatedOn,
                ExtraPaymentAmount = process.MortgageExtraPayment.ExtraPaymentAmountIncludingFee,
                PrincipalAmount = process.MortgageExtraPayment.Principal,
                ExtraPaymentDate = DateOnly.FromDateTime(process.MortgageExtraPayment.ExtraPaymentDate),
                PaymentState = process.MortgageExtraPayment.PaymentState,
                IsExtraPaymentFullyRepaid = process.MortgageExtraPayment.IsFinalExtraPayment,
                RefinancingStateId = refinancingState,
                StateIndicator = (EnumStateIndicators)state.Indicator,
				StateName = state.Name
			};
        })
        .ToList();
    }
}
