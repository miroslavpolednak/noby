from datetime import date
from datetime import datetime
from datetime import timedelta


from Pipeline_root import Pipeline_root
from Attribute_iterator import Attribute_iterator
from Attribute_loop import Attribute_loop
from Attribute_array import Attribute_array
from Attribute_struct import Attribute_struct
from Cleaner import Cleaner
from Writer import Writer
from Writer_sqlite import Writer_sqlite
from Flag import Flag

from loan_purposes_eval import loan_purposes_eval

#Nultý prvek seznamu musí být typu PipelineRoot. Ten inicializuje výchozí dokument.
pipe=next=Pipeline_root()


#Normální/americká hypotéka
product_type=next=Attribute_iterator("ProductTypeId",next,[20001,20010])

#Ve variantě s nemovitostí/bez
loan_kind=next=Attribute_iterator("LoanKindId",next,[2000,2001])

#Americká hypotéka bez nemovitosti není validní kombinace
loan_kind.register_filter(lambda doc: not (doc['LoanKindId']==2001 and doc['ProductTypeId']==20010))

#Zkusíme pro hodnotu zástavy 4.000.000Kč a 5.500.000Kč
collateral_amount=next=Attribute_iterator("CollateralAmount",next,[4000000,5500000])
#Aby nevzniklo příliš mnoho kombinací, vyzkoušíme pouze hypotéku s nemovitostí se zástavou 5.500.000Kč
#a hypotéku bez nemovitosti se zástavou 4.000.000Kč (kombinace nultý a prvním a první s nultým elementem pole)
collateral_amount.register_filter(lambda doc: doc['__LoanKindId']['count']+doc['__CollateralAmount']['count']==1)

#Poskytneme úvěry na 80% zástavy +/- 1Kč a 90% zástavy +/- 1Kč
loan_amount=next=Attribute_iterator("LoanAmount",next,[(80,-1),(80,0),(80,1),(90,-1),(90,0),(90,1)])
loan_amount.register_eval(lambda doc,val: doc['CollateralAmount']*val[0]/100+val[1])
#Aby nevzniklo příliš mnoho kombinací, vyzkoušíme všechny varianty výše úvěru pouze u Standardní hypotéky s nemovitostí
loan_amount.register_filter(lambda doc: (doc['LoanKindId']==2000 and doc['ProductTypeId']==20001) or (doc['__LoanAmount']['count']==1))
#Pro další použití si označíme jako hlavní Case ten, kde je hypotéka přesně 80% zástavy. To nám zjednoduší podmínky v implementaci dalších filtrů
flag_main=next=Flag("mainCase",next,lambda doc:(doc['LoanKindId']==2000 and doc['ProductTypeId']==20001) and (doc['__LoanAmount']['count']==1))

#Pro hlavní případ (filtr o řádku níže) vyzkoušíme termíny čerpání plus 30 dní, zítra, dnes, včera. Pro ostatní případy zkoušíme pouze variantu s čerpáním za 30 dní.
expected_date_of_drawing=next=Attribute_iterator('ExpectedDateOfDrawing',next,[str(date.today()+timedelta(30)),str(date.today()+timedelta(1)),str(date.today()+timedelta(0)),str(date.today()+timedelta(-1))])
expected_date_of_drawing.register_filter(lambda doc:doc["__"]["mainCase"] or doc["__ExpectedDateOfDrawing"]["count"]==0)
flag_main=next=Flag("mainCase",next,lambda doc:doc["__"]["mainCase"] and doc["__ExpectedDateOfDrawing"]["count"]==0)

#K jednotlivým případům, tak jak přijdou, postupně přiřadíme čerpání za 61,67,79,84,95,101,134 měsíců
loan_duration=next=Attribute_loop("LoanDuration",next,[[61,67,79,84,95,101,134]])

