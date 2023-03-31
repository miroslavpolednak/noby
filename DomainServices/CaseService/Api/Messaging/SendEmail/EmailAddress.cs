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
	/// Represents email address.. ## Email as a contact on party.. ## 'PartyInBankRole'Party role is description of relationship of (any view on) party to KB or KBGroup. I.e. has information about any view on party in context of KB (or other KB group entity) in SPECIFIC CONTEXT/role. CDM entity name(s): EmailAddress,EmailContact,PartyRole.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class EmailAddress : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"EmailAddress\",\"doc\":\"Represents email address.. ## Email" +
				" as a contact on party.. ## \'PartyInBankRole\'Party role is description of relati" +
				"onship of (any view on) party to KB or KBGroup. I.e. has information about any v" +
				"iew on party in context of KB (or other KB group entity) in SPECIFIC CONTEXT/rol" +
				"e. CDM entity name(s): EmailAddress,EmailContact,PartyRole.\",\"namespace\":\"cz.kb." +
				"osbs.mcs.sender.sendapi.v4\",\"fields\":[{\"name\":\"party\",\"default\":null,\"type\":[\"nu" +
				"ll\",{\"type\":\"record\",\"name\":\"Party\",\"doc\":\" This element is choice - only one of" +
				" the child fields can be filled.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v4\"" +
				",\"fields\":[{\"name\":\"legalPerson\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\"," +
				"\"name\":\"LegalPerson\",\"doc\":\"Juridical person that is NOT <b>fixed</b> to <b>exac" +
				"tly one </b>natural person. CDM entity name(s): LegalPerson.\",\"namespace\":\"cz.kb" +
				".osbs.mcs.sender.sendapi.v4\",\"fields\":[{\"name\":\"name\",\"doc\":\"Official name of ju" +
				"ridical person. CDM attribute class name: JuridicalPersonName.\",\"type\":{\"type\":\"" +
				"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}}]}]},{\"name\":\"natura" +
				"lPerson\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"NaturalPerson\",\"" +
				"doc\":\"Flesh and blood party - human being.Fyzicka osobaE.g. Jozko Mrkvicka. CDM " +
				"entity name(s): NaturalPerson.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v4\",\"" +
				"fields\":[{\"name\":\"firstName\",\"doc\":\"First name of the NaturalPerson. CDM attribu" +
				"te class name: NaturalPersonFirstName.\",\"type\":{\"type\":\"string\",\"avro.java.strin" +
				"g\":\"String\",\"pattern\":\"^.{0,40}$\"}},{\"name\":\"middleName\",\"doc\":\"Middle name of t" +
				"he natural person. CDM attribute class name: NaturalPersonMiddleName.\",\"default\"" +
				":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{" +
				"0,40}$\"}]},{\"name\":\"surname\",\"doc\":\"Surname of the NaturalPerson. CDM attribute " +
				"class name: NaturalPersonSurname.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"S" +
				"tring\",\"pattern\":\"^.{0,80}$\"}}]}]}]}]},{\"name\":\"value\",\"doc\":\"Textual email addr" +
				"essE.g. jozko.mrkvicka@gmail.com. CDM attribute class name: EmailAddressValue.\"," +
				"\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}}]}");
		private cz.kb.osbs.mcs.sender.sendapi.v4.Party _party;
		/// <summary>
		/// Textual email addressE.g. jozko.mrkvicka@gmail.com. CDM attribute class name: EmailAddressValue.
		/// </summary>
		private string _value;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return EmailAddress._SCHEMA;
			}
		}
		public cz.kb.osbs.mcs.sender.sendapi.v4.Party party
		{
			get
			{
				return this._party;
			}
			set
			{
				this._party = value;
			}
		}
		/// <summary>
		/// Textual email addressE.g. jozko.mrkvicka@gmail.com. CDM attribute class name: EmailAddressValue.
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
			case 0: return this.party;
			case 1: return this.value;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.party = (cz.kb.osbs.mcs.sender.sendapi.v4.Party)fieldValue; break;
			case 1: this.value = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
