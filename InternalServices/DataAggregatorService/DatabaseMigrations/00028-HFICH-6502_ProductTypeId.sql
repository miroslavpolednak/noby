INSERT INTO EasFormDynamicInputParameter VALUES (2, 2, 2, 1, 2)

DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName = 'uv_produkt'

SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (194, 2, 'Case.Data.ProductTypeId', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[EasFormDataField] ON

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (119, 2, 194, 2, 'uv_produkt')

SET IDENTITY_INSERT [dbo].[EasFormDataField] OFF