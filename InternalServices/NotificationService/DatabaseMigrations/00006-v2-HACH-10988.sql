
Alter Table [dbo].[Notification] DROP COLUMN
    [DocumentHash],
    [HashAlgorithm],
    [CaseId]
Go

Alter Table [dbo].[Notification] ADD
    [DocumentHashes] nvarchar(max),
    [ProductId] varchar(100),
    [ProductType] int
Go