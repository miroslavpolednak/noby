USE [NOBY]
GO

/****** Object:  Table [dbo].[FormInstanceInterface]    Script Date: 20.06.2022 0:28:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FormInstanceInterface](
	[DOKUMENT_ID] [varchar](30) NULL,
	[TYP_FORMULARE] [varchar](7) NULL,
	[CISLO_SMLOUVY] [varchar](13) NULL,
	[STATUS] [tinyint] NULL,
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


