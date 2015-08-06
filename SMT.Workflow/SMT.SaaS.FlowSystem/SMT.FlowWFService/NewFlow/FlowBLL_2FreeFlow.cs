using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.FLOWDAL;
using System.Collections.ObjectModel;
using SMT.WFLib;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using SMT.FlowWFService.PublicClass;
using System.ServiceModel.Description;

using SMT.Foundation.Log;
using SMT.FLOWDAL.ADO;
using System.Data.OracleClient;

using System.Data.SqlClient;
using System.Configuration;
using SMT.Workflow.Common.Model.FlowEngine;
using System.Collections;
using SMT.Foundation.Core;
using System.Data;
using SMT.Workflow.SMTCache;
using SMT.Workflow.Common.Model;
using SMT.FlowWFService.XmlFlowManager;
using SMT.HRM.BLL;
using SMT.HRM.BLL.Permission;

namespace SMT.FlowWFService.NewFlow
{
    public partial class FlowBLL
    {

        #region 自选流程审批
        /// <summary>
        /// 自选流程审批（对服务操作）
        /// </summary>
        /// <param name="ApprovalData"></param>
        /// <param name="APPDataResult"></param>
        /// <returns></returns>
        public DataResult SubmitFreeFlow(SubmitData ApprovalData, DataResult APPDataResult, ref FlowUser user)
        {
            // WorkflowRuntime workflowRuntime = null;
            WorkflowInstance instance = null;
            FLOW_FLOWRECORDDETAIL_T entity = new FLOW_FLOWRECORDDETAIL_T();
            entity.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();
            UserInfo AppUser = new UserInfo(); //下一审核人
            UserInfo AgentAppUser = new UserInfo(); //代理下一审核人
            try
            {
                entity.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT = ApprovalData.XML;
                entity.FLOW_FLOWRECORDMASTER_T.FORMID = ApprovalData.FormID;
                entity.FLOW_FLOWRECORDMASTER_T.MODELCODE = ApprovalData.ModelCode;

                // entity.FLOWRECORDDETAILID = FlowGUID;
                entity.CREATECOMPANYID = ApprovalData.ApprovalUser.CompanyID;
                entity.CREATEDEPARTMENTID = ApprovalData.ApprovalUser.DepartmentID;
                entity.CREATEPOSTID = ApprovalData.ApprovalUser.PostID;
                entity.CREATEUSERID = ApprovalData.ApprovalUser.UserID;
                entity.CREATEUSERNAME = ApprovalData.ApprovalUser.UserName;
                entity.FLOW_FLOWRECORDMASTER_T.FLOWTYPE = ((int)ApprovalData.FlowType).ToString();
                entity.FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE = ((int)ApprovalData.FlowSelectType).ToString();
                entity.FLOW_FLOWRECORDMASTER_T.FLOWCODE = "FreeFlow";
                workflowRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(true);
                instance = SMTWorkFlowManage.CreateFreeWorkflowInstance(workflowRuntime, "FreeFlow.xml");//自选流程使用
                user.TrackingMessage += "自选流程使用 AddFreeFlow(try)创建工作流实例完成 ID=" + instance.InstanceId;
                entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID = instance.InstanceId.ToString();


                //下一审核人赋值
                AppUser = ApprovalData.NextApprovalUser;

                APPDataResult.RunTime += "---DoFlowStart:" + DateTime.Now.ToString();
                ApprovalData.NextStateCode = "Approval";
                AgentAppUser = GetAgentUserInfo(ApprovalData.ModelCode, AppUser.UserID); //查询是否启用了代理人（对服务操作）
                if (AgentAppUser != null)
                {
                    Tracer.Debug("查询 启用了代理人 FormID=" + user.FormID + " UserID=" + AgentAppUser.UserID);
                }
                else
                {
                    Tracer.Debug("查询 没有启用了代理人 FormID=" + user.FormID + "  AgentAppUser=null");
                }
                APPDataResult = DoFlowRecord(workflowRuntime, instance, entity, ApprovalData.NextStateCode, AppUser, AgentAppUser, ApprovalData.SubmitFlag, ApprovalData.FlowType, ref user); //处理流程数据
                APPDataResult.AgentUserInfo = AgentAppUser;
                APPDataResult.RunTime += "---DoFlowEnd:" + DateTime.Now.ToString();
                Tracer.Debug("Formid=" + ApprovalData.FormID + ";自选流程工作流实例ID:" + instance.InstanceId.ToString());
                return APPDataResult;
            }
            catch (Exception e)
            {
                user.ErrorMsg += "自选流程审批出错 FormID=" + ApprovalData.FormID + ";异常信息:\r\n" + e.ToString() + "\r\n";
                Tracer.Debug("自选流程审批出错 FormID=" + ApprovalData.FormID + ";跟踪信息:\r\n" + user.TrackingMessage + "异常信息:\r\n" + e.ToString());
                throw new Exception("自选流程审批出错,请联系管理员! \r\n FormID=" + user.FormID + "");
            }
            finally
            {
                AppUser = null;
                AgentAppUser = null;
                entity = null;
                instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(workflowRuntime);
            }
        }


