USE [NobyAudit]
CREATE USER [VSSKB\XX_NOBY_SVC_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_SVC_USR_PROD] 