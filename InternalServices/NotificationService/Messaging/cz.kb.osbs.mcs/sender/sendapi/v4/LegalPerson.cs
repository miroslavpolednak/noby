// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.sender.sendapi.v4
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Juridical person that is NOT <b>fixed</b> to <b>exactly one </b>natural person. CDM entity name(s): LegalPerson.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class LegalPerson : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""LegalPerson"",""doc"":""Juridical person that is NOT <b>fixed</b> to <b>exactly one </b>natural person. CDM entity name(s): LegalPerson."",""namespace"":""cz.kb.osbs.mcs.sender.sendapi.v4"",""fields"":[{""name"":""name"",""doc"":""Official name of juridical person. CDM attribute class name: JuridicalPersonName."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,255}$""}}]}");
		/// <summary>
		/// Official name of juridical person. CDM attribute class name: JuridicalPersonName.
		/// </summary>
		private string _name;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return LegalPerson._SCHEMA;
			}
		}
		/// <summary>
		/// Official name of juridical person. CDM attribute class name: JuridicalPersonName.
		/// </summary>
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.name;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.name = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}