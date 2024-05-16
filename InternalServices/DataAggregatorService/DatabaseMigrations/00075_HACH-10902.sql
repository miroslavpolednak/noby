INSERT INTO DocumentDataField
SELECT DocumentId, '002', AcroFieldName, DataFieldId, StringFormat, TextAlign, DefaultTextIfNull, VAlign FROM DocumentDataField
WHERE DocumentId = 15

INSERT INTO DocumentSpecialDataField
SELECT DocumentId, '002', AcroFieldName, DataServiceId, FieldPath, StringFormat, TextAlign, DefaultTextIfNull, VAlign FROM DocumentSpecialDataField
WHERE DocumentId = 15

UPDATE DocumentSpecialDataField SET DataServiceId = 11 WHERE DocumentId = 15 AND DocumentVersion = '002' AND AcroFieldName = 'TextOznameni'