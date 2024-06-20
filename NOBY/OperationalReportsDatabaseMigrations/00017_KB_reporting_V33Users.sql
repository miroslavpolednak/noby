CREATE OR ALTER PROCEDURE [dbo].[sp_SyncV33Users] AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @V33ID INT;
    DECLARE v33UserCursor CURSOR FOR SELECT DISTINCT CreatedUserId FROM [dbo].[vw_CaseWithArchived];

    DROP TABLE IF EXISTS [dbo].[V33Users];

	SELECT * INTO [dbo].[V33Users] FROM [dbo].[xxvvss_v33PMP_User_S] WHERE v33id IN (SELECT DISTINCT CreatedUserId FROM [dbo].[vw_CaseWithArchived])
END;

GO

CREATE OR ALTER PROCEDURE [dbo].[sp_CopyNobyData] AS 
BEGIN

	SET NOCOUNT ON;

	DROP TABLE IF EXISTS [dbo].[Case_new]
	DROP TABLE IF EXISTS [dbo].[CaseHistory_new]
	DROP TABLE IF EXISTS [dbo].[SalesArrangement_new]
	DROP TABLE IF EXISTS [dbo].[SalesArrangementHistory_new]
	DROP TABLE IF EXISTS [dbo].[Offer_new]
	DROP TABLE IF EXISTS [dbo].[OfferMortgageData_new]

	BEGIN TRAN

	--Copy data to new tables
	SELECT * INTO [dbo].[Case_new] FROM [dbo].[NOBY_Case_S]
	SELECT * INTO [dbo].[CaseHistory_new] FROM [dbo].[NOBY_CaseHistory_S]
	SELECT * INTO [dbo].[SalesArrangement_new] FROM [dbo].[NOBY_SalesArrangement_S]
	SELECT * INTO [dbo].[SalesArrangementHistory_new] FROM [dbo].[NOBY_SalesArrangementHistory_S]
	SELECT * INTO [dbo].[Offer_new] FROM [dbo].[NOBY_Offer_S]
	SELECT * INTO [dbo].[OfferMortgageData_new] FROM [dbo].[NOBY_OfferMortgageData_S]

	--Set indexes
	ALTER TABLE [dbo].[Case_new] ADD PRIMARY KEY (CaseId)
	ALTER TABLE [dbo].[Case_new] ADD PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])

	ALTER TABLE [dbo].[SalesArrangement_new] ADD PRIMARY KEY (SalesArrangementId)
	ALTER TABLE [dbo].[SalesArrangement_new] ADD PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])

	CREATE INDEX IX_Offer_OfferId ON [dbo].[Offer_new] (OfferId)
	CREATE INDEX IX_Offer_OfferId ON [dbo].[OfferMortgageData_new] (DocumentDataEntityId)

	--Drop old tables
	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Case' AND temporal_type = 2)
	BEGIN
		ALTER TABLE [dbo].[Case] SET ( SYSTEM_VERSIONING = OFF  )
		DROP TABLE IF EXISTS [dbo].[CaseHistory]
	END

	IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SalesArrangement' AND temporal_type = 2)
	BEGIN
		ALTER TABLE [dbo].[SalesArrangement] SET ( SYSTEM_VERSIONING = OFF  )
		DROP TABLE IF EXISTS [dbo].[SalesArrangementHistory]
	END

	DROP TABLE IF EXISTS [dbo].[Case]
	DROP TABLE IF EXISTS [dbo].[SalesArrangement]
	DROP TABLE IF EXISTS [dbo].[Offer]
	DROP TABLE IF EXISTS [dbo].[OfferMortgageData]

	--Rename new tables
	EXEC sp_rename 'Case_New', 'Case';
	EXEC sp_rename 'CaseHistory_New', 'CaseHistory';
	EXEC sp_rename 'SalesArrangement_new', 'SalesArrangement';
	EXEC sp_rename 'SalesArrangementHistory_new', 'SalesArrangementHistory';
	EXEC sp_rename 'Offer_new', 'Offer';
    EXEC sp_rename 'OfferMortgageData_new', 'OfferMortgageData';

	--Turn on versioning (enable SELECT with SYSTEM_TIME ALL)
	ALTER TABLE [dbo].[Case] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CaseHistory]));
	ALTER TABLE [dbo].[SalesArrangement] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SalesArrangementHistory]));

	COMMIT

	EXEC [dbo].[sp_SyncV33Users]
END

GO

EXEC [dbo].[sp_CopyNobyData]

GO

