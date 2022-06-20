using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using _Case = DomainServices.CaseService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using _Prod = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Services;

internal class SalesArrangementDataMortgage : ISalesArrangementDataService
{
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> _connectionProvider;

    public SalesArrangementDataMortgage(
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService, 
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> connectionProvider)
    {
        _codebookService = codebookService;
        _connectionProvider = connectionProvider;
        _caseService = caseService;
        _offerService = offerService;
    }
    
    public async Task<Dto.MortgageDetailDto> GetData(
        long caseId,
        int? offerId,
        SalesArrangementStates salesArrangementState,
        CancellationToken cancellationToken)
        => salesArrangementState switch
        {
            SalesArrangementStates.InProgress => await getDataInternal(caseId, offerId, cancellationToken),
            _ => await getDataKonsDb(caseId, cancellationToken)
        };

    async Task<Dto.MortgageDetailDto> getDataKonsDb(long caseId, CancellationToken cancellationToken)
    {
        var model = await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<Dto.MortgageDetailDto>(_konsDbSqlQuery, new {id = caseId}, cancellationToken)
            ?? throw new CisNotFoundException(ErrorCodes.CaseNotFoundInKonsDb, $"Case #{caseId} not found in KonsDb");

        model.ProductName = (await _codebookService.LoanKinds(cancellationToken))
            .FirstOrDefault(t => t.Id == model.LoanKindId)?
            .Name ?? "-";

        return model;
    }

    /*async Task<Dto.MortgageDetailDto> getDataFromProductService(long caseId, CancellationToken cancellationToken)
    {
        var data = ServiceCallResult.ResolveAndThrowIfError<_Prod.GetMortgageResponse>(await _productService.GetMortgage(caseId, cancellationToken));
        return new Dto.MortgageDetailDto
        {
            ContractNumber = data.Mortgage.ContractNumber,
            DateOfDrawing = data.Mortgage.DrawingMaxOn
        };
    }*/

    async Task<Dto.MortgageDetailDto> getDataInternal(long caseId, int? offerId, CancellationToken cancellationToken)
    {
        if (!offerId.HasValue)
            throw new CisArgumentNullException(ErrorCodes.SalesArrangementOfferIdIsNull, $"Offer does not exist for Case #{caseId}", nameof(offerId));
        
        // instance Case
        var saCase = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(caseId, cancellationToken));
        
        // get mortgage data
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(offerId.Value, cancellationToken));

        var loanKindName = (await _codebookService.LoanKinds(cancellationToken)).FirstOrDefault(t => t.Id == offerInstance.SimulationInputs.LoanKindId)?.Name ?? "-";

        // loan purpose
        string? loanPurposeName = null;
        decimal? loanPurposeSum = null;
        if (offerInstance.SimulationInputs.LoanPurposes is not null && offerInstance.SimulationInputs.LoanPurposes.Any())
        {
            var purpose = offerInstance.SimulationInputs.LoanPurposes.First();
            
            loanPurposeName = (await _codebookService.LoanPurposes(cancellationToken)).FirstOrDefault(t => t.Id == purpose.LoanPurposeId)?.Name;
            loanPurposeSum = purpose.Sum;
        }

        // create response object
        return new Dto.MortgageDetailDto()
        {
            LoanPurposeName = loanPurposeName,
            LoanPurposeSum = loanPurposeSum,
            PaymentDay = offerInstance.SimulationInputs.PaymentDay,
            LoanDueDate = offerInstance.SimulationResults.LoanDueDate,
            ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing,
            FixedRatePeriod = offerInstance.SimulationInputs.FixedRatePeriod,
            LoanInterestRateProvided = offerInstance.SimulationResults.LoanInterestRateProvided,
            LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount,
            LoanKindId = offerInstance.SimulationInputs.LoanKindId,
            ContractNumber = saCase.Data.ContractNumber,
            LoanInterestRate = offerInstance.SimulationResults.LoanInterestRate,
            LoanAmount = offerInstance.SimulationResults.LoanAmount,
            ProductName = loanKindName
        };
    }

    private const string _konsDbSqlQuery = @"SELECT
    A.AkceUveruId 'LoanKindId',
	A.CisloSmlouvy 'ContractNumber',
	B.Text 'ProductName',
	A.VyseUveru 'LoanAmount',
	A.RadnaSazba 'LoanInterestRate',
	A.DatumUzavreniSmlouvy 'ContractStartDate',
	A.DatumFixaceUrokoveSazby 'FixationDate',
	A.ZustatekCelkem 'AccountBalance',
	A.ZbyvaCerpat 'AmountToWithdraw',
	A.DatumKonceCerpani 'DateOfDrawing',
	A.MesicniSplatka 'LoanPaymentAmount',
	null 'LoanTermsValidFrom',
	null 'YearlyAccountStatement',

    A.Jistina 'Jistina',
    A.DruhUveru 'DruhUveru',
    A.DelkaFixaceUrokoveSazby 'DelkaFixaceUrokoveSazby',
    A.UcelUveru 'UcelUveru',
    A.DatumPrvniVyplatyZUveru 'DatumPrvniVyplatyZUveru',
    A.DatumPredpSplatnosti 'DatumPredpSplatnosti'
FROM dbo.Uver A
INNER JOIN cis.AkceUveru B ON A.AkceUveruId=B.Id
WHERE A.Id=@id";
}