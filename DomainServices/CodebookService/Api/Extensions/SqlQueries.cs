namespace DomainServices.CodebookService.Api.Extensions;

internal static class SqlQueries
{
    public const string AcademicDegreesAfter = "SELECT KOD 'Id', TEXT 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_TITULY_ZA] ORDER BY TEXT ASC";

    public const string AcademicDegreesBefore = "SELECT Id, Name, CAST(1 as bit) 'IsValid' FROM dbo.AcademicDegreesBefore ORDER BY Id ASC";

    public const string BankCodes = "SELECT KOD_BANKY 'BankCode', NAZOV_BANKY 'Name', SKRAT_NAZOV_BANKY 'ShortName', SKRATKA_STATU_PRE_IBAN 'State', CASE WHEN SYSDATETIME() <= ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.CIS_KODY_BANK ORDER BY KOD_BANKY ASC";

    public const string ClassificationOfEconomicActivities = "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_OKEC] ORDER BY KOD ASC";

    public const string CollateralTypes = "SELECT TYP_ZABEZPECENIA 'CollateralType', NULLIF(MANDANT, 0) 'MandantId', KOD_BGM 'CodeBgm', TEXT_BGM 'TextBgm', TEXT_K_TYPU 'NameType' FROM [SBR].[CIS_VAHY_ZABEZPECENI] ORDER BY TYP_ZABEZPECENIA ASC";

    public const string ContactTypes = "SELECT TYP_KONTAKTU 'Id', TEXT 'Name', NULLIF(MANDANT, 0) 'MandantId', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_TYPY_KONTAKTOV] ORDER BY TYP_KONTAKTU ASC";

    public const string Countries = "SELECT KOD 'Id', SKRATKA 'ShortName', TEXT 'Name', TEXT_CELY 'LongName', DEF 'IsDefault', RIZIKOVOST 'Risk', CLEN_EU 'EuMember', EUROZONA 'Eurozone' FROM [SBR].[CIS_STATY] WHERE KOD != -1 ORDER BY KOD ASC";

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
}
