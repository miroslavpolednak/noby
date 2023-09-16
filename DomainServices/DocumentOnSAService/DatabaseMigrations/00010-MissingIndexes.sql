GO
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_DocumentOnSa_FormId' AND object_id = OBJECT_ID('DocumentOnSa'))
    BEGIN
       CREATE INDEX [IX_DocumentOnSa_FormId] ON [DocumentOnSa] ([FormId]);
    END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_DocumentOnSa_SalesArrangementId' AND object_id = OBJECT_ID('DocumentOnSa'))
    BEGIN
       CREATE INDEX [IX_DocumentOnSa_SalesArrangementId] ON [DocumentOnSa] ([SalesArrangementId]);
    END