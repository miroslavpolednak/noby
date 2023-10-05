// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Data that the task or process carries 'inside'. These are typically data that are needed for process or task runtime, such as used in UIs, make business decisions inside the process or to integrate external services and interfaces. CDM entity name(s): BusinessObject.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class BusinessObject : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""BusinessObject"",""doc"":""Data that the task or process carries 'inside'. These are typically data that are needed for process or task runtime, such as used in UIs, make business decisions inside the process or to integrate external services and interfaces. CDM entity name(s): BusinessObject."",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged"",""fields"":[{""name"":""informationRequestProcessData"",""type"":{""type"":""record"",""name"":""InformationRequestProcessData"",""doc"":"""",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged"",""fields"":[{""name"":""processPhase"",""type"":{""type"":""record"",""name"":""ProcessPhase"",""doc"":"""",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged"",""fields"":[{""name"":""code"",""type"":""int""},{""name"":""name"",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,255}$""}}]}}]}}]}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.InformationRequestProcessData _informationRequestProcessData;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return BusinessObject._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.InformationRequestProcessData informationRequestProcessData
		{
			get
			{
				return this._informationRequestProcessData;
			}
			set
			{
				this._informationRequestProcessData = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.informationRequestProcessData;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.informationRequestProcessData = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.InformationRequestProcessData)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
