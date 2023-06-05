UPDATE DocumentDataField SET AcroFieldName = 'VyseSplatkyCelkem' WHERE DocumentDataFieldId = 6
UPDATE DocumentDataField SET AcroFieldName = 'VyseSplatkyCelkem' WHERE DocumentDataFieldId = 35

UPDATE DynamicStringFormatCondition SET EqualToValue = 'VYSE_PRIJMU_UVERU', DataFieldId = 73 WHERE DynamicStringFormatId IN (10, 34) AND DataFieldId = 14

UPDATE DynamicStringFormat SET [Format] = 'Výše uvedená úroková sazba je námi garantována za podmínky schválení Žádosti o úvěr do {0} a splnění výše uvedených podmínek. Veškeré námi požadované podklady ke schválení Žádosti o úvěr nám musí být dodány v dostatečném předstihu, minimálně však 5 pracovních dnů před výše uvedeným posledním dnem garance úrokové sazby. Seznam podkladů je uveden na: www.kb.cz.' 
WHERE DynamicStringFormatId = 12

UPDATE DynamicStringFormatCondition SET EqualToValue = '' WHERE DataFieldId = 29

SET IDENTITY_INSERT [dbo].[DocumentDataField] ON 

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign])
 VALUES (25, 1, '001', 26, 'VyseSplatky', NULL, NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign])
 VALUES (45, 2, '001', 26, 'VyseSplatky', NULL, NULL, NULL)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF