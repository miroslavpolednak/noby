SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- table 'SalesArrangementType'
DROP TABLE IF EXISTS [dbo].[SalesArrangementType];
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SalesArrangementType')
BEGIN
	CREATE TABLE [dbo].[SalesArrangementType](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](150) NOT NULL,
		[ProductTypeId] [int] NULL,
		[SalesArrangementCategory] [int] NOT NULL
		CONSTRAINT [PK_SalesArrangementType] PRIMARY KEY CLUSTERED
		(
		[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	INSERT INTO [dbo].[SalesArrangementType]([Id],[Name],[ProductTypeId], [SalesArrangementCategory])
   VALUES
    (1, 'Žádost o hypotéční úvěr', 20001, 1),
	(2, 'Žádost o hypoteční překlenovací úvěry', 20002, 1),
	(3, 'Žádost o hypoteční úvěr bez příjmu', 20003, 1),
	(4, 'Žádost o doprodej neúčelové části', 20004, 1),
	(5, 'Žádost o americkou hypotéku', 20010, 1),
	(6, 'Žádost o čerpání', NULL, 2),
	(7, 'Žádost o obecnou změnu', NULL, 2),
	(8, 'Žádost o změnu HUBN', NULL, 2);
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
	(5, 'Email'),
	(13, 'Mobile'),
	(14, 'Email');
GO

-- table 'ObligationTypeExtension'
DROP TABLE IF EXISTS [dbo].[ObligationTypeExtension];
CREATE TABLE [dbo].[ObligationTypeExtension](
    [ObligationTypeId] [int] NOT NULL,
	[ObligationProperty] [varchar](50) NULL,
    CONSTRAINT [PK_ObligationTypeExtension] PRIMARY KEY CLUSTERED
    (
    [ObligationTypeId] ASC
     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ObligationTypeExtension]([ObligationTypeId],[ObligationProperty])
    VALUES
    (1, 'amount'),
	(2, 'amount'),
	(3, 'limit'),
	(4, 'limit'),
	(5, 'amount');
GO


-- table 'DocumentOnSAType'
DROP TABLE IF EXISTS [dbo].[DocumentOnSAType];
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DocumentOnSAType')
BEGIN
	CREATE TABLE [dbo].[DocumentOnSAType](
		[Id] [int] NOT NULL,
		[Name] [nvarchar](150) NOT NULL,
		[SalesArrangementTypeId] [int] NULL,
		[FormTypeId] [int] NOT NULL
		CONSTRAINT [PK_DocumentOnSAType] PRIMARY KEY CLUSTERED
		(
		[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	INSERT INTO [dbo].[DocumentOnSAType]([Id],[Name],[SalesArrangementTypeId], [FormTypeId])
   VALUES
    (1, 'Žádost o poskytnutí úvěru', NULL, 3601001),
	(2, 'Prohlášení účastníka k žádosti o úvěr (spolužadatelská domácnost)', NULL, 3602001),
	(3, 'Prohlášení účastníka k žádosti o úvěr (ručitelská domácnost)', NULL, 3602001),
	(4, 'Žádost o čerpání', 6, 3700001);
END


-- table 'IdentificationDocumentTypeExtension'
DROP TABLE IF EXISTS [dbo].[IdentificationDocumentTypeExtension];
CREATE TABLE [dbo].[IdentificationDocumentTypeExtension](
	[IdentificationDocumentTypeId] [int] NOT NULL,
	[MpDigiApiCode] [varchar](20) NULL,
 CONSTRAINT [PK_IdentificationDocumentTypeExtension] PRIMARY KEY CLUSTERED 
(
	[IdentificationDocumentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

	INSERT INTO [dbo].[IdentificationDocumentTypeExtension]([IdentificationDocumentTypeId],[MpDigiApiCode])
    VALUES
    (0, 'Undefined'),
	(1, 'IDCard'),
	(2, 'Passport'),
	(3, 'ResidencePermit'),
	(4, 'Foreign');
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

-- table 'ProfessionCategoryExtension'
DROP TABLE IF EXISTS [dbo].[ProfessionCategoryExtension];
CREATE TABLE [dbo].[ProfessionCategoryExtension](
	[ProfessionCategoryId] [int] NOT NULL,
	[ProfessionIds] [nvarchar](100) NULL,
CONSTRAINT [PK_ProfessionCategoryExtension] PRIMARY KEY CLUSTERED 
(
	[ProfessionCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

	INSERT INTO [dbo].[ProfessionCategoryExtension]([ProfessionCategoryId],[ProfessionIds])
    VALUES
	(0, '0'),
    (1, '3, 4, 7, 8, 9, 1, 2, 5, 6, 10, 12');
GO


-- table 'NetMonthEarningsExtension'
DROP TABLE IF EXISTS [dbo].[NetMonthEarningsExtension];
CREATE TABLE [dbo].[NetMonthEarningsExtension](
	[NetMonthEarningId] [int] NOT NULL,
	[RdmCode] [varchar](50) NULL,
CONSTRAINT [PK_NetMonthEarningsExtension] PRIMARY KEY CLUSTERED 
(
	[NetMonthEarningId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

	INSERT INTO [dbo].[NetMonthEarningsExtension]([NetMonthEarningId],[RdmCode])
    VALUES
	(1, 'A'),
	(2, 'B'),
	(3, 'C'),
	(4, 'D'),
	(5, 'E');
GO

DROP TABLE IF EXISTS [dbo].IncomeMainTypesAMLExtension;
CREATE TABLE [dbo].IncomeMainTypesAMLExtension(
	Id [int] NOT NULL,
	[RdmCode] [varchar](50) NULL,
CONSTRAINT [PK_IncomeMainTypesAMLExtension] PRIMARY KEY CLUSTERED 
(
	Id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

	INSERT INTO [dbo].IncomeMainTypesAMLExtension(Id,[RdmCode])
    VALUES
	(1, '1'),
	(2, '2'),
	(3, '3'),
	(4, '4'),
	(5, '5'),
	(6, '6');
GO


-- table 'DocumentTemplateVersion'
DROP TABLE IF EXISTS [dbo].[DocumentTemplateVersion];
CREATE TABLE [dbo].[DocumentTemplateVersion](
	[DocumentTemplateVersionId] [int] NOT NULL,
	[DocumentTemplateTypeId] [int] NOT NULL,
	[DocumentVersion] [nvarchar](50) NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
CONSTRAINT [PK_DocumentTemplateVersion] PRIMARY KEY CLUSTERED 
(
	[DocumentTemplateVersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

	INSERT INTO [dbo].[DocumentTemplateVersion]([DocumentTemplateVersionId], [DocumentTemplateTypeId], [DocumentVersion], [ValidFrom])
    VALUES
	(1, 1, '001A', '2022-01-01'),
	(2, 2, '001A', '2022-01-01'),
	(3, 3, '001A', '2022-01-01'),
	(4, 4, '001A', '2022-01-01'),
	(5, 5, '001A', '2022-01-01'),
	(6, 6, '001A', '2022-01-01');
GO

CREATE TABLE [dbo].[SmsNotificationType](
	[Id] [int] NOT NULL,
	[Code] [varchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SmsText] [nvarchar](max) NULL,
	[McsCode] [varchar](100) NULL,
	[IsAuditLogEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_SmsNotificationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


-- table 'DocumentTypes'
DROP TABLE IF EXISTS [dbo].[DocumentTypes];
CREATE TABLE [dbo].[DocumentTypes](
	[Id] [int] NOT NULL,
	[ShortName] [varchar](20) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[FileName] [varchar](200) NOT NULL,
	[SalesArrangementTypeId] [int] NULL,
	[FormTypeId] [bigint] NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

	INSERT INTO [dbo].[DocumentTypes]([Id], [ShortName], [Name], [FileName], [SalesArrangementTypeId], [FormTypeId], [ValidFrom], [ValidTo])
    VALUES
    (1, 'NABIDKA', 'Nabídka hypotečního úvěru', 'Nabidka_hypotecniho_uveru', NULL, NULL, '2022-01-01', NULL),
	(2, 'KALKULHU', 'Hypoteční úvěr - kalkulace', 'Kalkulace_hypotecniho_uveru', NULL, NULL, '2022-01-01', NULL),
	(3, 'SPLKALHU', 'Simulace splátkového kalendáře', 'Simulace_splatkoveho_kalendare', NULL, NULL, '2022-01-01', NULL),
	(4, 'ZADOSTHU', 'Žádost o poskytnutí hypotečního úvěru - první domácnost', 'Zadost_o_poskytnuti_hypotecniho_uveru_d1', NULL, 3601001, '2022-01-01', NULL),
	(5, 'ZADOSTHD', 'Žádost o poskytnutí hypotečního úvěru - druhá domácnost', 'Zadost_o_poskytnuti_hypotecniho_uveru_d2', NULL, 3602001, '2022-01-01', NULL),
	(6, 'ZADOCERP', 'Žádost o čerpání hypotečního úvěru', 'Zadost_o_cerpani_hypotecniho_uveru', 6, 3700001, '2022-01-01', NULL),
	(7, 'SDELUCET', 'Sdělení čísla účtu pro čerpání', 'Sdeleni_cisla_uctu_pro_cerpani', NULL, NULL, '2022-01-01', NULL),
	(8, 'ZAOZMPAR', 'Žádost o změnu obecná', 'Zadost_o_zmenu_obecna', NULL, NULL, '2022-01-01', NULL),
	(9, 'ZAOZMDLU', 'Žádost o změnu dlužníka', 'Zadost_o_zmenu_dluznika', NULL, 3602001, '2022-01-01', NULL),
	(10, 'ZAODHUBN', 'Žádost o změnu - HÚ bez nemovitosti', 'Zadost_o_zmena_hu_bez_nemovitosti', NULL, NULL, '2022-01-01', NULL),
	(11, 'ZADOOPCI', 'Žádost o změnu Flexi', 'Zadost_o_zmena_Flexi', NULL, NULL, '2022-01-01', NULL);
GO

