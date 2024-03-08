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
	/// Multicodebook property of marketable item instance state (agreement, product instance, service instance,...) identifier.It is multicodebook because state is generic property and individual product/agreements/... have individually defined state machines.#CZ#Vicehodnotova ciselnikova promenna identifikatoru stavu predejnych polozek (obchod, instance produktu, instance sluzby...). Jedna se o vicehodnotou ciselnikovou promenou, protoze stav je obecna promenna. Ta ma individualne definovane stavy v zavislosti na tom, zda jde o obchod, produkt... . CDM entity name(s): InstanceStateCode.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class InstanceStateCode : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""InstanceStateCode"",""doc"":""Multicodebook property of marketable item instance state (agreement, product instance, service instance,...) identifier.It is multicodebook because state is generic property and individual product/agreements/... have individually defined state machines.#CZ#Vicehodnotova ciselnikova promenna identifikatoru stavu predejnych polozek (obchod, instance produktu, instance sluzby...). Jedna se o vicehodnotou ciselnikovou promenou, protoze stav je obecna promenna. Ta ma individualne definovane stavy v zavislosti na tom, zda jde o obchod, produkt... . CDM entity name(s): InstanceStateCode."",""namespace"":""cz.kb.api.mortgageservicingevents.v3.mortgageapplication"",""fields"":[{""name"":""state"",""doc"":""Codebook value of state itself.#CZ#Vlastni ciselnikova hodnota stavu. Attribute has specific codebook type: CB_MortgageLifeCyclePhase. CDM attribute class name: InstanceStateCodeState. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_MortgageLifeCyclePhase."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}}]}");
		/// <summary>
		/// Codebook value of state itself.#CZ#Vlastni ciselnikova hodnota stavu. Attribute has specific codebook type: CB_MortgageLifeCyclePhase. CDM attribute class name: InstanceStateCodeState. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_MortgageLifeCyclePhase.
		/// </summary>
		private string _state;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return InstanceStateCode._SCHEMA;
			}
		}
		/// <summary>
		/// Codebook value of state itself.#CZ#Vlastni ciselnikova hodnota stavu. Attribute has specific codebook type: CB_MortgageLifeCyclePhase. CDM attribute class name: InstanceStateCodeState. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_MortgageLifeCyclePhase.
		/// </summary>
		public string state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.state;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.state = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
