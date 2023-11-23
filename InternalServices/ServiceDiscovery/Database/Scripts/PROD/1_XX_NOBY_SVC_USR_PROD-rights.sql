USE [master]
GO
CREATE LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD] FROM WINDOWS WITH DEFAULT_DATABASE=[master]


USE [CaseService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [CIS]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]

USE [CodebookService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]

USE [DataAggregatorService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [DocumentArchiveService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [DocumentOnSAService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [HouseholdService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [NOBY]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [NobyAudit]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [NotificationService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]

USE [OfferService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [RealEstateValuationService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 

USE [SalesArrangementService]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 