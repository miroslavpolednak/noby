CREATE TABLE [dbo].[AcvAttachmentCategory](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](150) NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (1, N'V', N'List vlastnictví z KN')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (2, N'MAPA', N'Snímek z KM')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (3, N'NAB_TITUL', N'Nabývací tituly')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (4, N'NAJEM_SML', N'Pachtovní nájemní smlouvy')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (5, N'VEC_BREM', N'Smlouva o věc. břemeni')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (6, N'PLOMBA', N'Vysvětlení plomby')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (7, N'PRED_ODHAD', N'Předešlý odhad nebo znalecký posudek')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (8, N'STAV_POVOL', N'Stavební povolení')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (9, N'PROJ_DOK', N'Projektová dokumentace')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (10, N'SML_DILO', N'Smlouva o dílo')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (11, N'ROZPOCET', N'Rozpočet')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (12, N'GEOM_PLAN', N'Geometrický plán')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (13, N'KOLAUD_SOUHL', N'Kolaudační rozhodnutí')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (14, N'PROHL_VLAST', N'Prohlášení vlastníka / Smlouva o výstavbě')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (15, N'JINE', N'Jiné')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (16, N'FOTO', N'Fotodokumentace')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (17, N'DOKL_POPL', N'Doklad o úhradě poplatku')
GO
INSERT [dbo].[AcvAttachmentCategory] ([Id], [Code], [Name]) VALUES (18, N'PRAVO_STAV', N'Smlouva o právu stavby')
GO
INSERT INTO SqlQuery (SqlQueryId, SqlQueryText, DatabaseProvider) VALUES ('AcvAttachmentCategories', 'SELECT * FROM dbo.AcvAttachmentCategory', 4);
GO