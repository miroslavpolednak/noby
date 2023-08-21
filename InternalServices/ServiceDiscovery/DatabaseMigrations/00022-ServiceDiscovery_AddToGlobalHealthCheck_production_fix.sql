USE [CIS]
GO

update [CIS].[dbo].[ServiceDiscovery]
set AddToGlobalHealthCheck = 0
where EnvironmentName = 'PROD' and ServiceName = 'CIS:ServiceDiscovery'
 
GO