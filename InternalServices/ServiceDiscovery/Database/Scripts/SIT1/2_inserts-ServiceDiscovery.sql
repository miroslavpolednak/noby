USE [CIS]
GO

SET IDENTITY_INSERT [dbo].[ServiceDiscovery] ON 
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (1, N'SIT1', N'CIS:Storage', N'https://172.30.35.51:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (2, N'SIT1', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (3, N'SIT1', N'DS:OfferService', N'https://172.30.35.51:32106', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (4, N'SIT1', N'CIS:ServiceDiscovery', N'https://172.30.35.51:32100', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (5, N'SIT1', N'DS:CustomerService', N'https://172.30.35.51:32104', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (6, N'SIT1', N'DS:CodebookService', N'https://172.30.35.51:32103', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (7, N'SIT1', N'DS:CodebookService', N'https://172.30.35.51:32102', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (8, N'SIT1', N'ES:EAS', N'https://sb2_test_server.mpss.cz/SIT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (9, N'SIT1', N'DS:UserService', N'https://172.30.35.51:32110', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (10, N'SIT1', N'DS:CaseService', N'https://172.30.35.51:32101', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (11, N'SIT1', N'DS:SalesArrangementService', N'https://172.30.35.51:32109', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (12, N'SIT1', N'DS:ProductService', N'https://172.30.35.51:32107', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (13, N'SIT1', N'ES:MpHome', N'https://hfsit1mpdigi.mpss.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (14, N'SIT1', N'ES:CustomerManagement', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
SET IDENTITY_INSERT [dbo].[ServiceDiscovery] OFF
GO
