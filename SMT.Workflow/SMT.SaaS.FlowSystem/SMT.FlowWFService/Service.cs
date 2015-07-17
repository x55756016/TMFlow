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

using SMT.FLOWDAL.ADO;
using System.Data.OracleClient;
using SMT.Workflow.Common.Model.FlowEngine;
using SMT.Workflow.Common.Model;
using SMT.FlowWFService.NewFlow;
using SMT.FlowWFService.XmlFlowManager;




namespace SMT.FlowWFService
{
    // 注意: 如果更改此处的类名“IService1”，也必须更新 App.config 中对“IService1”的引用。
    public class Service : IService
    {
        private static string _strIsFlowEngine;
        private static string strIsFlowEngine
        {
            get
            {
                if (_strIsFlowEngine == null)
                {
                    _strIsFlowEngine = ConfigurationManager.AppSettings["IsFlowEngine"];
                    if (_strIsFlowEngine == null)
                    {
                        _strIsFlowEngine = "";
                    }
                }
                return _strIsFlowEngine;
            }
        }

        #region 咨询
        public void AddConsultation(FLOW_CONSULTATION_T flowConsultation, SubmitData submitData)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            s2.AddConsultation(flowConsultation,  submitData);
            #region 旧代码
            //if (strIsFlowEngine.ToLower() == "true")
            //{
            //    try
            //    {
            //        flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T =
            //            FlowBLL.GetFLOW_FLOWRECORDMASTER_T(flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);
            //        FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
            //        FlowEngineService.CustomUserMsg[] cs = new FlowEngineService.CustomUserMsg[1];

            //        FlowEngineService.CustomUserMsg cu = new FlowEngineService.CustomUserMsg();
            //        cu.FormID = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID;
            //        cu.UserID = flowConsultation.REPLYUSERID;
            //        cs[0] = cu;
            //        ModelInfo modelinfo = FlowBLL.GetSysCodeByModelCode(submitData.ModelCode);
            //        MessageData tmpMessageData = new MessageData("Flow", modelinfo.SysCode,
            //            flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID,
            //            submitData.ModelCode, modelinfo.ModelName, submitData.FormID, flowConsultation.FLOW_FLOWRECORDDETAIL_T.STATECODE, flowConsultation.FLOW_FLOWRECORDDETAIL_T.CHECKSTATE, "", "", "", "");
            //        FlowBLL flowBLL = new FlowBLL();
            //        StringBuilder FlowResultXml = flowBLL.BuildMessageData(tmpMessageData);
            //        //FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
            //        //log = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
            //        if (!string.IsNullOrEmpty(flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT))
            //        {
            //            submitData.XML = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT;
            //        }

            //        bool b = FlowEngine.FlowConsultati(cs, "", FlowResultXml.ToString(), submitData.XML);
            //        if (!b)
            //        {
            //            Tracer.Debug("FlowEngineService-FlowConsultati:" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID
            //                + "\nsubmitData:" + submitData.XML);
            //        }

            //        FlowBLL bll = new FlowBLL();

            //        bll.AddConsultation(flowConsultation);
            //    }
            //    catch (Exception ex)
            //    {
            //        Tracer.Debug("AddConsultation: -" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "-" + ex.InnerException + ex.Message);
            //        throw ex;
            //    }
            //}

            #endregion

        }
        public void ReplyConsultation(FLOW_CONSULTATION_T flowConsultation, SubmitData submitData)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            s2.ReplyConsultation( flowConsultation, submitData);
            #region 旧代码
            //if (strIsFlowEngine.ToLower() == "true")
            //{
            //    try
            //    {


            //        FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
            //        //Byte[] Bo = System.Text.UTF8Encoding.UTF8.GetBytes(submitData.XML);
            //        //XElement xemeBoObject = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(Bo)));
            //        //string strSystemCode = (from item in xemeBoObject.Descendants("Name") select item).FirstOrDefault().Value;


            //        ModelInfo modelinfo = FlowBLL.GetSysCodeByModelCode(submitData.ModelCode);
            //        FlowEngine.FlowConsultatiClose(modelinfo.SysCode, flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID, flowConsultation.REPLYUSERID);

