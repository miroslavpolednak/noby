DROP TABLE IF EXISTS dbo.[MigrationHistory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerOnSAIncome]') AND type in (N'U'))
ALTER TABLE [dbo].[CustomerOnSAIncome] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAIncome]
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAIncomeHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerOnSAObligation]') AND type in (N'U'))
ALTER TABLE [dbo].[CustomerOnSAObligation] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAObligation]
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAObligationHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerOnSA]') AND type in (N'U'))
ALTER TABLE [dbo].[CustomerOnSA] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSA]
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Household]') AND type in (N'U'))
ALTER TABLE [dbo].[Household] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[Household]
GO
DROP TABLE IF EXISTS [dbo].[HouseholdHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerOnSAIdentity]') AND type in (N'U'))
ALTER TABLE [dbo].[CustomerOnSAIdentity] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAIdentity]
GO
DROP TABLE IF EXISTS [dbo].[CustomerOnSAIdentityHistory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAIdentityHistory])
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Household](
	[HouseholdId] [int] IDENTITY(1,1) NOT NULL,
	[SalesArrangementId] [int] NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[HouseholdTypeId] [tinyint] NOT NULL,
	[ChildrenUpToTenYearsCount] [int] NULL,
	[ChildrenOverTenYearsCount] [int] NULL,
	[PropertySettlementId] [int] NULL,
	[SavingExpenseAmount] [int] NULL,
	[InsuranceExpenseAmount] [int] NULL,
	[HousingExpenseAmount] [int] NULL,
	[OtherExpenseAmount] [int] NULL,
	[AreBothPartnersDeptors] [bit] NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[HouseholdHistory])
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerOnSA](
	[CustomerOnSAId] [int] IDENTITY(1,1) NOT NULL,
	[SalesArrangementId] [int] NOT NULL,
	[CustomerRoleId] [tinyint] NOT NULL,
	[HasPartner] [bit] NULL,
	[FirstNameNaturalPerson] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[DateOfBirthNaturalPerson] [datetime] NULL,
	[LockedIncomeDateTime] [datetime] NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
	[MaritalStatusId] [int] NULL,
	[AdditionalDataBin] [varbinary](max) NULL,
	[AdditionalData] [nvarchar](max) NULL,
	[ChangeData] [nvarchar](max) NULL,
 CONSTRAINT [PK_CustomerOnSA] PRIMARY KEY CLUSTERED 
(
	[CustomerOnSAId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAHistory])
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerOnSAObligation](
	[CustomerOnSAObligationId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerOnSAId] [int] NOT NULL,
	[ObligationTypeId] [int] NOT NULL,
	[InstallmentAmount] [decimal](12, 2) NULL,
	[LoanPrincipalAmount] [decimal](12, 2) NULL,
	[CreditCardLimit] [decimal](12, 2) NULL,
	[ObligationState] [int] NULL,
	[CreditorId] [varchar](10) NULL,
	[CreditorName] [nvarchar](250) NULL,
	[CreditorIsExternal] [bit] NULL,
	[CorrectionTypeId] [int] NULL,
	[InstallmentAmountCorrection] [decimal](12, 2) NULL,
	[LoanPrincipalAmountCorrection] [decimal](12, 2) NULL,
	[CreditCardLimitCorrection] [decimal](12, 2) NULL,
	[AmountConsolidated] [decimal](12, 2) NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAObligationHistory])
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerOnSAIncome](
	[CustomerOnSAIncomeId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerOnSAId] [int] NOT NULL,
	[IncomeTypeId] [int] NOT NULL,
	[Sum] [decimal](12, 2) NULL,
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
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CustomerOnSAIncomeHistory])
)
GO
