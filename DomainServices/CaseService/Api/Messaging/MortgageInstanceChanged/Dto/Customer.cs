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
	/// Subject registred in Customer Management. CDM entity name(s): Customer.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Customer : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""Customer"",""doc"":""Subject registred in Customer Management. CDM entity name(s): Customer."",""namespace"":""cz.kb.api.mortgageservicingevents.v3.mortgageinstance"",""fields"":[{""name"":""customerId"",""doc"":""Identifier of the customer  (KBID - identifier of customer in Customer Management system). CDM attribute class name: CustomerCustomerId."",""type"":""long""}]}");
		/// <summary>
		/// Identifier of the customer  (KBID - identifier of customer in Customer Management system). CDM attribute class name: CustomerCustomerId.
		/// </summary>
		private long _customerId;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Customer._SCHEMA;
			}
		}
		/// <summary>
		/// Identifier of the customer  (KBID - identifier of customer in Customer Management system). CDM attribute class name: CustomerCustomerId.
		/// </summary>
		public long customerId
		{
			get
			{
				return this._customerId;
			}
			set
			{
				this._customerId = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.customerId;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.customerId = (System.Int64)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
