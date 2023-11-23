/*  
 AS-IS: 
  existing login VSSKB\XX_NOBY_APP_USR_PROD
  rights to NobyAudit to VSSKB\XX_NOBY_APP_USR_PROD are part of NobyAudit scripts
*/

USE [NOBY]
CREATE USER [VSSKB\XX_NOBY_APP_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_APP_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_APP_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_APP_USR_PROD] 

USE [NobyAudit]
CREATE USER [VSSKB\XX_NOBY_APP_USR_PROD] FOR LOGIN [VSSKB\XX_NOBY_APP_USR_PROD]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\XX_NOBY_APP_USR_PROD]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\XX_NOBY_APP_USR_PROD] 