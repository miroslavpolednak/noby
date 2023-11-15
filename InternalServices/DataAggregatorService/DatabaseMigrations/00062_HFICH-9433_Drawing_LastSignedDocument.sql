INSERT INTO DataField VALUES (220, 11, 'Custom.DocumentOnSa.LastSignedDocument.FormId')
INSERT INTO DataField VALUES (221, 11, 'Custom.DocumentOnSa.LastSignedDocument.EArchivId')

UPDATE EasFormDataField SET DataFieldId = 220 WHERE EasRequestTypeId = 1 AND EasFormTypeId = 1 AND JsonPropertyName = 'business_id_formulare'
UPDATE EasFormDataField SET DataFieldId = 221 WHERE EasRequestTypeId = 1 AND EasFormTypeId = 1 AND JsonPropertyName = 'cislo_dokumentu'