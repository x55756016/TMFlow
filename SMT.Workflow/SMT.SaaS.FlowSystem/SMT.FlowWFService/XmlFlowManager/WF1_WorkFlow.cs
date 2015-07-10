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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class WF1_WorkFlow
    {

        private string systemField;

        private List<WF2_WorkFlowActivitysActivity> activitysField;

        private List<WF3_WorkFlowRulesRule> rulesField;

        /// <remarks/>
        public string System
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

        /// <remarks/>
        public List<WF2_WorkFlowActivitysActivity> Activitys
        {
            get
            {
                return this.activitysField;
            }
            set
            {
                this.activitysField = value;
            }
        }

        /// <remarks/>
        public List<WF3_WorkFlowRulesRule> Rules
        {
            get
            {
                return this.rulesField;
            }
            set
            {
                this.rulesField = value;
            }
        }
    }
}
