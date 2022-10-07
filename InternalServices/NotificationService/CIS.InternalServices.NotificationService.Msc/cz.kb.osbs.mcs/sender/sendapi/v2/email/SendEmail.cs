// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.osbs.mcs.sender.sendapi.v2.email
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Timing when notification is sent to recipient.. ## Activity of sending short (digital) message from bank to a user or party (typically, but not limited to customer). Typically about important:- task events- case changesThis is one notification and will be sent to all recipients through the same defined channels and the same attached documents.E.g. if there is need to send different attachments, then multiple notifications has to be created. CDM entity name(s): DeliveryTiming,Notification.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class SendEmail : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"SendEmail\",\"doc\":\"Timing when notification is sent to re" +
				"cipient.. ## Activity of sending short (digital) message from bank to a user or " +
				"party (typically, but not limited to customer). Typically about important:- task" +
				" events- case changesThis is one notification and will be sent to all recipients" +
				" through the same defined channels and the same attached documents.E.g. if there" +
				" is need to send different attachments, then multiple notifications has to be cr" +
				"eated. CDM entity name(s): DeliveryTiming,Notification.\",\"namespace\":\"cz.kb.osbs" +
				".mcs.sender.sendapi.v2.email\",\"fields\":[{\"name\":\"attachments\",\"default\":null,\"ty" +
				"pe\":[\"null\",{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"Attachment\",\"doc\":\"" +
				" This element is choice - only one of the child fields can be filled.\",\"namespac" +
				"e\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"binary\",\"default\":null," +
				"\"type\":[\"null\",{\"type\":\"record\",\"name\":\"Binary\",\"doc\":\"Verze dokumentuDocument c" +
				"an change over time. Document version captures document in a given time.Verze do" +
				"kumentu (SLV 24-001, kapitola 01, bod 38 [1]) je nositelem vlastniho obsahu doku" +
				"mentu. Kazdy dokument ma alespon jednu verzi. K jednomu dokumentu muze byt N ver" +
				"zi dokumentu. Verze jsou vyuziti pri postupnem vyvoji obsahu jednoho dokumentu (" +
				"smlouva ve verzi 1, smlouva ve verzi 2 atp.)Reprezentuji postupny vyvoj dokument" +
				"u. Dalsi verze plne nahrazuje verzi predchozi, ktera se stava timto neplatnou.. " +
				"## Represents activity of content generation. Used to hold relationships to enti" +
				"ties involved.. ## Represents any content.. ## Version information about content" +
				" template. CDM entity name(s): DocumentVersion,ContentGeneration,Content,Content" +
				"TemplateVersion.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"nam" +
				"e\":\"binary\",\"doc\":\"Content in base64 binary format. CDM attribute class name: Co" +
				"ntentBinary.\",\"type\":\"bytes\"},{\"name\":\"filename\",\"doc\":\"Name of the file in whic" +
				"h the content is stored. As on filesystem. Full file name, suffix is included.E." +
				"g. consumerLoanContract2410.pdfNazev souboru s priponou windows complaint podobe" +
				": https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).asp" +
				"x#naming_conventions. CDM attribute class name: ContentFilename.\",\"type\":{\"type\"" +
				":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}}]}]},{\"name\":\"docu" +
				"ment\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"Document\",\"doc\":\"Do" +
				"kument z DMSX. Identifikovan je pres dokumentID. Volitelne je mozne specifikovat" +
				" verzi dokumentu. Pokud verze nebude zadana, vezme se posledni.. ## DokumentDocu" +
				"ment is any paper or electronic record or information.Dokument je kazdy pisemny," +
				" obrazovy, zvukovy, elektronicky nebo jiny zaznam, at jiz v podobe analogove ci " +
				"digitalni, ktery vznikl z cinnosti puvodce.. ## Unique identifier of document. D" +
				"oes not change with document version.Multi identifier - there are at least two s" +
				"eparate identifiers used for documents across bank.. ## Obsah dokumentu/souborDM" +
				"S: Rendition. ## Verze dokumentuDocument can change over time. Document version " +
				"captures document in a given time.Verze dokumentu (SLV 24-001, kapitola 01, bod " +
				"38 [1]) je nositelem vlastniho obsahu dokumentu. Kazdy dokument ma alespon jednu" +
				" verzi. K jednomu dokumentu muze byt N verzi dokumentu. Verze jsou vyuziti pri p" +
				"ostupnem vyvoji obsahu jednoho dokumentu (smlouva ve verzi 1, smlouva ve verzi 2" +
				" atp.)Reprezentuji postupny vyvoj dokumentu. Dalsi verze plne nahrazuje verzi pr" +
				"edchozi, ktera se stava timto neplatnou. CDM entity name(s): Document,DocumentId" +
				",DocumentContent,DocumentVersion.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2" +
				"\",\"fields\":[{\"name\":\"fileExtension\",\"doc\":\"File extension of document content st" +
				"ored.E.g. exe, pdf, docx. CDM attribute class name: DocumentContentFileExtension" +
				". Codebook type: CB_FileExtension.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"" +
				"String\"}},{\"name\":\"id\",\"doc\":\"Unique identifier of document. Does not change wit" +
				"h document version. Multi identifier.Jednoznacny identifikator dokumentu. DMS: C" +
				"hronicleID, RootDocID. Pokud je dokument vkladany do DMS uprostred sveho zivotni" +
				"ho cyklu, je zadouci, aby konzument pri jeho ulozeni vyplnoval docId v pripade, " +
				"ze vyhovuje formatu = cerpa ze stejne rady IGS docId.Escudo:. CDM attribute clas" +
				"s name: DocumentIdId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"patt" +
				"ern\":\"^.{0,30}$\"}},{\"name\":\"version\",\"doc\":\"Version of document. Only contains i" +
				"dentifier of version, e.g 14Called \'business\' version, as this document version " +
				"must be increased by explicit consumer.Ciselne oznaceni verze v kompetenci DMS. " +
				"Viz INS 22-011. CDM attribute class name: DocumentVersionVersion.\",\"default\":nul" +
				"l,\"type\":[\"null\",\"int\"]}]}]},{\"name\":\"s3Content\",\"default\":null,\"type\":[\"null\",{" +
				"\"type\":\"record\",\"name\":\"S3Content\",\"doc\":\"Represents any content.. ## Relation b" +
				"etwwe bucket and content - describes, how contents are stored/managed on buckets" +
				". Every content managed on bucket has its unique objectKey. CDM entity name(s): " +
				"Content,ContentInBucket.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields" +
				"\":[{\"name\":\"filename\",\"doc\":\"Name of the file in which the content is stored. As" +
				" on filesystem. Full file name, suffix is included.E.g. consumerLoanContract2410" +
				".pdfNazev souboru s priponou windows complaint podobe: https://msdn.microsoft.co" +
				"m/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#naming_conventions. CDM a" +
				"ttribute class name: ContentFilename.\",\"type\":{\"type\":\"string\",\"avro.java.string" +
				"\":\"String\",\"pattern\":\"^.{0,255}$\"}},{\"name\":\"objectKey\",\"doc\":\" CDM attribute cl" +
				"ass name: ContentInBucketObjectKey.\",\"type\":{\"type\":\"string\",\"avro.java.string\":" +
				"\"String\",\"pattern\":\"^.{0,100}$\"}}]}]}]}}]},{\"name\":\"bcc\",\"default\":null,\"type\":[" +
				"\"null\",{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"EmailAddress\",\"doc\":\"Rep" +
				"resents email address.. ## \'PartyInBankRole\'Party role is description of relatio" +
				"nship of (any view on) party to KB or KBGroup. I.e. has information about any vi" +
				"ew on party in context of KB (or other KB group entity) in SPECIFIC CONTEXT/role" +
				".. ## Email as a contact on party. CDM entity name(s): EmailAddress,PartyRole,Em" +
				"ailContact.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"p" +
				"arty\",\"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"Party\",\"doc\":\" This" +
				" element is choice - only one of the child fields can be filled.\",\"namespace\":\"c" +
				"z.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"legalPerson\",\"default\":null," +
				"\"type\":[\"null\",{\"type\":\"record\",\"name\":\"LegalPerson\",\"doc\":\"Juridical person tha" +
				"t is NOT <b>fixed</b> to <b>exactly one </b>natural person. CDM entity name(s): " +
				"LegalPerson.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"" +
				"name\",\"doc\":\"Official name of juridical person. CDM attribute class name: Juridi" +
				"calPersonName.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^" +
				".{0,255}$\"}}]}]},{\"name\":\"naturalPerson\",\"default\":null,\"type\":[\"null\",{\"type\":\"" +
				"record\",\"name\":\"NaturalPerson\",\"doc\":\"Flesh and blood party - human being.Fyzick" +
				"a osobaE.g. Jozko Mrkvicka. CDM entity name(s): NaturalPerson.\",\"namespace\":\"cz." +
				"kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"firstName\",\"doc\":\"First name o" +
				"f the NaturalPerson. CDM attribute class name: NaturalPersonFirstName.\",\"type\":{" +
				"\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,40}$\"}},{\"name\":\"mid" +
				"dleName\",\"doc\":\"Middle name of the natural person. CDM attribute class name: Nat" +
				"uralPersonMiddleName.\",\"default\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java" +
				".string\":\"String\",\"pattern\":\"^.{0,40}$\"}]},{\"name\":\"surname\",\"doc\":\"Surname of t" +
				"he NaturalPerson. CDM attribute class name: NaturalPersonSurname.\",\"type\":{\"type" +
				"\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,80}$\"}}]}]}]}]},{\"name\":\"" +
				"value\",\"doc\":\"Textual email addressE.g. jozko.mrkvicka@gmail.com. CDM attribute " +
				"class name: EmailAddressValue.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"Stri" +
				"ng\",\"pattern\":\"^.{0,255}$\"}}]}}]},{\"name\":\"cc\",\"default\":null,\"type\":[\"null\",{\"t" +
				"ype\":\"array\",\"items\":\"cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress\"}]},{\"name\":" +
				"\"content\",\"type\":{\"type\":\"record\",\"name\":\"Content\",\"doc\":\"Represents any content" +
				". CDM entity name(s): Content.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"" +
				"fields\":[{\"name\":\"charset\",\"doc\":\"Charset of content. CDM attribute class name: " +
				"ContentCharset. Codebook type: CB_CharacterSet.\",\"default\":null,\"type\":[\"null\",{" +
				"\"type\":\"string\",\"avro.java.string\":\"String\"}]},{\"name\":\"format\",\"doc\":\"Format of" +
				" content stored. CDM attribute class name: ContentFormat. Codebook type: CB_MIME" +
				"Type.\",\"default\":null,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\":\"String" +
				"\"}]},{\"name\":\"language\",\"doc\":\"Language of content. Codebook value. CDM attribut" +
				"e class name: ContentLanguage. Codebook type: CB_ContentLanguage.\",\"default\":nul" +
				"l,\"type\":[\"null\",{\"type\":\"string\",\"avro.java.string\":\"String\"}]},{\"name\":\"text\"," +
				"\"doc\":\"Content in text format. CDM attribute class name: ContentText.\",\"type\":{\"" +
				"type\":\"string\",\"avro.java.string\":\"String\"}}]}},{\"name\":\"id\",\"type\":{\"type\":\"str" +
				"ing\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,100}$\"}},{\"name\":\"kBCustomer\",\"" +
				"default\":null,\"type\":[\"null\",{\"type\":\"record\",\"name\":\"KBCustomer\",\"doc\":\"Custome" +
				"r is a party role. It is view on party from banks perspective and context that p" +
				"arty had/has/will have or is even only related to a product or service from KB.(" +
				"party has some relationship to bank in context of Customer Management). CDM enti" +
				"ty name(s): KBCustomer.\",\"namespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\"" +
				":[{\"name\":\"id\",\"doc\":\"Unique identifier of the customer.KBI_ID, sometimes also r" +
				"efered to as KBID e.g. in CB_IdentityScheme. CDM attribute class name: KBCustome" +
				"rId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"[1-9][0-9]*" +
				"\"}}]}]},{\"name\":\"latestOn\",\"doc\":\"Time, before which notification should be sent" +
				" to recipient . CDM attribute class name: DeliveryTimingLatestOn.\",\"default\":nul" +
				"l,\"type\":[\"null\",{\"type\":\"long\",\"logicalType\":\"local-timestamp-millis\"}]},{\"name" +
				"\":\"notificationConsumer\",\"type\":{\"type\":\"record\",\"name\":\"NotificationConsumer\",\"" +
				"doc\":\"ConsumerID bude prirazeno pri registraci konzumenta na Sender.\",\"namespace" +
				"\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"consumerId\",\"type\":{\"typ" +
				"e\":\"string\",\"avro.java.string\":\"String\"}}]}},{\"name\":\"processingPriority\",\"doc\":" +
				"\"Priority of notification that determines order in which notification are sent.e" +
				".g. high or standard. CDM attribute class name: NotificationProcessingPriority.\"" +
				",\"default\":null,\"type\":[\"null\",\"int\"]},{\"name\":\"replyTo\",\"default\":null,\"type\":[" +
				"\"null\",\"cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress\"]},{\"name\":\"sender\",\"type\"" +
				":\"cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress\"},{\"name\":\"soonestOn\",\"doc\":\"Tim" +
				"e, after which notification should be sent to recipient . CDM attribute class na" +
				"me: DeliveryTimingSoonestOn.\",\"default\":null,\"type\":[\"null\",{\"type\":\"long\",\"logi" +
				"calType\":\"local-timestamp-millis\"}]},{\"name\":\"subject\",\"doc\":\"Name/title of noti" +
				"fication. CDM attribute class name: NotificationSubject.\",\"type\":{\"type\":\"string" +
				"\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,400}$\"}},{\"name\":\"to\",\"default\":nu" +
				"ll,\"type\":[\"null\",{\"type\":\"array\",\"items\":\"cz.kb.osbs.mcs.sender.sendapi.v2.Emai" +
				"lAddress\"}]}]}");
		private IList<cz.kb.osbs.mcs.sender.sendapi.v2.Attachment> _attachments;
		private IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress> _bcc;
		private IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress> _cc;
		private cz.kb.osbs.mcs.sender.sendapi.v2.Content _content;
		private string _id;
		private cz.kb.osbs.mcs.sender.sendapi.v2.KBCustomer _kBCustomer;
		/// <summary>
		/// Time, before which notification should be sent to recipient . CDM attribute class name: DeliveryTimingLatestOn.
		/// </summary>
		private System.Nullable<System.DateTime> _latestOn;
		private cz.kb.osbs.mcs.sender.sendapi.v2.NotificationConsumer _notificationConsumer;
		/// <summary>
		/// Priority of notification that determines order in which notification are sent.e.g. high or standard. CDM attribute class name: NotificationProcessingPriority.
		/// </summary>
		private System.Nullable<System.Int32> _processingPriority;
		private cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress _replyTo;
		private cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress _sender;
		/// <summary>
		/// Time, after which notification should be sent to recipient . CDM attribute class name: DeliveryTimingSoonestOn.
		/// </summary>
		private System.Nullable<System.DateTime> _soonestOn;
		/// <summary>
		/// Name/title of notification. CDM attribute class name: NotificationSubject.
		/// </summary>
		private string _subject;
		private IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress> _to;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return SendEmail._SCHEMA;
			}
		}
		public IList<cz.kb.osbs.mcs.sender.sendapi.v2.Attachment> attachments
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
		public IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress> bcc
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
		public IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress> cc
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
		public cz.kb.osbs.mcs.sender.sendapi.v2.Content content
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
		public cz.kb.osbs.mcs.sender.sendapi.v2.KBCustomer kBCustomer
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
		public cz.kb.osbs.mcs.sender.sendapi.v2.NotificationConsumer notificationConsumer
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
		public cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress replyTo
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
		public cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress sender
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
		public IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress> to
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
			case 4: return this.id;
			case 5: return this.kBCustomer;
			case 6: return this.latestOn;
			case 7: return this.notificationConsumer;
			case 8: return this.processingPriority;
			case 9: return this.replyTo;
			case 10: return this.sender;
			case 11: return this.soonestOn;
			case 12: return this.subject;
			case 13: return this.to;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.attachments = (IList<cz.kb.osbs.mcs.sender.sendapi.v2.Attachment>)fieldValue; break;
			case 1: this.bcc = (IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress>)fieldValue; break;
			case 2: this.cc = (IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress>)fieldValue; break;
			case 3: this.content = (cz.kb.osbs.mcs.sender.sendapi.v2.Content)fieldValue; break;
			case 4: this.id = (System.String)fieldValue; break;
			case 5: this.kBCustomer = (cz.kb.osbs.mcs.sender.sendapi.v2.KBCustomer)fieldValue; break;
			case 6: this.latestOn = (System.Nullable<System.DateTime>)fieldValue; break;
			case 7: this.notificationConsumer = (cz.kb.osbs.mcs.sender.sendapi.v2.NotificationConsumer)fieldValue; break;
			case 8: this.processingPriority = (System.Nullable<System.Int32>)fieldValue; break;
			case 9: this.replyTo = (cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress)fieldValue; break;
			case 10: this.sender = (cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress)fieldValue; break;
			case 11: this.soonestOn = (System.Nullable<System.DateTime>)fieldValue; break;
			case 12: this.subject = (System.String)fieldValue; break;
			case 13: this.to = (IList<cz.kb.osbs.mcs.sender.sendapi.v2.EmailAddress>)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
