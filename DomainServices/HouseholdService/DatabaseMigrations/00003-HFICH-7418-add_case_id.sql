IF COL_LENGTH('dbo.CustomerOnSA', 'CaseId') IS NULL
BEGIN
ALTER TABLE [CustomerOnSA] ADD [CaseId] bigint NOT NULL;
END
GO