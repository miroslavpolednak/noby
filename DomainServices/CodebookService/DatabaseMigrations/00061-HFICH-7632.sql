UPDATE CodebookService.dbo.DocumentTypes set EACodeMainId = 616574 WHERE Id = 13;

DELETE FROM CodebookService.dbo.EaCodesMainExtension WHERE EaCodesMainId=605569; -- DocumentTypes.Id = 1
DELETE FROM CodebookService.dbo.EaCodesMainExtension WHERE EaCodesMainId=616578; -- DocumentTypes.Id = 13
DELETE FROM CodebookService.dbo.EaCodesMainExtension WHERE EaCodesMainId=608522; -- DocumentTypes.Id = 15
DELETE FROM CodebookService.dbo.EaCodesMainExtension WHERE EaCodesMainId=616525; -- DocumentTypes.Id = 14 (zmena klientskych udaju)
INSERT INTO CodebookService.dbo.EaCodesMainExtension (EaCodesMainId, IsFormIdRequested) VALUES(608579, 1); -- DocumentTypes.Id = 10
INSERT INTO CodebookService.dbo.EaCodesMainExtension (EaCodesMainId, IsFormIdRequested) VALUES(616574, 1); -- DocumentTypes.Id = 13

UPDATE CodebookService.dbo.GeneralDocumentList set Name = 'Identifikace klienta', Filename  = 'identifikace_klienta.pdf' WHERE Id = 1;
UPDATE CodebookService.dbo.GeneralDocumentList set Name = 'Potvrzení o výši pracovního příjmu', Filename  = 'potvrzeni_o_vysi_pracovniho_prijmu.pdf' WHERE Id = 2;
UPDATE CodebookService.dbo.GeneralDocumentList set Name = 'Sdělení čísla účtu ke splácení úvěru', Filename  = 'sdeleni_cisla_uctu_ke_splaceni_uveru.pdf' WHERE Id = 3;
UPDATE CodebookService.dbo.GeneralDocumentList set Name = 'Seznam dokladů', Filename  = 'seznam_dokladu.pdf' WHERE Id = 4;
UPDATE CodebookService.dbo.GeneralDocumentList set Name = 'Čestné prohlášení o rodinném stavu zástavce', Filename  = 'cestne_prohlaseni_o_rodinnem_stavu_zastavce.pdf' WHERE Id = 6;

INSERT INTO CodebookService.dbo.GeneralDocumentList (Id,Name,Filename,Format) VALUES (13,N'Zápis z jednání o HÚ (developerský projekt)',N'zapis_z_jednani_o_hu_dp.pdf',N'PDF'); 