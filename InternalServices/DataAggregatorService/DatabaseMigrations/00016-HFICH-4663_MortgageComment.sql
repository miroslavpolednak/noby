SET IDENTITY_INSERT [dbo].[DataField] ON 

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (161, 1, 'SalesArrangement.Mortgage.Comment', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[EasFormDataField] ON

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (98, 2, 161, 2, 'komentar')

SET IDENTITY_INSERT [dbo].[EasFormDataField] OFF