CREATE OR ALTER   VIEW [dbo].[vw_Report_KB_NobyLoanInitialOffer] AS
SELECT sa.CaseId,
    c.CreatedUserId,
    c.CreatedUserName,
	usr.v33OSCIS,
	usr.v33cpm,
	usr.v33icp,
    c.FirstNameNaturalPerson + ' ' + c.Name as CustomerFullName,
    CASE WHEN c.CustomerIdentityScheme = 1 THEN 'MP' WHEN c.CustomerIdentityScheme = 2 THEN 'KB' ELSE NULL END as CustomerIdentityScheme,
    c.CustomerIdentityId,
    CASE 
        WHEN c.State = 1 THEN 'InProgress'
        WHEN c.State = 2 OR c.State = 8 THEN 'InApproval'
        WHEN c.State = 3 THEN 'InSigning'
        WHEN c.State = 4 THEN 'InDisbursement'
        WHEN c.State = 5 THEN 'InAdministration'
        WHEN c.State = 6 THEN 'Finished'
        WHEN c.State = 7 OR c.State = 9 OR c.State = 10 THEN 'Cancelled'
        ELSE 'N/A'
    END as CaseState,
    SimulationInputs.ProductTypeId,
    SimulationInputs.LoanKindId,
    LoanPurpose.LoanPurposeId,
    LoanPurpose.LoanPurposeValue,
	sa.FirstSignatureDate,
    sa.OfferGuaranteeDateTo,
    sa.FirstLoanAssessmentDate,
    DATEADD(day, -90, sa.RiskBusinessCaseExpirationDate) as RiskBusinessCaseRequestedDate,
    sa.RiskBusinessCaseExpirationDate,
    SimulationInputs.Domicile,
    SimulationInputs.HealthRiskInsurance,
    SimulationInputs.RealEstateInsurance,
    SimulationInputs.IncomeLoanRatioDiscount,
    SimulationInputs.UserVip,
    SimulationInputs.IsEmployeeBonusRequested,
    SimulationInputs.LoanAmount,
    SimulationInputs.LoanDuration,
    SimulationInputs.FixedRatePeriod,
    SimulationResults.LoanPaymentAmount,
	SimulationInputs.InterestRateDiscount,
    SimulationResults.LoanInterestRateProvided,
	SimulationResults.LoanInterestRateAnnounced,
    SimulationResults.LoanInterestRateAnnouncedType,
    c.CreatedTime as CaseCreatedTime,
    o.FirstGeneratedDocumentDate as OfferGenerateTime
FROM [vw_SalesArrangementWithArchived] sa
INNER JOIN [vw_CaseWithArchived] c ON c.CaseId = sa.CaseId
INNER JOIN [Offer] o ON o.OfferId = sa.OfferId
INNER JOIN [OfferMortgageData] od ON od.DocumentDataEntityId = sa.OfferId
LEFT JOIN [V33Users] usr ON usr.V33Id = c.CreatedUserId
CROSS APPLY OPENJSON(od.[Data]) WITH
(
	ProductTypeId INT '$.SimulationInputs.ProductTypeId',
    LoanKindId INT '$.SimulationInputs.LoanKindId',
    LoanPurposesJson NVARCHAR(MAX) '$.SimulationInputs.LoanPurposes' AS JSON,
    Domicile BIT '$.SimulationInputs.MarketingActions.Domicile',
    HealthRiskInsurance BIT '$.SimulationInputs.MarketingActions.HealthRiskInsurance',
    RealEstateInsurance BIT '$.SimulationInputs.MarketingActions.RealEstateInsurance',
    IncomeLoanRatioDiscount BIT '$.SimulationInputs.MarketingActions.IncomeLoanRatioDiscount',
    UserVip BIT '$.SimulationInputs.MarketingActions.UserVip',
    IsEmployeeBonusRequested BIT '$.SimulationInputs.IsEmployeeBonusRequested',
    LoanAmount DECIMAL '$.SimulationInputs.LoanAmount',
    LoanDuration INT '$.SimulationInputs.LoanDuration',
    FixedRatePeriod INT '$.SimulationInputs.FixedRatePeriod',
	InterestRateDiscount DECIMAL (14, 2) '$.SimulationInputs.InterestRateDiscount'
) as SimulationInputs
CROSS APPLY OPENJSON(od.[Data]) WITH
(
    LoanPaymentAmount DECIMAL(14, 2) '$.SimulationOutputs.LoanPaymentAmount',
    LoanInterestRateProvided DECIMAL(14, 2) '$.SimulationOutputs.LoanInterestRateProvided',
	LoanInterestRateAnnounced DECIMAL(14, 2) '$.SimulationOutputs.LoanInterestRateAnnounced',
	LoanInterestRateAnnouncedType INT '$.SimulationOutputs.LoanInterestRateAnnouncedType'
) as SimulationResults
OUTER APPLY (SELECT TOP 1 * FROM OPENJSON(SimulationInputs.LoanPurposesJson) WITH 
(
    LoanPurposeId INT '$.LoanPurposeId',
    LoanPurposeValue DECIMAL '$.Sum'
) as LoanPurposes ORDER BY LoanPurposeValue desc) as LoanPurpose
WHERE sa.SalesArrangementTypeId = 1 --Mortgage

GO