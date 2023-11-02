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

INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'0', 1, N'[{"Key":"Name","Value":"odm\u00EDtl sd\u011Blit"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'1', 1, N'[{"Key":"Name","Value":"st\u00E1tn\u00ED zam\u011Bstnanec"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'2', 1, N'[{"Key":"Name","Value":"zam\u011Bstnanec subjektu se st\u00E1tn\u00ED majetkovou \u00FA\u010Dast\u00ED"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'3', 1, N'[{"Key":"Name","Value":"zam\u011Bstnanec subjektu se zahrani\u010Dn\u00EDm vlastn\u00EDkem"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'4', 1, N'[{"Key":"Name","Value":"podnikatel"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'5', 1, N'[{"Key":"Name","Value":"zam\u011Bstnanec soukrom\u00E9 spole\u010Dnosti"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'6', 1, N'[{"Key":"Name","Value":"bez zam\u011Bstn\u00E1n\u00ED"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'7', 1, N'[{"Key":"Name","Value":"nezji\u0161t\u011Bno"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'CB_CmEpProfessionCategory', N'8', 1, N'[{"Key":"Name","Value":"kombinace profes\u00ED"}]')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"14"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"20"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"22"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"6","Value":"0"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"20"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"8"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"17"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"21"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"23"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"25"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"6","Value":"28"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"8"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"14"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"15"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"20"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"22"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"23"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"26"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"23"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"25"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"17"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"17"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"25"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"7"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"7"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"23"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"24"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"24"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"20"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"17"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"21"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"24"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"8"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"22"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"25"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"7","Value":"0"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"25"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"21"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"22"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"14"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"17"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"20"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"7"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"14"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"20"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"24"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"26"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"6","Value":"27"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"7"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"17"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"21"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"8"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"0","Value":"0"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"14"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"26"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"15"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"26"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"21"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"8"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"23"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"15"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"24"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"25"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"7"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"22"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"14"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"15"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"15"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"22"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"24"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"26"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"21"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"8"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"23"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"15"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"26"}')
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"7"}')
GO
