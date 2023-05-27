namespace DomainServices.CodebookService.Api.Extensions;

internal static class SqlQueries
{
    public const string AcademicDegreesAfter = "SELECT KOD 'Id', TEXT 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_TITULY_ZA] ORDER BY TEXT ASC";

    public const string AcademicDegreesBefore = "SELECT Id, Name, CAST(1 as bit) 'IsValid' FROM dbo.AcademicDegreesBefore ORDER BY Id ASC";

    public const string BankCodes = "SELECT KOD_BANKY 'BankCode', NAZOV_BANKY 'Name', SKRAT_NAZOV_BANKY 'ShortName', SKRATKA_STATU_PRE_IBAN 'State', CAST(CASE WHEN SYSDATETIME() <= ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM SBR.CIS_KODY_BANK ORDER BY KOD_BANKY ASC";

    public const string ClassificationOfEconomicActivities = "SELECT KOD 'Id', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_OKEC] ORDER BY KOD ASC";

    public const string CollateralTypes = "SELECT TYP_ZABEZPECENIA 'CollateralType', NULLIF(MANDANT, 0) 'MandantId', KOD_BGM 'CodeBgm', TEXT_BGM 'TextBgm', TEXT_K_TYPU 'NameType' FROM [SBR].[CIS_VAHY_ZABEZPECENI] ORDER BY TYP_ZABEZPECENIA ASC";

    public const string ContactTypes = "SELECT TYP_KONTAKTU 'Id', TEXT 'Name', NULLIF(MANDANT, 0) 'MandantId', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_TYPY_KONTAKTOV] ORDER BY TYP_KONTAKTU ASC";

    public const string Countries = "SELECT KOD 'Id', SKRATKA 'ShortName', TEXT 'Name', TEXT_CELY 'LongName', DEF 'IsDefault', RIZIKOVOST 'Risk', CLEN_EU 'EuMember', EUROZONA 'Eurozone' FROM [SBR].[CIS_STATY] WHERE KOD != -1 ORDER BY [TEXT] ASC";

    public const string Currencies = "SELECT DISTINCT MENA 'Code', POVOLENO_PRO_MENU_PRIJMU 'AllowedForIncomeCurrency', POVOLENO_PRO_MENU_BYDLISTE 'AllowedForResidencyCurrency', DEF 'IsDefault' FROM [SBR].[CIS_STATY] WHERE MENA LIKE '[A-Z][A-Z][A-Z]' ORDER BY MENA ASC";

