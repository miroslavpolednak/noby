SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Result] SET (SYSTEM_VERSIONING = OFF)
GO

-- DROP TABLE [dbo].[Result]
-- GO

CREATE TABLE [dbo].[Result]
(
    [Id] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [State] [int] NOT NULL,
    [Channel] [int] NOT NULL,
    [Errors] [nvarchar](max) NOT NULL,

    [Created] [datetime] NOT NULL,
    [Updated] [datetime] NOT NULL
    
    CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )
    WITH
    (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
)
ON [PRIMARY]
GO