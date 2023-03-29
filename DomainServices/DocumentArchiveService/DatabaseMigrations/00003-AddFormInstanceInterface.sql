GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormInstanceInterface]') AND type in (N'U'))
DROP TABLE [dbo].[FormInstanceInterface]
GO
CREATE TABLE [FormInstanceInterface] (
    [DOCUMENT_ID] varchar(30) NOT NULL,
    [FORM_TYPE] varchar(7) NULL,
    [STATUS] smallint NULL,
    [FORM_KIND] char(1) NULL,
    [CPM] varchar(10) NULL,
    [ICP] varchar(10) NULL,
    [CreatedAt] datetime2 NULL,
    [STORNO] tinyint NULL,
    [DATA_TYPE] tinyint NULL,
    [JSON_DATA_CLOB] varchar(max) NULL,
    CONSTRAINT [PK_FormInstanceInterface] PRIMARY KEY ([DOCUMENT_ID]),
    CONSTRAINT [FK_FormInstanceInterface_DocumentInterface_DOCUMENT_ID] FOREIGN KEY ([DOCUMENT_ID]) REFERENCES [DocumentInterface] ([DOCUMENT_ID]) ON DELETE CASCADE
);
GO