    public const string DeveloperSearchWithProjects = @"
        WITH terms AS (SELECT * FROM (VALUES <terms>) T(term))
        SELECT DEVELOPER_ID 'DeveloperId', NAZEV 'DeveloperName', ICO_RC 'DeveloperCIN', DEVELOPER_PROJEKT_ID 'DeveloperProjectId', PROJEKT 'DeveloperProjectName'
        FROM (
	        SELECT A.DEVELOPER_ID, A.NAZEV, A.ICO_RC, B.DEVELOPER_PROJEKT_ID, B.PROJEKT,
	        (
		        SELECT SUM(rate) FROM(
			        SELECT CAST(CAST(CHARINDEX(term, ISNULL(A.NAZEV,'')) AS BIT) AS INT)*1.01 + CAST(CAST(CHARINDEX(term, ISNULL(B.PROJEKT,'')) AS BIT) AS INT) + CAST(CAST(CHARINDEX(term, ISNULL(A.ICO_RC,'')) AS BIT) AS INT) AS rate FROM terms
		        )r
	        ) AS RATE
	        FROM [SBR].[CIS_DEVELOPER] A
	        INNER JOIN [SBR].[CIS_DEVELOPER_PROJEKTY_SPV] B ON A.DEVELOPER_ID=B.DEVELOPER_ID
	        WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], '9999-12-31') AND GETDATE() BETWEEN B.[PLATNOST_OD] AND ISNULL(B.[PLATNOST_DO], '9999-12-31')
        )s
        WHERE RATE > 0 
        ORDER BY RATE DESC, NAZEV ASC, PROJEKT ASC";

    public const string DeveloperSearch = @"
        WITH terms AS (SELECT * FROM (VALUES <terms>) T(term))
        SELECT DEVELOPER_ID 'DeveloperId', NAZEV 'DeveloperName', ICO_RC 'DeveloperCIN', null 'DeveloperProjectId', null 'DeveloperProjectName'
        FROM (
            SELECT A.DEVELOPER_ID, A.NAZEV, A.ICO_RC,
            (
                SELECT SUM(rate) FROM(
                    SELECT CAST(CAST(CHARINDEX(term, ISNULL(A.NAZEV,'')) AS BIT) AS INT)*1.01 + CAST(CAST(CHARINDEX(term, ISNULL(A.ICO_RC,'')) AS BIT) AS INT) AS rate FROM terms
                )r
            ) AS RATE
            FROM [SBR].[CIS_DEVELOPER] A
            WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], '9999-12-31')
        )s
        WHERE RATE > 0
        ORDER BY RATE DESC, NAZEV ASC";

    public const string DocumentOnSATypes = "SELECT Id, Name, SalesArrangementTypeId, FormTypeId FROM [dbo].[DocumentOnSAType]";

    public const string DocumentTemplateVariants = "SELECT Id, DocumentTemplateVersionId, DocumentVariant, Description FROM [dbo].[DocumentTemplateVariant] ORDER BY Id ASC";

    public const string DocumentTemplateVersions = "SELECT Id, DocumentTypeId, DocumentVersion, FormTypeId, CAST(CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [dbo].[DocumentTemplateVersion] ORDER BY Id ASC";

    public const string DocumentTypes = "SELECT [Id], Id 'EnumValue', [ShortName],[Name],[FileName],[SalesArrangementTypeId],[EACodeMainId],[IsFormIdRequested], CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [dbo].[DocumentTypes] ORDER BY [Id]";

    public const string DrawingDurations = "SELECT KOD 'Id', LHUTA_K_CERPANI 'DrawingDuration', DEF 'IsDefault', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_LHUTA_K_CERPANI] ORDER BY KOD ASC";

    public const string EaCodesMain1 = "SELECT KOD 'Id', POPIS 'Name', popis_klient 'DescriptionForClient', KATEGORIE 'Category', DRUH_KB 'KindKb', viditelnost_ps_kb_prodejni_sit_kb 'IsVisibleForKb', viditelnost_pro_vlozeni_noby 'IsInsertingAllowedNoby', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [dbo].[EA_CIS_EACODEMAIN] ORDER BY kod ASC";

    public const string EaCodesMain2 = "SELECT EaCodesMainId, IsFormIdRequested FROM dbo.EaCodesMainExtension";

    public const string EducationLevels = "SELECT KOD 'Id', TEXT 'Name', CODE_NAME 'ShortName', CODE 'RdmCode',  KOD_SCORING 'ScoringCode', CAST(CASE WHEN PLATNY_PRE_ES = 1 AND SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_VZDELANIE] WHERE MANDANT IN (0, 2) ORDER BY KOD ASC";

    public const string EmploymentTypes = "SELECT Kod 'Id', CODE 'Code', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO, '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM SBR.CIS_PRACOVNY_POMER ORDER BY Kod";

    public const string Fees = "SELECT POPLATEK_ID 'Id', POPLATEK_ID_KB 'IdKb', NULLIF(MANDANT, 0) 'MandantId', TEXT 'ShortName', TEXT_DOKUMENTACE 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_POPLATKY_UV_DEF] ORDER BY POPLATEK_ID ASC";

    public const string FixedRatePeriods = "SELECT KOD_PRODUKTU 'ProductTypeId', PERIODA_FIXACE 'FixedRatePeriod', NULLIF(MANDANT, 0) 'MandantId', NOVY_PRODUKT 'IsNewProduct', ALGORITMUS_SAZBY 'InterestRateAlgorithm', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM SBR.CIS_PERIODY_FIXACE_V";

    public const string FormTypes = "SELECT FORMULAR_ID 'Id', CISLO 'Type', VERZE 'Version', NAZEV 'Name', NULLIF(MANDANT, 0) 'MandantId', CAST(CASE WHEN SYSDATETIME() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO, '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_FORMULARE] ORDER BY FORMULAR_ID ASC";

    public const string GetDeveloper = @"
        SELECT DEVELOPER_ID 'Id', 
            NAZEV 'Name', 
            ICO_RC 'Cin', 
            CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid',
            PRIZNAK_OK 'StatusId', 
            CASE WHEN PRIZNAK_OK=-1 THEN 'Probíhá prověřování' WHEN PRIZNAK_OK=0 THEN 'Zamítnutý' ELSE 'Schválený' END 'StatusText',
            CAST(CASE WHEN BALICEK_BENEFITU=1 THEN 1 ELSE 0 END as bit) 'BenefitPackage',
            CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_BENEFITU_OD] AND ISNULL([PLATNOST_BENEFITU_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsBenefitValid',
            CAST(CASE WHEN BENEFITY_NAD_RAMEC_BALICKU IS NOT NULL THEN 1 ELSE 0 END as bit) 'BenefitsBeyondPackage'
        FROM [SBR].[CIS_DEVELOPER]
        WHERE DEVELOPER_ID=@DeveloperId";

    public const string GetDeveloperProject = @"SELECT 
	DEVELOPER_PROJEKT_ID 'Id', 
	DEVELOPER_ID 'DeveloperId', 
	PROJEKT 'Name', 
	UPOZORNENI_PRO_KB 'WarningForKb', 
	UPOZORNENI_PRO_MPSS 'WarningForMp', 
	STRANKY_PROJEKTU 'Web', 
	LOKALITA 'Place', 
	CASE WHEN HROMADNE_OCENENI=-1 THEN 'Probíhá zpracování' WHEN HROMADNE_OCENENI=0 THEN 'NE' ELSE 'ANO' END 'MassEvaluationText', 
	DOPORUCENI 'Recommandation', 
	CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' 
FROM [SBR].[CIS_DEVELOPER_PROJEKTY_SPV]
WHERE DEVELOPER_PROJEKT_ID=@DeveloperProjectId AND DEVELOPER_ID=@DeveloperId";

    public const string GetGeneralDocumentList = "SELECT Id, Name, Filename, Format FROM [dbo].[GeneralDocumentList] ORDER BY Id ASC";

    public const string GetOperator = "SELECT MENO 'PerformerName', [LOGIN] 'PerformerLogin' FROM [SBR].[OPERATOR] WHERE DATUM_ZMENY IS NULL AND [LOGIN]=@PerformerLogin";

    public const string HousingConditions = "SELECT KOD 'Id', TEXT 'Name', CODE 'Code', CODE 'RdmCode', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_FORMA_BYVANIA] ORDER BY KOD ASC";

    public const string Channels1 = "SELECT KOD 'Id', TEXT 'Name', NULLIF(MANDANT, 0) 'MandantId', CODE 'Code', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_ALT_KANALY] ORDER BY KOD ASC";

    public const string Channels2 = "SELECT ChannelId, RdmCbChannelCode FROM dbo.ChannelExtension";

    public const string IdentificationDocumentTypes1 = "SELECT KOD 'Id', TEXT 'Name', TEXT_SKRATKA 'ShortName', CODE 'RdmCode', CAST(DEF as bit) 'IsDefault' FROM [SBR].[CIS_TYPY_DOKLADOV] ORDER BY KOD ASC";

    public const string IdentificationDocumentTypes2 = "SELECT [IdentificationDocumentTypeId],[MpDigiApiCode] FROM [dbo].[IdentificationDocumentTypeExtension]";

    public const string IncomeForeignTypes = "SELECT KOD 'Id', CODE 'Code', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_PRIJEM_ZO_ZAHRANICIA] ORDER BY KOD ASC";

    public const string IncomeMainTypes = "SELECT KOD 'Id', CODE 'Code', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_ZDROJ_PRIJMU_HLAVNI] ORDER BY KOD ASC";

    public const string IncomeMainTypesAML1 = "SELECT KOD 'Id', NAZEV 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_AML_ZDROJ_PRIJMU_HLAVNI] ORDER BY KOD ASC";

    public const string IncomeMainTypesAML2 = "SELECT Id, RdmCode FROM dbo.IncomeMainTypesAMLExtension";

    public const string IncomeOtherTypes = "SELECT KOD 'Id', CODE 'Code', TEXT_CZE 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_ZDROJ_PRIJMU_VEDLAJSI] ORDER BY KOD ASC";

    public const string JobTypes = "SELECT KOD 'Id', TEXT 'Name', DEF 'IsDefault' FROM [SBR].[CIS_PRACOVNI_POZICE] ORDER BY KOD ASC";

    public const string LoanKinds = "SELECT KOD 'Id', NULLIF(MANDANT, 0) 'MandantId', DRUH_UVERU_TEXT 'Name', CAST(DEFAULT_HODNOTA as bit) 'IsDefault', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD_ES], '1901-01-01') AND ISNULL([DATUM_DO_ES], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_DRUH_UVERU] ORDER BY KOD ASC";

    public const string LoanPurposes = "SELECT CAST(KOD as int) 'Id', TEXT 'Name', CAST(NULLIF(MANDANT, 0) as int) 'MandantId', KOD_UVER 'ProductTypeIds', CAST(PORADI as int) 'Order', CAST(MAPOVANI_C4M as int) 'C4MId', CAST(CASE WHEN SYSDATETIME() BETWEEN [DATUM_PLATNOSTI_OD] AND ISNULL([DATUM_PLATNOSTI_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM SBR.CIS_UCEL_UVERU_INT1 ORDER BY KOD";

    public const string MaritalStatuses1 = "SELECT KOD 'Id', TEXT 'Name', DEF 'IsDefault' FROM [SBR].[CIS_RODINNE_STAVY] ORDER BY KOD";

    public const string MaritalStatuses2 = "SELECT MaritalStatusId, RDMCode FROM MaritalStatusExtension";

    public const string MarketingActions = "SELECT KOD_MA_AKCIE 'Id', TYP_AKCIE 'Code', NULLIF(MANDANT, 0) 'MandantId', NAZOV 'Name', POPIS 'Description', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_MA_AKCIE] ORDER BY KOD_MA_AKCIE ASC";

    public const string Nationalities = "SELECT Id, NazevStatniPrislusnost 'Name', CAST(1 as bit) 'IsValid' FROM [cis].[Zeme] ORDER BY NazevStatniPrislusnost ASC";

    public const string NetMonthEarnings1 = "SELECT KOD 'Id', NAZEV 'Name' FROM [SBR].[CIS_AML_IDENTIFIKACE_PRIJMU] ORDER BY KOD ASC";

    public const string NetMonthEarnings2 = "SELECT NetMonthEarningId, RdmCode FROM dbo.NetMonthEarningsExtension";

    public const string ObligationCorrectionTypes = "SELECT KOD 'Id', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO, '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid', CODE 'Code' FROM SBR.CIS_KOREKCE_ZAVAZKU ORDER BY KOD";

    public const string ObligationLaExposures = "SELECT KOD 'Id', CODE 'RdmCode', TEXT 'Name', DRUH_ZAVAZKU_KATEGORIE 'ObligationTypeId', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].CIS_ZAVAZKY_LA_EXPOSURE ORDER BY KOD ASC";

    public const string ObligationTypes1 = "SELECT KOD 'Id', CODE 'Code', TEXT 'Name', KOREKCE_ZAVAZKU 'ObligationCorrectionTypeId', PORADIE 'Order', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].CIS_DRUH_ZAVAZKU ORDER BY KOD ASC";

    public const string ObligationTypes2 = "SELECT [ObligationTypeId], [ObligationProperty] FROM [dbo].[ObligationTypeExtension]";

    public const string PaymentDays = "SELECT DEN_SPLACENI 'PaymentDay', DEN_ZAPOCTENI_SPLATKY 'PaymentAccountDay', NULLIF(MANDANT, 0) 'MandantId', DEF 'IsDefault', NABIZET_PORTAL 'ShowOnPortal' FROM [SBR].[CIS_DEN_SPLACENI] ORDER BY DEN_SPLACENI ASC";

    public const string PostCodes = "SELECT TOP 20 PSC 'PostCode', TRIM(NAZEV) 'Name', KOD_KRAJA 'Disctrict', KOD_OBCE 'Municipality' FROM [SBR].[CIS_PSC] ORDER BY PSC ASC";

    public const string ProductTypes1 = @"
SELECT KOD_PRODUKTU 'Id', NAZOV_PRODUKTU 'Name', PORADIE_ZOBRAZENIA 'Order', MIN_VYSKA_UV 'LoanAmountMin', MAX_VYSKA_UV 'LoanAmountMax', MIN_SPLATNOST_V_ROKOCH 'LoanDurationMin', MAX_SPLATNOST_V_ROKOCH 'LoanDurationMax', MIN_VYSKA_LTV 'LtvMin', MAX_VYSKA_LTV 'LtvMax', DRUH_UV_POVOLENY 'MpHomeApiLoanType', CAST(CASE WHEN GETDATE() BETWEEN PLATNOST_OD_ES AND ISNULL(PLATNOST_DO_ES,'2099-01-01') THEN 1 ELSE 0 END as bit) 'IsValid', ID_PRODUKTU_PCP 'PcpProductId'
FROM SBR.HTEDM_CIS_HYPOTEKY_PRODUKTY
ORDER BY PORADIE_ZOBRAZENIA ASC";

    public const string ProductTypes2 = "SELECT ProductTypeId, MpHomeApiLoanType, KonsDbLoanType, MandantId FROM dbo.ProductTypeExtension";

    public const string ProfessionCategories = "SELECT ProfessionCategoryId, ProfessionTypeIds, IncomeMainTypeAMLIds FROM dbo.ProfessionCategoryExtension";

    public const string ProfessionTypes = "SELECT KOD 'Id',  ID_CM 'RdmCode', NAZEV_CM 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_POVOLANI] ORDER BY KOD ASC";

    public const string ProofTypes = "SELECT KOD 'Id', CODE 'Code', TEXT_CZE 'Name', TEXT_ENG 'NameEnglish', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_TYP_POTVRDENIE_PRIJMU] ORDER BY KOD ASC";

    public const string PropertySettlements = "SELECT CAST(KOD as int) 'Id', TEXT_CZE 'Name', TEXT_ENG 'NameEnglish', ISNULL(ROD_STAV,'') 'MaritalStateId', CAST(PORADI as int) 'Order', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_VYPORADANI_MAJETKU] ORDER BY KOD ASC";

    public const string RealEstatePurchaseTypes = "SELECT CAST(KOD as int) 'Id', POPIS 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid', DEF 'IsDefault', PORADI 'Order', CODE 'Code', NULLIF(MANDANT, 0) 'MandantId' FROM [SBR].[CIS_UCEL_PORIZENI_UV] ORDER BY PORADI ASC";

    public const string RealEstateTypes = "SELECT CAST(KOD as int) 'Id', POPIS 'Name', NULLIF(MANDANT, 0) 'MandantId', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid', DEF 'IsDefault', CAST(PORADI as int) 'Order' FROM [SBR].[CIS_TYPY_NEHNUTELNOSTI_UV] ORDER BY PORADI ASC";

    public const string RelationshipCustomerProductTypes1 = "SELECT [ID_VZTAHU] 'Id', [POPIS_VZTAHU] 'Name' FROM [SBR].[VZTAH] ORDER BY [ID_VZTAHU] ASC";

    public const string RelationshipCustomerProductTypes2 = "SELECT [RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby] FROM [dbo].[RelationshipCustomerProductTypeExtension]";

    public const string RiskApplicationTypes = "SELECT CAST(ID as int) 'Id', CAST(NULLIF(MANDANT, 0) as int) 'MandantId', UV_PRODUKT_ID 'ProductId', MA, CAST(DRUH_UVERU as int) 'LoanKindId', CAST(LTV_OD as int) 'LtvFrom', CAST(LTV_DO as int) 'LtvTo', CLUSTER_CODE 'C4mAplCode', C4M_APL_TYPE_ID 'C4mAplTypeId', C4M_APL_TYPE_NAZEV 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], '1901-01-01') AND ISNULL([DATUM_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].CIS_APL_TYPE ORDER BY ID ASC";

    public const string SalesArrangementTypes = "SELECT Id, Name, ProductTypeId, SalesArrangementCategory, Description FROM [dbo].[SalesArrangementType] ORDER BY Id";

    public const string SmsNotificationTypes = "SELECT * FROM [dbo].[SmsNotificationType]";

    public const string StatementFrequencies = "SELECT KOD 'Id', CODE 'FrequencyCode', FREQ 'FrequencyValue', SORT 'Order', [TEXT] 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid', DEF 'IsDefault' FROM [SBR].[CIS_HU_VYPIS_FREQ] ORDER BY SORT";

    public const string StatementSubscriptionTypes = "SELECT KOD 'Id', CODE 'Code', [TEXT] 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid', DEF 'IsDefault' FROM [SBR].[CIS_HU_ZODB_VYPIS]";

    public const string StatementTypes = "SELECT KOD 'Id', C_POPIS_R_CZ 'Name', C_POPIS_CZ 'ShortName', N_SORT_ORDER 'Order', CAST(CASE WHEN SYSDATETIME() BETWEEN[VALID_FROM] AND ISNULL([VALID_TO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_HU_TYP_VYPIS] ORDER BY KOD ASC";

    public const string TinFormatsByCountry = "SELECT Id, CountryCode, RegularExpression, IsForFo, Tooltip, CAST(CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [dbo].[TinFormatsByCountry]";

    public const string TinNoFillReasonsByCountry = "SELECT Id, IsTinMandatory, ReasonForBlankTin, CAST(CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [dbo].[TinNoFillReasonsByCountry]";

    public const string WorkflowConsultationMatrixResponse1 = "SELECT CAST(KOD as int) 'Kod', [TEXT] 'Text' FROM SBR.HTEDM_CIS_WFL_CIS_HODNOTY WHERE ciselnik_id = 139";

    public const string WorkflowConsultationMatrixResponse2 = "SELECT [TaskSubtypeId],[ProcessTypeId],[ProcessPhaseId],[IsConsultation] FROM [dbo].[WorkflowConsultationMatrix]";

    public const string WorkflowTaskConsultationTypes = "SELECT KOD 'Id', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], '1901-01-01') AND ISNULL(DATUM_DO, '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM SBR.HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID = 139 ORDER BY KOD";

    public const string WorkflowTaskSigningResponseTypes = "SELECT KOD 'Id', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], '1901-01-01') AND ISNULL(DATUM_DO, '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM SBR.HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID = 144 ORDER BY KOD";

    public const string WorkflowTaskStates1 = "SELECT KOD 'Id', TEXT 'Name' FROM [SBR].[CIS_WFL_UKOLY_STAVY] ORDER BY KOD ASC";

    public const string WorkflowTaskStates2 = "SELECT WorkflowTaskStateId, Flag FROM WorkflowTaskStateExtension";

    public const string WorkflowTaskStatesNoby = "SELECT [Id],[Name],[Filter],[Indicator] FROM [dbo].[WorkflowTaskStatesNoby] ORDER BY [Id] ASC";

    public const string WorkflowTaskTypes1 = "SELECT UKOL_TYP 'Id', UKOL_NAZOV 'Name' FROM [SBR].[CIS_WFL_UKOLY] ORDER BY UKOL_TYP ASC";

    public const string WorkflowTaskTypes2 = "SELECT [WorkflowTaskTypeId], [CategoryId] FROM WorkflowTaskTypeExtension";

    public const string WorkSectors = "SELECT KOD 'Id', TEXT 'Name', CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_PRACOVNI_SEKTOR] ORDER BY KOD ASC";

    public const string CovenantTypes = "SELECT CAST(TYP_ZMLUVA as int) 'Id', [TEXT] 'Name', POPIS 'Description', CAST(PORADI_ZOBRAZENI as int) 'Order' FROM [SBR].[HTEDM_CIS_TERMINOVNIK_TYP_SMLOUVY]";
}