UPDATE DocumentDataField 
SET StringFormat = 'Tato nabídka má pouze informační charakter a úroková sazba zde uvedená není námi garantována.


Seznam podkladů k Žádosti o úvěr je možné nalézt zde: www.kb.cz.' 
WHERE DocumentId = 1 AND AcroFieldName = 'Text'

DELETE FROM DocumentDynamicStringFormat WHERE DynamicStringFormatId = 13