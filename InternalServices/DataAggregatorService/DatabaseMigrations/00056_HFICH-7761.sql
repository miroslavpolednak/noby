UPDATE DocumentDataField SET TextAlign = 4 WHERE DocumentId = 9 AND AcroFieldName = 'DetailniPopisText'

INSERT INTO DataField VALUES (217, 1, 'SalesArrangement.GeneralChange.DueDate.ConnectionExtraordinaryPayment')

INSERT INTO DocumentDynamicStringFormat VALUES (122, 8, '001', 'NovaDobaSplatnosti', '{0:Y} v souvislosti s mimořádnou splátkou', 2)

INSERT INTO DocumentDynamicStringFormatCondition VALUES (122, 'True', 217)

UPDATE DocumentSpecialDataField SET FieldPath = 'ExtensionDrawingDateText', StringFormat = NULL WHERE DocumentId = 8 AND AcroFieldName = 'ZmenaLhutyCerpani'

UPDATE DocumentDataField SET StringFormat = '{0}. den v měsíci' WHERE DocumentId = 4 AND AcroFieldName = 'DenSplaceni'
UPDATE DocumentDataField SET StringFormat = '{0}. dni v měsíci' WHERe DocumentId IN (1, 2) AND AcroFieldName = 'DenSplaceni'

UPDATE DocumentDataField
SET StringFormat = 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a~jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDynamicStringFormat
SET StringFormat = 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a~jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
where DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDataField
SET StringFormat = 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

INSERT INTO DataField VALUES (218, 1, 'SalesArrangement.CustomerChange.Agent.IsActive')
INSERT INTO DocumentDynamicStringFormat VALUES (123, 9, '001', 'ZmocnenecProDorucovani', '{0}', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (123, 'True', 218)

UPDATE DocumentDataField SET StringFormat = '--' WHERE DocumentId = 9 AND AcroFieldName = 'ZmocnenecProDorucovani'

UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 9 AND AcroFieldName IN ('NoveCisloUctu', 'MajitelUctu')

UPDATE DocumentSpecialDataField SET FieldPath = 'Customer1.SignerName' WHERE DocumentId = 9 AND AcroFieldName = 'PodpisJmenoKlienta'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer2.SignerName' WHERE DocumentId = 9 AND AcroFieldName = 'PodpisJmenoKlienta2'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer3.SignerName' WHERE DocumentId = 9 AND AcroFieldName = 'PodpisJmenoKlienta3'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer4.SignerName' WHERE DocumentId = 9 AND AcroFieldName = 'PodpisJmenoKlienta4'

INSERT INTO DocumentDynamicInputParameter VALUES (9, '001', 7)

INSERT INTO DataField VALUES (219, 2, 'Case.CaseOwner.UserId')
INSERT INTO DynamicInputParameter VALUES (8, 'UserId', 4, 219)
UPDATE DocumentDynamicInputParameter SET DynamicInputParameterId = 8 WHERE DocumentId IN (4, 5, 16) AND DynamicInputParameterId = 7