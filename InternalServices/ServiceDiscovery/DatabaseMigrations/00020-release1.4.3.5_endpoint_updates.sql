delete from [dbo].[ServiceDiscovery] where  ServiceName = 'DS:RealEstateValuationService';

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'DS:RealEstateValuationService', N'https://172.30.35.51:30021', 1)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'DS:RealEstateValuationService', N'https://ds-discovery-fat.vsskb.cz:31021', 1)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'DS:RealEstateValuationService', N'https://172.30.35.51:32021', 1)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'DS:RealEstateValuationService', N'https://172.30.35.52:33021', 1)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'DS:RealEstateValuationService', N'https://adpra189.vsskb.cz:38021', 1)



delete from ServiceDiscovery where ServiceName = 'ES:PreorderService:V1';

INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('DEV', 'ES:PreorderService:V1', 'https://api.fat.car.kbcloud/v1/preorder', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('FAT', 'ES:PreorderService:V1', 'https://api.fat.car.kbcloud/v1/preorder', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('SIT1', 'ES:PreorderService:V1', 'https://api.fat.car.kbcloud/v1/preorder', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('UAT', 'ES:PreorderService:V1', 'https://api.stage.car.kbcloud/v1/preorder', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('PREPROD', 'ES:PreorderService:V1', 'https://api.stage.car.kbcloud/v1/preorder', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('PROD', 'ES:PreorderService:V1', 'TODO', 3, 0);


delete from ServiceDiscovery where ServiceName = 'ES:AcvEnumService:V1';

INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('DEV', 'ES:AcvEnumService:V1', 'https://api.fat.car.kbcloud/v1/enums', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('FAT', 'ES:AcvEnumService:V1', 'https://api.fat.car.kbcloud/v1/enums', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('SIT1', 'ES:AcvEnumService:V1', 'https://api.fat.car.kbcloud/v1/enums', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('UAT', 'ES:AcvEnumService:V1', 'https://api.stage.car.kbcloud/v1/enums', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('PREPROD', 'ES:AcvEnumService:V1', 'https://api.stage.car.kbcloud/v1/enums', 3, 0);
INSERT INTO ServiceDiscovery (EnvironmentName, ServiceName, ServiceUrl, ServiceType, AddToGlobalHealthCheck) VALUES ('PROD', 'ES:AcvEnumService:V1', 'TODO', 3, 0);



