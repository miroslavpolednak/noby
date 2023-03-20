BEGIN TRANSACTION;
GO
IF COL_LENGTH('dbo.FormInstanceInterface', 'DOKUMENT_ID') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.FormInstanceInterface.DOKUMENT_ID', 'DOCUMENT_ID';
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'TYP_FORMULARE') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.FormInstanceInterface.TYP_FORMULARE', 'FORM_TYPE';
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'CISLO_SMLOUVY') IS NOT NULL
BEGIN
 ALTER TABLE dbo.FormInstanceInterface DROP COLUMN CISLO_SMLOUVY;
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'DRUH_FROMULARE') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.FormInstanceInterface.DRUH_FROMULARE', 'FORM_KIND';
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'FORMID') IS NOT NULL
BEGIN
 ALTER TABLE dbo.FormInstanceInterface DROP COLUMN FORMID;
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'HESLO_KOD') IS NOT NULL
BEGIN
 ALTER TABLE dbo.FormInstanceInterface DROP COLUMN HESLO_KOD;
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'STATUS') IS NOT NULL
BEGIN
 ALTER TABLE dbo.FormInstanceInterface DROP COLUMN STATUS;
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'STORNOVANO') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.FormInstanceInterface.STORNOVANO', 'STORNO';
END
GO

IF COL_LENGTH('dbo.FormInstanceInterface', 'TYP_DAT') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.FormInstanceInterface.TYP_DAT', 'DATA_TYPE';
END
GO
COMMIT;
GO