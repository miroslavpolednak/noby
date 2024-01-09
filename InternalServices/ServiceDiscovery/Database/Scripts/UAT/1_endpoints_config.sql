USE [CIS]
GO

TRUNCATE TABLE [dbo].[ServiceDiscovery]

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'CIS:DataAggregatorService', N'https://172.30.35.52:33020', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'CIS:DocumentGeneratorService', N'https://172.30.35.52:33014', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_uat1,password=MpssUat1Pass,allowAdmin=false,tieBreaker=', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'CIS:NotificationService', N'https://172.30.35.52:33015', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'CIS:NotificationService', N'https://172.30.35.52:33016', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'CIS:ServiceDiscovery', N'https://172.30.35.52:33000', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:CaseService', N'https://172.30.35.52:33001', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:CodebookService', N'https://172.30.35.52:33003', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:CodebookService', N'https://172.30.35.52:33002', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:CustomerService', N'https://172.30.35.52:33004', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:DocumentArchiveService', N'https://172.30.35.52:33005', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:DocumentOnSAService', N'https://172.30.35.52:33019', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:HouseholdService', N'https://172.30.35.52:33018', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:OfferService', N'https://172.30.35.52:33006', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:ProductService', N'https://172.30.35.52:33007', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:RealEstateValuationService', N'https://172.30.35.52:33021', 1, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:RiskIntegrationService', N'https://172.30.35.52:33012', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:RiskIntegrationService', N'https://172.30.35.52:33013', 2, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:SalesArrangementService', N'https://172.30.35.52:33009', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'DS:UserService', N'https://172.30.35.52:33010', 1, 1)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:AcvEnumService:V1', N'https://api.stage.car.kbcloud/v1/enums', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:C4MCreditWorthiness:V3', N'https://stage.risk-loan-assessment.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:C4MCustomerExposure:V3', N'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-exposure-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:C4MLoanApplication:V3', N'https://stage.risk-loan-assessment.kbcloud/la-loan-application-hf-adapter-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:C4MLoanApplicationAssessment:V3', N'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-assessment-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:C4MRiskBusinessCase:V3', N'https://stage.risk-loan-assessment.kbcloud/laa-risk-business-case-service-3/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:C4MRiskCharacteristics:V2', N'https://stage.risk-loan-assessment.kbcloud/laa-loan-application-risk-chars-calculation-service-2/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:Contacts:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:Crem:V1', N'https://api.stage.crem.kbcloud/v1/deed-of-ownership/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:CustomerAddressService:V2', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:CustomerManagement:V1', N'https://cm-uat.kb.cz/be-cm/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:CustomerManagement:V2', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:CustomerProfile:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:EAS:V1', N'https://sb2-test-server.vsskb.cz/UAT/EAS_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:EasSimulationHT:V1', N'https://sb2-test-server.vsskb.cz/UAT/HT_WS_SB_Services.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:ESignatures:V1', N'https://uatepodpisy.mpss.cz/WS', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:Kyc:V1', N'https://cm-be-v1.stage.custmng.kbcloud/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:LuxpiService:V1', N'https://acvapi-uat3.dslab.kb.cz/kblux', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:MpHome:V1', N'https://hfuatmpdigi.mpss.cz/api/1.1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:Pcp:V1', N'https://iib-uat1.kb.cz/services/ProductInstanceBEService/v1', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:PreorderService:V1', N'https://api.stage.car.kbcloud/v1/preorder', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:RDM:V1', N'https://codebooks-dev.kb.cz/int-codebooks-rest/api/v3', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:RuianAddress:V1', N'https://api.stage.crem.kbcloud/v1/ruian/api', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:SbWebApi:V1', N'https://sb2-test-server.vsskb.cz/WebApi/UAT', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'UAT', N'ES:Sulm:V1', N'https://sulm-be-v1.stage.sulm.kbcloud', 3, 0)

