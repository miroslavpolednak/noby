
UPDATE FormValidationTransformation SET AlterSeverity = 2 WHERE Id = 215
UPDATE FormValidationTransformation SET AlterSeverity = 0 WHERE Id = 238

SET IDENTITY_INSERT [dbo].[FormValidationTransformation] ON 

INSERT [dbo].[FormValidationTransformation] ([Id], [FormId], [FieldPath], [Category], [CategoryOrder], [Text], [AlterSeverity]) 
VALUES (463, '3700001', 'business_id_formulare', 'Čerpání - Ostatní', 5, 'Identifikátor formuláře žádosti (FORMID)', 2)

SET IDENTITY_INSERT [dbo].[FormValidationTransformation] OFF