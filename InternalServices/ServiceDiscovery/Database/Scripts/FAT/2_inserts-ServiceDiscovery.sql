USE [CIS]
GO

SET IDENTITY_INSERT [dbo].[ServiceDiscovery] ON 
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (1, N'FAT', N'CIS:Storage', N'https://172.30.35.51:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (2, N'FAT', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_fat,password=MpssFatPass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (3, N'FAT', N'DS:OfferService', N'https://172.30.35.51:31006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (4, N'FAT', N'CIS:ServiceDiscovery', N'https://172.30.35.51:31000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (5, N'FAT', N'DS:CustomerService', N'https://172.30.35.51:31004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (6, N'FAT', N'DS:CodebookService', N'https://172.30.35.51:31003', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (7, N'FAT', N'DS:CodebookService', N'https://172.30.35.51:31002', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (8, N'FAT', N'ES:EAS', N'https://sb2_test_server.mpss.cz/FAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (9, N'FAT', N'DS:UserService', N'https://172.30.35.51:31010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (10, N'FAT', N'DS:CaseService', N'https://172.30.35.51:31001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (11, N'FAT', N'DS:SalesArrangementService', N'https://172.30.35.51:31009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (12, N'FAT', N'DS:ProductService', N'https://172.30.35.51:31007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (13, N'FAT', N'ES:MpHome', N'https://hffatmpdigi.mpss.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (14, N'FAT', N'ES:CustomerManagement', N'https://cm-fat.kb.cz/be-cm/api', 3)
GO
SET IDENTITY_INSERT [dbo].[ServiceDiscovery] OFF
GO
