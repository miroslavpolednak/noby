UPDATE DocumentTypes SET EACodeMainId = 605569 WHERE Id = 1
UPDATE DocumentTypes SET [FileName] = 'Splatkovy_kalendar' WHERE Id = 3
UPDATE DocumentTypes SET [FileName] = 'Zadost_HU1' WHERE Id = 4
UPDATE DocumentTypes SET [FileName] = 'Zadost_HD2' WHERE Id = 5

INSERT INTO DocumentTypes VALUES (7, 'ZADOOPCI', 'Žádost o změnu Flexi', 'Zadost_Flexi', NULL, 608578, 1, '2024-01-01 00:00:00.000', NULL)

UPDATE DocumentTypes SET Id = 11, SalesArrangementTypeId = 12 WHERE Id = 12
UPDATE DocumentTypes SET Id = 12, SalesArrangementTypeId = 10 WHERE Id = 13
UPDATE DocumentTypes SET Id = 13 WHERE Id = 14
UPDATE DocumentTypes SET Id = 14 WHERE Id = 15
UPDATE DocumentTypes SET Id = 15 WHERE Id = 16

INSERT INTO DocumentTypes VALUES (16, 'ZADOSTHD', 'Žádost o přidání spoludlužníka', 'Pridani_spoludluznika', 11, 608243, 1, '2022-01-01 00:00:00.000', NULL)

UPDATE DocumentTemplateVersion SET FormTypeId = '3602001' WHERE Id = 10
UPDATE DocumentTemplateVersion SET FormTypeId = '3602001' WHERE Id = 11
UPDATE DocumentTemplateVersion SET FormTypeId = '3602001' WHERE Id = 15

INSERT INTO DocumentTemplateVariant VALUES (17, 16, 'A', 'jeden spoludlužník, zprostředkovatel')
INSERT INTO DocumentTemplateVariant VALUES (18, 16, 'B', 'jeden spoludlužník, bez zprostředkovatele')
INSERT INTO DocumentTemplateVariant VALUES (19, 16, 'C', 'dva spoludlužníci, zprostředkovatel')
INSERT INTO DocumentTemplateVariant VALUES (20, 16, 'D', 'dva spoludlužníci, bez zprostředkovatelel')