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
	/// Multiattribute property of any catalog item identification (product, service, offer...). The identifier (object code) does not change when the version changes Multi-attribute is used because there is not only one identification of the bank's product items (products, services), but there can be several.#CZ#Multiatributova vlastnost jakekoli identifikace katalogove polozky (produktu, sluzby, nabidky...). Identifikator (object code) se nemeni pri zmene verze Vice atributu je pouzito z duvodu ze neexistuje pouze jedina identifikace produktovych polozek banky (produktu, sluzeb), ale muze jich byt vice. CDM entity name(s): CatalogueItemObjectCode.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class CatalogueItemObjectCode : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"CatalogueItemObjectCode\",\"doc\":\"Multiattribute property " +
				"of any catalog item identification (product, service, offer...). The identifier " +
				"(object code) does not change when the version changes Multi-attribute is used b" +
				"ecause there is not only one identification of the bank\'s product items (product" +
				"s, services), but there can be several.#CZ#Multiatributova vlastnost jakekoli id" +
				"entifikace katalogove polozky (produktu, sluzby, nabidky...). Identifikator (obj" +
				"ect code) se nemeni pri zmene verze Vice atributu je pouzito z duvodu ze neexist" +
				"uje pouze jedina identifikace produktovych polozek banky (produktu, sluzeb), ale" +
				" muze jich byt vice. CDM entity name(s): CatalogueItemObjectCode.\",\"namespace\":\"" +
				"cz.kb.api.mortgageservicingevents.v3.mortgageapplication\",\"fields\":[{\"name\":\"obj" +
				"ectCode\",\"doc\":\"The identifier of the catalog item (product, service, offering, " +
				"etc.) that does not change when the version of the item changes.For example, the" +
				" Standard Tariff Offer has different settings in different versions (e.g. differ" +
				"ent types and quantities of products included), these versions have different ID" +
				"s but the same OC as long as it is still the same offer from the bank\'s point of" +
				" view.#CZ#Identifikator katalogve polozky (produkt, sluzba, nabidka apod.), kter" +
				"y se nemeni pri zmene verze polozky.Napr. Nabidka tarifu Standard ma v ruznych v" +
				"erzich ruzna nastaveni (napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyt" +
				"o verze maji odlisne ID, ale stejny OC, dokud se jedna z pohledu banky o stale s" +
				"tejnou nabidku. CDM attribute class name: CatalogueItemObjectCodeObjectCode. Att" +
				"ribute has simple type ST_CodeDefault with description: Standard data type to be" +
				" used on *Code* attributes.\'Code\' is very similar to \'ID\' (i.e. unique identifie" +
				"r) but it is supposed to be human-readable.E.g. SystemApplicationCode, ProductGr" +
				"oupCode.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,10" +
				"0}$\"}}]}");
		/// <summary>
		/// The identifier of the catalog item (product, service, offering, etc.) that does not change when the version of the item changes.For example, the Standard Tariff Offer has different settings in different versions (e.g. different types and quantities of products included), these versions have different IDs but the same OC as long as it is still the same offer from the bank's point of view.#CZ#Identifikator katalogve polozky (produkt, sluzba, nabidka apod.), ktery se nemeni pri zmene verze polozky.Napr. Nabidka tarifu Standard ma v ruznych verzich ruzna nastaveni (napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyto verze maji odlisne ID, ale stejny OC, dokud se jedna z pohledu banky o stale stejnou nabidku. CDM attribute class name: CatalogueItemObjectCodeObjectCode. Attribute has simple type ST_CodeDefault with description: Standard data type to be used on *Code* attributes.'Code' is very similar to 'ID' (i.e. unique identifier) but it is supposed to be human-readable.E.g. SystemApplicationCode, ProductGroupCode.
		/// </summary>
		private string _objectCode;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return CatalogueItemObjectCode._SCHEMA;
			}
		}
		/// <summary>
		/// The identifier of the catalog item (product, service, offering, etc.) that does not change when the version of the item changes.For example, the Standard Tariff Offer has different settings in different versions (e.g. different types and quantities of products included), these versions have different IDs but the same OC as long as it is still the same offer from the bank's point of view.#CZ#Identifikator katalogve polozky (produkt, sluzba, nabidka apod.), ktery se nemeni pri zmene verze polozky.Napr. Nabidka tarifu Standard ma v ruznych verzich ruzna nastaveni (napr. ruzne typy a mnozstvi produktu v ni zahrnute), tyto verze maji odlisne ID, ale stejny OC, dokud se jedna z pohledu banky o stale stejnou nabidku. CDM attribute class name: CatalogueItemObjectCodeObjectCode. Attribute has simple type ST_CodeDefault with description: Standard data type to be used on *Code* attributes.'Code' is very similar to 'ID' (i.e. unique identifier) but it is supposed to be human-readable.E.g. SystemApplicationCode, ProductGroupCode.
		/// </summary>
		public string objectCode
		{
			get
			{
				return this._objectCode;
			}
			set
			{
				this._objectCode = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.objectCode;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.objectCode = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
