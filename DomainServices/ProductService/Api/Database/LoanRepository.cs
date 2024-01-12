using CIS.Core.Data;
using CIS.Infrastructure.Data;
using DomainServices.ProductService.Api.Database.Models;

namespace DomainServices.ProductService.Api.Database;

[ScopedService, SelfService]
internal sealed class LoanRepository
{
    private readonly IConnectionProvider _connectionProvider;
    
    public LoanRepository(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    public Task<Loan?> GetLoan(long caseId, CancellationToken cancellationToken)
    { 
        const string Query =
            """
            SELECT [Id] as Id,
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
                   [Datum1AnuitniSplatky] as FirstAnnuityPaymentDate,
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
                   [Neaktivni] as IsCancelled,
                   StavHU as MortgageState,
                   DatumDocerpani as DrawingFinishedDate
            FROM [dbo].[Uver]
            WHERE [Id] = @caseId AND [Neaktivni] = 0
            """;
        
        return _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Loan>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<List<Models.LoanPurpose>> GetLoanPurposes(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UcelUveruId] as LoanPurposeId, [SumaUcelu] as Sum
            FROM [dbo].[UverUcely]
            WHERE [UverId] = @caseId
            """;

	    return _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.LoanPurpose>(Query, param: new { caseId }, cancellationToken);
    }
    
    public Task<LoanStatement?> GetLoanStatement(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [Ulice] as Street,
                   [CisloDomu4] as StreetNumber,
                   [CisloDomu2] as HouseNumber,
                   [Psc] as Postcode,
                   [Mesto] as City,
                   [ZemeId] as CountryId,
                   [StatPodkategorie] as AddressPointId
            FROM [dbo].[UverVypisy]
            WHERE [Id] = @caseId
            """;

	    return _connectionProvider.ExecuteDapperFirstOrDefaultAsync<LoanStatement>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<List<Collateral>> GetCollateral(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as ProductId, [NemovitostId] as RealEstateId
            FROM [dbo].[Zabezpeceni]
            WHERE [UverId] = @caseId
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<Collateral>(Query, param: new { caseId }, cancellationToken);
    }
    
    public Task<List<Models.Relationship>> GetRelationships(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [PartnerId] as PartnerId,
                   [VztahId] as ContractRelationshipTypeId,
                   [KbId] as KbId,
                   [Zmocnenec] as Agent,
                   [StavKyc] as Kyc
            FROM [dbo].[VztahUver] 
            LEFT JOIN [dbo].[Partner] ON [VztahUver].PartnerId = [Partner].Id
            WHERE [UverId] = @caseId AND [Neaktivni] = 0
            """;
        
        return _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.Relationship>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<Covenant?> GetCovenant(long caseId, int order, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as CaseId,
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
            WHERE [UverId] = @caseId AND [PoradoveCislo] = @order
            """;
	 
	    return _connectionProvider.ExecuteDapperFirstOrDefaultAsync<Covenant>(Query, param: new { caseId, order }, cancellationToken);
    }

    public Task<List<Covenant>> GetCovenants(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as CaseId,  
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
            WHERE [UverId] = @caseId
            """;

	    return _connectionProvider.ExecuteDapperRawSqlToListAsync<Covenant>(Query, param: new { caseId }, cancellationToken);
    }
    
    public Task<List<CovenantPhase>> GetCovenantPhases(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as CaseId,   
                   [Nazev] as Name,
                   [Poradi] as [Order],
                   [PoradiPismeno] as OrderLetter
            FROM [dbo].[TerminovnikFaze]
            WHERE [UverId] = @caseId
            """;

	    return _connectionProvider.ExecuteDapperRawSqlToListAsync<CovenantPhase>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<List<Models.LoanRealEstate>> GetLoanRealEstates(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [NemovitostId] as RealEstateId, [TypKod] as RealEstateTypeId, [UcelKod] as RealEstatePurchaseTypeId
            FROM [dbo].[UverNemovitost]
            INNER JOIN [dbo].[Nemovitost] ON [Id] = [NemovitostId]
            WHERE [UverId] = @caseId
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<Models.LoanRealEstate>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<List<Obligation>> GetObligations(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as ProductId, 
                   [UcelUveruInt] as LoanPurposeId,
                   [TypZavazku] as ObligationTypeId,
                   [Castka] as Amount,
                   [Veritel] as CreditorName,
                   [PredcisliUctu] as AccountNumberPrefix,
                   [CisloUctu] as AccountNumber,
                   [KodBanky] as BankCode,
                   [VariabilniSymbol] as VariableSymbol
            FROM [dbo].[Zavazky]
            WHERE [UverId] = @caseId AND [TypZavazku] <> 0 AND [Castka] > 0 AND [Veritel] IS NOT NULL
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<Obligation>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<string?> GetPcpIdByCaseId(long caseId, CancellationToken cancellationToken)
    {
        const string Query = "SELECT [PcpInstId] FROM [dbo].[RezervaceSmluv] WHERE [UverId] = @caseId";

        return _connectionProvider.ExecuteDapperScalarAsync<string>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<long?> GetCaseIdByContractNumber(string contractNumber, CancellationToken cancellationToken)
    {
	    const string Query = "SELECT [Id] FROM [dbo].[Uver] WHERE CisloSmlouvy = @contractNumber AND [Neaktivni] = 0";

	    return _connectionProvider.ExecuteDapperScalarAsync<long?>(Query, param: new { contractNumber }, cancellationToken);
    }

    public Task<long?> GetCaseIdByPaymentAccount(string prefix, string accountNumber, CancellationToken cancellationToken)
    {
        const string Query = """
                             SELECT [Id] FROM [dbo].[Uver]
                             WHERE PredcisliUctu = @prefix AND [CisloUctu] = @accountNumber AND [Neaktivni] = 0
                             """;

	    return _connectionProvider.ExecuteDapperScalarAsync<long?>(Query, param: new { prefix, accountNumber }, cancellationToken);
    }
    
    public Task<long?> GetCaseIdByPcpId(string pcpId, CancellationToken cancellationToken)
    {
	    const string Query = "SELECT [UverId] FROM [dbo].[RezervaceSmluv] WHERE [PcpInstId] = @pcpId";

	    return _connectionProvider.ExecuteDapperScalarAsync<long?>(Query, param: new { pcpId }, cancellationToken);
    }
    
    public Task<bool> LoanExists(long caseId, CancellationToken cancellationToken)
    { 
	    const string Query = "SELECT COUNT(1) FROM [dbo].[Uver] WHERE [Id] = @caseId AND [Neaktivni] = 0";
	    
	    return _connectionProvider.ExecuteDapperScalarAsync<bool>(Query, param: new { caseId }, cancellationToken);
    }
    
    public Task<bool> RelationshipExists(long caseId, long partnerId, CancellationToken cancellationToken)
    { 
	    const string Query = "SELECT COUNT(1) from [dbo].[VztahUver] where [UverId] = @caseId and [PartnerId] = @partnerId";

        return _connectionProvider.ExecuteDapperScalarAsync<bool>(Query, param: new { caseId, partnerId }, cancellationToken);
    }

    public Task<bool> PartnerExists(long partnerId, CancellationToken cancellationToken)
    {
	    const string Query = "SELECT COUNT(1) FROM [dbo].[Partner] WHERE [Id] = @partnerId";

	    return _connectionProvider.ExecuteDapperScalarAsync<bool>(Query, param: new { partnerId }, cancellationToken);
    }
}