SET IDENTITY_INSERT [dbo].[DataField] ON 

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (159, 4, 'User.CPM', NULL)
INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (160, 4, 'User.ICP', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[EasFormDataField] ON

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (92, 1, 159, 1, 'sjednal_CPM')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (93, 1, 160, 1, 'sjednal_ICP')

INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (94, 2, 159, 2, 'sjednal_CPM')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (95, 2, 159, 3, 'sjednal_CPM')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (96, 2, 160, 2, 'sjednal_ICP')
INSERT [dbo].[EasFormDataField] ([EasFormDataFieldId], [EasRequestTypeId], [DataFieldId], [EasFormTypeId], [JsonPropertyName]) VALUES (97, 2, 160, 3, 'sjednal_ICP')

SET IDENTITY_INSERT [dbo].[EasFormDataField] OFF

DELETE FROM EasFormSpecialDataField WHERE JsonPropertyName IN ('sjednal_CPM', 'sjednal_ICP')

UPDATE EasFormSpecialDataField SET FieldPath = 'DefaultValues3601.EaCodeMainId' WHERE JsonPropertyName = 'ea_kod' AND EasFormTypeId = 2
UPDATE EasFormSpecialDataField SET FieldPath = 'DefaultValues3602.EaCodeMainId' WHERE JsonPropertyName = 'ea_kod' AND EasFormTypeId = 3