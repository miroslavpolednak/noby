// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.sender.sendapi.v3
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Customer is a party role. It is view on party from banks perspective and context that party had/has/will have or is even only related to a product or service from KB.(party has some relationship to bank in context of Customer Management). CDM entity name(s): KBCustomer.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class KBCustomer : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""KBCustomer"",""doc"":""Customer is a party role. It is view on party from banks perspective and context that party had/has/will have or is even only related to a product or service from KB.(party has some relationship to bank in context of Customer Management). CDM entity name(s): KBCustomer."",""namespace"":""cz.kb.osbs.mcs.sender.sendapi.v3"",""fields"":[{""name"":""id"",""doc"":""Unique identifier of the customer.KBI_ID, sometimes also refered to as KBID e.g. in CB_IdentityScheme. CDM attribute class name: KBCustomerId."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""[1-9][0-9]*""}}]}");
		/// <summary>
		/// Unique identifier of the customer.KBI_ID, sometimes also refered to as KBID e.g. in CB_IdentityScheme. CDM attribute class name: KBCustomerId.
		/// </summary>
		private string _id;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return KBCustomer._SCHEMA;
			}
		}
		/// <summary>
		/// Unique identifier of the customer.KBI_ID, sometimes also refered to as KBID e.g. in CB_IdentityScheme. CDM attribute class name: KBCustomerId.
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
