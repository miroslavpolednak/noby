INSERT [dbo].[ContactTypeExtension] ([ContactTypeId], [MpDigiApiCode]) VALUES (1, N'Mobile')
GO
INSERT [dbo].[ContactTypeExtension] ([ContactTypeId], [MpDigiApiCode]) VALUES (5, N'Email')
GO
INSERT [dbo].[ContactTypeExtension] ([ContactTypeId], [MpDigiApiCode]) VALUES (13, N'Mobile')
GO
INSERT [dbo].[ContactTypeExtension] ([ContactTypeId], [MpDigiApiCode]) VALUES (14, N'Email')
GO
INSERT [dbo].[DocumentOnSAType] ([Id], [Name], [SalesArrangementTypeId], [FormTypeId]) VALUES (1, N'Žádost o poskytnutí úvěru', NULL, 3601001)
GO
INSERT [dbo].[DocumentOnSAType] ([Id], [Name], [SalesArrangementTypeId], [FormTypeId]) VALUES (2, N'Prohlášení účastníka k žádosti o úvěr (spolužadatelská domácnost)', NULL, 3602001)
GO
INSERT [dbo].[DocumentOnSAType] ([Id], [Name], [SalesArrangementTypeId], [FormTypeId]) VALUES (3, N'Prohlášení účastníka k žádosti o úvěr (ručitelská domácnost)', NULL, 3602001)
GO
INSERT [dbo].[DocumentOnSAType] ([Id], [Name], [SalesArrangementTypeId], [FormTypeId]) VALUES (4, N'Žádost o čerpání', 6, 3700001)
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (1, 4, N'A', N'jeden dlužník, zprostředkovatel')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (2, 4, N'B', N'jeden dlužník, bez zprostředkovatele')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (3, 4, N'C', N'dva dlužníci, zprostředkovatel')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (4, 4, N'D', N'dva dlužníci, bez zprostředkovatele')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (5, 5, N'A', N'jeden spoludlužník, zprostředkovatel')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (6, 5, N'B', N'jeden spoludlužník, bez zprostředkovatele')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (7, 5, N'C', N'dva spoludlužníci, zprostředkovatel')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (8, 5, N'D', N'dva spoludlužníci, bez zprostředkovatele')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (9, 9, N'A', N'podepisuje jeden dlužník')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (10, 9, N'B', N'podepisují dva dlužníci')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (11, 9, N'C', N'podepisují tři dlužníci')
GO
INSERT [dbo].[DocumentTemplateVariant] ([Id], [DocumentTemplateVersionId], [DocumentVariant], [Description]) VALUES (12, 9, N'D', N'podepisují čtyři dlužníci')
GO
INSERT [dbo].[DocumentTemplateVersion] ([Id], [DocumentTypeId], [DocumentVersion], [FormTypeId], [ValidFrom], [ValidTo]) VALUES (1, 1, N'001', NULL, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTemplateVersion] ([Id], [DocumentTypeId], [DocumentVersion], [FormTypeId], [ValidFrom], [ValidTo]) VALUES (2, 2, N'001', NULL, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTemplateVersion] ([Id], [DocumentTypeId], [DocumentVersion], [FormTypeId], [ValidFrom], [ValidTo]) VALUES (3, 3, N'001', NULL, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTemplateVersion] ([Id], [DocumentTypeId], [DocumentVersion], [FormTypeId], [ValidFrom], [ValidTo]) VALUES (4, 4, N'001', 3601001, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTemplateVersion] ([Id], [DocumentTypeId], [DocumentVersion], [FormTypeId], [ValidFrom], [ValidTo]) VALUES (5, 5, N'001', 3602001, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTemplateVersion] ([Id], [DocumentTypeId], [DocumentVersion], [FormTypeId], [ValidFrom], [ValidTo]) VALUES (6, 6, N'001', 3700001, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTypes] ([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [EACodeMainId], [IsFormIdRequested], [ValidFrom], [ValidTo]) VALUES (1, N'NABIDKA', N'Nabídka hypotečního úvěru', N'Nabidka_HU', NULL, 605469, 0, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTypes] ([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [EACodeMainId], [IsFormIdRequested], [ValidFrom], [ValidTo]) VALUES (2, N'KALKULHU', N'Hypoteční úvěr - kalkulace', N'Kalkulace_HU', NULL, NULL, 0, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTypes] ([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [EACodeMainId], [IsFormIdRequested], [ValidFrom], [ValidTo]) VALUES (3, N'SPLKALHU', N'Simulace splátkového kalendáře', N'Simulace_SK', NULL, NULL, 0, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTypes] ([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [EACodeMainId], [IsFormIdRequested], [ValidFrom], [ValidTo]) VALUES (4, N'ZADOSTHU', N'Žádost o poskytnutí hypotečního úvěru - první domácnost', N'Zadost_HU', NULL, 608248, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTypes] ([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [EACodeMainId], [IsFormIdRequested], [ValidFrom], [ValidTo]) VALUES (5, N'ZADOSTHD', N'Žádost o poskytnutí hypotečního úvěru - druhá domácnost', N'Zadost_HD', NULL, 608243, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[DocumentTypes] ([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [EACodeMainId], [IsFormIdRequested], [ValidFrom], [ValidTo]) VALUES (6, N'ZADOCERP', N'Žádost o čerpání hypotečního úvěru', N'Cerpani_HU', 6, 613226, 1, CAST(N'2022-01-01T00:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (601230, N'Souhrn Úvěr', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2095-12-31' AS Date), N'Návrhy', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (601231, N'Souhrn Změnový', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2100-12-31' AS Date), N'Návrhy', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602209, N'LV - Výpis z KN', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2100-12-31' AS Date), N'Ocenění/Zajištění', N'VYP_KAT_NEMOV', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602266, N'Cena obvyklá', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2100-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602322, N'ZOV / Rozestavěnost', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', N'ZPR_STAVVYSTAV', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602325, N'Návrh na vklad/výmaz', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', N'NAVRH_VKLAD', 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602341, N'Poplatek odhad ZOV', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602342, N'Infocena - poradce', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602343, N'Vyhodnocení rizik spojených s nemovitou zástavou', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', N'OCEN_VYSLEDEK', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602345, N'Doporučení - klient', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602348, N'Vzdání se Zástavního práva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602349, N'Upřesnění Zástavní smlouvy', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602350, N'Zpětvzetí Zástavní smlouvy', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602351, N'Souhlas s dispozicí zástavy', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602382, N'Přehled investičních nákladů', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602387, N'Anonymizované LV', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602428, N'DZ - Výzva', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602429, N'DZ - Vyrozumění', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602430, N'DZ - Změna na LV', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602431, N'DZ – Plomba', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602550, N'Souhlas se zatížením zástavy', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602551, N'Souhlas zástavce s dohodou o změně závazku', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602552, N'Zástavní smlouva k pohledávce', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602553, N'Souhlas s výstavbou na sousedním pozemku', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602554, N'Fotodokumentace', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602555, N'Geometrický plán', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602556, N'Karta nemovitosti', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602557, N'Nájemní smlouva', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602558, N'Objednání ocenění', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602559, N'Projektová dokumentace', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602560, N'Snímek z KM', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602561, N'Prohlášení vlastníka', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602562, N'Doklady k žádosti - ocenění', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602564, N'Vzdání se práva odvolání k zpětvzětí', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (602565, N'Zrušení vinkulace / zrušení zástavního práva k pohledávce', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ocenění/Zajištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603225, N'Smlouva k HÚ', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Smluvní dokumentace', N'KLD_HYPOUVER', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603249, N'Smlouva k HÚ', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Smluvní dokumentace', N'KLD_HYPOUVER', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603255, N'Zástavní smlouva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Smluvní dokumentace', N'SML_ZASTAVNI', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603256, N'Ručitelské prohlášení', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Smluvní dokumentace', N'RUCITEL_PROHL', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603257, N'Smlouva o zástavě pohledávky', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Smluvní dokumentace', N'N/A?', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603527, N'Smlouva o převzetí dluhu', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Smluvní dokumentace', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603528, N'Dohoda o převzetí dluhů ze zástavní smlouvy', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Smluvní dokumentace', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603529, N'Úvěrové podmínky', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Smluvní dokumentace', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603530, N'Pravidla časové úhrady pohledávek', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Smluvní dokumentace', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603563, N'Prohlášení poddlužníka', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Smluvní dokumentace', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (603572, N'Smlouva k AMHÚ', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Smluvní dokumentace', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604232, N'Dodatek ke smlouvě HÚ', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', N'DOD_HU', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604581, N'Dodatek Převzetí dluhu', NULL, CAST(N'2022-10-25' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604582, N'Dodatek Přistoupení k dluhu', NULL, CAST(N'2022-10-25' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604583, N'Dodatek Uvolnění dluhu', NULL, CAST(N'2022-10-25' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604584, N'Dodatek ke Smlouvě o úvěru - změna smluvních stran', NULL, CAST(N'2022-11-15' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604585, N'Dohoda o ukončení smlouvy o hypotečním úvěru při nečerpání úvěru', NULL, CAST(N'2022-11-15' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604586, N'Dodatek - ukončení HU při nečerpání', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604587, N'Dodatek - refixace', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604588, N'Dodatek - retence', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604589, N'Oznámení o změně ÚS', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604590, N'Oznámení ÚS - ukončení prac. Poměru zaměstnance', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604591, N'Akceptace/zamítnutí Flexi opce', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604592, N'Dodatek - restruktutralizace', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604593, N'Dodatek - úmrtí dlužníka', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604594, N'Dodatek HÚ bez nemovitosti', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (604595, N'Dodatek - zrušení hedge', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Dodatky ke smlouvám', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605121, N'Dokumenty z GREC', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ostatní', N'DOK_GREC', 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605125, N'Výpisy', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ostatní', N'VYP_PERPOH_PN', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605126, N'Potvrzení o zaplacených úrocích', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ostatní', N'POT_ZAPUR_PN', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605289, N'Potvrzení o zániku zástavního práva', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605309, N'Doklady po čerpání', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ostatní', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605347, N'Shoda/Neshoda', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ostatní', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605352, N'Podpůrný dokument', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605353, N'Sdělení k obnově úrokové sazby', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605354, N'ESIP', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605355, N'Esip - restrukturalizace', NULL, CAST(N'2023-02-28' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605454, N'Notářský zápis', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Ostatní', N'NOTAR_ZAPIS', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605534, N'Kvitance JPÚ', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605540, N'Oznámení k dědictví', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605541, N'Potvrzení o splacení úvěru', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605542, N'Výzva k uhrazení smluvní pokuty', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605543, N'Oznámení o neplnění podmínek smlouvy', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605544, N'Restrukturalizace - souhlas', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605545, N'Souhlas s výplatou odkupného', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605546, N'Výzva k úhradě za nedočerpání úvěru', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605566, N'Doklady ke změně', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605567, N'Písemný souhlas banky s výmazem zástavního práva', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605568, N'Rozhodnutí soudu', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605569, N'Nabídka', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605570, N'Prohlášení majitele účtu', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605573, N'Průvodní dopis ke sdělení flexi HÚ', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605575, N'Splátkový kalendář (tabulka umoření) - HÚ', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605576, N'Vyčíslení zůstatku úvěru (bez platebních údajů)', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (605577, N'Vyčíslení zůstatku úvěru (s platebními údaji)', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Ostatní', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608243, N'Prohlášení účastníka', N'Prohlášení účastníka', CAST(N'2008-04-24' AS Date), CAST(N'2098-12-31' AS Date), N'Žádosti', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608248, N'KB Žádost o hypoteční úvěr', NULL, CAST(N'2022-06-21' AS Date), CAST(N'2098-04-13' AS Date), N'Žádosti', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608279, N'Žádost o změnu', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Žádosti', N'ZAD_ZMENPODHU', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608287, N'Žádost o předčasné splacení', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608295, N'Nerealizovaný účastník', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Žádosti', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608298, N'Nerealizovaná žádost', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Žádosti', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608389, N'Žádost o změny v pojištění pro KP', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Žádosti', N'ZAD_POJISTKP', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608390, N'Průvodní list', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Žádosti', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608522, N'Odstoupení od žádosti', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608523, N'Zápis o jednání o HÚ (developerský projekt)', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608524, N'Údaje pro změnu dlužníků', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608578, N'Žádost o změnu FLEXI', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608579, N'Žádost o změnu - superdodatek', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (608580, N'Žádost o změnu dlužníka', NULL, CAST(N'2023-01-18' AS Date), CAST(N'2099-12-31' AS Date), N'Žádosti', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (609217, N'Zamítací dopis', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Schválení obchodu/změny', N'DPS_ZAMITACI', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (611391, N'Potvrzení o splacení úvěru', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Hodnocení klienta', N'POT_SPLACENIU', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (611392, N'Potvrzení o zůstatku úvěru', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Hodnocení klienta', N'POT_ZUSTATEKUV', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612141, N'Vinkulace pojistného plnění', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Pojištění', N'VINKULACE_POHL', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612253, N'Oznámení pojišťovně o vzniku/zániku ZP', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Pojištění', NULL, 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612384, N'Souhlas se změnou RŽP', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Pojištění', N'SOUHLAS_RIZZP', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612385, N'Zamítací dopis RŽP', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Pojištění', N'DPS_ZAMITACI', 0, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612386, N'Pojistná smlouva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Pojištění', N'POJ_SML_NEMOV', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612388, N'Sdělení účtu k pojistnému plnění', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Pojištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (612395, N'Oznámení o vzdání se práva na pojistné plnění', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Pojištění', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (613226, N'Žádost o čerpání úvěru', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Čerpání úvěru', N'ZAD_CERPUVERU', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (613304, N'Doklady k čerpání', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Čerpání úvěru', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (613548, N'Potvrzení o čerpání', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Čerpání úvěru', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (613549, N'Oznámení o nevyčerpání', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Čerpání úvěru', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614365, N'Darovací smlouva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'DAROVACISML', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614366, N'Dohoda o složení zálohy', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614367, N'Dražební protokol', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614368, N'Rezervační smlouva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614369, N'Smlouva o advokátní/notářské úschově', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'SML_NOTAR_USCH', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614370, N'Smlouva o dílo', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'SML_DILO', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614371, N'Smlouva o převodu podílu v družstvu', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'SML_PREV_DRUZS', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614372, N'Smlouva o smlouvě budoucí', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'SML_BUD_KUP', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614373, N'Smlouva o úvěru/půjčce', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'SML_UV_BANK', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614374, N'Vyčíslení zůstatku', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'VYCISLENI_ZUST', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614375, N'Faktura', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', N'FAKTURA_UCUV', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614376, N'Kolaudační rozhodnutí', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614377, N'Kupní smlouva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614378, N'Rozhodnutí o dědictví', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614379, N'Rozpočet', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614380, N'Stavební povolení', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (614381, N'Doklady k žádosti - účel', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Účel', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615356, N'Účetní výkazy, daňové přiznání, zpráva auditora', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', N'UCETNI_VYKAZY', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615357, N'Potvrzení o příjmech klienta', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', N'POT_PRIJ_KB_FORM', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615358, N'Pracovní smlouva', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', N'PRACOVNI_SML', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615359, N'Daňové přiznání', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', N'DAN_PRIZNANI', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615360, N'Dávky státní sociální podpory', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', N'DAVKA_SSP', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615361, N'Výpis z účtu', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', N'VYPISBU', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (615362, N'Doklady k žádosti - příjmy', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Příjmy', NULL, 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616092, N'Plná moc', NULL, CAST(N'2022-06-29' AS Date), CAST(N'2098-12-31' AS Date), N'Klient', N'v DOC_TYPE zrušeno', 1, 1)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616535, N'Čestné prohlášení o rodinném stavu zástavce', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616536, N'Prohlášení manžela/ky zástavce', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616537, N'Prohlášení manžela/ky prodávajícího', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616538, N'Nepropsané údaje do KB CURE', NULL, CAST(N'2023-03-09' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616539, N'Identifikace zástavce', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616571, N'Osobní doklady', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EA_CIS_EACODEMAIN] ([kod], [popis], [popis_klient], [platnost_od], [platnost_do], [kategorie], [druh_kb], [viditelnost_pro_vlozeni_noby], [viditelnost_ps_kb_prodejni_sit_kb]) VALUES (616578, N'CRS  dotazník', NULL, CAST(N'2023-02-21' AS Date), CAST(N'2099-12-31' AS Date), N'Klient', NULL, 0, 0)
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (0, N'N')
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (1, N'Z')
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (2, N'S')
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (3, N'M')
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (4, N'B')
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (5, N'D')
GO
INSERT [dbo].[EducationLevelExtension] ([EducationLevelId], [RDMCode]) VALUES (6, N'V')
GO
INSERT [dbo].[ChannelExtension] ([ChannelId], [RdmCbChannelCode]) VALUES (4, N'CH0001')
GO
INSERT [dbo].[ChannelExtension] ([ChannelId], [RdmCbChannelCode]) VALUES (6, N'CH0002')
GO
INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [MpDigiApiCode]) VALUES (0, N'Undefined')
GO
INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [MpDigiApiCode]) VALUES (1, N'IDCard')
GO
INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [MpDigiApiCode]) VALUES (2, N'Passport')
GO
INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [MpDigiApiCode]) VALUES (3, N'ResidencePermit')
GO
INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [MpDigiApiCode]) VALUES (4, N'Foreign')
GO
INSERT [dbo].[IncomeMainTypesAMLExtension] ([Id], [RdmCode]) VALUES (1, N'1')
GO
INSERT [dbo].[IncomeMainTypesAMLExtension] ([Id], [RdmCode]) VALUES (2, N'2')
GO
INSERT [dbo].[IncomeMainTypesAMLExtension] ([Id], [RdmCode]) VALUES (3, N'3')
GO
INSERT [dbo].[IncomeMainTypesAMLExtension] ([Id], [RdmCode]) VALUES (4, N'4')
GO
INSERT [dbo].[IncomeMainTypesAMLExtension] ([Id], [RdmCode]) VALUES (5, N'5')
GO
INSERT [dbo].[IncomeMainTypesAMLExtension] ([Id], [RdmCode]) VALUES (6, N'6')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (1, N'S')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (2, N'M')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (3, N'D')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (4, N'W')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (6, N'R')
GO
INSERT [dbo].[NetMonthEarningsExtension] ([NetMonthEarningId], [RdmCode]) VALUES (1, N'A')
GO
INSERT [dbo].[NetMonthEarningsExtension] ([NetMonthEarningId], [RdmCode]) VALUES (2, N'B')
GO
INSERT [dbo].[NetMonthEarningsExtension] ([NetMonthEarningId], [RdmCode]) VALUES (3, N'C')
GO
INSERT [dbo].[NetMonthEarningsExtension] ([NetMonthEarningId], [RdmCode]) VALUES (4, N'D')
GO
INSERT [dbo].[NetMonthEarningsExtension] ([NetMonthEarningId], [RdmCode]) VALUES (5, N'E')
GO
INSERT [dbo].[ObligationTypeExtension] ([ObligationTypeId], [ObligationProperty]) VALUES (1, N'amount')
GO
INSERT [dbo].[ObligationTypeExtension] ([ObligationTypeId], [ObligationProperty]) VALUES (2, N'amount')
GO
INSERT [dbo].[ObligationTypeExtension] ([ObligationTypeId], [ObligationProperty]) VALUES (3, N'limit')
GO
INSERT [dbo].[ObligationTypeExtension] ([ObligationTypeId], [ObligationProperty]) VALUES (4, N'limit')
GO
INSERT [dbo].[ObligationTypeExtension] ([ObligationTypeId], [ObligationProperty]) VALUES (5, N'amount')
GO
INSERT [dbo].[ProductTypeExtension] ([ProductTypeId], [MpHomeApiLoanType], [KonsDbLoanType]) VALUES (20001, N'KBMortgage', 3)
GO
INSERT [dbo].[ProductTypeExtension] ([ProductTypeId], [MpHomeApiLoanType], [KonsDbLoanType]) VALUES (20002, N'KBMortgage', 4)
GO
INSERT [dbo].[ProductTypeExtension] ([ProductTypeId], [MpHomeApiLoanType], [KonsDbLoanType]) VALUES (20003, N'KBMortgage', 5)
GO
INSERT [dbo].[ProductTypeExtension] ([ProductTypeId], [MpHomeApiLoanType], [KonsDbLoanType]) VALUES (20004, N'KBMortgage', 6)
GO
INSERT [dbo].[ProductTypeExtension] ([ProductTypeId], [MpHomeApiLoanType], [KonsDbLoanType]) VALUES (20010, N'KBMortgage', 7)
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (0, N'0', NULL)
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (1, N'1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12', N'1, 6')
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (2, N'1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12', N'1, 6')
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (3, N'1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12', N'1, 6')
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (4, N'1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12', N'2, 6')
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (5, N'1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12', N'1, 6')
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (6, N'0, 13, 14', N'3, 4, 5')
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (7, N'0', NULL)
GO
INSERT [dbo].[ProfessionCategoryExtension] ([ProfessionCategoryId], [ProfessionTypeIds], [IncomeMainTypeAMLIds]) VALUES (8, N'1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12', N'1, 2, 3, 6')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (0, NULL, N'NotSpecified', N'nezadán')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (1, N'A', N'Owner', N'Hlavní dlužník')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (2, N'S', N'CoDebtor', N'Spoludlužník')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (3, NULL, N'Accessor', N'Přistupitel')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (4, NULL, N'HusbandOrWife', N'Manžel-ka')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (5, NULL, N'LegalRepresentative', N'Zákonný zástupce')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (6, NULL, N'CollisionGuardian', N'Kolizní opatrovník')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (7, NULL, N'Guardian', N'Opatrovník')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (8, N'R', N'Guarantor', N'Ručitel')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (9, NULL, N'GuarantorHusbandOrWife', N'Manžel-ka ručitele')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (11, NULL, N'Intermediary', N'Sprostředkovatel')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (12, NULL, N'ManagingDirector', N'Jednatel')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby]) VALUES (13, NULL, N'Child', N'Dítě')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (1, 2, CAST(N'2023-03-01' AS Date), NULL, N'20003', NULL, NULL, NULL, NULL, N'MLHY', N'ATK0001', N'Hypoteční úvěr s LTV do 60%')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (2, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', NULL, NULL, CAST(0.0000 AS Numeric(16, 4)), CAST(85.0000 AS Numeric(16, 4)), N'MLHY', N'ATK0002', N'Klasik')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (3, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', NULL, NULL, CAST(85.0000 AS Numeric(16, 4)), CAST(100.0000 AS Numeric(16, 4)), N'MLHY', N'ATK0003', N'Plus')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (4, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', NULL, NULL, CAST(100.0000 AS Numeric(16, 4)), NULL, N'MLHY', N'ATK0004', N'Plus150')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (5, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', NULL, N'31,32,33,34', CAST(0.0000 AS Numeric(16, 4)), CAST(85.0000 AS Numeric(16, 4)), N'MLHY', N'ATK0005', N'Flexi')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (6, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', NULL, N'31,32,33,34', CAST(85.0000 AS Numeric(16, 4)), NULL, N'MLHY', N'ATK0006', N'Flexi HÚ Plus')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (7, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', N'1001', NULL, CAST(0.0000 AS Numeric(16, 4)), CAST(85.0000 AS Numeric(16, 4)), N'MLHY', N'ATK0007', N'Hypotéka bez nemovitosti – klasik')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (8, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', N'1001', NULL, CAST(85.0000 AS Numeric(16, 4)), NULL, N'MLHY', N'ATK0008', N'Hypotéka bez nemovitosti – plus')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (9, 2, CAST(N'2023-03-01' AS Date), NULL, N'20001', N'1002', NULL, NULL, NULL, N'MLHY', N'ATK0009', N'Předhypoteční úvěr')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (10, 2, CAST(N'2023-03-01' AS Date), NULL, N'20100', NULL, NULL, NULL, NULL, N'MLHY', N'ATK0010', N'Předhypoteční úvěr')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (11, 2, CAST(N'2023-03-01' AS Date), NULL, N'20004', NULL, NULL, CAST(0.0000 AS Numeric(16, 4)), CAST(85.0000 AS Numeric(16, 4)), N'MLHY', N'ATK0011', N'Klasik doprodej')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (12, 2, CAST(N'2023-03-01' AS Date), NULL, N'20004', NULL, NULL, CAST(85.0000 AS Numeric(16, 4)), NULL, N'MLHY', N'ATK0012', N'Plus doprodej')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (13, 2, CAST(N'2023-03-01' AS Date), NULL, N'20010', NULL, NULL, NULL, NULL, N'MLHA', N'ATK0013', N'Americká hypotéka')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (14, 2, CAST(N'2023-03-01' AS Date), NULL, N'20002', NULL, NULL, NULL, NULL, N'MLKL', N'ATK0014', N'Překlenovací úvěr')
GO
INSERT [dbo].[RiskApplicationType] ([Id], [Mandant], [DateFrom], [DateTo], [LoanProductsId], [LoanType], [MarketingActions], [LtvFrom], [LtvTo], [ClusterCode], [C4mAplTypeId], [C4mAplTypeName]) VALUES (15, 1, CAST(N'2023-03-01' AS Date), NULL, N'89,90,91,92', NULL, NULL, CAST(0.0000 AS Numeric(16, 4)), CAST(85.0000 AS Numeric(16, 4)), N'MPSS_HYPO', N'ATM0001', N'Klasik')
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (1, N'Žádost o hypotéční úvěr', 20001, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (2, N'Žádost o hypoteční překlenovací úvěry', 20002, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (3, N'Žádost o hypoteční úvěr bez příjmu', 20003, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (4, N'Žádost o doprodej neúčelové části', 20004, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (5, N'Žádost o americkou hypotéku', 20010, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (6, N'Žádost o čerpání', NULL, 2)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (7, N'Žádost o obecnou změnu', NULL, 2)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (8, N'Žádost o změnu HUBN', NULL, 2)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (9, N'Žádost o změnu dlužníků', NULL, 2)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (10, N'Žádost o změnu detailu dlužníka (3602)', NULL, 2)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [SalesArrangementCategory]) VALUES (11, N'Žádost o přidání spoludlužníka (3602)', NULL, 2)
GO
INSERT [dbo].[SmsNotificationType] ([Id], [Code], [Description], [SmsText], [McsCode], [IsAuditLogEnabled]) VALUES (1, N'RETENTION', N'Popis', NULL, N'MCS_HF_INSIGN_001', 1)
GO
INSERT [dbo].[SmsNotificationType] ([Id], [Code], [Description], [SmsText], [McsCode], [IsAuditLogEnabled]) VALUES (2, N'RETENTION_TEMPLATE', N'Testovací template', N'Toto je template sms z prostředí {{environment}}.', N'MCS_HF_INSIGN_001', 1)
GO
INSERT [dbo].[SmsNotificationType] ([Id], [Code], [Description], [SmsText], [McsCode], [IsAuditLogEnabled]) VALUES (3, N'TESTING', N'temlate pro FAT test', N'Mara bude spamovat od {{date_from}} do {{date_to}} v poctu {{count}} sms zprav. Teste.', N'MCS_HF_INSIGN_001', 1)
GO
INSERT [dbo].[WorkflowTaskStateExtension] ([WorkflowTaskStateId], [Flag]) VALUES (0, 1)
GO
INSERT [dbo].[WorkflowTaskStateExtension] ([WorkflowTaskStateId], [Flag]) VALUES (30, 1)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (4220, 1)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (4245, 2)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (4246, 2)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (4247, 2)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (4250, 3)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (4251, 3)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (42210, 1)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (42220, 1)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (42461, 2)
GO
INSERT [dbo].[WorkflowTaskTypeExtension] ([WorkflowTaskTypeId], [CategoryId]) VALUES (42471, 2)
GO
