IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RealEstateValuation]') AND type in (N'U'))
ALTER TABLE [dbo].[RealEstateValuation] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[RealEstateValuation]
GO
DROP TABLE IF EXISTS [dbo].[RealEstateValuationHistory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RealEstateValuationDetail]') AND type in (N'U'))
ALTER TABLE [dbo].[RealEstateValuationDetail] SET ( SYSTEM_VERSIONING = OFF  )
GO
DROP TABLE IF EXISTS [dbo].[RealEstateValuationDetail]
GO
DROP TABLE IF EXISTS [dbo].[RealEstateValuationDetailHistory]
GO

CREATE TABLE [dbo].[RealEstateValuation](
	[RealEstateValuationId] [int] IDENTITY(1,1) NOT NULL,
	[CaseId] [bigint] NOT NULL,
	[RealEstateTypeId] [int] NOT NULL,
	[ValuationStateId] [int] NOT NULL,
	[IsLoanRealEstate] [bit] NOT NULL,
	[ValuationTypeId] [int] NOT NULL,
	[IsRevaluationRequired] [bit] NOT NULL,
	[DeveloperAllowed] [bit] NOT NULL,
	[DeveloperApplied] [bit] NOT NULL,
	[ValuationSentDate] [date] NULL,
	[Address] [nvarchar](500) NULL,
	[RealEstateStateId] [int] NOT NULL,
	[OrderId] [int] NULL,
	[ValuationResultCurrentPrice] [int] NULL,
	[ValuationResultFuturePrice] [int] NULL,
	[CreatedUserName] [nvarchar](100) NOT NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_RealEstateValuation] PRIMARY KEY CLUSTERED 
(
	[RealEstateValuationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RealEstateValuationHistory])
)
GO

CREATE TABLE [dbo].[RealEstateValuationDetail](
	[RealEstateValuationId] [int] NOT NULL,
	[RealEstateSubtypeId] [int] NULL,
	[LoanPurposeDetails] [nvarchar](max) NULL,
	[LoanPurposeDetailsBin] [varbinary](max) NULL,
	[SpecificDetail] [nvarchar](max) NULL,
	[SpecificDetailBin] [varbinary](max) NULL,
	[CreatedUserName] [nvarchar](100) NOT NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ModifiedUserName] [nvarchar](100) NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_RealEstateValuationDetail] PRIMARY KEY CLUSTERED 
(
	[RealEstateValuationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[RealEstateValuationDetailHistory])
)
GO

