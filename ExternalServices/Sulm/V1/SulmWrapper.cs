﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExternalServices.Sulm.V1.EasWrapper
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/core/dataTypes/1/0")]
    public partial class SystemExceptionDetails
    {
        
        private string errorCodeField;
        
        private string errorMessageField;
        
        private string errorDetailsField;
        
        private SystemErrorEnum errorTypeField;
        
        private bool errorTypeFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="integer", Order=0)]
        public string errorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string errorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string errorDetails
        {
            get
            {
                return this.errorDetailsField;
            }
            set
            {
                this.errorDetailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public SystemErrorEnum errorType
        {
            get
            {
                return this.errorTypeField;
            }
            set
            {
                this.errorTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool errorTypeSpecified
        {
            get
            {
                return this.errorTypeFieldSpecified;
            }
            set
            {
                this.errorTypeFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/core/dataTypes/1/0")]
    public enum SystemErrorEnum
    {
        
        /// <remarks/>
        UNSPECIFIED,
        
        /// <remarks/>
        ILLEGAL_STATE,
        
        /// <remarks/>
        ILLEGAL_ARGUMENT,
        
        /// <remarks/>
        CALLED_SOURCE_UNAVALIABLE,
        
        /// <remarks/>
        CALLED_SOURCE_FAILED,
        
        /// <remarks/>
        ACCESS_DENIED,
        
        /// <remarks/>
        PERMISSION_DENIED,
        
        /// <remarks/>
        NOT_SUPPORTED,
        
        /// <remarks/>
        UPDATE_CONFLICT,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0")]
    public partial class startUseResponse
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0")]
    public partial class startUseRequest
    {
        
        private long partyIdField;
        
        private string usageCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long partyId
        {
            get
            {
                return this.partyIdField;
            }
            set
            {
                this.partyIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string usageCode
        {
            get
            {
                return this.usageCodeField;
            }
            set
            {
                this.usageCodeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", ConfigurationName="ExternalServices.Sulm.V1.EasWrapper.SulmService")]
    internal interface SulmService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(ExternalServices.Sulm.V1.EasWrapper.SystemExceptionDetails), Action="", Name="system_fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.startUseResponse1> startUseAsync(ExternalServices.Sulm.V1.EasWrapper.startUseRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(ExternalServices.Sulm.V1.EasWrapper.SystemExceptionDetails), Action="", Name="system_fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.stopUseResponse1> stopUseAsync(ExternalServices.Sulm.V1.EasWrapper.stopUseRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(ExternalServices.Sulm.V1.EasWrapper.SystemExceptionDetails), Action="", Name="system_fault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.getStateResponse1> getStateAsync(ExternalServices.Sulm.V1.EasWrapper.getStateRequest1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/core/dataTypes/1/0")]
    public partial class CallerContext
    {
        
        private string applicationField;
        
        private string callerIdField;
        
        private string workstationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string callerId
        {
            get
            {
                return this.callerIdField;
            }
            set
            {
                this.callerIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string workstation
        {
            get
            {
                return this.workstationField;
            }
            set
            {
                this.workstationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://correlation.kb.cz/datatypes/1/0")]
    public partial class CorrelationContext
    {
        
        private string idField;
        
        private string applicationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    internal partial class startUseRequest1
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://esb.kb.cz/core/dataTypes/1/0")]
        public ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://correlation.kb.cz/datatypes/1/0")]
        public ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", Order=0)]
        public ExternalServices.Sulm.V1.EasWrapper.startUseRequest startUseRequest;
        
        public startUseRequest1()
        {
        }
        
        public startUseRequest1(ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext, ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext, ExternalServices.Sulm.V1.EasWrapper.startUseRequest startUseRequest)
        {
            this.callerContext = callerContext;
            this.correlationContext = correlationContext;
            this.startUseRequest = startUseRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    internal partial class startUseResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", Order=0)]
        public ExternalServices.Sulm.V1.EasWrapper.startUseResponse startUseResponse;
        
        public startUseResponse1()
        {
        }
        
        public startUseResponse1(ExternalServices.Sulm.V1.EasWrapper.startUseResponse startUseResponse)
        {
            this.startUseResponse = startUseResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0")]
    public partial class stopUseRequest
    {
        
        private long partyIdField;
        
        private string usageCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long partyId
        {
            get
            {
                return this.partyIdField;
            }
            set
            {
                this.partyIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string usageCode
        {
            get
            {
                return this.usageCodeField;
            }
            set
            {
                this.usageCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0")]
    public partial class stopUseResponse
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    internal partial class stopUseRequest1
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://esb.kb.cz/core/dataTypes/1/0")]
        public ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://correlation.kb.cz/datatypes/1/0")]
        public ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", Order=0)]
        public ExternalServices.Sulm.V1.EasWrapper.stopUseRequest stopUseRequest;
        
        public stopUseRequest1()
        {
        }
        
        public stopUseRequest1(ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext, ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext, ExternalServices.Sulm.V1.EasWrapper.stopUseRequest stopUseRequest)
        {
            this.callerContext = callerContext;
            this.correlationContext = correlationContext;
            this.stopUseRequest = stopUseRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    internal partial class stopUseResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", Order=0)]
        public ExternalServices.Sulm.V1.EasWrapper.stopUseResponse stopUseResponse;
        
        public stopUseResponse1()
        {
        }
        
        public stopUseResponse1(ExternalServices.Sulm.V1.EasWrapper.stopUseResponse stopUseResponse)
        {
            this.stopUseResponse = stopUseResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0")]
    public partial class getStateRequest
    {
        
        private long partyIdField;
        
        private string systemField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long partyId
        {
            get
            {
                return this.partyIdField;
            }
            set
            {
                this.partyIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string system
        {
            get
            {
                return this.systemField;
            }
            set
            {
                this.systemField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0")]
    public partial class getStateResponse
    {
        
        private string stateCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string stateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    internal partial class getStateRequest1
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://esb.kb.cz/core/dataTypes/1/0")]
        public ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://correlation.kb.cz/datatypes/1/0")]
        public ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", Order=0)]
        public ExternalServices.Sulm.V1.EasWrapper.getStateRequest getStateRequest;
        
        public getStateRequest1()
        {
        }
        
        public getStateRequest1(ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext, ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext, ExternalServices.Sulm.V1.EasWrapper.getStateRequest getStateRequest)
        {
            this.callerContext = callerContext;
            this.correlationContext = correlationContext;
            this.getStateRequest = getStateRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    internal partial class getStateResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esb.kb.cz/Sulm/interface/1/0", Order=0)]
        public ExternalServices.Sulm.V1.EasWrapper.getStateResponse getStateResponse;
        
        public getStateResponse1()
        {
        }
        
        public getStateResponse1(ExternalServices.Sulm.V1.EasWrapper.getStateResponse getStateResponse)
        {
            this.getStateResponse = getStateResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    internal interface SulmServiceChannel : ExternalServices.Sulm.V1.EasWrapper.SulmService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    internal partial class SulmServiceClient : System.ServiceModel.ClientBase<ExternalServices.Sulm.V1.EasWrapper.SulmService>, ExternalServices.Sulm.V1.EasWrapper.SulmService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public SulmServiceClient() : 
                base(SulmServiceClient.GetDefaultBinding(), SulmServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.SulmPort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SulmServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(SulmServiceClient.GetBindingForEndpoint(endpointConfiguration), SulmServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SulmServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SulmServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SulmServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SulmServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SulmServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.startUseResponse1> ExternalServices.Sulm.V1.EasWrapper.SulmService.startUseAsync(ExternalServices.Sulm.V1.EasWrapper.startUseRequest1 request)
        {
            return base.Channel.startUseAsync(request);
        }
        
        public System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.startUseResponse1> startUseAsync(ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext, ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext, ExternalServices.Sulm.V1.EasWrapper.startUseRequest startUseRequest)
        {
            ExternalServices.Sulm.V1.EasWrapper.startUseRequest1 inValue = new ExternalServices.Sulm.V1.EasWrapper.startUseRequest1();
            inValue.callerContext = callerContext;
            inValue.correlationContext = correlationContext;
            inValue.startUseRequest = startUseRequest;
            return ((ExternalServices.Sulm.V1.EasWrapper.SulmService)(this)).startUseAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.stopUseResponse1> ExternalServices.Sulm.V1.EasWrapper.SulmService.stopUseAsync(ExternalServices.Sulm.V1.EasWrapper.stopUseRequest1 request)
        {
            return base.Channel.stopUseAsync(request);
        }
        
        public System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.stopUseResponse1> stopUseAsync(ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext, ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext, ExternalServices.Sulm.V1.EasWrapper.stopUseRequest stopUseRequest)
        {
            ExternalServices.Sulm.V1.EasWrapper.stopUseRequest1 inValue = new ExternalServices.Sulm.V1.EasWrapper.stopUseRequest1();
            inValue.callerContext = callerContext;
            inValue.correlationContext = correlationContext;
            inValue.stopUseRequest = stopUseRequest;
            return ((ExternalServices.Sulm.V1.EasWrapper.SulmService)(this)).stopUseAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.getStateResponse1> ExternalServices.Sulm.V1.EasWrapper.SulmService.getStateAsync(ExternalServices.Sulm.V1.EasWrapper.getStateRequest1 request)
        {
            return base.Channel.getStateAsync(request);
        }
        
        public System.Threading.Tasks.Task<ExternalServices.Sulm.V1.EasWrapper.getStateResponse1> getStateAsync(ExternalServices.Sulm.V1.EasWrapper.CallerContext callerContext, ExternalServices.Sulm.V1.EasWrapper.CorrelationContext correlationContext, ExternalServices.Sulm.V1.EasWrapper.getStateRequest getStateRequest)
        {
            ExternalServices.Sulm.V1.EasWrapper.getStateRequest1 inValue = new ExternalServices.Sulm.V1.EasWrapper.getStateRequest1();
            inValue.callerContext = callerContext;
            inValue.correlationContext = correlationContext;
            inValue.getStateRequest = getStateRequest;
            return ((ExternalServices.Sulm.V1.EasWrapper.SulmService)(this)).getStateAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SulmPort))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SulmPort))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:9080/SulmService/1/0");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return SulmServiceClient.GetBindingForEndpoint(EndpointConfiguration.SulmPort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return SulmServiceClient.GetEndpointAddress(EndpointConfiguration.SulmPort);
        }
        
        public enum EndpointConfiguration
        {
            
            SulmPort,
        }
    }
}
