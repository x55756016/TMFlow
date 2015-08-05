using EngineDataModel;
using SMT.Global.IEngineContract;
using SMT.Workflow.Engine.Services.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace SMT.Workflow.Engine.Services
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AjaxWFService
    {
        // 要使用 HTTP GET，请添加 [WebGet] 特性。(默认 ResponseFormat 为 WebMessageFormat.Json)
        // 要创建返回 XML 的操作，
        //     请添加 [WebGet(ResponseFormat=WebMessageFormat.Xml)]，
        //     并在操作正文中包括以下行:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        /// <summary>
        /// 移动端获取待办数据
        /// </summary>
        /// <param name="UserID">用户id</param>
        /// <param name="Status">状态已处理CLOSE或open待处理</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="MessageBody">搜索的待办内容like</param>
        /// <param name="IsAutofresh">是否是自动刷新还是手动查询</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "AjaxGetTaskList/{UserID}/{Status}/{MessageBody}/{PageIndex}/{IsAutofresh}", ResponseFormat = WebMessageFormat.Json)]
        taskResult AjaxGetTaskList(string UserID, string Status, int PageIndex, string MessageBody, string IsAutofresh)
        {
            taskResult result = new taskResult();
            result.IsSuccess = "1";
            EngineServicesBLL bll = new EngineServicesBLL();
            MsgParms msgParams = new MsgParms();
            msgParams.BeginDate = DateTime.MinValue;
            msgParams.EndDate = DateTime.MinValue;
            msgParams.MessageBody = MessageBody;
            msgParams.PageIndex = PageIndex;
            msgParams.Status = Status;
            msgParams.UserID = UserID;
            msgParams.PageSize = 20;//写死显示20条
            msgParams.Top = 20;//写死显示20条
            msgParams.LastDay = 30;//参数最近30天，然而并没有什么卵用
            if (!string.IsNullOrEmpty(IsAutofresh))//是否自动刷新
            {
                if (IsAutofresh == "0")
                {
                    result.IsAutofresh = false;
                }
                else
                {
                    result.IsAutofresh = true;
                }
            }
            else
            {
                result.IsAutofresh = true;
            }
            
            result.ObjData = bll.PendingTasksParmsPageIndex(msgParams, ref result.IsAutofresh, ref result.HaveNewTask, ref result.rowCount, ref result.pageCount);
            if(result.ObjData==null)
            {
                result.IsSuccess = "0";
            }
            return result;
        }
        // 在此处添加更多操作并使用 [OperationContract] 标记它们
    }

    public class taskResult
    {
        public string IsSuccess;
        public bool IsAutofresh;
        public bool HaveNewTask;
        public int rowCount;
        public int pageCount;
        public List<T_FLOW_ENGINEMSGLIST> ObjData;
    }
}
