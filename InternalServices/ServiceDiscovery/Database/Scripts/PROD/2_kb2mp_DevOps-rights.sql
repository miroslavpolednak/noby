USE [master]
CREATE LOGIN [VSSKB\kb2mp_DevOps] FROM WINDOWS WITH DEFAULT_DATABASE=[master]
GO

use [Maintenance]
CREATE USER [VSSKB\kb2mp_DevOps] FOR LOGIN [VSSKB\kb2mp_DevOps]
GRANT EXECUTE ON [dbo].[PreRelaseBackup] TO [VSSKB\kb2mp_DevOps]

 
--Pipeline odsud dolů
use [CIS]
CREATE USER [VSSKB\kb2mp_DevOps] FOR LOGIN [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_backupoperator] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_ddladmin] ADD MEMBER [VSSKB\kb2mp_DevOps]
GRANT ALTER ON SCHEMA::[dbo] TO [VSSKB\kb2mp_DevOps]
GRANT CONTROL ON SCHEMA::[dbo] TO [VSSKB\kb2mp_DevOps]
 

use [CodebookService]
CREATE USER [VSSKB\kb2mp_DevOps] FOR LOGIN [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_backupoperator] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_ddladmin] ADD MEMBER [VSSKB\kb2mp_DevOps]
GRANT ALTER ON SCHEMA::[dbo] TO [VSSKB\kb2mp_DevOps]
GRANT CONTROL ON SCHEMA::[dbo] TO [VSSKB\kb2mp_DevOps]


use [NotificationService]
CREATE USER [VSSKB\kb2mp_DevOps] FOR LOGIN [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_backupoperator] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_datareader] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_datawriter] ADD MEMBER [VSSKB\kb2mp_DevOps]
ALTER ROLE [db_ddladmin] ADD MEMBER [VSSKB\kb2mp_DevOps]
GRANT ALTER ON SCHEMA::[dbo] TO [VSSKB\kb2mp_DevOps]
GRANT CONTROL ON SCHEMA::[dbo] TO [VSSKB\kb2mp_DevOps]
