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
	/// Multicodebook property of technical product instance state. As the name suggests, it defines state of product instance in its core system.It is multicodebook as state of technical product instance is generic concept and will differ across technical product instance core systems. CDM entity name(s): TechnicalItemInstanceState.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class TechnicalItemInstanceState : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""TechnicalItemInstanceState"",""doc"":""Multicodebook property of technical product instance state. As the name suggests, it defines state of product instance in its core system.It is multicodebook as state of technical product instance is generic concept and will differ across technical product instance core systems. CDM entity name(s): TechnicalItemInstanceState."",""namespace"":""cz.kb.api.mortgageservicingevents.v3.mortgageinstance"",""fields"":[{""name"":""class"",""doc"":""Attribute that defines codebook that is used in attribute state. CDM attribute class name: TechnicalItemInstanceStateClass. Codebook type: CB_TechnicalProductInstanceStateClass."",""type"":{""type"":""string"",""avro.java.string"":""String""}},{""name"":""state"",""doc"":""Multicodebook value of state itself. Attribute has specific codebook type: CB_Undefined. CDM attribute class name: TechnicalItemInstanceStateState. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_Undefined."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}}]}");
		/// <summary>
		/// Attribute that defines codebook that is used in attribute state. CDM attribute class name: TechnicalItemInstanceStateClass. Codebook type: CB_TechnicalProductInstanceStateClass.
		/// </summary>
		private string _class;
		/// <summary>
		/// Multicodebook value of state itself. Attribute has specific codebook type: CB_Undefined. CDM attribute class name: TechnicalItemInstanceStateState. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_Undefined.
		/// </summary>
		private string _state;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return TechnicalItemInstanceState._SCHEMA;
			}
		}
		/// <summary>
		/// Attribute that defines codebook that is used in attribute state. CDM attribute class name: TechnicalItemInstanceStateClass. Codebook type: CB_TechnicalProductInstanceStateClass.
		/// </summary>
		public string @class
		{
			get
			{
				return this._class;
			}
			set
			{
				this._class = value;
			}
		}
		/// <summary>
		/// Multicodebook value of state itself. Attribute has specific codebook type: CB_Undefined. CDM attribute class name: TechnicalItemInstanceStateState. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_Undefined.
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
			case 0: return this.@class;
			case 1: return this.state;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.@class = (System.String)fieldValue; break;
			case 1: this.state = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
