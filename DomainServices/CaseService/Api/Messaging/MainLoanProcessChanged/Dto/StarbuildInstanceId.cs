// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Multiattribute property of marketable item instance.Multiattribute: Identifiers of marketable item instances are not yet unified, there is no one central catalogue.This also covers identifiers valid for only some part of marketable item instance lifecycle, e.g. proposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenna instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejych polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez identifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanci prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator, TSS identifikator. CDM entity name(s): InstanceId.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class StarbuildInstanceId : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""StarbuildInstanceId"",""doc"":""Multiattribute property of marketable item instance.Multiattribute: Identifiers of marketable item instances are not yet unified, there is no one central catalogue.This also covers identifiers valid for only some part of marketable item instance lifecycle, e.g. proposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenna instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejych polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez identifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanci prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator, TSS identifikator. CDM entity name(s): InstanceId."",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged"",""fields"":[{""name"":""id"",""doc"":""Identifier of product instance item itself.#CZ#Vlastni identifikator instance prodejne polozky. CDM attribute class name: InstanceIdId."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,30}$""}}]}");
		/// <summary>
		/// Identifier of product instance item itself.#CZ#Vlastni identifikator instance prodejne polozky. CDM attribute class name: InstanceIdId.
		/// </summary>
		private string _id;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return StarbuildInstanceId._SCHEMA;
			}
		}
		/// <summary>
		/// Identifier of product instance item itself.#CZ#Vlastni identifikator instance prodejne polozky. CDM attribute class name: InstanceIdId.
		/// </summary>
		public string id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.id;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.id = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
