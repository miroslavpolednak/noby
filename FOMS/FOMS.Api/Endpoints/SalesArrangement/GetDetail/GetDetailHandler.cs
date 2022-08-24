using CIS.Foms.Enums;
using DomainServices.CaseService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _CA = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_CA.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));

        // get mortgage data
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId.Value, cancellationToken));

        var parameters = getParameters(saInstance);
        var data = await getDataInternal(saInstance, offerInstance, cancellationToken);
        if (!data.ExpectedDateOfDrawing.HasValue)
            data.ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing;

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
            Data = data,
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

    static SalesArrangement.Dto.ParametersMortgage? getParameters(_SA.SalesArrangement saInstance)
        => saInstance.ParametersCase switch
        {
            _SA.SalesArrangement.ParametersOneofCase.Mortgage => new SalesArrangement.Dto.ParametersMortgage
            {
                ContractSignatureTypeId = saInstance.Mortgage.ContractSignatureTypeId,
                SalesArrangementSignatureTypeId = saInstance.Mortgage.SalesArrangementSignatureTypeId,
                ExpectedDateOfDrawing = saInstance.Mortgage.ExpectedDateOfDrawing,
                IncomeCurrencyCode = saInstance.Mortgage.IncomeCurrencyCode,
                ResidencyCurrencyCode = saInstance.Mortgage.ResidencyCurrencyCode,
                Agent = saInstance.Mortgage.Agent,
                LoanRealEstates = saInstance.Mortgage.LoanRealEstates is null ? null : saInstance.Mortgage.LoanRealEstates.Select(x => new SalesArrangement.Dto.LoanRealEstateDto
                {
                    IsCollateral = x.IsCollateral,
                    RealEstatePurchaseTypeId = x.RealEstatePurchaseTypeId,
                    RealEstateTypeId = x.RealEstateTypeId
                }).ToList()
            },
            _SA.SalesArrangement.ParametersOneofCase.None => null,
            _ => throw new NotImplementedException("Api/SalesArrangement/GetDetailHandler/getParameters")
        };

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly Services.SalesArrangementDataFactory _dataFactory;
    
    public GetDetailHandler(
        ICaseServiceAbstraction caseService,
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        Services.SalesArrangementDataFactory dataFactory,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _codebookService = codebookService;
        _offerService = offerService;
        _caseService = caseService;
        _dataFactory = dataFactory;
        _salesArrangementService = salesArrangementService;
    }
}