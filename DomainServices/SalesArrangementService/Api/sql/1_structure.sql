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
