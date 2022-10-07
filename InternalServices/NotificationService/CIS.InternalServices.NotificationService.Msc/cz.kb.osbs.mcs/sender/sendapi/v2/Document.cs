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
	/// Dokument z DMSX. Identifikovan je pres dokumentID. Volitelne je mozne specifikovat verzi dokumentu. Pokud verze nebude zadana, vezme se posledni.. ## DokumentDocument is any paper or electronic record or information.Dokument je kazdy pisemny, obrazovy, zvukovy, elektronicky nebo jiny zaznam, at jiz v podobe analogove ci digitalni, ktery vznikl z cinnosti puvodce.. ## Unique identifier of document. Does not change with document version.Multi identifier - there are at least two separate identifiers used for documents across bank.. ## Obsah dokumentu/souborDMS: Rendition. ## Verze dokumentuDocument can change over time. Document version captures document in a given time.Verze dokumentu (SLV 24-001, kapitola 01, bod 38 [1]) je nositelem vlastniho obsahu dokumentu. Kazdy dokument ma alespon jednu verzi. K jednomu dokumentu muze byt N verzi dokumentu. Verze jsou vyuziti pri postupnem vyvoji obsahu jednoho dokumentu (smlouva ve verzi 1, smlouva ve verzi 2 atp.)Reprezentuji postupny vyvoj dokumentu. Dalsi verze plne nahrazuje verzi predchozi, ktera se stava timto neplatnou. CDM entity name(s): Document,DocumentId,DocumentContent,DocumentVersion.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Document : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"Document\",\"doc\":\"Dokument z DMSX. Identifikovan je pres " +
				"dokumentID. Volitelne je mozne specifikovat verzi dokumentu. Pokud verze nebude " +
				"zadana, vezme se posledni.. ## DokumentDocument is any paper or electronic recor" +
				"d or information.Dokument je kazdy pisemny, obrazovy, zvukovy, elektronicky nebo" +
				" jiny zaznam, at jiz v podobe analogove ci digitalni, ktery vznikl z cinnosti pu" +
				"vodce.. ## Unique identifier of document. Does not change with document version." +
				"Multi identifier - there are at least two separate identifiers used for document" +
				"s across bank.. ## Obsah dokumentu/souborDMS: Rendition. ## Verze dokumentuDocum" +
				"ent can change over time. Document version captures document in a given time.Ver" +
				"ze dokumentu (SLV 24-001, kapitola 01, bod 38 [1]) je nositelem vlastniho obsahu" +
				" dokumentu. Kazdy dokument ma alespon jednu verzi. K jednomu dokumentu muze byt " +
				"N verzi dokumentu. Verze jsou vyuziti pri postupnem vyvoji obsahu jednoho dokume" +
				"ntu (smlouva ve verzi 1, smlouva ve verzi 2 atp.)Reprezentuji postupny vyvoj dok" +
				"umentu. Dalsi verze plne nahrazuje verzi predchozi, ktera se stava timto neplatn" +
				"ou. CDM entity name(s): Document,DocumentId,DocumentContent,DocumentVersion.\",\"n" +
				"amespace\":\"cz.kb.osbs.mcs.sender.sendapi.v2\",\"fields\":[{\"name\":\"fileExtension\",\"" +
				"doc\":\"File extension of document content stored.E.g. exe, pdf, docx. CDM attribu" +
				"te class name: DocumentContentFileExtension. Codebook type: CB_FileExtension.\",\"" +
				"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}},{\"name\":\"id\",\"doc\":\"Unique " +
				"identifier of document. Does not change with document version. Multi identifier." +
				"Jednoznacny identifikator dokumentu. DMS: ChronicleID, RootDocID. Pokud je dokum" +
				"ent vkladany do DMS uprostred sveho zivotniho cyklu, je zadouci, aby konzument p" +
				"ri jeho ulozeni vyplnoval docId v pripade, ze vyhovuje formatu = cerpa ze stejne" +
				" rady IGS docId.Escudo:. CDM attribute class name: DocumentIdId.\",\"type\":{\"type\"" +
				":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}},{\"name\":\"version\"," +
				"\"doc\":\"Version of document. Only contains identifier of version, e.g 14Called \'b" +
				"usiness\' version, as this document version must be increased by explicit consume" +
				"r.Ciselne oznaceni verze v kompetenci DMS. Viz INS 22-011. CDM attribute class n" +
				"ame: DocumentVersionVersion.\",\"default\":null,\"type\":[\"null\",\"int\"]}]}");
		/// <summary>
		/// File extension of document content stored.E.g. exe, pdf, docx. CDM attribute class name: DocumentContentFileExtension. Codebook type: CB_FileExtension.
		/// </summary>
		private string _fileExtension;
		/// <summary>
		/// Unique identifier of document. Does not change with document version. Multi identifier.Jednoznacny identifikator dokumentu. DMS: ChronicleID, RootDocID. Pokud je dokument vkladany do DMS uprostred sveho zivotniho cyklu, je zadouci, aby konzument pri jeho ulozeni vyplnoval docId v pripade, ze vyhovuje formatu = cerpa ze stejne rady IGS docId.Escudo:. CDM attribute class name: DocumentIdId.
		/// </summary>
		private string _id;
		/// <summary>
		/// Version of document. Only contains identifier of version, e.g 14Called 'business' version, as this document version must be increased by explicit consumer.Ciselne oznaceni verze v kompetenci DMS. Viz INS 22-011. CDM attribute class name: DocumentVersionVersion.
		/// </summary>
		private System.Nullable<System.Int32> _version;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Document._SCHEMA;
			}
		}
		/// <summary>
		/// File extension of document content stored.E.g. exe, pdf, docx. CDM attribute class name: DocumentContentFileExtension. Codebook type: CB_FileExtension.
		/// </summary>
		public string fileExtension
		{
			get
			{
				return this._fileExtension;
			}
			set
			{
				this._fileExtension = value;
			}
		}
		/// <summary>
		/// Unique identifier of document. Does not change with document version. Multi identifier.Jednoznacny identifikator dokumentu. DMS: ChronicleID, RootDocID. Pokud je dokument vkladany do DMS uprostred sveho zivotniho cyklu, je zadouci, aby konzument pri jeho ulozeni vyplnoval docId v pripade, ze vyhovuje formatu = cerpa ze stejne rady IGS docId.Escudo:. CDM attribute class name: DocumentIdId.
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
		/// <summary>
		/// Version of document. Only contains identifier of version, e.g 14Called 'business' version, as this document version must be increased by explicit consumer.Ciselne oznaceni verze v kompetenci DMS. Viz INS 22-011. CDM attribute class name: DocumentVersionVersion.
		/// </summary>
		public System.Nullable<System.Int32> version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.fileExtension;
			case 1: return this.id;
			case 2: return this.version;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.fileExtension = (System.String)fieldValue; break;
			case 1: this.id = (System.String)fieldValue; break;
			case 2: this.version = (System.Nullable<System.Int32>)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
