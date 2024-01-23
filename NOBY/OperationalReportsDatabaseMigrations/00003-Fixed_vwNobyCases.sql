CREATE OR ALTER VIEW [dbo].[vw_NobyCases] AS
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
    o.CreatedTime as OfferGenerateTime
FROM SalesArrangement sa
INNER JOIN Offer o ON o.OfferId = sa.OfferId
INNER JOIN [Case] c ON c.CaseId = sa.CaseId
CROSS APPLY OPENJSON(o.SimulationInputs) WITH
(
    ProductTypeId INT '$.ProductTypeId',
    LoanKindId INT '$.LoanKindId',
    LoanPurposesJson NVARCHAR(MAX) '$.LoanPurposes' AS JSON,
    Domicile BIT '$.MarketingActions.Domicile',
    HealthRiskInsurance BIT '$.MarketingActions.HealthRiskInsurance',
    RealEstateInsurance BIT '$.MarketingActions.RealEstateInsurance',
    IncomeLoanRatioDiscount BIT '$.MarketingActions.IncomeLoanRatioDiscount',
    UserVip BIT '$.MarketingActions.UserVip',
    IsEmployeeBonusRequested BIT '$.IsEmployeeBonusRequested',
    LoanAmount DECIMAL '$.LoanAmount.Units',
    LoanDuration INT '$.LoanDuration',
    FixedRatePeriod INT '$.FixedRatePeriod'
) as SimulationInputs
CROSS APPLY OPENJSON(o.SimulationResults) WITH
(
    LoanInterestRateProvidedUnits VARCHAR '$.LoanInterestRateProvided.Units',
    LoanInterestRateProvidedNanos VARCHAR '$.LoanInterestRateProvided.Nanos'
) as SimulationResults
OUTER APPLY (SELECT TOP 1 * FROM OPENJSON(SimulationInputs.LoanPurposesJson) WITH 
(
    LoanPurposeId INT '$.LoanPurposeId',
    LoanPurposeValue DECIMAL '$.Sum.Units'
) as LoanPurposes ORDER BY LoanPurposeValue desc) as LoanPurpose