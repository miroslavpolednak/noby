// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// 
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class MainLoanProcessData : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""MainLoanProcessData"",""doc"":"""",""namespace"":""cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged"",""fields"":[{""name"":""processPhase"",""type"":{""type"":""record"",""name"":""ProcessPhase"",""doc"":"""",""namespace"":""cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged"",""fields"":[{""name"":""code"",""type"":""int""},{""name"":""name"",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,255}$""}}]}}]}");
		private cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.ProcessPhase _processPhase;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return MainLoanProcessData._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.ProcessPhase processPhase
		{
			get
			{
				return this._processPhase;
			}
			set
			{
				this._processPhase = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.processPhase;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.processPhase = (cz.mpss.api.starbuild.mortgage.workflow.processevents.v1.mainloanprocesschanged.ProcessPhase)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
