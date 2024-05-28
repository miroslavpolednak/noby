USE [NobyOperationalReports]
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
    SimulationResults.LoanPaymentAmount,
    SimulationResults.LoanInterestRateProvided,
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
    LoanAmount DECIMAL '$.SimulationInputs.LoanAmount',
    LoanDuration INT '$.SimulationInputs.LoanDuration',
    FixedRatePeriod INT '$.SimulationInputs.FixedRatePeriod'
) as SimulationInputs
CROSS APPLY OPENJSON(od.[Data]) WITH
(
    LoanPaymentAmount DECIMAL(14, 2) '$.SimulationOutputs.LoanPaymentAmount',
    LoanInterestRateProvided DECIMAL(14, 2) '$.SimulationOutputs.LoanInterestRateProvided'
) as SimulationResults
OUTER APPLY (SELECT TOP 1 * FROM OPENJSON(SimulationInputs.LoanPurposesJson) WITH 
(
    LoanPurposeId INT '$.LoanPurposeId',
    LoanPurposeValue DECIMAL '$.Sum'
) as LoanPurposes ORDER BY LoanPurposeValue desc) as LoanPurpose

GO