// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.kb.api.mortgageservicingevents.v3.mortgageinstance
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Information entity with various loan/credit balance information on given date.. CDM entity name(s): LoanBalance.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class LoanBalance : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"LoanBalance\",\"doc\":\"Information entity with various loan" +
				"/credit balance information on given date.. CDM entity name(s): LoanBalance.\",\"n" +
				"amespace\":\"cz.kb.api.mortgageservicingevents.v3.mortgageinstance\",\"fields\":[{\"na" +
				"me\":\"amountNotYetWithdrawn\",\"doc\":\"Amount money that was not yet withdrawn from " +
				"this loan. CDM attribute class name: LoanBalanceAmountNotYetWithdrawn. Attribute" +
				" has simple type ST_AmountMoney with description: Castka.Decimal number represen" +
				"ting an amount of money.\",\"default\":null,\"type\":[\"null\",{\"type\":\"bytes\",\"logical" +
				"Type\":\"decimal\",\"precision\":17,\"scale\":2}]},{\"name\":\"amountWithdrawn\",\"doc\":\"Amo" +
				"unt money that was already withdrawn from this loan. CDM attribute class name: L" +
				"oanBalanceAmountWithdrawn. Attribute has simple type ST_AmountMoney with descrip" +
				"tion: Castka.Decimal number representing an amount of money.\",\"default\":null,\"ty" +
				"pe\":[\"null\",{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":17,\"scale\":2}]}," +
				"{\"name\":\"interestAmount\",\"doc\":\"Actual interest amount to be paid. CDM attribute" +
				" class name: LoanBalanceInterestAmount. Attribute has simple type ST_AmountMoney" +
				" with description: Castka.Decimal number representing an amount of money.\",\"defa" +
				"ult\":null,\"type\":[\"null\",{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":17," +
				"\"scale\":2}]},{\"name\":\"lateFee\",\"doc\":\"All non_standard=late yet unpaid fees rela" +
				"ted to loan soldproduct. (additional fees, fees for late repayment of installmen" +
				"t). CDM attribute class name: LoanBalanceLateFee. Attribute has simple type ST_A" +
				"mountMoney with description: Castka.Decimal number representing an amount of mon" +
				"ey.\",\"default\":null,\"type\":[\"null\",{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"prec" +
				"ision\":17,\"scale\":2}]},{\"name\":\"loanFee\",\"doc\":\"All standard yet unpaid fees rel" +
				"ated to this loan soldproduct. CDM attribute class name: LoanBalanceLoanFee. Att" +
				"ribute has simple type ST_AmountMoney with description: Castka.Decimal number re" +
				"presenting an amount of money.\",\"default\":null,\"type\":[\"null\",{\"type\":\"bytes\",\"l" +
				"ogicalType\":\"decimal\",\"precision\":17,\"scale\":2}]},{\"name\":\"overdueInstallmentAmo" +
				"unt\",\"doc\":\"Sum of overdue installments amount.Has to be disputed!!. CDM attribu" +
				"te class name: LoanBalanceOverdueInstallmentAmount. Attribute has simple type ST" +
				"_AmountMoney with description: Castka.Decimal number representing an amount of m" +
				"oney.\",\"default\":null,\"type\":[\"null\",{\"type\":\"bytes\",\"logicalType\":\"decimal\",\"pr" +
				"ecision\":17,\"scale\":2}]},{\"name\":\"principalAmount\",\"doc\":\"Actual (not yet repaid" +
				") principal amount. Relevant for loans. CDM attribute class name: LoanBalancePri" +
				"ncipalAmount. Attribute has simple type ST_AmountMoney with description: Castka." +
				"Decimal number representing an amount of money.\",\"default\":null,\"type\":[\"null\",{" +
				"\"type\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":17,\"scale\":2}]},{\"name\":\"tota" +
				"lAmount\",\"doc\":\"If loan was to be completely repaid at this time, this is total " +
				"amount that would be needed. CDM attribute class name: LoanBalanceTotalAmount. A" +
				"ttribute has simple type ST_AmountMoney with description: Castka.Decimal number " +
				"representing an amount of money.\",\"default\":null,\"type\":[\"null\",{\"type\":\"bytes\"," +
				"\"logicalType\":\"decimal\",\"precision\":17,\"scale\":2}]},{\"name\":\"totalAmountWithoutU" +
				"ncreditedInterest\",\"doc\":\".If loan was to be completely repaid at this time, thi" +
				"s is total amount that would be needed. CDM attribute class name: LoanBalanceTot" +
				"alAmount. Attribute has simple type ST_AmountMoney with description: Castka.Deci" +
				"mal number representing an amount of money.\",\"default\":null,\"type\":[\"null\",{\"typ" +
				"e\":\"bytes\",\"logicalType\":\"decimal\",\"precision\":17,\"scale\":2}]}]}");
		/// <summary>
		/// Amount money that was not yet withdrawn from this loan. CDM attribute class name: LoanBalanceAmountNotYetWithdrawn. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _amountNotYetWithdrawn;
		/// <summary>
		/// Amount money that was already withdrawn from this loan. CDM attribute class name: LoanBalanceAmountWithdrawn. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _amountWithdrawn;
		/// <summary>
		/// Actual interest amount to be paid. CDM attribute class name: LoanBalanceInterestAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _interestAmount;
		/// <summary>
		/// All non_standard=late yet unpaid fees related to loan soldproduct. (additional fees, fees for late repayment of installment). CDM attribute class name: LoanBalanceLateFee. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _lateFee;
		/// <summary>
		/// All standard yet unpaid fees related to this loan soldproduct. CDM attribute class name: LoanBalanceLoanFee. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _loanFee;
		/// <summary>
		/// Sum of overdue installments amount.Has to be disputed!!. CDM attribute class name: LoanBalanceOverdueInstallmentAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _overdueInstallmentAmount;
		/// <summary>
		/// Actual (not yet repaid) principal amount. Relevant for loans. CDM attribute class name: LoanBalancePrincipalAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _principalAmount;
		/// <summary>
		/// If loan was to be completely repaid at this time, this is total amount that would be needed. CDM attribute class name: LoanBalanceTotalAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _totalAmount;
		/// <summary>
		/// .If loan was to be completely repaid at this time, this is total amount that would be needed. CDM attribute class name: LoanBalanceTotalAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		private System.Nullable<Avro.AvroDecimal> _totalAmountWithoutUncreditedInterest;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return LoanBalance._SCHEMA;
			}
		}
		/// <summary>
		/// Amount money that was not yet withdrawn from this loan. CDM attribute class name: LoanBalanceAmountNotYetWithdrawn. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> amountNotYetWithdrawn
		{
			get
			{
				return this._amountNotYetWithdrawn;
			}
			set
			{
				this._amountNotYetWithdrawn = value;
			}
		}
		/// <summary>
		/// Amount money that was already withdrawn from this loan. CDM attribute class name: LoanBalanceAmountWithdrawn. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> amountWithdrawn
		{
			get
			{
				return this._amountWithdrawn;
			}
			set
			{
				this._amountWithdrawn = value;
			}
		}
		/// <summary>
		/// Actual interest amount to be paid. CDM attribute class name: LoanBalanceInterestAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> interestAmount
		{
			get
			{
				return this._interestAmount;
			}
			set
			{
				this._interestAmount = value;
			}
		}
		/// <summary>
		/// All non_standard=late yet unpaid fees related to loan soldproduct. (additional fees, fees for late repayment of installment). CDM attribute class name: LoanBalanceLateFee. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> lateFee
		{
			get
			{
				return this._lateFee;
			}
			set
			{
				this._lateFee = value;
			}
		}
		/// <summary>
		/// All standard yet unpaid fees related to this loan soldproduct. CDM attribute class name: LoanBalanceLoanFee. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> loanFee
		{
			get
			{
				return this._loanFee;
			}
			set
			{
				this._loanFee = value;
			}
		}
		/// <summary>
		/// Sum of overdue installments amount.Has to be disputed!!. CDM attribute class name: LoanBalanceOverdueInstallmentAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> overdueInstallmentAmount
		{
			get
			{
				return this._overdueInstallmentAmount;
			}
			set
			{
				this._overdueInstallmentAmount = value;
			}
		}
		/// <summary>
		/// Actual (not yet repaid) principal amount. Relevant for loans. CDM attribute class name: LoanBalancePrincipalAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> principalAmount
		{
			get
			{
				return this._principalAmount;
			}
			set
			{
				this._principalAmount = value;
			}
		}
		/// <summary>
		/// If loan was to be completely repaid at this time, this is total amount that would be needed. CDM attribute class name: LoanBalanceTotalAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> totalAmount
		{
			get
			{
				return this._totalAmount;
			}
			set
			{
				this._totalAmount = value;
			}
		}
		/// <summary>
		/// .If loan was to be completely repaid at this time, this is total amount that would be needed. CDM attribute class name: LoanBalanceTotalAmount. Attribute has simple type ST_AmountMoney with description: Castka.Decimal number representing an amount of money.
		/// </summary>
		public System.Nullable<Avro.AvroDecimal> totalAmountWithoutUncreditedInterest
		{
			get
			{
				return this._totalAmountWithoutUncreditedInterest;
			}
			set
			{
				this._totalAmountWithoutUncreditedInterest = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.amountNotYetWithdrawn;
			case 1: return this.amountWithdrawn;
			case 2: return this.interestAmount;
			case 3: return this.lateFee;
			case 4: return this.loanFee;
			case 5: return this.overdueInstallmentAmount;
			case 6: return this.principalAmount;
			case 7: return this.totalAmount;
			case 8: return this.totalAmountWithoutUncreditedInterest;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.amountNotYetWithdrawn = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 1: this.amountWithdrawn = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 2: this.interestAmount = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 3: this.lateFee = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 4: this.loanFee = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 5: this.overdueInstallmentAmount = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 6: this.principalAmount = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 7: this.totalAmount = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			case 8: this.totalAmountWithoutUncreditedInterest = (System.Nullable<Avro.AvroDecimal>)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
