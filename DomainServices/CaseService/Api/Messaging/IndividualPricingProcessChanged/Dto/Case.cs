// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Obchodni pripadCase is group of activities or record about them that is from business perspective perceived as of enough importance to be recorded, monitored a informed about.Can be any bank's agenda.E.g. fufilling client's request, sale of one productExamples:- consumer loan origination- loan drawing- administrative action on the product- recovery- complaint handlingCase has some attributes, that are common for any case category and are mandatory. Additionally, specific case categories can have additional data.In wmt it is called business request (there is child entity for it). CDM entity name(s): Case.
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
				" it is called business request (there is child entity for it). CDM entity name(s" +
				"): Case.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocesseve" +
				"nts.v1.individualpricingprocesschanged\",\"fields\":[{\"name\":\"caseId\",\"type\":{\"type" +
				"\":\"record\",\"name\":\"CaseId\",\"doc\":\"Multiattribute representing unique identifier " +
				"of case.It is multiattribute because there is multiple case management solutions" +
				" running now. CDM entity name(s): CaseId.\",\"namespace\":\"cz.mpss.api.starbuild.mo" +
				"rtgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged\",\"fields" +
				"\":[{\"name\":\"id\",\"doc\":\"Unique identifier of case id itself. CDM attribute class " +
				"name: CaseIdId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"" +
				"^.{0,36}$\"}}]}},{\"name\":\"mortgageInstance\",\"type\":{\"type\":\"record\",\"name\":\"Mortg" +
				"ageInstance\",\"doc\":\"Mortgage product instance. CDM entity name(s): MortgageInsta" +
				"nce.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents." +
				"v1.individualpricingprocesschanged\",\"fields\":[{\"name\":\"Starbuild\",\"type\":{\"type\"" +
				":\"record\",\"name\":\"StarbuildInstanceId\",\"doc\":\"Multiattribute property of marketa" +
				"ble item instance.Multiattribute: Identifiers of marketable item instances are n" +
				"ot yet unified, there is no one central catalogue.This also covers identifiers v" +
				"alid for only some part of marketable item instance lifecycle, e.g. proposed pro" +
				"duct.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova promenna instanci " +
				"prodejnych polozek. Viceatributovost: Identifikatory instanci prodejych polozek " +
				"zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez identifikato" +
				"ry, ktere jsou platne pouze pro nektere casti zivotniho cyklu instanci prodejnyc" +
				"h polozek (napriklad navrhovany produkt). Priklad: PCP identifikator, TSS identi" +
				"fikator. CDM entity name(s): InstanceId.\",\"namespace\":\"cz.mpss.api.starbuild.mor" +
				"tgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged\",\"fields\"" +
				":[{\"name\":\"id\",\"doc\":\"Identifier of product instance item itself.#CZ#Vlastni ide" +
				"ntifikator instance prodejne polozky. CDM attribute class name: InstanceIdId.\",\"" +
				"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}}]}}" +
				"]}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.CaseId _caseId;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.MortgageInstance _mortgageInstance;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return Case._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.CaseId caseId
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
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.MortgageInstance mortgageInstance
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
			case 1: return this.mortgageInstance;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.caseId = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.CaseId)fieldValue; break;
			case 1: this.mortgageInstance = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.individualpricingprocesschanged.MortgageInstance)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}