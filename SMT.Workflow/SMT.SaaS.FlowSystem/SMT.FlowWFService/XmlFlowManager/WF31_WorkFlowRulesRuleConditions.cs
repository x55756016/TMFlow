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
    public partial class WF31_WorkFlowRulesRuleConditions
    {

        private List<WF311_WorkFlowRulesRuleConditionsCondition> conditionField=new List<WF311_WorkFlowRulesRuleConditionsCondition>();

        private string nameField;

        private string objectField;

        private string codiCombModeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Condition", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<WF311_WorkFlowRulesRuleConditionsCondition> Condition
        {
            get
            {
                return this.conditionField;
            }
            set
            {
                this.conditionField = value;
            }
        }

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
        public string Object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodiCombMode
        {
            get
            {
                return this.codiCombModeField;
            }
            set
            {
                this.codiCombModeField = value;
            }
        }
    }
    
}
