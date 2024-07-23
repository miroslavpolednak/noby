INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'ES:Pcp:V2', N'https://be-productinstanceservice-v1.production.pcp-mdm.kbcloud/services/ProductInstanceBEService/v2', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'ES:Party:V1', N'https://partygeneral-v1.prod.prs.kbcloud/services/PartyGeneralBEService', 3, 0)

INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'ES:Pcp:V3', N'https://be-productinstanceservice-v2.production.pcp-mdm.kbcloud/services/ProductInstanceBEService/v2', 3, 0)