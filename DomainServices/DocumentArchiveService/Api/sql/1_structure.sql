BEGIN TRANSACTION;
GO

CREATE TABLE [DocumentInterface] (
    [DOCUMENT_ID] varchar(30) NOT NULL,
    [DOCUMENT_DATA] varbinary(max) NOT NULL,
    [FILENAME] nvarchar(64) NOT NULL,
    [FILENAME_SUFFIX] varchar(10) NOT NULL,
    [DESCRIPTION] nvarchar(254) NULL,
    [CASEID] bigint NOT NULL,
    [DATUM_PRIJETI] datetime NOT NULL,
    [AUTHOR_USER_LOGIN] varchar(10) NOT NULL,
    [CONTRACT_NUMBER] varchar(13) NULL,
    [STATUS] int NOT NULL DEFAULT 100,
    [STATUS_ERROR_TEXT] varchar(1000) NULL,
    [FORMID] varchar(15) NULL,
    [EA_CODE_MAIN_ID] int NOT NULL,
    [DOCUMENT_DIRECTION] varchar(1) NOT NULL DEFAULT 'E',
    [FOLDER_DOCUMENT] varchar(1) NOT NULL DEFAULT 'N',
    [FOLDER_DOCUMENT_ID] varchar(30) NULL,
    [KDV] tinyint NOT NULL,
    CONSTRAINT [PK_DocumentInterface] PRIMARY KEY ([DOCUMENT_ID])
);
GO

    CREATE SEQUENCE dbo.GenerateDocumentIdSequence  
	AS bigint
    START WITH 1  
    INCREMENT BY 1;  
GO
COMMIT;
GO
