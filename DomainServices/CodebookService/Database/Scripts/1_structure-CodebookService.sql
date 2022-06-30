USE [CodebookService]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EA_CIS_EACODEMAIN](
	[kod] [int] NOT NULL,
	[popis] [varchar](100) NULL,
	[popis_klient] [varchar](100) NULL,
	[platnost_od] [date] NULL,
	[platnost_do] [date] NULL,
	[kategorie] [varchar](64) NULL,
	[druh_kb] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[kod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[EducationLevelExtension](
	[EducationLevelId] [int] NOT NULL,
	[RDMCode] [varchar](10) NULL,
 CONSTRAINT [PK_EducationLevelExtension] PRIMARY KEY CLUSTERED 
(
	[EducationLevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[RiskApplicationType](
	[Id] [int] NOT NULL,
	[Mandant] [tinyint] NOT NULL,
	[DateFrom] [date] NOT NULL,
	[DateTo] [date] NULL,
	[LoanProductsId] [nvarchar](100) NOT NULL,
	[LoanType] [nvarchar](100) NULL,
	[MarketingActions] [nvarchar](100) NULL,
	[LtvFrom] [numeric](16, 4) NULL,
	[LtvTo] [numeric](16, 4) NULL,
	[ClusterCode] [nvarchar](50) NOT NULL,
	[C4mAplTypeId] [nvarchar](50) NOT NULL,
	[C4mAplTypeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RiskApplicationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[IdentificationDocumentTypeExtension](
	[IdentificationDocumentTypeId] [int] NOT NULL,
	[RDMCode] [varchar](10) NULL,
 CONSTRAINT [PK_IdentificationDocumentTypeExtension] PRIMARY KEY CLUSTERED 
(
	[IdentificationDocumentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaritalStatusExtension](
	[MaritalStatusId] [int] NOT NULL,
	[RDMCode] [varchar](10) NULL,
 CONSTRAINT [PK_MaritalStatusExtension] PRIMARY KEY CLUSTERED 
(
	[MaritalStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[SalesArrangementType](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](150) NOT NULL,
    [ProductTypeId] [int] NULL,
    [IsDefault] [bit] NOT NULL,
    CONSTRAINT [PK_SalesArrangementType] PRIMARY KEY CLUSTERED
    (
    [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ProductTypeExtension](
    [ProductTypeId] [int] NOT NULL,
    [ProductCategory] [tinyint] NOT NULL,
    [MpHomeApiLoanType] [varchar](50) NULL,
    [KonsDbLoanType] [tinyint] NOT NULL,
    CONSTRAINT [PK_ProductTypeExtension] PRIMARY KEY CLUSTERED
    (
    [ProductTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[RelationshipCustomerProductTypeExtension](
    [RelationshipCustomerProductTypeId] [int] NOT NULL,
    [MpHomeApiContractRelationshipType] [varchar](50) NOT NULL,
    CONSTRAINT [PK_RelationshipCustomerProductTypeExtension] PRIMARY KEY CLUSTERED
    (
    [RelationshipCustomerProductTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[WorkflowTaskStateExtension](
	[WorkflowTaskStateId] [int] NOT NULL,
	[Flag] [tinyint] NOT NULL,
 CONSTRAINT [PK_WorkflowTaskStateExtension] PRIMARY KEY CLUSTERED 
(
	[WorkflowTaskStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[WorkflowTaskTypeExtension](
	[WorkflowTaskTypeId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_WorkflowTaskTypeExtension] PRIMARY KEY CLUSTERED 
(
	[WorkflowTaskTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
