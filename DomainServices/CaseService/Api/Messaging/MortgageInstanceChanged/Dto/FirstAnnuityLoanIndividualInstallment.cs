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
	/// Individual/concrete repayment of loan - one loan installment. CDM entity name(s): LoanIndividualInstallment.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class FirstAnnuityLoanIndividualInstallment : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""FirstAnnuityLoanIndividualInstallment"",""doc"":""Individual/concrete repayment of loan - one loan installment. CDM entity name(s): LoanIndividualInstallment."",""namespace"":""cz.kb.api.mortgageservicingevents.v2.mortgageinstance"",""fields"":[{""name"":""on"",""doc"":""Date of installment to be repaid/was repaid. CDM attribute class name: LoanIndividualInstallmentOn. Standard avro date represented by the number of days from the unix epoch, see avro specification: https://avro.apache.org/docs/1.8.0/spec.html. Consider using converters to date class in code generator (see KB Integration Platform wiki for schema registry chapter)."",""type"":{""type"":""int"",""logicalType"":""date""}}]}");
		/// <summary>
		/// Date of installment to be repaid/was repaid. CDM attribute class name: LoanIndividualInstallmentOn. Standard avro date represented by the number of days from the unix epoch, see avro specification: https://avro.apache.org/docs/1.8.0/spec.html. Consider using converters to date class in code generator (see KB Integration Platform wiki for schema registry chapter).
		/// </summary>
		private System.DateTime _on;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return FirstAnnuityLoanIndividualInstallment._SCHEMA;
			}
		}
		/// <summary>
		/// Date of installment to be repaid/was repaid. CDM attribute class name: LoanIndividualInstallmentOn. Standard avro date represented by the number of days from the unix epoch, see avro specification: https://avro.apache.org/docs/1.8.0/spec.html. Consider using converters to date class in code generator (see KB Integration Platform wiki for schema registry chapter).
		/// </summary>
		public System.DateTime on
		{
			get
			{
				return this._on;
			}
			set
			{
				this._on = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.on;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.on = (System.DateTime)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
