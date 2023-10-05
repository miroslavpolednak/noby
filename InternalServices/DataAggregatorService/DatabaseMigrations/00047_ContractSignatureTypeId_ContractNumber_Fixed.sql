DELETE FROM EasFormDataField WHERE JsonPropertyName = 'zpusob_podpisu_smluv_dok' AND EasRequestTypeId = 1 AND EasFormTypeId = 3

UPDATE EasFormDataField SET DataFieldId = 38 WHERE EasRequestTypeId = 1 AND EasFormTypeId = 3 AND JsonPropertyName = 'cislo_smlouvy'