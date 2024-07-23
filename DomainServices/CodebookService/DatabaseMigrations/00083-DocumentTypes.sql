alter table dbo.DocumentTypes add IsElectronicSigningEnabled bit default(1);
GO

update dbo.DocumentTypes set IsElectronicSigningEnabled=1;

update SqlQuery set SqlQueryText='SELECT [Id], Id ''EnumValue'', IsElectronicSigningEnabled,[ShortName],[Name],[FileName],[SalesArrangementTypeId],[EACodeMainId], CAST(CASE WHEN CAST(GETDATE() as date) BETWEEN [ValidFrom] AND ISNULL([ValidTo], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [dbo].[DocumentTypes] ORDER BY [Id]' where SqlQueryId='DocumentTypes';

update [dbo].[SqlQuery] set SqlQueryText='SELECT KOD_PRODUKTU ''ProductTypeId'', PERIODA_FIXACE ''FixedRatePeriod'', NULLIF(MANDANT, 0) ''MandantId'', MAX(NOVY_PRODUKT) ''IsNewProduct'', MAX(ALGORITMUS_SAZBY) ''InterestRateAlgorithm'', CAST(CASE WHEN CAST(GETDATE() as date) BETWEEN MAX(ISNULL([PLATNOST_OD], ''1901-01-01'')) AND MAX(ISNULL([PLATNOST_DO], ''9999-12-31'')) THEN 1 ELSE 0 END as bit) ''IsValid'' FROM SBR.HTEDM_CIS_PERIODY_FIXACE_V GROUP BY KOD_PRODUKTU, NULLIF(MANDANT, 0), PERIODA_FIXACE
' where SqlQueryId='FixedRatePeriods';
