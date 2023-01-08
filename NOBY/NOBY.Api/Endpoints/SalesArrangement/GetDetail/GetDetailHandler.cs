﻿using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail;

internal sealed class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var caseInstance = await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken);
        
        var parameters = getParameters(saInstance);
        /*Dto.MortgageDetailDto? data = null;
        if (saInstance.SalesArrangementTypeId == 1)
        {
            // get mortgage data
            var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId.Value, cancellationToken));

            data = await getDataInternal(saInstance, offerInstance, cancellationToken);
            if (!data.ExpectedDateOfDrawing.HasValue)
                data.ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing;
        }*/

        return new GetDetailResponse()
        {
            ProductTypeId = caseInstance.Data.ProductTypeId,
            SalesArrangementId = saInstance.SalesArrangementId,
            SalesArrangementTypeId = saInstance.SalesArrangementTypeId,
            LoanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId,
            CreatedBy = saInstance.Created.UserName,
            CreatedTime = saInstance.Created.DateTime,
            OfferGuaranteeDateFrom = saInstance.OfferGuaranteeDateFrom,
            OfferGuaranteeDateTo = saInstance.OfferGuaranteeDateTo,
            //Data = data,
            Parameters = parameters
        };
    }

    //TODO tohle se musi predelat az se bude vedet jak - rozdeleni mezi ProductSvc a Noby entity
    async Task<Dto.MortgageDetailDto> getDataInternal(_SA.SalesArrangement saInstance, _Offer.GetMortgageOfferResponse offerInstance, CancellationToken cancellationToken)
    {
        if (!saInstance.OfferId.HasValue)
            throw new CisArgumentNullException(ErrorCodes.SalesArrangementOfferIdIsNull, $"Offer does not exist for Case #{saInstance.OfferId}", "OfferId");

        var loanKindName = (await _codebookService.LoanKinds(cancellationToken)).FirstOrDefault(t => t.Id == offerInstance.SimulationInputs.LoanKindId)?.Name ?? "-";

        // create response object
        return new Dto.MortgageDetailDto()
        {
            ContractNumber = saInstance.ContractNumber,
            LoanKindName = loanKindName,
            LoanAmount = offerInstance.SimulationResults.LoanAmount,
            LoanInterestRate = offerInstance.SimulationResults.LoanInterestRateProvided,
            ContractSignedDate = offerInstance.SimulationResults.ContractSignedDate,
            DrawingDateTo = offerInstance.SimulationResults.DrawingDateTo,
            LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount,
            LoanKindId = offerInstance.SimulationInputs.LoanKindId,
            FixedRatePeriod = offerInstance.SimulationInputs.FixedRatePeriod!.Value,
            ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing,
            LoanDueDate = offerInstance.SimulationResults.LoanDueDate,
            PaymentDay = offerInstance.SimulationInputs.PaymentDay,
            LoanPurposes = offerInstance.SimulationInputs.LoanPurposes is null ? null : offerInstance.SimulationInputs.LoanPurposes.Select(x => new Dto.MortgageDetailLoanPurpose
            {
                LoanPurposeId = x.LoanPurposeId,
                Sum = x.Sum
            }).ToList()
        };
    }

    static object? getParameters(_SA.SalesArrangement saInstance)
        => saInstance.ParametersCase switch
        {
            _SA.SalesArrangement.ParametersOneofCase.Mortgage => saInstance.Mortgage.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.Drawing => saInstance.Drawing.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.GeneralChange => saInstance.GeneralChange.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.HUBN => saInstance.HUBN.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.None => null,
            _ => throw new NotImplementedException($"getParameters for {saInstance.ParametersCase} not implemented")
        };

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    
    public GetDetailHandler(
        ICaseServiceClient caseService,
        DomainServices.CodebookService.Clients.ICodebookServiceClients codebookService,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _codebookService = codebookService;
        _offerService = offerService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}