DROP TABLE IF EXISTS [dbo].[ProfessionCategory]
GO
DROP TABLE IF EXISTS dbo.SigningMethodsForNaturalPerson
GO
DROP TABLE IF EXISTS [dbo].[ProfessionCategoryExtension]
GO
DROP TABLE IF EXISTS [dbo].[RdmCodebook]
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
DELETE FROM SqlQuery WHERE SqlQueryId='SigningMethodsForNaturalPerson1';
UPDATE SqlQuery SET SqlQueryId='SigningMethodsForNaturalPerson' WHERE SqlQueryId='SigningMethodsForNaturalPerson2';
GO

CREATE TABLE [dbo].[RdmCodebook](
	[RdmCodebookName] [varchar](50) NOT NULL,
	[EntryCode] [varchar](50) NULL,
	[EntryIsValid] [bit] NOT NULL,
	[EntryProperties] [nvarchar](max) NULL,
	[SortOrder] [int] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RdmCodebook] ADD  DEFAULT ((0)) FOR [SortOrder]
GO

INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'0', 1, N'[{"Key":"Name","Value":"odm\u00EDtl sd\u011Blit"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'1', 1, N'[{"Key":"Name","Value":"st\u00E1tn\u00ED zam\u011Bstnanec"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'2', 1, N'[{"Key":"Name","Value":"zam\u011Bstnanec subjektu se st\u00E1tn\u00ED majetkovou \u00FA\u010Dast\u00ED"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'3', 1, N'[{"Key":"Name","Value":"zam\u011Bstnanec subjektu se zahrani\u010Dn\u00EDm vlastn\u00EDkem"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'4', 1, N'[{"Key":"Name","Value":"podnikatel"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'5', 1, N'[{"Key":"Name","Value":"zam\u011Bstnanec soukrom\u00E9 spole\u010Dnosti"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'6', 1, N'[{"Key":"Name","Value":"bez zam\u011Bstn\u00E1n\u00ED"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'7', 1, N'[{"Key":"Name","Value":"nezji\u0161t\u011Bno"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_CmEpProfessionCategory', N'8', 1, N'[{"Key":"Name","Value":"kombinace profes\u00ED"}]', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'APOC', 1, N'[{"Key":"Name","Value":"Automatizovan\u00FD Podpis Osobn\u00EDm Certifik\u00E1tem"},{"Key":"Description","Value":null}]', 3)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'PAAT', 1, N'[{"Key":"Name","Value":"KB kl\u00ED\u010D"},{"Key":"Description","Value":null}]', 1)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'SAML', 1, N'[{"Key":"Name","Value":"SAML"},{"Key":"Description","Value":null}]', 1)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'INT_CERT_FILE', 1, N'[{"Key":"Name","Value":"Intern\u00ED certifik\u00E1t v souboru"},{"Key":"Description","Value":null}]', 2)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'OFFERED', 1, N'[{"Key":"Name","Value":"Delegovan\u00E1 metoda podpisu"},{"Key":"Description","Value":"deprecated"}]', 4)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'NDB', 1, N'[{"Key":"Name","Value":"KB\u002B"},{"Key":"Description","Value":null}]', 1)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'PHYSICAL', 1, N'[{"Key":"Name","Value":"Ru\u010Dn\u00ED podpis"},{"Key":"Description","Value":"Fyzick\u00FD/ru\u010Dn\u00ED podpis dokumentu."}]', 1)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"14"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"20"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"22"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"6","Value":"0"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"20"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"8"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"17"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"21"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"23"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"25"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"6","Value":"28"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"8"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"14"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"15"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"20"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"22"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"23"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"26"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"23"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"25"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"17"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"17"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"25"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"7"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"7"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"23"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"24"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"24"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"20"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"17"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"21"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"24"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"8"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"22"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"25"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"7","Value":"0"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"25"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"21"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"22"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"14"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"17"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"20"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"7"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"14"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"20"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"24"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"26"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"6","Value":"27"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"8","Value":"7"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"17"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"21"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"8"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"0","Value":"0"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"14"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"26"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"15"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"1","Value":"26"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"21"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"8"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"23"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"15"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"24"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"25"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"7"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"22"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"14"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"15"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"15"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"22"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"2","Value":"24"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"3","Value":"26"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"21"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"8"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"23"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"15"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"5","Value":"26"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'MAP_CB_CmEpProfessionCategory_CB_CmEpProfession', NULL, 1, N'{"Key":"4","Value":"7"}', 0)
GO
INSERT [dbo].[RdmCodebook] ([RdmCodebookName], [EntryCode], [EntryIsValid], [EntryProperties], [SortOrder]) VALUES (N'CB_StandardMethodOfArrAcceptanceByNPType', N'DELEGATE', 1, N'[{"Key":"Name","Value":"P\u0159\u00EDm\u00E9 bankovnictv\u00ED"},{"Key":"Description","Value":"P\u0159\u00EDm\u00E9 bankovnictv\u00ED - Delegovan\u00E1 metoda podpisu"}]', 1)
GO