        /// <summary>
        /// 自选流程（对数据库操作、对服务操作）
        /// </summary>
        /// <param name="ApprovalData"></param>
        /// <param name="APPDataResult"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public DataResult ApprovalFreeFlow(SubmitData ApprovalData, DataResult APPDataResult, List<FLOW_FLOWRECORDDETAIL_T> fd, ref FlowUser user)
        {
            // WorkflowRuntime workflowRuntime = null;
            WorkflowInstance instance = null;
            FLOW_FLOWRECORDDETAIL_T entity = new FLOW_FLOWRECORDDETAIL_T();
            entity.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();
            UserInfo AppUser = new UserInfo(); //下一审核人
            UserInfo AgentAppUser = new UserInfo(); //代理下一审核人
            try
            {
                APPDataResult.RunTime += "---GetAppInfoStart:" + DateTime.Now.ToString();
                List<FLOW_FLOWRECORDDETAIL_T> tmp = fd.Where(c => (c.EDITUSERID == ApprovalData.ApprovalUser.UserID || c.AGENTUSERID == ApprovalData.ApprovalUser.UserID) && c.FLAG == "0").ToList();
                APPDataResult.RunTime += "---GetAppInfoEnd:" + DateTime.Now.ToString();
                if (tmp == null)
                {
                    APPDataResult.FlowResult = FlowResult.FAIL;
                    APPDataResult.Err = "没有找到待审核信息";
                    user.TrackingMessage += "没有找到待审核信息\r\n";
                    // DataResult.UserInfo = null;
                    return APPDataResult;
                }

                entity = tmp[0];
                entity.EDITDATE = DateTime.Now;  //审批时间

                if (entity.AGENTUSERID == ApprovalData.ApprovalUser.UserID)
                {
                    entity.AGENTEDITDATE = entity.EDITDATE;  //代理审批时审批时间与代理审批时间到致
                }
                //entity.EditUserID = AppUserId;
                entity.CONTENT = ApprovalData.ApprovalContent;
                entity.CHECKSTATE = ((int)ApprovalData.ApprovalResult).ToString();

                try
                {
                    workflowRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(true);
                    Tracer.Debug("Formid=" + ApprovalData.FormID + ";开始 审核获取[自选流程]工作流实例ID=" + tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID);
                    instance = SMTWorkFlowManage.GetWorkflowInstance(workflowRuntime, tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID);// workflowRuntime.GetWorkflow(new Guid(tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID));
                    Tracer.Debug("Formid=" + ApprovalData.FormID + ";完成 审核获取[自选流程]工作流实例ID=" + instance.InstanceId.ToString());
                }
                catch
                {
                    Tracer.Debug("Formid=" + ApprovalData.FormID + ";完成 审核获取[自选流程]工作流实例 出错,需要重新构造工作流程实例,原来的实例ID=" + tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID);
                    workflowRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(true);
                    instance = SMTWorkFlowManage.CreateFreeWorkflowInstance(workflowRuntime, "FreeFlow.xml");//自选流程使用 
                    tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID = instance.InstanceId.ToString();
                    Tracer.Debug("Formid=" + ApprovalData.FormID + ";完成 重新构造[自选流程]工作流程实例,新的实例ID=" + tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID);
                }
                //不同意状态处理
                if (ApprovalData.ApprovalResult == ApprovalResult.NoPass)
                {
                    instance.Terminate("0");
                    user.TrackingMessage += "终审不通过,中止流程 FORMID=" + user.FormID;
                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "3"; //设为终审不通过
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = ApprovalData.ApprovalUser.UserID;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = ApprovalData.ApprovalUser.UserName;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now;
                    UpdateFlowDetailRecord(entity, ApprovalData.NextStateCode, ApprovalData.NextApprovalUser.UserID);
                    FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);//对数据库操作
                    APPDataResult.CheckState = "3";//
                    user.TrackingMessage += "终审不通过,设置状态 CheckState=3;FORMID=" + user.FormID;
                    APPDataResult.FlowResult = FlowResult.END;
                    // DataResult.UserInfo = null;
                    // return DataResult;

                }
                else
                {


                    //下一审核人赋值
                    if (ApprovalData.NextApprovalUser != null && !string.IsNullOrEmpty(ApprovalData.NextApprovalUser.UserID))
                    {
                        AppUser = ApprovalData.NextApprovalUser;
                    }
                    else
                    {
                        AppUser = ApprovalData.ApprovalUser;//如果没有下一审核人，下一审核人就是当前的审核人
                    }
                    user.TrackingMessage += "选择了下一个审核人 AppUser=" + AppUser.UserID + ";FORMID=" + user.FormID;
                    AgentAppUser = GetAgentUserInfo(ApprovalData.ModelCode, AppUser.UserID); //查询是否启用了代理人（对服务操作）
                    user.TrackingMessage += "查询是否启用了代理人 AppUser=" + AppUser.UserID + ";FORMID=" + user.FormID;
                    FlowDataType.FlowData FlowData = new FlowDataType.FlowData();
                    FlowData.xml = ApprovalData.XML;



                    //workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e)
                    //{//完成工作流实例

                    //    instance = null;

                    //};

                    ApprovalData.NextStateCode = ApprovalData.NextStateCode == "EndFlow" ? "EndFlow" : "Approval";
                    APPDataResult.RunTime += "---DoFlowStart:" + DateTime.Now.ToString();

                    APPDataResult = DoFlowRecord(workflowRuntime, instance, entity, ApprovalData.NextStateCode, AppUser, AgentAppUser, ApprovalData.SubmitFlag, ApprovalData.FlowType, ref user); //处理流程数据
                    APPDataResult.AgentUserInfo = AgentAppUser;
                    APPDataResult.RunTime += "---DoFlowEnd:" + DateTime.Now.ToString();
                    if (ApprovalData.NextStateCode == "EndFlow")
                    {
                        //ManualWorkflowSchedulerService scheduleService = workflowRuntime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;
                        //scheduleService.RunWorkflow(instance.InstanceId);

                        //workflowRuntime.GetService<FlowEvent>().OnDoFlow(instance.InstanceId, FlowData); //激发流程引擎执行到一下流程
                        //scheduleService.RunWorkflow(instance.InstanceId);
                        //System.Threading.Thread.Sleep(1000);
                    }



                }
                return APPDataResult;
            }
            catch (Exception e)
            {
                Tracer.Debug("自选流程提交出错:FORMID=" + user.FormID + "\r\n 异常信息:" + e.ToString());
                throw new Exception("自选流程提交出错,请联系管理员! \r\n FormID=" + user.FormID + "");
            }
            finally
            {
                AppUser = null;
                AgentAppUser = null;
                entity = null;
                instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(workflowRuntime);
            }
        }
        #endregion
    }
}
