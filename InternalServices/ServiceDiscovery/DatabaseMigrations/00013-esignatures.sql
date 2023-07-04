UPDATE [dbo].[ServiceDiscovery] SET [ServiceUrl]='https://fatepodpisy.mpss.cz/WS' 
	WHERE EnvironmentName='DEV' AND ServiceName='ES:ESignatures:V1' AND ServiceType=3;
