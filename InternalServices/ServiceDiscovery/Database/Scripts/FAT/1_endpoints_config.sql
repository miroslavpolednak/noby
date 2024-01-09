USE [CIS]
GO

TRUNCATE TABLE [dbo].[ServiceDiscovery]

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'CIS:DataAggregatorService', N'https://ds-dataaggregator-fat.vsskb.cz:31020', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'CIS:DocumentGeneratorService', N'https://ds-documentgenerator-fat.vsskb.cz:31014', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'CIS:NotificationService', N'https://ds-notification-fat.vsskb.cz:31015', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'CIS:NotificationService', N'https://ds-notification-fat.vsskb.cz:31016', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'CIS:ServiceDiscovery', N'https://ds-discovery-fat.vsskb.cz:31000', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:CaseService', N'https://ds-case-fat.vsskb.cz:31001', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:CodebookService', N'https://ds-codebook-fat.vsskb.cz:31003', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:CodebookService', N'https://ds-codebook-fat.vsskb.cz:31002', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:CustomerService', N'https://ds-customer-fat.vsskb.cz:31004', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:DocumentArchiveService', N'https://ds-documentarchive-fat.vsskb.cz:31005', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:DocumentArchiveService', N'https://ds-documentarchive-fat.vsskb.cz:31017', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:DocumentOnSAService', N'https://ds-documentonsa-fat.vsskb.cz:31019', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:HouseholdService', N'https://ds-household-fat.vsskb.cz:31018', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:OfferService', N'https://ds-offer-fat.vsskb.cz:31006', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:ProductService', N'https://ds-product-fat.vsskb.cz:31007', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:RealEstateValuationService', N'https://ds-realestatevaluation-fat.vsskb.cz:31021', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:RiskIntegrationService', N'https://ds-riskintegration-fat.vsskb.cz:31012', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:RiskIntegrationService', N'https://ds-riskintegration-fat.vsskb.cz:31013', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:SalesArrangementService', N'https://ds-salesarrangement-fat.vsskb.cz:31009', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'DS:UserService', N'https://ds-user-fat.vsskb.cz:31010', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:AcvEnumService:V1', N'https://api.fat.car.kbcloud/v1/enums', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:C4MCreditWorthiness:V3', N'https://uat.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:C4MCustomerExposure:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:C4MLoanApplication:V3', N'https://uat.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:C4MLoanApplicationAssessment:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:C4MRiskBusinessCase:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:C4MRiskCharacteristics:V2', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:Contacts:V1', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:Crem:V1', N'https://api.fat.crem.kbcloud/v1/deed-of-ownership/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:CustomerAddressService:V2', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:CustomerManagement:V2', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:CustomerProfile:V1', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:EAS:V1', N'https://sb2-test-server.vsskb.cz/FAT/EAS_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:EasSimulationHT:V1', N'https://sb2-test-server.vsskb.cz/FAT/HT_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:ESignatures:V1', N'https://fatepodpisy.mpss.cz/WS', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:Kyc:V1', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:LuxpiService:V1', N'https://kblux-test.dslab.kb.cz/kblux', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:MpHome:V1', N'https://hffatmpdigi.mpss.cz/api/1.1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:Pcp:V1', N'https://iib-fatbs.kb.cz/services/ProductInstanceBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:PreorderService:V1', N'https://api.fat.car.kbcloud/v1/preorder', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:RDM:V1', N'https://codebooks-dev.kb.cz/int-codebooks-rest/api/v3', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:RuianAddress:V1', N'https://api.fat.crem.kbcloud/v1/ruian/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:SbWebApi:V1', N'https://sb2-test-server.vsskb.cz/WebApi/FAT', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'FAT', N'ES:Sulm:V1', N'https://sulm-be-v1.fat.sulm.kbcloud', 3, 0)

