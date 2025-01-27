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
	/// Information about catalogue specification of business product in offer.catalogue = no situation/context available, i.e. generic definition of product.  CDM entity name(s): CatalogueMktItemInOfferSpecsInfo.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class CatalogueMktItemInOfferSpecsInfo : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"CatalogueMktItemInOfferSpecsInfo\",\"doc\":\"Information abo" +
				"ut catalogue specification of business product in offer.catalogue = no situation" +
				"/context available, i.e. generic definition of product.  CDM entity name(s): Cat" +
				"alogueMktItemInOfferSpecsInfo.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v" +
				"3.mortgageapplication\",\"fields\":[{\"name\":\"catalogueItemObjectCode\",\"type\":{\"type" +
				"\":\"record\",\"name\":\"CatalogueItemObjectCode\",\"doc\":\"Multiattribute property of an" +
				"y catalog item identification (product, service, offer...). The identifier (obje" +
				"ct code) does not change when the version changes Multi-attribute is used becaus" +
				"e there is not only one identification of the bank\'s product items (products, se" +
				"rvices), but there can be several.#CZ#Multiatributova vlastnost jakekoli identif" +
				"ikace katalogove polozky (produktu, sluzby, nabidky...). Identifikator (object c" +
				"ode) se nemeni pri zmene verze Vice atributu je pouzito z duvodu ze neexistuje p" +
				"ouze jedina identifikace produktovych polozek banky (produktu, sluzeb), ale muze" +
				" jich byt vice. CDM entity name(s): CatalogueItemObjectCode.\",\"namespace\":\"cz.kb" +
				".api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"objectCo" +
				"de\",\"doc\":\"The identifier of the catalog item (product, service, offering, etc.)" +
				" that does not change when the version of the item changes.For example, the Stan" +
				"dard Tariff Offer has different settings in different versions (e.g. different t" +
				"ypes and quantities of products included), these versions have different IDs but" +
				" the same OC as long as it is still the same offer from the bank\'s point of view" +
				".#CZ#Identifikator katalogve polozky (produkt, sluzba, nabidka apod.), ktery se " +
				"nemeni pri zmene verze polozky.Napr. Nabidka tarifu Standard ma v ruznych verzic" +
				"h ruzna nastaveni (napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyto ver" +
				"ze maji odlisne ID, ale stejny OC, dokud se jedna z pohledu banky o stale stejno" +
				"u nabidku. CDM attribute class name: CatalogueItemObjectCodeObjectCode. Attribut" +
				"e has simple type ST_CodeDefault with description: Standard data type to be used" +
				" on *Code* attributes.\'Code\' is very similar to \'ID\' (i.e. unique identifier) bu" +
				"t it is supposed to be human-readable.E.g. SystemApplicationCode, ProductGroupCo" +
				"de.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}" +
				"}]}}]}");
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.CatalogueItemObjectCode _catalogueItemObjectCode;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return CatalogueMktItemInOfferSpecsInfo._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.CatalogueItemObjectCode catalogueItemObjectCode
		{
			get
			{
				return this._catalogueItemObjectCode;
			}
			set
			{
				this._catalogueItemObjectCode = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.catalogueItemObjectCode;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.catalogueItemObjectCode = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.CatalogueItemObjectCode)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
