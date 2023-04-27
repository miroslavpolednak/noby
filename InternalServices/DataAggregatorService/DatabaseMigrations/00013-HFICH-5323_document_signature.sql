SET IDENTITY_INSERT [dbo].[DataField] ON

INSERT [dbo].[DataField] ([DataFieldId], [DataServiceId], [FieldPath], [DefaultStringFormat]) VALUES (162, 4, 'User.FullNameWithDetails', NULL)

SET IDENTITY_INSERT [dbo].[DataField] OFF

SET IDENTITY_INSERT [dbo].[DocumentDataField] ON 

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (165, 10, '001', 162, 'ZadostPrijal', 'Tuto žádost přijal {0}', NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (166, 8, '001', 162, 'ZadostPrijal', 'Tuto žádost přijal {0}', NULL, NULL)

INSERT [dbo].[DocumentDataField] ([DocumentDataFieldId], [DocumentId], [DocumentVersion], [DataFieldId], [AcroFieldName], [StringFormat], [DefaultTextIfNull], [TextAlign]) 
VALUES (167, 9, '001', 162, 'ZadostPrijal', 'Tuto žádost přijal {0}', NULL, NULL)

SET IDENTITY_INSERT [dbo].[DocumentDataField] OFF

INSERT INTO DocumentSpecialDataField VALUES (10, 'PodpisJmenoKlienta', 5, 'FullName', NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (8, 'PodpisJmenoKlienta', 5, 'FullName', NULL, NULL)