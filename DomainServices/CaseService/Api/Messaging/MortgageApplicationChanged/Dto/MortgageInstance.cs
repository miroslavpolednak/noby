// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v2.mortgageapplication
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
				"s.v2.mortgageapplication\",\"fields\":[{\"name\":\"instanceState\",\"default\":null,\"type" +
				"\":[\"null\",{\"type\":\"record\",\"name\":\"InstanceState\",\"doc\":\"State of any marketable" +
				" item instance (agreement, product instance, service instance,...).#CZ#Stav inst" +
				"ance jakekoliv prodejne polozky (obchod, instance produktu, instance sluzby...)." +
				" CDM entity name(s): InstanceState.\",\"namespace\":\"cz.kb.api.mortgageservicingeve" +
				"nts.v2.mortgageapplication\",\"fields\":[{\"name\":\"instanceStateCode\",\"type\":{\"type\"" +
				":\"record\",\"name\":\"InstanceStateCode\",\"doc\":\"Multicodebook property of marketable" +
				" item instance state (agreement, product instance, service instance,...) identif" +
				"ier.It is multicodebook because state is generic property and individual product" +
				"/agreements/... have individually defined state machines.#CZ#Vicehodnotova cisel" +
				"nikova promenna identifikatoru stavu predejnych polozek (obchod, instance produk" +
				"tu, instance sluzby...). Jedna se o vicehodnotou ciselnikovou promenou, protoze " +
				"stav je obecna promenna. Ta ma individualne definovane stavy v zavislosti na tom" +
				", zda jde o obchod, produkt... . CDM entity name(s): InstanceStateCode.\",\"namesp" +
				"ace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name" +
				"\":\"state\",\"doc\":\"Codebook value of state itself.#CZ#Vlastni ciselnikova hodnota " +
				"stavu. Attribute has specific codebook type: CB_MortgageLifeCyclePhase. CDM attr" +
				"ibute class name: InstanceStateCodeState. Attribute has simple type CB_MultiCode" +
				"bookValue with description: Represents information that value can be from more t" +
				"han one codebook. Codebook type: CB_MortgageLifeCyclePhase.\",\"type\":{\"type\":\"str" +
				"ing\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}]},{\"name\":\"LifeCy" +
				"cleInstanceState\",\"default\":null,\"type\":[\"null\",\"InstanceState\"]},{\"name\":\"loanA" +
				"mount\",\"type\":{\"type\":\"record\",\"name\":\"LoanAmount\",\"doc\":\"Vyska uveruInformation" +
				" about loan amount. This is static information. Change only with sale or mainten" +
				"ance action. CDM entity name(s): LoanAmount.\",\"namespace\":\"cz.kb.api.mortgageser" +
				"vicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"limit\",\"doc\":\"Vyska uver" +
				"u, ktora bola schvalena. (Pozor, nie je nutne suma, ktoru si klient skutocne poz" +
				"iacia)Allowed loan limit. It is maximum amount that customer can borrow from ban" +
				"k.E.g. credit card limit, overdraft limit, or contracted consumer loan amount. C" +
				"DM attribute class name: LoanAmountLimit. Attribute has simple type ST_AmountMon" +
				"ey with description: Castka.Decimal number representing an amount of money.\",\"ty" +
				"pe\":{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":17,\"scale\":2}}]}},{\"name" +
				"\":\"loanInstanceInfo\",\"type\":{\"type\":\"record\",\"name\":\"LoanInstanceInfo\",\"doc\":\"In" +
				"formation entity representing any loan product (instance of product). CDM entity" +
				" name(s): LoanInstanceInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.m" +
				"ortgageapplication\",\"fields\":[{\"name\":\"loanInstallments\",\"type\":{\"type\":\"record\"" +
				",\"name\":\"LoanInstallments\",\"doc\":\"One installment amount. It is information enti" +
				"ty. It can be used for any installment. CDM entity name(s): LoanInstallments.\",\"" +
				"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"fields\":[" +
				"{\"name\":\"dayInMonth\",\"doc\":\"Day within month when installment is due to be paid." +
				" CDM attribute class name: LoanInstallmentsDayInMonth. Codebook type: CB_DayOfMo" +
				"nth.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"firstAnnuit" +
				"yLoanIndividualInstallment\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name" +
				"\":\"FirstAnnuityLoanIndividualInstallment\",\"doc\":\"Individual/concrete repayment o" +
				"f loan - one loan installment. CDM entity name(s): LoanIndividualInstallment.\",\"" +
				"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"fields\":[" +
				"{\"name\":\"on\",\"doc\":\"Date of installment to be repaid/was repaid. CDM attribute c" +
				"lass name: LoanIndividualInstallmentOn. Standard avro date represented by the nu" +
				"mber of days from the unix epoch, see avro specification: https://avro.apache.or" +
				"g/docs/1.8.0/spec.html. Consider using converters to date class in code generato" +
				"r (see KB Integration Platform wiki for schema registry chapter).\",\"type\":{\"type" +
				"\":\"int\",\"logicalType\":\"date\"}}]}]},{\"name\":\"loanIndividualInstallment\",\"type\":{\"" +
				"type\":\"record\",\"name\":\"LoanIndividualInstallment\",\"doc\":\"Individual/concrete rep" +
				"ayment of loan - one loan installment. CDM entity name(s): LoanIndividualInstall" +
				"ment.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"f" +
				"ields\":[{\"name\":\"amount\",\"doc\":\"Total amount that has to paid as one loan instal" +
				"lment. CDM attribute class name: LoanIndividualInstallmentAmount. Attribute has " +
				"simple type ST_AmountMoney with description: Castka.Decimal number representing " +
				"an amount of money.\",\"type\":{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":" +
				"17,\"scale\":2}}]}}]}}]}},{\"name\":\"loanInterestRate\",\"type\":{\"type\":\"record\",\"name" +
				"\":\"LoanInterestRate\",\"doc\":\"Specific interest rate of proposed or sold loan busi" +
				"ness product. This is only THE (primary) interest rate. This is NOT any other in" +
				"terest rate on product. CDM entity name(s): LoanInterestRate.\",\"namespace\":\"cz.k" +
				"b.api.mortgageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"fixedRa" +
				"tePeriod\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"FixedRatePeriod" +
				"\",\"doc\":\"Time that interest rate is fixed and can not change. CDM entity name(s)" +
				": FixedRatePeriod.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageap" +
				"plication\",\"fields\":[{\"name\":\"period\",\"doc\":\"Total length of period in units. If" +
				" no units are given, default is month. CDM attribute class name: FixedRatePeriod" +
				"Period. Attribute has simple type ST_PositiveInt with description: Integer with " +
				"a constraint. The value must be a positive number, i.e. zero is excluded.\",\"type" +
				"\":\"int\"}]}]},{\"name\":\"value\",\"doc\":\"Resulting interest rate, i.e. what customer " +
				"sees.Urokova mira uveru.(High level) vbs + srn + margin. CDM attribute class nam" +
				"e: LoanInterestRateValue. Attribute has simple type ST_InterestRate with descrip" +
				"tion: Urok.Represents an interest rate or its part, (Interest rate, SRN, Margin)" +
				".E. g. 1.0 (=100%), 0.25 (=25%)Limited at 100000%\",\"type\":{\"type\":\"bytes\",\"logic" +
				"alType\":\"decimal\",\"precision\":11,\"scale\":8}}]}},{\"name\":\"mktItemInstanceInfo\",\"t" +
				"ype\":{\"type\":\"record\",\"name\":\"MktItemInstanceInfo\",\"doc\":\"Information entity. En" +
				"tity that is used to provide information about any instance of marketable item (" +
				"product, service, frame agreement, bundle,...). CDM entity name(s): MktItemInsta" +
				"nceInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\"" +
				",\"fields\":[{\"name\":\"catalogueMktItemInOfferSpecsInfo\",\"type\":{\"type\":\"record\",\"n" +
				"ame\":\"CatalogueMktItemInOfferSpecsInfo\",\"doc\":\"Information about catalogue speci" +
				"fication of business product in offer.catalogue = no situation/context available" +
				", i.e. generic definition of product.  CDM entity name(s): CatalogueMktItemInOff" +
				"erSpecsInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplicat" +
				"ion\",\"fields\":[{\"name\":\"catalogueItemObjectCode\",\"type\":{\"type\":\"record\",\"name\":" +
				"\"CatalogueItemObjectCode\",\"doc\":\"Multiattribute property of any catalog item ide" +
				"ntification (product, service, offer...). The identifier (object code) does not " +
				"change when the version changes Multi-attribute is used because there is not onl" +
				"y one identification of the bank\'s product items (products, services), but there" +
				" can be several.#CZ#Multiatributova vlastnost jakekoli identifikace katalogove p" +
				"olozky (produktu, sluzby, nabidky...). Identifikator (object code) se nemeni pri" +
				" zmene verze Vice atributu je pouzito z duvodu ze neexistuje pouze jedina identi" +
				"fikace produktovych polozek banky (produktu, sluzeb), ale muze jich byt vice. CD" +
				"M entity name(s): CatalogueItemObjectCode.\",\"namespace\":\"cz.kb.api.mortgageservi" +
				"cingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"objectCode\",\"doc\":\"The ide" +
				"ntifier of the catalog item (product, service, offering, etc.) that does not cha" +
				"nge when the version of the item changes.For example, the Standard Tariff Offer " +
				"has different settings in different versions (e.g. different types and quantitie" +
				"s of products included), these versions have different IDs but the same OC as lo" +
				"ng as it is still the same offer from the bank\'s point of view.#CZ#Identifikator" +
				" katalogve polozky (produkt, sluzba, nabidka apod.), ktery se nemeni pri zmene v" +
				"erze polozky.Napr. Nabidka tarifu Standard ma v ruznych verzich ruzna nastaveni " +
				"(napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyto verze maji odlisne ID" +
				", ale stejny OC, dokud se jedna z pohledu banky o stale stejnou nabidku. CDM att" +
				"ribute class name: CatalogueItemObjectCodeObjectCode. Attribute has simple type " +
				"ST_CodeDefault with description: Standard data type to be used on *Code* attribu" +
				"tes.\'Code\' is very similar to \'ID\' (i.e. unique identifier) but it is supposed t" +
				"o be human-readable.E.g. SystemApplicationCode, ProductGroupCode.\",\"type\":{\"type" +
				"\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}},{\"name\":\"p" +
				"artyInMktItemInstanceInfoes\",\"type\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"na" +
				"me\":\"PartyInMktItemInstanceInfo\",\"doc\":\"Information entity. Describes party occu" +
				"rrence in context of a marketing item instance (product, service, frame agreemen" +
				"t, bundle,...). CDM entity name(s): PartyInMktItemInstanceInfo.\",\"namespace\":\"cz" +
				".kb.api.mortgageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"custo" +
				"mer\",\"type\":{\"type\":\"record\",\"name\":\"Customer\",\"doc\":\" CDM entity name(s): Custo" +
				"mer.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"fi" +
				"elds\":[{\"name\":\"customerId\",\"doc\":\" CDM attribute class name: CustomerCustomerId" +
				".\",\"type\":\"long\"}]}},{\"name\":\"partyInMktItemInstanceRole\",\"type\":{\"type\":\"record" +
				"\",\"name\":\"PartyInMktItemInstanceRole\",\"doc\":\"The role that a party has in a part" +
				"icular product instance.It can be specific to each product, common across a grou" +
				"p of products, or even across all products.E.g. applicant, co-applicant, guarant" +
				"or for consumer loans#CZ#Role, kterou ma strana v konkretni instanci produktu.Mu" +
				"ze byt specificka pro kazdy produkt, spolecna v ramci skupiny produktu nebo doko" +
				"nce pro vsechny produkty.Napr. zadatel, spoluzadatel, rucitel pro spotrebni uver" +
				"y. CDM entity name(s): PartyInMktItemInstanceRole.\",\"namespace\":\"cz.kb.api.mortg" +
				"ageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"partyInMktItemInst" +
				"anceRoleCode\",\"type\":{\"type\":\"record\",\"name\":\"PartyInMktItemInstanceRoleCode\",\"d" +
				"oc\":\"Multicodebook property representing code of role (unique identifier) that p" +
				"arties in general can have on products.It is multicodebook because roles always " +
				"exists, but differs per product groups and are not managed centrally. CDM entity" +
				" name(s): PartyInMktItemInstanceRoleCode.\",\"namespace\":\"cz.kb.api.mortgageservic" +
				"ingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"code\",\"doc\":\"Human readable" +
				" unique identifier of role on product instance. MultiCodebook value itself.Uniqu" +
				"e across all products. Attribute has specific codebook type: CB_CustomerInMortga" +
				"geInstanceRole. CDM attribute class name: PartyInMktItemInstanceRoleCodeCode. At" +
				"tribute has simple type CB_MultiCodebookValue with description: Represents infor" +
				"mation that value can be from more than one codebook. Codebook type: CB_Customer" +
				"InMortgageInstanceRole.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pa" +
				"ttern\":\"^.{0,100}$\"}}]}}]}}]}}}]}},{\"name\":\"mortgageLoanKind\",\"doc\":\" CDM attrib" +
				"ute class name: MortgageLoanKind. Codebook type: CB_MortgageLoanKind.\",\"type\":{\"" +
				"type\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"PCP\",\"default\":null,\"type\"" +
				":[\"null\",{\"type\":\"record\",\"name\":\"PCPInstanceId\",\"doc\":\"Multiattribute property " +
				"of marketable item instance.Multiattribute: Identifiers of marketable item insta" +
				"nces are not yet unified, there is no one central catalogue.This also covers ide" +
				"ntifiers valid for only some part of marketable item instance lifecycle, e.g. pr" +
				"oposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenna" +
				" instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejyc" +
				"h polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez id" +
				"entifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanci" +
				" prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator, " +
				"TSS identifikator. CDM entity name(s): InstanceId.\",\"namespace\":\"cz.kb.api.mortg" +
				"ageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"id\",\"doc\":\"Identif" +
				"ier of product instance item itself.#CZ#Vlastni identifikator instance prodejne " +
				"polozky. CDM attribute class name: InstanceIdId. Attribute has simple type ST_Id" +
				"StringDefault with description: Standard data type to be used ID, i.e. unique id" +
				"entifier. It is not supposed to be human-readable.E.g.: AgreementID\",\"type\":{\"ty" +
				"pe\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}]},{\"name\":\"Pr" +
				"oductDetailURIInNoby\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"URI" +
				"\",\"doc\":\"Uniform Resource IdentifierNapr./typicky http odkaz. CDM entity name(s)" +
				": URI.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"" +
				"fields\":[{\"name\":\"value\",\"doc\":\"Complete URI. CDM attribute class name: URIValue" +
				". Attribute has simple type ST_URI with description: A string representing a URI" +
				". Limited to 2047 characters.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"Strin" +
				"g\",\"pattern\":\"^.{0,2048}$\"}}]}]},{\"name\":\"Starbuild\",\"default\":null,\"type\":[\"nul" +
				"l\",{\"type\":\"record\",\"name\":\"StarbuildInstanceId\",\"doc\":\"Multiattribute property " +
				"of marketable item instance.Multiattribute: Identifiers of marketable item insta" +
				"nces are not yet unified, there is no one central catalogue.This also covers ide" +
				"ntifiers valid for only some part of marketable item instance lifecycle, e.g. pr" +
				"oposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenna" +
				" instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejyc" +
				"h polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez id" +
				"entifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanci" +
				" prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator, " +
				"TSS identifikator. CDM entity name(s): InstanceId.\",\"namespace\":\"cz.kb.api.mortg" +
				"ageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"id\",\"doc\":\"Identif" +
				"ier of product instance item itself.#CZ#Vlastni identifikator instance prodejne " +
				"polozky. CDM attribute class name: InstanceIdId. Attribute has simple type ST_Id" +
				"StringDefault with description: Standard data type to be used ID, i.e. unique id" +
				"entifier. It is not supposed to be human-readable.E.g.: AgreementID\",\"type\":{\"ty" +
				"pe\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}]}]}");
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.InstanceState _instanceState;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.InstanceState _LifeCycleInstanceState;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanAmount _loanAmount;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanInstanceInfo _loanInstanceInfo;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanInterestRate _loanInterestRate;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.MktItemInstanceInfo _mktItemInstanceInfo;
		/// <summary>
		///  CDM attribute class name: MortgageLoanKind. Codebook type: CB_MortgageLoanKind.
		/// </summary>
		private string _mortgageLoanKind;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.PCPInstanceId _PCP;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.URI _ProductDetailURIInNoby;
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.StarbuildInstanceId _Starbuild;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return MortgageInstance._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.InstanceState instanceState
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.InstanceState LifeCycleInstanceState
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanAmount loanAmount
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanInstanceInfo loanInstanceInfo
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanInterestRate loanInterestRate
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.MktItemInstanceInfo mktItemInstanceInfo
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.PCPInstanceId PCP
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.URI ProductDetailURIInNoby
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
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.StarbuildInstanceId Starbuild
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
			case 2: return this.loanAmount;
			case 3: return this.loanInstanceInfo;
			case 4: return this.loanInterestRate;
			case 5: return this.mktItemInstanceInfo;
			case 6: return this.mortgageLoanKind;
			case 7: return this.PCP;
			case 8: return this.ProductDetailURIInNoby;
			case 9: return this.Starbuild;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.instanceState = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.InstanceState)fieldValue; break;
			case 1: this.LifeCycleInstanceState = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.InstanceState)fieldValue; break;
			case 2: this.loanAmount = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanAmount)fieldValue; break;
			case 3: this.loanInstanceInfo = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanInstanceInfo)fieldValue; break;
			case 4: this.loanInterestRate = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.LoanInterestRate)fieldValue; break;
			case 5: this.mktItemInstanceInfo = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.MktItemInstanceInfo)fieldValue; break;
			case 6: this.mortgageLoanKind = (System.String)fieldValue; break;
			case 7: this.PCP = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.PCPInstanceId)fieldValue; break;
			case 8: this.ProductDetailURIInNoby = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.URI)fieldValue; break;
			case 9: this.Starbuild = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.StarbuildInstanceId)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}