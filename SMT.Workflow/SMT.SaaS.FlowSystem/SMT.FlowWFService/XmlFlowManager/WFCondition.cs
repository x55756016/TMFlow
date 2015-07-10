using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FlowWFService.XmlFlowManager
{
    /// <summary>
    /// 用来判断同样的条件情况下谁更优先
    /// </summary>
    public class WFCondition
    {
        public string StrStartActive;
        public string StrEndActive;
        public string ConditionCode;
        public string ConditoinOperate;
        public string conditionValue;
        public string conditionType;
    }

    public class WFRoles
    {
        public string StrStartActive;
        public string StrEndActive;
    }
}
