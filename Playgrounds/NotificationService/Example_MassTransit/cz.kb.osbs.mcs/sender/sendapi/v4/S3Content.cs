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
	/// Relation betwwe bucket and content - describes, how contents are stored/managed on buckets. Every content managed on bucket has its unique objectKey.. ## Represents any content. CDM entity name(s): ContentInBucket,Content.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class S3Content : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""S3Content"",""doc"":""Relation betwwe bucket and content - describes, how contents are stored/managed on buckets. Every content managed on bucket has its unique objectKey.. ## Represents any content. CDM entity name(s): ContentInBucket,Content."",""namespace"":""cz.kb.osbs.mcs.sender.sendapi.v4"",""fields"":[{""name"":""filename"",""doc"":""Name of the file in which the content is stored. As on filesystem. Full file name, suffix is included.E.g. consumerLoanContract2410.pdfNazev souboru s priponou windows complaint podobe: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#naming_conventions. CDM attribute class name: ContentFilename."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,255}$""}},{""name"":""objectKey"",""doc"":"" CDM attribute class name: ContentInBucketObjectKey."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}}]}");
		/// <summary>
		/// Name of the file in which the content is stored. As on filesystem. Full file name, suffix is included.E.g. consumerLoanContract2410.pdfNazev souboru s priponou windows complaint podobe: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#naming_conventions. CDM attribute class name: ContentFilename.
		/// </summary>
		private string _filename;
		/// <summary>
		///  CDM attribute class name: ContentInBucketObjectKey.
		/// </summary>
		private string _objectKey;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return S3Content._SCHEMA;
			}
		}
		/// <summary>
		/// Name of the file in which the content is stored. As on filesystem. Full file name, suffix is included.E.g. consumerLoanContract2410.pdfNazev souboru s priponou windows complaint podobe: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#naming_conventions. CDM attribute class name: ContentFilename.
		/// </summary>
		public string filename
		{
			get
			{
				return this._filename;
			}
			set
			{
				this._filename = value;
			}
		}
		/// <summary>
		///  CDM attribute class name: ContentInBucketObjectKey.
		/// </summary>
		public string objectKey
		{
			get
			{
				return this._objectKey;
			}
			set
			{
				this._objectKey = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.filename;
			case 1: return this.objectKey;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.filename = (System.String)fieldValue; break;
			case 1: this.objectKey = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}