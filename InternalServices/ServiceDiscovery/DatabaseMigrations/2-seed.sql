INSERT INTO dbo.ServiceDiscovery2 ([EnvironmentName],[ServiceName],[ServiceUrl],[ServiceType]) VALUES
('DEV','CIS:DocumentGeneratorService','https://172.30.35.51:30014',1),
('DEV','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=',3),
('DEV','CIS:NotificationService','https://172.30.35.51:30015',1),
('DEV','CIS:NotificationService','https://172.30.35.51:30016',2),
('DEV','CIS:ServiceDiscovery','https://172.30.35.51:30000',1),
('DEV','DS:CaseService','https://172.30.35.51:30001',1),
('DEV','DS:CodebookService','https://172.30.35.51:30003',1),
('DEV','DS:CodebookService','https://172.30.35.51:30002',2),
('DEV','DS:CustomerService','https://172.30.35.51:30004',1),
('DEV','DS:DocumentArchiveService','https://172.30.35.51:30005',1),
('DEV','DS:HouseholdService','https://172.30.35.51:30018',1),
('DEV','DS:OfferService','https://172.30.35.51:30006',1),
('DEV','DS:ProductService','https://172.30.35.51:30007',1),
('DEV','DS:RiskIntegrationService','https://172.30.35.51:30012',1),
('DEV','DS:RiskIntegrationService','https://172.30.35.51:30013',2),
('DEV','DS:SalesArrangementService','https://172.30.35.51:30009',1),
('DEV','DS:UserService','https://172.30.35.51:30010',1),
('DEV','ES:AddressWhisperer:V1','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('DEV','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('DEV','ES:C4MCustomersExposure:V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('DEV','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('DEV','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('DEV','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('DEV','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('DEV','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('DEV','ES:CustomerManagement:V1','https://cm-fat.kb.cz/be-cm/api',3),
('DEV','ES:CustomerProfile:V1','https://cm-fat.kb.cz/be-cm/api',3),
('DEV','ES:EAS:R21','https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc',3),
('DEV','ES:EasSimulationHT:V6','https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc',3),
('DEV','ES:ESignatures:V1','https://testbio.mpss.cz/ePodpisy',3),
('DEV','ES:IdentifiedSubjectBr:V1','https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api/',3),
('DEV','ES:MpHome:V1_1','https://hffatmpdigi.mpss.cz/api/1.1',3),
('DEV','ES:SbWebApi:V1','https://sb2_test_server.mpss.cz/WebApi/FAT',3),
('DEV','ES:Sdf:V1','https://adpra043.vsskb.cz/SDF/ExtendedServices.svc',3),
('DEV','ES:Sulm:V1','https://iib-sit1.kb.cz/SulmService/1/0',3);
GO

INSERT INTO dbo.ServiceDiscovery2 ([EnvironmentName],[ServiceName],[ServiceUrl],[ServiceType]) VALUES
('FAT','CIS:DocumentGeneratorService','https://172.30.35.51:31014',1),
('FAT','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=',3),
('FAT','CIS:NotificationService','https://172.30.35.51:31015',1),
('FAT','CIS:NotificationService','https://172.30.35.51:31016',2),
('FAT','CIS:ServiceDiscovery','https://172.30.35.51:31000',1),
('FAT','DS:CaseService','https://172.30.35.51:31001',1),
('FAT','DS:CodebookService','https://172.30.35.51:31003',1),
('FAT','DS:CodebookService','https://172.30.35.51:31002',2),
('FAT','DS:CustomerService','https://172.30.35.51:31004',1),
('FAT','DS:DocumentArchiveService','https://172.30.35.51:31005',1),
('FAT','DS:HouseholdService','https://172.30.35.51:31018',1),
('FAT','DS:OfferService','https://172.30.35.51:31006',1),
('FAT','DS:ProductService','https://172.30.35.51:31007',1),
('FAT','DS:RiskIntegrationService','https://172.30.35.51:31012',1),
('FAT','DS:RiskIntegrationService','https://172.30.35.51:31013',2),
('FAT','DS:SalesArrangementService','https://172.30.35.51:31009',1),
('FAT','DS:UserService','https://172.30.35.51:31010',1),
('FAT','ES:AddressWhisperer:V1','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('FAT','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('FAT','ES:C4MCustomersExposure:V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('FAT','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('FAT','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('FAT','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('FAT','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('FAT','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('FAT','ES:CustomerManagement:V1','https://cm-fat.kb.cz/be-cm/api',3),
('FAT','ES:CustomerProfile:V1','https://cm-fat.kb.cz/be-cm/api',3),
('FAT','ES:EAS:R21','https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc',3),
('FAT','ES:EasSimulationHT:V6','https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc',3),
('FAT','ES:ESignatures:V1','',3),
('FAT','ES:IdentifiedSubjectBr:V1','https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api/',3),
('FAT','ES:MpHome:V1_1','https://hffatmpdigi.mpss.cz/api/1.1',3),
('FAT','ES:SbWebApi:V1','https://sb2_test_server.mpss.cz/WebApi/FAT',3),
('FAT','ES:Sdf:V1','',3),
('FAT','ES:Sulm:V1','https://iib-sit1.kb.cz/SulmService/1/0',3);
GO

INSERT INTO dbo.ServiceDiscovery2 ([EnvironmentName],[ServiceName],[ServiceUrl],[ServiceType]) VALUES
('SIT1','CIS:DocumentGeneratorService','https://172.30.35.51:32014',1),
('SIT1','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=',3),
('SIT1','CIS:NotificationService','https://172.30.35.51:32015',1),
('SIT1','CIS:NotificationService','https://172.30.35.51:32016',2),
('SIT1','CIS:ServiceDiscovery','https://172.30.35.51:32000',1),
('SIT1','DS:CaseService','https://172.30.35.51:32001',1),
('SIT1','DS:CodebookService','https://172.30.35.51:32003',1),
('SIT1','DS:CodebookService','https://172.30.35.51:32002',2),
('SIT1','DS:CustomerService','https://172.30.35.51:32004',1),
('SIT1','DS:DocumentArchiveService','https://172.30.35.51:32005',1),
('SIT1','DS:HouseholdService','https://172.30.35.51:32018',1),
('SIT1','DS:OfferService','https://172.30.35.51:32006',1),
('SIT1','DS:ProductService','https://172.30.35.51:32007',1),
('SIT1','DS:RiskIntegrationService','https://172.30.35.51:32012',1),
('SIT1','DS:RiskIntegrationService','https://172.30.35.51:32013',2),
('SIT1','DS:SalesArrangementService','https://172.30.35.51:32009',1),
('SIT1','DS:UserService','https://172.30.35.51:32010',1),
('SIT1','ES:AddressWhisperer:V1','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('SIT1','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('SIT1','ES:C4MCustomersExposure:V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('SIT1','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('SIT1','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('SIT1','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('SIT1','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('SIT1','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('SIT1','ES:CustomerManagement:V1','https://cm-fat.kb.cz/be-cm/api',3),
('SIT1','ES:CustomerProfile:V1','https://cm-fat.kb.cz/be-cm/api',3),
('SIT1','ES:EAS:R21','https://sb2_test_server.mpss.cz/SIT/EAS_WS_SB_Services.svc',3),
('SIT1','ES:EasSimulationHT:V6','https://sb2_test_server.mpss.cz/SIT/HT_WS_SB_Services.svc',3),
('SIT1','ES:ESignatures:V1','',3),
('SIT1','ES:IdentifiedSubjectBr:V1','https://cm-identified-subject-br-v1.fat.custmng.kbcloud/api/',3),
('SIT1','ES:MpHome:V1_1','https://hfsit1mpdigi.mpss.cz/api/1.1',3),
('SIT1','ES:SbWebApi:V1','https://sb2_test_server.mpss.cz/WebApi/SIT',3),
('SIT1','ES:Sdf:V1','',3),
('SIT1','ES:Sulm:V1','https://iib-sit1.kb.cz/SulmService/1/0',3);
GO

INSERT INTO dbo.ServiceDiscovery2 ([EnvironmentName],[ServiceName],[ServiceUrl],[ServiceType]) VALUES
('UAT','CIS:DocumentGeneratorService','https://172.30.35.51:33014',1),
('UAT','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=',3),
('UAT','CIS:NotificationService','https://172.30.35.51:33015',1),
('UAT','CIS:NotificationService','https://172.30.35.51:33016',2),
('UAT','CIS:ServiceDiscovery','https://172.30.35.51:33000',1),
('UAT','DS:CaseService','https://172.30.35.51:33001',1),
('UAT','DS:CodebookService','https://172.30.35.51:33003',1),
('UAT','DS:CodebookService','https://172.30.35.51:33002',2),
('UAT','DS:CustomerService','https://172.30.35.51:33004',1),
('UAT','DS:DocumentArchiveService','https://172.30.35.51:33005',1),
('UAT','DS:HouseholdService','https://172.30.35.51:33018',1),
('UAT','DS:OfferService','https://172.30.35.51:33006',1),
('UAT','DS:ProductService','https://172.30.35.51:33007',1),
('UAT','DS:RiskIntegrationService','https://172.30.35.51:33012',1),
('UAT','DS:RiskIntegrationService','https://172.30.35.51:33013',2),
('UAT','DS:SalesArrangementService','https://172.30.35.51:33009',1),
('UAT','DS:UserService','https://172.30.35.51:33010',1),
('UAT','ES:AddressWhisperer:V1','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('UAT','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('UAT','ES:C4MCustomersExposure:V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('UAT','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('UAT','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('UAT','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('UAT','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('UAT','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('UAT','ES:CustomerManagement:V1','https://cm-fat.kb.cz/be-cm/api',3),
('UAT','ES:CustomerProfile:V1','https://cm-fat.kb.cz/be-cm/api',3),
('UAT','ES:EAS:R21','https://sb2_test_server.mpss.cz/UAT/EAS_WS_SB_Services.svc',3),
('UAT','ES:EasSimulationHT:V6','https://sb2_test_server.mpss.cz/UAT/HT_WS_SB_Services.svc',3),
('UAT','ES:ESignatures:V1','',3),
('UAT','ES:IdentifiedSubjectBr:V1','https://cm-identified-subject-br-v1.stage.custmng.kbcloud/api/',3),
('UAT','ES:MpHome:V1_1','https://hfuat1mpdigi.mpss.cz/api/1.1',3),
('UAT','ES:SbWebApi:V1','https://sb2_test_server.mpss.cz/WebApi/UAT',3),
('UAT','ES:Sdf:V1','',3),
('UAT','ES:Sulm:V1','https://iib-sit1.kb.cz/SulmService/1/0',3);
GO
