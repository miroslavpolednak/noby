// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v1.mortgageinstance
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Uniform Resource IdentifierNapr./typicky http odkaz. CDM entity name(s): URI.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class URI : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""URI"",""doc"":""Uniform Resource IdentifierNapr./typicky http odkaz. CDM entity name(s): URI."",""namespace"":""cz.kb.api.mortgageservicingevents.v1.mortgageinstance"",""fields"":[{""name"":""value"",""doc"":""Complete URI. CDM attribute class name: URIValue. Attribute has simple type ST_URI with description: A string representing a URI. Limited to 2047 characters."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,2048}$""}}]}");
		/// <summary>
		/// Complete URI. CDM attribute class name: URIValue. Attribute has simple type ST_URI with description: A string representing a URI. Limited to 2047 characters.
		/// </summary>
		private string _value;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return URI._SCHEMA;
			}
		}
		/// <summary>
		/// Complete URI. CDM attribute class name: URIValue. Attribute has simple type ST_URI with description: A string representing a URI. Limited to 2047 characters.
		/// </summary>
		public string value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.value;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.value = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}