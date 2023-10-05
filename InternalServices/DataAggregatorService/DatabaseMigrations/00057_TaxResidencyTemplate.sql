UPDATE DataField SET FieldPath = 'Customer.TaxResidenceCountriesLimited[].Tin' WHERE DataFieldId = 195

UPDATE DocumentDataField SET DataFieldId = 215 WHERE DocumentId = 13 AND AcroFieldName IN ('ObcanUSA', 'RezidentUSA')

UPDATE DocumentDataField SET DefaultTextIfNull = '--' WHERE DocumentId = 9 AND AcroFieldName = 'DetailniPopisText'

UPDATE DocumentDataField SET StringFormat = '--', DefaultTextIfNull = '--' WHERE DocumentId = 9 AND AcroFieldName = 'ZmocnenecProDorucovani'