﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CaseInstance](
	[CaseId] [bigint] NOT NULL,
	[OwnerUserId] [int] NOT NULL,
	[OwnerUserName] [nvarchar](100) NOT NULL,
	[CustomerIdentityScheme] [tinyint] NULL,
	[CustomerIdentityId] [int] NULL,
	[FirstNameNaturalPerson] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[DateOfBirthNaturalPerson] [date] NULL,
	[Cin] [varchar](10) NULL,
	[ContractNumber] [varchar](10) NULL,
	[TargetAmount] [int] NULL,
	[IsActionRequired] [bit] NOT NULL,
	[ProductInstanceTypeId] [int] NOT NULL,
	[State] [int] NOT NULL,
	[StateUpdateTime] [datetime] NOT NULL,
	[CreatedUserName] [nvarchar](100) NOT NULL,
	[CreatedUserId] [int] NOT NULL,
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
SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [dbo].[CaseInstanceHistory] )
)
GO

ALTER TABLE [dbo].[CaseInstance] ADD  CONSTRAINT [DF_CaseInstance_ActionRequired]  DEFAULT ((0)) FOR [IsActionRequired]
GO