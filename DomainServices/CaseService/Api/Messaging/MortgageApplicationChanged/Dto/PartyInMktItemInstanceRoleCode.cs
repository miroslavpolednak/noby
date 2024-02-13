// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v2.mortgageapplication
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Multicodebook property representing code of role (unique identifier) that parties in general can have on products.It is multicodebook because roles always exists, but differs per product groups and are not managed centrally. CDM entity name(s): PartyInMktItemInstanceRoleCode.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class PartyInMktItemInstanceRoleCode : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""PartyInMktItemInstanceRoleCode"",""doc"":""Multicodebook property representing code of role (unique identifier) that parties in general can have on products.It is multicodebook because roles always exists, but differs per product groups and are not managed centrally. CDM entity name(s): PartyInMktItemInstanceRoleCode."",""namespace"":""cz.kb.api.mortgageservicingevents.v2.mortgageapplication"",""fields"":[{""name"":""code"",""doc"":""Human readable unique identifier of role on product instance. MultiCodebook value itself.Unique across all products. Attribute has specific codebook type: CB_CustomerInMortgageInstanceRole. CDM attribute class name: PartyInMktItemInstanceRoleCodeCode. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_CustomerInMortgageInstanceRole."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}}]}");
		/// <summary>
		/// Human readable unique identifier of role on product instance. MultiCodebook value itself.Unique across all products. Attribute has specific codebook type: CB_CustomerInMortgageInstanceRole. CDM attribute class name: PartyInMktItemInstanceRoleCodeCode. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_CustomerInMortgageInstanceRole.
		/// </summary>
		private string _code;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return PartyInMktItemInstanceRoleCode._SCHEMA;
			}
		}
		/// <summary>
		/// Human readable unique identifier of role on product instance. MultiCodebook value itself.Unique across all products. Attribute has specific codebook type: CB_CustomerInMortgageInstanceRole. CDM attribute class name: PartyInMktItemInstanceRoleCodeCode. Attribute has simple type CB_MultiCodebookValue with description: Represents information that value can be from more than one codebook. Codebook type: CB_CustomerInMortgageInstanceRole.
		/// </summary>
		public string code
		{
			get
			{
				return this._code;
			}
			set
			{
				this._code = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.code;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.code = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
