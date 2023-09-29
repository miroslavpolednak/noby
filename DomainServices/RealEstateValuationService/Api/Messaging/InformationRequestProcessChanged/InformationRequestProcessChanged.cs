// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	/// <summary>
	/// Schema belongs to API MortgageProcessEvents.v1. Root type description: General entity for any event.. ## Process represents of one process instance. Process is a group of related activities that fulfills goal and delivers clearly specified value to a customer (process output). CDM entity name(s): Event,Process.
	/// </summary>
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class InformationRequestProcessChanged : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"InformationRequestProcessChanged\",\"doc\":\"Schema belongs " +
				"to API MortgageProcessEvents.v1. Root type description: General entity for any e" +
				"vent.. ## Process represents of one process instance. Process is a group of rela" +
				"ted activities that fulfills goal and delivers clearly specified value to a cust" +
				"omer (process output). CDM entity name(s): Event,Process.\",\"namespace\":\"cz.mpss." +
				"api.starbuild.mortgageworkflow.mortgageprocessevents.v1\",\"fields\":[{\"name\":\"case" +
				"\",\"type\":{\"type\":\"record\",\"name\":\"Case\",\"doc\":\"Obchodni pripadCase is group of a" +
				"ctivities or record about them that is from business perspective perceived as of" +
				" enough importance to be recorded, monitored a informed about.Can be any bank\'s " +
				"agenda.E.g. fufilling client\'s request, sale of one productExamples:- consumer l" +
				"oan origination- loan drawing- administrative action on the product- recovery- c" +
				"omplaint handlingCase has some attributes, that are common for any case category" +
				" and are mandatory. Additionally, specific case categories can have additional d" +
				"ata.In wmt it is called business request (there is child entity for it). CDM ent" +
				"ity name(s): Case.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgage" +
				"processevents.v1.informationrequestprocesschanged\",\"fields\":[{\"name\":\"caseId\",\"t" +
				"ype\":{\"type\":\"record\",\"name\":\"CaseId\",\"doc\":\"Multiattribute representing unique " +
				"identifier of case.It is multiattribute because there is multiple case managemen" +
				"t solutions running now. CDM entity name(s): CaseId.\",\"namespace\":\"cz.mpss.api.s" +
				"tarbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschan" +
				"ged\",\"fields\":[{\"name\":\"id\",\"doc\":\"Unique identifier of case id itself. CDM attr" +
				"ibute class name: CaseIdId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"" +
				",\"pattern\":\"^.{0,36}$\"}}]}},{\"name\":\"mortgageInstance\",\"type\":{\"type\":\"record\",\"" +
				"name\":\"MortgageInstance\",\"doc\":\"Mortgage product instance. CDM entity name(s): M" +
				"ortgageInstance.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgagepr" +
				"ocessevents.v1.informationrequestprocesschanged\",\"fields\":[{\"name\":\"Starbuild\",\"" +
				"type\":{\"type\":\"record\",\"name\":\"StarbuildInstanceId\",\"doc\":\"Multiattribute proper" +
				"ty of marketable item instance.Multiattribute: Identifiers of marketable item in" +
				"stances are not yet unified, there is no one central catalogue.This also covers " +
				"identifiers valid for only some part of marketable item instance lifecycle, e.g." +
				" proposed product.E.g. PCP identifiers, TSS identifiers.#CZ#Viceatributova prome" +
				"nna instanci prodejnych polozek. Viceatributovost: Identifikatory instanci prode" +
				"jych polozek zatim nejsou sjednoceny. Neexistujce centralni katalog. Pokryva tez" +
				" identifikatory, ktere jsou platne pouze pro nektere casti zivotniho cyklu insta" +
				"nci prodejnych polozek (napriklad navrhovany produkt). Priklad: PCP identifikato" +
				"r, TSS identifikator. CDM entity name(s): InstanceId.\",\"namespace\":\"cz.mpss.api." +
				"starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesscha" +
				"nged\",\"fields\":[{\"name\":\"id\",\"doc\":\"Identifier of product instance item itself.#" +
				"CZ#Vlastni identifikator instance prodejne polozky. CDM attribute class name: In" +
				"stanceIdId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0" +
				",30}$\"}}]}}]}}]}},{\"name\":\"currentParentProcess\",\"type\":{\"type\":\"record\",\"name\":" +
				"\"CurrentParentProcess\",\"doc\":\"Process represents of one process instance. Proces" +
				"s is a group of related activities that fulfills goal and delivers clearly speci" +
				"fied value to a customer (process output). CDM entity name(s): Process.\",\"namesp" +
				"ace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informatio" +
				"nrequestprocesschanged\",\"fields\":[{\"name\":\"id\",\"doc\":\"Unique identifier of proce" +
				"ss instance within a workflow system. CDM attribute class name: ProcessId.\",\"typ" +
				"e\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,128}$\"}},{\"name\"" +
				":\"name\",\"doc\":\"Name of the process instance, created at runtime. CDM attribute c" +
				"lass name: ProcessName.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pa" +
				"ttern\":\"^.{0,400}$\"}},{\"name\":\"type\",\"type\":\"int\"}]}},{\"name\":\"currentTask\",\"typ" +
				"e\":{\"type\":\"record\",\"name\":\"CurrentTask\",\"doc\":\"Task is a unit of work that usua" +
				"lly performs 1 role in 1 time in 1 place.Lombardi: task instance, wmt:.... CDM e" +
				"ntity name(s): Task.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortga" +
				"geprocessevents.v1.informationrequestprocesschanged\",\"fields\":[{\"name\":\"id\",\"doc" +
				"\":\"Identifier of a task instance within a \'BPM\' system where task originates. No" +
				"t unique across BPM system instances.In IBM BAW also known as: tkiid, taskId. CD" +
				"M attribute class name: TaskId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"Str" +
				"ing\",\"pattern\":\"^.{0,50}$\"}},{\"name\":\"name\",\"doc\":\"Runtime name of the task. Typ" +
				"ically,  what is presented to users in task/work queue.In IBM BAW also known as:" +
				" subject.Note: Limited to 255 chars due to column definition in BAW internal dat" +
				"abase (BPMDB). CDM attribute class name: TaskName.\",\"type\":{\"type\":\"string\",\"avr" +
				"o.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}},{\"name\":\"type\",\"type\":\"int\"}]}}" +
				",{\"name\":\"eventId\",\"doc\":\"Unique identifier of event. CDM attribute class name: " +
				"EventId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,10" +
				"0}$\"}},{\"name\":\"id\",\"doc\":\"Unique identifier of process instance within a workfl" +
				"ow system. CDM attribute class name: ProcessId.\",\"type\":{\"type\":\"string\",\"avro.j" +
				"ava.string\":\"String\",\"pattern\":\"^.{0,128}$\"}},{\"name\":\"name\",\"doc\":\"Name of the " +
				"process instance, created at runtime. CDM attribute class name: ProcessName.\",\"t" +
				"ype\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,400}$\"}},{\"nam" +
				"e\":\"occurredOn\",\"doc\":\"Date and time when event occurred. CDM attribute class na" +
				"me: EventOccurredOn.\",\"type\":{\"type\":\"long\",\"logicalType\":\"local-timestamp-milli" +
				"s\"}},{\"name\":\"processData\",\"type\":{\"type\":\"record\",\"name\":\"ProcessData\",\"doc\":\"P" +
				"rocess data represent data that are needed for the process execution. For exampl" +
				"e to guide flow of the process based on data.These are typically needed on the \'" +
				"edges\' of activities in the process. Edge meaning when a task is \'entered (input" +
				" variables)\' and \'exited (output variables)\'. CDM entity name(s): ProcessData.\"," +
				"\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.inf" +
				"ormationrequestprocesschanged\",\"fields\":[{\"name\":\"private\",\"type\":{\"type\":\"recor" +
				"d\",\"name\":\"BusinessObject\",\"doc\":\"Data that the task or process carries \'inside\'" +
				". These are typically data that are needed for process or task runtime, such as " +
				"used in UIs, make business decisions inside the process or to integrate external" +
				" services and interfaces. CDM entity name(s): BusinessObject.\",\"namespace\":\"cz.m" +
				"pss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestpr" +
				"ocesschanged\",\"fields\":[{\"name\":\"informationRequestProcessData\",\"type\":{\"type\":\"" +
				"record\",\"name\":\"InformationRequestProcessData\",\"doc\":\"\",\"namespace\":\"cz.mpss.api" +
				".starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocessch" +
				"anged\",\"fields\":[{\"name\":\"processPhase\",\"type\":{\"type\":\"record\",\"name\":\"ProcessP" +
				"hase\",\"doc\":\"\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageproce" +
				"ssevents.v1.informationrequestprocesschanged\",\"fields\":[{\"name\":\"code\",\"type\":\"i" +
				"nt\"},{\"name\":\"name\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern" +
				"\":\"^.{0,255}$\"}}]}}]}}]}}]}},{\"name\":\"state\",\"doc\":\"Runtime state of the process" +
				" within the BPM engine. More of a lifecycle state of the process intance within " +
				"the BPM engine.Not necesarilly \'business\' state of the process.  CDM attribute c" +
				"lass name: ProcessState.\",\"type\":{\"type\":\"enum\",\"name\":\"ProcessStateEnum\",\"doc\":" +
				"\"\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1\"" +
				",\"symbols\":[\"ACTIVE\",\"COMPLETED\",\"DELETED\",\"FAILED\",\"SUSPENDED\",\"TERMINATED\"]}}," +
				"{\"name\":\"type\",\"type\":\"int\"}],\"javaAnnotation\":\"cz.kb.api.common.annotation.Conf" +
				"luentSchemaRegistryCompatible\"}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.Case _case;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.CurrentParentProcess _currentParentProcess;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.CurrentTask _currentTask;
		/// <summary>
		/// Unique identifier of event. CDM attribute class name: EventId.
		/// </summary>
		private string _eventId;
		/// <summary>
		/// Unique identifier of process instance within a workflow system. CDM attribute class name: ProcessId.
		/// </summary>
		private string _id;
		/// <summary>
		/// Name of the process instance, created at runtime. CDM attribute class name: ProcessName.
		/// </summary>
		private string _name;
		/// <summary>
		/// Date and time when event occurred. CDM attribute class name: EventOccurredOn.
		/// </summary>
		private System.DateTime _occurredOn;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.ProcessData _processData;
		/// <summary>
		/// Runtime state of the process within the BPM engine. More of a lifecycle state of the process intance within the BPM engine.Not necesarilly 'business' state of the process.  CDM attribute class name: ProcessState.
		/// </summary>
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum _state;
		private int _type;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return InformationRequestProcessChanged._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.Case @case
		{
			get
			{
				return this._case;
			}
			set
			{
				this._case = value;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.CurrentParentProcess currentParentProcess
		{
			get
			{
				return this._currentParentProcess;
			}
			set
			{
				this._currentParentProcess = value;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.CurrentTask currentTask
		{
			get
			{
				return this._currentTask;
			}
			set
			{
				this._currentTask = value;
			}
		}
		/// <summary>
		/// Unique identifier of event. CDM attribute class name: EventId.
		/// </summary>
		public string eventId
		{
			get
			{
				return this._eventId;
			}
			set
			{
				this._eventId = value;
			}
		}
		/// <summary>
		/// Unique identifier of process instance within a workflow system. CDM attribute class name: ProcessId.
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
		/// Name of the process instance, created at runtime. CDM attribute class name: ProcessName.
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
		/// <summary>
		/// Date and time when event occurred. CDM attribute class name: EventOccurredOn.
		/// </summary>
		public System.DateTime occurredOn
		{
			get
			{
				return this._occurredOn;
			}
			set
			{
				this._occurredOn = value;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.ProcessData processData
		{
			get
			{
				return this._processData;
			}
			set
			{
				this._processData = value;
			}
		}
		/// <summary>
		/// Runtime state of the process within the BPM engine. More of a lifecycle state of the process intance within the BPM engine.Not necesarilly 'business' state of the process.  CDM attribute class name: ProcessState.
		/// </summary>
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
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
			case 0: return this.@case;
			case 1: return this.currentParentProcess;
			case 2: return this.currentTask;
			case 3: return this.eventId;
			case 4: return this.id;
			case 5: return this.name;
			case 6: return this.occurredOn;
			case 7: return this.processData;
			case 8: return this.state;
			case 9: return this.type;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.@case = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.Case)fieldValue; break;
			case 1: this.currentParentProcess = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.CurrentParentProcess)fieldValue; break;
			case 2: this.currentTask = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.CurrentTask)fieldValue; break;
			case 3: this.eventId = (System.String)fieldValue; break;
			case 4: this.id = (System.String)fieldValue; break;
			case 5: this.name = (System.String)fieldValue; break;
			case 6: this.occurredOn = (System.DateTime)fieldValue; break;
			case 7: this.processData = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.informationrequestprocesschanged.ProcessData)fieldValue; break;
			case 8: this.state = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum)fieldValue; break;
			case 9: this.type = (System.Int32)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}