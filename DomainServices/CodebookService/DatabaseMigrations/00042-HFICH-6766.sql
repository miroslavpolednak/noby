TRUNCATE TABLE DocumentTypes;
GO

INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(1, N'NABIDKA', N'Nabídka hypotečního úvěru', N'Nabidka_HU', NULL, 605569, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(2, N'KALKULHU', N'Hypoteční úvěr - kalkulace', N'Kalkulace_HU', NULL, NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(3, N'SPLKALHU', N'Simulace splátkového kalendáře', N'Splatkovy_kalendar', NULL, NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(4, N'ZADOSTHU', N'Žádost o poskytnutí úvěru', N'Zadost_HU1', NULL, 608248, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(5, N'ZADOSTHD', N'Žádost o poskytnutí úvěru (spoludlužníci)', N'Zadost_HU2', NULL, 608243, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(6, N'ZADOCERP', N'Žádost o čerpání hypotečního úvěru', N'Cerpani_HU', 6, 613226, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(7, N'ZADOOPCI', N'Žádost o uplatnění Flexi opce', N'Zadost_Flexi', 13, 608578, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(8, N'ZAOZMPAR', N'Žádost o změnu parametrů', N'Zadost_parametricka', 7, 608279, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(9, N'ZAOZMDLU', N'Žádost o změnu dlužníka', N'Zadost_dluznik', 9, 608580, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(10, N'ZAODHUBN', N'Žádost o změnu HÚ bez nemovitosti', N'Zadost_bezNem', 8, 608579, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(11, N'ZUSTAVSI', N'Údaje o zůstávajícím v úvěru', N'Zustavajici_v_HU', 12, 608524, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(12, N'PRISTOUP', N'Údaje o přistupujícím k úvěru', N'Pristupujici_k_HU', 10, 608524, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(13, N'DANRESID', N'Prohlášení o daňové rezidenci', N'Prohlaseni_dan', NULL, 616578, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(15, N'ODSTOUP', N'Ukončení žádosti o úvěr', N'Ukonceni_zadosti_HU', NULL, 608522, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTypes (Id, ShortName, Name, FileName, SalesArrangementTypeId, EACodeMainId, ValidFrom, ValidTo) VALUES(16, N'ZADOSTHD', N'Žádost o poskytnutí úvěru (spoludlužníci)', N'Pridani_spoludluznika', 11, 608243, '2022-01-01 00:00:00.000', NULL);

TRUNCATE TABLE DocumentTemplateVersion;
GO

INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(1, 1, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(2, 2, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(3, 3, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(4, 4, N'001', 3601001, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(5, 5, N'001', 3602001, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(6, 6, N'001', 3700001, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(7, 7, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(8, 8, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(9, 9, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(10, 10, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(11, 11, N'001', 3602001, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(12, 12, N'001', 3602001, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(13, 13, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(15, 15, N'001', NULL, '2022-01-01 00:00:00.000', NULL);
INSERT INTO DocumentTemplateVersion (Id, DocumentTypeId, DocumentVersion, FormTypeId, ValidFrom, ValidTo) VALUES(16, 16, N'001', 3602001, '2022-01-01 00:00:00.000', NULL);

TRUNCATE TABLE DocumentTemplateVariant;
GO

INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(1, 4, N'A', N'jeden dlužník, zprostředkovatel');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(2, 4, N'B', N'jeden dlužník, bez zprostředkovatele');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(3, 4, N'C', N'dva dlužníci, zprostředkovatel');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(4, 4, N'D', N'dva dlužníci, bez zprostředkovatele');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(5, 5, N'A', N'jeden spoludlužník, zprostředkovatel');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(6, 5, N'B', N'jeden spoludlužník, bez zprostředkovatele');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(7, 5, N'C', N'dva spoludlužníci, zprostředkovatel');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(8, 5, N'D', N'dva spoludlužníci, bez zprostředkovatele');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(9, 9, N'A', N'podepisuje jeden dlužník');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(10, 9, N'B', N'podepisují dva dlužníci');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(11, 9, N'C', N'podepisují tři dlužníci');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(12, 9, N'D', N'podepisují čtyři dlužníci');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(13, 11, N'A', N'podepisuje jeden dlužník');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(14, 11, N'B', N'podepisují dva dlužníci');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(15, 12, N'A', N'podepisuje jeden dlužník');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(16, 12, N'B', N'podepisují dva dlužníci');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(17, 16, N'A', N'jeden spoludlužník, zprostředkovatel');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(18, 16, N'B', N'jeden spoludlužník, bez zprostředkovatele');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(19, 16, N'C', N'dva spoludlužníci, zprostředkovatel');
INSERT INTO DocumentTemplateVariant (Id, DocumentTemplateVersionId, DocumentVariant, Description) VALUES(20, 16, N'D', N'dva spoludlužníci, bez zprostředkovatelel');
