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
    public partial class WF21_WorkFlowActivitysActivityCountersigns
    {

        private List<WF211_WorkFlowActivitysActivityCountersignsCountersign> countersignField;

        private string countersignTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Countersign", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<WF211_WorkFlowActivitysActivityCountersignsCountersign> Countersign
        {
            get
            {
                return this.countersignField;
            }
            set
            {
                this.countersignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CountersignType
        {
            get
            {
                return this.countersignTypeField;
            }
            set
            {
                this.countersignTypeField = value;
            }
        }
    }
}
