using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using SMT.WFLib;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Transactions;
using SMT.Foundation.Log;
using System.Xml.Linq;
using System.Diagnostics;
using System.Data.OracleClient;
using SMT.FLOWDAL.ADO;
using System.Data;
using System.Data.SqlClient;
using SMT.Workflow.Common.Model.FlowEngine;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Core;
using SMT.FlowWFService.XmlFlowManager;
using TM_SaaS_OA_EFModel;
using SMT.HRM.BLL.Permission;


namespace SMT.FlowWFService.NewFlow
{
    // 注意: 如果更改此处的类名“IService1”，也必须更新 App.config 中对“IService1”的引用。
    public partial class FlowService
    {
    }
}
