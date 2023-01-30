INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'CIS:DataAggregatorService', N'https://172.30.35.51:30020', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'CIS:DocumentGeneratorService', N'https://172.30.35.51:30014', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'CIS:NotificationService', N'https://172.30.35.51:30015', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'CIS:NotificationService', N'https://172.30.35.51:30016', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'CIS:ServiceDiscovery', N'https://172.30.35.51:30000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:CaseService', N'https://172.30.35.51:30001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:CodebookService', N'https://172.30.35.51:30003', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:CodebookService', N'https://172.30.35.51:30002', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:CustomerService', N'https://172.30.35.51:30004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:DocumentArchiveService', N'https://172.30.35.51:30005', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:HouseholdService', N'https://172.30.35.51:30018', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:OfferService', N'https://172.30.35.51:30006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:ProductService', N'https://172.30.35.51:30007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:RiskIntegrationService', N'https://172.30.35.51:30012', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:RiskIntegrationService', N'https://172.30.35.51:30013', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:SalesArrangementService', N'https://172.30.35.51:30009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:UserService', N'https://172.30.35.51:30010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:C4MCreditWorthiness:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:C4MCustomersExposure:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:C4MLoanApplication:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:C4MLoanApplicationAssessment:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:C4MRiskBusinessCase:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:C4MRiskCharakteristics:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:CustomerManagement:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:CustomerProfile:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:EAS:R21', N'https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:EasSimulationHT:V6', N'https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:ESignatures:V1', N'https://testbio.mpss.cz/ePodpisy', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:Kyc:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:MpHome:V1_1', N'https://hffatmpdigi.mpss.cz/api/1.1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:SbWebApi:V1', N'https://sb2_test_server.mpss.cz/WebApi/FAT', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:Sulm:V1', N'https://iib-sit1.kb.cz/SulmService/1/0', 3)
GO
