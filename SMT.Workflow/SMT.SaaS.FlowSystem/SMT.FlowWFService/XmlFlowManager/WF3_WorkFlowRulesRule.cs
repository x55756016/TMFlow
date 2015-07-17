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
    public partial class WF3_WorkFlowRulesRule
    {

        private List<WF31_WorkFlowRulesRuleConditions> conditionsField=new List<WF31_WorkFlowRulesRuleConditions>();

        private string nameField;

        private string remarkField;

        private string strStartActiveField;

        private string strEndActiveField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Conditions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<WF31_WorkFlowRulesRuleConditions> Conditions
        {
            get
            {
                return this.conditionsField;
            }
            set
            {
                this.conditionsField = value;
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
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StrStartActive
        {
            get
            {
                return this.strStartActiveField;
            }
            set
            {
                this.strStartActiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StrEndActive
        {
            get
            {
                return this.strEndActiveField;
            }
            set
            {
                this.strEndActiveField = value;
            }
        }
    }
}
