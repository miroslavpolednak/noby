USE [CIS]
GO

TRUNCATE TABLE [dbo].[ServiceDiscovery]

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'CIS:DataAggregatorService', N'https://ds-dataaggregator-sit1.vsskb.cz:32020', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'CIS:DocumentGeneratorService', N'https://ds-documentgenerator-sit1.vsskb.cz:32014', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_sit1,password=MpssSit1Pass,allowAdmin=false,tieBreaker=', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'CIS:NotificationService', N'https://ds-notification-sit1.vsskb.cz:32015', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'CIS:NotificationService', N'https://ds-notification-sit1.vsskb.cz:32016', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'CIS:ServiceDiscovery', N'https://ds-discovery-sit1.vsskb.cz:32000', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:CaseService', N'https://ds-case-sit1.vsskb.cz:32001', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:CodebookService', N'https://ds-codebook-sit1.vsskb.cz:32003', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:CodebookService', N'https://ds-codebook-sit1.vsskb.cz:32002', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:CustomerService', N'https://ds-customer-sit1.vsskb.cz:32004', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:DocumentArchiveService', N'https://ds-documentarchive-sit1.vsskb.cz:32005', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:DocumentArchiveService', N'https://ds-documentarchive-sit1.vsskb.cz:32017', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:DocumentOnSAService', N'https://ds-documentonsa-sit1.vsskb.cz:32019', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:HouseholdService', N'https://ds-household-sit1.vsskb.cz:32018', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:OfferService', N'https://ds-offer-sit1.vsskb.cz:32006', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:ProductService', N'https://ds-product-sit1.vsskb.cz:32007', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:RealEstateValuationService', N'https://ds-realestatevaluation-sit1.vsskb.cz:32021', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:RiskIntegrationService', N'https://ds-riskintegration-sit1.vsskb.cz:32012', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:RiskIntegrationService', N'https://ds-riskintegration-sit1.vsskb.cz:32013', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:SalesArrangementService', N'https://ds-salesarrangement-sit1.vsskb.cz:32009', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'DS:UserService', N'https://ds-user-sit1.vsskb.cz:32010', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:AcvEnumService:V1', N'https://api.fat.car.kbcloud/v1/enums', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:C4MCreditWorthiness:V3', N'https://uat.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:C4MCustomerExposure:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:C4MLoanApplication:V3', N'https://uat.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:C4MLoanApplicationAssessment:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:C4MRiskBusinessCase:V3', N'https://uat.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:C4MRiskCharacteristics:V2', N'https://uat.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:Contacts:V1', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:Crem:V1', N'https://api.fat.crem.kbcloud/v1/deed-of-ownership/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:CustomerAddressService:V2', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:CustomerManagement:V2', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:CustomerProfile:V1', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:EAS:V1', N'https://sb2-test-server.vsskb.cz/SIT/EAS_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:EasSimulationHT:V1', N'https://sb2-test-server.vsskb.cz/SIT/HT_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:ESignatures:V1', N'https://sitepodpisy.mpss.cz/WS', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:Kyc:V1', N'https://cm-be-v1.fat.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:LuxpiService:V1', N'https://kblux-test.dslab.kb.cz/kblux', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:MpHome:V1', N'https://hfsit1mpdigi.mpss.cz/api/1.1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:Pcp:V1', N'https://iib-fatbs.kb.cz/services/ProductInstanceBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:PreorderService:V1', N'https://api.fat.car.kbcloud/v1/preorder', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:RDM:V1', N'https://codebooks-dev.kb.cz/int-codebooks-rest/api/v3', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:RuianAddress:V1', N'https://api.fat.crem.kbcloud/v1/ruian/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:SbWebApi:V1', N'https://sb2-test-server.vsskb.cz/WebApi/SIT', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'SIT1', N'ES:Sulm:V1', N'https://sulm-be-v1.fat.sulm.kbcloud', 3, 0)

