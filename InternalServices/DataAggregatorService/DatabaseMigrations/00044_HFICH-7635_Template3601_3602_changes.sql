UPDATE DocumentDataField SET StringFormat = '{0}.den v měsíci' WHERE DocumentId = 4 AND DocumentVersion = '001' AND AcroFieldName = 'DenSplaceni'
UPDATE DocumentDataField SET DataFieldId = 72 WHERE DocumentId IN (4, 5) AND DocumentVersion = '001' AND AcroFieldName = 'ZahajeniCerpani'

UPDATE DocumentDataField 
SET StringFormat = 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId IN (4, 5, 16) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a jmenování a osoba jemu blízká; člen bankovní rady České národní banky),' 
WHERE DynamicStringFormatId IN (45, 55, 85, 50, 60, 90)

UPDATE DocumentDataField 
SET StringFormat = 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů (za osobu blízkou k zaměstnanci Banky se považuje např. sourozenec, manžel nebo registrovaný partner, jiné osoby v poměru rodinném nebo obdobném se pokládají za osoby sobě navzájem blízké, pokud by újmu, kterou utrpěla jedna z nich, druhá důvodně pociťovala jako újmu vlastní),'
WHERE DocumentId IN (4, 5, 16) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem2Odrazka', 'SpoluzadatelJsemNejsem2Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů (za osobu blízkou k zaměstnanci Banky se považuje např. sourozenec, manžel nebo registrovaný partner, jiné osoby v poměru rodinném nebo obdobném se pokládají za osoby sobě navzájem blízké, pokud by újmu, kterou utrpěla jedna z nich, druhá důvodně pociťovala jako újmu vlastní),' 
WHERE DynamicStringFormatId IN (46, 56, 86, 51, 61, 91)

UPDATE DocumentDataField 
SET StringFormat = 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId IN (4, 5, 16) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,' 
WHERE DynamicStringFormatId IN (47, 57, 87, 52, 62, 92)

UPDATE DocumentDataField 
SET StringFormat = 'nejsem propojen s právnickou osobou (za propojení se považuje situace, kdy je žadatel o úvěr členem statutárního orgánu právnické osoby nebo je ovládající osobou právnické osoby, případně jedná ve shodě s jinou osobou při ovládání právnické osoby, nebo je zakladatelem nebo beneficientem/obmyšleným nadace, nadačního fondu, svěřenského fondu nebo je svěřenským správcem svěřenského fondu),'
WHERE DocumentId IN (4, 5, 16) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem4Odrazka', 'SpoluzadatelJsemNejsem4Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem propojen s právnickou osobou (za propojení se považuje situace, kdy je žadatel o úvěr členem statutárního orgánu právnické osoby nebo je ovládající osobou právnické osoby, případně jedná ve shodě s jinou osobou při ovládání právnické osoby, nebo je zakladatelem nebo beneficientem/obmyšleným nadace, nadačního fondu, svěřenského fondu nebo je svěřenským správcem svěřenského fondu),' 
WHERE DynamicStringFormatId IN (48, 58, 88, 53, 63, 93)

INSERT INTO DocumentSpecialDataField 
VALUES (4, 
	    '001', 
		'ProhlaseniZadatele', 
		9,
		'DebtorCustomer.HasConfirmedContacts', 
		'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený')


