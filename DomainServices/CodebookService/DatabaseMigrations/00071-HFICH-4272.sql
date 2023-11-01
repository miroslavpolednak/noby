DROP TABLE IF EXISTS [dbo].[ProfessionCategory]
GO
DROP TABLE IF EXISTS [dbo].[ProfessionCategoryExtension]
GO

DROP TABLE IF EXISTS [dbo].[RdmCodebook]
GO

CREATE TABLE [dbo].[RdmCodebook](
	[RdmCodebookName] [varchar](50) NOT NULL,
	[EntryCode] [varchar](50) NULL,
	[EntryIsValid] [bit] NOT NULL,
	[EntryProperties] [nvarchar](max) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ProfessionCategoryExtension](
	[ProfessionCategoryId] [int] NOT NULL,
	[IncomeMainTypeAMLIds] [varchar](250) NULL,
 CONSTRAINT [PK_ProfessionCategoryExtension] PRIMARY KEY CLUSTERED 
(
	[ProfessionCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [IncomeMainTypeAMLIds]) VALUES 
	(1,'1,6'),
	(2,'1,6'),
	(3,'1,6'),
	(4,'2,6'),
	(5,'1,6'),
	(6,'3,4,5'),
	(8,'1,2,3,6');
GO

UPDATE SqlQuery SET SqlQueryText='SELECT KOD ''Id'',  CAST(ID_CM as int) ''RdmCode'' FROM [SBR].[HTEDM_CIS_POVOLANI]', DatabaseProvider=2 WHERE SqlQueryId='ProfessionCategories1'
GO
