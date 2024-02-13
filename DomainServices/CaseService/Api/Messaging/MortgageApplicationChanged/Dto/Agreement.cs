// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v2.mortgageapplication
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// 'Obchod'Agreement is performing sale to customer. Agreement contains product, service instances, product offer and context required. Agreement is only present during sale action (selling, modification or closing  of product or service instances. There is no agreement in presales..#CZ#Obchod zprostredkovava prodej zakaznikovi. Obchod obsahuje produkt, instanci sluzby, nabidku produktu a pozadovany kontext. O obchodu je mozne mluvit pouze v pripade akce predeje (prodej, zmena nebo uzavreni produktu, potazmo instance sluzby). O obchodu se neda mluvit v pripade predprodeje. CDM entity name(s): Agreement.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Agreement : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"Agreement\",\"doc\":\"\'Obchod\'Agreement is performing sale t" +
				"o customer. Agreement contains product, service instances, product offer and con" +
				"text required. Agreement is only present during sale action (selling, modificati" +
				"on or closing  of product or service instances. There is no agreement in presale" +
				"s..#CZ#Obchod zprostredkovava prodej zakaznikovi. Obchod obsahuje produkt, insta" +
				"nci sluzby, nabidku produktu a pozadovany kontext. O obchodu je mozne mluvit pou" +
				"ze v pripade akce predeje (prodej, zmena nebo uzavreni produktu, potazmo instanc" +
				"e sluzby). O obchodu se neda mluvit v pripade predprodeje. CDM entity name(s): A" +
				"greement.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v2.mortgageapplication" +
				"\",\"fields\":[{\"name\":\"agreementApproval\",\"default\":null,\"type\":[\"null\",{\"type\":\"r" +
				"ecord\",\"name\":\"Approval\",\"doc\":\"General entity representing approval process E.g" +
				". agreement or exception approval. CDM entity name(s): Approval.\",\"namespace\":\"c" +
				"z.kb.api.mortgageservicingevents.v2.mortgageapplication\",\"fields\":[{\"name\":\"on\"," +
				"\"doc\":\"Time when overall approval was made. CDM attribute class name: ApprovalOn" +
				". Standard avro dateTime represented by the number of milliseconds from the unix" +
				" epoch, see avro specification: https://avro.apache.org/docs/1.8.0/spec.html. Co" +
				"nsider using converters to dateTime class in code generator (see KB Integration " +
				"Platform wiki for schema registry chapter).\",\"default\":null,\"type\":[\"null\",{\"typ" +
				"e\":\"long\",\"logicalType\":\"timestamp-millis\"}]},{\"name\":\"requestedOn\",\"doc\":\"Time " +
				"when overall approval was requested. CDM attribute class name: ApprovalRequested" +
				"On. Standard avro dateTime represented by the number of milliseconds from the un" +
				"ix epoch, see avro specification: https://avro.apache.org/docs/1.8.0/spec.html. " +
				"Consider using converters to dateTime class in code generator (see KB Integratio" +
				"n Platform wiki for schema registry chapter).\",\"type\":{\"type\":\"long\",\"logicalTyp" +
				"e\":\"timestamp-millis\"}}]}]}]}");
		private cz.kb.api.mortgageservicingevents.v2.mortgageapplication.Approval _agreementApproval;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Agreement._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v2.mortgageapplication.Approval agreementApproval
		{
			get
			{
				return this._agreementApproval;
			}
			set
			{
				this._agreementApproval = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.agreementApproval;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.agreementApproval = (cz.kb.api.mortgageservicingevents.v2.mortgageapplication.Approval)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
