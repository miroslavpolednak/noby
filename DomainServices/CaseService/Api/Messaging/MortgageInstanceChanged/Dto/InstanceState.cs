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
	/// State of any marketable item instance (agreement, product instance, service instance,...).#CZ#Stav instance jakekoliv prodejne polozky (obchod, instance produktu, instance sluzby...). CDM entity name(s): InstanceState.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class InstanceState : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"InstanceState\",\"doc\":\"State of any marketable item insta" +
				"nce (agreement, product instance, service instance,...).#CZ#Stav instance jakeko" +
				"liv prodejne polozky (obchod, instance produktu, instance sluzby...). CDM entity" +
				" name(s): InstanceState.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mort" +
				"gageinstance\",\"fields\":[{\"name\":\"instanceStateCode\",\"type\":{\"type\":\"record\",\"nam" +
				"e\":\"InstanceStateCode\",\"doc\":\"Multicodebook property of marketable item instance" +
				" state (agreement, product instance, service instance,...) identifier.It is mult" +
				"icodebook because state is generic property and individual product/agreements/.." +
				". have individually defined state machines.#CZ#Vicehodnotova ciselnikova promenn" +
				"a identifikatoru stavu predejnych polozek (obchod, instance produktu, instance s" +
				"luzby...). Jedna se o vicehodnotou ciselnikovou promenou, protoze stav je obecna" +
				" promenna. Ta ma individualne definovane stavy v zavislosti na tom, zda jde o ob" +
				"chod, produkt... . CDM entity name(s): InstanceStateCode.\",\"namespace\":\"cz.kb.ap" +
				"i.mortgageservicingevents.v3.mortgageinstance\",\"fields\":[{\"name\":\"state\",\"doc\":\"" +
				"Codebook value of state itself.#CZ#Vlastni ciselnikova hodnota stavu. Attribute " +
				"has specific codebook type: CB_MortgageLifeCyclePhase. CDM attribute class name:" +
				" InstanceStateCodeState. Attribute has simple type CB_MultiCodebookValue with de" +
				"scription: Represents information that value can be from more than one codebook." +
				" Codebook type: CB_MortgageLifeCyclePhase.\",\"type\":{\"type\":\"string\",\"avro.java.s" +
				"tring\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}}]}");
		private cz.kb.api.mortgageservicingevents.v3.mortgageinstance.InstanceStateCode _instanceStateCode;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return InstanceState._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageinstance.InstanceStateCode instanceStateCode
		{
			get
			{
				return this._instanceStateCode;
			}
			set
			{
				this._instanceStateCode = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.instanceStateCode;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.instanceStateCode = (cz.kb.api.mortgageservicingevents.v3.mortgageinstance.InstanceStateCode)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
