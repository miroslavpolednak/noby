DROP TABLE IF EXISTS [dbo].[RealEstateSubtypes];
GO

CREATE TABLE [dbo].[RealEstateSubtypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	ShortName nvarchar(100) NOT NULL,
	RealEstateTypeId int NOT NULL
 CONSTRAINT [PK_RealEstateSubtypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (1, N'RD do 6 místností', N'Rodinný dům do 6 obytných místností ', 1)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (2, N'RD od 7 místností', N'Rodinný dům od 7 obytných místností ', 1)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (3, N'Byt', N'Bytová jednotka vymezená dle zákona (nemá upřesnění typu nemovitosti)', 2)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (4, N'NA', N'NA', 3)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (5, N'Pozemek do 3000 m2', N'Pozemek do 3000 m2 jako jediný předmět zajištění (nemá upřesnění typu nemovitosti)', 4)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (6, N'Chata / chalupa', N'Objekt individuální rekreace (rekreační chata, chalupa) (nemá upřesnění typu nemovitosti)', 5)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (7, N'Obytný dům s nebyt pr.do 20 bytů', N'Obytný dům s obecně komerčně využitelnými nebytovými prostory do 20 bytů ', 6)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (8, N'Dům s byty do 20 jednotek', N'Dům s byty a nebytovými prostory do 20 jednotek dle zákona s obecně komerčně využitelnými nebytovými prostory ', 6)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (9, N'Obytný dům s nebyt. pr. nad  20 bytů', N'Obytný dům s obecně komerčně využitelnými nebytovými prostory nad  20 bytů ', 6)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (10, N'Dům s byty nad 20 jednotek', N'Dům s jednotkami vymezenými dle zákona  nad 20 jednotek s obecně komerčně využitelnými nebytovými prostory ', 6)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (11, N'Obytný dům', N'Obytný dům bez obecně komerčně využitelných nebytových prostor (za obecně komerčně využitelné nebytové prostory nejsou považovány např. kočárkárny, sušárny, využívané vlastníkem či nájemníkem bytu v domě) ', 6)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (12, N'Dům s jednotkami', N'Dům s jednotkami vymezenými  dle zákona  bez obecně komerčně využitelných nebytových prostor (za obecně komerčně využitelné nebytové prostory nejsou považovány např. kočárkárny, sušárny, využívané vlastníkem či nájemníkem bytu v domě) ', 6)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (13, N'Zemědělská půda do 1ha včetně', N'Zemědělská půda pro spolupráci s PGRLF do 1ha včetně', 7)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (14, N'Zemědělská půda od 1 ha do 5 ha včetně', N'Zemědělská půda pro spolupráci s PGRLF od 1 ha do 5 ha včetně', 7)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (15, N'Zemědělská půda od 5 ha do 10 ha včetně', N'Zemědělská půda pro spolupráci s PGRLF od 5 ha do 10 ha včetně', 7)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (16, N'Zemědělská půda nad 10 ha', N'Zemědělská půda pro spolupráci s PGRLF nad 10 ha', 7)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (17, N'Hotel nad 50 lůžek', N'Hotel, penzion, rekreační zařízení do 20 lůžek', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (18, N'Objekt občanské vybavenosti', N'Jednoduchý objekt občanské vybavenosti nebo jednoduchý provozní objekt do 500 m2 zastavěné plochy, s maximálně 2 NP (např. objekt smíšeného zboží, restaurace, apod.)', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (19, N'Administrativní budova do 4 NP', N'Samostatná administrativní budova do 500 m2 zastavěné plochy a max. 4 NP', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (20, N'Provozní objekt  do 500 m2', N'Provozní objekt (výrobní nebo skladový objekt, objekt garážových stání) do 500 m2 zastavěné plochy a max. 4 NP)', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (21, N'Areál do 5. hlavních objektů', N'Výrobní, skladový nebo ostatní areál nejvýše s 5 hlavními objekty o max. 4 NP a celkové zastavěné max. 2 500 m2 a zároveň o celkové maximální ploše areálu 10 000 m2. (Za hlavní objekty se považují takové, které jsou využívány nebo mohou být využívány k provozním účelům, např. výrobě, skladování, obchodu, administrativě, apod.)', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (22, N'Hotel 21 - 50 lůžek', N'Hotel, penzion, rekreační zařízení, 21 - 50 lůžek', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (23, N'Polyfunkční objekt do 4 NP', N'Polyfunkční objekt do 500 m2 zastavěné plochy nejvýše se 4 NP (objekt s kombinací několika funkcí a s více typy nebytových prostorů speciálně upravených pro provoz určitých činností, např. kombinace bydlení, restauračního provozu, pobočky pošty nebo banky)', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (24, N'Administrativní budova nad 4 NP', N'Administrativní budova o více než 4 NP nebo soubor administrativních budov', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (25, N'Polyfunkční objekt nad 4 NP', N'Polyfunkční objekt o více než 4 NP jako jediný předmět zajištění (objekt s kombinací několika funkcí a více typy nebytových prostorů speciálně upravených pro provoz určitých činností, např. kombinace bydlení, restauračního provozu, pobočky pošty nebo banky, fitnesscentra, administrativy, obchodních prostor, garáží, apod.)', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (26, N'Hotel nad 50 lůžek', N'Hotel, penzion, rekreační zařízení nad 50 lůžek', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (27, N'Areál s více než 5 hlavními objekty', N'Výrobní, skladový nebo ostatní areál s více než 5 hlavními objekty a celkové zastavěné ploše nad 2500 m2 nebo o ploše areálu nad  10.000 m2.', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (28, N'Speciální nemovitosti', N'Speciální nemovitosti (nemovitosti s vestavěnou jednoúčelovou technologií např. pivovar, chemická výroba ap.)', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (29, N'Čerpací stanice pohonných hmot', N'Čerpací stanice pohonných hmot', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (30, N'Nezatřiditelný typ nemovitosti', N'Nezatřiditelný typ nemovitosti', 8)
GO
INSERT [dbo].[RealEstateSubtypes] ([Id], [ShortName], [Name], [RealEstateTypeId]) VALUES (31, N'Nebytový prostor', N'Nebytový prostor (jednotka) vymezený podle zákona (nemá upřesnění typu nemovitosti)', 9)
GO
