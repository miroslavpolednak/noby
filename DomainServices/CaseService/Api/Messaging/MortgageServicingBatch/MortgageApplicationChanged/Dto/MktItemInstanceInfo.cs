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
	/// Information entity. Entity that is used to provide information about any instance of marketable item (product, service, frame agreement, bundle,...). CDM entity name(s): MktItemInstanceInfo.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class MktItemInstanceInfo : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"MktItemInstanceInfo\",\"doc\":\"Information entity. Entity t" +
				"hat is used to provide information about any instance of marketable item (produc" +
				"t, service, frame agreement, bundle,...). CDM entity name(s): MktItemInstanceInf" +
				"o.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fiel" +
				"ds\":[{\"name\":\"catalogueMktItemInOfferSpecsInfo\",\"type\":{\"type\":\"record\",\"name\":\"" +
				"CatalogueMktItemInOfferSpecsInfo\",\"doc\":\"Information about catalogue specificati" +
				"on of business product in offer.catalogue = no situation/context available, i.e." +
				" generic definition of product.  CDM entity name(s): CatalogueMktItemInOfferSpec" +
				"sInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"" +
				"fields\":[{\"name\":\"catalogueItemObjectCode\",\"type\":{\"type\":\"record\",\"name\":\"Catal" +
				"ogueItemObjectCode\",\"doc\":\"Multiattribute property of any catalog item identific" +
				"ation (product, service, offer...). The identifier (object code) does not change" +
				" when the version changes Multi-attribute is used because there is not only one " +
				"identification of the bank\'s product items (products, services), but there can b" +
				"e several.#CZ#Multiatributova vlastnost jakekoli identifikace katalogove polozky" +
				" (produktu, sluzby, nabidky...). Identifikator (object code) se nemeni pri zmene" +
				" verze Vice atributu je pouzito z duvodu ze neexistuje pouze jedina identifikace" +
				" produktovych polozek banky (produktu, sluzeb), ale muze jich byt vice. CDM enti" +
				"ty name(s): CatalogueItemObjectCode.\",\"namespace\":\"cz.kb.api.mortgageservicingev" +
				"ents.v3.mortgageapplication\",\"fields\":[{\"name\":\"objectCode\",\"doc\":\"The identifie" +
				"r of the catalog item (product, service, offering, etc.) that does not change wh" +
				"en the version of the item changes.For example, the Standard Tariff Offer has di" +
				"fferent settings in different versions (e.g. different types and quantities of p" +
				"roducts included), these versions have different IDs but the same OC as long as " +
				"it is still the same offer from the bank\'s point of view.#CZ#Identifikator katal" +
				"ogve polozky (produkt, sluzba, nabidka apod.), ktery se nemeni pri zmene verze p" +
				"olozky.Napr. Nabidka tarifu Standard ma v ruznych verzich ruzna nastaveni (napr." +
				" ruzne typy a mnozstvi produktu v ni zahrnute), tyto verze maji odlisne ID, ale " +
				"stejny OC, dokud se jedna z pohledu banky o stale stejnou nabidku. CDM attribute" +
				" class name: CatalogueItemObjectCodeObjectCode. Attribute has simple type ST_Cod" +
				"eDefault with description: Standard data type to be used on *Code* attributes.\'C" +
				"ode\' is very similar to \'ID\' (i.e. unique identifier) but it is supposed to be h" +
				"uman-readable.E.g. SystemApplicationCode, ProductGroupCode.\",\"type\":{\"type\":\"str" +
				"ing\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}},{\"name\":\"partyIn" +
				"MktItemInstanceInfoes\",\"type\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"P" +
				"artyInMktItemInstanceInfo\",\"doc\":\"Information entity. Describes party occurrence" +
				" in context of a marketing item instance (product, service, frame agreement, bun" +
				"dle,...). CDM entity name(s): PartyInMktItemInstanceInfo.\",\"namespace\":\"cz.kb.ap" +
				"i.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"customer\",\"" +
				"type\":{\"type\":\"record\",\"name\":\"Customer\",\"doc\":\" CDM entity name(s): Customer.\"," +
				"\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":" +
				"[{\"name\":\"customerId\",\"doc\":\" CDM attribute class name: CustomerCustomerId.\",\"ty" +
				"pe\":\"long\"}]}},{\"name\":\"partyInMktItemInstanceRole\",\"type\":{\"type\":\"record\",\"nam" +
				"e\":\"PartyInMktItemInstanceRole\",\"doc\":\"The role that a party has in a particular" +
				" product instance.It can be specific to each product, common across a group of p" +
				"roducts, or even across all products.E.g. applicant, co-applicant, guarantor for" +
				" consumer loans#CZ#Role, kterou ma strana v konkretni instanci produktu.Muze byt" +
				" specificka pro kazdy produkt, spolecna v ramci skupiny produktu nebo dokonce pr" +
				"o vsechny produkty.Napr. zadatel, spoluzadatel, rucitel pro spotrebni uvery. CDM" +
				" entity name(s): PartyInMktItemInstanceRole.\",\"namespace\":\"cz.kb.api.mortgageser" +
				"vicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"partyInMktItemInstanceRo" +
				"leCode\",\"type\":{\"type\":\"record\",\"name\":\"PartyInMktItemInstanceRoleCode\",\"doc\":\"M" +
				"ulticodebook property representing code of role (unique identifier) that parties" +
				" in general can have on products.It is multicodebook because roles always exists" +
				", but differs per product groups and are not managed centrally. CDM entity name(" +
				"s): PartyInMktItemInstanceRoleCode.\",\"namespace\":\"cz.kb.api.mortgageservicingeve" +
				"nts.v3.mortgageapplication\",\"fields\":[{\"name\":\"code\",\"doc\":\"Human readable uniqu" +
				"e identifier of role on product instance. MultiCodebook value itself.Unique acro" +
				"ss all products. Attribute has specific codebook type: CB_CustomerInMortgageInst" +
				"anceRole. CDM attribute class name: PartyInMktItemInstanceRoleCodeCode. Attribut" +
				"e has simple type CB_MultiCodebookValue with description: Represents information" +
				" that value can be from more than one codebook. Codebook type: CB_CustomerInMort" +
				"gageInstanceRole.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\"" +
				":\"^.{0,100}$\"}}]}}]}}]}}}]}");
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.CatalogueMktItemInOfferSpecsInfo _catalogueMktItemInOfferSpecsInfo;
		private IList<cz.kb.api.mortgageservicingevents.v3.mortgageapplication.PartyInMktItemInstanceInfo> _partyInMktItemInstanceInfoes;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return MktItemInstanceInfo._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.CatalogueMktItemInOfferSpecsInfo catalogueMktItemInOfferSpecsInfo
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
		public IList<cz.kb.api.mortgageservicingevents.v3.mortgageapplication.PartyInMktItemInstanceInfo> partyInMktItemInstanceInfoes
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
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.catalogueMktItemInOfferSpecsInfo;
			case 1: return this.partyInMktItemInstanceInfoes;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.catalogueMktItemInOfferSpecsInfo = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.CatalogueMktItemInOfferSpecsInfo)fieldValue; break;
			case 1: this.partyInMktItemInstanceInfoes = (IList<cz.kb.api.mortgageservicingevents.v3.mortgageapplication.PartyInMktItemInstanceInfo>)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
