// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v1.mortgageinstance
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Information entity. Entity that is used to provide information about any instance of marketable item (product, service, frame agreement, bundle,...). CDM entity name(s): MktItemInstanceInfo.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class MktItemInstanceInfo : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"MktItemInstanceInfo\",\"doc\":\"Information entity. Entity t" +
				"hat is used to provide information about any instance of marketable item (produc" +
				"t, service, frame agreement, bundle,...). CDM entity name(s): MktItemInstanceInf" +
				"o.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v1.mortgageinstance\",\"fields\"" +
				":[{\"name\":\"catalogueMktItemInOfferSpecsInfo\",\"type\":{\"type\":\"record\",\"name\":\"Cat" +
				"alogueMktItemInOfferSpecsInfo\",\"doc\":\"Information about catalogue specification " +
				"of business product in offer.catalogue = no situation/context available, i.e. ge" +
				"neric definition of product.  CDM entity name(s): CatalogueMktItemInOfferSpecsIn" +
				"fo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v1.mortgageinstance\",\"fields" +
				"\":[{\"name\":\"catalogueItemObjectCode\",\"type\":{\"type\":\"record\",\"name\":\"CatalogueIt" +
				"emObjectCode\",\"doc\":\"Multiattribute property of any catalog item identification " +
				"(product, service, offer...). The identifier (object code) does not change when " +
				"the version changes Multi-attribute is used because there is not only one identi" +
				"fication of the bank\'s product items (products, services), but there can be seve" +
				"ral.#CZ#Multiatributova vlastnost jakekoli identifikace katalogove polozky (prod" +
				"uktu, sluzby, nabidky...). Identifikator (object code) se nemeni pri zmene verze" +
				" Vice atributu je pouzito z duvodu ze neexistuje pouze jedina identifikace produ" +
				"ktovych polozek banky (produktu, sluzeb), ale muze jich byt vice. CDM entity nam" +
				"e(s): CatalogueItemObjectCode.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v" +
				"1.mortgageinstance\",\"fields\":[{\"name\":\"objectCode\",\"doc\":\"The identifier of the " +
				"catalog item (product, service, offering, etc.) that does not change when the ve" +
				"rsion of the item changes.For example, the Standard Tariff Offer has different s" +
				"ettings in different versions (e.g. different types and quantities of products i" +
				"ncluded), these versions have different IDs but the same OC as long as it is sti" +
				"ll the same offer from the bank\'s point of view.#CZ#Identifikator katalogve polo" +
				"zky (produkt, sluzba, nabidka apod.), ktery se nemeni pri zmene verze polozky.Na" +
				"pr. Nabidka tarifu Standard ma v ruznych verzich ruzna nastaveni (napr. ruzne ty" +
				"py a mnozstvi produktu v ni zahrnute), tyto verze maji odlisne ID, ale stejny OC" +
				", dokud se jedna z pohledu banky o stale stejnou nabidku. CDM attribute class na" +
				"me: CatalogueItemObjectCodeObjectCode. Attribute has simple type ST_CodeDefault " +
				"with description: Standard data type to be used on *Code* attributes.\'Code\' is v" +
				"ery similar to \'ID\' (i.e. unique identifier) but it is supposed to be human-read" +
				"able.E.g. SystemApplicationCode, ProductGroupCode.\",\"type\":{\"type\":\"string\",\"avr" +
				"o.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}},{\"name\":\"partyInMktItemIn" +
				"stanceInfoes\",\"type\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"PartyInMkt" +
				"ItemInstanceInfo\",\"doc\":\"Information entity. Describes party occurrence in conte" +
				"xt of a marketing item instance (product, service, frame agreement, bundle,...)." +
				" CDM entity name(s): PartyInMktItemInstanceInfo.\",\"namespace\":\"cz.kb.api.mortgag" +
				"eservicingevents.v1.mortgageinstance\",\"fields\":[{\"name\":\"customer\",\"type\":{\"type" +
				"\":\"record\",\"name\":\"Customer\",\"doc\":\" CDM entity name(s): Customer.\",\"namespace\":" +
				"\"cz.kb.api.mortgageservicingevents.v1.mortgageinstance\",\"fields\":[{\"name\":\"custo" +
				"merId\",\"doc\":\" CDM attribute class name: CustomerCustomerId.\",\"type\":\"long\"}]}}," +
				"{\"name\":\"partyInMktItemInstanceRole\",\"type\":{\"type\":\"record\",\"name\":\"PartyInMktI" +
				"temInstanceRole\",\"doc\":\"The role that a party has in a particular product instan" +
				"ce.It can be specific to each product, common across a group of products, or eve" +
				"n across all products.E.g. applicant, co-applicant, guarantor for consumer loans" +
				"#CZ#Role, kterou ma strana v konkretni instanci produktu.Muze byt specificka pro" +
				" kazdy produkt, spolecna v ramci skupiny produktu nebo dokonce pro vsechny produ" +
				"kty.Napr. zadatel, spoluzadatel, rucitel pro spotrebni uvery. CDM entity name(s)" +
				": PartyInMktItemInstanceRole.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v1" +
				".mortgageinstance\",\"fields\":[{\"name\":\"partyInMktItemInstanceRoleCode\",\"type\":{\"t" +
				"ype\":\"record\",\"name\":\"PartyInMktItemInstanceRoleCode\",\"doc\":\"Multicodebook prope" +
				"rty representing code of role (unique identifier) that parties in general can ha" +
				"ve on products.It is multicodebook because roles always exists, but differs per " +
				"product groups and are not managed centrally. CDM entity name(s): PartyInMktItem" +
				"InstanceRoleCode.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v1.mortgageins" +
				"tance\",\"fields\":[{\"name\":\"code\",\"doc\":\"Human readable unique identifier of role " +
				"on product instance. MultiCodebook value itself.Unique across all products. Attr" +
				"ibute has specific codebook type: CB_CustomerInMortgageInstanceRole. CDM attribu" +
				"te class name: PartyInMktItemInstanceRoleCodeCode. Attribute has simple type CB_" +
				"MultiCodebookValue with description: Represents information that value can be fr" +
				"om more than one codebook. Codebook type: CB_CustomerInMortgageInstanceRole.\",\"t" +
				"ype\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}}" +
				"]}}},{\"name\":\"termination\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\"" +
				":\"Termination\",\"doc\":\" CDM entity name(s): Termination.\",\"namespace\":\"cz.kb.api." +
				"mortgageservicingevents.v1.mortgageinstance\",\"fields\":[{\"name\":\"deactivatedExact" +
				"lyOn\",\"doc\":\"Date when product instance item was terminated or will be terminate" +
				"d (any way, e.g. canceled, loan repaid, account closed). CDM attribute class nam" +
				"e: DeactivatedExactlyOn. Standard avro dateTime represented by the number of mil" +
				"liseconds from the unix epoch, see avro specification: https://avro.apache.org/d" +
				"ocs/1.8.0/spec.html. Consider using converters to dateTime class in code generat" +
				"or (see KB Integration Platform wiki for schema registry chapter).\",\"type\":{\"typ" +
				"e\":\"long\",\"logicalType\":\"timestamp-millis\"}}]}]}]}");
		private cz.kb.api.mortgageservicingevents.v1.mortgageinstance.CatalogueMktItemInOfferSpecsInfo _catalogueMktItemInOfferSpecsInfo;
		private IList<cz.kb.api.mortgageservicingevents.v1.mortgageinstance.PartyInMktItemInstanceInfo> _partyInMktItemInstanceInfoes;
		private cz.kb.api.mortgageservicingevents.v1.mortgageinstance.Termination _termination;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return MktItemInstanceInfo._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v1.mortgageinstance.CatalogueMktItemInOfferSpecsInfo catalogueMktItemInOfferSpecsInfo
		{
			get
			{
				return this._catalogueMktItemInOfferSpecsInfo;
			}
			set
			{
				this._catalogueMktItemInOfferSpecsInfo = value;
			}
		}
		public IList<cz.kb.api.mortgageservicingevents.v1.mortgageinstance.PartyInMktItemInstanceInfo> partyInMktItemInstanceInfoes
		{
			get
			{
				return this._partyInMktItemInstanceInfoes;
			}
			set
			{
				this._partyInMktItemInstanceInfoes = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v1.mortgageinstance.Termination termination
		{
			get
			{
				return this._termination;
			}
			set
			{
				this._termination = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.catalogueMktItemInOfferSpecsInfo;
			case 1: return this.partyInMktItemInstanceInfoes;
			case 2: return this.termination;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.catalogueMktItemInOfferSpecsInfo = (cz.kb.api.mortgageservicingevents.v1.mortgageinstance.CatalogueMktItemInOfferSpecsInfo)fieldValue; break;
			case 1: this.partyInMktItemInstanceInfoes = (IList<cz.kb.api.mortgageservicingevents.v1.mortgageinstance.PartyInMktItemInstanceInfo>)fieldValue; break;
			case 2: this.termination = (cz.kb.api.mortgageservicingevents.v1.mortgageinstance.Termination)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}