#Taktéž nastavíme fixaci na postupně přicházející případy na 96,36,12,108,48,60 měsíců
fixed_rate_period=next=Attribute_loop("FixedRatePeriod",next,[[96,36,12,108,48,60]])
#S výhradou situace, kdy by splatnost byla kratší než fixace. V takovém případě zarovnáme fixaci na nejvyšší celý rok, který se vejde do splatnosti
fixed_rate_period.register_eval(lambda doc,val: min(val,(doc['LoanDuration']//12)*12))


#Prokombinovat vhodným způsobem loanPurposes je trochu složitější úloha a tak si pomůžeme složitější evaluační metodou
loan_purposes=next=Attribute_iterator("LoanPurposes",next,["AmVse","Splaceni","Koupe","Vystavba","Rekonstrukce","ZhodnoceniPozemku","Vyporadani","PrevodDruztvo","KoupeSpolecnosti","Neucel","2a","2b","2c"])
loan_purposes.register_eval(loan_purposes_eval)
#Metoda loan_purposes_eval nastavuje 0 jako hodnotu atributu doc["LoanPurposes"] pokud je kombinace nevalidní (0 byla zvolena neboť null (resp None) a [] mohou být v JSON validní hodnoty)
loan_purposes.register_filter(lambda doc:doc["LoanPurposes"] != 0)
#Hlavní případ bude nadále ten se dvěma účely Koupě a Rekonstrukce
flag_main=next=Flag("mainCase",next,lambda doc:doc["__"]["mainCase"] and len(doc["LoanPurposes"])==2 and doc["LoanPurposes"][0]["LoanPurposeId"]==202 and doc["LoanPurposes"][1]["LoanPurposeId"]==204)

#Na hlavním případu vyzkoušíme všechny kombinace marketingových akcí
marketing_actions=next=Attribute_struct("MarketingActions",next)
ma_domicile=next=Attribute_iterator("MarketingActions.Domicile",next,[False,True])
ma_health_risk_insurance=next=Attribute_iterator("MarketingActions.HealthRiskInsurance",next,[False,True])
ma_real_estate_insurance=next=Attribute_iterator("MarketingActions.RealEstateInsurance",next,[False,True])
ma_income_loan_ratio_discount=next=Attribute_iterator("MarketingActions.IncomeLoanRatioDiscount",next,[False,True])
ma_user_vip=next=Attribute_iterator("MarketingActions.UserVip",next,[False,True])
#V ostatních případech zkoušíme pouze Domicilace, RŽP od KP a Pojištění nemovitosti od KP
ma_user_vip.register_filter(lambda doc: doc["__"]["mainCase"] or ((doc["MarketingActions"]["Domicile"]) and (doc["MarketingActions"]["HealthRiskInsurance"]) and (doc["MarketingActions"]["RealEstateInsurance"]) and (not doc["MarketingActions"]["IncomeLoanRatioDiscount"]) and (not doc["MarketingActions"]["UserVip"]) ))
#Hlavní případ bude nadále jen ten s Domicilací, RŽP od KP a Pojištěním nemovitosti od KP
flag_main=next=Flag("mainCase",next,lambda doc: doc["__"]["mainCase"] and ((doc["MarketingActions"]["Domicile"]) and (doc["MarketingActions"]["HealthRiskInsurance"]) and (doc["MarketingActions"]["RealEstateInsurance"]) and (not doc["MarketingActions"]["IncomeLoanRatioDiscount"]) and (not doc["MarketingActions"]["UserVip"]) ))

#Vybereme jeden případ americké hypotéky, pro který chceme otestovat simulaci s garancí 20 a 45 dní zpětně
flag_main=next=Flag("ameriCase",next,lambda doc:(doc["ProductTypeId"]==20010 and len(doc["LoanPurposes"])==2 and doc["LoanPurposes"][0]["LoanPurposeId"]==201 and doc["LoanPurposes"][1]["LoanPurposeId"]==210))
guarantee_date_from=next=Attribute_iterator('GuaranteeDateFrom',next,[str(date.today()),str(date.today()-timedelta(20)),str(date.today()-timedelta(45))])
#Simulaci s garancí 20 a 45 dní zpětně otestujeme pro hlavní případ a vybraný případ amarické hypotéky
guarantee_date_from.register_filter(lambda doc:doc["__"]["mainCase"] or doc["__"]["ameriCase"] or doc["__GuaranteeDateFrom"]["count"]==0)

#Smaže metadata a flagy, odstraní null položky polí
cleaner=next=Cleaner(next)
#Registrujeme jednoduchý Writer, který výsledky zapíše na konzoli (lze implementovat další typy writerů)
writer=next=Writer(next)
writer2=next=Writer_sqlite('OFFER',next)
#Spustíme generování
pipe.process()

