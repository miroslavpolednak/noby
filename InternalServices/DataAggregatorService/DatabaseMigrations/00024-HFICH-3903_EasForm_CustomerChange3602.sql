ALTER TABLE EasFormDynamicInputParameter ADD EasFormTypeId INT NULL, FOREIGN KEY(EasFormTypeId) REFERENCES EasFormType(EasFormTypeId)

GO

UPDATE EasFormDynamicInputParameter SET EasFormTypeId = 1 WHERE EasRequestTypeId = 1
UPDATE EasFormDynamicInputParameter SET EasFormTypeId = 2 WHERE EasRequestTypeId = 2

GO

ALTER TABLE EasFormDynamicInputParameter ALTER COLUMN EasFormTypeId INT NOT NULL

GO

ALTER TABLE EasFormDynamicInputParameter DROP CONSTRAINT PK_EasFormDynamicInputParameter

ALTER TABLE EasFormDynamicInputParameter ADD PRIMARY KEY (EasRequestTypeId, EasFormTypeId, InputParameterId, TargetDataServiceId)

GO

INSERT INTO EasFormDataField ([EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName])
SELECT 1 as EasRequestTypeId, DataFieldId, 3 as EasFormTypeId, JsonPropertyName FROM EasFormDataField WHERE EasRequestTypeId = 2 AND EasFormTypeId = 3

INSERT INTO EasFormSpecialDataField ([EasRequestTypeId], [JsonPropertyName], [DataServiceId], [EasFormTypeId], [FieldPath])
SELECT 1 as EasRequestTypeId, JsonPropertyName, DataServiceId, 3 as EasFormTypeId, FieldPath FROM EasFormSpecialDataField WHERE EasRequestTypeId = 2 AND EasFormTypeId = 3

DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'zmenovy_navrh'

INSERT INTO EasFormSpecialDataField VALUES (1, 'zmenovy_navrh', 1, 3, 'ChangeProposal')
INSERT INTO EasFormSpecialDataField VALUES (1, 'seznam_ucastniku[].klient.manzel_v_dluhu_je', 1, 3, 'HouseholdData.Customers[].IsSpouseInDebt')

INSERT [dbo].[EasFormDynamicInputParameter] ([EasRequestTypeId], [EasFormTypeId], [InputParameterId], [TargetDataServiceId], [SourceDataFieldId]) VALUES (1, 3, 3, 3, 2)