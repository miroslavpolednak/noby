UPDATE DocumentDataField SET StringFormat = '{0}.dne v mesíci' WHERE DocumentDataFieldId = 96

UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'ZeZamestnani'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'ZPodnikani'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'ZPronajmu'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'Ostatni'

UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelZeZamestnani'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelZPodnikani'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelZPronajmu'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 4 AND AcroFieldName = 'SpoluzadatelOstatni'

UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' where DocumentId = 4 AND FieldPath LIKE 'DebtorObligation.%'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' where DocumentId = 4 AND FieldPath LIKE 'CodebtorObligation.%'