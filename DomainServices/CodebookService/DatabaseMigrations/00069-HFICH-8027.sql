DROP TABLE IF EXISTS [dbo].[ProfessionCategory];
GO

CREATE TABLE [dbo].[ProfessionCategory](
	[ProfessionCategoryId] [int] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[IsValid] [bit] NULL,
 CONSTRAINT [PK_ProfessionCategory] PRIMARY KEY CLUSTERED 
(
	[ProfessionCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ProfessionCategory] ([ProfessionCategoryId], [Name], [IsValid]) VALUES
	(0,N'odmítl sdělit',1),
	(1,N'státní zaměstnanec',1),
	(2,N'zaměstnanec subjektu se státní majetkovou účastí',1),
	(3,N'zaměstnanec subjektu se zahraničním vlastníkem',1),
	(4,N'podnikatel',1),
	(5,N'zaměstnanec soukromé společnosti',1),
	(6,N'bez zaměstnání',1),
	(7,N'nezjištěno',1),
	(8,N'kombinace profesí',1);
GO

DELETE FROM SqlQuery WHERE SqlQueryId IN ('ProfessionCategories','ProfessionCategories1','ProfessionCategories2');
UPDATE SqlQuery SET SqlQueryId='ProfessionCategories2' where SqlQueryId='ProfessionCategories';
INSERT INTO SqlQuery (SqlQueryId, SqlQueryText, DatabaseProvider) VALUES ('ProfessionCategories1','SELECT * FROM dbo.ProfessionCategory',4);
INSERT INTO SqlQuery (SqlQueryId, SqlQueryText, DatabaseProvider) VALUES ('ProfessionCategories2','SELECT * FROM dbo.ProfessionCategoryExtension',4);
GO

DROP TABLE IF EXISTS [dbo].[SigningMethodsForNaturalPerson];
DROP TABLE IF EXISTS [dbo].[SigningMethodsForNaturalPersonExtension];
GO

CREATE TABLE [dbo].[SigningMethodsForNaturalPerson](
	[Code] [varchar](30) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[Order] [int] NULL,
	[IsValid] [bit] NULL,
 CONSTRAINT [PK_SigningMethodsForNaturalPerson] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SigningMethodsForNaturalPersonExtension](
	[Code] [varchar](30) NOT NULL,
	[StarbuildEnumId] [int] NULL,
 CONSTRAINT [PK_SigningMethodsForNaturalPersonExtension] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

DELETE FROM dbo.SqlQuery WHERE SqlQueryId IN ('SigningMethodsForNaturalPerson','SigningMethodsForNaturalPerson1','SigningMethodsForNaturalPerson2');
INSERT INTO dbo.SqlQuery (SqlQueryId, SqlQueryText, DatabaseProvider) VALUES ('SigningMethodsForNaturalPerson1','SELECT * FROM SigningMethodsForNaturalPerson',4);
INSERT INTO dbo.SqlQuery (SqlQueryId, SqlQueryText, DatabaseProvider) VALUES ('SigningMethodsForNaturalPerson2','SELECT * FROM SigningMethodsForNaturalPersonExtension',4);
GO

INSERT INTO SigningMethodsForNaturalPerson (Code, [Name], [Description], [Order], IsValid) VALUES
	('PAAT',N'KB klíč',null,1,1),
	('PHYSICAL',N'Ruční podpis',N'Fyzický/ruční podpis dokumentu.',1,1),
	('DELEGATE',N'Přímé bankovnictví',N'Přímé bankovnictví - Delegovaná metoda podpisu',1,1),
	('INT_CERT_FILE',N'Interní certifikát v souboru',null,2,1),
	('APOC',N'Automatizovaný Podpis Osobním Certifikátem',null,3,1),
	('OFFERED',N'Delegovaná metoda podpisu',N'deprecated',4,1);
INSERT INTO SigningMethodsForNaturalPersonExtension ([Code], StarbuildEnumId) VALUES ('PHYSICAL',1);
GO
