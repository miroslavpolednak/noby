using System.Data;
using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.ProductService.Api.Database.Abstraction;

namespace DomainServices.ProductService.Api.Database;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal class LoanRepository
{
    private readonly IConnectionProvider<IProductServiceConnectionProvider> _connectionProvider;
    
    public LoanRepository(
        IConnectionProvider<IProductServiceConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    public async Task<Models.Loan?> GetLoan(long loanId, CancellationToken cancellation)
    { 
	    const string query = @"SELECT
		[Id] as Id,
		[PartnerId] as PartnerId,
		[KodProduktyUv] as ProductTypeId,
		[CisloSmlouvy] as ContractNumber,
		[MesicniSplatka] as LoanPaymentAmount,
		[VyseUveru] as LoanAmount,
		[RadnaSazba] as LoanInterestRate,
		[PeriodaFixace] as FixedRatePeriod,
		[DruhUveru] as LoanKindId,
		[PredpDatum1Cerpani] as ExpectedDateOfDrawing,
		[DatumPodpisuPrvniZadosti] as FirstSignatureDate,
		[PoradceId] as CaseOwnerUserCurrentId,
		[PoradceOrigId] as CaseOwnerUserOrigId,
		[ZbyvaCerpat] as AvailableForDrawing,
		[ZustatekCelkem] as CurrentAmount,
		[DatumKonceCerpani] as DrawingDateTo,
		[DatumUzavreniSmlouvy] as ContractSignedDate,
		[DatumFixaceUrokoveSazby] as FixedRateValidTo,
		[Datum1AnuitniSplatky] as FirstAnnuityInstallmentDate,
		[DatumPredpSplatnosti] as LoanDueDate,
		[CisloUctu] as PaymentAccountNumber,
		[PredcisliUctu] as PaymentAccountPrefix,
		[Jistina] as Principal,
		[CelkovyDluhPoSplatnosti] as CurrentOverdueAmount,
		[PohledavkaPoplatkyPo] as AllOverdueFees,
		[PocetBankovnichDniPoSpl] as OverdueDaysNumber,
		[SazbaZProdleni] as InterestInArrears,
		[SplatkyDen] as PaymentDay,
		[PobockaObsluhyId] as BranchConsultantId,
		[InkasoPredcisli] as RepaymentAccountPrefix,
		[InkasoCislo] as RepaymentAccountNumber,
		[InkasoBanka] as RepaymentAccountBankCode,
		[HuVypisFrekvence] as StatementFrequencyId,
		[HuVypisZodb] as StatementSubscriptionTypeId,
		[HuVypisTyp] as StatementTypeId,
		[VypisEmail1] as EmailAddress1,
	    [VypisEmail2] as EmailAddress2,
        [Neaktivni] as IsCancelled
    	FROM [dbo].[Uver]
    	WHERE Id = @LoanId AND Neaktivni = 0";
	    
        var parameters = new DynamicParameters();
        parameters.Add("@LoanId", loanId, DbType.Int64, ParameterDirection.Input);
        
        return await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Models.Loan>(query, parameters, cancellation);
    }

    public async Task<List<Models.LoanPurpose>> GetLoanPurposes(long loanId, CancellationToken cancellation)
    {
	    const string query = @"SELECT
    	[UcelUveruId] as LoanPurposeId,
    	[SumaUcelu] as Sum
		FROM [dbo].[UverUcely]
		WHERE UverId = @LoanId";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@LoanId", loanId, DbType.Int64, ParameterDirection.Input);
        
	    return await _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.LoanPurpose>(query, parameters, cancellation);
    }
    
    public async Task<Models.LoanStatement?> GetLoanStatement(long loanId, CancellationToken cancellation)
    {
	    const string query = @"SELECT
		[Ulice] as Street,
		[CisloDomu4] as StreetNumber,
		[CisloDomu2] as HouseNumber,
		[Psc] as Postcode,
		[Mesto] as City,
		[ZemeId] as CountryId,
		[StatPodkategorie] as AddressPointId
		FROM [dbo].[UverVypisy]
		WHERE Id = @LoanId";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@LoanId", loanId, DbType.Int64, ParameterDirection.Input);

