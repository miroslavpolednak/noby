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
	/// Mortgage product instance. CDM entity name(s): MortgageInstance.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class MortgageInstance : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""MortgageInstance"",""doc"":""Mortgage product instance. CDM entity name(s): MortgageInstance."",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged"",""fields"":[{""name"":""Starbuild"",""type"":{""type"":""record"",""name"":""StarbuildInstanceId"",""doc"":""Multiattribute property of marketable item instance.Multiattribute: Identifiers of marketable item instances are not yet unified, there is no one central catalogue.This also covers identifiers valid for only some part of marketable item instance lifecycle, e.g. proposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenna instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prodejych polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez identifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanci prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikator, TSS identifikator. CDM entity name(s): InstanceId."",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged"",""fields"":[{""name"":""id"",""doc"":""Identifier of product instance item itself.#CZ#Vlastni identifikator instance prodejne polozky. CDM attribute class name: InstanceIdId."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,30}$""}}]}}]}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged.StarbuildInstanceId _Starbuild;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return MortgageInstance._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged.StarbuildInstanceId Starbuild
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
			case 0: return this.Starbuild;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.Starbuild = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.mainloanprocesschanged.StarbuildInstanceId)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
