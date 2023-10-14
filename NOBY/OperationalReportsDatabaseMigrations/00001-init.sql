CREATE TABLE [dbo].[Case](
	[CaseId] [bigint] NOT NULL,
	[OwnerUserId] [int] NOT NULL,
	[OwnerUserName] [nvarchar](100) NOT NULL,
	[CustomerIdentityScheme] [tinyint] NULL,
	[CustomerIdentityId] [bigint] NULL,
	[FirstNameNaturalPerson] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[DateOfBirthNaturalPerson] [date] NULL,
	[Cin] [varchar](10) NULL,
	[EmailForOffer] [varchar](50) NULL,
	[PhoneNumberForOffer] [varchar](20) NULL,
	[PhoneIDCForOffer] [varchar](10) NULL,
	[ContractNumber] [varchar](20) NULL,
	[TargetAmount] [decimal](12, 2) NULL,
	[IsEmployeeBonusRequested] [bit] NULL,
	[ProductTypeId] [int] NOT NULL,
	[State] [int] NOT NULL,
	[StateUpdatedInStarbuild] [tinyint] NOT NULL,
	[StateUpdateTime] [datetime] NOT NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NOT NULL
)

CREATE TABLE [dbo].[Offer](
	[OfferId] [int] NOT NULL,
	[ResourceProcessId] [uniqueidentifier] NOT NULL,
	[BasicParameters] [nvarchar](max) NOT NULL,
	[SimulationInputs] [nvarchar](max) NOT NULL,
	[SimulationResults] [nvarchar](max) NOT NULL,
	[AdditionalSimulationResults] [nvarchar](max) NOT NULL,
	[BasicParametersBin] [varbinary](max) NOT NULL,
	[SimulationInputsBin] [varbinary](max) NOT NULL,
	[SimulationResultsBin] [varbinary](max) NOT NULL,
	[AdditionalSimulationResultsBin] [varbinary](max) NOT NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[IsCreditWorthinessSimpleRequested] [bit] NOT NULL,
	[CreditWorthinessSimpleInputs] [nvarchar](max) NULL,
	[CreditWorthinessSimpleInputsBin] [varbinary](max) NULL
)

CREATE TABLE [dbo].[SalesArrangement](
	[SalesArrangementId] [int] NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[OfferId] [int] NULL,
	[ResourceProcessId] [uniqueidentifier] NULL,
	[RiskBusinessCaseId] [varchar](50) NULL,
	[LoanApplicationAssessmentId] [varchar](50) NULL,
	[RiskSegment] [varchar](50) NULL,
	[CommandId] [varchar](50) NULL,
	[ContractNumber] [varchar](20) NULL,
	[SalesArrangementTypeId] [int] NOT NULL,
	[State] [int] NOT NULL,
	[StateUpdateTime] [datetime] NOT NULL,
	[ChannelId] [int] NOT NULL,
	[OfferGuaranteeDateFrom] [date] NULL,
	[OfferGuaranteeDateTo] [date] NULL,
	[RiskBusinessCaseExpirationDate] [date] NULL,
	[OfferDocumentId] [varchar](100) NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NOT NULL
)

GO

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
CROSS APPLY (SELECT TOP 1 * FROM OPENJSON(SimulationInputs.LoanPurposesJson) WITH 
(
	LoanPurposeId INT '$.LoanPurposeId',
	LoanPurposeValue DECIMAL '$.Sum.Units'
) as LoanPurposes ORDER BY LoanPurposeValue desc) as LoanPurpose
GO


