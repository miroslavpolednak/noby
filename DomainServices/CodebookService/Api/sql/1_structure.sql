SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SalesArrangementType](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](150) NOT NULL,
    [ProductTypeId] [int] NULL,
    [IsDefault] [bit] NOT NULL,
    CONSTRAINT [PK_SalesArrangementType] PRIMARY KEY CLUSTERED
    (
    [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ProductTypeExtension](
    [ProductTypeId] [int] NOT NULL,
    [ProductCategory] [tinyint] NOT NULL,
    [MpHomeApiLoanType] [varchar](50) NULL,
    [KonsDbLoanType] [tinyint] NOT NULL,
    CONSTRAINT [PK_ProductTypeExtension] PRIMARY KEY CLUSTERED
    (
    [ProductTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[RelationshipCustomerProductTypeExtension](
    [RelationshipCustomerProductTypeId] [int] NOT NULL,
    [MpHomeApiContractRelationshipType] [varchar](50) NOT NULL,
    CONSTRAINT [PK_RelationshipCustomerProductTypeExtension] PRIMARY KEY CLUSTERED
    (
    [RelationshipCustomerProductTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/*
TRUNCATE TABLE [dbo].[RelationshipCustomerProductTypeExtension];
GO

INSERT INTO [dbo].[RelationshipCustomerProductTypeExtension]
           ([RelationshipCustomerProductTypeId],[MpHomeApiContractRelationshipType])
     VALUES
           (0,'NotSpecified'),
		   (1,'Owner'),
		   (2,'CoDebtor'),
		   (3,'Accessor'),
		   (4,'HusbandOrWife'),
		   (5,'LegalRepresentative'),
		   (6,'CollisionGuardian'),
		   (7,'Guardian'),
		   (8,'Guarantor'),
		   (9,'GuarantorHusbandOrWife'),
		   (11,'Intermediary'),
		   (12,'ManagingDirector'),
		   (13,'Child');
GO
*/