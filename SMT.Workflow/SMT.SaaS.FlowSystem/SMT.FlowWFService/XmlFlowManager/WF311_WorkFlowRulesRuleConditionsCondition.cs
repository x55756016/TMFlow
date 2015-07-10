using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FlowWFService.XmlFlowManager
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class WF311_WorkFlowRulesRuleConditionsCondition
    {

        private string nameField;

        private string descriptionField;

        private string compAttrField;

        private string dataTypeField;

        private string operateField;

        private string compareValueMarkField;

        private string compareValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CompAttr
        {
            get
            {
                return this.compAttrField;
            }
            set
            {
                this.compAttrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DataType
        {
            get
            {
                return this.dataTypeField;
            }
            set
            {
                this.dataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Operate
        {
            get
            {
                return this.operateField;
            }
            set
            {
                this.operateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CompareValueMark
        {
            get
            {
                return this.compareValueMarkField;
            }
            set
            {
                this.compareValueMarkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CompareValue
        {
            get
            {
                return this.compareValueField;
            }
            set
            {
                this.compareValueField = value;
            }
        }
    }
}
