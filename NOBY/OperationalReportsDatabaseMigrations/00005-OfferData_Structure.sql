CREATE OR ALTER     PROCEDURE [dbo].[sp_CopyNobyData] AS 
BEGIN

	SET NOCOUNT ON;

	DROP TABLE IF EXISTS [dbo].[Case_new]
	DROP TABLE IF EXISTS [dbo].[SalesArrangement_new]
	DROP TABLE IF EXISTS [dbo].[Offer_new]
	DROP TABLE IF EXISTS [dbo].[OfferData_new]

	BEGIN TRAN

	SELECT * INTO [dbo].[Case_new] FROM [dbo].[NOBY_Case]
	SELECT * INTO [dbo].[SalesArrangement_new] FROM [dbo].[NOBY_SalesArrangement]
	SELECT * INTO [dbo].[Offer_new] FROM [dbo].[NOBY_Offer]
	SELECT * INTO [dbo].[OfferData_new] FROM [dbo].[NOBY_OfferData]

	CREATE INDEX IX_Case_CaseId ON [dbo].[Case_new] (CaseId)
	CREATE INDEX IX_SalesArrangement_SalesArrangementId ON [dbo].[SalesArrangement_new] (SalesArrangementId)
	CREATE INDEX IX_Offer_OfferId ON [dbo].[Offer_new] (OfferId)
	CREATE INDEX IX_Offer_OfferId ON [dbo].[OfferData_new] (DocumentDataEntityId)

	DROP TABLE IF EXISTS [dbo].[Case]
	DROP TABLE IF EXISTS [dbo].[SalesArrangement]
	DROP TABLE IF EXISTS [dbo].[Offer]
	DROP TABLE IF EXISTS [dbo].[OfferData]

	EXEC sp_rename 'Case_New', 'Case';
	EXEC sp_rename 'SalesArrangement_new', 'SalesArrangement';
	EXEC sp_rename 'Offer_new', 'Offer';
    EXEC sp_rename 'OfferData_new', 'OfferData';

	COMMIT
END

GO

EXECUTE [dbo].[sp_CopyNobyData] 

GO

CREATE OR ALTER   VIEW [dbo].[vw_NobyCases] AS
SELECT sa.CaseId,
    sa.CreatedUserId,
    sa.CreatedUserName,
    c.FirstNameNaturalPerson + ' ' + c.Name as CustomerFullName,
    CASE WHEN c.CustomerIdentityScheme = 1 THEN 'MP' WHEN c.CustomerIdentityScheme = 2 THEN 'KB' ELSE 'N/A' END as CustomerIdentityScheme,
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
    CAST(SimulationResults.LoanInterestRateProvidedUnits + '.' + SimulationResults.LoanInterestRateProvidedNanos AS DECIMAL(14, 2)) as LoanInterestRateProvided,
    c.CreatedTime as CaseCreatedTime,
    o.FirstGeneratedDocumentDate as OfferGenerateTime
FROM SalesArrangement sa
INNER JOIN Offer o ON o.OfferId = sa.OfferId
INNER JOIN OfferData od ON od.DocumentDataEntityId = sa.OfferId
INNER JOIN [Case] c ON c.CaseId = sa.CaseId
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
    LoanAmount DECIMAL '$.SimulationInputs.LoanAmount.Units',
    LoanDuration INT '$.SimulationInputs.LoanDuration',
    FixedRatePeriod INT '$.SimulationInputs.FixedRatePeriod'
) as SimulationInputs
CROSS APPLY OPENJSON(od.[Data]) WITH
(
    LoanInterestRateProvidedUnits VARCHAR '$.SimulationOutputs.LoanInterestRateProvided.Units',
    LoanInterestRateProvidedNanos VARCHAR '$.SimulationOutputs.LoanInterestRateProvided.Nanos'
) as SimulationResults
OUTER APPLY (SELECT TOP 1 * FROM OPENJSON(SimulationInputs.LoanPurposesJson) WITH 
(
    LoanPurposeId INT '$.LoanPurposeId',
    LoanPurposeValue DECIMAL '$.Sum.Units'
) as LoanPurposes ORDER BY LoanPurposeValue desc) as LoanPurpose

GO
