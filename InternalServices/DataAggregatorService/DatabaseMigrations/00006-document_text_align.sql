ALTER TABLE DocumentDataField ADD TextAlign TINYINT NULL
ALTER TABLE DocumentSpecialDataField ADD TextAlign TINYINT NULL

GO

UPDATE DocumentDataField SET TextAlign = 4 WHERE DocumentDataFieldId = 14

UPDATE DocumentDataField SET StringFormat = NULL WHERE DataFieldId IN (11, 12)
UPDATE DataField SET DefaultStringFormat = '{0:#,#.##} Kč/ročně' WHERE DataFieldId IN (11, 12)
UPDATE DynamicStringFormat SET Format = 'ne' WHERE DynamicStringFormatId = 4
UPDATE DynamicStringFormat SET Format = 'ano' WHERE DynamicStringFormatId = 5