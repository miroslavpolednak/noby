// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.noby.notification.sendapi.v1.email
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Activity of sending short (digital) message from bank to a user or party (typically, but not limited to customer). Typically about important:- task events- case changesThis is one notification and will be sent to all recipients through the same defined channels and the same attached documents.E.g. if there is need to send different attachments, then multiple notifications has to be created.. ## Timing when notification is sent to recipient. CDM entity name(s): Notification,DeliveryTiming.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class SendEmail : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"SendEmail\",\"doc\":\"Activity of sending short (digital) me" +
				"ssage from bank to a user or party (typically, but not limited to customer). Typ" +
				"ically about important:- task events- case changesThis is one notification and w" +
				"ill be sent to all recipients through the same defined channels and the same att" +
				"ached documents.E.g. if there is need to send different attachments, then multip" +
				"le notifications has to be created.. ## Timing when notification is sent to reci" +
				"pient. CDM entity name(s): Notification,DeliveryTiming.\",\"namespace\":\"cz.mpss.ap" +
				"i.noby.notification.sendapi.v1.email\",\"fields\":[{\"name\":\"attachments\",\"default\":" +
				"null,\"type\":[\"null\",{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"Attachment\"" +
				",\"doc\":\" This element is choice - only one of the child fields can be filled.\",\"" +
				"namespace\":\"cz.mpss.api.noby.notification.sendapi.v1\",\"fields\":[{\"name\":\"binary\"" +
				",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"Binary\",\"doc\":\"Verze dok" +
				"umentuDocument can change over time. Document version captures document in a giv" +
				"en time.Verze dokumentu (SLV 24-001, kapitola 01, bod 38 [1]) je nositelem vlast" +
				"niho obsahu dokumentu. Kazdy dokument ma alespon jednu verzi. K jednomu dokument" +
				"u muze byt N verzi dokumentu. Verze jsou vyuziti pri postupnem vyvoji obsahu jed" +
				"noho dokumentu (smlouva ve verzi 1, smlouva ve verzi 2 atp.)Reprezentuji postupn" +
				"y vyvoj dokumentu. Dalsi verze plne nahrazuje verzi predchozi, ktera se stava ti" +
				"mto neplatnou.. ## Represents any content.. ## Represents activity of content ge" +
				"neration. Used to hold relationships to entities involved.. ## Version informati" +
				"on about content template. CDM entity name(s): DocumentVersion,Content,ContentGe" +
				"neration,ContentTemplateVersion.\",\"namespace\":\"cz.mpss.api.noby.notification.sen" +
				"dapi.v1\",\"fields\":[{\"name\":\"binary\",\"doc\":\"Content in base64 binary format. CDM " +
				"attribute class name: ContentBinary.\",\"type\":\"bytes\"},{\"name\":\"filename\",\"doc\":\"" +
				"Name of the file in which the content is stored. As on filesystem. Full file nam" +
				"e, suffix is included.E.g. consumerLoanContract2410.pdfNazev souboru s priponou " +
				"windows complaint podobe: https://msdn.microsoft.com/en-us/library/windows/deskt" +
				"op/aa365247(v=vs.85).aspx#naming_conventions. CDM attribute class name: ContentF" +
				"ilename.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,25" +
				"5}$\"}}]}]},{\"name\":\"document\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"na" +
				"me\":\"Document\",\"doc\":\"Dokument z DMSX. Identifikovan je pres dokumentID. Volitel" +
				"ne je mozne specifikovat verzi dokumentu. Pokud verze nebude zadana, vezme se po" +
				"sledni.. ## Obsah dokumentu/souborDMS: Rendition. ## DokumentDocument is any pap" +
				"er or electronic record or information.Dokument je kazdy pisemny, obrazovy, zvuk" +
				"ovy, elektronicky nebo jiny zaznam, at jiz v podobe analogove ci digitalni, kter" +
				"y vznikl z cinnosti puvodce.. ## Verze dokumentuDocument can change over time. D" +
				"ocument version captures document in a given time.Verze dokumentu (SLV 24-001, k" +
				"apitola 01, bod 38 [1]) je nositelem vlastniho obsahu dokumentu. Kazdy dokument " +
				"ma alespon jednu verzi. K jednomu dokumentu muze byt N verzi dokumentu. Verze js" +
				"ou vyuziti pri postupnem vyvoji obsahu jednoho dokumentu (smlouva ve verzi 1, sm" +
				"louva ve verzi 2 atp.)Reprezentuji postupny vyvoj dokumentu. Dalsi verze plne na" +
				"hrazuje verzi predchozi, ktera se stava timto neplatnou.. ## Unique identifier o" +
				"f document. Does not change with document version.Multi identifier - there are a" +
				"t least two separate identifiers used for documents across bank. CDM entity name" +
				"(s): DocumentContent,Document,DocumentVersion,DocumentId.\",\"namespace\":\"cz.mpss." +
				"api.noby.notification.sendapi.v1\",\"fields\":[{\"name\":\"fileExtension\",\"doc\":\"File " +
				"extension of document content stored.E.g. exe, pdf, docx. CDM attribute class na" +
				"me: DocumentContentFileExtension. Codebook type: CB_FileExtension.\",\"type\":{\"typ" +
				"e\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"id\",\"doc\":\"Unique identifier " +
				"of document. Does not change with document version. Multi identifier.Jednoznacny" +
				" identifikator dokumentu. DMS: ChronicleID, RootDocID. Pokud je dokument vkladan" +
				"y do DMS uprostred sveho zivotniho cyklu, je zadouci, aby konzument pri jeho ulo" +
				"zeni vyplnoval docId v pripade, ze vyhovuje formatu = cerpa ze stejne rady IGS d" +
				"ocId.Escudo:. CDM attribute class name: DocumentIdId.\",\"type\":{\"type\":\"string\",\"" +
				"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}},{\"name\":\"version\",\"doc\":\"Vers" +
				"ion of document. Only contains identifier of version, e.g 14Called \'business\' ve" +
				"rsion, as this document version must be increased by explicit consumer.Ciselne o" +
				"znaceni verze v kompetenci DMS. Viz INS 22-011. CDM attribute class name: Docume" +
				"ntVersionVersion.\",\"default\":null,\"type\":[\"null\",\"int\"]}]}]},{\"name\":\"s3Content\"" +
				",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"S3Content\",\"doc\":\"Relati" +
				"on betwwe bucket and content - describes, how contents are stored/managed on buc" +
				"kets. Every content managed on bucket has its unique objectKey.. ## Represents a" +
				"ny content. CDM entity name(s): ContentInBucket,Content.\",\"namespace\":\"cz.mpss.a" +
				"pi.noby.notification.sendapi.v1\",\"fields\":[{\"name\":\"filename\",\"doc\":\"Name of the" +
				" file in which the content is stored. As on filesystem. Full file name, suffix i" +
				"s included.E.g. consumerLoanContract2410.pdfNazev souboru s priponou windows com" +
				"plaint podobe: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247" +
				"(v=vs.85).aspx#naming_conventions. CDM attribute class name: ContentFilename.\",\"" +
				"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}},{\"na" +
				"me\":\"objectKey\",\"doc\":\" CDM attribute class name: ContentInBucketObjectKey.\",\"ty" +
				"pe\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}}]}]}]}}" +
				"]},{\"name\":\"bcc\",\"default\":null,\"type\":[\"null\",{\"type\":\"array\",\"items\":{\"type\":\"" +
				"record\",\"name\":\"EmailAddress\",\"doc\":\"Represents email address.. ## Email as a co" +
				"ntact on party.. ## \'PartyInBankRole\'Party role is description of relationship o" +
				"f (any view on) party to KB or KBGroup. I.e. has information about any view on p" +
				"arty in context of KB (or other KB group entity) in SPECIFIC CONTEXT/role. CDM e" +
				"ntity name(s): EmailAddress,EmailContact,PartyRole.\",\"namespace\":\"cz.mpss.api.no" +
				"by.notification.sendapi.v1\",\"fields\":[{\"name\":\"party\",\"default\":null,\"type\":[\"nu" +
				"ll\",{\"type\":\"record\",\"name\":\"Party\",\"doc\":\" This element is choice - only one of" +
				" the child fields can be filled.\",\"namespace\":\"cz.mpss.api.noby.notification.sen" +
				"dapi.v1\",\"fields\":[{\"name\":\"legalPerson\",\"default\":null,\"type\":[\"null\",{\"type\":\"" +
				"record\",\"name\":\"LegalPerson\",\"doc\":\"Juridical person that is NOT <b>fixed</b> to" +
				" <b>exactly one </b>natural person. CDM entity name(s): LegalPerson.\",\"namespace" +
				"\":\"cz.mpss.api.noby.notification.sendapi.v1\",\"fields\":[{\"name\":\"name\",\"doc\":\"Off" +
				"icial name of juridical person. CDM attribute class name: JuridicalPersonName.\"," +
				"\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}}]}]}" +
				",{\"name\":\"naturalPerson\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"" +
				"NaturalPerson\",\"doc\":\"Flesh and blood party - human being.Fyzicka osobaE.g. Jozk" +
				"o Mrkvicka. CDM entity name(s): NaturalPerson.\",\"namespace\":\"cz.mpss.api.noby.no" +
				"tification.sendapi.v1\",\"fields\":[{\"name\":\"firstName\",\"doc\":\"First name of the Na" +
				"turalPerson. CDM attribute class name: NaturalPersonFirstName.\",\"type\":{\"type\":\"" +
				"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,40}$\"}},{\"name\":\"middleName\"" +
				",\"doc\":\"Middle name of the natural person. CDM attribute class name: NaturalPers" +
				"onMiddleName.\",\"default\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\"" +
				":\"String\",\"pattern\":\"^.{0,40}$\"}]},{\"name\":\"surname\",\"doc\":\"Surname of the Natur" +
				"alPerson. CDM attribute class name: NaturalPersonSurname.\",\"type\":{\"type\":\"strin" +
				"g\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,80}$\"}}]}]}]}]},{\"name\":\"value\",\"" +
				"doc\":\"Textual email addressE.g. jozko.mrkvicka@gmail.com. CDM attribute class na" +
				"me: EmailAddressValue.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pat" +
				"tern\":\"^.{0,255}$\"}}]}}]},{\"name\":\"cc\",\"default\":null,\"type\":[\"null\",{\"type\":\"ar" +
				"ray\",\"items\":\"cz.mpss.api.noby.notification.sendapi.v1.EmailAddress\"}]},{\"name\":" +
				"\"content\",\"type\":{\"type\":\"record\",\"name\":\"Content\",\"doc\":\"Represents any content" +
				". CDM entity name(s): Content.\",\"namespace\":\"cz.mpss.api.noby.notification.senda" +
				"pi.v1\",\"fields\":[{\"name\":\"charset\",\"doc\":\"Charset of content. CDM attribute clas" +
				"s name: ContentCharset. Codebook type: CB_CharacterSet.\",\"default\":null,\"type\":[" +
				"\"null\",{\"type\":\"string\",\"avro.java.string\":\"String\"}]},{\"name\":\"format\",\"doc\":\"F" +
				"ormat of content stored. CDM attribute class name: ContentFormat. Codebook type:" +
				" CB_MIMEType.\",\"default\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\"" +
				":\"String\"}]},{\"name\":\"language\",\"doc\":\"Language of content. Codebook value. CDM " +
				"attribute class name: ContentLanguage. Codebook type: CB_ContentLanguage.\",\"defa" +
				"ult\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\":\"String\"}]},{\"name\"" +
				":\"text\",\"doc\":\"Content in text format. CDM attribute class name: ContentText.\",\"" +
				"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}}]}},{\"name\":\"deliveryConfirm" +
				"ation\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"EmailConfirmation\"" +
				",\"doc\":\"Represents email address. CDM entity name(s): EmailAddress.\",\"namespace\"" +
				":\"cz.mpss.api.noby.notification.sendapi.v1\",\"fields\":[{\"name\":\"value\",\"doc\":\"Tex" +
				"tual email addressE.g. jozko.mrkvicka@gmail.com. CDM attribute class name: Email" +
				"AddressValue.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^." +
				"{0,255}$\"}}]}]},{\"name\":\"id\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"" +
				",\"pattern\":\"^.{0,100}$\"}},{\"name\":\"kBCustomer\",\"default\":null,\"type\":[\"null\",{\"t" +
				"ype\":\"record\",\"name\":\"KBCustomer\",\"doc\":\"Customer is a party role. It is view on" +
				" party from banks perspective and context that party had/has/will have or is eve" +
				"n only related to a product or service from KB.(party has some relationship to b" +
				"ank in context of Customer Management). CDM entity name(s): KBCustomer.\",\"namesp" +
				"ace\":\"cz.mpss.api.noby.notification.sendapi.v1\",\"fields\":[{\"name\":\"id\",\"doc\":\"Un" +
				"ique identifier of the customer.KBI_ID, sometimes also refered to as KBID e.g. i" +
				"n CB_IdentityScheme. CDM attribute class name: KBCustomerId.\",\"type\":{\"type\":\"st" +
				"ring\",\"avro.java.string\":\"String\",\"pattern\":\"[1-9][0-9]*\"}}]}]},{\"name\":\"latestO" +
				"n\",\"doc\":\"Time, before which notification should be sent to recipient . CDM attr" +
				"ibute class name: DeliveryTimingLatestOn.\",\"default\":null,\"type\":[\"null\",{\"type\"" +
				":\"long\",\"logicalType\":\"timestamp-millis\"}]},{\"name\":\"notificationConsumer\",\"type" +
				"\":{\"type\":\"record\",\"name\":\"NotificationConsumer\",\"doc\":\"ConsumerID bude prirazen" +
				"o pri registraci konzumenta na Sender.\",\"namespace\":\"cz.mpss.api.noby.notificati" +
				"on.sendapi.v1\",\"fields\":[{\"name\":\"consumerId\",\"type\":{\"type\":\"string\",\"avro.java" +
				".string\":\"String\"}}]}},{\"name\":\"processingPriority\",\"doc\":\"Priority of notificat" +
				"ion that determines order in which notification are sent.e.g. high or standard. " +
				"CDM attribute class name: NotificationProcessingPriority.\",\"default\":null,\"type\"" +
				":[\"null\",\"int\"]},{\"name\":\"readConfirmation\",\"default\":null,\"type\":[\"null\",\"cz.mp" +
				"ss.api.noby.notification.sendapi.v1.EmailConfirmation\"]},{\"name\":\"replyTo\",\"defa" +
				"ult\":null,\"type\":[\"null\",\"cz.mpss.api.noby.notification.sendapi.v1.EmailAddress\"" +
				"]},{\"name\":\"sender\",\"type\":\"cz.mpss.api.noby.notification.sendapi.v1.EmailAddres" +
				"s\"},{\"name\":\"soonestOn\",\"doc\":\"Time, after which notification should be sent to " +
				"recipient . CDM attribute class name: DeliveryTimingSoonestOn.\",\"default\":null,\"" +
				"type\":[\"null\",{\"type\":\"long\",\"logicalType\":\"timestamp-millis\"}]},{\"name\":\"subjec" +
				"t\",\"doc\":\"Name/title of notification. CDM attribute class name: NotificationSubj" +
				"ect.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,400}$\"" +
				"}},{\"name\":\"to\",\"default\":null,\"type\":[\"null\",{\"type\":\"array\",\"items\":\"cz.mpss.a" +
				"pi.noby.notification.sendapi.v1.EmailAddress\"}]}],\"javaAnnotation\":\"cz.kb.api.co" +
				"mmon.annotation.ConfluentSchemaRegistryCompatible\"}");
		private IList<cz.mpss.api.noby.notification.sendapi.v1.Attachment> _attachments;
		private IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress> _bcc;
		private IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress> _cc;
		private cz.mpss.api.noby.notification.sendapi.v1.Content _content;
		private cz.mpss.api.noby.notification.sendapi.v1.EmailConfirmation _deliveryConfirmation;
		private string _id;
		private cz.mpss.api.noby.notification.sendapi.v1.KBCustomer _kBCustomer;
		/// <summary>
		/// Time, before which notification should be sent to recipient . CDM attribute class name: DeliveryTimingLatestOn.
		/// </summary>
		private System.Nullable<System.DateTime> _latestOn;
		private cz.mpss.api.noby.notification.sendapi.v1.NotificationConsumer _notificationConsumer;
		/// <summary>
		/// Priority of notification that determines order in which notification are sent.e.g. high or standard. CDM attribute class name: NotificationProcessingPriority.
		/// </summary>
		private System.Nullable<System.Int32> _processingPriority;
		private cz.mpss.api.noby.notification.sendapi.v1.EmailConfirmation _readConfirmation;
		private cz.mpss.api.noby.notification.sendapi.v1.EmailAddress _replyTo;
		private cz.mpss.api.noby.notification.sendapi.v1.EmailAddress _sender;
		/// <summary>
		/// Time, after which notification should be sent to recipient . CDM attribute class name: DeliveryTimingSoonestOn.
		/// </summary>
		private System.Nullable<System.DateTime> _soonestOn;
		/// <summary>
		/// Name/title of notification. CDM attribute class name: NotificationSubject.
		/// </summary>
		private string _subject;
		private IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress> _to;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return SendEmail._SCHEMA;
			}
		}
		public IList<cz.mpss.api.noby.notification.sendapi.v1.Attachment> attachments
		{
			get
			{
				return this._attachments;
			}
			set
			{
				this._attachments = value;
			}
		}
		public IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress> bcc
		{
			get
			{
				return this._bcc;
			}
			set
			{
				this._bcc = value;
			}
		}
		public IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress> cc
		{
			get
			{
				return this._cc;
			}
			set
			{
				this._cc = value;
			}
		}
		public cz.mpss.api.noby.notification.sendapi.v1.Content content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}
		public cz.mpss.api.noby.notification.sendapi.v1.EmailConfirmation deliveryConfirmation
		{
			get
			{
				return this._deliveryConfirmation;
			}
			set
			{
				this._deliveryConfirmation = value;
			}
		}
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
		public cz.mpss.api.noby.notification.sendapi.v1.KBCustomer kBCustomer
		{
			get
			{
				return this._kBCustomer;
			}
			set
			{
				this._kBCustomer = value;
			}
		}
		/// <summary>
		/// Time, before which notification should be sent to recipient . CDM attribute class name: DeliveryTimingLatestOn.
		/// </summary>
		public System.Nullable<System.DateTime> latestOn
		{
			get
			{
				return this._latestOn;
			}
			set
			{
				this._latestOn = value;
			}
		}
		public cz.mpss.api.noby.notification.sendapi.v1.NotificationConsumer notificationConsumer
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
		/// <summary>
		/// Priority of notification that determines order in which notification are sent.e.g. high or standard. CDM attribute class name: NotificationProcessingPriority.
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
		public cz.mpss.api.noby.notification.sendapi.v1.EmailConfirmation readConfirmation
		{
			get
			{
				return this._readConfirmation;
			}
			set
			{
				this._readConfirmation = value;
			}
		}
		public cz.mpss.api.noby.notification.sendapi.v1.EmailAddress replyTo
		{
			get
			{
				return this._replyTo;
			}
			set
			{
				this._replyTo = value;
			}
		}
		public cz.mpss.api.noby.notification.sendapi.v1.EmailAddress sender
		{
			get
			{
				return this._sender;
			}
			set
			{
				this._sender = value;
			}
		}
		/// <summary>
		/// Time, after which notification should be sent to recipient . CDM attribute class name: DeliveryTimingSoonestOn.
		/// </summary>
		public System.Nullable<System.DateTime> soonestOn
		{
			get
			{
				return this._soonestOn;
			}
			set
			{
				this._soonestOn = value;
			}
		}
		/// <summary>
		/// Name/title of notification. CDM attribute class name: NotificationSubject.
		/// </summary>
		public string subject
		{
			get
			{
				return this._subject;
			}
			set
			{
				this._subject = value;
			}
		}
		public IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress> to
		{
			get
			{
				return this._to;
			}
			set
			{
				this._to = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.attachments;
			case 1: return this.bcc;
			case 2: return this.cc;
			case 3: return this.content;
			case 4: return this.deliveryConfirmation;
			case 5: return this.id;
			case 6: return this.kBCustomer;
			case 7: return this.latestOn;
			case 8: return this.notificationConsumer;
			case 9: return this.processingPriority;
			case 10: return this.readConfirmation;
			case 11: return this.replyTo;
			case 12: return this.sender;
			case 13: return this.soonestOn;
			case 14: return this.subject;
			case 15: return this.to;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.attachments = (IList<cz.mpss.api.noby.notification.sendapi.v1.Attachment>)fieldValue; break;
			case 1: this.bcc = (IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress>)fieldValue; break;
			case 2: this.cc = (IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress>)fieldValue; break;
			case 3: this.content = (cz.mpss.api.noby.notification.sendapi.v1.Content)fieldValue; break;
			case 4: this.deliveryConfirmation = (cz.mpss.api.noby.notification.sendapi.v1.EmailConfirmation)fieldValue; break;
			case 5: this.id = (System.String)fieldValue; break;
			case 6: this.kBCustomer = (cz.mpss.api.noby.notification.sendapi.v1.KBCustomer)fieldValue; break;
			case 7: this.latestOn = (System.Nullable<System.DateTime>)fieldValue; break;
			case 8: this.notificationConsumer = (cz.mpss.api.noby.notification.sendapi.v1.NotificationConsumer)fieldValue; break;
			case 9: this.processingPriority = (System.Nullable<System.Int32>)fieldValue; break;
			case 10: this.readConfirmation = (cz.mpss.api.noby.notification.sendapi.v1.EmailConfirmation)fieldValue; break;
			case 11: this.replyTo = (cz.mpss.api.noby.notification.sendapi.v1.EmailAddress)fieldValue; break;
			case 12: this.sender = (cz.mpss.api.noby.notification.sendapi.v1.EmailAddress)fieldValue; break;
			case 13: this.soonestOn = (System.Nullable<System.DateTime>)fieldValue; break;
			case 14: this.subject = (System.String)fieldValue; break;
			case 15: this.to = (IList<cz.mpss.api.noby.notification.sendapi.v1.EmailAddress>)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
