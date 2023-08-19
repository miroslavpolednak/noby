UPDATE [dbo].[ServiceDiscovery] SET [ServiceUrl]='https://adpra021.vsskb.cz/SDF/ExtendedServices.svc' 
WHERE EnvironmentName='PREPROD' AND ServiceName='ES:Sdf:V1' AND ServiceType=3;