// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.collateralvaluationprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Process data represent data that are needed for the process execution. For example to guide flow of the process based on data.These are typically needed on the 'edges' of activities in the process. Edge meaning when a task is 'entered (input variables)' and 'exited (output variables)'. CDM entity name(s): ProcessData.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class ProcessData : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"ProcessData\",\"doc\":\"Process data represent data that are" +
				" needed for the process execution. For example to guide flow of the process base" +
				"d on data.These are typically needed on the \'edges\' of activities in the process" +
				". Edge meaning when a task is \'entered (input variables)\' and \'exited (output va" +
				"riables)\'. CDM entity name(s): ProcessData.\",\"namespace\":\"cz.mpss.api.starbuild." +
				"mortgageworkflow.mortgageprocessevents.v1.collateralvaluationprocesschanged\",\"fi" +
				"elds\":[{\"name\":\"private\",\"type\":{\"type\":\"record\",\"name\":\"BusinessObject\",\"doc\":\"" +
				"Data that the task or process carries \'inside\'. These are typically data that ar" +
				"e needed for process or task runtime, such as used in UIs, make business decisio" +
				"ns inside the process or to integrate external services and interfaces. CDM enti" +
				"ty name(s): BusinessObject.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow" +
				".mortgageprocessevents.v1.collateralvaluationprocesschanged\",\"fields\":[{\"name\":\"" +
				"collateralValuationProcessData\",\"type\":{\"type\":\"record\",\"name\":\"CollateralValuat" +
				"ionProcessData\",\"doc\":\"\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mor" +
				"tgageprocessevents.v1.collateralvaluationprocesschanged\",\"fields\":[{\"name\":\"proc" +
				"essPhase\",\"type\":{\"type\":\"record\",\"name\":\"ProcessPhase\",\"doc\":\"\",\"namespace\":\"cz" +
				".mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.collateralvaluatio" +
				"nprocesschanged\",\"fields\":[{\"name\":\"code\",\"type\":\"int\"},{\"name\":\"name\",\"type\":{\"" +
				"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}}]}}]}}]}}]}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.collateralvaluationprocesschanged.BusinessObject _private;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return ProcessData._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.collateralvaluationprocesschanged.BusinessObject @private
		{
			get
			{
				return this._private;
			}
			set
			{
				this._private = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.@private;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.@private = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.collateralvaluationprocesschanged.BusinessObject)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