	    return await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Models.LoanStatement>(query, parameters, cancellation);
    }
    
    public async Task<List<Models.Relationship>> GetRelationships(long loanId, CancellationToken cancellation)
    {
		const string query = @"SELECT
		[PartnerId] as PartnerId,
		[VztahId] as ContractRelationshipTypeId,
    	[KbId] as KbId,
    	[Zmocnenec] as Agent,
    	[StavKyc] as Kyc
		FROM [dbo].[VztahUver] LEFT JOIN [dbo].[Partner] ON [VztahUver].PartnerId = [Partner].Id
		WHERE UverId = @LoanId AND Neaktivni = 0";

		var parameters = new DynamicParameters();
        parameters.Add("@LoanId", loanId, DbType.Int64, ParameterDirection.Input);
        
        return await _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.Relationship>(query, parameters, cancellation);
    }

    public async Task<Models.Covenant?> GetCovenant(long caseId, int order, CancellationToken cancellation)
    {
	    const string query = @"SELECT
		[UverId] as CaseId,  
		[PoradoveCislo] as [Order],
		[TextNazevProKlienta] as Name,
		[TextVysvetlujiciDokument] as Description,
		[TextDoUveroveSmlouvy] as Text,
		[PriznakSplnena] as IsFulFilled,
		[SplnitDo] as FulfillDate,
		[TypSmlouvaPoradiPismeno] as OrderLetter,
		[TypSmlouvy] as CovenantTypeId,
		[FazePoradi] as PhaseOrder
		FROM [dbo].[Terminovnik]
		WHERE UverId = @CaseId AND PoradoveCislo = @Order";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@CaseId", caseId, DbType.Int64, ParameterDirection.Input);
	    parameters.Add("@Order", order, DbType.Int32, ParameterDirection.Input);
	 
	    return await _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Models.Covenant>(query, parameters, cancellation);
    }

    public async Task<List<Models.Covenant>> GetCovenants(long caseId, CancellationToken cancellation)
    {
	    const string query = @"SELECT
		[UverId] as CaseId,  
		[PoradoveCislo] as [Order],
		[TextNazevProKlienta] as Name,
		[TextVysvetlujiciDokument] as Description,
		[TextDoUveroveSmlouvy] as Text,
		[PriznakSplnena] as IsFulFilled,
		[SplnitDo] as FulfillDate,
		[TypSmlouvaPoradiPismeno] as OrderLetter,
		[TypSmlouvy] as CovenantTypeId,
		[FazePoradi] as PhaseOrder
		FROM [dbo].[Terminovnik]
		WHERE UverId = @CaseId";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@CaseId", caseId, DbType.Int64, ParameterDirection.Input);
	 
	    return await _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.Covenant>(query, parameters, cancellation);
    }
    
    public async Task<List<Models.CovenantPhase>> GetCovenantPhases(long caseId, CancellationToken cancellation)
    {
	    const string query = @"SELECT
		[UverId] as CaseId,   
		[Nazev] as Name,
		[Poradi] as [Order],
		[PoradiPismeno] as OrderLetter
		FROM [dbo].[TerminovnikFaze]
		WHERE UverId = @CaseId";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@CaseId", caseId, DbType.Int64, ParameterDirection.Input);
	 
	    return await _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.CovenantPhase>(query, parameters, cancellation);
    }
    
    public async Task<long?> GetCaseIdByContractNumber(string contractNumber, CancellationToken cancellation)
    {
	    const string query = @"SELECT Id FROM [dbo].[Uver] WHERE CisloSmlouvy = @ContractNumber AND Neaktivni = 0";

	    var parameters = new DynamicParameters();
	    parameters.Add("@ContractNumber", contractNumber, DbType.String, ParameterDirection.Input);

	    return await _connectionProvider.ExecuteDapperScalarAsync<long?>(query, parameters, cancellation);
    }

    public async Task<long?> GetCaseIdByPaymentAccount(string prefix, string accountNumber, CancellationToken cancellation)
    {
	    const string query = @"SELECT Id FROM [dbo].[Uver]
		WHERE PredcisliUctu = @Prefix AND CisloUctu = @AccountNumber AND Neaktivni = 0";

	    var parameters = new DynamicParameters();
	    parameters.Add("@Prefix", prefix, DbType.String, ParameterDirection.Input);
	    parameters.Add("@AccountNumber", accountNumber, DbType.String, ParameterDirection.Input);

	    return await _connectionProvider.ExecuteDapperScalarAsync<long?>(query, parameters, cancellation);
    }
    
    public async Task<long?> GetCaseIdByPcpId(string pcpId, CancellationToken cancellation)
    {
	    const string query = @"SELECT UverId FROM [dbo].[RezervaceSmluv] WHERE PcpInstId = @PcpId";

	    var parameters = new DynamicParameters();
	    parameters.Add("@PcpId", pcpId, DbType.String, ParameterDirection.Input);

	    return await _connectionProvider.ExecuteDapperScalarAsync<long?>(query, parameters, cancellation);
    }
    
    public async Task<bool> ExistsLoan(long loanId, CancellationToken cancellation)
    { 
	    const string query = "SELECT COUNT(1) FROM [dbo].[Uver] WHERE Id = @LoanId AND Neaktivni = 0";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@LoanId", loanId, DbType.Int64, ParameterDirection.Input);
	    
	    return await _connectionProvider.ExecuteDapperScalarAsync<bool>(query, parameters, cancellation);
    }
    
    public async Task<bool> ExistsRelationship(long loanId, long partnerId, CancellationToken cancellation)
    { 
	    const string query = "SELECT COUNT(1) from [dbo].[VztahUver] where UverId = @LoanId and PartnerId = @PartnerId";
	    
	    var parameters = new DynamicParameters();
        parameters.Add("@LoanId", loanId, DbType.Int64, ParameterDirection.Input);
        parameters.Add("@PartnerId", partnerId, DbType.Int64, ParameterDirection.Input);
        
        return await _connectionProvider.ExecuteDapperScalarAsync<bool>(query, parameters, cancellation);
    }

    public async Task<bool> ExistsPartner(long partnerId, CancellationToken cancellation)
    {
	    const string query = "SELECT COUNT(1) FROM [dbo].[Partner] WHERE Id = @PartnerId";
	    
	    var parameters = new DynamicParameters();
	    parameters.Add("@PartnerId", partnerId, DbType.Int64, ParameterDirection.Input);
	    
	    return await _connectionProvider.ExecuteDapperScalarAsync<bool>(query, parameters, cancellation);
    }
}