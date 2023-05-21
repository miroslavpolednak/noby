INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'DEV', N'ES:CustomerManagement:V2', N'https://be-cm-v1.fat.custmng.kbcloud/api', 3)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'FAT', N'ES:CustomerManagement:V2', N'https://be-cm-v1.fat.custmng.kbcloud/api', 3)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'SIT1', N'ES:CustomerManagement:V2', N'https://be-cm-v1.fat.custmng.kbcloud/api', 3)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'UAT', N'ES:CustomerManagement:V2', N'https://be-cm-v1.stage.custmng.kbcloud/api', 3)
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (N'PREPROD', N'ES:CustomerManagement:V2', N'https://be-cm-v1.stage.custmng.kbcloud/api', 3)

UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.fat.custmng.kbcloud/api' WHERE EnvironmentName = 'DEV' AND ServiceName = 'ES:CustomerProfile:V1'
UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.fat.custmng.kbcloud/api' WHERE EnvironmentName = 'FAT' AND ServiceName = 'ES:CustomerProfile:V1'
UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.fat.custmng.kbcloud/api' WHERE EnvironmentName = 'SIT1' AND ServiceName = 'ES:CustomerProfile:V1'
UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.stage.custmng.kbcloud/api' WHERE EnvironmentName = 'UAT' AND ServiceName = 'ES:CustomerProfile:V1'

UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.stage.custmng.kbcloud/api' WHERE EnvironmentName = 'DEV' AND ServiceName = 'ES:Kyc:V1'
UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.stage.custmng.kbcloud/api' WHERE EnvironmentName = 'FAT' AND ServiceName = 'ES:Kyc:V1'
UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.stage.custmng.kbcloud/api' WHERE EnvironmentName = 'SIT1' AND ServiceName = 'ES:Kyc:V1'
UPDATE ServiceDiscovery SET ServiceUrl = 'https://be-cm-v1.stage.custmng.kbcloud/api' WHERE EnvironmentName = 'UAT' AND ServiceName = 'ES:Kyc:V1'