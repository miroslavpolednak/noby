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
            // posledni dohoda je takova, ze se vraci data jen z nasich entit
            //SalesArrangementStates.InProgress => await getDataInternal(caseId, offerId, cancellationToken),
            //_ => await getDataKonsDb(caseId, cancellationToken)
            _ => await getDataInternal(caseId, offerId, cancellationToken),
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

        // create response object
        return new Dto.MortgageDetailDto()
        {
            ContractNumber = saCase.Data.ContractNumber,
            ProductName = loanKindName,
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

    /*

    SELECT
	A.CisloSmlouvy 'ContractNumber',
	B.Text 'ProductName',
	A.VyseUveru 'LoanAmount',
	A.RadnaSazba 'LoanInterestRate',
	A.DelkaFixaceUrokoveSazby 'FixedRatePeriod ',
	A.TypUveru 'ProductTypeId',
	A.MesicniSplatka 'LoanPaymentAmount',
	A.ZustatekCelkem 'CurrentAmount',
	A.DatumKonceCerpani 'DrawingDateTo',
	A.DatumUzavreniSmlouvy 'ContractStartDate',
	A.DatumFixaceUrokoveSazby 'FixationDate',
	A.ZbyvaCerpat 'AmountToWithdraw',
	--dbo.VztahUver.PartnerId
	--dbo.VztahUver.VztahId
	A.DatumUzavreniSmlouvy 'ContractSignedDate',
	A.DatumFixaceUrokoveSazby 'FixedRateValidTo',
	A.ZbyvaCerpat 'AvailableForDrawing',
	null 'Principal', -- A.Jistina ???
	A.DruhUveru 'LoanKindId',
	A.DatumPrvniVyplatyZUveru 'ExpectedDateOfDrawing',
	null 'LoanDueDate', -- DatumPredpSplatnosti, DatumZbytkoveSplCelkem ???
	null 'PaymentDay', -- ???
	null 'LoanInterestRateRefix', -- ???
	null 'LoanInterestRateValidFromRefix', -- ???
	null 'FixedRatePeriodRefix', -- ???
	null 'Cpm', -- A.CPM ???
	A.PoradceId 'Icp',
	null 'LoanTermsValidFrom',
	null 'YearlyAccountStatement',
    A.DruhUveru 'DruhUveru',
    A.DelkaFixaceUrokoveSazby 'DelkaFixaceUrokoveSazby'
FROM dbo.Uver A
INNER JOIN cis.AkceUveru B ON A.AkceUveruId=B.Id
--WHERE A.Id=@id

     */
}