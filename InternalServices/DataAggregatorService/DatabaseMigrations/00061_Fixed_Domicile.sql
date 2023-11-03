UPDATE DocumentDataField SET StringFormat = 'ne' WHERE DocumentId = 2 AND AcroFieldName = 'Domicilace'

UPDATE DocumentDataField 
SET StringFormat = 'směřování Vašich příjmů na bankovní účet vedený u nás, a to alespoň ve výši 1,5 násobku výše splátky úvěru'
WHERE DocumentId = 1 AND AcroFieldName = 'Domicilace'

INSERT INTO DocumentDynamicStringFormatCondition VALUES (42, 'DOMICILACE', 73)