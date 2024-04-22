--HUBN
INSERT INTO DataField VALUES (222, 1, 'SalesArrangement.HUBN.DrawingDateTo.IsActive')
INSERT INTO DataField VALUES (223, 1, 'SalesArrangement.HUBN.DrawingDateTo.IsDrawingDateEarlier')

INSERT INTO DocumentDataField VALUES (10, '001', 'LhutaUkonceniCerpaniLabel', 207, 'Změna lhůty čerpání', NULL, NULL, NULL)

INSERT INTO DocumentDynamicStringFormat VALUES (124, 10, '001', 'LhutaUkonceniCerpaniLabel', 'Zkrácení lhůty čerpání', 1)

INSERT INTO DocumentDynamicStringFormatCondition VALUES (124, 'True', 222)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (124, 'True', 223)

INSERT INTO DocumentDynamicStringFormat VALUES (125, 10, '001', 'LhutaUkonceniCerpaniLabel', 'Prodloužení lhůty čerpání', 2)

INSERT INTO DocumentDynamicStringFormatCondition VALUES (125, 'True', 222)

--General Change
INSERT INTO DataField VALUES (224, 1, 'SalesArrangement.GeneralChange.DrawingDateTo.IsActive')
INSERT INTO DataField VALUES (225, 1, 'SalesArrangement.GeneralChange.DrawingDateTo.IsDrawingDateEarlier')

DELETE FROM DocumentSpecialDataField WHERE DocumentId = 8 AND DocumentVersion = '001' AND AcroFieldName = 'ZmenaLhutyCerpaniLabel'

INSERT INTO DocumentDataField VALUES (8, '001', 'ZmenaLhutyCerpaniLabel', 207, 'Změna lhůty čerpání', NULL, NULL, NULL)

INSERT INTO DocumentDynamicStringFormat VALUES (126, 8, '001', 'ZmenaLhutyCerpaniLabel', 'Zkrácení lhůty čerpání', 1)

INSERT INTO DocumentDynamicStringFormatCondition VALUES (126, 'True', 224)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (126, 'True', 225)

INSERT INTO DocumentDynamicStringFormat VALUES (127, 8, '001', 'ZmenaLhutyCerpaniLabel', 'Prodloužení lhůty čerpání', 2)

INSERT INTO DocumentDynamicStringFormatCondition VALUES (127, 'True', 224)