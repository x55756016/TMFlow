using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FlowWFService.XmlFlowManager
{
    public class WorkflowRuntime
    {
        public string Name;
        public event EventHandler WorkflowCompleted;
        public bool IsStarted;
        public void StartRuntime()
        {

        }
    }
}
