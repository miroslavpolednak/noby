SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- table 'SalesArrangementType'
-- DROP TABLE IF EXISTS [dbo].[SalesArrangementType];
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SalesArrangementType')
BEGIN
	CREATE TABLE [dbo].[SalesArrangementType](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](150) NOT NULL,
		[ProductTypeId] [int] NULL
		CONSTRAINT [PK_SalesArrangementType] PRIMARY KEY CLUSTERED
		(
		[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	INSERT INTO [dbo].[SalesArrangementType]([Id],[Name],[ProductTypeId])
   VALUES
    (1, 'Žádost o hypotéční úvěr', 20001),
	(2, 'Žádost o hypoteční překlenovací úvěry', 20002),
	(3, 'Žádost o hypoteční úvěr bez příjmu', 20003),
	(4, 'Žádost o doprodej neúčelové části', 20004),
	(5, 'Žádost o americkou hypotéku', 20010);
END


-- table 'ProductTypeExtension'
DROP TABLE IF EXISTS [dbo].[ProductTypeExtension];
CREATE TABLE [dbo].[ProductTypeExtension](
	[ProductTypeId] [int] NOT NULL,
	[MpHomeApiLoanType] [varchar](50) NULL,
	[KonsDbLoanType] [tinyint] NOT NULL,
CONSTRAINT [PK_ProductTypeExtension] PRIMARY KEY CLUSTERED 
(
	[ProductTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

	--INSERT INTO [dbo].[ProductTypeExtension]([ProductTypeId],[MpHomeApiLoanType],[KonsDbLoanType])
	--VALUES
	--(20001, 'KBMortgage', 3),
	--(20002, 'KBBridgingMortgageLoan', 4),
	--(20003, 'KBMortgageWithoutIncome', 5),
	--(20004, 'KBMortgageLoanNonPurposePart', 6),
	--(20010, 'KBAmericanMortgage', 7);

	-- dočasné mapování (Do rozšíření KonsDB o ProductTypeId) [https://wiki.kb.cz/pages/viewpage.action?pageId=392883923]
	INSERT INTO [dbo].[ProductTypeExtension]([ProductTypeId],[MpHomeApiLoanType],[KonsDbLoanType])
    VALUES
    (20001, 'KBMortgage', 3),
	(20002, 'KBMortgage', 4),
	(20003, 'KBMortgage', 5),
	(20004, 'KBMortgage', 6),
	(20010, 'KBMortgage', 7);
GO

-- table 'RelationshipCustomerProductTypeExtension'
DROP TABLE IF EXISTS [dbo].[RelationshipCustomerProductTypeExtension];
CREATE TABLE [dbo].[RelationshipCustomerProductTypeExtension](
    [RelationshipCustomerProductTypeId] [int] NOT NULL,
    [RdmCode] [varchar](50) NULL,
	[MpDigiApiCode] [varchar](50) NULL,
	[NameNoby] [varchar](50) NULL,
    CONSTRAINT [PK_RelationshipCustomerProductTypeExtension] PRIMARY KEY CLUSTERED
    (
    [RelationshipCustomerProductTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[RelationshipCustomerProductTypeExtension]([RelationshipCustomerProductTypeId],[RdmCode],[MpDigiApiCode],[NameNoby])
    VALUES
    (0, null, 'NotSpecified', 'nezadán'),
	(1, 'A', 'Owner', 'Hlavní dlužník'),
	(2, 'S', 'CoDebtor', 'Spoludlužník'),
	(3, null, 'Accessor', 'Přistupitel'),
	(4, null, 'HusbandOrWife', 'Manžel-ka'),
	(5, null, 'LegalRepresentative', 'Zákonný zástupce'),
	(6, null, 'CollisionGuardian', 'Kolizní opatrovník'),
	(7, null, 'Guardian', 'Opatrovník'),
	(8, 'R', 'Guarantor', 'Ručitel'),
	(9, null, 'GuarantorHusbandOrWife', 'Manžel-ka ručitele'),
	(11, null, 'Intermediary', 'Sprostředkovatel'),
	(12, null, 'ManagingDirector', 'Jednatel'),
	(13, null, 'Child', 'Dítě');
GO

-- table 'WorkflowTaskTypeExtension'
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'WorkflowTaskTypeExtension')
BEGIN
    CREATE TABLE [dbo].[WorkflowTaskTypeExtension](
    [WorkflowTaskTypeId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL,
    CONSTRAINT [PK_WorkflowTaskTypeExtension] PRIMARY KEY CLUSTERED
    (
    [WorkflowTaskTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	INSERT INTO [dbo].[WorkflowTaskTypeExtension]([WorkflowTaskTypeId],[CategoryId])
    VALUES
    (4220, 1),
	(42210, 1),
	(42220, 1),
	(4245, 2),
	(4246, 2),
	(42461, 2),
	(4247, 2),
	(42471, 2),
	(4250, 3),
	(4251, 3);
END


-- table 'WorkflowTaskStateExtension'
-- DROP TABLE IF EXISTS [dbo].[WorkflowTaskStateExtension];
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'WorkflowTaskStateExtension')
BEGIN
    CREATE TABLE [dbo].[WorkflowTaskStateExtension](
    [WorkflowTaskStateId] [int] NOT NULL,
    [Flag] [tinyint] NOT NULL,
    CONSTRAINT [PK_WorkflowTaskStateExtension] PRIMARY KEY CLUSTERED
    (
    [WorkflowTaskStateId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	INSERT INTO [dbo].[WorkflowTaskStateExtension]([WorkflowTaskStateId],[Flag])
    VALUES
	(0, 1),
	(30, 1);
END


-- table 'ContactTypeExtension'
DROP TABLE IF EXISTS [dbo].[ContactTypeExtension];
CREATE TABLE [dbo].[ContactTypeExtension](
    [ContactTypeId] [int] NOT NULL,
	[MpDigiApiCode] [varchar](50) NULL,
    CONSTRAINT [PK_ContactTypeExtension] PRIMARY KEY CLUSTERED
    (
    [ContactTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ContactTypeExtension]([ContactTypeId],[MpDigiApiCode])
    VALUES
    (1, 'Mobile'),
	(2, 'BusinessMobile'),
	(3, 'FixedHomeLine'),
	(5, 'Email');
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