            //        FlowBLL bll = new FlowBLL();
            //        bll.ReplyConsultation(flowConsultation);
            //    }
            //    catch (Exception ex)
            //    {
            //        Tracer.Debug("ReplyConsultation: -" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "-" + ex.InnerException + ex.Message);
            //        throw ex;
            //    }
            //}
            #endregion
        }
        #endregion

   
        #region 流程与审批
        public DataResult SubimtFlow(SubmitData submitData)
        {
            SMT.FlowWFService.NewFlow.FlowService FlowSv = new SMT.FlowWFService.NewFlow.FlowService();
            return FlowSv.SubimtFlow(submitData);
        }
        #endregion

        /// <summary>
        /// 获取用户有哪些未处理的单据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<FLOW_FLOWRECORDMASTER_T> GetFlowDataByUserID(string UserID)
        {
            try
            {
                FlowBLL Flow = new FlowBLL();
                return Flow.GetFlowDataByUserID(UserID);
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetFlowDataByUserID:" + UserID + " Ex:" + ex.Message);
                throw ex;
            }
        }
     

        #region 查询下一节点用户

        /// <summary>
        /// 启动与工作流程相同类型流程，查询对应节点用户
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="FlowGUID">待审批流GUID，新增时为空或者为StartFlow</param>
        /// <returns></returns>
        public DataResult GetAppUser(string CompanyID, string ModelCode, string FlowGUID, string xml)
        {
            SMT.FlowWFService.NewFlow.FlowService FlowSV = new SMT.FlowWFService.NewFlow.FlowService();
            return FlowSV.GetAppUser( CompanyID,  ModelCode,  FlowGUID,  xml);
        }

        #endregion

        private bool GetUser(string OptFlag, string Xoml, string Rules, string xml, ref DataResult DataResult)
        {          
            #region 旧代码
            try
            {
                WorkflowRuntime WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
                WorkflowInstance Instance = SMTWorkFlowManage.CreateWorkflowInstance(WfRuntime, Xoml, Rules);
                string strNextState = SMTWorkFlowManage.GetFlowNextStepRoles(WfRuntime, Instance, "StartFlow", xml);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }

            return true;
            #endregion
        }

        public string UpdateFlow(FLOW_FLOWRECORDDETAIL_T entity)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            return s2.UpdateFlow( entity);
            #region 旧代码
            //FlowBLL bll = new FlowBLL();
            //bll.UpdateFlowRecord(entity, "", "");

            //return "";
            #endregion
        }

        #region 查询流程信息
        /// <summary>
        /// 查询审批流程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<FLOW_FLOWRECORDDETAIL_T> GetFlowInfo(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {
                SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
                return s2.GetFlowInfo(  FormID,  FlowGUID,  CheckState,  Flag,  ModelCode,  CompanyID,  EditUserID);
            
            #region 旧代码
            //List<FLOW_FLOWRECORDDETAIL_T> list = new List<FLOW_FLOWRECORDDETAIL_T>();
            //try
            //{
            //    List<FlowType> FlowTypeList = new List<FlowWFService.FlowType>();
            //    FlowTypeList.Add(FlowType.Approval);
            //    FlowTypeList.Add(FlowType.Pending);
            //    //Debug.WriteLine("GetFlowInfo\n");
            //    //Debug.WriteLine(DateTime.Now.ToString());
            //    //Debug.WriteLine("\n");
            //    //有formid和modelcode不对返回数据量作限制，否则只返回前20条master数据
            //    if (!string.IsNullOrEmpty(FormID) && !string.IsNullOrEmpty(ModelCode))
            //    {
            //        list = FlowBLL.GetFlowInfoV(FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
            //    }
            //    else
            //    {
            //        Tracer.Debug("GetFlowInfoTop: formID:" + FormID + "--FlowGuid:" + FlowGUID
            //            + "--CheckState:" + CheckState + "--Flag:" + Flag + "--ModelCode:" + ModelCode + "--CompanyID:" + CompanyID + "--EditUserID:" + EditUserID);
            //        list = FlowBLL.GetFlowInfoTop(FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Tracer.Debug("GetFlowInfo: -" + FormID + "-" + ex.InnerException + "\n" + ex.Message);
            //    throw ex;

            //}

            //return list;
            #endregion
        }

        public List<FLOW_FLOWRECORDMASTER_T> GetFlowRecordMaster(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            return s2.GetFlowRecordMaster( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
            #region 旧代码
            //try
            //{
            //    return FlowBLL.GetFlowRecordMaster(FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
            //}
            //catch (Exception ex)
            //{
            //    Tracer.Debug("GetFlowRecordMaster: -" + ex.InnerException + ex.Message);
            //    throw ex;
            //}
            #endregion
            
        }

        /// <summary>
        /// 查询任务信息
        /// </summary>
        /// <param name="FormID"></param>
        /// <param name="FlowGUID"></param>
        /// <param name="CheckState"></param>
        /// <param name="Flag"></param>
        /// <param name="ModelCode"></param>
        /// <param name="CompanyID"></param>
        /// <param name="EditUserID"></param>
        /// <returns></returns>
        public List<TaskInfo> GetTaskInfo(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            return s2.GetTaskInfo( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
            #region 旧代码
            //return FlowBLL.GetTaskInfo(FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
            #endregion
        }
        #endregion

        #region "查询待审核单据"
        /// <summary>
        /// 根据模块代码和用户id查询待审核单据
        /// </summary>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="EditUserID">用户id</param>
        /// <returns>Formids</returns>
        public List<string> GetWaitingApprovalForm(string ModelCode, string EditUserID)
        {
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            return s2.GetWaitingApprovalForm(ModelCode, EditUserID);
        }
        #endregion

        public string GetFlowDefine(SubmitData ApprovalData)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            return s2.GetFlowDefine( ApprovalData);
            #region 旧代码
            //FlowBLL Flow = new FlowBLL();
            //return Flow.GetFlowDefine(ApprovalData);
            #endregion
           
        }

        /// <summary>
        /// 对外接口：根据我的单据ID获取记录实体
        /// </summary>
        /// <param name="personalrecordid">我的单据ID</param>
        /// <returns></returns>
        public T_WF_PERSONALRECORD GetPersonalRecordByID(string personalrecordid)
        {
            SMT.FlowWFService.NewFlow.FlowService s2 = new SMT.FlowWFService.NewFlow.FlowService();
            return s2.GetPersonalRecordByID(personalrecordid);
        }

        public string IsExistFlowDataByUserID(string UserID, string PostID)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            try
            {
                FlowBLL Flow = new FlowBLL();
                return Flow.IsExistFlowDataByUserID( UserID, PostID);
            }
            catch (Exception ex)
            {
                Tracer.Debug("IsExistFlowDataByUserID:" + UserID + " Ex:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 根据流程ID获取流程的所有分支
        /// </summary>
        /// <param name="FlowID"></param>
        /// <returns></returns>
        public List<string> GetFlowBranch(string FlowID)
        {
            SMT.FlowWFService.NewFlow.FlowService Flow = new SMT.FlowWFService.NewFlow.FlowService();
            return Flow.GetFlowBranch(FlowID);
        }
        /// <summary>
        /// 判断是否可能用自选流程或提单人可以撒回流程
        /// string[0]=1 可以用自选流程
        /// string[1]=1 提交人可以撒回流程
        /// </summary>
        /// <param name="modelcode">模块代码</param>
        /// <param name="companyid">公司ID</param>       
        public string[] IsFreeFlowAndIsCancel(string modelcode, string companyid)
        {
            SMT.FlowWFService.NewFlow.FlowService Flow = new SMT.FlowWFService.NewFlow.FlowService();
            return Flow.IsFreeFlowAndIsCancel(modelcode, companyid);
        }
        public List<FLOW_MODELFLOWRELATION_T> GetModelFlowRelationInfosListBySearch(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount)
        {
            SMT.FlowWFService.NewFlow.FlowService Flow = new SMT.FlowWFService.NewFlow.FlowService();
            return Flow.GetModelFlowRelationInfosListBySearch( pageIndex,  pageSize,  sort,  filterString, paras, ref  pageCount); 
        }
        /// <summary>
        /// 获取元数据
        /// </summary>
        /// <param name="formid"></param>
        /// <returns></returns>       
        public string GetMetadataByFormid(string formid)
        {
            SMT.FlowWFService.NewFlow.FlowService Flow = new SMT.FlowWFService.NewFlow.FlowService();            
            return Flow.GetMetadataByFormid(formid);
        }
        /// <summary>
        /// 更新元数据
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="xml"></param>
        /// <returns></returns>       
        public bool UpdateMetadataByFormid(string formid, string xml)
        {
            SMT.FlowWFService.NewFlow.FlowService Flow = new SMT.FlowWFService.NewFlow.FlowService();
            return Flow.UpdateMetadataByFormid(formid, xml);
        }

    }
}
