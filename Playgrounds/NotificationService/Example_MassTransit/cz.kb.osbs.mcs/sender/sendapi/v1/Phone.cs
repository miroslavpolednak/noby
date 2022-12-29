// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.sender.sendapi.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Phone as a contact on party.. ## Represents telephone number. CDM entity name: PhoneContact,PhoneNumber.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Phone : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""Phone"",""doc"":""Phone as a contact on party.. ## Represents telephone number. CDM entity name: PhoneContact,PhoneNumber."",""namespace"":""cz.kb.osbs.mcs.sender.sendapi.v1"",""fields"":[{""name"":""countryCode"",""doc"":""Country code of the MSISDN. It is 'CC' part of msisdn, see belowBy definition, MSISDN is composed of CC+NDC+SN where<ul>\t<li>CC stands for 'Country Code'</li>\t<li>NDC stands for 'National Destination Code'</li>\t<li>SN stands for 'Subscriber Name'</li></ul>E.g.: 420. CDM attribute class name: PhoneNumberCountryCode."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,5}$""}},{""name"":""nationalPhoneNumber"",""doc"":""Local part of the MSISDN. It is concatenated 'NDC+SN', see definition below.By definition, MSISDN is composed of CC+NDC+SN where<ul>\t<li>CC stands for 'Country Code'</li>\t<li>NDC stands for 'National Destination Code'</li>\t<li>SN stands for 'Subscriber Name'</li></ul>E.g.: 603123456. CDM attribute class name: PhoneNumberNationalPhoneNumber."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,14}$""}}]}");
		/// <summary>
		/// Country code of the MSISDN. It is 'CC' part of msisdn, see belowBy definition, MSISDN is composed of CC+NDC+SN where<ul>	<li>CC stands for 'Country Code'</li>	<li>NDC stands for 'National Destination Code'</li>	<li>SN stands for 'Subscriber Name'</li></ul>E.g.: 420. CDM attribute class name: PhoneNumberCountryCode.
		/// </summary>
		private string _countryCode;
		/// <summary>
		/// Local part of the MSISDN. It is concatenated 'NDC+SN', see definition below.By definition, MSISDN is composed of CC+NDC+SN where<ul>	<li>CC stands for 'Country Code'</li>	<li>NDC stands for 'National Destination Code'</li>	<li>SN stands for 'Subscriber Name'</li></ul>E.g.: 603123456. CDM attribute class name: PhoneNumberNationalPhoneNumber.
		/// </summary>
		private string _nationalPhoneNumber;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Phone._SCHEMA;
			}
		}
		/// <summary>
		/// Country code of the MSISDN. It is 'CC' part of msisdn, see belowBy definition, MSISDN is composed of CC+NDC+SN where<ul>	<li>CC stands for 'Country Code'</li>	<li>NDC stands for 'National Destination Code'</li>	<li>SN stands for 'Subscriber Name'</li></ul>E.g.: 420. CDM attribute class name: PhoneNumberCountryCode.
		/// </summary>
		public string countryCode
		{
			get
			{
				return this._countryCode;
			}
			set
			{
				this._countryCode = value;
			}
		}
		/// <summary>
		/// Local part of the MSISDN. It is concatenated 'NDC+SN', see definition below.By definition, MSISDN is composed of CC+NDC+SN where<ul>	<li>CC stands for 'Country Code'</li>	<li>NDC stands for 'National Destination Code'</li>	<li>SN stands for 'Subscriber Name'</li></ul>E.g.: 603123456. CDM attribute class name: PhoneNumberNationalPhoneNumber.
		/// </summary>
		public string nationalPhoneNumber
		{
			get
			{
				return this._nationalPhoneNumber;
			}
			set
			{
				this._nationalPhoneNumber = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.countryCode;
			case 1: return this.nationalPhoneNumber;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.countryCode = (System.String)fieldValue; break;
			case 1: this.nationalPhoneNumber = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}