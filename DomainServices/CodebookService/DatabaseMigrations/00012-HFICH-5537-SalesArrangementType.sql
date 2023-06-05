-- SalesArrangementType
ALTER TABLE [dbo].[SalesArrangementType] ADD [Description] [nvarchar](250) NULL;
GO

-- SalesArrangementType [data]
TRUNCATE TABLE [dbo].[SalesArrangementType];
GO

INSERT INTO [dbo].[SalesArrangementType] ([Id],[Name],[Description],[ProductTypeId],[SalesArrangementCategory]) VALUES
(1,'Žádost o hypotéční úvěr',NULL,20001,1),
(2,'Žádost o hypoteční překlenovací úvěry',NULL,20002,1),
(3,'Žádost o hypoteční úvěr bez příjmu',NULL,20003,1),
(4,'Žádost o doprodej neúčelové části',NULL,20004,1),
(5,'Žádost o americkou hypotéku',NULL,20010,1),
(6,'Žádost o čerpání','Formulář k podání žádosti o čerpání',NULL,2),
(7,'Žádost o změnu podmínek smlouvy o hypotečním úvěru','Formulář k podání žádosti o změnu hypotečního úvěru',NULL,2),
(8,'Žádost o změnu/doplnění podmínek smlouvy o hypotečním úvěru bez nemovitosti','Formulář k vyplnění detailních informací k přípravě dodatku k hypotéce bez nemovitosti',NULL,2),
(9,'Žádost o změnu dlužníků','Formulář k podání žádosti o změnu dlužníků - přistoupení / uvolnění',NULL,2),
(10,'Údaje o přistupujícím k dluhu','Formulář pro vyplnění informací o klientovi a jeho domácnosti, který přistupuje k dluhu',NULL,2),
(11,'Žádost o přidání spoludlužníka','Formulář k přidání spoludlužníka do rozpracované žádosti o úvěr',NULL,2),
(12,'Údaje o zůstávajícím v dluhu','Formulář pro vyplnění informací o klientovi a jeho domácnosti, který zůstává v dluhu',NULL,2);
GO