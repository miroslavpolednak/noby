// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Obchodni pripadCase is group of activities or record about them that is from business perspective perceived as of enough importance to be recorded, monitored a informed about.Can be any bank's agenda.E.g. fufilling client's request, sale of one productExamples:- consumer loan origination- loan drawing- administrative action on the product- recovery- complaint handlingCase has some attributes, that are common for any case category and are mandatory. Additionally, specific case categories can have additional data.In wmt it is called business request (there is child entity for it).. ## Case that encompasses process(es) related to loan risk assessmentC4M: Business Case. CDM entity name(s): Case,LoanRiskCase.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class Case : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"Case\",\"doc\":\"Obchodni pripadCase is group of activities " +
				"or record about them that is from business perspective perceived as of enough im" +
				"portance to be recorded, monitored a informed about.Can be any bank\'s agenda.E.g" +
				". fufilling client\'s request, sale of one productExamples:- consumer loan origin" +
				"ation- loan drawing- administrative action on the product- recovery- complaint h" +
				"andlingCase has some attributes, that are common for any case category and are m" +
				"andatory. Additionally, specific case categories can have additional data.In wmt" +
				" it is called business request (there is child entity for it).. ## Case that enc" +
				"ompasses process(es) related to loan risk assessmentC4M: Business Case. CDM enti" +
				"ty name(s): Case,LoanRiskCase.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkf" +
				"low.mortgageprocessevents.v1.loanextrapaymentprocesschanged\",\"fields\":[{\"name\":\"" +
				"caseId\",\"type\":{\"type\":\"record\",\"name\":\"CaseId\",\"doc\":\"Multiattribute representi" +
				"ng unique identifier of case.It is multiattribute because there is multiple case" +
				" management solutions running now. CDM entity name(s): CaseId.\",\"namespace\":\"cz." +
				"mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentpro" +
				"cesschanged\",\"fields\":[{\"name\":\"id\",\"doc\":\"Unique identifier of case id itself. " +
				"CDM attribute class name: CaseIdId. Attribute has simple type ST_String36 with d" +
				"escription: General string of max length 36.\",\"type\":{\"type\":\"string\",\"avro.java" +
				".string\":\"String\",\"pattern\":\"^.{0,36}$\"}}]}},{\"name\":\"loanRiskCase\",\"type\":{\"typ" +
				"e\":\"record\",\"name\":\"RiskBusinessCaseId\",\"namespace\":\"cz.mpss.api.starbuild.mortg" +
				"ageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged\",\"fields\":[{" +
				"\"name\":\"id\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"}}]}},{\"name\":\"mo" +
				"rtgageInstance\",\"type\":{\"type\":\"record\",\"name\":\"MortgageInstance\",\"doc\":\"Mortgag" +
				"e product instance. CDM entity name(s): MortgageInstance.\",\"namespace\":\"cz.mpss." +
				"api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocessc" +
				"hanged\",\"fields\":[{\"name\":\"Starbuild\",\"type\":{\"type\":\"record\",\"name\":\"StarbuildI" +
				"nstanceId\",\"doc\":\"Multiattribute property of marketable item instance.Multiattri" +
				"bute: Identifiers of marketable item instances are not yet unified, there is no " +
				"one central catalogue.This also covers identifiers valid for only some part of m" +
				"arketable item instance lifecycle, e.g. proposed product.E.g. PCP identifiers, T" +
				"SS identifiers.#CZ#Viceatributova promenna instanci prodejnych polozek. Viceatri" +
				"butovost: Identifikatory instanci prodejych polozek zatim nejsou sjednoceny. Nee" +
				"xistujce centralni katalog. Pokryva tez identifikatory, ktere jsou platne pouze " +
				"pro nektere casti zivotniho cyklu instanci prodejnych polozek (napriklad navrhov" +
				"any produkt). Priklad: PCP identifikator, TSS identifikator. CDM entity name(s):" +
				" InstanceId.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageproces" +
				"sevents.v1.loanextrapaymentprocesschanged\",\"fields\":[{\"name\":\"id\",\"doc\":\"Identif" +
				"ier of product instance item itself.#CZ#Vlastni identifikator instance prodejne " +
				"polozky. CDM attribute class name: InstanceIdId. Attribute has simple type ST_Id" +
				"StringDefault with description: Standard data type to be used ID, i.e. unique id" +
				"entifier. It is not supposed to be human-readable.E.g.: AgreementID\",\"type\":{\"ty" +
				"pe\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}}]}}]}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.CaseId _caseId;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.RiskBusinessCaseId _loanRiskCase;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.MortgageInstance _mortgageInstance;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Case._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.CaseId caseId
		{
			get
			{
				return this._caseId;
			}
			set
			{
				this._caseId = value;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.RiskBusinessCaseId loanRiskCase
		{
			get
			{
				return this._loanRiskCase;
			}
			set
			{
				this._loanRiskCase = value;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.MortgageInstance mortgageInstance
		{
			get
			{
				return this._mortgageInstance;
			}
			set
			{
				this._mortgageInstance = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.caseId;
			case 1: return this.loanRiskCase;
			case 2: return this.mortgageInstance;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.caseId = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.CaseId)fieldValue; break;
			case 1: this.loanRiskCase = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.RiskBusinessCaseId)fieldValue; break;
			case 2: this.mortgageInstance = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanextrapaymentprocesschanged.MortgageInstance)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
