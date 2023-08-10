SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT INTO [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (214, 4, 'User.IsExternal', NULL)
INSERT INTO [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (215, 4, 'StaticValues.DefaultOneValue', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[EasFormDataField] ON

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (120, 2, 214, 2, 'zprostredkovano_3_stranou')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (121, 2, 215, 2, 'forma_splaceni')

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (122, 1, 215, 1, 'podpis_zadatele')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (123, 1, 215, 1, 'ucet_splaceni.zpusob_splaceni')

SET IDENTITY_INSERT [dbo].[EasFormDataField] OFF

DELETE FROM EasFormSpecialDataField WHERE FieldPath LIKE '%MockValues%'