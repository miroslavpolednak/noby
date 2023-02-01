IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Case]') AND type in (N'U'))
ALTER TABLE [dbo].[Case] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[Case]
GO
DROP TABLE IF EXISTS [dbo].[CaseHistory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveTask]') AND type in (N'U'))
ALTER TABLE [dbo].[ActiveTask] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[ActiveTask]
GO
DROP TABLE IF EXISTS [dbo].[ActiveTaskHistory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
	[ProductTypeId] [int] NOT NULL,
	[State] [int] NOT NULL,
	[StateUpdateTime] [datetime] NOT NULL,
	[CreatedUserName] [nvarchar](100) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_Case] PRIMARY KEY CLUSTERED 
(
	[CaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[CaseHistory])
)
GO


CREATE TABLE [dbo].[ActiveTask](
	[ActiveTaskId] [int] IDENTITY(1,1) NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[TaskTypeId] [int] NOT NULL,
	[TaskProcessId] [int] NOT NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_ActiveTask] PRIMARY KEY CLUSTERED 
(
	[ActiveTaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ActiveTaskHistory])
)
GO