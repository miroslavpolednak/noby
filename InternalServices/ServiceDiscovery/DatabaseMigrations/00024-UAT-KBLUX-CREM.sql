update [CIS].[dbo].[ServiceDiscovery] set ServiceUrl = 'https://api.stage.crem.kbcloud/v1/deed-of-ownership/api' where EnvironmentName = 'UAT' and ServiceName='ES:Crem:V1';
 
update [CIS].[dbo].[ServiceDiscovery] set ServiceUrl = 'https://acvapi-uat3.dslab.kb.cz/kblux' where EnvironmentName = 'UAT' and ServiceName='ES:LuxpiService:V1';