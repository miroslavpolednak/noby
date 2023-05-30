IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SqlQuery]') AND type in (N'U'))
DROP TABLE [dbo].[SqlQuery]
GO

CREATE TABLE [dbo].[SqlQuery](
	[SqlQueryId] [varchar](50) NOT NULL,
	[SqlQueryText] [varchar](max) NOT NULL,
	[DatabaseProvider] [tinyint] NOT NULL,
 CONSTRAINT [PK_SqlQuery] PRIMARY KEY CLUSTERED 
(
	[SqlQueryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'AcademicDegreesAfter', N'SELECT KOD ''Id'', TEXT ''Name'', CAST(1 as bit) ''IsValid'' FROM [SBR].[CIS_TITULY_ZA] ORDER BY TEXT ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'AcademicDegreesBefore', N'SELECT Id, Name, CAST(1 as bit) ''IsValid'' FROM dbo.AcademicDegreesBefore ORDER BY Id ASC', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'BankCodes', N'SELECT KOD_BANKY ''BankCode'', NAZOV_BANKY ''Name'', SKRAT_NAZOV_BANKY ''ShortName'', SKRATKA_STATU_PRE_IBAN ''State'', CAST(CASE WHEN SYSDATETIME() <= ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.CIS_KODY_BANK ORDER BY KOD_BANKY ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ClassificationOfEconomicActivities', N'SELECT KOD ''Id'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_OKEC] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'CollateralTypes', N'SELECT TYP_ZABEZPECENIA ''CollateralType'', NULLIF(MANDANT, 0) ''MandantId'', KOD_BGM ''CodeBgm'', TEXT_BGM ''TextBgm'', TEXT_K_TYPU ''NameType'' FROM [SBR].[CIS_VAHY_ZABEZPECENI] ORDER BY TYP_ZABEZPECENIA ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ContactTypes1', N'SELECT TYP_KONTAKTU ''Id'', TEXT ''Name'', NULLIF(MANDANT, 0) ''MandantId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_TYPY_KONTAKTOV] ORDER BY TYP_KONTAKTU ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ContactTypes2', N'SELECT [ContactTypeId], [MpDigiApiCode] FROM [dbo].[ContactTypeExtension]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'Countries', N'SELECT KOD ''Id'', SKRATKA ''ShortName'', TEXT ''Name'', TEXT_CELY ''LongName'', DEF ''IsDefault'', RIZIKOVOST ''Risk'', CLEN_EU ''EuMember'', EUROZONA ''Eurozone'' FROM [SBR].[CIS_STATY] WHERE KOD != -1 ORDER BY [TEXT] ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'CovenantTypes', N'SELECT CAST(TYP_ZMLUVA as int) ''Id'', [TEXT] ''Name'', POPIS ''Description'', CAST(PORADI_ZOBRAZENI as int) ''Order'' FROM [SBR].[HTEDM_CIS_TERMINOVNIK_TYP_SMLOUVY] WHERE MANDANT=2', 2)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'Currencies', N'SELECT DISTINCT MENA ''Code'', POVOLENO_PRO_MENU_PRIJMU ''AllowedForIncomeCurrency'', POVOLENO_PRO_MENU_BYDLISTE ''AllowedForResidencyCurrency'', DEF ''IsDefault'' FROM [SBR].[CIS_STATY] WHERE MENA LIKE ''[A-Z][A-Z][A-Z]'' ORDER BY MENA ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DeveloperSearch', N'WITH terms AS (SELECT * FROM (VALUES <terms>) T(term))
        SELECT DEVELOPER_ID ''DeveloperId'', NAZEV ''DeveloperName'', ICO_RC ''DeveloperCIN'', null ''DeveloperProjectId'', null ''DeveloperProjectName''
        FROM (
            SELECT A.DEVELOPER_ID, A.NAZEV, A.ICO_RC,
            (
                SELECT SUM(rate) FROM(
                    SELECT CAST(CAST(CHARINDEX(term, ISNULL(A.NAZEV,'''')) AS BIT) AS INT)*1.01 + CAST(CAST(CHARINDEX(term, ISNULL(A.ICO_RC,'''')) AS BIT) AS INT) AS rate FROM terms
                )r
            ) AS RATE
            FROM [SBR].[CIS_DEVELOPER] A
            WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], ''9999-12-31'')
        )s
        WHERE RATE > 0
        ORDER BY RATE DESC, NAZEV ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DeveloperSearchWithProjects', N'WITH terms AS (SELECT * FROM (VALUES <terms>) T(term))
        SELECT DEVELOPER_ID ''DeveloperId'', NAZEV ''DeveloperName'', ICO_RC ''DeveloperCIN'', DEVELOPER_PROJEKT_ID ''DeveloperProjectId'', PROJEKT ''DeveloperProjectName''
        FROM (
	        SELECT A.DEVELOPER_ID, A.NAZEV, A.ICO_RC, B.DEVELOPER_PROJEKT_ID, B.PROJEKT,
	        (
		        SELECT SUM(rate) FROM(
			        SELECT CAST(CAST(CHARINDEX(term, ISNULL(A.NAZEV,'''')) AS BIT) AS INT)*1.01 + CAST(CAST(CHARINDEX(term, ISNULL(B.PROJEKT,'''')) AS BIT) AS INT) + CAST(CAST(CHARINDEX(term, ISNULL(A.ICO_RC,'''')) AS BIT) AS INT) AS rate FROM terms
		        )r
	        ) AS RATE
	        FROM [SBR].[CIS_DEVELOPER] A
	        INNER JOIN [SBR].[CIS_DEVELOPER_PROJEKTY_SPV] B ON A.DEVELOPER_ID=B.DEVELOPER_ID
	        WHERE GETDATE() BETWEEN A.[PLATNOST_OD] AND ISNULL(A.[PLATNOST_DO], ''9999-12-31'') AND GETDATE() BETWEEN B.[PLATNOST_OD] AND ISNULL(B.[PLATNOST_DO], ''9999-12-31'')
        )s
        WHERE RATE > 0 
        ORDER BY RATE DESC, NAZEV ASC, PROJEKT ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DocumentOnSATypes', N'SELECT Id, Name, SalesArrangementTypeId, FormTypeId FROM [dbo].[DocumentOnSAType]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DocumentTemplateVariants', N'SELECT Id, DocumentTemplateVersionId, DocumentVariant, Description FROM [dbo].[DocumentTemplateVariant] ORDER BY Id ASC', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DocumentTemplateVersions', N'SELECT Id, DocumentTypeId, DocumentVersion, FormTypeId, CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[DocumentTemplateVersion] ORDER BY Id ASC', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DocumentTypes', N'SELECT [Id], Id ''EnumValue'', [ShortName],[Name],[FileName],[SalesArrangementTypeId],[EACodeMainId],[IsFormIdRequested], CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[DocumentTypes] ORDER BY [Id]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'DrawingDurations', N'SELECT KOD ''Id'', LHUTA_K_CERPANI ''DrawingDuration'', DEF ''IsDefault'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_LHUTA_K_CERPANI] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'EaCodesMain1', N'SELECT KOD ''Id'', POPIS ''Name'', popis_klient ''DescriptionForClient'', KATEGORIE ''Category'', DRUH_KB ''KindKb'', viditelnost_ps_kb_prodejni_sit_kb ''IsVisibleForKb'', viditelnost_pro_vlozeni_noby ''IsInsertingAllowedNoby'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[EA_CIS_EACODEMAIN] ORDER BY kod ASC', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'EaCodesMain2', N'SELECT EaCodesMainId, IsFormIdRequested FROM dbo.EaCodesMainExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'EducationLevels', N'SELECT KOD ''Id'', TEXT ''Name'', CODE_NAME ''ShortName'', CODE ''RdmCode'',  KOD_SCORING ''ScoringCode'', CAST(CASE WHEN PLATNY_PRE_ES = 1 AND SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_VZDELANIE] WHERE MANDANT IN (0, 2) ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'EmploymentTypes', N'SELECT Kod ''Id'', CODE ''Code'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL(PLATNOST_DO, ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.CIS_PRACOVNY_POMER ORDER BY Kod', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'Fees', N'SELECT POPLATEK_ID ''Id'', POPLATEK_ID_KB ''IdKb'', NULLIF(MANDANT, 0) ''MandantId'', TEXT ''ShortName'', TEXT_DOKUMENTACE ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_POPLATKY_UV_DEF] ORDER BY POPLATEK_ID ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'FixedRatePeriods', N'SELECT KOD_PRODUKTU ''ProductTypeId'', PERIODA_FIXACE ''FixedRatePeriod'', NULLIF(MANDANT, 0) ''MandantId'', NOVY_PRODUKT ''IsNewProduct'', ALGORITMUS_SAZBY ''InterestRateAlgorithm'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.CIS_PERIODY_FIXACE_V', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'FormTypes', N'SELECT FORMULAR_ID ''Id'', CISLO ''Type'', VERZE ''Version'', NAZEV ''Name'', NULLIF(MANDANT, 0) ''MandantId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL(PLATNOST_DO, ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_FORMULARE] ORDER BY FORMULAR_ID ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'GetDeveloper', N'SELECT DEVELOPER_ID ''Id'', 
            NAZEV ''Name'', 
            ICO_RC ''Cin'', 
            CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'',
            PRIZNAK_OK ''StatusId'', 
            CASE WHEN PRIZNAK_OK=-1 THEN ''Probíhá prověřování'' WHEN PRIZNAK_OK=0 THEN ''Zamítnutý'' ELSE ''Schválený'' END ''StatusText'',
            CAST(CASE WHEN BALICEK_BENEFITU=1 THEN 1 ELSE 0 END as bit) ''BenefitPackage'',
            CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_BENEFITU_OD] AND ISNULL([PLATNOST_BENEFITU_DO], ''9999-12-31'') THEN 1 ELSE 0 END ''IsBenefitValid'',
            CAST(CASE WHEN BENEFITY_NAD_RAMEC_BALICKU IS NOT NULL THEN 1 ELSE 0 END as bit) ''BenefitsBeyondPackage''
        FROM [SBR].[CIS_DEVELOPER]
        WHERE DEVELOPER_ID=@DeveloperId', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'GetDeveloperProject', N'SELECT 
	DEVELOPER_PROJEKT_ID ''Id'', 
	DEVELOPER_ID ''DeveloperId'', 
	PROJEKT ''Name'', 
	UPOZORNENI_PRO_KB ''WarningForKb'', 
	UPOZORNENI_PRO_MPSS ''WarningForMp'', 
	STRANKY_PROJEKTU ''Web'', 
	LOKALITA ''Place'', 
	CASE WHEN HROMADNE_OCENENI=-1 THEN ''Probíhá zpracování'' WHEN HROMADNE_OCENENI=0 THEN ''NE'' ELSE ''ANO'' END ''MassEvaluationText'', 
	DOPORUCENI ''Recommandation'', 
	CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' 
FROM [SBR].[CIS_DEVELOPER_PROJEKTY_SPV]
WHERE DEVELOPER_PROJEKT_ID=@DeveloperProjectId AND DEVELOPER_ID=@DeveloperId', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'GetGeneralDocumentList', N'SELECT Id, Name, Filename, Format FROM [dbo].[GeneralDocumentList] ORDER BY Id ASC', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'GetOperator', N'SELECT MENO ''PerformerName'', [LOGIN] ''PerformerLogin'' FROM [SBR].[OPERATOR] WHERE DATUM_ZMENY IS NULL AND [LOGIN]=@PerformerLogin', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'HousingConditions', N'SELECT KOD ''Id'', TEXT ''Name'', CODE ''Code'', CODE ''RdmCode'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_FORMA_BYVANIA] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'Channels1', N'SELECT KOD ''Id'', TEXT ''Name'', NULLIF(MANDANT, 0) ''MandantId'', CODE ''Code'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_ALT_KANALY] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'Channels2', N'SELECT ChannelId, RdmCbChannelCode FROM dbo.ChannelExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IdentificationDocumentTypes1', N'SELECT KOD ''Id'', TEXT ''Name'', TEXT_SKRATKA ''ShortName'', CODE ''RdmCode'', CAST(DEF as bit) ''IsDefault'' FROM [SBR].[CIS_TYPY_DOKLADOV] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IdentificationDocumentTypes2', N'SELECT [IdentificationDocumentTypeId],[MpDigiApiCode] FROM [dbo].[IdentificationDocumentTypeExtension]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IncomeForeignTypes', N'SELECT KOD ''Id'', CODE ''Code'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_PRIJEM_ZO_ZAHRANICIA] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IncomeMainTypes', N'SELECT KOD ''Id'', CODE ''Code'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_ZDROJ_PRIJMU_HLAVNI] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IncomeMainTypesAML1', N'SELECT KOD ''Id'', NAZEV ''Name'', CAST(1 as bit) ''IsValid'' FROM [SBR].[CIS_AML_ZDROJ_PRIJMU_HLAVNI] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IncomeMainTypesAML2', N'SELECT Id, RdmCode FROM dbo.IncomeMainTypesAMLExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'IncomeOtherTypes', N'SELECT KOD ''Id'', CODE ''Code'', TEXT_CZE ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_ZDROJ_PRIJMU_VEDLAJSI] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'JobTypes', N'SELECT KOD ''Id'', TEXT ''Name'', DEF ''IsDefault'' FROM [SBR].[CIS_PRACOVNI_POZICE] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'LoanKinds', N'SELECT KOD ''Id'', NULLIF(MANDANT, 0) ''MandantId'', DRUH_UVERU_TEXT ''Name'', CAST(DEFAULT_HODNOTA as bit) ''IsDefault'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD_ES], ''1901-01-01'') AND ISNULL([DATUM_DO_ES], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_DRUH_UVERU] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'LoanPurposes', N'SELECT CAST(KOD as int) ''Id'', TEXT ''Name'', CAST(NULLIF(MANDANT, 0) as int) ''MandantId'', KOD_UVER ''ProductTypeIds'', CAST(PORADI as int) ''Order'', CAST(MAPOVANI_C4M as int) ''C4MId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_PLATNOSTI_OD], ''1901-01-01'') AND ISNULL([DATUM_PLATNOSTI_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.CIS_UCEL_UVERU_INT1 ORDER BY KOD', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'MaritalStatuses1', N'SELECT KOD ''Id'', TEXT ''Name'', DEF ''IsDefault'' FROM [SBR].[CIS_RODINNE_STAVY] ORDER BY KOD', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'MaritalStatuses2', N'SELECT MaritalStatusId, RDMCode FROM MaritalStatusExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'MarketingActions', N'SELECT KOD_MA_AKCIE ''Id'', TYP_AKCIE ''Code'', NULLIF(MANDANT, 0) ''MandantId'', NAZOV ''Name'', POPIS ''Description'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_MA_AKCIE] ORDER BY KOD_MA_AKCIE ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'Nationalities', N'SELECT Id, NazevStatniPrislusnost ''Name'', CAST(1 as bit) ''IsValid'' FROM [cis].[Zeme] ORDER BY NazevStatniPrislusnost ASC', 3)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'NetMonthEarnings1', N'SELECT KOD ''Id'', NAZEV ''Name'' FROM [SBR].[CIS_AML_IDENTIFIKACE_PRIJMU] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'NetMonthEarnings2', N'SELECT NetMonthEarningId, RdmCode FROM dbo.NetMonthEarningsExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ObligationCorrectionTypes', N'SELECT KOD ''Id'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL(PLATNOST_DO, ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', CODE ''Code'' FROM SBR.CIS_KOREKCE_ZAVAZKU ORDER BY KOD', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ObligationLaExposures', N'SELECT KOD ''Id'', CODE ''RdmCode'', TEXT ''Name'', DRUH_ZAVAZKU_KATEGORIE ''ObligationTypeId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].CIS_ZAVAZKY_LA_EXPOSURE ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ObligationTypes1', N'SELECT KOD ''Id'', CODE ''Code'', TEXT ''Name'', KOREKCE_ZAVAZKU ''ObligationCorrectionTypeId'', PORADIE ''Order'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].CIS_DRUH_ZAVAZKU ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ObligationTypes2', N'SELECT [ObligationTypeId], [ObligationProperty] FROM [dbo].[ObligationTypeExtension]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'PaymentDays', N'SELECT DEN_SPLACENI ''PaymentDay'', DEN_ZAPOCTENI_SPLATKY ''PaymentAccountDay'', NULLIF(MANDANT, 0) ''MandantId'', DEF ''IsDefault'', NABIZET_PORTAL ''ShowOnPortal'' FROM [SBR].[CIS_DEN_SPLACENI] ORDER BY DEN_SPLACENI ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'PostCodes', N'SELECT TOP 20 PSC ''PostCode'', TRIM(NAZEV) ''Name'', KOD_KRAJA ''Disctrict'', KOD_OBCE ''Municipality'' FROM [SBR].[CIS_PSC] ORDER BY PSC ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ProductTypes1', N'SELECT KOD_PRODUKTU ''Id'', NAZOV_PRODUKTU ''Name'', PORADIE_ZOBRAZENIA ''Order'', MIN_VYSKA_UV ''LoanAmountMin'', MAX_VYSKA_UV ''LoanAmountMax'', MIN_SPLATNOST_V_ROKOCH ''LoanDurationMin'', MAX_SPLATNOST_V_ROKOCH ''LoanDurationMax'', MIN_VYSKA_LTV ''LtvMin'', MAX_VYSKA_LTV ''LtvMax'', DRUH_UV_POVOLENY ''MpHomeApiLoanType'', CAST(CASE WHEN GETDATE() BETWEEN PLATNOST_OD_ES AND ISNULL(PLATNOST_DO_ES,''2099-01-01'') THEN 1 ELSE 0 END as bit) ''IsValid'', ID_PRODUKTU_PCP ''PcpProductId''
FROM SBR.HTEDM_CIS_HYPOTEKY_PRODUKTY
ORDER BY PORADIE_ZOBRAZENIA ASC', 2)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ProductTypes2', N'SELECT ProductTypeId, MpHomeApiLoanType, KonsDbLoanType, MandantId FROM dbo.ProductTypeExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ProfessionCategories', N'SELECT ProfessionCategoryId, ProfessionTypeIds, IncomeMainTypeAMLIds FROM dbo.ProfessionCategoryExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ProfessionTypes', N'SELECT KOD ''Id'',  ID_CM ''RdmCode'', NAZEV_CM ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_POVOLANI] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'ProofTypes', N'SELECT KOD ''Id'', CODE ''Code'', TEXT_CZE ''Name'', TEXT_ENG ''NameEnglish'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_TYP_POTVRDENIE_PRIJMU] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'PropertySettlements', N'SELECT CAST(KOD as int) ''Id'', TEXT_CZE ''Name'', TEXT_ENG ''NameEnglish'', ISNULL(ROD_STAV,'''') ''MaritalStateId'', CAST(PORADI as int) ''Order'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_VYPORADANI_MAJETKU] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'RealEstatePurchaseTypes', N'SELECT CAST(KOD as int) ''Id'', POPIS ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', DEF ''IsDefault'', PORADI ''Order'', CODE ''Code'', NULLIF(MANDANT, 0) ''MandantId'' FROM [SBR].[CIS_UCEL_PORIZENI_UV] ORDER BY PORADI ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'RealEstateTypes', N'SELECT CAST(KOD as int) ''Id'', POPIS ''Name'', NULLIF(MANDANT, 0) ''MandantId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', DEF ''IsDefault'', CAST(PORADI as int) ''Order'' FROM [SBR].[CIS_TYPY_NEHNUTELNOSTI_UV] ORDER BY PORADI ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'RelationshipCustomerProductTypes1', N'SELECT [ID_VZTAHU] ''Id'', [POPIS_VZTAHU] ''Name'' FROM [SBR].[VZTAH] ORDER BY [ID_VZTAHU] ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'RelationshipCustomerProductTypes2', N'SELECT [RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby] FROM [dbo].[RelationshipCustomerProductTypeExtension]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'RiskApplicationTypes', N'SELECT CAST(ID as int) ''Id'', CAST(NULLIF(MANDANT, 0) as int) ''MandantId'', UV_PRODUKT_ID ''ProductId'', MA, CAST(DRUH_UVERU as int) ''LoanKindId'', CAST(LTV_OD as int) ''LtvFrom'', CAST(LTV_DO as int) ''LtvTo'', CLUSTER_CODE ''C4MAplCode'', C4M_APL_TYPE_ID ''C4MAplTypeId'', C4M_APL_TYPE_NAZEV ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], ''1901-01-01'') AND ISNULL([DATUM_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].CIS_APL_TYPE ORDER BY ID ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'SalesArrangementTypes', N'SELECT Id, Name, ProductTypeId, SalesArrangementCategory, Description FROM [dbo].[SalesArrangementType] ORDER BY Id', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'SmsNotificationTypes', N'SELECT * FROM [dbo].[SmsNotificationType]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'StatementFrequencies', N'SELECT KOD ''Id'', CODE ''FrequencyCode'', FREQ ''FrequencyValue'', SORT ''Order'', [TEXT] ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', DEF ''IsDefault'' FROM [SBR].[CIS_HU_VYPIS_FREQ] ORDER BY SORT', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'StatementSubscriptionTypes', N'SELECT KOD ''Id'', CODE ''Code'', [TEXT] ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', DEF ''IsDefault'' FROM [SBR].[CIS_HU_ZODB_VYPIS]', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'StatementTypes', N'SELECT KOD ''Id'', C_POPIS_R_CZ ''Name'', C_POPIS_CZ ''ShortName'', N_SORT_ORDER ''Order'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([VALID_FROM], ''1901-01-01'') AND ISNULL([VALID_TO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_HU_TYP_VYPIS] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'TinFormatsByCountry', N'SELECT Id, CountryCode, RegularExpression, IsForFo, Tooltip, CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[TinFormatsByCountry]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'TinNoFillReasonsByCountry', N'SELECT Id, IsTinMandatory, ReasonForBlankTin, CAST(CASE WHEN SYSDATETIME() BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[TinNoFillReasonsByCountry]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowConsultationMatrixResponse1', N'SELECT CAST(KOD as int) ''Kod'', [TEXT] ''Text'' FROM SBR.HTEDM_CIS_WFL_CIS_HODNOTY WHERE ciselnik_id = 139', 2)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowConsultationMatrixResponse2', N'SELECT [TaskSubtypeId],[ProcessTypeId],[ProcessPhaseId],[IsConsultation] FROM [dbo].[WorkflowConsultationMatrix]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskConsultationTypes', N'SELECT KOD ''Id'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], ''1901-01-01'') AND ISNULL(DATUM_DO, ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID = 139 ORDER BY KOD', 2)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskSigningResponseTypes', N'SELECT KOD ''Id'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], ''1901-01-01'') AND ISNULL(DATUM_DO, ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID = 144 ORDER BY KOD', 2)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskStates1', N'SELECT KOD ''Id'', TEXT ''Name'' FROM [SBR].[CIS_WFL_UKOLY_STAVY] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskStates2', N'SELECT WorkflowTaskStateId, Flag FROM WorkflowTaskStateExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskStatesNoby', N'SELECT [Id],[Name],[Filter],[Indicator] FROM [dbo].[WorkflowTaskStatesNoby] ORDER BY [Id] ASC', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskTypes1', N'SELECT UKOL_TYP ''Id'', UKOL_NAZOV ''Name'' FROM [SBR].[CIS_WFL_UKOLY] ORDER BY UKOL_TYP ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowTaskTypes2', N'SELECT [WorkflowTaskTypeId], [CategoryId] FROM WorkflowTaskTypeExtension', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkSectors', N'SELECT KOD ''Id'', TEXT ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].[CIS_PRACOVNI_SEKTOR] ORDER BY KOD ASC', 1)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'CountryCodePhoneIdc', N'SELECT Id, Idc, [Name], IsPriority, CAST(CASE WHEN Id=''CZ+420'' THEN 1 ELSE 0 END as bit) ''IsDefault'', CAST(1 as bit) ''IsValid'' FROM [dbo].[CountryCodePhoneIdc]', 4)
GO
INSERT [dbo].[SqlQuery] ([SqlQueryId], [SqlQueryText], [DatabaseProvider]) VALUES (N'WorkflowProcessType', N'SELECT CAST(KOD as int) ''Id'', [TEXT] as ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_OD], ''1901-01-01'') AND ISNULL([DATUM_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID=11', 2)
GO
ALTER TABLE [dbo].[SqlQuery] ADD  CONSTRAINT [DF_SqlQuery_DatabaseProvider]  DEFAULT ((1)) FOR [DatabaseProvider]
GO
