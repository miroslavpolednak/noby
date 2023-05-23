DELETE FROM DocumentSpecialDataFieldVariant WHERE DocumentId = 5 AND AcroFieldName = 'Zahlavi1Radek'
DELETE FROM DocumentSpecialDataFieldVariant WHERE DocumentId = 5 AND AcroFieldName = 'Zahlavi2Radky'

DELETE FROM DocumentSpecialDataField WHERE DocumentId = 5 AND AcroFieldName = 'Zahlavi1Radek'
DELETE FROM DocumentSpecialDataField WHERE DocumentId = 5 AND AcroFieldName = 'Zahlavi2Radky'

UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'RodneCislo'
UPDATE DocumentSpecialDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'SpoluzadatelRodneCislo'

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'ZeZamestnani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'ZPodnikani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'ZPronajmu'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'Ostatni'

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'SpoluzadatelZeZamestnani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'SpoluzadatelZPodnikani'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'SpoluzadatelZPronajmu'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' WHERE DocumentId = 5 AND AcroFieldName = 'SpoluzadatelOstatni'

UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' where DocumentId = 5 AND FieldPath LIKE 'Customer1Obligation%'
UPDATE DocumentSpecialDataField SET StringFormat = '{0:CustomCurrency}', DefaultTextIfNull = '--' where DocumentId = 5 AND FieldPath LIKE 'Customer2Obligation%'

UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 119
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 120
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 121
UPDATE DataField SET DefaultStringFormat = '{0:CustomCurrency}' WHERE DataFieldId = 122

INSERT INTO DocumentSpecialDataField VALUES (5, 'PodpisJmenoKlienta', 10, 'Customer1.FullName', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (5, 'PodpisJmenoKlienta2', 10, 'Customer2.FullName', NULL, NULL, NULL)

INSERT INTO DocumentSpecialDataFieldVariant VALUES (5, 'PodpisJmenoKlienta', 'A')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (5, 'PodpisJmenoKlienta', 'B')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (5, 'PodpisJmenoKlienta', 'C')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (5, 'PodpisJmenoKlienta', 'D')

INSERT INTO DocumentSpecialDataFieldVariant VALUES (5, 'PodpisJmenoKlienta2', 'C')
INSERT INTO DocumentSpecialDataFieldVariant VALUES (5, 'PodpisJmenoKlienta2', 'D')