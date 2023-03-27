ALTER TABLE dbo.ServiceDiscovery ADD AddToGlobalHealthCheck bit default(0);
GO
UPDATE dbo.ServiceDiscovery SET AddToGlobalHealthCheck=0;
UPDATE dbo.ServiceDiscovery SET AddToGlobalHealthCheck=1 WHERE ServiceType=1 AND ServiceName!='CIS:ServiceDiscovery';