INSERT INTO DocumentSpecialDataField 
VALUES (4, 
	    '001', 
		'ProhlaseniZadateleCast2', 
		9,
		'DebtorCustomer.HasConfirmedContacts', 
		'',
4,
'prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentSpecialDataField 
VALUES (4, 
	    '001', 
		'ProhlaseniSpoluzadatele', 
		9,
		'CodebtorCustomer.HasConfirmedContacts', 
		'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

UPDATE DocumentDataField SET DefaultTextIfNull = '--' 
WHERE DocumentId IN (4, 5, 16) AND DocumentVersion = '001' AND AcroFieldName IN ('PocetDetiDo10', 'PocetDetiNad10', 'NakladyBydleni', 'Pojisteni', 'Sporeni', 'OstatniVydaje')

INSERT INTO DocumentDynamicStringFormat VALUES (112, 4, '001', 'PocetDetiDo10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (112, '0', 105)

INSERT INTO DocumentDynamicStringFormat VALUES (113, 5, '001', 'PocetDetiDo10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (113, '0', 117)

INSERT INTO DocumentDynamicStringFormat VALUES (114, 16, '001', 'PocetDetiDo10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (114, '0', 117)

INSERT INTO DocumentDynamicStringFormat VALUES (115, 4, '001', 'PocetDetiNad10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (115, '0', 106)

INSERT INTO DocumentDynamicStringFormat VALUES (116, 5, '001', 'PocetDetiNad10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (116, '0', 118)

INSERT INTO DocumentDynamicStringFormat VALUES (117, 16, '001', 'PocetDetiNad10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (117, '0', 118)

INSERT DocumentDataField VALUES (4, '001', 'Zprostredkovatel', 27, NULL, NULL, NULL)
INSERT DocumentDataField VALUES (5, '001', 'Zprostredkovatel', 27, NULL, NULL, NULL)
INSERT DocumentDataField VALUES (16, '001', 'Zprostredkovatel', 27, NULL, NULL, NULL)

INSERT INTO DocumentSpecialDataField VALUES (4, '001', 'ZprostredkovatelTelEmail', 4, 'BrokerEmailAndPhone', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (5, '001', 'ZprostredkovatelTelEmail', 4, 'BrokerEmailAndPhone', NULL, NULL, NULL)
INSERT INTO DocumentSpecialDataField VALUES (16, '001', 'ZprostredkovatelTelEmail', 4, 'BrokerEmailAndPhone', NULL, NULL, NULL)

UPDATE DocumentSpecialDataField SET FieldPath = 'DebtorCustomer.SignerName' WHERE DocumentId = 4 AND DocumentVersion = '001' AND AcroFieldName = 'PodpisJmenoKlienta'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer1.SignerName' WHERE DocumentId = 5 AND DocumentVersion = '001' AND AcroFieldName = 'PodpisJmenoKlienta'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer1.SignerName' WHERE DocumentId = 16 AND DocumentVersion = '001' AND AcroFieldName = 'PodpisJmenoKlienta'

UPDATE DocumentSpecialDataField SET FieldPath = 'CodebtorCustomer.SignerName' WHERE DocumentId = 4 AND DocumentVersion = '001' AND AcroFieldName = 'PodpisJmenoKlienta2'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer2.SignerName' WHERE DocumentId = 5 AND DocumentVersion = '001' AND AcroFieldName = 'PodpisJmenoKlienta2'
UPDATE DocumentSpecialDataField SET FieldPath = 'Customer2.SignerName' WHERE DocumentId = 16 AND DocumentVersion = '001' AND AcroFieldName = 'PodpisJmenoKlienta2'

DELETE FROM DocumentSpecialDataField WHERE DocumentId IN (5, 16) AND AcroFieldName = 'SplatnostLabel'

INSERT INTO DocumentDataField VALUES (5, '001', 'SplatnostLabel', 207, 'Splatnost úvěru', NULL, NULL)
INSERT INTO DocumentDataField VALUES (16, '001', 'SplatnostLabel', 207, 'Předpokládané datum splatnosti', NULL, NULL)


INSERT INTO DocumentSpecialDataField VALUES (
5,
'001',
'ProhlaseniZadatele', 10,
'Customer1.HasConfirmedContacts',
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentSpecialDataField VALUES (
5,
'001',
'ProhlaseniSpoluzadatele', 10,
'Customer2.HasConfirmedContacts',
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit')

INSERT INTO DocumentSpecialDataField VALUES (
5,
'001',
'ProhlaseniSpoluzadateleCast2', 10,
'Customer2.HasConfirmedContacts',
'Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentSpecialDataField VALUES (
16,
'001',
'ProhlaseniZadatele', 10,
'Customer1.HasConfirmedContacts',
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentSpecialDataField VALUES (
16,
'001',
'ProhlaseniSpoluzadatele', 10,
'Customer2.HasConfirmedContacts',
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v této žádosti. Potvrzuji, že údaje uvedené v této žádosti slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v této žádosti nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v této žádosti těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit')

INSERT INTO DocumentSpecialDataField VALUES (
16,
'001',
'ProhlaseniSpoluzadateleCast2', 10,
'Customer2.HasConfirmedContacts',
'Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadatele', 'A')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadatele', 'B')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadatele', 'C')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadatele', 'D')

INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadateleCast2', 'A')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadateleCast2', 'B')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadateleCast2', 'C')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniZadateleCast2', 'D')

INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniSpoluzadatele', 'C')
INSERT INTO DocumentVariant VALUES (4, '001', 'ProhlaseniSpoluzadatele', 'D')

INSERT INTO DocumentVariant VALUES (4, '001', 'Zprostredkovatel', 'A')
INSERT INTO DocumentVariant VALUES (4, '001', 'Zprostredkovatel', 'C')

INSERT INTO DocumentVariant VALUES (4, '001', 'ZprostredkovatelTelEmail', 'A')
INSERT INTO DocumentVariant VALUES (4, '001', 'ZprostredkovatelTelEmail', 'C')

INSERT INTO DocumentVariant VALUES (5, '001', 'Zprostredkovatel', 'A')
INSERT INTO DocumentVariant VALUES (5, '001', 'Zprostredkovatel', 'C')

INSERT INTO DocumentVariant VALUES (5, '001', 'ZprostredkovatelTelEmail', 'A')
INSERT INTO DocumentVariant VALUES (5, '001', 'ZprostredkovatelTelEmail', 'C')

INSERT INTO DocumentVariant VALUES (16, '001', 'Zprostredkovatel', 'A')
INSERT INTO DocumentVariant VALUES (16, '001', 'Zprostredkovatel', 'C')

INSERT INTO DocumentVariant VALUES (16, '001', 'ZprostredkovatelTelEmail', 'A')
INSERT INTO DocumentVariant VALUES (16, '001', 'ZprostredkovatelTelEmail', 'C')

INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniZadatele', 'A')
INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniZadatele', 'B')
INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniZadatele', 'C')
INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniZadatele', 'D')

INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniZadatele', 'A')
INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniZadatele', 'B')
INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniZadatele', 'C')
INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniZadatele', 'D')

INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniSpoluzadatele', 'C')
INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniSpoluzadatele', 'D')

INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniSpoluzadatele', 'C')
INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniSpoluzadatele', 'D')

INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniSpoluzadateleCast2', 'C')
INSERT INTO DocumentVariant VALUES (5, '001', 'ProhlaseniSpoluzadateleCast2', 'D')

INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniSpoluzadateleCast2', 'C')
INSERT INTO DocumentVariant VALUES (16, '001', 'ProhlaseniSpoluzadateleCast2', 'D')