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
     ContractNumber varchar(20),
     [SalesArrangementTypeId] [int] NOT NULL,
     [State] [int] NOT NULL,
     [StateUpdateTime] [datetime] NOT NULL,
     [ChannelId] int not null,
     [CreatedUserName] [nvarchar](100) NOT NULL,
     [CreatedUserId] [int] NOT NULL,
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
     [HasPartner] [bit] NOT NULL,
     [FirstNameNaturalPerson] [nvarchar](100) NULL,
     [Name] [nvarchar](100) NULL,
     [DateOfBirthNaturalPerson] [datetime] NULL,
     [CreatedUserName] [nvarchar](100) NOT NULL,
     [CreatedUserId] [int] NOT NULL,
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
     [Id] [int] NOT NULL,
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
      [HouseholdTypeId] tinyint NOT NULL,
      [ChildrenUpToTenYearsCount] [int] NULL,
      [ChildrenOverTenYearsCount] [int] NULL,
      [PropertySettlementId] [int] NULL,
      SavingExpenseAmount [int] NULL,
      InsuranceExpenseAmount [int] NULL,
      HousingExpenseAmount [int] NULL,
      OtherExpenseAmount [int] NULL,
      [CustomerOnSAId1] [int] NULL,
      [CustomerOnSAId2] [int] NULL,
      [CreatedUserName] [nvarchar](100) NOT NULL,
      [CreatedUserId] [int] NOT NULL,
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
