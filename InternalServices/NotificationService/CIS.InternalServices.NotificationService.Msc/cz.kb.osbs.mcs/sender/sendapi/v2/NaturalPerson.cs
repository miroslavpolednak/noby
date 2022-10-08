// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.sender.sendapi.v2
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Flesh and blood party - human being.Fyzicka osobaE.g. Jozko Mrkvicka. CDM entity name(s): NaturalPerson.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class NaturalPerson : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""NaturalPerson"",""doc"":""Flesh and blood party - human being.Fyzicka osobaE.g. Jozko Mrkvicka. CDM entity name(s): NaturalPerson."",""namespace"":""cz.kb.osbs.mcs.sender.sendapi.v2"",""fields"":[{""name"":""firstName"",""doc"":""First name of the NaturalPerson. CDM attribute class name: NaturalPersonFirstName."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,40}$""}},{""name"":""middleName"",""doc"":""Middle name of the natural person. CDM attribute class name: NaturalPersonMiddleName."",""default"":null,""type"":[""null"",{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,40}$""}]},{""name"":""surname"",""doc"":""Surname of the NaturalPerson. CDM attribute class name: NaturalPersonSurname."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,80}$""}}]}");
		/// <summary>
		/// First name of the NaturalPerson. CDM attribute class name: NaturalPersonFirstName.
		/// </summary>
		private string _firstName;
		/// <summary>
		/// Middle name of the natural person. CDM attribute class name: NaturalPersonMiddleName.
		/// </summary>
		private string _middleName;
		/// <summary>
		/// Surname of the NaturalPerson. CDM attribute class name: NaturalPersonSurname.
		/// </summary>
		private string _surname;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return NaturalPerson._SCHEMA;
			}
		}
		/// <summary>
		/// First name of the NaturalPerson. CDM attribute class name: NaturalPersonFirstName.
		/// </summary>
		public string firstName
		{
			get
			{
				return this._firstName;
			}
			set
			{
				this._firstName = value;
			}
		}
		/// <summary>
		/// Middle name of the natural person. CDM attribute class name: NaturalPersonMiddleName.
		/// </summary>
		public string middleName
		{
			get
			{
				return this._middleName;
			}
			set
			{
				this._middleName = value;
			}
		}
		/// <summary>
		/// Surname of the NaturalPerson. CDM attribute class name: NaturalPersonSurname.
		/// </summary>
		public string surname
		{
			get
			{
				return this._surname;
			}
			set
			{
				this._surname = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.firstName;
			case 1: return this.middleName;
			case 2: return this.surname;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.firstName = (System.String)fieldValue; break;
			case 1: this.middleName = (System.String)fieldValue; break;
			case 2: this.surname = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
