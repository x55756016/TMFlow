using SMT.Workflow.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FlowWFService.XmlFlowManager
{
   public class WorkflowInstance
    {
       public string InstanceId;
        public string XMLDefine;
        public string CurrentActivit;
        public FLOW_FLOWRECORDMASTER_T Master;
        public FLOW_FLOWRECORDDETAIL_T Details;

       public void Terminate(string value)
        {

        }

       public void SetState(string stateCode)
       {

       }
    }
}
