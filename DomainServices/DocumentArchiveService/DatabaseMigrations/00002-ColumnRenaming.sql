GO
IF COL_LENGTH('dbo.DocumentInterface', 'DATUM_PRIJETI') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.DocumentInterface.DATUM_PRIJETI', 'CREATED_ON';
END