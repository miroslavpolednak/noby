// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v3.mortgageinstance
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
				"3.mortgageinstance\",\"fields\":[{\"name\":\"catalogueItemObjectCode\",\"type\":{\"type\":\"" +
				"record\",\"name\":\"CatalogueItemObjectCode\",\"doc\":\"Multiattribute property of any c" +
				"atalog item identification (product, service, offer...). The identifier (object " +
				"code) does not change when the version changes Multi-attribute is used because t" +
				"here is not only one identification of the bank\'s product items (products, servi" +
				"ces), but there can be several.#CZ#Multiatributova vlastnost jakekoli identifika" +
				"ce katalogove polozky (produktu, sluzby, nabidky...). Identifikator (object code" +
				") se nemeni pri zmene verze Vice atributu je pouzito z duvodu ze neexistuje pouz" +
				"e jedina identifikace produktovych polozek banky (produktu, sluzeb), ale muze ji" +
				"ch byt vice. CDM entity name(s): CatalogueItemObjectCode.\",\"namespace\":\"cz.kb.ap" +
				"i.mortgageservicingevents.v3.mortgageinstance\",\"fields\":[{\"name\":\"objectCode\",\"d" +
				"oc\":\"The identifier of the catalog item (product, service, offering, etc.) that " +
				"does not change when the version of the item changes.For example, the Standard T" +
				"ariff Offer has different settings in different versions (e.g. different types a" +
				"nd quantities of products included), these versions have different IDs but the s" +
				"ame OC as long as it is still the same offer from the bank\'s point of view.#CZ#I" +
				"dentifikator katalogve polozky (produkt, sluzba, nabidka apod.), ktery se nemeni" +
				" pri zmene verze polozky.Napr. Nabidka tarifu Standard ma v ruznych verzich ruzn" +
				"a nastaveni (napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyto verze maj" +
				"i odlisne ID, ale stejny OC, dokud se jedna z pohledu banky o stale stejnou nabi" +
				"dku. CDM attribute class name: CatalogueItemObjectCodeObjectCode. Attribute has " +
				"simple type ST_CodeDefault with description: Standard data type to be used on *C" +
				"ode* attributes.\'Code\' is very similar to \'ID\' (i.e. unique identifier) but it i" +
				"s supposed to be human-readable.E.g. SystemApplicationCode, ProductGroupCode.\",\"" +
				"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}" +
				"");
		private cz.kb.api.mortgageservicingevents.v3.mortgageinstance.CatalogueItemObjectCode _catalogueItemObjectCode;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return CatalogueMktItemInOfferSpecsInfo._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageinstance.CatalogueItemObjectCode catalogueItemObjectCode
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
			case 0: this.catalogueItemObjectCode = (cz.kb.api.mortgageservicingevents.v3.mortgageinstance.CatalogueItemObjectCode)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
