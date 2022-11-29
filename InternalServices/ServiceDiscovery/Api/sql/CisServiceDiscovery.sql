USE [CIS]
GO

DECLARE @environment NVARCHAR(10) = 'DEV';

----------------------------------------------------------------------------------------------------------------
-- CONTENT
----------------------------------------------------------------------------------------------------------------
DECLARE @services TABLE(EnvironmentName VARCHAR(50), ServiceName VARCHAR(50) , ServiceUrl VARCHAR(250), ServiceType tinyint);

INSERT INTO @services VALUES

-- DEV:
('DEV','CIS:DocumentArchiveService','https://172.30.35.51:30005',1),
('DEV','CIS:DocumentArchiveService','https://172.30.35.51:30017',2),
('DEV','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=',3),
('DEV','CIS:NotificationService','https://172.30.35.51:30015',1),
('DEV','CIS:NotificationService','https://172.30.35.51:30016',2),
('DEV','CIS:ServiceDiscovery','https://172.30.35.51:30000',1),
('DEV','CIS:Storage','https://172.30.35.51:5004',1),
('DEV','DS:CaseService','https://172.30.35.51:30001',1),
('DEV','DS:CodebookService','https://172.30.35.51:30003',1),
('DEV','DS:CodebookService','https://172.30.35.51:30002',2),
('DEV','DS:CustomerService','https://172.30.35.51:30004',1),
('DEV','DS:HouseholdService','https://172.30.35.51:30018',1),
('DEV','DS:OfferService','https://172.30.35.51:30006',1),
('DEV','DS:ProductService','https://172.30.35.51:30007',1),
('DEV','DS:RiskIntegrationService','https://172.30.35.51:30012',1),
('DEV','DS:RiskIntegrationService','https://172.30.35.51:30013',2),
('DEV','DS:SalesArrangementService','https://172.30.35.51:30009',1),
('DEV','DS:UserService','https://172.30.35.51:30010',1),
('DEV','ES:AddressWhisperer','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('DEV','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('DEV','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('DEV','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('DEV','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('DEV','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('DEV','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('DEV','ES:CustomerManagement','https://cm-fat.kb.cz/be-cm/api',3),
('DEV','ES:EAS','https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc',3),
('DEV','ES:EasSimulationHT','https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc',3),
('DEV','ES:MpHome','https://hffatmpdigi.mpss.cz',3),
('DEV','ES:Rip','https://rip-sit1.vsskb.cz',3),
('DEV','ES:SbWebApi','https://sb2_test_server.mpss.cz/WebApi/FAT',3),
('DEV','ES:Sulm','https://iib-sit1.kb.cz/SulmService/1/0',3),

-- FAT:
('FAT','CIS:DocumentArchiveService','https://ds-discovery-fat.vsskb.cz:31005',1),
('FAT','CIS:DocumentArchiveService','https://ds-discovery-fat.vsskb.cz:31017',2),
('FAT','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=',3),
('FAT','CIS:NotificationService','https://ds-discovery-fat.vsskb.cz:31015',1),
('FAT','CIS:NotificationService','https://ds-discovery-fat.vsskb.cz:31016',2),
('FAT','CIS:ServiceDiscovery','https://ds-discovery-fat.vsskb.cz:31000',1),
('FAT','CIS:Storage','https://172.30.35.51:5004',1),
('FAT','DS:CaseService','https://ds-discovery-fat.vsskb.cz:31001',1),
('FAT','DS:CodebookService','https://ds-discovery-fat.vsskb.cz:31003',1),
('FAT','DS:CodebookService','https://ds-discovery-fat.vsskb.cz:31002',2),
('FAT','DS:CustomerService','https://ds-discovery-fat.vsskb.cz:31004',1),
('FAT','DS:HouseholdService','https://ds-discovery-fat.vsskb.cz:31018',1),
('FAT','DS:OfferService','https://ds-discovery-fat.vsskb.cz:31006',1),
('FAT','DS:ProductService','https://ds-discovery-fat.vsskb.cz:31007',1),
('FAT','DS:RiskIntegrationService','https://ds-discovery-fat.vsskb.cz:31012',1),
('FAT','DS:RiskIntegrationService','https://ds-discovery-fat.vsskb.cz:31013',2),
('FAT','DS:SalesArrangementService','https://ds-discovery-fat.vsskb.cz:31009',1),
('FAT','DS:UserService','https://ds-discovery-fat.vsskb.cz:31010',1),
('FAT','ES:AddressWhisperer','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('FAT','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('FAT','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('FAT','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('FAT','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('FAT','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('FAT','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('FAT','ES:CustomerManagement','https://cm-fat.kb.cz/be-cm/api',3),
('FAT','ES:EAS','https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc',3),
('FAT','ES:EasSimulationHT','https://sb2_test_server.mpss.cz/FAT/HT_WS_SB_Services.svc',3),
('FAT','ES:MpHome','https://hffatmpdigi.mpss.cz',3),
('FAT','ES:Rip','https://rip-sit1.vsskb.cz',3),
('FAT','ES:SbWebApi','https://sb2_test_server.mpss.cz/WebApi/FAT',3),
('FAT','ES:Sulm','https://iib-sit1.kb.cz/SulmService/1/0',3),

-- SIT:
('SIT1','CIS:DocumentArchiveService','https://172.30.35.51:32005',1),
('SIT1','CIS:DocumentArchiveService','https://172.30.35.51:32017',2),
('SIT1','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_sit1,password=MpssSit1Pass,allowAdmin=false,tieBreaker=',3),
('SIT1','CIS:NotificationService','https://172.30.35.51:32015',1),
('SIT1','CIS:NotificationService','https://172.30.35.51:32016',2),
('SIT1','CIS:ServiceDiscovery','https://172.30.35.51:32000',1),
('SIT1','CIS:Storage','https://172.30.35.51:5004',1),
('SIT1','DS:CaseService','https://172.30.35.51:32001',1),
('SIT1','DS:CodebookService','https://172.30.35.51:32003',1),
('SIT1','DS:CodebookService','https://172.30.35.51:32002',2),
('SIT1','DS:CustomerService','https://172.30.35.51:32004',1),
('SIT1','DS:HouseholdService','https://172.30.35.51:32018',1),
('SIT1','DS:OfferService','https://172.30.35.51:32006',1),
('SIT1','DS:ProductService','https://172.30.35.51:32007',1),
('SIT1','DS:RiskIntegrationService','https://172.30.35.51:32012',1),
('SIT1','DS:RiskIntegrationService','https://172.30.35.51:32013',2),
('SIT1','DS:SalesArrangementService','https://172.30.35.51:32009',1),
('SIT1','DS:UserService','https://172.30.35.51:32010',1),
('SIT1','ES:AddressWhisperer','https://iib-uat1.kb.cz/AddressWhispererBEService/v1',3),
('SIT1','ES:C4MCreditWorthiness_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/kbgroup-laa-credit-worthiness-calculation-service-1/api',3),
('SIT1','ES:C4MCustomersExposure_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-exposure-service-1/api',3),
('SIT1','ES:C4MLoanApplication_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/la-loan-application-hf-adapter-service-1/api',3),
('SIT1','ES:C4MLoanApplicationAssessment_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-assessment-service-1/api',3),
('SIT1','ES:C4MRiskBusinessCase_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-risk-business-case-service-1/api',3),
('SIT1','ES:C4MRiskCharakteristics_V1','https://uat.ml-granting.kbgroup.risk.kbcloud/laa-loan-application-risk-characteristics-calculation-service-1/api',3),
('SIT1','ES:CustomerManagement','https://cm-fat.kb.cz/be-cm/api',3),
('SIT1','ES:EAS','https://sb2_test_server.mpss.cz/SIT/EAS_WS_SB_Services.svc',3),
('SIT1','ES:EasSimulationHT','https://sb2_test_server.mpss.cz/SIT/HT_WS_SB_Services.svc',3),
('SIT1','ES:MpHome','https://hfsit1mpdigi.mpss.cz',3),
('SIT1','ES:Rip','https://rip-sit1.vsskb.cz',3),
('SIT1','ES:SbWebApi','https://sb2_test_server.mpss.cz/WebApi/SIT',3),
('SIT1','ES:Sulm','https://iib-sit1.kb.cz/SulmService/1/0',3),

-- UAT:
('UAT','CIS:DocumentArchiveService','https://172.30.35.51:33005',1),
('UAT','CIS:DocumentArchiveService','https://172.30.35.51:33017',2),
('UAT','CIS:GlobalCache:Redis','redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_uat1,password=MpssUat1Pass,allowAdmin=false,tieBreaker=',3),
('UAT','CIS:NotificationService','https://172.30.35.51:33015',1),
('UAT','CIS:NotificationService','https://172.30.35.51:33016',2),
('UAT','CIS:ServiceDiscovery','https://172.30.35.52:33000',1),
('UAT','CIS:Storage','https://172.30.35.52:5004',1),
('UAT','DS:CaseService','https://172.30.35.52:33001',1),
('UAT','DS:CodebookService','https://172.30.35.52:33003',1),
('UAT','DS:CodebookService','https://172.30.35.52:33002',2),
('UAT','DS:CustomerService','https://172.30.35.52:33004',1),
('UAT','DS:HouseholdService','https://172.30.35.52:33018',1),
('UAT','DS:OfferService','https://172.30.35.52:33006',1),
('UAT','DS:ProductService','https://172.30.35.52:33007',1),
('UAT','DS:RiskIntegrationService','?',1),
('UAT','DS:RiskIntegrationService','?',2),
('UAT','DS:SalesArrangementService','https://172.30.35.52:33009',1),
('UAT','DS:UserService','https://172.30.35.52:33010',1),
('UAT','ES:AddressWhisperer','?',3),
('UAT','ES:C4MCreditWorthiness_V1','?',3),
('UAT','ES:C4MCustomersExposure_V1','?',3),
('UAT','ES:C4MLoanApplication_V1','?',3),
('UAT','ES:C4MLoanApplicationAssessment_V1','?',3),
('UAT','ES:C4MRiskBusinessCase_V1','?',3),
('UAT','ES:C4MRiskCharakteristics_V1','?',3),
('UAT','ES:CustomerManagement','https://cm-uat.kb.cz/be-cm/api',3),
('UAT','ES:EAS','https://sb2_test_server.mpss.cz/UAT/EAS_WS_SB_Services.svc',3),
('UAT','ES:EasSimulationHT','https://sb2_test_server.mpss.cz/UAT/HT_WS_SB_Services.svc',3),
('UAT','ES:MpHome','https://hfuat1mpdigi.mpss.cz',3),
('UAT','ES:Rip','https://rip-sit1.vsskb.cz',3),
('UAT','ES:SbWebApi','https://sb2_test_server.mpss.cz/WebApi/UAT',3),
('UAT','ES:Sulm','-',3);


----------------------------------------------------------------------------------------------------------------
-- BACKUP
----------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo' AND TABLE_NAME = N'ServiceDiscovery')
BEGIN
	DECLARE @backup_timestamp NCHAR(15) = (SELECT FORMAT (GetDate(), 'yyyyMMdd_hhmmss'));
	DECLARE @backup_table NCHAR(100) = FORMATMESSAGE('[dbo].[ServiceDiscovery_%s]', @backup_timestamp);
	DECLARE @backup_stmt NVARCHAR(200) = FORMATMESSAGE('SELECT * INTO %s FROM dbo.ServiceDiscovery;', @backup_table);
	EXEC (@backup_stmt);
	PRINT FORMATMESSAGE('Current content backuped to table %s', @backup_table);
END


----------------------------------------------------------------------------------------------------------------
-- RECREATE TABLE
----------------------------------------------------------------------------------------------------------------
DROP TABLE IF EXISTS [dbo].[ServiceDiscovery]

CREATE TABLE [dbo].[ServiceDiscovery](
	[EnvironmentName] [varchar](50) NOT NULL,
	[ServiceName] [varchar](50) NOT NULL,
	[ServiceUrl] [varchar](250) NOT NULL,
	[ServiceType] [tinyint] NOT NULL,
 CONSTRAINT [PK_ServiceDiscovery] PRIMARY KEY CLUSTERED 
(
	[ServiceName] ASC,
	[ServiceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];


----------------------------------------------------------------------------------------------------------------
-- INSERT DATA
----------------------------------------------------------------------------------------------------------------
INSERT INTO [dbo].[ServiceDiscovery] ([EnvironmentName],[ServiceName],[ServiceUrl],[ServiceType])
SELECT [EnvironmentName],[ServiceName],[ServiceUrl],[ServiceType] FROM @services WHERE UPPER([EnvironmentName]) = UPPER(@environment) ORDER BY ServiceName, ServiceType


PRINT FORMATMESSAGE('New table contains %s rows.', CAST(@@ROWCOUNT as NVARCHAR(10)));

/*
SELECT CONCAT('(', '''UAT''', ',''', ServiceName, ''',''', '-'',', ServiceType, '),' )
FROM @services
WHERE EnvironmentName = 'DEV' AND CONCAT(ServiceName, '|', ServiceType) NOT IN (SELECT CONCAT(ServiceName, '|', ServiceType) FROM @services WHERE EnvironmentName = 'UAT') 
*/