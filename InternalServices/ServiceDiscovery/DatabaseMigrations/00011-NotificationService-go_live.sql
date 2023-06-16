INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'CIS:NotificationService', N'https://ds-notification-prod.vsskb.cz:39015', 1, 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'CIS:NotificationService', N'https://ds-notification-prod.vsskb.cz:39016', 2, 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'CIS:ServiceDiscovery', N'https://ds-discovery-prod.vsskb.cz:39000', 1, 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'DS:CodebookService', N'https://ds-codebook-prod.vsskb.cz:39003', 1, 1)
GO
INSERT [dbo].[ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], [ServiceType], [AddToGlobalHealthCheck]) VALUES (N'PROD', N'DS:CodebookService', N'https://ds-codebook-prod.vsskb.cz:39002', 2, 1)
GO