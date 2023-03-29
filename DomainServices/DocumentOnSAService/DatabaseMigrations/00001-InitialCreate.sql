/****** Object:  Table [dbo].[GeneratedFormId]    Script Date: 14.03.2023 13:23:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GeneratedFormId]') AND type in (N'U'))
DROP TABLE [dbo].[GeneratedFormId]
GO
/****** Object:  Table [dbo].[DocumentOnSa]    Script Date: 14.03.2023 13:23:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentOnSa]') AND type in (N'U'))
DROP TABLE [dbo].[DocumentOnSa]
GO
/****** Object:  Table [dbo].[DocumentOnSa]    Script Date: 14.03.2023 13:23:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentOnSa](
	[DocumentOnSAId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTypeId] [int] NOT NULL,
	[DocumentTemplateVersionId] [int] NULL,
	[FormId] [nvarchar](15) NOT NULL,
	[EArchivId] [nvarchar](50) NULL,
	[DmsxId] [nvarchar](50) NULL,
	[SalesArrangementId] [int] NOT NULL,
	[HouseholdId] [int] NULL,
	[IsValid] [bit] NOT NULL,
	[IsSigned] [bit] NOT NULL,
	[IsDocumentArchived] [bit] NOT NULL,
	[SignatureMethodCode] [nvarchar](15) NULL,
	[SignatureDateTime] [datetime2](7) NULL,
	[SignatureConfirmedBy] [int] NULL,
	[CreatedUserName] [nvarchar](50) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[IsFinal] [bit] NOT NULL,
 CONSTRAINT [PK_DocumentOnSa] PRIMARY KEY CLUSTERED 
(
	[DocumentOnSAId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneratedFormId]    Script Date: 14.03.2023 13:23:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneratedFormId](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HouseholdId] [int] NULL,
	[Version] [smallint] NOT NULL,
	[IsFormIdFinal] [bit] NOT NULL,
	[TargetSystem] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_GeneratedFormId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DocumentOnSa] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsValid]
GO
ALTER TABLE [dbo].[DocumentOnSa] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsSigned]
GO
ALTER TABLE [dbo].[DocumentOnSa] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDocumentArchived]
GO
ALTER TABLE [dbo].[DocumentOnSa] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsFinal]
GO
ALTER TABLE [dbo].[GeneratedFormId] ADD  DEFAULT (N'N') FOR [TargetSystem]
GO
