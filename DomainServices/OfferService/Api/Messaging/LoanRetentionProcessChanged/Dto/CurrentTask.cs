// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanretentionprocesschanged
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Task is a unit of work that usually performs 1 role in 1 time in 1 place.Lombardi: task instance, wmt:.... CDM entity name(s): Task.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class CurrentTask : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""CurrentTask"",""doc"":""Task is a unit of work that usually performs 1 role in 1 time in 1 place.Lombardi: task instance, wmt:.... CDM entity name(s): Task."",""namespace"":""cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.loanretentionprocesschanged"",""fields"":[{""name"":""id"",""doc"":""Identifier of a task instance within a 'BPM' system where task originates. Not unique across BPM system instances.In IBM BAW also known as: tkiid, taskId. CDM attribute class name: TaskId."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,50}$""}},{""name"":""name"",""doc"":""Runtime name of the task. Typically,  what is presented to users in task/work queue.In IBM BAW also known as: subject.Note: Limited to 255 chars due to column definition in BAW internal database (BPMDB). CDM attribute class name: TaskName."",""type"":{""type"":""string"",""avro.java.string"":""String"",""pattern"":""^.{0,255}$""}},{""name"":""type"",""type"":""int""}]}");
		/// <summary>
		/// Identifier of a task instance within a 'BPM' system where task originates. Not unique across BPM system instances.In IBM BAW also known as: tkiid, taskId. CDM attribute class name: TaskId.
		/// </summary>
		private string _id;
		/// <summary>
		/// Runtime name of the task. Typically,  what is presented to users in task/work queue.In IBM BAW also known as: subject.Note: Limited to 255 chars due to column definition in BAW internal database (BPMDB). CDM attribute class name: TaskName.
		/// </summary>
		private string _name;
		private int _type;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return CurrentTask._SCHEMA;
			}
		}
		/// <summary>
		/// Identifier of a task instance within a 'BPM' system where task originates. Not unique across BPM system instances.In IBM BAW also known as: tkiid, taskId. CDM attribute class name: TaskId.
		/// </summary>
		public string id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
		/// <summary>
		/// Runtime name of the task. Typically,  what is presented to users in task/work queue.In IBM BAW also known as: subject.Note: Limited to 255 chars due to column definition in BAW internal database (BPMDB). CDM attribute class name: TaskName.
		/// </summary>
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}
		public int type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.id;
			case 1: return this.name;
			case 2: return this.type;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.id = (System.String)fieldValue; break;
			case 1: this.name = (System.String)fieldValue; break;
			case 2: this.type = (System.Int32)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
