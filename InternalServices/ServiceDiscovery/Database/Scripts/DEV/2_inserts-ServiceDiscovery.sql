USE [CIS]
GO

SET IDENTITY_INSERT [dbo].[ServiceDiscovery] ON 
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (1, N'DEV', N'CIS:Storage', N'https://172.30.35.51:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (2, N'DEV', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (3, N'DEV', N'DS:OfferService', N'https://172.30.35.51:30006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (4, N'DEV', N'CIS:ServiceDiscovery', N'https://172.30.35.51:30000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (5, N'DEV', N'DS:CustomerService', N'https://172.30.35.51:30004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (6, N'DEV', N'DS:CodebookService', N'https://172.30.35.51:30003', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (7, N'DEV', N'DS:CodebookService', N'https://172.30.35.51:30002', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (8, N'DEV', N'ES:EAS', N'https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (9, N'DEV', N'DS:UserService', N'https://172.30.35.51:30010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (10, N'DEV', N'DS:CaseService', N'https://172.30.35.51:30001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (11, N'DEV', N'DS:SalesArrangementService', N'https://172.30.35.51:30009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (12, N'DEV', N'DS:ProductService', N'https://172.30.35.51:30007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (13, N'DEV', N'ES:MpHome', N'https://hffatmpdigi.mpss.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (14, N'DEV', N'ES:CustomerManagement', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
SET IDENTITY_INSERT [dbo].[ServiceDiscovery] OFF
GO
