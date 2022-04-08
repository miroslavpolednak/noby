using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using CaseContracts = DomainServices.CaseService.Contracts;
using OfferContracts = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Services;

internal class SalesArrangementDataMortgage : ISalesArrangementDataService
{
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> _connectionProvider;

    public SalesArrangementDataMortgage(
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService, 
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> connectionProvider)
    {
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
        return await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<Dto.MortgageDetailDto>(_konsDbSqlQuery, new {id = caseId}, cancellationToken)
            ?? throw new CisNotFoundException(ErrorCodes.CaseNotFoundInKonsDb, $"Case #{caseId} not found in KonsDb");
    }

    async Task<Dto.MortgageDetailDto> getDataInternal(long caseId, int? offerId, CancellationToken cancellationToken)
    {
        if (!offerId.HasValue)
            throw new CisArgumentNullException(ErrorCodes.SalesArrangementOfferIdIsNull, $"Offer does not exist for Case #{caseId}", nameof(offerId));
        
        // instance Case
        var saCase = ServiceCallResult.Resolve<CaseContracts.Case>(await _caseService.GetCaseDetail(caseId, cancellationToken));
        
        // get mortgage data
        var offerInstance = ServiceCallResult.Resolve<OfferContracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(offerId.Value, cancellationToken));
        
        // create response object
        return new Dto.MortgageDetailDto()
        {
            ContractNumber = saCase.Data.ContractNumber,
            LoanInterestRate = offerInstance.Outputs.LoanInterestRate,
            LoanAmount = offerInstance.Outputs.LoanAmount,
            ProductName = "co tady ma byt?"
        };
    }

    private const string _konsDbSqlQuery = @"SELECT
	A.CisloSmlouvy 'ContractNumber',
	B.Text 'ProductName',
	A.VyseUveru 'LoanAmount',
	A.RadnaSazba 'InterestRate',
	A.DatumUzavreniSmlouvy 'ContractStartDate',
	A.DatumFixaceUrokoveSazby 'FixationDate',
	A.ZustatekCelkem 'AccountBalance',
	A.ZbyvaCerpat 'AmountToWithdraw',
	A.DatumKonceCerpani 'DateOfDrawing',
	A.MesicniSplatka 'MonthlyPayment',
	null 'LoanTermsValidFrom',
	null 'YearlyAccountStatement'
FROM dbo.Uver A
INNER JOIN cis.AkceUveru B ON A.AkceUveruId=B.Id
WHERE A.Id=@id";
}