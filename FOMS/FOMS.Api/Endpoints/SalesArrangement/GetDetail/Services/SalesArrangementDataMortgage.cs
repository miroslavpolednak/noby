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
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageDataResponse>(await _offerService.GetMortgageData(offerId.Value, cancellationToken));

        var loanKindName = (await _codebookService.LoanKinds(cancellationToken)).FirstOrDefault(t => t.Id == offerInstance.Inputs.LoanKindId)?.Name ?? "-";
        
        // create response object
        return new Dto.MortgageDetailDto()
        {
            MonthlyPayment = offerInstance.Inputs.LoanPaymentAmount,
            LoanKindId = offerInstance.Inputs.LoanKindId,
            ContractNumber = saCase.Data.ContractNumber,
            LoanInterestRate = offerInstance.Outputs.LoanInterestRate,
            LoanAmount = offerInstance.Outputs.LoanAmount,
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
	A.MesicniSplatka 'MonthlyPayment',
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