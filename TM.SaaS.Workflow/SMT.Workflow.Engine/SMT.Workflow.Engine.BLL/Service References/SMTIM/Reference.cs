﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMT.Workflow.Engine.BLL.SMTIM {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MessageDataObject", Namespace="http://schemas.datacontract.org/2004/07/SMTIM.Application.DataObjects")]
    [System.SerializableAttribute()]
    public partial class MessageDataObject : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MsgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RemarkField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TargetListField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TargetTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UrlField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Msg {
            get {
                return this.MsgField;
            }
            set {
                if ((object.ReferenceEquals(this.MsgField, value) != true)) {
                    this.MsgField = value;
                    this.RaisePropertyChanged("Msg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Remark {
            get {
                return this.RemarkField;
            }
            set {
                if ((object.ReferenceEquals(this.RemarkField, value) != true)) {
                    this.RemarkField = value;
                    this.RaisePropertyChanged("Remark");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TargetList {
            get {
                return this.TargetListField;
            }
            set {
                if ((object.ReferenceEquals(this.TargetListField, value) != true)) {
                    this.TargetListField = value;
                    this.RaisePropertyChanged("TargetList");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TargetType {
            get {
                return this.TargetTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TargetTypeField, value) != true)) {
                    this.TargetTypeField = value;
                    this.RaisePropertyChanged("TargetType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Type {
            get {
                return this.TypeField;
            }
            set {
                if ((this.TypeField.Equals(value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url {
            get {
                return this.UrlField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlField, value) != true)) {
                    this.UrlField = value;
                    this.RaisePropertyChanged("Url");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JsonResult", Namespace="http://schemas.datacontract.org/2004/07/SMTIM.Application.DataObjects")]
    [System.SerializableAttribute()]
    public partial class JsonResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string Msgk__BackingFieldField;
        
        private bool Successk__BackingFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Msg>k__BackingField", IsRequired=true)]
        public string Msgk__BackingField {
            get {
                return this.Msgk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Msgk__BackingFieldField, value) != true)) {
                    this.Msgk__BackingFieldField = value;
                    this.RaisePropertyChanged("Msgk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Success>k__BackingField", IsRequired=true)]
        public bool Successk__BackingField {
            get {
                return this.Successk__BackingFieldField;
            }
            set {
                if ((this.Successk__BackingFieldField.Equals(value) != true)) {
                    this.Successk__BackingFieldField = value;
                    this.RaisePropertyChanged("Successk__BackingField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SMTIM.IMessageService")]
    public interface IMessageService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMessageService/SendMessage", ReplyAction="http://tempuri.org/IMessageService/SendMessageResponse")]
        SMT.Workflow.Engine.BLL.SMTIM.JsonResult SendMessage(SMT.Workflow.Engine.BLL.SMTIM.MessageDataObject dataObject);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMessageServiceChannel : SMT.Workflow.Engine.BLL.SMTIM.IMessageService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MessageServiceClient : System.ServiceModel.ClientBase<SMT.Workflow.Engine.BLL.SMTIM.IMessageService>, SMT.Workflow.Engine.BLL.SMTIM.IMessageService {
        
        public MessageServiceClient() {
        }
        
        public MessageServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MessageServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MessageServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MessageServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SMT.Workflow.Engine.BLL.SMTIM.JsonResult SendMessage(SMT.Workflow.Engine.BLL.SMTIM.MessageDataObject dataObject) {
            return base.Channel.SendMessage(dataObject);
        }
    }
}
