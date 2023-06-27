CREATE TABLE [dbo].[RealEstateSubtypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	ShortName nvarchar(100) NOT NULL,
	RealEstateTypeId int NOT NULL
 CONSTRAINT [PK_RealEstateSubtypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[RealEstateSubtypes] (Id, ShortName, [Name], RealEstateTypeId) VALUES
	(1, N'Rodinný dům do 6 obytných místností', N'Rodinný dům do 6 obytných místností', 1);
