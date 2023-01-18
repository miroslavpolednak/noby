// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.noby.notification.notificationreport.eventapi.v1.report
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// NotificationReportEvent is designed for the purposes of sending notification status to the consumer in order to ensure that a consumer is notified regarding each notification message (SMS, EMAIL, MSGBOX, PUSH)Further details available at https://wiki.kb.cz/display/BST/NotificationReportNotificationReportEvent works over the Kafka Asynchronous topic and is essentially used for sending a state for each notification. All available states are reflected at https://wiki.kb.cz/confluence/display/PATHFINDER/CB_NotificationStateVersion 3 changes includes:- /finalState is removed- /channelId kardinality has changed to compulsory- /MsgBoxSpecific.installation.externalId has been added. ## Represents activity of sending content to recipient through multi channel sender.. ## Timestamp is defined data and time when something was done.. ## Multicodebook component representing state of sending.Multicodebook because states can be different for sending different content type (sms, email, push, direct channel message). CDM entity name(s): Send,Timestamp,SendState.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class NotificationReport : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"NotificationReport\",\"doc\":\"NotificationReportEvent is de" +
				"signed for the purposes of sending notification status to the consumer in order " +
				"to ensure that a consumer is notified regarding each notification message (SMS, " +
				"EMAIL, MSGBOX, PUSH)Further details available at https://wiki.kb.cz/display/BST/" +
				"NotificationReportNotificationReportEvent works over the Kafka Asynchronous topi" +
				"c and is essentially used for sending a state for each notification. All availab" +
				"le states are reflected at https://wiki.kb.cz/confluence/display/PATHFINDER/CB_N" +
				"otificationStateVersion 3 changes includes:- /finalState is removed- /channelId " +
				"kardinality has changed to compulsory- /MsgBoxSpecific.installation.externalId h" +
				"as been added. ## Represents activity of sending content to recipient through mu" +
				"lti channel sender.. ## Timestamp is defined data and time when something was do" +
				"ne.. ## Multicodebook component representing state of sending.Multicodebook beca" +
				"use states can be different for sending different content type (sms, email, push" +
				", direct channel message). CDM entity name(s): Send,Timestamp,SendState.\",\"names" +
				"pace\":\"cz.mpss.api.noby.notification.notificationreport.eventapi.v1.report\",\"fie" +
				"lds\":[{\"name\":\"channel\",\"type\":{\"type\":\"record\",\"name\":\"Channel\",\"doc\":\"Represen" +
				"ts communication channel of bank with party. . ## Multicodebook property that un" +
				"iquely identifies channel. It is multicodebook as here can be any of subsets of " +
				"Channel codebook, which are defined as standalone codebooks. But here should not" +
				" be any principally different codebook. (e.g. ObtainType should not be here). CD" +
				"M entity name(s): Channel,ChannelIdentification.\",\"namespace\":\"cz.mpss.api.noby." +
				"notification.notificationreport.eventapi.v1.notificationreport\",\"fields\":[{\"name" +
				"\":\"id\",\"doc\":\"Multicodebook value of channel id itself. CDM attribute class name" +
				": ChannelIdentificationID. Codebook type: CB_MultiCodebookValue.\",\"type\":{\"type\"" +
				":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}},{\"name\":\"exact" +
				"lyOn\",\"doc\":\"DateTime format of timestamp. CDM attribute class name: TimestampEx" +
				"actlyOn.\",\"default\":null,\"type\":[\"null\",{\"type\":\"long\",\"logicalType\":\"local-time" +
				"stamp-millis\"}]},{\"name\":\"id\",\"doc\":\"Unique identifier. CDM attribute class name" +
				": SendId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,1" +
				"00}$\"}},{\"name\":\"msgBoxSpecific\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\"," +
				"\"name\":\"MsgBoxSpecific\",\"doc\":\"\",\"namespace\":\"cz.mpss.api.noby.notification.noti" +
				"ficationreport.eventapi.v1.notificationreport\",\"fields\":[{\"name\":\"installation\"," +
				"\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"Installation\",\"doc\":\"Spec" +
				"ific instance of direct channel fat application, that is installed on specific u" +
				"ser\'s device. CDM entity name(s): DirectChannelFatAppInstallation.\",\"namespace\":" +
				"\"cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport" +
				"\",\"fields\":[{\"name\":\"externalId\",\"doc\":\"Unique identifier of installation. CDM a" +
				"ttribute class name: ExternalId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"St" +
				"ring\",\"pattern\":\"^.{0,100}$\"}}]}]},{\"name\":\"msgAction\",\"default\":null,\"type\":[\"n" +
				"ull\",{\"type\":\"record\",\"name\":\"MsgAction\",\"doc\":\" CDM entity name(s): MessageActi" +
				"on.\",\"namespace\":\"cz.mpss.api.noby.notification.notificationreport.eventapi.v1.n" +
				"otificationreport\",\"fields\":[{\"name\":\"area\",\"doc\":\" CDM attribute class name: Di" +
				"rectChannelMessageArea. Codebook type: CB_MsgbArea.\",\"default\":null,\"type\":[\"nul" +
				"l\",{\"type\":\"string\",\"avro.java.string\":\"String\"}]},{\"name\":\"platform\",\"doc\":\" CD" +
				"M attribute class name: DirectChannelMessagePlatform. Codebook type: CB_MsgbPlat" +
				"form.\",\"default\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\":\"String" +
				"\"}]}]}]}]}]},{\"name\":\"notificationErrors\",\"default\":null,\"type\":[\"null\",{\"type\":" +
				"\"array\",\"items\":{\"type\":\"record\",\"name\":\"NotificationError\",\"doc\":\"Description o" +
				"f something that went wrong.. ## Any category of error in categorization.. ## Mu" +
				"lticodebook property uniquely identifying error type.Multicodebook because there" +
				" are many different error categorizations. CDM entity name(s): Error,ErrorType,E" +
				"rrorTypeCode.\",\"namespace\":\"cz.mpss.api.noby.notification.notificationreport.eve" +
				"ntapi.v1.notificationreport\",\"fields\":[{\"name\":\"code\",\"doc\":\"Unique identifier o" +
				"f error type itself. CDM attribute class name: ErrorTypeCodeCode. Codebook type:" +
				" CB_MultiCodebookValue.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pa" +
				"ttern\":\"^.{0,100}$\"}},{\"name\":\"message\",\"doc\":\"Message about error given to acto" +
				"r affected by error. CDM attribute class name: ErrorMessage.\",\"type\":{\"type\":\"st" +
				"ring\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,4000}$\"}}]}}]},{\"name\":\"pushSp" +
				"ecific\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"PushSpecific\",\"do" +
				"c\":\"\",\"namespace\":\"cz.mpss.api.noby.notification.notificationreport.eventapi.v1." +
				"notificationreport\",\"fields\":[{\"name\":\"installation\",\"type\":\"Installation\"}]}]}," +
				"{\"name\":\"state\",\"doc\":\"Value of multicodebook attribute state itself. CDM attrib" +
				"ute class name: SendStateState. Codebook type: CB_MultiCodebookValue.\",\"type\":{\"" +
				"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}],\"javaAnnota" +
				"tion\":\"cz.kb.api.common.annotation.ConfluentSchemaRegistryCompatible\"}");
		private cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.Channel _channel;
		/// <summary>
		/// DateTime format of timestamp. CDM attribute class name: TimestampExactlyOn.
		/// </summary>
		private System.Nullable<System.DateTime> _exactlyOn;
		/// <summary>
		/// Unique identifier. CDM attribute class name: SendId.
		/// </summary>
		private string _id;
		private cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.MsgBoxSpecific _msgBoxSpecific;
		private IList<cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.NotificationError> _notificationErrors;
		private cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.PushSpecific _pushSpecific;
		/// <summary>
		/// Value of multicodebook attribute state itself. CDM attribute class name: SendStateState. Codebook type: CB_MultiCodebookValue.
		/// </summary>
		private string _state;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return NotificationReport._SCHEMA;
			}
		}
		public cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.Channel channel
		{
			get
			{
				return this._channel;
			}
			set
			{
				this._channel = value;
			}
		}
		/// <summary>
		/// DateTime format of timestamp. CDM attribute class name: TimestampExactlyOn.
		/// </summary>
		public System.Nullable<System.DateTime> exactlyOn
		{
			get
			{
				return this._exactlyOn;
			}
			set
			{
				this._exactlyOn = value;
			}
		}
		/// <summary>
		/// Unique identifier. CDM attribute class name: SendId.
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
		public cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.MsgBoxSpecific msgBoxSpecific
		{
			get
			{
				return this._msgBoxSpecific;
			}
			set
			{
				this._msgBoxSpecific = value;
			}
		}
		public IList<cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.NotificationError> notificationErrors
		{
			get
			{
				return this._notificationErrors;
			}
			set
			{
				this._notificationErrors = value;
			}
		}
		public cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.PushSpecific pushSpecific
		{
			get
			{
				return this._pushSpecific;
			}
			set
			{
				this._pushSpecific = value;
			}
		}
		/// <summary>
		/// Value of multicodebook attribute state itself. CDM attribute class name: SendStateState. Codebook type: CB_MultiCodebookValue.
		/// </summary>
		public string state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.channel;
			case 1: return this.exactlyOn;
			case 2: return this.id;
			case 3: return this.msgBoxSpecific;
			case 4: return this.notificationErrors;
			case 5: return this.pushSpecific;
			case 6: return this.state;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.channel = (cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.Channel)fieldValue; break;
			case 1: this.exactlyOn = (System.Nullable<System.DateTime>)fieldValue; break;
			case 2: this.id = (System.String)fieldValue; break;
			case 3: this.msgBoxSpecific = (cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.MsgBoxSpecific)fieldValue; break;
			case 4: this.notificationErrors = (IList<cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.NotificationError>)fieldValue; break;
			case 5: this.pushSpecific = (cz.mpss.api.noby.notification.notificationreport.eventapi.v1.notificationreport.PushSpecific)fieldValue; break;
			case 6: this.state = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
