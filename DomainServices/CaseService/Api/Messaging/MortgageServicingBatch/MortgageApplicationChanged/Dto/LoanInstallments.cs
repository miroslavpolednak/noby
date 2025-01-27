// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v3.mortgageapplication
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// One installment amount. It is information entity. It can be used for any installment. CDM entity name(s): LoanInstallments.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class LoanInstallments : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"LoanInstallments\",\"doc\":\"One installment amount. It is i" +
				"nformation entity. It can be used for any installment. CDM entity name(s): LoanI" +
				"nstallments.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplicat" +
				"ion\",\"fields\":[{\"name\":\"dayInMonth\",\"doc\":\"Day within month when installment is " +
				"due to be paid. CDM attribute class name: LoanInstallmentsDayInMonth. Codebook t" +
				"ype: CB_DayOfMonth.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}},{\"nam" +
				"e\":\"firstAnnuityLoanIndividualInstallment\",\"default\":null,\"type\":[\"null\",{\"type\"" +
				":\"record\",\"name\":\"FirstAnnuityLoanIndividualInstallment\",\"doc\":\"Individual/concr" +
				"ete repayment of loan - one loan installment. CDM entity name(s): LoanIndividual" +
				"Installment.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageapplicat" +
				"ion\",\"fields\":[{\"name\":\"on\",\"doc\":\"Date of installment to be repaid/was repaid. " +
				"CDM attribute class name: LoanIndividualInstallmentOn. Standard avro date repres" +
				"ented by the number of days from the unix epoch, see avro specification: https:/" +
				"/avro.apache.org/docs/1.8.0/spec.html. Consider using converters to date class i" +
				"n code generator (see KB Integration Platform wiki for schema registry chapter)." +
				"\",\"type\":{\"type\":\"int\",\"logicalType\":\"date\"}}]}]},{\"name\":\"loanIndividualInstall" +
				"ment\",\"type\":{\"type\":\"record\",\"name\":\"LoanIndividualInstallment\",\"doc\":\"Individu" +
				"al/concrete repayment of loan - one loan installment. CDM entity name(s): LoanIn" +
				"dividualInstallment.\",\"namespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgage" +
				"application\",\"fields\":[{\"name\":\"amount\",\"doc\":\"Total amount that has to paid as " +
				"one loan installment. CDM attribute class name: LoanIndividualInstallmentAmount." +
				" Attribute has simple type ST_AmountMoney with description: Castka.Decimal numbe" +
				"r representing an amount of money.\",\"type\":{\"type\":\"bytes\",\"logicalType\":\"decima" +
				"l\",\"precision\":17,\"scale\":2}}]}}]}");
		/// <summary>
		/// Day within month when installment is due to be paid. CDM attribute class name: LoanInstallmentsDayInMonth. Codebook type: CB_DayOfMonth.
		/// </summary>
		private string _dayInMonth;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.FirstAnnuityLoanIndividualInstallment _firstAnnuityLoanIndividualInstallment;
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanIndividualInstallment _loanIndividualInstallment;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return LoanInstallments._SCHEMA;
			}
		}
		/// <summary>
		/// Day within month when installment is due to be paid. CDM attribute class name: LoanInstallmentsDayInMonth. Codebook type: CB_DayOfMonth.
		/// </summary>
		public string dayInMonth
		{
			get
			{
				return this._dayInMonth;
			}
			set
			{
				this._dayInMonth = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.FirstAnnuityLoanIndividualInstallment firstAnnuityLoanIndividualInstallment
		{
			get
			{
				return this._firstAnnuityLoanIndividualInstallment;
			}
			set
			{
				this._firstAnnuityLoanIndividualInstallment = value;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanIndividualInstallment loanIndividualInstallment
		{
			get
			{
				return this._loanIndividualInstallment;
			}
			set
			{
				this._loanIndividualInstallment = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.dayInMonth;
			case 1: return this.firstAnnuityLoanIndividualInstallment;
			case 2: return this.loanIndividualInstallment;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.dayInMonth = (System.String)fieldValue; break;
			case 1: this.firstAnnuityLoanIndividualInstallment = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.FirstAnnuityLoanIndividualInstallment)fieldValue; break;
			case 2: this.loanIndividualInstallment = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.LoanIndividualInstallment)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
