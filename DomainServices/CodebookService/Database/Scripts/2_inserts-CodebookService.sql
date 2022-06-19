USE [CodebookService]
GO

INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [RDMCode]) VALUES (1, N'A')
GO
INSERT [dbo].[IdentificationDocumentTypeExtension] ([IdentificationDocumentTypeId], [RDMCode]) VALUES (2, N'B')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (1, N'S')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (2, N'M')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (3, N'D')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (4, N'W')
GO
INSERT [dbo].[MaritalStatusExtension] ([MaritalStatusId], [RDMCode]) VALUES (6, N'R')
GO
INSERT [dbo].[ProductTypeExtension] ([ProductTypeId], [ProductCategory], [MpHomeApiLoanType], [KonsDbLoanType]) VALUES (20001, 3, N'KBMortgage', 3)
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (0, N'NotSpecified')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (1, N'Owner')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (2, N'CoDebtor')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (3, N'Accessor')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (4, N'HusbandOrWife')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (5, N'LegalRepresentative')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (6, N'CollisionGuardian')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (7, N'Guardian')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (8, N'Guarantor')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (9, N'GuarantorHusbandOrWife')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (11, N'Intermediary')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (12, N'ManagingDirector')
GO
INSERT [dbo].[RelationshipCustomerProductTypeExtension] ([RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType]) VALUES (13, N'Child')
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [IsDefault]) VALUES (1, N'Žádost o hypotéèní úvìr', 20001, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [IsDefault]) VALUES (2, N'Žádost o americkou hypotéku', 20010, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [IsDefault]) VALUES (3, N'Žádost o doprodej neúèelové èásti', 20004, 1)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [IsDefault]) VALUES (4, N'Servisní žádost X', NULL, 0)
GO
INSERT [dbo].[SalesArrangementType] ([Id], [Name], [ProductTypeId], [IsDefault]) VALUES (5, N'Servisní žádost Y', NULL, 0)
GO

GO
