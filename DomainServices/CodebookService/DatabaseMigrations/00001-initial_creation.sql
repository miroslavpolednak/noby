DROP TABLE IF EXISTS dbo.[MigrationHistory]
GO

/****** Object:  Table [dbo].[WorkflowTaskTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkflowTaskTypeExtension]') AND type in (N'U'))
DROP TABLE [dbo].[WorkflowTaskTypeExtension]
GO
/****** Object:  Table [dbo].[WorkflowTaskStateExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkflowTaskStateExtension]') AND type in (N'U'))
DROP TABLE [dbo].[WorkflowTaskStateExtension]
GO
/****** Object:  Table [dbo].[SmsNotificationType]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SmsNotificationType]') AND type in (N'U'))
DROP TABLE [dbo].[SmsNotificationType]
GO
/****** Object:  Table [dbo].[SalesArrangementType]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesArrangementType]') AND type in (N'U'))
DROP TABLE [dbo].[SalesArrangementType]
GO
/****** Object:  Table [dbo].[RiskApplicationType]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RiskApplicationType]') AND type in (N'U'))
DROP TABLE [dbo].[RiskApplicationType]
GO
/****** Object:  Table [dbo].[RelationshipCustomerProductTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RelationshipCustomerProductTypeExtension]') AND type in (N'U'))
DROP TABLE [dbo].[RelationshipCustomerProductTypeExtension]
GO
/****** Object:  Table [dbo].[ProfessionCategoryExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProfessionCategoryExtension]') AND type in (N'U'))
DROP TABLE [dbo].[ProfessionCategoryExtension]
GO
/****** Object:  Table [dbo].[ProductTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductTypeExtension]') AND type in (N'U'))
DROP TABLE [dbo].[ProductTypeExtension]
GO
/****** Object:  Table [dbo].[ObligationTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObligationTypeExtension]') AND type in (N'U'))
DROP TABLE [dbo].[ObligationTypeExtension]
GO
/****** Object:  Table [dbo].[NetMonthEarningsExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NetMonthEarningsExtension]') AND type in (N'U'))
DROP TABLE [dbo].[NetMonthEarningsExtension]
GO
/****** Object:  Table [dbo].[MaritalStatusExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaritalStatusExtension]') AND type in (N'U'))
DROP TABLE [dbo].[MaritalStatusExtension]
GO
/****** Object:  Table [dbo].[IncomeMainTypesAMLExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IncomeMainTypesAMLExtension]') AND type in (N'U'))
DROP TABLE [dbo].[IncomeMainTypesAMLExtension]
GO
/****** Object:  Table [dbo].[IdentificationDocumentTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IdentificationDocumentTypeExtension]') AND type in (N'U'))
DROP TABLE [dbo].[IdentificationDocumentTypeExtension]
GO
/****** Object:  Table [dbo].[ChannelExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChannelExtension]') AND type in (N'U'))
DROP TABLE [dbo].[ChannelExtension]
GO
/****** Object:  Table [dbo].[EducationLevelExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EducationLevelExtension]') AND type in (N'U'))
DROP TABLE [dbo].[EducationLevelExtension]
GO
/****** Object:  Table [dbo].[EA_CIS_EACODEMAIN]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EA_CIS_EACODEMAIN]') AND type in (N'U'))
DROP TABLE [dbo].[EA_CIS_EACODEMAIN]
GO
/****** Object:  Table [dbo].[DocumentTypes]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTypes]') AND type in (N'U'))
DROP TABLE [dbo].[DocumentTypes]
GO
/****** Object:  Table [dbo].[DocumentTemplateVersion]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTemplateVersion]') AND type in (N'U'))
DROP TABLE [dbo].[DocumentTemplateVersion]
GO
/****** Object:  Table [dbo].[DocumentTemplateVariant]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTemplateVariant]') AND type in (N'U'))
DROP TABLE [dbo].[DocumentTemplateVariant]
GO
/****** Object:  Table [dbo].[DocumentOnSAType]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentOnSAType]') AND type in (N'U'))
DROP TABLE [dbo].[DocumentOnSAType]
GO
/****** Object:  Table [dbo].[ContactTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactTypeExtension]') AND type in (N'U'))
DROP TABLE [dbo].[ContactTypeExtension]
GO
/****** Object:  Table [dbo].[ContactTypeExtension]    Script Date: 14.03.2023 10:54:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactTypeExtension](
	[ContactTypeId] [int] NOT NULL,
	[MpDigiApiCode] [varchar](50) NULL,
 CONSTRAINT [PK_ContactTypeExtension] PRIMARY KEY CLUSTERED 
(
	[ContactTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentOnSAType]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentOnSAType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[SalesArrangementTypeId] [int] NULL,
	[FormTypeId] [int] NOT NULL,
 CONSTRAINT [PK_DocumentOnSAType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentTemplateVariant]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentTemplateVariant](
	[Id] [int] NOT NULL,
	[DocumentTemplateVersionId] [int] NOT NULL,
	[DocumentVariant] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_DocumentTemplateVariant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentTemplateVersion]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentTemplateVersion](
	[Id] [int] NOT NULL,
	[DocumentTypeId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](50) NOT NULL,
	[FormTypeId] [int] NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_DocumentTemplateVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentTypes]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentTypes](
	[Id] [int] NOT NULL,
	[ShortName] [varchar](20) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[FileName] [varchar](200) NOT NULL,
	[SalesArrangementTypeId] [int] NULL,
	[EACodeMainId] [int] NULL,
	[IsFormIdRequested] [bit] NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EA_CIS_EACODEMAIN]    Script Date: 14.03.2023 10:54:31 ******/
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
	[viditelnost_pro_vlozeni_noby] [int] NULL,
	[viditelnost_ps_kb_prodejni_sit_kb] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[kod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationLevelExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[ChannelExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChannelExtension](
	[ChannelId] [int] NOT NULL,
	[RdmCbChannelCode] [varchar](50) NULL,
 CONSTRAINT [PK_ChannelExtension] PRIMARY KEY CLUSTERED 
(
	[ChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentificationDocumentTypeExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentificationDocumentTypeExtension](
	[IdentificationDocumentTypeId] [int] NOT NULL,
	[MpDigiApiCode] [varchar](20) NULL,
 CONSTRAINT [PK_IdentificationDocumentTypeExtension] PRIMARY KEY CLUSTERED 
(
	[IdentificationDocumentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomeMainTypesAMLExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomeMainTypesAMLExtension](
	[Id] [int] NOT NULL,
	[RdmCode] [varchar](50) NULL,
 CONSTRAINT [PK_IncomeMainTypesAMLExtension] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaritalStatusExtension]    Script Date: 14.03.2023 10:54:31 ******/
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
/****** Object:  Table [dbo].[NetMonthEarningsExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetMonthEarningsExtension](
	[NetMonthEarningId] [int] NOT NULL,
	[RdmCode] [varchar](50) NULL,
 CONSTRAINT [PK_NetMonthEarningsExtension] PRIMARY KEY CLUSTERED 
(
	[NetMonthEarningId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObligationTypeExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObligationTypeExtension](
	[ObligationTypeId] [int] NOT NULL,
	[ObligationProperty] [varchar](50) NULL,
 CONSTRAINT [PK_ObligationTypeExtension] PRIMARY KEY CLUSTERED 
(
	[ObligationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTypeExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTypeExtension](
	[ProductTypeId] [int] NOT NULL,
	[MpHomeApiLoanType] [varchar](50) NULL,
	[KonsDbLoanType] [tinyint] NOT NULL,
 CONSTRAINT [PK_ProductTypeExtension] PRIMARY KEY CLUSTERED 
(
	[ProductTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfessionCategoryExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfessionCategoryExtension](
	[ProfessionCategoryId] [int] NOT NULL,
	[ProfessionTypeIds] [nvarchar](100) NULL,
	[IncomeMainTypeAMLIds] [nvarchar](100) NULL,
 CONSTRAINT [PK_ProfessionCategoryExtension] PRIMARY KEY CLUSTERED 
(
	[ProfessionCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RelationshipCustomerProductTypeExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelationshipCustomerProductTypeExtension](
	[RelationshipCustomerProductTypeId] [int] NOT NULL,
	[RdmCode] [varchar](50) NULL,
	[MpDigiApiCode] [varchar](50) NULL,
	[NameNoby] [varchar](50) NULL,
 CONSTRAINT [PK_RelationshipCustomerProductTypeExtension] PRIMARY KEY CLUSTERED 
(
	[RelationshipCustomerProductTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RiskApplicationType]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[SalesArrangementType]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesArrangementType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[ProductTypeId] [int] NULL,
	[SalesArrangementCategory] [int] NOT NULL,
 CONSTRAINT [PK_SalesArrangementType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmsNotificationType]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmsNotificationType](
	[Id] [int] NOT NULL,
	[Code] [varchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SmsText] [nvarchar](max) NULL,
	[McsCode] [varchar](100) NULL,
	[IsAuditLogEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_SmsNotificationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkflowTaskStateExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[WorkflowTaskTypeExtension]    Script Date: 14.03.2023 10:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
