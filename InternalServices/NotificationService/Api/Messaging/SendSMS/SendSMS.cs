// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.sender.sendapi.v4.sms
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Represents any content.. ## Activity of sending short (digital) message from bank to a user or party (typically, but not limited to customer). Typically about important:- task events- case changesThis is one notification and will be sent to all recipients through the same defined channels and the same attached documents.E.g. if there is need to send different attachments, then multiple notifications has to be created. CDM entity name(s): Content,Notification.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class SendSMS : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"SendSMS\",\"doc\":\"Represents any content.. ## Activity of " +
				"sending short (digital) message from bank to a user or party (typically, but not" +
				" limited to customer). Typically about important:- task events- case changesThis" +
				" is one notification and will be sent to all recipients through the same defined" +
				" channels and the same attached documents.E.g. if there is need to send differen" +
				"t attachments, then multiple notifications has to be created. CDM entity name(s)" +
				": Content,Notification.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v4.sms\",\"fie" +
				"lds\":[{\"name\":\"deliveryTiming\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"n" +
				"ame\":\"DeliveryTiming\",\"doc\":\"Date and time range.At least one of min, max must b" +
				"e defined. If only one is defined, then the other one is automatically +,- infin" +
				"ity. CDM entity name(s): DateTimeRange.\",\"namespace\":\"cz.kb.osbs.mcs.sender.send" +
				"api.v4\",\"fields\":[{\"name\":\"max\",\"doc\":\"max, inclusive. CDM attribute class name:" +
				" DateTimeRangeMax.##\",\"default\":null,\"type\":[\"null\",{\"type\":\"long\",\"logicalType\"" +
				":\"timestamp-millis\"}]},{\"name\":\"min\",\"doc\":\"min, inclusive. CDM attribute class " +
				"name: DateTimeRangeMin.##\",\"default\":null,\"type\":[\"null\",{\"type\":\"long\",\"logical" +
				"Type\":\"timestamp-millis\"}]}]}]},{\"name\":\"id\",\"doc\":\"ID of the notification. CDM " +
				"attribute class name: NotificationId.##\",\"type\":{\"type\":\"string\",\"avro.java.stri" +
				"ng\":\"String\",\"pattern\":\"^.{0,30}$\"}},{\"name\":\"notificationConsumer\",\"type\":{\"typ" +
				"e\":\"record\",\"name\":\"NotificationConsumer\",\"doc\":\"ConsumerID bude prirazeno pri r" +
				"egistraci konzumenta na Sender.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v4\"," +
				"\"fields\":[{\"name\":\"consumerId\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"Strin" +
				"g\"}}]}},{\"name\":\"phone\",\"type\":{\"type\":\"record\",\"name\":\"Phone\",\"doc\":\"Phone as a" +
				" contact on party.. ## Represents telephone number. CDM entity name(s): PhoneCon" +
				"tact,PhoneNumber.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v4\",\"fields\":[{\"na" +
				"me\":\"countryCode\",\"doc\":\"Country code of the MSISDN. It is \'CC\' part of msisdn, " +
				"see belowBy definition, MSISDN is composed of CC+NDC+SN where<ul>\\t<li>CC stands" +
				" for \'Country Code\'</li>\\t<li>NDC stands for \'National Destination Code\'</li>\\t<" +
				"li>SN stands for \'Subscriber Name\'</li></ul>E.g.: 420. CDM attribute class name:" +
				" PhoneNumberCountryCode.##\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"," +
				"\"pattern\":\"^.{0,5}$\"}},{\"name\":\"nationalPhoneNumber\",\"doc\":\"Local part of the MS" +
				"ISDN. It is concatenated \'NDC+SN\', see definition below.By definition, MSISDN is" +
				" composed of CC+NDC+SN where<ul>\\t<li>CC stands for \'Country Code\'</li>\\t<li>NDC" +
				" stands for \'National Destination Code\'</li>\\t<li>SN stands for \'Subscriber Name" +
				"\'</li></ul>E.g.: 603123456. CDM attribute class name: PhoneNumberNationalPhoneNu" +
				"mber.##\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,14}" +
				"$\"}}]}},{\"name\":\"processingPriority\",\"doc\":\"Priority of notification that determ" +
				"ines order in which notification are sent.e.g. high or standard. CDM attribute c" +
				"lass name: NotificationProcessingPriority.##\",\"default\":null,\"type\":[\"null\",\"int" +
				"\"]},{\"name\":\"sendingTimeframe\",\"doc\":\"##\",\"default\":null,\"type\":[\"null\",{\"type\":" +
				"\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,20}$\"}]},{\"name\":\"text\",\"do" +
				"c\":\"Content in text format. CDM attribute class name: ContentText.##\",\"type\":{\"t" +
				"ype\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"type\",\"doc\":\"Multicodebook " +
				"attribute type itself. CDM attribute class name: NotificationTypeType. Codebook " +
				"type: CB_MultiCodebookValue.##\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"Stri" +
				"ng\",\"pattern\":\"^.{0,100}$\"}}],\"javaAnnotation\":\"cz.kb.api.common.annotation.Conf" +
				"luentSchemaRegistryCompatible\"}");
		private cz.kb.osbs.mcs.sender.sendapi.v4.DeliveryTiming _deliveryTiming;
		/// <summary>
		/// ID of the notification. CDM attribute class name: NotificationId.##
		/// </summary>
		private string _id;
		private cz.kb.osbs.mcs.sender.sendapi.v4.NotificationConsumer _notificationConsumer;
		private cz.kb.osbs.mcs.sender.sendapi.v4.Phone _phone;
		/// <summary>
		/// Priority of notification that determines order in which notification are sent.e.g. high or standard. CDM attribute class name: NotificationProcessingPriority.##
		/// </summary>
		private System.Nullable<System.Int32> _processingPriority;
		/// <summary>
		/// ##
		/// </summary>
		private string _sendingTimeframe;
		/// <summary>
		/// Content in text format. CDM attribute class name: ContentText.##
		/// </summary>
		private string _text;
		/// <summary>
		/// Multicodebook attribute type itself. CDM attribute class name: NotificationTypeType. Codebook type: CB_MultiCodebookValue.##
		/// </summary>
		private string _type;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return SendSMS._SCHEMA;
			}
		}
		public cz.kb.osbs.mcs.sender.sendapi.v4.DeliveryTiming deliveryTiming
		{
			get
			{
				return this._deliveryTiming;
			}
			set
			{
				this._deliveryTiming = value;
			}
		}
		/// <summary>
		/// ID of the notification. CDM attribute class name: NotificationId.##
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
		public cz.kb.osbs.mcs.sender.sendapi.v4.NotificationConsumer notificationConsumer
		{
			get
			{
				return this._notificationConsumer;
			}
			set
			{
				this._notificationConsumer = value;
			}
		}
		public cz.kb.osbs.mcs.sender.sendapi.v4.Phone phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				this._phone = value;
			}
		}
		/// <summary>
		/// Priority of notification that determines order in which notification are sent.e.g. high or standard. CDM attribute class name: NotificationProcessingPriority.##
		/// </summary>
		public System.Nullable<System.Int32> processingPriority
		{
			get
			{
				return this._processingPriority;
			}
			set
			{
				this._processingPriority = value;
			}
		}
		/// <summary>
		/// ##
		/// </summary>
		public string sendingTimeframe
		{
			get
			{
				return this._sendingTimeframe;
			}
			set
			{
				this._sendingTimeframe = value;
			}
		}
		/// <summary>
		/// Content in text format. CDM attribute class name: ContentText.##
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
		/// <summary>
		/// Multicodebook attribute type itself. CDM attribute class name: NotificationTypeType. Codebook type: CB_MultiCodebookValue.##
		/// </summary>
		public string type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.deliveryTiming;
			case 1: return this.id;
			case 2: return this.notificationConsumer;
			case 3: return this.phone;
			case 4: return this.processingPriority;
			case 5: return this.sendingTimeframe;
			case 6: return this.text;
			case 7: return this.type;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.deliveryTiming = (cz.kb.osbs.mcs.sender.sendapi.v4.DeliveryTiming)fieldValue; break;
			case 1: this.id = (System.String)fieldValue; break;
			case 2: this.notificationConsumer = (cz.kb.osbs.mcs.sender.sendapi.v4.NotificationConsumer)fieldValue; break;
			case 3: this.phone = (cz.kb.osbs.mcs.sender.sendapi.v4.Phone)fieldValue; break;
			case 4: this.processingPriority = (System.Nullable<System.Int32>)fieldValue; break;
			case 5: this.sendingTimeframe = (System.String)fieldValue; break;
			case 6: this.text = (System.String)fieldValue; break;
			case 7: this.type = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
