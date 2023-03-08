// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.noby.notification.sendapi.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Represents any content. CDM entity name(s): Content.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Content : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""Content"",""doc"":""Represents any content. CDM entity name(s): Content."",""namespace"":""cz.mpss.api.noby.notification.sendapi.v1"",""fields"":[{""name"":""charset"",""doc"":""Charset of content. CDM attribute class name: ContentCharset. Codebook type: CB_CharacterSet."",""default"":null,""type"":[""null"",{""type"":""string"",""avro.java.string"":""String""}]},{""name"":""format"",""doc"":""Format of content stored. CDM attribute class name: ContentFormat. Codebook type: CB_MIMEType."",""default"":null,""type"":[""null"",{""type"":""string"",""avro.java.string"":""String""}]},{""name"":""language"",""doc"":""Language of content. Codebook value. CDM attribute class name: ContentLanguage. Codebook type: CB_ContentLanguage."",""default"":null,""type"":[""null"",{""type"":""string"",""avro.java.string"":""String""}]},{""name"":""text"",""doc"":""Content in text format. CDM attribute class name: ContentText."",""type"":{""type"":""string"",""avro.java.string"":""String""}}]}");
		/// <summary>
		/// Charset of content. CDM attribute class name: ContentCharset. Codebook type: CB_CharacterSet.
		/// </summary>
		private string _charset;
		/// <summary>
		/// Format of content stored. CDM attribute class name: ContentFormat. Codebook type: CB_MIMEType.
		/// </summary>
		private string _format;
		/// <summary>
		/// Language of content. Codebook value. CDM attribute class name: ContentLanguage. Codebook type: CB_ContentLanguage.
		/// </summary>
		private string _language;
		/// <summary>
		/// Content in text format. CDM attribute class name: ContentText.
		/// </summary>
		private string _text;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Content._SCHEMA;
			}
		}
		/// <summary>
		/// Charset of content. CDM attribute class name: ContentCharset. Codebook type: CB_CharacterSet.
		/// </summary>
		public string charset
		{
			get
			{
				return this._charset;
			}
			set
			{
				this._charset = value;
			}
		}
		/// <summary>
		/// Format of content stored. CDM attribute class name: ContentFormat. Codebook type: CB_MIMEType.
		/// </summary>
		public string format
		{
			get
			{
				return this._format;
			}
			set
			{
				this._format = value;
			}
		}
		/// <summary>
		/// Language of content. Codebook value. CDM attribute class name: ContentLanguage. Codebook type: CB_ContentLanguage.
		/// </summary>
		public string language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
			}
		}
		/// <summary>
		/// Content in text format. CDM attribute class name: ContentText.
		/// </summary>
		public string text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.charset;
			case 1: return this.format;
			case 2: return this.language;
			case 3: return this.text;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.charset = (System.String)fieldValue; break;
			case 1: this.format = (System.String)fieldValue; break;
			case 2: this.language = (System.String)fieldValue; break;
			case 3: this.text = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}