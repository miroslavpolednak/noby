DROP TABLE IF EXISTS dbo.[MigrationHistory]
GO

DROP SEQUENCE IF EXISTS dbo.GenerateDocumentIdSequence;
GO
/****** Object:  Table [dbo].[DocumentInterface]    Script Date: 14.03.2023 11:05:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentInterface]') AND type in (N'U'))
DROP TABLE [dbo].[DocumentInterface]
GO
/****** Object:  Table [dbo].[DocumentInterface]    Script Date: 14.03.2023 11:05:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentInterface](
	[DOCUMENT_ID] [varchar](30) NOT NULL,
	[DOCUMENT_DATA] [varbinary](max) NOT NULL,
	[FILENAME] [nvarchar](64) NOT NULL,
	[FILENAME_SUFFIX] [varchar](10) NOT NULL,
	[DESCRIPTION] [nvarchar](254) NULL,
	[CASEID] [bigint] NOT NULL,
	[DATUM_PRIJETI] [datetime] NOT NULL,
	[AUTHOR_USER_LOGIN] [varchar](10) NOT NULL,
	[CONTRACT_NUMBER] [varchar](13) NULL,
	[STATUS] [int] NOT NULL,
	[STATUS_ERROR_TEXT] [varchar](1000) NULL,
	[FORMID] [varchar](15) NULL,
	[EA_CODE_MAIN_ID] [int] NOT NULL,
	[DOCUMENT_DIRECTION] [varchar](1) NOT NULL,
	[FOLDER_DOCUMENT] [varchar](1) NOT NULL,
	[FOLDER_DOCUMENT_ID] [varchar](30) NULL,
	[KDV] [tinyint] NOT NULL,
	[SEND_DOCUMENT_ONLY] [tinyint] NOT NULL,
 CONSTRAINT [PK_DocumentInterface] PRIMARY KEY CLUSTERED 
(
	[DOCUMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[DocumentInterface] ADD  DEFAULT ((100)) FOR [STATUS]
GO
ALTER TABLE [dbo].[DocumentInterface] ADD  DEFAULT ('E') FOR [DOCUMENT_DIRECTION]
GO
ALTER TABLE [dbo].[DocumentInterface] ADD  DEFAULT ('N') FOR [FOLDER_DOCUMENT]
GO
ALTER TABLE [dbo].[DocumentInterface] ADD  DEFAULT (CONVERT([tinyint],(0))) FOR [SEND_DOCUMENT_ONLY]
GO
	CREATE SEQUENCE dbo.GenerateDocumentIdSequence  
	AS bigint
    START WITH 290  
    INCREMENT BY 1;  
GO