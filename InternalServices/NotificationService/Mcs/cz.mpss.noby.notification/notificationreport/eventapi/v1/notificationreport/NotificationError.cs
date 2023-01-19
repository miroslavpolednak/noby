// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Description of something that went wrong.. ## Any category of error in categorization.. ## Multicodebook property uniquely identifying error type.Multicodebook because there are many different error categorizations. CDM entity name(s): Error,ErrorType,ErrorTypeCode.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class NotificationError : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""NotificationError"",""doc"":""Description of something that went wrong.. ## Any category of error in categorization.. ## Multicodebook property uniquely identifying error type.Multicodebook because there are many different error categorizations. CDM entity name(s): Error,ErrorType,ErrorTypeCode."",""namespace"":""cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport"",""fields"":[{""name"":""code"",""doc"":""Unique identifier of error type itself. CDM attribute class name: ErrorTypeCodeCode. Codebook type: CB_MultiCodebookValue."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}},{""name"":""message"",""doc"":""Message about error given to actor affected by error. CDM attribute class name: ErrorMessage."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,4000}$""}}]}");
		/// <summary>
		/// Unique identifier of error type itself. CDM attribute class name: ErrorTypeCodeCode. Codebook type: CB_MultiCodebookValue.
		/// </summary>
		private string _code;
		/// <summary>
		/// Message about error given to actor affected by error. CDM attribute class name: ErrorMessage.
		/// </summary>
		private string _message;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return NotificationError._SCHEMA;
			}
		}
		/// <summary>
		/// Unique identifier of error type itself. CDM attribute class name: ErrorTypeCodeCode. Codebook type: CB_MultiCodebookValue.
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
		/// <summary>
		/// Message about error given to actor affected by error. CDM attribute class name: ErrorMessage.
		/// </summary>
		public string message
		{
			get
			{
				return this._message;
			}
			set
			{
				this._message = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.code;
			case 1: return this.message;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.code = (System.String)fieldValue; break;
			case 1: this.message = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
