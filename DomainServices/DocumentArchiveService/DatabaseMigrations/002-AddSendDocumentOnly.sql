BEGIN TRANSACTION;
GO

ALTER TABLE [DocumentInterface] ADD [SEND_DOCUMENT_ONLY] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
GO

COMMIT;
GO