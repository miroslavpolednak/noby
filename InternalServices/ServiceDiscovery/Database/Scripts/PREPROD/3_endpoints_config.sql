USE [CIS]
GO

TRUNCATE TABLE [dbo].[ServiceDiscovery]

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'CIS:DataAggregatorService', N'https://ds-dataaggregator-preprod.vsskb.cz:38020', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'CIS:DocumentGeneratorService', N'https://ds-documentgenerator-preprod.vsskb.cz:38014', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'CIS:GlobalCache:Redis', N'', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'CIS:NotificationService', N'https://ds-notification-preprod.vsskb.cz:38015', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'CIS:NotificationService', N'https://ds-notification-preprod.vsskb.cz:38016', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'CIS:ServiceDiscovery', N'https://ds-discovery-preprod.vsskb.cz:38000', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:CaseService', N'https://ds-case-preprod.vsskb.cz:38001', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:CodebookService', N'https://ds-codebook-preprod.vsskb.cz:38003', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:CodebookService', N'https://ds-codebook-preprod.vsskb.cz:38002', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:CustomerService', N'https://ds-customer-preprod.vsskb.cz:38004', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:DocumentArchiveService', N'https://ds-documentarchive-preprod.vsskb.cz:38005', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:DocumentArchiveService', N'https://ds-documentarchive-preprod.vsskb.cz:38017', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:DocumentOnSAService', N'https://ds-documentonsa-preprod.vsskb.cz:38019', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:HouseholdService', N'https://ds-household-preprod.vsskb.cz:38018', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:OfferService', N'https://ds-offer-preprod.vsskb.cz:38006', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:ProductService', N'https://ds-product-preprod.vsskb.cz:38007', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:RealEstateValuationService', N'https://ds-realestatevaluation-preprod.vsskb.cz:38021', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:RiskIntegrationService', N'https://ds-riskintegration-preprod.vsskb.cz:38012', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:RiskIntegrationService', N'https://ds-riskintegration-preprod.vsskb.cz:38013', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:SalesArrangementService', N'https://ds-salesarrangement-preprod.vsskb.cz:38009', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'DS:UserService', N'https://ds-user-preprod.vsskb.cz:38010', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:AcvEnumService:V1', N'https://api.stage.car.kbcloud/v1/enums', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:C4MCreditWorthiness:V3', N'https://stage.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:C4MCustomerExposure:V3', N'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:C4MLoanApplication:V3', N'https://stage.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:C4MLoanApplicationAssessment:V3', N'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:C4MRiskBusinessCase:V3', N'https://stage.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:C4MRiskCharacteristics:V2', N'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Contacts:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Crem:V1', N'https://api.fat.crem.kbcloud/v1/deed-of-ownership/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:CustomerAddressService:V2', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:CustomerManagement:V2', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:CustomerProfile:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:EAS:R21', N'https://sb2-preprod.vsskb.cz/PREPROD/EAS_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:EAS:V1', N'https://sb2-preprod.vsskb.cz/PREPROD/EAS_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:EasSimulationHT:V1', N'https://sb2-preprod.vsskb.cz/PREPROD/HT_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:EasSimulationHT:V6', N'https://sb2-preprod.vsskb.cz/PREPROD/HT_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:ESignatures:V1', N'https://preprodepodpisy.mpss.cz/WS', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.uat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Kyc:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:LuxpiService:V1', N'https://kblux-test.dslab.kb.cz/kblux', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:MpHome:V1', N'https://hfpreprodmpdigi.mpss.cz/api/1.1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Pcp:V1', N'https://iib-uat1.kb.cz/services/ProductInstanceBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Pcp:V2', N'https://be-productinstanceservice-v1.stage.pcp-mdm.kbcloud/services/ProductInstanceBEService/v2', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:PreorderService:V1', N'https://api.stage.car.kbcloud/v1/preorder', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:RDM:V1', N'https://codebooks-uat.kb.cz/int-codebooks-rest/api/v3', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:RuianAddress:V1', N'https://api.fat.crem.kbcloud/v1/ruian/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:SbWebApi:V1', N'https://sb2-preprod.vsskb.cz/WebApi/PREPROD', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Sdf:V1', N'https://adpra021.vsskb.cz/SDF/ExtendedServices.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Sulm:V1', N'https://sulm-be-v1.stage.sulm.kbcloud', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Party:V1', N'https://partygeneral-v1.stage.prs.kbcloud/services/PartyGeneralBEService', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PREPROD', N'ES:Pcp:V3', N'https://be-productinstanceservice-v2.stage.pcp-mdm.kbcloud/services/ProductInstanceBEService/v2', 3, 0)

