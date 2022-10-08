// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.notificationreport.eventapi.v2.notificationreport
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Specific instance of direct channel fat application, that is installed on specific user's device. CDM entity name(s): DirectChannelFatAppInstallation.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Installation : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""Installation"",""doc"":""Specific instance of direct channel fat application, that is installed on specific user's device. CDM entity name(s): DirectChannelFatAppInstallation."",""namespace"":""cz.kb.osbs.mcs.notificationreport.eventapi.v2.notificationreport"",""fields"":[{""name"":""externalId"",""doc"":""Unique identifier of installation. CDM attribute class name: ExternalId."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}}]}");
		/// <summary>
		/// Unique identifier of installation. CDM attribute class name: ExternalId.
		/// </summary>
		private string _externalId;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Installation._SCHEMA;
			}
		}
		/// <summary>
		/// Unique identifier of installation. CDM attribute class name: ExternalId.
		/// </summary>
		public string externalId
		{
			get
			{
				return this._externalId;
			}
			set
			{
				this._externalId = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.externalId;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.externalId = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
