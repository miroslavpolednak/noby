ALTER TABLE DocumentSpecialDataField ADD DefaultTextIfNull NVARCHAR(500) NULL

GO

UPDATE DocumentDataField SET StringFormat = '{0}.dni v měsíci', DefaultTextIfNull = '--' WHERE DocumentDataFieldId = 96

UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 101
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 102

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'VyseUveruNeucelova'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'RodneCislo'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelRodneCislo'

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'ZeZamestnani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'ZPodnikani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'ZPronajmu'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'Ostatni'

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelZeZamestnani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelZPodnikani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelZPronajmu'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelOstatni'

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' where DocumentId = 4 AND FieldPath LIKE 'DebtorObligation.%' AND StringFormat = '{0:C}'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}' where DocumentId = 4 AND FieldPath LIKE 'CodebtorObligation.%' AND StringFormat = '{0:C}'

INSERT INTO DataField VALUES (163, 10, 'CustomerOnSaDebtor.CustomerAdditionalData.HasRelationshipWithKB', NULL)
INSERT INTO DataField VALUES (164, 10, 'CustomerOnSaDebtor.CustomerAdditionalData.HasRelationshipWithKBEmployee', NULL)
INSERT INTO DataField VALUES (165, 10, 'CustomerOnSaDebtor.CustomerAdditionalData.IsPoliticallyExposed', NULL)
INSERT INTO DataField VALUES (166, 10, 'CustomerOnSaDebtor.CustomerAdditionalData.HasRelationshipWithCorporate', NULL)
INSERT INTO DataField VALUES (167, 10, 'CustomerOnSaDebtor.CustomerAdditionalData.IsUSPerson', NULL)

INSERT INTO DocumentDataField VALUES (168, 4, '001', 163, 'JsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)
INSERT INTO DocumentDataField VALUES (169, 4, '001', 164, 'JsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů', NULL, 4)
INSERT INTO DocumentDataField VALUES (170, 4, '001', 165, 'JsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)
INSERT INTO DocumentDataField VALUES (171, 4, '001', 166, 'JsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací,', NULL, 4)
INSERT INTO DocumentDataField VALUES (172, 4, '001', 167, 'JsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

INSERT INTO DocumentSpecialDataField VALUES (4, 'JsemNejsem5Odrazka', 10, 'DebtorCustomer.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')

INSERT INTO DataField VALUES (168, 10, 'CustomerOnSaCodebtor.CustomerAdditionalData.HasRelationshipWithKB', NULL)
INSERT INTO DataField VALUES (169, 10, 'CustomerOnSaCodebtor.CustomerAdditionalData.HasRelationshipWithKBEmployee', NULL)
INSERT INTO DataField VALUES (170, 10, 'CustomerOnSaCodebtor.CustomerAdditionalData.IsPoliticallyExposed', NULL)
INSERT INTO DataField VALUES (171, 10, 'CustomerOnSaCodebtor.CustomerAdditionalData.HasRelationshipWithCorporate', NULL)
INSERT INTO DataField VALUES (172, 10, 'CustomerOnSaCodebtor.CustomerAdditionalData.IsUSPerson', NULL)

INSERT INTO DocumentDataField VALUES (173, 4, '001', 168, 'SpoluzadatelJsemNejsem1Odrazka', 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů,', NULL, 4)
INSERT INTO DocumentDataField VALUES (174, 4, '001', 169, 'SpoluzadatelJsemNejsem2Odrazka', 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů', NULL, 4)
INSERT INTO DocumentDataField VALUES (175, 4, '001', 170, 'SpoluzadatelJsemNejsem3Odrazka', 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,', NULL, 4)
INSERT INTO DocumentDataField VALUES (176, 4, '001', 171, 'SpoluzadatelJsemNejsem4Odrazka', 'nejsem propojen s obchodní korporací,', NULL, 4)
INSERT INTO DocumentDataField VALUES (177, 4, '001', 172, 'SpoluzadatelJsemNejsem6Odrazka', 'nejsem Americkou osobou.', NULL, 4)

INSERT INTO DocumentSpecialDataField VALUES (4, 'SpoluzadatelJsemNejsem5Odrazka', 10, 'CodebtorCustomer.CzechResidence', 'jsem českým daňovým residentem,', 4, 'nejsem českým daňovým residentem,')

DELETE FROM DocumentSpecialDataFieldVariant WHERE DocumentId = 4 AND AcroFieldName = 'UdajeDomacnosti'
DELETE FROM DocumentSpecialDataFieldVariant WHERE DocumentId = 4 AND AcroFieldName = 'VydajeDomacnosti'
DELETE FROM DocumentSpecialDataField WHERE DocumentId = 4 AND AcroFieldName = 'UdajeDomacnosti'
DELETE FROM DocumentSpecialDataField WHERE DocumentId = 4 AND AcroFieldName = 'VydajeDomacnosti'

UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 107
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 108
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 109
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 110

INSERT INTO DynamicStringFormat VALUES (43, 107, 'Kč', 1)
INSERT INTO DynamicStringFormat VALUES (44, 108, 'Kč', 1)

INSERT INTO DynamicStringFormatCondition VALUES (43, 'CZK', 84)
INSERT INTO DynamicStringFormatCondition VALUES (44, 'CZK', 85)

INSERT INTO DocumentSpecialDataField VALUES (4, 'PodpisJmenoKlienta', 10, 'DebtorCustomer.FullName', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (4, 'PodpisJmenoKlienta2', 10, 'CodebtorCustomer.FullName', NULL, NULL, NULL)

INSERT INTO DocumentSpecialDataFieldVariant VALUES (4, 'PodpisJmenoKlienta', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (4, 'PodpisJmenoKlienta', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (4, 'PodpisJmenoKlienta', 'C')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (4, 'PodpisJmenoKlienta', 'D')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (4, 'PodpisJmenoKlienta2', 'C')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (4, 'PodpisJmenoKlienta2', 'D')

DELETE FROM DocumentDataField WHERE DataFieldId = 26 AND AcroFieldName = 'VyseSplatky'

CREATE UNIQUE INDEX IX_DocumentDataField_AcroField ON DocumentDataField(DocumentId, DocumentVersion, AcroFieldName)