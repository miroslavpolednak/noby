UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT POPLATEK_ID ''Id'', POPLATEK_ID_KB ''IdKb'', NULLIF(MANDANT, 0) ''MandantId'', TEXT ''ShortName'', TEXT_DOKUMENTACE_VZ_PRIKL ''Name'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].HTEDM_CIS_POPLATKY_UV_DEF ORDER BY POPLATEK_ID ASC' WHERE SqlQueryId='Fees';

UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT KOD ''Id'', TEXT ''Name'', DEF ''IsDefault'' FROM [SBR].HTEDM_CIS_RODINNE_STAVY WHERE MANDANT=0 ORDER BY KOD' WHERE SqlQueryId='MaritalStatuses1';

UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT CAST(KOD as int) ''Id'', TEXT ''Name'', CAST(NULLIF(MANDANT, 0) as int) ''MandantId'', KOD_UVER ''ProductTypeIds'', CAST(PORADI as int) ''Order'', CAST(MAPOVANI_C4M as int) ''C4MId'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([DATUM_PLATNOSTI_OD], ''1901-01-01'') AND ISNULL([DATUM_PLATNOSTI_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'', MAPOVANI_ACV ''AcvId'' FROM SBR.HTEDM_CIS_UCEL_UVERU_INT1 ORDER BY KOD' WHERE SqlQueryId='LoanPurposes';

UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT KOD ''Id'', TEXT ''Name'', TEXT_SKRATKA ''ShortName'', CODE ''RdmCode'', CAST(DEF as bit) ''IsDefault'', CAST(TYP_OSOBY as int) ''PersonType'' FROM [SBR].HTEDM_CIS_TYPY_DOKLADOV WHERE TYP_OSOBY IN (0,1) ORDER BY KOD ASC' WHERE SqlQueryId='IdentificationDocumentTypes1';

UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT TYP_ZABEZPECENIA ''CollateralType'', NULLIF(MANDANT, 0) ''MandantId'', KOD_BGM ''CodeBgm'', TEXT_BGM ''TextBgm'', TEXT_K_TYPU ''NameType'' FROM [SBR].HTEDM_CIS_TYP_ZABEZPECENI ORDER BY TYP_ZABEZPECENIA ASC' WHERE SqlQueryId='CollateralTypes';

UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT KOD ''Id'', TEXT ''Name'', NULLIF(MANDANT, 0) ''MandantId'', CODE ''Code'', CAST(CASE WHEN SYSDATETIME() BETWEEN ISNULL([PLATNOST_OD], ''1901-01-01'') AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' FROM [SBR].HTEDM_CIS_ALT_KANALY ORDER BY KOD ASC' WHERE SqlQueryId='Channels1';
INSERT INTO [dbo].[ChannelExtension] (ChannelId, [RdmCbChannelCode]) VALUES (18,'CH0001');

UPDATE SqlQuery SET DatabaseProvider=2, SqlQueryText='SELECT 
	DEVELOPER_PROJEKT_ID ''Id'', 
	DEVELOPER_ID ''DeveloperId'', 
	PROJEKT ''Name'', 
	UPOZORNENI_PRO_KB ''WarningForKb'', 
	UPOZORNENI_PRO_MPSS ''WarningForMp'', 
	STRANKY_PROJEKTU ''Web'', 
	LOKALITA ''Place'', 
	CASE WHEN HROMADNE_OCENENI=-1 THEN ''Probíhá zpracování'' WHEN HROMADNE_OCENENI=0 THEN ''NE'' ELSE ''ANO'' END ''MassEvaluationText'', 
	DOPORUCENI ''Recommandation'', 
	CAST(HROMADNE_OCENENI as int) ''MassValuation'',
	CAST(CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], ''9999-12-31'') THEN 1 ELSE 0 END as bit) ''IsValid'' 
FROM [SBR].HTEDM_CIS_DEV_PROJEKTY_SPV
WHERE DEVELOPER_PROJEKT_ID=@DeveloperProjectId AND DEVELOPER_ID=@DeveloperId' WHERE SqlQueryId='GetDeveloperProject';
