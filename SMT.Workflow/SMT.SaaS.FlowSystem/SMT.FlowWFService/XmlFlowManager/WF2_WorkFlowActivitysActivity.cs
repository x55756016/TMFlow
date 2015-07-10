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
    public partial class WF2_WorkFlowActivitysActivity
    {

        private List<WF21_WorkFlowActivitysActivityCountersigns> countersignsField;

        private string nameField;

        private string xField;

        private string yField;

        private string roleNameField;

        private string userTypeField;

        private string isOtherCompanyField;

        private string otherCompanyIDField;

        private string otherCompanyNameField;

        private string remarkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Countersigns", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<WF21_WorkFlowActivitysActivityCountersigns> Countersigns
        {
            get
            {
                return this.countersignsField;
            }
            set
            {
                this.countersignsField = value;
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
        public string X
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RoleName
        {
            get
            {
                return this.roleNameField;
            }
            set
            {
                this.roleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string UserType
        {
            get
            {
                return this.userTypeField;
            }
            set
            {
                this.userTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IsOtherCompany
        {
            get
            {
                return this.isOtherCompanyField;
            }
            set
            {
                this.isOtherCompanyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OtherCompanyID
        {
            get
            {
                return this.otherCompanyIDField;
            }
            set
            {
                this.otherCompanyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OtherCompanyName
        {
            get
            {
                return this.otherCompanyNameField;
            }
            set
            {
                this.otherCompanyNameField = value;
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
    }
}
