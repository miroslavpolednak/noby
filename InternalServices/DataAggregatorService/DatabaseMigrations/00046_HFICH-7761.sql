DELETE FROM DocumentDataField WHERE DocumentId IN (11, 12) AND AcroFieldName = 'DelkaFixace'

UPDATE DocumentDataField SET DefaultTextIfNull = '--' 
WHERE DocumentId IN (11, 12) AND DocumentVersion = '001' AND AcroFieldName IN ('PocetDetiDo10', 'PocetDetiNad10', 'NakladyBydleni', 'Pojisteni', 'Sporeni', 'OstatniVydaje')

INSERT INTO DocumentDynamicStringFormat VALUES (118, 11, '001', 'PocetDetiDo10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (118, '0', 117)

INSERT INTO DocumentDynamicStringFormat VALUES (119, 12, '001', 'PocetDetiDo10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (119, '0', 117)

INSERT INTO DocumentDynamicStringFormat VALUES (120, 11, '001', 'PocetDetiNad10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (120, '0', 118)

INSERT INTO DocumentDynamicStringFormat VALUES (121, 12, '001', 'PocetDetiNad10', '--', 1)
INSERT INTO DocumentDynamicStringFormatCondition VALUES (121, '0', 118)

UPDATE DocumentDataField 
SET StringFormat = 'nejsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a jmenování a osoba jemu blízká; člen bankovní rady České národní banky),'
WHERE DocumentId IN (11, 12) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem osobou se zvláštním vztahem k Bance ve smyslu ustanovení § 19 z.č. 21/1992 Sb., o bankách, ve znění pozdějších předpisů (za osobu se zvláštním vztahem k Bance se považuje např. člen statutárního orgánu Banky a osoba jemu blízká; člen správní rady a dozorčí rady Banky a člen výboru pro audit, rizika, odměňování a jmenování a osoba jemu blízká; člen bankovní rady České národní banky),' 
WHERE DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem1Odrazka', 'SpoluzadatelJsemNejsem1Odrazka')

UPDATE DocumentDataField 
SET StringFormat = 'nejsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů (za osobu blízkou k zaměstnanci Banky se považuje např. sourozenec, manžel nebo registrovaný partner, jiné osoby v poměru rodinném nebo obdobném se pokládají za osoby sobě navzájem blízké, pokud by újmu, kterou utrpěla jedna z nich, druhá důvodně pociťovala jako újmu vlastní),'
WHERE DocumentId IN (11, 12) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem2Odrazka', 'SpoluzadatelJsemNejsem2Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem osobou blízkou k zaměstnanci Banky ve smyslu ustanovení § 22 z.č. 89/2012 Sb., občanského zákoníku, ve znění pozdějších předpisů (za osobu blízkou k zaměstnanci Banky se považuje např. sourozenec, manžel nebo registrovaný partner, jiné osoby v poměru rodinném nebo obdobném se pokládají za osoby sobě navzájem blízké, pokud by újmu, kterou utrpěla jedna z nich, druhá důvodně pociťovala jako újmu vlastní),' 
WHERE DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem2Odrazka', 'SpoluzadatelJsemNejsem2Odrazka')

UPDATE DocumentDataField 
SET StringFormat = 'nejsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE DocumentId IN (11, 12) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o některých opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,' 
WHERE DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentDataField 
SET StringFormat = 'nejsem propojen s právnickou osobou (za propojení se považuje situace, kdy je žadatel o úvěr členem statutárního orgánu právnické osoby nebo je ovládající osobou právnické osoby, případně jedná ve shodě s jinou osobou při ovládání právnické osoby, nebo je zakladatelem nebo beneficientem/obmyšleným nadace, nadačního fondu, svěřenského fondu nebo je svěřenským správcem svěřenského fondu),'
WHERE DocumentId IN (11, 12) AND DocumentVersion = '001' AND AcroFieldName IN ('JsemNejsem4Odrazka', 'SpoluzadatelJsemNejsem4Odrazka')

UPDATE DocumentDynamicStringFormat 
SET StringFormat = 'jsem propojen s právnickou osobou (za propojení se považuje situace, kdy je žadatel o úvěr členem statutárního orgánu právnické osoby nebo je ovládající osobou právnické osoby, případně jedná ve shodě s jinou osobou při ovládání právnické osoby, nebo je zakladatelem nebo beneficientem/obmyšleným nadace, nadačního fondu, svěřenského fondu nebo je svěřenským správcem svěřenského fondu),' 
WHERE DocumentId IN (11, 12) AND AcroFieldName IN ('JsemNejsem4Odrazka', 'SpoluzadatelJsemNejsem4Odrazka')


INSERT INTO DocumentSpecialDataField 
VALUES (
11, 
'001', 
'ProhlaseniZadatele', 
10,
'Customer1.HasConfirmedContacts', 
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje. 

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. 

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v tomto dokumentu. Potvrzuji, že údaje uvedené tomto dokumentu slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v tomto dokumentu nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v tomto dokumentu těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentSpecialDataField 
VALUES (
12, 
'001', 
'ProhlaseniZadatele', 
10,
'Customer1.HasConfirmedContacts', 
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje. 

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. 

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v tomto dokumentu. Potvrzuji, že údaje uvedené tomto dokumentu slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v tomto dokumentu nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v tomto dokumentu těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')

INSERT INTO DocumentSpecialDataField 
VALUES (
11, 
'001', 
'ProhlaseniSpoluzadatele', 
10,
'Customer2.HasConfirmedContacts', 
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje. 

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v tomto dokumentu. Potvrzuji, že údaje uvedené tomto dokumentu slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v tomto dokumentu nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v tomto dokumentu těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.')

INSERT INTO DocumentSpecialDataField 
VALUES (
12, 
'001', 
'ProhlaseniSpoluzadatele', 
10,
'Customer2.HasConfirmedContacts', 
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje. 

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.',
4,
'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to na e-mail nebo telefon mnou uvedený v tomto dokumentu. Potvrzuji, že údaje uvedené tomto dokumentu slouží k veškeré naší komunikaci týkající se tohoto úvěru a případně k naší další běžné komunikaci, není-li sjednáno v konkrétním případě jinak, a že je mohu měnit jen způsobem Bankou akceptovaným. Tyto údaje slouží také pro zasílání smluvní dokumentace, beru proto na vědomí, že e-mailem uvedeným v tomto dokumentu nahrazuji e-mail pro zasílání smluvní dokumentace, mám-li jej sjednán.

Pokud si sjednám kontaktní e-mail nebo kontaktní telefon, u kterých potvrdím, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a zasílání hesel a kódů, pak budou údaje uvedené v tomto dokumentu těmito kontaktními údaji zcela nahrazeny. Ke vzájemné komunikaci mezi mnou a Bankou bude sloužit přednostně schránka v novém internetovém bankovnictví KB+, mám-li ji sjednanou, nebo můj kontaktní e-mail, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu.')


INSERT INTO DocumentSpecialDataField 
VALUES (
11, 
'001', 
'ProhlaseniSpoluzadateleCast2', 
10,
'Customer2.HasConfirmedContacts', 
'Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')


INSERT INTO DocumentSpecialDataField 
VALUES (
12, 
'001', 
'ProhlaseniSpoluzadateleCast2', 
10,
'Customer2.HasConfirmedContacts', 
'Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.',
4,
'Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu. Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.')
