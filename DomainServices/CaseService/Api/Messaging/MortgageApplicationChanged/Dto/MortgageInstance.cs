// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v3.mortgageapplication
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Mortgage product instance. CDM entity name(s): MortgageInstance.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class MortgageInstance : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"MortgageInstance\",\"doc\":\"Mortgage product instance. CDM " +
				"entity name(s): MortgageInstance.\",\"namespace\":\"cz.kb.api.mortgageservicingevent" +
				"s.v3.mortgageapplication\",\"fields\":[{\"name\":\"instanceState\",\"default\":null,\"type" +
				"\":[\"null\",{\"type\":\"record\",\"name\":\"InstanceState\",\"doc\":\"State of any marketable" +
				" item instance (agreement, product instance, service instance,...).#CZ#Stav inst" +
				"ance jakekoliv prodejne polozky (obchod, instance produktu, instance sluzby...)." +
				" CDM entity name(s): InstanceState.\",\"namespace\":\"cz.kb.api.mortgageservicingeve" +
				"nts.v3.mortgageapplication\",\"fields\":[{\"name\":\"instanceStateCode\",\"type\":{\"type\"" +
				":\"record\",\"name\":\"InstanceStateCode\",\"doc\":\"Multicodebook property of marketable" +
				" item instance state (agreement, product instance, service instance,...) identif" +
				"ier.It is multicodebook because state is generic property and individual product" +
				"/agreements/... have individually defined state machines.#CZ#Vicehodnotova cisel" +
				"nikova promenna identifikatoru stavu predejnych polozek (obchod, instance produk" +
				"tu, instance sluzby...). Jedna se o vicehodnotou ciselnikovou promenou, protoze " +
				"stav je obecna promenna. Ta ma individualne definovane stavy v zavislosti na tom" +
				", zda jde o obchod, produkt... . CDM entity name(s): InstanceStateCode.\",\"namesp" +
				"ace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name" +
				"\":\"state\",\"doc\":\"Codebook value of state itself.#CZ#Vlastni ciselnikova hodnota " +
				"stavu. Attribute has specific codebook type: CB_MortgageLifeCyclePhase. CDM attr" +
				"ibute class name: InstanceStateCodeState. Attribute has simple type CB_MultiCode" +
				"bookValue with description: Represents information that value can be from more t" +
				"han one codebook. Codebook type: CB_MortgageLifeCyclePhase.\",\"type\":{\"type\":\"str" +
				"ing\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}]},{\"name\":\"LifeCy" +
				"cleInstanceState\",\"default\":null,\"type\":[\"null\",\"InstanceState\"]},{\"name\":\"LoanA" +
				"ccount\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"Account\",\"doc\":\"T" +
				"his entity represents information about account as is used in payment landscape." +
				" For example has properties current balance, account number, iban, currency, ava" +
				"ilable balance,.. . If this entity represents multicurrency (MCY) master account" +
				" (multicurrency=true):: the account is called master (parent) account; it acts a" +
				"s envelope for supported currencies subaccounts set; balances and limits attribu" +
				"tes represent aggregated values from the currencies subaccounts (converted to ba" +
				"se currency); currency represents base (nominated) currency of master account.; " +
				"payment domain systems work only with master account information or identificati" +
				"on (single master account number plus currency); transactions, holds and interes" +
				"ts occure only on specific currency subaccounts.;In KB finance called \'aplikacni" +
				" ucet\'.This is not product sales entity. Do not confuse with CurrentAccountProdu" +
				"ct. Also do not represents general ledger accounts.. ## A representation of a pr" +
				"oduct instance as implemented in bank\'s core systems, systems delivering product" +
				"\'s functionality to customer. CDM entity name(s): Account,TechnicalItemInstance." +
				"\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields" +
				"\":[{\"name\":\"accountNumber\",\"type\":{\"type\":\"record\",\"name\":\"AccountNumber\",\"doc\":" +
				"\"AccountNumber: .AccountNumberPrefix; .AccountNumberCore; .AccountNumberBankCode" +
				"; .AccountNumberIban;. ## IBAN (International Bank Account Number) representatio" +
				"n of the account number by ISO 13616 - for detail description see attribute note" +
				"s. ## Account identification number defined by CNB for domestic paymentshttps://" +
				"www.cnb.cz/export/sites/cnb/cs/legislativa/.galleries/vyhlasky/vyhlaska_169_2011" +
				".pdf. CDM entity name(s): AccountNumberIBAN,AccountNumberCNB.\",\"namespace\":\"cz.k" +
				"b.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"bankCod" +
				"e\",\"doc\":\"Bank code: 4 digits; contains values from \'Directory of payment system" +
				" codes\' by CNB (https://www.cnb.cz/en/payments/accounts-bank-codes/); e.g.: for " +
				"\'1234-567890/0100\' the bank code is \'0100\';. CDM attribute class name: AccountNu" +
				"mberCNBBankCode. Attribute has simple type ST_BankCode with description: Kod ban" +
				"ky.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,4}$\"}}," +
				"{\"name\":\"core\",\"doc\":\"Core/base of account number (i.e. the number without the b" +
				"ank code and without prefix): 2 to 10 digits; mandatory; e.g.: for \'1234-567890/" +
				"0100\' the core is \'567890\';. CDM attribute class name: AccountNumberCNBCore. Att" +
				"ribute has simple type ST_AccountNumberCore with description: Cislo uctu bez pre" +
				"fixu.An account number excluding a prefix.\",\"type\":{\"type\":\"string\",\"avro.java.s" +
				"tring\":\"String\",\"pattern\":\"0*[1-9][0-9]*[1-9][0-9]*\"}},{\"name\":\"iban\",\"doc\":\"IBA" +
				"N representation of the account number: up to 32 characters - XX## #### #### ###" +
				"# #### #### #### ####; first 2 characters - country code; next 2 characters are " +
				"check digits; rest is country dependent - for CZ&amp;SK - 4 digits bank code, 6 " +
				"digits prefix, 10 digits core/base account number; without blanks for electronic" +
				" communication; grouped by 4 characters for user interfaces;. CDM attribute clas" +
				"s name: AccountNumberIBANIban. Attribute has simple type ST_IBAN with descriptio" +
				"n: The International Bank Account Number (IBAN).\",\"type\":{\"type\":\"string\",\"avro." +
				"java.string\":\"String\",\"pattern\":\"[A-Z][A-Z][0-9][0-9][0-9A-Z]*\"}},{\"name\":\"prefi" +
				"x\",\"doc\":\"Account number prefix: up to 6 digits; optional - could be empty or co" +
				"ntain zeroes \'000000\'; e.g.: for \'1234-567890/0100\' the prefix is \'1234\';. CDM a" +
				"ttribute class name: AccountNumberCNBPrefix. Attribute has simple type ST_Accoun" +
				"tNumberPrefix with description: Cislo uctu prefix.An account number prefix. \",\"d" +
				"efault\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\":\"String\",\"patter" +
				"n\":\"[0-9]*\"}]}]}},{\"name\":\"AccountState\",\"default\":null,\"type\":[\"null\",{\"type\":\"" +
				"record\",\"name\":\"TechnicalItemInstanceState\",\"doc\":\"Multicodebook property of tec" +
				"hnical product instance state. As the name suggests, it defines state of product" +
				" instance in its core system.It is multicodebook as state of technical product i" +
				"nstance is generic concept and will differ across technical product instance cor" +
				"e systems. CDM entity name(s): TechnicalItemInstanceState.\",\"namespace\":\"cz.kb.a" +
				"pi.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"class\",\"do" +
				"c\":\"Attribute that defines codebook that is used in attribute state. CDM attribu" +
				"te class name: TechnicalItemInstanceStateClass. Codebook type: CB_TechnicalProdu" +
				"ctInstanceStateClass.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}},{\"n" +
				"ame\":\"state\",\"doc\":\"Multicodebook value of state itself. CDM attribute class nam" +
				"e: TechnicalItemInstanceStateState. Attribute has simple type CB_MultiCodebookVa" +
				"lue with description: Represents information that value can be from more than on" +
				"e codebook. Codebook type: CB_MultiCodebookValue.\",\"type\":{\"type\":\"string\",\"avro" +
				".java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}]}]}]},{\"name\":\"loanAmount\",\"ty" +
				"pe\":{\"type\":\"record\",\"name\":\"LoanAmount\",\"doc\":\"Vyska uveruInformation about loa" +
				"n amount. This is static information. Change only with sale or maintenance actio" +
				"n. CDM entity name(s): LoanAmount.\",\"namespace\":\"cz.kb.api.mortgageservicingeven" +
				"ts.v3.mortgageapplication\",\"fields\":[{\"name\":\"limit\",\"doc\":\"Vyska uveru, ktora b" +
				"ola schvalena. (Pozor, nie je nutne suma, ktoru si klient skutocne poziacia)Allo" +
				"wed loan limit. It is maximum amount that customer can borrow from bank.E.g. cre" +
				"dit card limit, overdraft limit, or contracted consumer loan amount. CDM attribu" +
				"te class name: LoanAmountLimit. Attribute has simple type ST_AmountMoney with de" +
				"scription: Castka.Decimal number representing an amount of money.\",\"type\":{\"type" +
				"\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":17,\"scale\":2}}]}},{\"name\":\"loanIns" +
				"tanceInfo\",\"type\":{\"type\":\"record\",\"name\":\"LoanInstanceInfo\",\"doc\":\"Information " +
				"entity representing any loan product (instance of product). CDM entity name(s): " +
				"LoanInstanceInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapp" +
				"lication\",\"fields\":[{\"name\":\"ExpectedDrawdownPeriod\",\"default\":null,\"type\":[\"nul" +
				"l\",{\"type\":\"record\",\"name\":\"DrawdownPeriod\",\"doc\":\"Defines when loan can be with" +
				"drawn. CDM entity name(s): DrawdownPeriod.\",\"namespace\":\"cz.kb.api.mortgageservi" +
				"cingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"dateFrom\",\"doc\":\"Defines e" +
				"xact date when withdrawal of the loan can be started. CDM attribute class name: " +
				"DrawdownPeriodDateFrom. Standard avro date represented by the number of days fro" +
				"m the unix epoch, see avro specification: https://avro.apache.org/docs/1.8.0/spe" +
				"c.html. Consider using converters to date class in code generator (see KB Integr" +
				"ation Platform wiki for schema registry chapter).\",\"type\":{\"type\":\"int\",\"logical" +
				"Type\":\"date\"}},{\"name\":\"dateUntil\",\"doc\":\"Defines exact date when all purposes o" +
				"n the loan must be completely withdrawn. CDM attribute class name: DrawdownPerio" +
				"dDateUntil. Standard avro date represented by the number of days from the unix e" +
				"poch, see avro specification: https://avro.apache.org/docs/1.8.0/spec.html. Cons" +
				"ider using converters to date class in code generator (see KB Integration Platfo" +
				"rm wiki for schema registry chapter).\",\"default\":null,\"type\":[\"null\",{\"type\":\"in" +
				"t\",\"logicalType\":\"date\"}]}]}]},{\"name\":\"loanInstallments\",\"type\":{\"type\":\"record" +
				"\",\"name\":\"LoanInstallments\",\"doc\":\"One installment amount. It is information ent" +
				"ity. It can be used for any installment. CDM entity name(s): LoanInstallments.\"," +
				"\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":" +
				"[{\"name\":\"dayInMonth\",\"doc\":\"Day within month when installment is due to be paid" +
				". CDM attribute class name: LoanInstallmentsDayInMonth. Codebook type: CB_DayOfM" +
				"onth.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"firstAnnui" +
				"tyLoanIndividualInstallment\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"nam" +
				"e\":\"FirstAnnuityLoanIndividualInstallment\",\"doc\":\"Individual/concrete repayment " +
				"of loan - one loan installment. CDM entity name(s): LoanIndividualInstallment.\"," +
				"\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":" +
				"[{\"name\":\"on\",\"doc\":\"Date of installment to be repaid/was repaid. CDM attribute " +
				"class name: LoanIndividualInstallmentOn. Standard avro date represented by the n" +
				"umber of days from the unix epoch, see avro specification: https://avro.apache.o" +
				"rg/docs/1.8.0/spec.html. Consider using converters to date class in code generat" +
				"or (see KB Integration Platform wiki for schema registry chapter).\",\"type\":{\"typ" +
				"e\":\"int\",\"logicalType\":\"date\"}}]}]},{\"name\":\"loanIndividualInstallment\",\"type\":{" +
				"\"type\":\"record\",\"name\":\"LoanIndividualInstallment\",\"doc\":\"Individual/concrete re" +
				"payment of loan - one loan installment. CDM entity name(s): LoanIndividualInstal" +
				"lment.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"" +
				"fields\":[{\"name\":\"amount\",\"doc\":\"Total amount that has to paid as one loan insta" +
				"llment. CDM attribute class name: LoanIndividualInstallmentAmount. Attribute has" +
				" simple type ST_AmountMoney with description: Castka.Decimal number representing" +
				" an amount of money.\",\"type\":{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"precision\"" +
				":17,\"scale\":2}}]}}]}}]}},{\"name\":\"loanInterestRate\",\"type\":{\"type\":\"record\",\"nam" +
				"e\":\"LoanInterestRate\",\"doc\":\"Specific interest rate of proposed or sold loan bus" +
				"iness product. This is only THE (primary) interest rate. This is NOT any other i" +
				"nterest rate on product. CDM entity name(s): LoanInterestRate.\",\"namespace\":\"cz." +
				"kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"fixedR" +
				"atePeriod\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"FixedRatePerio" +
				"d\",\"doc\":\"Time that interest rate is fixed and can not change. CDM entity name(s" +
				"): FixedRatePeriod.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgagea" +
				"pplication\",\"fields\":[{\"name\":\"period\",\"doc\":\"Total length of period in units. I" +
				"f no units are given, default is month. CDM attribute class name: FixedRatePerio" +
				"dPeriod. Attribute has simple type ST_PositiveInt with description: Integer with" +
				" a constraint. The value must be a positive number, i.e. zero is excluded.\",\"typ" +
				"e\":\"int\"}]}]},{\"name\":\"value\",\"doc\":\"Resulting interest rate, i.e. what customer" +
				" sees.Urokova mira uveru.(High level) vbs + srn + margin. CDM attribute class na" +
				"me: LoanInterestRateValue. Attribute has simple type ST_InterestRate with descri" +
				"ption: Urok.Represents an interest rate or its part, (Interest rate, SRN, Margin" +
				").E. g. 1.0 (=100%), 0.25 (=25%)Limited at 100000%\",\"type\":{\"type\":\"bytes\",\"logi" +
				"calType\":\"decimal\",\"precision\":11,\"scale\":8}}]}},{\"name\":\"mktItemInstanceInfo\",\"" +
				"type\":{\"type\":\"record\",\"name\":\"MktItemInstanceInfo\",\"doc\":\"Information entity. E" +
				"ntity that is used to provide information about any instance of marketable item " +
				"(product, service, frame agreement, bundle,...). CDM entity name(s): MktItemInst" +
				"anceInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication" +
				"\",\"fields\":[{\"name\":\"catalogueMktItemInOfferSpecsInfo\",\"type\":{\"type\":\"record\",\"" +
				"name\":\"CatalogueMktItemInOfferSpecsInfo\",\"doc\":\"Information about catalogue spec" +
				"ification of business product in offer.catalogue = no situation/context availabl" +
				"e, i.e. generic definition of product.  CDM entity name(s): CatalogueMktItemInOf" +
				"ferSpecsInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplica" +
				"tion\",\"fields\":[{\"name\":\"catalogueItemObjectCode\",\"type\":{\"type\":\"record\",\"name\"" +
				":\"CatalogueItemObjectCode\",\"doc\":\"Multiattribute property of any catalog item id" +
				"entification (product, service, offer...). The identifier (object code) does not" +
				" change when the version changes Multi-attribute is used because there is not on" +
				"ly one identification of the bank\'s product items (products, services), but ther" +
				"e can be several.#CZ#Multiatributova vlastnost jakekoli identifikace katalogove " +
				"polozky (produktu, sluzby, nabidky...). Identifikator (object code) se nemeni pr" +
				"i zmene verze Vice atributu je pouzito z duvodu ze neexistuje pouze jedina ident" +
				"ifikace produktovych polozek banky (produktu, sluzeb), ale muze jich byt vice. C" +
				"DM entity name(s): CatalogueItemObjectCode.\",\"namespace\":\"cz.kb.api.mortgageserv" +
				"icingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"objectCode\",\"doc\":\"The id" +
				"entifier of the catalog item (product, service, offering, etc.) that does not ch" +
				"ange when the version of the item changes.For example, the Standard Tariff Offer" +
				" has different settings in different versions (e.g. different types and quantiti" +
				"es of products included), these versions have different IDs but the same OC as l" +
				"ong as it is still the same offer from the bank\'s point of view.#CZ#Identifikato" +
				"r katalogve polozky (produkt, sluzba, nabidka apod.), ktery se nemeni pri zmene " +
				"verze polozky.Napr. Nabidka tarifu Standard ma v ruznych verzich ruzna nastaveni" +
				" (napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyto verze maji odlisne I" +
				"D, ale stejny OC, dokud se jedna z pohledu banky o stale stejnou nabidku. CDM at" +
				"tribute class name: CatalogueItemObjectCodeObjectCode. Attribute has simple type" +
				" ST_CodeDefault with description: Standard data type to be used on *Code* attrib" +
				"utes.\'Code\' is very similar to \'ID\' (i.e. unique identifier) but it is supposed " +
				"to be human-readable.E.g. SystemApplicationCode, ProductGroupCode.\",\"type\":{\"typ" +
				"e\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}},{\"name\":\"" +
				"partyInMktItemInstanceInfoes\",\"type\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"n" +
				"ame\":\"PartyInMktItemInstanceInfo\",\"doc\":\"Information entity. Describes party occ" +
				"urrence in context of a marketing item instance (product, service, frame agreeme" +
				"nt, bundle,...). CDM entity name(s): PartyInMktItemInstanceInfo.\",\"namespace\":\"c" +
				"z.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"cust" +
				"omer\",\"type\":{\"type\":\"record\",\"name\":\"Customer\",\"doc\":\" CDM entity name(s): Cust" +
				"omer.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"f" +
				"ields\":[{\"name\":\"customerId\",\"doc\":\" CDM attribute class name: CustomerCustomerI" +
				"d.\",\"type\":\"long\"}]}},{\"name\":\"partyInMktItemInstanceRole\",\"type\":{\"type\":\"recor" +
				"d\",\"name\":\"PartyInMktItemInstanceRole\",\"doc\":\"The role that a party has in a par" +
				"ticular product instance.It can be specific to each product, common across a gro" +
				"up of products, or even across all products.E.g. applicant, co-applicant, guaran" +
				"tor for consumer loans#CZ#Role, kterou ma strana v konkretni instanci produktu.M" +
				"uze byt specificka pro kazdy produkt, spolecna v ramci skupiny produktu nebo dok" +
				"once pro vsechny produkty.Napr. zadatel, spoluzadatel, rucitel pro spotrebni uve" +
				"ry. CDM entity name(s): PartyInMktItemInstanceRole.\",\"namespace\":\"cz.kb.api.mort" +
				"gageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"partyInMktItemIns" +
				"tanceRoleCode\",\"type\":{\"type\":\"record\",\"name\":\"PartyInMktItemInstanceRoleCode\",\"" +
				"doc\":\"Multicodebook property representing code of role (unique identifier) that " +
				"parties in general can have on products.It is multicodebook because roles always" +
				" exists, but differs per product groups and are not managed centrally. CDM entit" +
				"y name(s): PartyInMktItemInstanceRoleCode.\",\"namespace\":\"cz.kb.api.mortgageservi" +
				"cingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"code\",\"doc\":\"Human readabl" +
				"e unique identifier of role on product instance. MultiCodebook value itself.Uniq" +
				"ue across all products. Attribute has specific codebook type: CB_CustomerInMortg" +
				"ageInstanceRole. CDM attribute class name: PartyInMktItemInstanceRoleCodeCode. A" +
				"ttribute has simple type CB_MultiCodebookValue with description: Represents info" +
				"rmation that value can be from more than one codebook. Codebook type: CB_Custome" +
				"rInMortgageInstanceRole.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"p" +
				"attern\":\"^.{0,100}$\"}}]}}]}}]}}}]}},{\"name\":\"mortgageLoanKind\",\"doc\":\" CDM attri" +
				"bute class name: MortgageLoanKind. Codebook type: CB_MortgageLoanKind.\",\"type\":{" +
				"\"type\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"PCP\",\"default\":null,\"type" +
				"\":[\"null\",{\"type\":\"record\",\"name\":\"PCPInstanceId\",\"doc\":\"Multiattribute property" +
				" of marketable item instance.Multiattribute: Identifiers of marketable item inst" +
				"ances are not yet unified, there is no one central catalogue.This also covers id" +
				"entifiers valid for only some part of marketable item instance lifecycle, e.g. p" +
				"roposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenn" +
				"a instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejy" +
				"ch polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez i" +
				"dentifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanc" +
				"i prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator," +
				" TSS identifikator. CDM entity name(s): InstanceId.\",\"namespace\":\"cz.kb.api.mort" +
				"gageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"id\",\"doc\":\"Identi" +
				"fier of product instance item itself.#CZ#Vlastni identifikator instance prodejne" +
				" polozky. CDM attribute class name: InstanceIdId. Attribute has simple type ST_I" +
				"dStringDefault with description: Standard data type to be used ID, i.e. unique i" +
				"dentifier. It is not supposed to be human-readable.E.g.: AgreementID\",\"type\":{\"t" +
				"ype\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}]},{\"name\":\"P" +
				"roductDetailURIInNoby\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"UR" +
				"I\",\"doc\":\"Uniform Resource IdentifierNapr./typicky http odkaz. CDM entity name(s" +
				"): URI.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\"," +
				"\"fields\":[{\"name\":\"value\",\"doc\":\"Complete URI. CDM attribute class name: URIValu" +
				"e. Attribute has simple type ST_URI with description: A string representing a UR" +
				"I. Limited to 2047 characters.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"Stri" +
				"ng\",\"pattern\":\"^.{0,2048}$\"}}]}]},{\"name\":\"Starbuild\",\"default\":null,\"type\":[\"nu" +
				"ll\",{\"type\":\"record\",\"name\":\"StarbuildInstanceId\",\"doc\":\"Multiattribute property" +
				" of marketable item instance.Multiattribute: Identifiers of marketable item inst" +
				"ances are not yet unified, there is no one central catalogue.This also covers id" +
				"entifiers valid for only some part of marketable item instance lifecycle, e.g. p" +
				"roposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenn" +
				"a instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejy" +
				"ch polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez i" +
				"dentifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanc" +
				"i prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator," +
				" TSS identifikator. CDM entity name(s): InstanceId.\",\"namespace\":\"cz.kb.api.mort" +
				"gageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"id\",\"doc\":\"Identi" +
				"fier of product instance item itself.#CZ#Vlastni identifikator instance prodejne" +
				" polozky. CDM attribute class name: InstanceIdId. Attribute has simple type ST_I" +
				"dStringDefault with description: Standard data type to be used ID, i.e. unique i" +
				"dentifier. It is not supposed to be human-readable.E.g.: AgreementID\",\"type\":{\"t" +
				"ype\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}]}]}");
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.InstanceState _instanceState;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.InstanceState _LifeCycleInstanceState;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.Account _LoanAccount;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanAmount _loanAmount;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanInstanceInfo _loanInstanceInfo;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanInterestRate _loanInterestRate;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.MktItemInstanceInfo _mktItemInstanceInfo;
		/// <summary>
		///  CDM attribute class name: MortgageLoanKind. Codebook type: CB_MortgageLoanKind.
		/// </summary>
		private string _mortgageLoanKind;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.PCPInstanceId _PCP;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.URI _ProductDetailURIInNoby;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.StarbuildInstanceId _Starbuild;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return MortgageInstance._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.InstanceState instanceState
		{
			get
			{
				return this._instanceState;
			}
			set
			{
				this._instanceState = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.InstanceState LifeCycleInstanceState
		{
			get
			{
				return this._LifeCycleInstanceState;
			}
			set
			{
				this._LifeCycleInstanceState = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.Account LoanAccount
		{
			get
			{
				return this._LoanAccount;
			}
			set
			{
				this._LoanAccount = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanAmount loanAmount
		{
			get
			{
				return this._loanAmount;
			}
			set
			{
				this._loanAmount = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanInstanceInfo loanInstanceInfo
		{
			get
			{
				return this._loanInstanceInfo;
			}
			set
			{
				this._loanInstanceInfo = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanInterestRate loanInterestRate
		{
			get
			{
				return this._loanInterestRate;
			}
			set
			{
				this._loanInterestRate = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.MktItemInstanceInfo mktItemInstanceInfo
		{
			get
			{
				return this._mktItemInstanceInfo;
			}
			set
			{
				this._mktItemInstanceInfo = value;
			}
		}
		/// <summary>
		///  CDM attribute class name: MortgageLoanKind. Codebook type: CB_MortgageLoanKind.
		/// </summary>
		public string mortgageLoanKind
		{
			get
			{
				return this._mortgageLoanKind;
			}
			set
			{
				this._mortgageLoanKind = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.PCPInstanceId PCP
		{
			get
			{
				return this._PCP;
			}
			set
			{
				this._PCP = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.URI ProductDetailURIInNoby
		{
			get
			{
				return this._ProductDetailURIInNoby;
			}
			set
			{
				this._ProductDetailURIInNoby = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.StarbuildInstanceId Starbuild
		{
			get
			{
				return this._Starbuild;
			}
			set
			{
				this._Starbuild = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.instanceState;
			case 1: return this.LifeCycleInstanceState;
			case 2: return this.LoanAccount;
			case 3: return this.loanAmount;
			case 4: return this.loanInstanceInfo;
			case 5: return this.loanInterestRate;
			case 6: return this.mktItemInstanceInfo;
			case 7: return this.mortgageLoanKind;
			case 8: return this.PCP;
			case 9: return this.ProductDetailURIInNoby;
			case 10: return this.Starbuild;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.instanceState = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.InstanceState)fieldValue; break;
			case 1: this.LifeCycleInstanceState = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.InstanceState)fieldValue; break;
			case 2: this.LoanAccount = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.Account)fieldValue; break;
			case 3: this.loanAmount = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanAmount)fieldValue; break;
			case 4: this.loanInstanceInfo = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanInstanceInfo)fieldValue; break;
			case 5: this.loanInterestRate = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanInterestRate)fieldValue; break;
			case 6: this.mktItemInstanceInfo = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.MktItemInstanceInfo)fieldValue; break;
			case 7: this.mortgageLoanKind = (System.String)fieldValue; break;
			case 8: this.PCP = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.PCPInstanceId)fieldValue; break;
			case 9: this.ProductDetailURIInNoby = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.URI)fieldValue; break;
			case 10: this.Starbuild = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.StarbuildInstanceId)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
