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
	/// Vyska uveruInformation about loan amount. This is static information. Change only with sale or maintenance action. CDM entity name(s): LoanAmount.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class LoanAmount : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""LoanAmount"",""doc"":""Vyska uveruInformation about loan amount. This is static information. Change only with sale or maintenance action. CDM entity name(s): LoanAmount."",""namespace"":""cz.kb.api.mortgageservicingevents.v3.mortgageapplication"",""fields"":[{""name"":""limit"",""doc"":""Vyska uveru, ktora bola schvalena. (Pozor, nie je nutne suma, ktoru si klient skutocne poziacia)Allowed loan limit. It is maximum amount that customer can borrow from bank.E.g. credit card limit, overdraft limit, or contracted consumer loan amount. CDM attribute class name: LoanAmountLimit. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money."",""type"":{""type"":""bytes"",""logicalType"":""decimal"",""precision"":17,""scale"":2}}]}");
		/// <summary>
		/// Vyska uveru, ktora bola schvalena. (Pozor, nie je nutne suma, ktoru si klient skutocne poziacia)Allowed loan limit. It is maximum amount that customer can borrow from bank.E.g. credit card limit, overdraft limit, or contracted consumer loan amount. CDM attribute class name: LoanAmountLimit. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private Avro.AvroDecimal _limit;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return LoanAmount._SCHEMA;
			}
		}
		/// <summary>
		/// Vyska uveru, ktora bola schvalena. (Pozor, nie je nutne suma, ktoru si klient skutocne poziacia)Allowed loan limit. It is maximum amount that customer can borrow from bank.E.g. credit card limit, overdraft limit, or contracted consumer loan amount. CDM attribute class name: LoanAmountLimit. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public Avro.AvroDecimal limit
		{
			get
			{
				return this._limit;
			}
			set
			{
				this._limit = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.limit;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.limit = (Avro.AvroDecimal)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
