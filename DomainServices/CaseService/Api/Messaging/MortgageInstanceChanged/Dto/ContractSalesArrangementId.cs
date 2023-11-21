// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v2.mortgageinstance
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Multiattribute representing sales arrangement unique identifier.Multiattribute because SalesArrangement is very large scope = across all product groups, ids will not be united.Second reason is existence of public id, sales arrangement identifier given to customer and therefore different from internal one. CDM entity name(s): SalesArrangementId.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class ContractSalesArrangementId : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""ContractSalesArrangementId"",""doc"":""Multiattribute representing sales arrangement unique identifier.Multiattribute because SalesArrangement is very large scope = across all product groups, ids will not be united.Second reason is existence of public id, sales arrangement identifier given to customer and therefore different from internal one. CDM entity name(s): SalesArrangementId."",""namespace"":""cz.kb.api.mortgageservicingevents.v2.mortgageinstance"",""fields"":[{""name"":""id"",""doc"":""Multi attribute identifier value itself. CDM attribute class name: SalesArrangementIdId. Attribute has simple type ST_IdString100Default with description: Standard data type to be used as ID, i.e. unique identifier. Longer vevrsion - 100 characters. It is not supposed to be human-readable.E.g.: AgreementID"",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,100}$""}}]}");
		/// <summary>
		/// Multi attribute identifier value itself. CDM attribute class name: SalesArrangementIdId. Attribute has simple type ST_IdString100Default with description: Standard data type to be used as ID, i.e. unique identifier. Longer vevrsion - 100 characters. It is not supposed to be human-readable.E.g.: AgreementID
		/// </summary>
		private string _id;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return ContractSalesArrangementId._SCHEMA;
			}
		}
		/// <summary>
		/// Multi attribute identifier value itself. CDM attribute class name: SalesArrangementIdId. Attribute has simple type ST_IdString100Default with description: Standard data type to be used as ID, i.e. unique identifier. Longer vevrsion - 100 characters. It is not supposed to be human-readable.E.g.: AgreementID
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
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.id;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.id = (System.String)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
