SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[SalesArrangement] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[SalesArrangement]
GO
DROP TABLE [dbo].[SalesArrangementHistory]
GO

CREATE TABLE [dbo].[SalesArrangement](
	[SalesArrangementId] [int] IDENTITY(1,1) NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[OfferId] [int] NULL,
	[ResourceProcessId] [uniqueidentifier] NULL,
	[RiskBusinessCaseId] [varchar](50) NULL,
    [LoanApplicationAssessmentId] varchar(50) NULL,
    [RiskSegment] varchar(50) NULL,
    [CommandId] varchar(50) NULL,
	[ContractNumber] [varchar](20) NULL,
	[SalesArrangementTypeId] [int] NOT NULL,
	[State] [int] NOT NULL,
	[StateUpdateTime] [datetime] NOT NULL,
	[ChannelId] [int] NOT NULL,
    [OfferGuaranteeDateFrom] date NULL,
	[OfferGuaranteeDateTo] date NULL,
    RiskBusinessCaseExpirationDate date NULL,
    [FirstSignedDate] [datetime] NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_SalesArrangement] PRIMARY KEY CLUSTERED 
(
	[SalesArrangementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[SalesArrangementHistory] )
)
GO


ALTER TABLE [dbo].[SalesArrangementParameters] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[SalesArrangementParameters]
GO
DROP TABLE [dbo].[SalesArrangementParametersHistory]
GO

CREATE TABLE [dbo].[SalesArrangementParameters](
	[SalesArrangementParametersId] [int] IDENTITY(1,1) NOT NULL,
	[SalesArrangementId] [int] NULL,
	[Parameters] [nvarchar](max) NULL,
    [ParametersBin] [varbinary](max) NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_SalesArrangementParameters] PRIMARY KEY CLUSTERED 
(
	[SalesArrangementParametersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY], PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[SalesArrangementParametersHistory] )
)
GO


ALTER TABLE [dbo].[CustomerOnSA] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[CustomerOnSA]
GO
DROP TABLE [dbo].[CustomerOnSAHistory]
GO

CREATE TABLE [dbo].[CustomerOnSA](
     [CustomerOnSAId] [int] IDENTITY(1,1) NOT NULL,
     [SalesArrangementId] [int] NOT NULL,
     [CustomerRoleId] [tinyint] NOT NULL,
     [FirstNameNaturalPerson] [nvarchar](100) NULL,
     [Name] [nvarchar](100) NULL,
     [DateOfBirthNaturalPerson] [datetime] NULL,
     LockedIncomeDateTime datetime NULL,
     [CreatedUserName] [nvarchar](100) NULL,
     [CreatedUserId] [int] NULL,
     [CreatedTime] [datetime] NOT NULL,
     [ModifiedUserId] [int] NULL,
     [ModifiedUserName] [nvarchar](100) NULL,
     [ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
     [ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
     CONSTRAINT [PK_CustomerOnSA] PRIMARY KEY CLUSTERED
         (
          [CustomerOnSAId] ASC
             )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY], PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
  WITH
      (
      SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[CustomerOnSAHistory] )
    )
GO

ALTER TABLE [dbo].[CustomerOnSAObligation] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[CustomerOnSAObligation]
GO
DROP TABLE [dbo].[CustomerOnSAObligationHistory]
GO

CREATE TABLE [dbo].[CustomerOnSAObligation](
	[CustomerOnSAObligationId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerOnSAId] [int] NOT NULL,
	[ObligationTypeId] [int] NOT NULL,
	[InstallmentAmount] decimal(12,2) NULL,
	[LoanPrincipalAmount] decimal(12,2) NULL,
	[CreditCardLimit] decimal(12,2) NULL,
	[ObligationState] [int] NULL,
	[CreditorId] [varchar(10)] NULL,
	[CreditorName] [nvarchar](250) NULL,
	[CreditorIsExternal] [bit] NULL,
	[CorrectionTypeId] [int] NULL,
	[InstallmentAmountCorrection] decimal(12,2) NULL,
	[LoanPrincipalAmountCorrection] decimal(12,2) NULL,
	[CreditCardLimitCorrection] decimal(12,2) NULL,
    [LoanPrincipalAmountConsolidated] decimal(12,2) NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_CustomerOnSAObligation] PRIMARY KEY CLUSTERED 
(
	[CustomerOnSAObligationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAObligationHistory])
)
GO

ALTER TABLE [dbo].[CustomerOnSAIdentity] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[CustomerOnSAIdentity]
GO
DROP TABLE [dbo].[CustomerOnSAIdentityHistory]
GO
CREATE TABLE [dbo].[CustomerOnSAIdentity](
     [CustomerOnSAIdentityId] [int] IDENTITY(1,1) NOT NULL,
     [CustomerOnSAId] [int] NULL,
     [IdentityScheme] [tinyint] NOT NULL,
     [IdentityId] [bigint] NOT NULL,
     [ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
     [ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
     CONSTRAINT [PK_CustomerOnSAIdentity] PRIMARY KEY CLUSTERED
         (
          [CustomerOnSAIdentityId] ASC
             )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY], PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
  WITH
      (
      SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[CustomerOnSAIdentityHistory] )
    )
GO

ALTER TABLE [dbo].[Household] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[Household]
GO
DROP TABLE [dbo].[HouseholdHistory]
GO
CREATE TABLE [dbo].[Household](
      [HouseholdId] [int] IDENTITY(1,1) NOT NULL,
      [SalesArrangementId] [int] NOT NULL,
      [CaseId] bigint NOT NULL,
      [HouseholdTypeId] tinyint NOT NULL,
      [ChildrenUpToTenYearsCount] [int] NULL,
      [ChildrenOverTenYearsCount] [int] NULL,
      [PropertySettlementId] [int] NULL,
      SavingExpenseAmount [int] NULL,
      InsuranceExpenseAmount [int] NULL,
      HousingExpenseAmount [int] NULL,
      OtherExpenseAmount [int] NULL,
      AreBothPartnersDeptors bit NULL,
      AreCustomersPartners bit NULL,
      [CustomerOnSAId1] [int] NULL,
      [CustomerOnSAId2] [int] NULL,
      [CreatedUserName] [nvarchar](100) NULL,
      [CreatedUserId] [int] NULL,
      [CreatedTime] [datetime] NOT NULL,
      [ModifiedUserId] [int] NULL,
      [ModifiedUserName] [nvarchar](100) NULL,
      [ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
      [ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
      CONSTRAINT [PK_Household] PRIMARY KEY CLUSTERED
          (
           [HouseholdId] ASC
              )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY], PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
)
  WITH
      (
      SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[HouseholdHistory] )
    )
GO

ALTER TABLE [dbo].[CustomerOnSAIncome] SET ( SYSTEM_VERSIONING = OFF)
GO
DROP TABLE [dbo].[CustomerOnSAIncome]
GO
DROP TABLE [dbo].[CustomerOnSAIncomeHistory]
GO

CREATE TABLE [dbo].[CustomerOnSAIncome](
	[CustomerOnSAIncomeId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerOnSAId] [int] NOT NULL,
	[IncomeTypeId] [int] NOT NULL,
	[Sum] decimal(12,2) NULL,
	[CurrencyCode] [varchar](3) NULL,
    [IncomeSource] [nvarchar](255) NULL,
    [HasProofOfIncome] [bit] NULL,
	[Data] [nvarchar](max) NULL,
    [DataBin] [varbinary](max) NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_CustomerIncome] PRIMARY KEY CLUSTERED 
(
	[CustomerOnSAIncomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[CustomerOnSAIncomeHistory] )
)
GO
