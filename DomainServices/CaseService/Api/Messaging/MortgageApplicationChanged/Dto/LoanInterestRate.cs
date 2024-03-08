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
	/// Specific interest rate of proposed or sold loan business product. This is only THE (primary) interest rate. This is NOT any other interest rate on product. CDM entity name(s): LoanInterestRate.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class LoanInterestRate : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""LoanInterestRate"",""doc"":""Specific interest rate of proposed or sold loan business product. This is only THE (primary) interest rate. This is NOT any other interest rate on product. CDM entity name(s): LoanInterestRate."",""namespace"":""cz.kb.api.mortgageservicingevents.v3.mortgageapplication"",""fields"":[{""name"":""fixedRatePeriod"",""default"":null,""type"":[""null"",{""type"":""record"",""name"":""FixedRatePeriod"",""doc"":""Time that interest rate is fixed and can not change. CDM entity name(s): FixedRatePeriod."",""namespace"":""cz.kb.api.mortgageservicingevents.v3.mortgageapplication"",""fields"":[{""name"":""period"",""doc"":""Total length of period in units. If no units are given, default is month. CDM attribute class name: FixedRatePeriodPeriod. Attribute has simple type ST_PositiveInt with description: Integer with a constraint. The value must be a positive number, i.e. zero is excluded."",""type"":""int""}]}]},{""name"":""value"",""doc"":""Resulting interest rate, i.e. what customer sees.Urokova mira uveru.(High level) vbs + srn + margin. CDM attribute class name: LoanInterestRateValue. Attribute has simple type ST_InterestRate with description: Urok.Represents an interest rate or its part, (Interest rate, SRN, Margin).E. g. 1.0 (=100%), 0.25 (=25%)Limited at 100000%"",""type"":{""type"":""bytes"",""logicalType"":""decimal"",""precision"":11,""scale"":8}}]}");
		private cz.kb.api.mortgageservicingevents.v3.mortgageapplication.FixedRatePeriod _fixedRatePeriod;
		/// <summary>
		/// Resulting interest rate, i.e. what customer sees.Urokova mira uveru.(High level) vbs + srn + margin. CDM attribute class name: LoanInterestRateValue. Attribute has simple type ST_InterestRate with description: Urok.Represents an interest rate or its part, (Interest rate, SRN, Margin).E. g. 1.0 (=100%), 0.25 (=25%)Limited at 100000%
		/// </summary>
		private Avro.AvroDecimal _value;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return LoanInterestRate._SCHEMA;
			}
		}
		public cz.kb.api.mortgageservicingevents.v3.mortgageapplication.FixedRatePeriod fixedRatePeriod
		{
			get
			{
				return this._fixedRatePeriod;
			}
			set
			{
				this._fixedRatePeriod = value;
			}
		}
		/// <summary>
		/// Resulting interest rate, i.e. what customer sees.Urokova mira uveru.(High level) vbs + srn + margin. CDM attribute class name: LoanInterestRateValue. Attribute has simple type ST_InterestRate with description: Urok.Represents an interest rate or its part, (Interest rate, SRN, Margin).E. g. 1.0 (=100%), 0.25 (=25%)Limited at 100000%
		/// </summary>
		public Avro.AvroDecimal value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.fixedRatePeriod;
			case 1: return this.value;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.fixedRatePeriod = (cz.kb.api.mortgageservicingevents.v3.mortgageapplication.FixedRatePeriod)fieldValue; break;
			case 1: this.value = (Avro.AvroDecimal)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
