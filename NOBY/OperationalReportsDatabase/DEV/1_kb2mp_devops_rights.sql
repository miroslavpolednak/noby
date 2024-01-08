USE [NobyOperationalReports]

CREATE USER [kb2mp_DevOps] FOR LOGIN [kb2mp_DevOps]
ALTER ROLE [db_backupoperator] ADD MEMBER [kb2mp_DevOps]
ALTER ROLE [db_datareader] ADD MEMBER [kb2mp_DevOps]
ALTER ROLE [db_datawriter] ADD MEMBER [kb2mp_DevOps]
ALTER ROLE [db_ddladmin] ADD MEMBER [kb2mp_DevOps]
GRANT ALTER ON SCHEMA::[dbo] TO [kb2mp_DevOps]
GRANT CONTROL ON SCHEMA::[dbo] TO [kb2mp_DevOps]