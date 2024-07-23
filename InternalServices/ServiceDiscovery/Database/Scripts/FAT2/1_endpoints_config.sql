USE [CIS]
GO

TRUNCATE TABLE [dbo].[ServiceDiscovery]

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'CIS:DataAggregatorService', N'https://ds-dataaggregator-fat2.vsskb.cz:31120', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'CIS:DocumentGeneratorService', N'https://ds-documentgenerator-fat2.vsskb.cz:31114', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat2,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'CIS:NotificationService', N'https://ds-notification-fat2.vsskb.cz:31115', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'CIS:NotificationService', N'https://ds-notification-fat2.vsskb.cz:31116', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'CIS:ServiceDiscovery', N'https://ds-discovery-fat2.vsskb.cz:31100', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:CaseService', N'https://ds-case-fat2.vsskb.cz:31101', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:CodebookService', N'https://ds-codebook-fat2.vsskb.cz:31103', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:CodebookService', N'https://ds-codebook-fat2.vsskb.cz:31102', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:CustomerService', N'https://ds-customer-fat2.vsskb.cz:31104', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:DocumentArchiveService', N'https://ds-documentarchive-fat2.vsskb.cz:31105', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:DocumentArchiveService', N'https://ds-documentarchive-fat2.vsskb.cz:31117', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:DocumentOnSAService', N'https://ds-documentonsa-fat2.vsskb.cz:31119', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:HouseholdService', N'https://ds-household-fat2.vsskb.cz:31118', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:OfferService', N'https://ds-offer-fat2.vsskb.cz:31106', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:ProductService', N'https://ds-product-fat2.vsskb.cz:31107', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:RealEstateValuationService', N'https://ds-realestatevaluation-fat2.vsskb.cz:31121', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:RiskIntegrationService', N'https://ds-riskintegration-fat2.vsskb.cz:31112', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:RiskIntegrationService', N'https://ds-riskintegration-fat2.vsskb.cz:31113', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:SalesArrangementService', N'https://ds-salesarrangement-fat2.vsskb.cz:31109', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'DS:UserService', N'https://ds-user-fat2.vsskb.cz:31110', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:AcvEnumService:V1', N'https://api.stage.car.kbcloud/v1/enums', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:C4MCreditWorthiness:V3', N'https://uat.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:C4MCustomerExposure:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:C4MLoanApplication:V3', N'https://uat.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:C4MLoanApplicationAssessment:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:C4MRiskBusinessCase:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:C4MRiskCharacteristics:V2', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Contacts:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Crem:V1', N'https://api.stage.crem.kbcloud/v1/deed-of-ownership/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:CustomerAddressService:V2', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:CustomerManagement:V2', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:CustomerProfile:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:EAS:V1', N'https://sb2-test-server.vsskb.cz/FAT2/EAS_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:EasSimulationHT:V1', N'https://sb2-test-server.vsskb.cz/FAT2/HT_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:ESignatures:V1', N'https://fat2epodpisy.mpss.cz/WS', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Kyc:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:LuxpiService:V1', N'https://kblux-test.dslab.kb.cz/kblux', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:MpHome:V1', N'https://fat2mpdigi.mpss.cz/api/1.1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Pcp:V1', N'https://iib-fatbs.kb.cz/services/ProductInstanceBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Pcp:V2', N'https://be-productinstanceservice-v1.stage.pcp-mdm.kbcloud/services/ProductInstanceBEService/v2', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:PreorderService:V1', N'https://api.stage.car.kbcloud/v1/preorder', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:RDM:V1', N'https://codebooks-uat.kb.cz/int-codebooks-rest/api/v3', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:RuianAddress:V1', N'https://api.stage.crem.kbcloud/v1/ruian/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:SbWebApi:V1', N'https://sb2-test-server.vsskb.cz/WebApi/FAT2', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Sulm:V1', N'https://sulm-be-v1.stage.sulm.kbcloud', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Party:V1', N'https://partygeneral-v1.stage.prs.kbcloud/services/PartyGeneralBEService', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT2', N'ES:Pcp:V3', N'https://be-productinstanceservice-v2.stage.pcp-mdm.kbcloud/services/ProductInstanceBEService/v2', 3, 0)