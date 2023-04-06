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
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:DocumentOnSAService', N'https://172.30.35.51:30021', 1)
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
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:EAS:V1', N'https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:EasSimulationHT:V1', N'https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc', 3)
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
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:Rip', N'https://rip-sit1.vsskb.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:SbWebApi:V1', N'https://sb2_test_server.mpss.cz/WebApi/FAT', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:Sulm:V1', N'https://sulm-be-v1.fat.sulm.kbcloud', 3)
GO


INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:DataAggregatorService', N'https://ds-discovery-fat.vsskb.cz:31020', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:DocumentGeneratorService', N'https://ds-discovery-fat.vsskb.cz:31014', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:NotificationService', N'https://ds-discovery-fat.vsskb.cz:31015', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:NotificationService', N'https://ds-discovery-fat.vsskb.cz:31016', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:ServiceDiscovery', N'https://ds-discovery-fat.vsskb.cz:31000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'CIS:Storage', N'https://172.30.35.51:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:CaseService', N'https://ds-discovery-fat.vsskb.cz:31001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:CodebookService', N'https://ds-discovery-fat.vsskb.cz:31003', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:CodebookService', N'https://ds-discovery-fat.vsskb.cz:31002', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:CustomerService', N'https://ds-discovery-fat.vsskb.cz:31004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:DocumentArchiveService', N'https://ds-discovery-fat.vsskb.cz:31005', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:DocumentOnSAService', N'https://ds-discovery-fat.vsskb.cz:31021', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:HouseholdService', N'https://ds-discovery-fat.vsskb.cz:31018', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:OfferService', N'https://ds-discovery-fat.vsskb.cz:31006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:ProductService', N'https://ds-discovery-fat.vsskb.cz:31007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:RiskIntegrationService', N'https://ds-discovery-fat.vsskb.cz:31012', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:RiskIntegrationService', N'https://ds-discovery-fat.vsskb.cz:31013', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:SalesArrangementService', N'https://ds-discovery-fat.vsskb.cz:31009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:UserService', N'https://ds-discovery-fat.vsskb.cz:31010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:C4MCreditWorthiness:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:C4MCustomersExposure:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:C4MLoanApplication:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:C4MLoanApplicationAssessment:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:C4MRiskBusinessCase:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:C4MRiskCharakteristics:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:CustomerManagement:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:CustomerProfile:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:EAS:R21', N'https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:EAS:V1', N'https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:EasSimulationHT:V1', N'https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:EasSimulationHT:V6', N'https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:ESignatures:V1', N'https://testbio.mpss.cz/ePodpisy', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:Kyc:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:MpHome:V1_1', N'https://hffatmpdigi.mpss.cz/api/1.1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:Rip', N'https://rip-sit1.vsskb.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:SbWebApi:V1', N'https://sb2_test_server.mpss.cz/WebApi/FAT', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:Sulm:V1', N'https://sulm-be-v1.fat.sulm.kbcloud', 3)
GO


INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:DataAggregatorService', N'https://172.30.35.51:32020', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:DocumentGeneratorService', N'https://172.30.35.51:32014', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_sit1,password=MpssSit1Pass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:NotificationService', N'https://172.30.35.51:32015', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:NotificationService', N'https://172.30.35.51:32016', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:ServiceDiscovery', N'https://172.30.35.51:32000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'CIS:Storage', N'https://172.30.35.51:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:CaseService', N'https://172.30.35.51:32001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:CodebookService', N'https://172.30.35.51:32003', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:CodebookService', N'https://172.30.35.51:32002', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:CustomerService', N'https://172.30.35.51:32004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:DocumentArchiveService', N'https://172.30.35.51:32005', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:DocumentOnSAService', N'https://172.30.35.51:32021', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:HouseholdService', N'https://172.30.35.51:32018', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:OfferService', N'https://172.30.35.51:32006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:ProductService', N'https://172.30.35.51:32007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:RiskIntegrationService', N'https://172.30.35.51:32012', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:RiskIntegrationService', N'https://172.30.35.51:32013', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:SalesArrangementService', N'https://172.30.35.51:32009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:UserService', N'https://172.30.35.51:32010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:C4MCreditWorthiness:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:C4MCustomersExposure:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:C4MLoanApplication:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:C4MLoanApplicationAssessment:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:C4MRiskBusinessCase:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:C4MRiskCharakteristics:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:CustomerManagement:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:CustomerProfile:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:EAS:R21', N'https://sb2_test_server.mpss.cz/SIT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:EAS:V1', N'https://sb2_test_server.mpss.cz/SIT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:EasSimulationHT:V1', N'https://sb2_test_server.mpss.cz/SIT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:EasSimulationHT:V6', N'https://sb2_test_server.mpss.cz/SIT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:ESignatures:V1', N'https://testbio.mpss.cz/ePodpisy', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:Kyc:V1', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:MpHome:V1_1', N'https://hfsit1mpdigi.mpss.cz/api/1.1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:Rip', N'https://rip-sit1.vsskb.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:SbWebApi:V1', N'https://sb2_test_server.mpss.cz/WebApi/SIT', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:Sulm:V1', N'https://iib-sit1.kb.cz/SulmService/1/0', 3)
GO


INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:DataAggregatorService', N'https://172.30.35.52:33020', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:DocumentGeneratorService', N'https://172.30.35.52:33014', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_uat1,password=MpssUat1Pass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:NotificationService', N'https://172.30.35.52:33015', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:NotificationService', N'https://172.30.35.52:33016', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:ServiceDiscovery', N'https://172.30.35.52:33000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'CIS:Storage', N'https://172.30.35.52:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:CaseService', N'https://172.30.35.52:33001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:CodebookService', N'https://172.30.35.52:33003', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:CodebookService', N'https://172.30.35.52:33002', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:CustomerService', N'https://172.30.35.52:33004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:DocumentArchiveService', N'https://172.30.35.52:33005', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:DocumentOnSAService', N'https://172.30.35.52:33021', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:HouseholdService', N'https://172.30.35.52:33018', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:OfferService', N'https://172.30.35.52:33006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:ProductService', N'https://172.30.35.52:33007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:RiskIntegrationService', N'https://172.30.35.52:33012', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:RiskIntegrationService', N'https://172.30.35.52:33013', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:SalesArrangementService', N'https://172.30.35.52:33009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:UserService', N'https://172.30.35.52:33010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:C4MCreditWorthiness:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:C4MCustomersExposure:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:C4MLoanApplication:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:C4MLoanApplicationAssessment:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:C4MRiskBusinessCase:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:C4MRiskCharakteristics:V1', N'https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:CustomerManagement:V1', N'https://cm-uat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:CustomerProfile:V1', N'https://cm-uat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:EAS:R21', N'https://sb2_test_server.mpss.cz/UAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:EAS:V1', N'https://sb2_test_server.mpss.cz/UAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:EasSimulationHT:V1', N'https://sb2_test_server.mpss.cz/UAT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:EasSimulationHT:V6', N'https://sb2_test_server.mpss.cz/UAT/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:ESignatures:V1', N'https://testbio.mpss.cz/ePodpisy', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.uat.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:Kyc:V1', N'https://cm-uat.kb.cz/be-cm/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:MpHome:V1_1', N'https://hfuat1mpdigi.mpss.cz/api/1.1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:Rip', N'https://rip-sit1.vsskb.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:SbWebApi:V1', N'https://sb2_test_server.mpss.cz/WebApi/UAT', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:Sulm:V1', N'https://sulm-be-v1.stage.sulm.kbcloud', 3)
GO


INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:DataAggregatorService', N'https://adpra189.vsskb.cz:38020', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:DocumentGeneratorService', N'https://adpra189.vsskb.cz:38014', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_preprod,password=xxx,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:NotificationService', N'https://adpra189.vsskb.cz:38015', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:NotificationService', N'https://adpra189.vsskb.cz:38016', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:ServiceDiscovery', N'https://adpra189.vsskb.cz:38000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'CIS:Storage', N'https://adpra189.vsskb.cz:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:CaseService', N'https://adpra189.vsskb.cz:38001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:CodebookService', N'https://adpra189.vsskb.cz:38003', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:CodebookService', N'https://adpra189.vsskb.cz:38002', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:CustomerService', N'https://adpra189.vsskb.cz:38004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:DocumentArchiveService', N'https://adpra189.vsskb.cz:38005', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:DocumentOnSAService', N'https://adpra189.vsskb.cz:38021', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:HouseholdService', N'https://adpra189.vsskb.cz:38018', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:OfferService', N'https://adpra189.vsskb.cz:38006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:ProductService', N'https://adpra189.vsskb.cz:38007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:RiskIntegrationService', N'https://adpra189.vsskb.cz:38012', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:RiskIntegrationService', N'https://adpra189.vsskb.cz:38013', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:SalesArrangementService', N'https://adpra189.vsskb.cz:38009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:UserService', N'https://adpra189.vsskb.cz:38010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:AddressWhisperer:V1', N'https://iib-uat1.kb.cz/AddressWhispererBEService/v1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:C4MCreditWorthiness:V1', N'https://stage.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:C4MCustomersExposure:V1', N'https://stage.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:C4MLoanApplication:V1', N'https://stage.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:C4MLoanApplicationAssessment:V1', N'https://stage.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:C4MRiskBusinessCase:V1', N'https://stage.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:C4MRiskCharakteristics:V1', N'https://stage.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:CustomerManagement:V1', N'https://be-cm-v1.stage.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:CustomerProfile:V1', N'https://be-cm-v1.stage.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:EAS:R21', N'https://sb2-preprod.vsskb.cz/PREPROD/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:EAS:V1', N'https://sb2-preprod.vsskb.cz/PREPROD/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:EasSimulationHT:V1', N'https://sb2-preprod.vsskb.cz/PREPROD/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:EasSimulationHT:V6', N'https://sb2-preprod.vsskb.cz/PREPROD/HT_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:ESignatures:V1', N'https://testbio.mpss.cz/ePodpisy', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:IdentifiedSubjectBr:V1', N'https://cm-identified-subject-br-v1.uat.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:Kyc:V1', N'https://be-cm-v1.stage.custmng.kbcloud/api', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:MpHome:V1_1', N'https://hfpreprodmpdigi.mpss.cz/api/1.1', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:Rip', N'https://rip-preprod.vsskb.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:SbWebApi:V1', N'https://sb2-preprod.vsskb.cz/WebApi/PREPROD', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:Sdf:V1', N'https://adpra043.vsskb.cz/SDF/ExtendedServices.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:Sulm:V1', N'https://sulm-be-v1.stage.sulm.kbcloud', 3)
GO