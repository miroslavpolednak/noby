-- GeneralDocumentList
CREATE TABLE [dbo].[GeneralDocumentList](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Filename] [nvarchar](250) NOT NULL,
	[Format] [nvarchar](10) NOT NULL
 CONSTRAINT [PK_GeneralDocumentList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


INSERT INTO [dbo].[GeneralDocumentList] ([Id],[Name],[Filename],[Format]) VALUES
(1,'Opis dokladu totožnosti','opis_dokladu_totoznosti.pdf','PDF'),
(2,'Potvrzení o příjmech','potvrzeni_o_prijmech.pdf','PDF'),
(3,'Sdělení čísla účtu pro splácení','sdeleni_cisla_uctu_pro_splaceni.pdf','PDF'),
(4,'Seznam dokumentů','seznam_dokumentu.pdf','PDF'),
(5,'Prohlášení zástavce','prohlaseni_zastavce.pdf','PDF'),
(6,'Čestné prohlášení o rodinném stavu poskytovatele zajištění','cestne_prohlaseni_o_rodinnem_stavu_poskytovatele_zajisteni.pdf','PDF'),
(7,'Kalkulačka příjmů z DAP','kalkulacka_prijmu_z_dap.xls','XLS'),
(8,'DTS šablona pro bytovou jednotku','dts_sablona_pro_bytovou_jednotku.pdf','PDF'),
(9,'DTS šablona pro rodinný dům','dts_sablona_pro_rodinny_dum.pdf','PDF'),
(10,'Informace k vyhodnocení rizik spojených s nemovitou zástavou','informace_k_vyhodnoceni_rizik_spojenych_s_nemovitou_zastavou.doc','DOC'),
(11,'Přehled investičních nákladů','prehled_investicnich_nakladu.xls','XLS'),
(12,'Navigátor příloh','navigator_priloh.xls','XLS');
GO