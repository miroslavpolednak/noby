USE [master]
GO
CREATE LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD] FROM WINDOWS WITH DEFAULT_DATABASE=[master]


USE [KonsDb_L1]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
