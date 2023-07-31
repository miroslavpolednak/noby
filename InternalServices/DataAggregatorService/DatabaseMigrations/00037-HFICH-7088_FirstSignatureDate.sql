SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT INTO [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (213, 1, 'SalesArrangement.Mortgage.FirstSignatureDate', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

UPDATE EasFormDataField SET DataFieldId = 213 WHERE EasFormDataFieldId = 91