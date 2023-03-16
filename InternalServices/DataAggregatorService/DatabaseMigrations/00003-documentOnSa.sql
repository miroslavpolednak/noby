INSERT INTO DataService VALUES (12, 'DocumentOnSa')

SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT INTO DataField VALUES (151, 12, 'Custom.DocumentOnSa.FinalDocument.FormId', NULL)
INSERT INTO DataField VALUES (152, 12, 'Custom.DocumentOnSa.FinalDocument.FormIdList[].FormId', NULL)
INSERT INTO DataField VALUES (153, 12, 'Custom.DocumentOnSa.FinalDocument.EArchivId', NULL)
INSERT INTO DataField VALUES (154, 12, 'Custom.DocumentOnSa.FinalDocument.SignatureMethodId', NULL)
INSERT INTO DataField VALUES (155, 12, 'Custom.DocumentOnSa.FinalDocument.FirstSignatureDate', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[EasFormDataField] ON

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (77, 1, 151, 1, N'business_id_formulare')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (79, 2, 151, 2, N'business_id_formulare')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (80, 2, 151, 3, N'business_id_formulare')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (81, 2, 152, 2, N'seznam_id_formulare[].id_formulare')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (82, 2, 152, 3, N'seznam_id_formulare[].id_formulare')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (83, 1, 153, 1, N'cislo_dokumentu')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (84, 2, 153, 2, N'cislo_dokumentu')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (85, 2, 153, 3, N'cislo_dokumentu')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (86, 1, 154, 1, N'zpusob_podpisu_zadosti')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (87, 2, 154, 2, N'zpusob_podpisu_zadosti')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (91, 2, 155, 2, N'datum_prvniho_podpisu')

SET IDENTITY_INSERT [dbo].[EasFormDataField] OFF

DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'business_id_formulare'
DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'seznam_id_formulare[].id_formulare'
DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'cislo_dokumentu'
DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'zpusob_podpisu_zadosti'
DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'datum_prvniho_podpisu'