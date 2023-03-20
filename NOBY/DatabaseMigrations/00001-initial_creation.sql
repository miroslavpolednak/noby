DROP TABLE IF EXISTS dbo.[MigrationHistory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP TABLE IF EXISTS [FormInstanceInterface]
GO
CREATE TABLE [dbo].[FormInstanceInterface](
	[DOKUMENT_ID] [varchar](30) NULL,
	[TYP_FORMULARE] [varchar](7) NULL,
	[CISLO_SMLOUVY] [varchar](13) NULL,
	[STATUS] [smallint] NULL,
	[DRUH_FROMULARE] [char](1) NULL,
	[FORMID] [varchar](15) NULL,
	[CPM] [varchar](10) NULL,
	[ICP] [varchar](10) NULL,
	[CREATED_AT] [datetime] NULL,
	[HESLO_KOD] [varchar](10) NULL,
	[STORNOVANO] [tinyint] NULL,
	[TYP_DAT] [tinyint] NULL,
	[JSON_DATA_CLOB] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

DROP TABLE IF EXISTS [DataProtectionKeys]
GO
CREATE TABLE [dbo].[DataProtectionKeys](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FriendlyName] [nvarchar](max) NULL,
	[Xml] [nvarchar](max) NULL,
 CONSTRAINT [PK_DataProtectionKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
