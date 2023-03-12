USE [DataAggregatorService]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormSpecialDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormSpecialDataField] DROP CONSTRAINT IF EXISTS [FK_EasFormSpecialDataField_EasRequestType_EasRequestTypeId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormSpecialDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormSpecialDataField] DROP CONSTRAINT IF EXISTS [FK_EasFormSpecialDataField_EasFormType_EasFormTypeId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormSpecialDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormSpecialDataField] DROP CONSTRAINT IF EXISTS [FK_EasFormSpecialDataField_DataService_DataServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_EasFormDynamicInputParameter_InputParameter_InputParameterId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_EasFormDynamicInputParameter_EasRequestType_EasRequestTypeId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_EasFormDynamicInputParameter_DataField_SourceDataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDataField] DROP CONSTRAINT IF EXISTS [FK_EasFormDataField_EasRequestType_EasRequestTypeId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDataField] DROP CONSTRAINT IF EXISTS [FK_EasFormDataField_EasFormType_EasFormTypeId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EasFormDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[EasFormDataField] DROP CONSTRAINT IF EXISTS [FK_EasFormDataField_DataField_DataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DynamicStringFormatCondition]') AND type in (N'U'))
ALTER TABLE [dbo].[DynamicStringFormatCondition] DROP CONSTRAINT IF EXISTS [FK_DynamicStringFormatCondition_DynamicStringFormat_DynamicStringFormatId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DynamicStringFormatCondition]') AND type in (N'U'))
ALTER TABLE [dbo].[DynamicStringFormatCondition] DROP CONSTRAINT IF EXISTS [FK_DynamicStringFormatCondition_DataField_DataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DynamicStringFormat]') AND type in (N'U'))
ALTER TABLE [dbo].[DynamicStringFormat] DROP CONSTRAINT IF EXISTS [FK_DynamicStringFormat_DocumentDataField_DocumentDataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTableColumn]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentTableColumn] DROP CONSTRAINT IF EXISTS [FK_DocumentTableColumn_DocumentTable_DocumentTableId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTable]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentTable] DROP CONSTRAINT IF EXISTS [FK_DocumentTable_Document_DocumentId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTable]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentTable] DROP CONSTRAINT IF EXISTS [FK_DocumentTable_DataField_DataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentSpecialDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentSpecialDataField] DROP CONSTRAINT IF EXISTS [FK_DocumentSpecialDataField_Document_DocumentId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentSpecialDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentSpecialDataField] DROP CONSTRAINT IF EXISTS [FK_DocumentSpecialDataField_DataService_DataServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_DocumentDynamicInputParameter_InputParameter_InputParameterId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_DocumentDynamicInputParameter_Document_DocumentId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_DocumentDynamicInputParameter_DataService_TargetDataServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentDynamicInputParameter]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentDynamicInputParameter] DROP CONSTRAINT IF EXISTS [FK_DocumentDynamicInputParameter_DataField_SourceDataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentDataField] DROP CONSTRAINT IF EXISTS [FK_DocumentDataField_Document_DocumentId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentDataField]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentDataField] DROP CONSTRAINT IF EXISTS [FK_DocumentDataField_DataField_DataFieldId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataField]') AND type in (N'U'))
ALTER TABLE [dbo].[DataField] DROP CONSTRAINT IF EXISTS [FK_DataField_DataService_DataServiceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTableColumn]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentTableColumn] DROP CONSTRAINT IF EXISTS [DF__DocumentT__Heade__4222D4EF]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentTableColumn]') AND type in (N'U'))
ALTER TABLE [dbo].[DocumentTableColumn] DROP CONSTRAINT IF EXISTS [DF__DocumentT__Order__412EB0B6]
GO
/****** Object:  Index [IX_EasFormSpecialDataField_EasFormTypeId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormSpecialDataField_EasFormTypeId] ON [dbo].[EasFormSpecialDataField]
GO
/****** Object:  Index [IX_EasFormSpecialDataField_DataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormSpecialDataField_DataServiceId] ON [dbo].[EasFormSpecialDataField]
GO
/****** Object:  Index [IX_EasFormDynamicInputParameter_TargetDataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormDynamicInputParameter_TargetDataServiceId] ON [dbo].[EasFormDynamicInputParameter]
GO
/****** Object:  Index [IX_EasFormDynamicInputParameter_SourceDataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormDynamicInputParameter_SourceDataFieldId] ON [dbo].[EasFormDynamicInputParameter]
GO
/****** Object:  Index [IX_EasFormDynamicInputParameter_InputParameterId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormDynamicInputParameter_InputParameterId] ON [dbo].[EasFormDynamicInputParameter]
GO
/****** Object:  Index [IX_EasFormDataField_EasRequestTypeId_EasFormTypeId_JsonPropertyName]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormDataField_EasRequestTypeId_EasFormTypeId_JsonPropertyName] ON [dbo].[EasFormDataField]
GO
/****** Object:  Index [IX_EasFormDataField_EasFormTypeId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormDataField_EasFormTypeId] ON [dbo].[EasFormDataField]
GO
/****** Object:  Index [IX_EasFormDataField_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_EasFormDataField_DataFieldId] ON [dbo].[EasFormDataField]
GO
/****** Object:  Index [IX_DynamicStringFormatCondition_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DynamicStringFormatCondition_DataFieldId] ON [dbo].[DynamicStringFormatCondition]
GO
/****** Object:  Index [IX_DynamicStringFormat_DocumentDataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DynamicStringFormat_DocumentDataFieldId] ON [dbo].[DynamicStringFormat]
GO
/****** Object:  Index [IX_DocumentTable_DocumentId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentTable_DocumentId] ON [dbo].[DocumentTable]
GO
/****** Object:  Index [IX_DocumentTable_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentTable_DataFieldId] ON [dbo].[DocumentTable]
GO
/****** Object:  Index [IX_DocumentSpecialDataField_DataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentSpecialDataField_DataServiceId] ON [dbo].[DocumentSpecialDataField]
GO
/****** Object:  Index [IX_DocumentDynamicInputParameter_TargetDataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentDynamicInputParameter_TargetDataServiceId] ON [dbo].[DocumentDynamicInputParameter]
GO
/****** Object:  Index [IX_DocumentDynamicInputParameter_SourceDataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentDynamicInputParameter_SourceDataFieldId] ON [dbo].[DocumentDynamicInputParameter]
GO
/****** Object:  Index [IX_DocumentDynamicInputParameter_InputParameterId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentDynamicInputParameter_InputParameterId] ON [dbo].[DocumentDynamicInputParameter]
GO
/****** Object:  Index [IX_DocumentDataField_DocumentId_DocumentVersion_AcroFieldName]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentDataField_DocumentId_DocumentVersion_AcroFieldName] ON [dbo].[DocumentDataField]
GO
/****** Object:  Index [IX_DocumentDataField_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DocumentDataField_DataFieldId] ON [dbo].[DocumentDataField]
GO
/****** Object:  Index [IX_DataField_FieldPath]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DataField_FieldPath] ON [dbo].[DataField]
GO
/****** Object:  Index [IX_DataField_DataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP INDEX IF EXISTS [IX_DataField_DataServiceId] ON [dbo].[DataField]
GO
/****** Object:  Table [dbo].[InputParameter]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[InputParameter]
GO
/****** Object:  Table [dbo].[EasRequestType]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[EasRequestType]
GO
/****** Object:  Table [dbo].[EasFormType]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[EasFormType]
GO
/****** Object:  Table [dbo].[EasFormSpecialDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[EasFormSpecialDataField]
GO
/****** Object:  Table [dbo].[EasFormDynamicInputParameter]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[EasFormDynamicInputParameter]
GO
/****** Object:  Table [dbo].[EasFormDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[EasFormDataField]
GO
/****** Object:  Table [dbo].[DynamicStringFormatCondition]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DynamicStringFormatCondition]
GO
/****** Object:  Table [dbo].[DynamicStringFormat]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DynamicStringFormat]
GO
/****** Object:  Table [dbo].[DocumentTableColumn]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DocumentTableColumn]
GO
/****** Object:  Table [dbo].[DocumentTable]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DocumentTable]
GO
/****** Object:  Table [dbo].[DocumentSpecialDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DocumentSpecialDataField]
GO
/****** Object:  Table [dbo].[DocumentDynamicInputParameter]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DocumentDynamicInputParameter]
GO
/****** Object:  Table [dbo].[DocumentDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DocumentDataField]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[Document]
GO
/****** Object:  Table [dbo].[DataService]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DataService]
GO
/****** Object:  Table [dbo].[DataField]    Script Date: 3/12/2023 8:26:56 PM ******/
DROP TABLE IF EXISTS [dbo].[DataField]
GO
/****** Object:  Table [dbo].[DataField]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataField](
	[DataFieldId] [int] IDENTITY(1,1) NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
	[DefaultStringFormat] [nvarchar](50) NULL,
 CONSTRAINT [PK_DataField] PRIMARY KEY CLUSTERED 
(
	[DataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataService]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataService](
	[DataServiceId] [int] NOT NULL,
	[DataServiceName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DataService] PRIMARY KEY CLUSTERED 
(
	[DataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[DocumentId] [int] NOT NULL,
	[DocumentName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentDataField](
	[DocumentDataFieldId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[DataFieldId] [int] NOT NULL,
	[AcroFieldName] [nvarchar](100) NOT NULL,
	[StringFormat] [nvarchar](500) NULL,
	[DefaultTextIfNull] [nvarchar](500) NULL,
 CONSTRAINT [PK_DocumentDataField] PRIMARY KEY CLUSTERED 
(
	[DocumentDataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentDynamicInputParameter]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentDynamicInputParameter](
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[InputParameterId] [int] NOT NULL,
	[TargetDataServiceId] [int] NOT NULL,
	[SourceDataFieldId] [int] NOT NULL,
 CONSTRAINT [PK_DocumentDynamicInputParameter] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC,
	[DocumentVersion] ASC,
	[InputParameterId] ASC,
	[TargetDataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentSpecialDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentSpecialDataField](
	[DocumentId] [int] NOT NULL,
	[AcroFieldName] [nvarchar](100) NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
	[StringFormat] [nvarchar](50) NULL,
 CONSTRAINT [PK_DocumentSpecialDataField] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC,
	[AcroFieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentTable]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentTable](
	[DocumentTableId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](5) NOT NULL,
	[DataFieldId] [int] NOT NULL,
	[AcroFieldPlaceholderName] [nvarchar](100) NOT NULL,
	[ConcludingParagraph] [nvarchar](max) NULL,
 CONSTRAINT [PK_DocumentTable] PRIMARY KEY CLUSTERED 
(
	[DocumentTableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentTableColumn]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentTableColumn](
	[DocumentTableId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
	[WidthPercentage] [real] NOT NULL,
	[Order] [int] NOT NULL,
	[StringFormat] [nvarchar](500) NULL,
	[Header] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_DocumentTableColumn] PRIMARY KEY CLUSTERED 
(
	[DocumentTableId] ASC,
	[FieldPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DynamicStringFormat]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DynamicStringFormat](
	[DynamicStringFormatId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataFieldId] [int] NOT NULL,
	[Format] [nvarchar](max) NOT NULL,
	[Priority] [int] NOT NULL,
 CONSTRAINT [PK_DynamicStringFormat] PRIMARY KEY CLUSTERED 
(
	[DynamicStringFormatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DynamicStringFormatCondition]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DynamicStringFormatCondition](
	[DynamicStringFormatId] [int] NOT NULL,
	[EqualToValue] [nvarchar](100) NULL,
	[DataFieldId] [int] NOT NULL,
 CONSTRAINT [PK_DynamicStringFormatCondition] PRIMARY KEY CLUSTERED 
(
	[DynamicStringFormatId] ASC,
	[DataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EasFormDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EasFormDataField](
	[EasFormDataFieldId] [int] IDENTITY(1,1) NOT NULL,
	[EasRequestTypeId] [int] NOT NULL,
	[DataFieldId] [int] NOT NULL,
	[EasFormTypeId] [int] NOT NULL,
	[JsonPropertyName] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_EasFormDataField] PRIMARY KEY CLUSTERED 
(
	[EasFormDataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EasFormDynamicInputParameter]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EasFormDynamicInputParameter](
	[EasRequestTypeId] [int] NOT NULL,
	[InputParameterId] [int] NOT NULL,
	[TargetDataServiceId] [int] NOT NULL,
	[SourceDataFieldId] [int] NOT NULL,
 CONSTRAINT [PK_EasFormDynamicInputParameter] PRIMARY KEY CLUSTERED 
(
	[EasRequestTypeId] ASC,
	[InputParameterId] ASC,
	[TargetDataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EasFormSpecialDataField]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EasFormSpecialDataField](
	[EasRequestTypeId] [int] NOT NULL,
	[JsonPropertyName] [nvarchar](250) NOT NULL,
	[DataServiceId] [int] NOT NULL,
	[EasFormTypeId] [int] NOT NULL,
	[FieldPath] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_EasFormSpecialDataField] PRIMARY KEY CLUSTERED 
(
	[EasRequestTypeId] ASC,
	[JsonPropertyName] ASC,
	[EasFormTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EasFormType]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EasFormType](
	[EasFormTypeId] [int] NOT NULL,
	[EasFormTypeName] [nvarchar](50) NOT NULL,
	[Version] [int] NOT NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_EasFormType] PRIMARY KEY CLUSTERED 
(
	[EasFormTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EasRequestType]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EasRequestType](
	[EasRequestTypeId] [int] NOT NULL,
	[EasRequestTypeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EasRequestType] PRIMARY KEY CLUSTERED 
(
	[EasRequestTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InputParameter]    Script Date: 3/12/2023 8:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InputParameter](
	[InputParameterId] [int] NOT NULL,
	[InputParameterName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_InputParameter] PRIMARY KEY CLUSTERED 
(
	[InputParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_DataField_DataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DataField_DataServiceId] ON [dbo].[DataField]
(
	[DataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DataField_FieldPath]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_DataField_FieldPath] ON [dbo].[DataField]
(
	[FieldPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentDataField_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentDataField_DataFieldId] ON [dbo].[DocumentDataField]
(
	[DataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DocumentDataField_DocumentId_DocumentVersion_AcroFieldName]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentDataField_DocumentId_DocumentVersion_AcroFieldName] ON [dbo].[DocumentDataField]
(
	[DocumentId] ASC,
	[DocumentVersion] ASC,
	[AcroFieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentDynamicInputParameter_InputParameterId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentDynamicInputParameter_InputParameterId] ON [dbo].[DocumentDynamicInputParameter]
(
	[InputParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentDynamicInputParameter_SourceDataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentDynamicInputParameter_SourceDataFieldId] ON [dbo].[DocumentDynamicInputParameter]
(
	[SourceDataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentDynamicInputParameter_TargetDataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentDynamicInputParameter_TargetDataServiceId] ON [dbo].[DocumentDynamicInputParameter]
(
	[TargetDataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentSpecialDataField_DataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentSpecialDataField_DataServiceId] ON [dbo].[DocumentSpecialDataField]
(
	[DataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentTable_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentTable_DataFieldId] ON [dbo].[DocumentTable]
(
	[DataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DocumentTable_DocumentId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DocumentTable_DocumentId] ON [dbo].[DocumentTable]
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DynamicStringFormat_DocumentDataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DynamicStringFormat_DocumentDataFieldId] ON [dbo].[DynamicStringFormat]
(
	[DocumentDataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DynamicStringFormatCondition_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_DynamicStringFormatCondition_DataFieldId] ON [dbo].[DynamicStringFormatCondition]
(
	[DataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormDataField_DataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormDataField_DataFieldId] ON [dbo].[EasFormDataField]
(
	[DataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormDataField_EasFormTypeId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormDataField_EasFormTypeId] ON [dbo].[EasFormDataField]
(
	[EasFormTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_EasFormDataField_EasRequestTypeId_EasFormTypeId_JsonPropertyName]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_EasFormDataField_EasRequestTypeId_EasFormTypeId_JsonPropertyName] ON [dbo].[EasFormDataField]
(
	[EasRequestTypeId] ASC,
	[EasFormTypeId] ASC,
	[JsonPropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormDynamicInputParameter_InputParameterId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormDynamicInputParameter_InputParameterId] ON [dbo].[EasFormDynamicInputParameter]
(
	[InputParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormDynamicInputParameter_SourceDataFieldId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormDynamicInputParameter_SourceDataFieldId] ON [dbo].[EasFormDynamicInputParameter]
(
	[SourceDataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormDynamicInputParameter_TargetDataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormDynamicInputParameter_TargetDataServiceId] ON [dbo].[EasFormDynamicInputParameter]
(
	[TargetDataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormSpecialDataField_DataServiceId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormSpecialDataField_DataServiceId] ON [dbo].[EasFormSpecialDataField]
(
	[DataServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EasFormSpecialDataField_EasFormTypeId]    Script Date: 3/12/2023 8:26:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_EasFormSpecialDataField_EasFormTypeId] ON [dbo].[EasFormSpecialDataField]
(
	[EasFormTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DocumentTableColumn] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[DocumentTableColumn] ADD  DEFAULT (N'') FOR [Header]
GO
ALTER TABLE [dbo].[DataField]  WITH CHECK ADD  CONSTRAINT [FK_DataField_DataService_DataServiceId] FOREIGN KEY([DataServiceId])
REFERENCES [dbo].[DataService] ([DataServiceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DataField] CHECK CONSTRAINT [FK_DataField_DataService_DataServiceId]
GO
ALTER TABLE [dbo].[DocumentDataField]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDataField_DataField_DataFieldId] FOREIGN KEY([DataFieldId])
REFERENCES [dbo].[DataField] ([DataFieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentDataField] CHECK CONSTRAINT [FK_DocumentDataField_DataField_DataFieldId]
GO
ALTER TABLE [dbo].[DocumentDataField]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDataField_Document_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([DocumentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentDataField] CHECK CONSTRAINT [FK_DocumentDataField_Document_DocumentId]
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDynamicInputParameter_DataField_SourceDataFieldId] FOREIGN KEY([SourceDataFieldId])
REFERENCES [dbo].[DataField] ([DataFieldId])
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter] CHECK CONSTRAINT [FK_DocumentDynamicInputParameter_DataField_SourceDataFieldId]
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDynamicInputParameter_DataService_TargetDataServiceId] FOREIGN KEY([TargetDataServiceId])
REFERENCES [dbo].[DataService] ([DataServiceId])
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter] CHECK CONSTRAINT [FK_DocumentDynamicInputParameter_DataService_TargetDataServiceId]
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDynamicInputParameter_Document_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([DocumentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter] CHECK CONSTRAINT [FK_DocumentDynamicInputParameter_Document_DocumentId]
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDynamicInputParameter_InputParameter_InputParameterId] FOREIGN KEY([InputParameterId])
REFERENCES [dbo].[InputParameter] ([InputParameterId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentDynamicInputParameter] CHECK CONSTRAINT [FK_DocumentDynamicInputParameter_InputParameter_InputParameterId]
GO
ALTER TABLE [dbo].[DocumentSpecialDataField]  WITH CHECK ADD  CONSTRAINT [FK_DocumentSpecialDataField_DataService_DataServiceId] FOREIGN KEY([DataServiceId])
REFERENCES [dbo].[DataService] ([DataServiceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentSpecialDataField] CHECK CONSTRAINT [FK_DocumentSpecialDataField_DataService_DataServiceId]
GO
ALTER TABLE [dbo].[DocumentSpecialDataField]  WITH CHECK ADD  CONSTRAINT [FK_DocumentSpecialDataField_Document_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([DocumentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentSpecialDataField] CHECK CONSTRAINT [FK_DocumentSpecialDataField_Document_DocumentId]
GO
ALTER TABLE [dbo].[DocumentTable]  WITH CHECK ADD  CONSTRAINT [FK_DocumentTable_DataField_DataFieldId] FOREIGN KEY([DataFieldId])
REFERENCES [dbo].[DataField] ([DataFieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentTable] CHECK CONSTRAINT [FK_DocumentTable_DataField_DataFieldId]
GO
ALTER TABLE [dbo].[DocumentTable]  WITH CHECK ADD  CONSTRAINT [FK_DocumentTable_Document_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([DocumentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentTable] CHECK CONSTRAINT [FK_DocumentTable_Document_DocumentId]
GO
ALTER TABLE [dbo].[DocumentTableColumn]  WITH CHECK ADD  CONSTRAINT [FK_DocumentTableColumn_DocumentTable_DocumentTableId] FOREIGN KEY([DocumentTableId])
REFERENCES [dbo].[DocumentTable] ([DocumentTableId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentTableColumn] CHECK CONSTRAINT [FK_DocumentTableColumn_DocumentTable_DocumentTableId]
GO
ALTER TABLE [dbo].[DynamicStringFormat]  WITH CHECK ADD  CONSTRAINT [FK_DynamicStringFormat_DocumentDataField_DocumentDataFieldId] FOREIGN KEY([DocumentDataFieldId])
REFERENCES [dbo].[DocumentDataField] ([DocumentDataFieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DynamicStringFormat] CHECK CONSTRAINT [FK_DynamicStringFormat_DocumentDataField_DocumentDataFieldId]
GO
ALTER TABLE [dbo].[DynamicStringFormatCondition]  WITH CHECK ADD  CONSTRAINT [FK_DynamicStringFormatCondition_DataField_DataFieldId] FOREIGN KEY([DataFieldId])
REFERENCES [dbo].[DataField] ([DataFieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DynamicStringFormatCondition] CHECK CONSTRAINT [FK_DynamicStringFormatCondition_DataField_DataFieldId]
GO
ALTER TABLE [dbo].[DynamicStringFormatCondition]  WITH CHECK ADD  CONSTRAINT [FK_DynamicStringFormatCondition_DynamicStringFormat_DynamicStringFormatId] FOREIGN KEY([DynamicStringFormatId])
REFERENCES [dbo].[DynamicStringFormat] ([DynamicStringFormatId])
GO
ALTER TABLE [dbo].[DynamicStringFormatCondition] CHECK CONSTRAINT [FK_DynamicStringFormatCondition_DynamicStringFormat_DynamicStringFormatId]
GO
ALTER TABLE [dbo].[EasFormDataField]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDataField_DataField_DataFieldId] FOREIGN KEY([DataFieldId])
REFERENCES [dbo].[DataField] ([DataFieldId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormDataField] CHECK CONSTRAINT [FK_EasFormDataField_DataField_DataFieldId]
GO
ALTER TABLE [dbo].[EasFormDataField]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDataField_EasFormType_EasFormTypeId] FOREIGN KEY([EasFormTypeId])
REFERENCES [dbo].[EasFormType] ([EasFormTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormDataField] CHECK CONSTRAINT [FK_EasFormDataField_EasFormType_EasFormTypeId]
GO
ALTER TABLE [dbo].[EasFormDataField]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDataField_EasRequestType_EasRequestTypeId] FOREIGN KEY([EasRequestTypeId])
REFERENCES [dbo].[EasRequestType] ([EasRequestTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormDataField] CHECK CONSTRAINT [FK_EasFormDataField_EasRequestType_EasRequestTypeId]
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDynamicInputParameter_DataField_SourceDataFieldId] FOREIGN KEY([SourceDataFieldId])
REFERENCES [dbo].[DataField] ([DataFieldId])
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter] CHECK CONSTRAINT [FK_EasFormDynamicInputParameter_DataField_SourceDataFieldId]
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId] FOREIGN KEY([TargetDataServiceId])
REFERENCES [dbo].[DataService] ([DataServiceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter] CHECK CONSTRAINT [FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId]
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDynamicInputParameter_EasRequestType_EasRequestTypeId] FOREIGN KEY([EasRequestTypeId])
REFERENCES [dbo].[EasRequestType] ([EasRequestTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter] CHECK CONSTRAINT [FK_EasFormDynamicInputParameter_EasRequestType_EasRequestTypeId]
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter]  WITH CHECK ADD  CONSTRAINT [FK_EasFormDynamicInputParameter_InputParameter_InputParameterId] FOREIGN KEY([InputParameterId])
REFERENCES [dbo].[InputParameter] ([InputParameterId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormDynamicInputParameter] CHECK CONSTRAINT [FK_EasFormDynamicInputParameter_InputParameter_InputParameterId]
GO
ALTER TABLE [dbo].[EasFormSpecialDataField]  WITH CHECK ADD  CONSTRAINT [FK_EasFormSpecialDataField_DataService_DataServiceId] FOREIGN KEY([DataServiceId])
REFERENCES [dbo].[DataService] ([DataServiceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormSpecialDataField] CHECK CONSTRAINT [FK_EasFormSpecialDataField_DataService_DataServiceId]
GO
ALTER TABLE [dbo].[EasFormSpecialDataField]  WITH CHECK ADD  CONSTRAINT [FK_EasFormSpecialDataField_EasFormType_EasFormTypeId] FOREIGN KEY([EasFormTypeId])
REFERENCES [dbo].[EasFormType] ([EasFormTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormSpecialDataField] CHECK CONSTRAINT [FK_EasFormSpecialDataField_EasFormType_EasFormTypeId]
GO
ALTER TABLE [dbo].[EasFormSpecialDataField]  WITH CHECK ADD  CONSTRAINT [FK_EasFormSpecialDataField_EasRequestType_EasRequestTypeId] FOREIGN KEY([EasRequestTypeId])
REFERENCES [dbo].[EasRequestType] ([EasRequestTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EasFormSpecialDataField] CHECK CONSTRAINT [FK_EasFormSpecialDataField_EasRequestType_EasRequestTypeId]
GO