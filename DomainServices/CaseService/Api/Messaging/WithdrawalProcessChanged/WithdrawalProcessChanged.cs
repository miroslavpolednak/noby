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
	public partial class WithdrawalProcessChanged : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"WithdrawalProcessChanged\",\"doc\":\"Schema belongs to API M" +
				"ortgageProcessEvents.v1. Root type description: General entity for any event.. #" +
				"# Process represents of one process instance. Process is a group of related acti" +
				"vities that fulfills goal and delivers clearly specified value to a customer (pr" +
				"ocess output). CDM entity name(s): Event,Process.\",\"namespace\":\"cz.mpss.api.star" +
				"build.mortgageworkflow.mortgageprocessevents.v1\",\"fields\":[{\"name\":\"case\",\"type\"" +
				":{\"type\":\"record\",\"name\":\"Case\",\"doc\":\"Obchodni pripadCase is group of activitie" +
				"s or record about them that is from business perspective perceived as of enough " +
				"importance to be recorded, monitored a informed about.Can be any bank\'s agenda.E" +
				".g. fufilling client\'s request, sale of one productExamples:- consumer loan orig" +
				"ination- loan drawing- administrative action on the product- recovery- complaint" +
				" handlingCase has some attributes, that are common for any case category and are" +
				" mandatory. Additionally, specific case categories can have additional data.In w" +
				"mt it is called business request (there is child entity for it). CDM entity name" +
				"(s): Case.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocesse" +
				"vents.v1.withdrawalprocesschanged\",\"fields\":[{\"name\":\"caseId\",\"type\":{\"type\":\"re" +
				"cord\",\"name\":\"CaseId\",\"doc\":\"Multiattribute representing unique identifier of ca" +
				"se.It is multiattribute because there is multiple case management solutions runn" +
				"ing now. CDM entity name(s): CaseId.\",\"namespace\":\"cz.mpss.api.starbuild.mortgag" +
				"eworkflow.mortgageprocessevents.v1.withdrawalprocesschanged\",\"fields\":[{\"name\":\"" +
				"id\",\"doc\":\"Unique identifier of case id itself. CDM attribute class name: CaseId" +
				"Id.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,36}$\"}}" +
				"]}},{\"name\":\"mortgageInstance\",\"type\":{\"type\":\"record\",\"name\":\"MortgageInstance\"" +
				",\"doc\":\"Mortgage product instance. CDM entity name(s): MortgageInstance.\",\"names" +
				"pace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawa" +
				"lprocesschanged\",\"fields\":[{\"name\":\"Starbuild\",\"type\":{\"type\":\"record\",\"name\":\"S" +
				"tarbuildInstanceId\",\"doc\":\"Multiattribute property of marketable item instance.M" +
				"ultiattribute: Identifiers of marketable item instances are not yet unified, the" +
				"re is no one central catalogue.This also covers identifiers valid for only some " +
				"part of marketable item instance lifecycle, e.g. proposed product.E.g. PCP ident" +
				"ifiers, TSS identifiers.#CZ#Viceatributova promenna instanci prodejnych polozek." +
				" Viceatributovost: Identifikatory instanci prodejych polozek zatim nejsou sjedno" +
				"ceny. Neexistujce centralni katalog. Pokryva tez identifikatory, ktere jsou plat" +
				"ne pouze pro nektere casti zivotniho cyklu instanci prodejnych polozek (naprikla" +
				"d navrhovany produkt). Priklad: PCP identifikator, TSS identifikator. CDM entity" +
				" name(s): InstanceId.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortg" +
				"ageprocessevents.v1.withdrawalprocesschanged\",\"fields\":[{\"name\":\"id\",\"doc\":\"Iden" +
				"tifier of product instance item itself.#CZ#Vlastni identifikator instance prodej" +
				"ne polozky. CDM attribute class name: InstanceIdId.\",\"type\":{\"type\":\"string\",\"av" +
				"ro.java.string\":\"String\",\"pattern\":\"^.{0,30}$\"}}]}}]}}]}},{\"name\":\"currentParent" +
				"Process\",\"type\":{\"type\":\"record\",\"name\":\"CurrentParentProcess\",\"doc\":\"Process re" +
				"presents of one process instance. Process is a group of related activities that " +
				"fulfills goal and delivers clearly specified value to a customer (process output" +
				"). CDM entity name(s): Process.\",\"namespace\":\"cz.mpss.api.starbuild.mortgagework" +
				"flow.mortgageprocessevents.v1.withdrawalprocesschanged\",\"fields\":[{\"name\":\"id\",\"" +
				"doc\":\"Unique identifier of process instance within a workflow system. CDM attrib" +
				"ute class name: ProcessId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\"," +
				"\"pattern\":\"^.{0,128}$\"}},{\"name\":\"name\",\"doc\":\"Name of the process instance, cre" +
				"ated at runtime. CDM attribute class name: ProcessName.\",\"type\":{\"type\":\"string\"" +
				",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,400}$\"}},{\"name\":\"type\",\"type\":\"int" +
				"\"}]}},{\"name\":\"currentTask\",\"type\":{\"type\":\"record\",\"name\":\"CurrentTask\",\"doc\":\"" +
				"Task is a unit of work that usually performs 1 role in 1 time in 1 place.Lombard" +
				"i: task instance, wmt:.... CDM entity name(s): Task.\",\"namespace\":\"cz.mpss.api.s" +
				"tarbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged\",\"fi" +
				"elds\":[{\"name\":\"id\",\"doc\":\"Identifier of a task instance within a \'BPM\' system w" +
				"here task originates. Not unique across BPM system instances.In IBM BAW also kno" +
				"wn as: tkiid, taskId. CDM attribute class name: TaskId.\",\"type\":{\"type\":\"string\"" +
				",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,50}$\"}},{\"name\":\"name\",\"doc\":\"Runti" +
				"me name of the task. Typically,  what is presented to users in task/work queue.I" +
				"n IBM BAW also known as: subject.Note: Limited to 255 chars due to column defini" +
				"tion in BAW internal database (BPMDB). CDM attribute class name: TaskName.\",\"typ" +
				"e\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}},{\"name\"" +
				":\"type\",\"type\":\"int\"}]}},{\"name\":\"eventId\",\"doc\":\"Unique identifier of event. CD" +
				"M attribute class name: EventId.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"St" +
				"ring\",\"pattern\":\"^.{0,100}$\"}},{\"name\":\"id\",\"doc\":\"Unique identifier of process " +
				"instance within a workflow system. CDM attribute class name: ProcessId.\",\"type\":" +
				"{\"type\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,128}$\"}},{\"name\":\"n" +
				"ame\",\"doc\":\"Name of the process instance, created at runtime. CDM attribute clas" +
				"s name: ProcessName.\",\"type\":{\"type\":\"string\",\"avro.java.string\":\"String\",\"patte" +
				"rn\":\"^.{0,400}$\"}},{\"name\":\"occurredOn\",\"doc\":\"Date and time when event occurred" +
				". CDM attribute class name: EventOccurredOn.\",\"type\":{\"type\":\"long\",\"logicalType" +
				"\":\"timestamp-millis\"}},{\"name\":\"processData\",\"type\":{\"type\":\"record\",\"name\":\"Pro" +
				"cessData\",\"doc\":\"Process data represent data that are needed for the process exe" +
				"cution. For example to guide flow of the process based on data.These are typical" +
				"ly needed on the \'edges\' of activities in the process. Edge meaning when a task " +
				"is \'entered (input variables)\' and \'exited (output variables)\'. CDM entity name(" +
				"s): ProcessData.\",\"namespace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgagepr" +
				"ocessevents.v1.withdrawalprocesschanged\",\"fields\":[{\"name\":\"private\",\"type\":{\"ty" +
				"pe\":\"record\",\"name\":\"BusinessObject\",\"doc\":\"Data that the task or process carrie" +
				"s \'inside\'. These are typically data that are needed for process or task runtime" +
				", such as used in UIs, make business decisions inside the process or to integrat" +
				"e external services and interfaces. CDM entity name(s): BusinessObject.\",\"namesp" +
				"ace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawal" +
				"processchanged\",\"fields\":[{\"name\":\"withdrawalProcessData\",\"type\":{\"type\":\"record" +
				"\",\"name\":\"WithdrawalProcessData\",\"doc\":\"\",\"namespace\":\"cz.mpss.api.starbuild.mor" +
				"tgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged\",\"fields\":[{\"nam" +
				"e\":\"processPhase\",\"type\":{\"type\":\"record\",\"name\":\"ProcessPhase\",\"doc\":\"\",\"namesp" +
				"ace\":\"cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawal" +
				"processchanged\",\"fields\":[{\"name\":\"code\",\"type\":\"int\"},{\"name\":\"name\",\"type\":{\"t" +
				"ype\":\"string\",\"avro.java.string\":\"String\",\"pattern\":\"^.{0,255}$\"}}]}}]}}]}}]}},{" +
				"\"name\":\"state\",\"doc\":\"Runtime state of the process within the BPM engine. More o" +
				"f a lifecycle state of the process intance within the BPM engine.Not necesarilly" +
				" \'business\' state of the process.  CDM attribute class name: ProcessState.\",\"typ" +
				"e\":{\"type\":\"enum\",\"name\":\"ProcessStateEnum\",\"doc\":\"\",\"namespace\":\"cz.mpss.api.st" +
				"arbuild.mortgageworkflow.mortgageprocessevents.v1\",\"symbols\":[\"ACTIVE\",\"COMPLETE" +
				"D\",\"DELETED\",\"FAILED\",\"SUSPENDED\",\"TERMINATED\"]}},{\"name\":\"type\",\"type\":\"int\"}]," +
				"\"javaAnnotation\":\"cz.kb.api.common.annotation.ConfluentSchemaRegistryCompatible\"" +
				"}");
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.Case _case;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.CurrentParentProcess _currentParentProcess;
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.CurrentTask _currentTask;
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
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.ProcessData _processData;
		/// <summary>
		/// Runtime state of the process within the BPM engine. More of a lifecycle state of the process intance within the BPM engine.Not necesarilly 'business' state of the process.  CDM attribute class name: ProcessState.
		/// </summary>
		private cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum _state;
		private int _type;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return WithdrawalProcessChanged._SCHEMA;
			}
		}
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.Case @case
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
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.CurrentParentProcess currentParentProcess
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
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.CurrentTask currentTask
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
		public cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.ProcessData processData
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
			case 0: this.@case = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.Case)fieldValue; break;
			case 1: this.currentParentProcess = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.CurrentParentProcess)fieldValue; break;
			case 2: this.currentTask = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.CurrentTask)fieldValue; break;
			case 3: this.eventId = (System.String)fieldValue; break;
			case 4: this.id = (System.String)fieldValue; break;
			case 5: this.name = (System.String)fieldValue; break;
			case 6: this.occurredOn = (System.DateTime)fieldValue; break;
			case 7: this.processData = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.withdrawalprocesschanged.ProcessData)fieldValue; break;
			case 8: this.state = (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum)fieldValue; break;
			case 9: this.type = (System.Int32)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}