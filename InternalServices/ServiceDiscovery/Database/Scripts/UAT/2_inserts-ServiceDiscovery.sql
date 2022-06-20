USE [CIS]
GO

SET IDENTITY_INSERT [dbo].[ServiceDiscovery] ON 
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (1, N'UAT', N'CIS:Storage', N'https://172.30.35.52:5004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (2, N'UAT', N'CIS:GlobalCache:Redis', N'redis-review-sentinel.service.ist.consul-nprod.kb.cz:6379,checkCertificateRevocation=false,connectRetry=1,abortConnect=false,ssl=true,user=xx_redis_mpss_uat1,password=MpssUat1Pass,allowAdmin=false,tieBreaker=', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (3, N'UAT', N'DS:OfferService', N'https://172.30.35.52:33006', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (4, N'UAT', N'CIS:ServiceDiscovery', N'https://172.30.35.52:33000', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (5, N'UAT', N'DS:CustomerService', N'https://172.30.35.52:33004', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (6, N'UAT', N'DS:CodebookService', N'https://172.30.35.52:33003', 2)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (7, N'UAT', N'DS:CodebookService', N'https://172.30.35.52:33002', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (8, N'UAT', N'ES:EAS', N'https://sb2_test_server.mpss.cz/UAT/EAS_WS_SB_Services.svc', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (9, N'UAT', N'DS:UserService', N'https://172.30.35.52:33010', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (10, N'UAT', N'DS:CaseService', N'https://172.30.35.52:33001', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (11, N'UAT', N'DS:SalesArrangementService', N'https://172.30.35.52:33009', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (12, N'UAT', N'DS:ProductService', N'https://172.30.35.52:33007', 1)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (13, N'UAT', N'ES:MpHome', N'https://hfuat1mpdigi.mpss.cz', 3)
GO
INSERT [dbo].[ServiceDiscovery] ([Id], [EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType]) VALUES (14, N'UAT', N'ES:CustomerManagement', N'https://cm-uat.kb.cz/be-cm/api', 3)
GO
SET IDENTITY_INSERT [dbo].[ServiceDiscovery] OFF
GO
