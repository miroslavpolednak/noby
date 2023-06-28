IF COL_LENGTH('dbo.FormInstanceInterface', 'CreatedAt') IS NOT NULL
BEGIN
  EXEC sp_rename 'dbo.FormInstanceInterface.CreatedAt', 'CREATED_AT';
END
GO