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
        #region 固定流程审批

        #region 新建流程
        /// <summary>
        /// 新增流程(对数据库操作)
        /// </summary>
        /// <param name="ApprovalData"></param>
        /// <param name="APPDataResult"></param>
        /// <returns></returns>    
        public DataResult SubmitFlow(SubmitData submitData, DataResult dataResult, ref FlowUser user)
        {
            WorkflowInstance instance = null;
            try
            {
                #region 获取定义的流程
                user.TrackingMessage += "获取定义的流程.GetFlowByModelName:submitData.ApprovalUser.DepartmentID=" + submitData.ApprovalUser.DepartmentID + ";OrgType='" + ((int)submitData.FlowType).ToString() + "'";
                List<FLOW_MODELFLOWRELATION_T> MODELFLOWRELATION = GetFlowByModelName(submitData.ApprovalUser.CompanyID, submitData.ApprovalUser.DepartmentID, submitData.ModelCode, ((int)submitData.FlowType).ToString(), ref user);//对数据库操作
                if (MODELFLOWRELATION == null || MODELFLOWRELATION.Count == 0)
                {
                    dataResult.FlowResult = FlowResult.FAIL;
                    dataResult.Err = "没有找到可使用的流程";
                    if (submitData.ApprovalUser.CompanyID == user.CompayID && submitData.ApprovalUser.DepartmentID == user.DepartmentID)
                    {
                        dataResult.Err = "没有找到公司[ " + user.CompayName + " ]下部门[ " + user.DepartmentName + " ]的匹配流程返回";
                    }
                    else
                    {
                        dataResult.Err = "没有找到公司[ " + user.CompayName + " ]的可使用匹配流程";

                    }
                    return dataResult;
                }

                #endregion
                FLOW_MODELFLOWRELATION_T flowRelation = MODELFLOWRELATION[0];//只取其中一条流程
                FLOW_FLOWDEFINE_T flowDefine = flowRelation.FLOW_FLOWDEFINE_T;
                user.FlowCode = flowDefine.FLOWCODE;//流程代码
                user.FlowName = flowDefine.DESCRIPTION;//流程名称
                if (flowDefine.RULES != null && flowDefine.RULES.Trim() == "")
                {
                    flowDefine.RULES = null;
                }
                if (string.IsNullOrEmpty(flowDefine.LAYOUT))
                {
                    dataResult.Err = "公司[ " + user.CompayName + " ]的匹配流程定义为空！";
                    return dataResult;
                }
                workflowRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(true);
                instance = SMTWorkFlowManage.CreateWorkflowInstance(workflowRuntime, flowDefine.LAYOUT, flowDefine.RULES);
                Tracer.Debug("新增 FormID=" + user.FormID + " 流程名称＝" + flowDefine.DESCRIPTION + "(" + flowDefine.FLOWCODE + ") 提交人＝" + user.UserName + " 公司名称＝" + user.CompayName + " 部门名称＝" + user.DepartmentName + " 岗位名称＝" + user.PostName + "  WorkflowInstance ID=" + instance.InstanceId.ToString());

                #region master赋值
                FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();
                master.INSTANCEID = instance.InstanceId.ToString();
                master.BUSINESSOBJECT = submitData.XML;
                master.FORMID = submitData.FormID;
                master.MODELCODE = submitData.ModelCode;
                master.ACTIVEROLE = FlowUtility.GetActiveRlue(flowDefine.LAYOUT);//此字段可以保存当前节点ActivitID
                master.FLOWTYPE = ((int)submitData.FlowType).ToString();
                master.FLOWSELECTTYPE = ((int)submitData.FlowSelectType).ToString();
                master.FLOWCODE = flowDefine.FLOWCODE;
                #endregion

                #region 获取下一状态数据
                user.TrackingMessage += "FORMID=" + user.FormID + "获取下一状态数据(开始)";


                TmFlowToNextStep(submitData.ApprovalUser.CompanyID, flowDefine.LAYOUT, flowDefine.RULES, master.ACTIVEROLE, submitData.XML, submitData.ApprovalUser.UserID, submitData.ApprovalUser.PostID, submitData.FlowType, ref dataResult, ref user);


                Tracer.Debug("FormID=" + user.FormID + " 获取下一状态数据! dataResult.FlowResult=" + dataResult.FlowResult.ToString());
                user.TrackingMessage += "FORMID=" + user.FormID + "获取下一状态数据(结束)";
                if (dataResult.FlowResult == FlowResult.FAIL)
                {
                    return dataResult;
                }
                submitData.NextStateCode = dataResult.AppState;
                if (dataResult.IsCountersign)
                {
                    #region 检查会签是角色是否有审核人员
                    #region 记录日志
                    if (submitData.DictCounterUser != null)
                    {
                        Tracer.Debug("FormID=" + user.FormID + " submitData.DictCounterUser=" + submitData.DictCounterUser.Count.ToString());
                    }
                    if (dataResult.DictCounterUser != null)
                    {
                        Tracer.Debug("FormID=" + user.FormID + "  dataResult.DictCounterUser=" + dataResult.DictCounterUser.Count.ToString());
                    }
                    #endregion

                    if (dataResult.FlowResult == FlowResult.Countersign)
                    {
                        if (submitData.DictCounterUser == null || submitData.DictCounterUser.Keys.Count == 0)
                        {
                            Tracer.Debug("FormID=" + user.FormID + " submitData.DictCounterUser 会签角色里没有发现有审核人员,所以返回!");
                            return dataResult;
                        }
                    }
                    else
                    {
                        if (submitData.DictCounterUser == null || submitData.DictCounterUser.Keys.Count == 0)
                        {
                            submitData.DictCounterUser = dataResult.DictCounterUser;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 检查非会签角色里是否有审核人员
                    if (dataResult.FlowResult == FlowResult.MULTIUSER)
                    {
                        Tracer.Debug("FormID=" + user.FormID + " 发现有多个审核人员!");
                        if (submitData.NextApprovalUser == null || (FlowUtility.GetString(submitData.NextApprovalUser.UserID) == "" || FlowUtility.GetString(submitData.NextApprovalUser.UserName) == ""))
                        {
                            Tracer.Debug("FormID=" + user.FormID + " 发现有多个审核人员!但下一审核人为空，所以返回选择审核人！");
                            return dataResult;
                        }
                        else
                        {
                            Tracer.Debug("FormID=" + user.FormID + " 发现有多个审核人员,但发现下一审核人不为空 usrid=" + (FlowUtility.GetString(submitData.NextApprovalUser.UserID) + " 姓名=" + FlowUtility.GetString(submitData.NextApprovalUser.UserName)));
                        }
                    }
                    else
                    {
                        if (submitData.DictCounterUser == null || submitData.DictCounterUser.Keys.Count == 0)
                        {
                            submitData.NextApprovalUser = dataResult.UserInfo[0];
                        }
                    }
                    #endregion
                }

                #endregion

                #region 流程明细赋值
                //当提交人为空时，创建人变成单据所属人，如果不为空，则创建人保存为系统登录人;创建公司，部门，岗位，仍然保存单据所属人的公司，部门，岗位
                FLOW_FLOWRECORDDETAIL_T entity = new FLOW_FLOWRECORDDETAIL_T();
                entity.FLOW_FLOWRECORDMASTER_T = master;
                entity.CREATECOMPANYID = submitData.ApprovalUser.CompanyID;
                entity.CREATEDEPARTMENTID = submitData.ApprovalUser.DepartmentID;
                entity.CREATEPOSTID = submitData.ApprovalUser.PostID;
                entity.CREATEUSERID = string.IsNullOrEmpty(submitData.SumbitUserID) ? submitData.ApprovalUser.UserID : submitData.SumbitUserID;
                entity.CREATEUSERNAME = string.IsNullOrEmpty(submitData.SumbitUserName) ? submitData.ApprovalUser.UserName : submitData.SumbitUserName;
                #endregion

                #region 处理kpi时间
                user.TrackingMessage += " 处理kpi时间\r\n";
                string KPITime = "";
                #region 加入缓存
                string pscResult = CacheProvider.GetCache<string>(flowRelation.MODELFLOWRELATIONID);
                if (string.IsNullOrEmpty(pscResult))
                {
                    //ken 暂时屏蔽kpi
                    //PerformanceServiceWS.PerformanceServiceClient psc = new PerformanceServiceWS.PerformanceServiceClient();
                    //pscResult = psc.GetKPIPointsByBusinessCode(flowRelation.MODELFLOWRELATIONID);//调用服务
                    //CacheProvider.Add<string>(flowRelation.MODELFLOWRELATIONID, pscResult);
                    //psc.Close();
                }
                #endregion
                //PerformanceServiceWS.PerformanceServiceClient psc = new PerformanceServiceWS.PerformanceServiceClient();
                //string pscResult = psc.GetKPIPointsByBusinessCode(flowRelation.MODELFLOWRELATIONID);//调用服务
                //psc.Close();
                if (!string.IsNullOrEmpty(pscResult))
                {
                    XElement xe = XElement.Parse(pscResult);
                    Func<XElement, bool> f = (x) =>
                    {
                        XAttribute xid = x.Attribute("id");
                        XAttribute xvalue = x.Attribute("value");
                        if (xid == null || xvalue == null)
                            return false;
                        else
                        {
                            if (xid.Value == dataResult.AppState)
                                return true;
                            else return false;
                        }
                    };
                    XElement FlowNode = xe.Elements("FlowNode").FirstOrDefault(f);
                    if (FlowNode != null)
                    {
                        KPITime = FlowNode.Attribute("value").Value;
                    }
                }

                dataResult.KPITime = KPITime;
                master.KPITIMEXML = pscResult;
                user.TrackingMessage += " 处理kpi时间完成\r\n";

                #endregion

                FlowDataType.FlowData FlowData = new FlowDataType.FlowData();
                FlowData.xml = submitData.XML;

                if (!dataResult.IsCountersign)
                {
                    #region  确定非会签的下一个审核人
                    UserInfo AppUser = new UserInfo(); //下一审核人
                    AppUser = submitData.NextApprovalUser;
                    dataResult.UserInfo.Clear();
                    dataResult.UserInfo.Add(AppUser);
                    //暂时不处理代理，ken2015-7-17
                    UserInfo AgentAppUser = null;// GetAgentUserInfo(submitData.ModelCode, AppUser.UserID);//查询是否启用了代理人                    
                    dataResult = AddOrUpdateFlowRecord(workflowRuntime, instance, entity, submitData.NextStateCode, AppUser, AgentAppUser, submitData.SubmitFlag, submitData.FlowType, ref user); //处理流程数据
                    dataResult.IsCountersign = false;
                    dataResult.AgentUserInfo = AgentAppUser;
                    #endregion
                }
                else
                {
                    user.TrackingMessage += " 会签\r\n";
                    #region  确定会签角色里的审核人员
                    //Tracer.Debug("-----DoFlowRecord_Add:" + DateTime.Now.ToString()+"\n");
                    dataResult.DictCounterUser = submitData.DictCounterUser;
                    Dictionary<UserInfo, UserInfo> dictAgentUserInfo = GetAgentUserInfo2(submitData.ModelCode, submitData.DictCounterUser);
                    dataResult = DoFlowRecord_Add(workflowRuntime, instance, entity, submitData.NextStateCode, submitData.DictCounterUser, dictAgentUserInfo, submitData.SubmitFlag, submitData.FlowType); //处理流程数据
                    //Tracer.Debug("-----DoFlowRecord_AddEnd:" + DateTime.Now.ToString()+"\n");
                    dataResult.IsCountersign = true;
                    dataResult.DictAgentUserInfo = dictAgentUserInfo;
                    #endregion
                    user.TrackingMessage += "会签完成\r\n";
                }
                user.TrackingMessage += "激发流程引擎执行到一下流程\r\n";
                #region 激发流程引擎执行到一下流程
                // string ss = "";
                // int n = 0;
                // //StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(workflowRuntime, instance.InstanceId);
                // //ManualWorkflowSchedulerService scheduleService = workflowRuntime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;
                //if (dataResult.AppState == null || dataResult.AppState == "")
                // {
                //     user.TrackingMessage += " workflowRuntime.GetService<FlowEvent>()\r\n";
                //     //scheduleService.RunWorkflow(workflowinstance.InstanceId);
                //     //workflowRuntime.GetService<FlowEvent>().OnDoFlow(instance.InstanceId, FlowData); //激发流程引擎执行到一下流程
                //     //scheduleService.RunWorkflow(workflowinstance.InstanceId);
                //     user.TrackingMessage += " workflowRuntime.GetService<FlowEvent>()完成\r\n";
                // }
                // else
                // {
                //     //scheduleService.RunWorkflow(workflowinstance.InstanceId);
                //     //workflowinstance.SetState(dataResult.AppState); //流程跳转到指定节点
                // }

                #endregion
                //user.TrackingMessage += "激发流程引擎执行到一下流程完成\r\n";
                //user.TrackingMessage += "System.Threading.Thread.Sleep(1000)\r\n";
                //System.Threading.Thread.Sleep(1000);//当前用到
                dataResult.ModelFlowRelationID = flowRelation.MODELFLOWRELATIONID; //返回关联ID
                dataResult.KPITime = KPITime;
                //dataResult.CanSendMessage = true;
                if (submitData.FlowType == FlowType.Task)
                {
                    dataResult.SubModelCode = FlowUtility.GetSubModelCode(master.ACTIVEROLE, dataResult.AppState); //返回下一子模块代码
                }
                //user.TrackingMessage += "System.Threading.Thread.Sleep(1000)完成\r\n";
                return dataResult;
            }

            catch (Exception e)
            {
                user.ErrorMsg += "新增流程出错 FormID=" + user.FormID + " CompayName=" + user.CompayName + "FlowName=" + user.FlowName + "异常信息:\r\n" + e.ToString() + "\r\n";
                Tracer.Debug("FormID=" + user.FormID + " CompayName=" + user.CompayName + "FlowName=" + user.FlowName + " 新增流程出错,异常信息:\r\n" + e.ToString());
                throw new Exception("FormID=" + user.FormID + " 时间：" + DateTime.Now.ToString() + " 新增流程出错,请联系管理员! ");
            }
            finally
            {
                instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(workflowRuntime);
            }


        }



        #endregion

        #region 审批流程

        public DataResult CancelFlow(SubmitData submitData, DataResult dataResult, List<FLOW_FLOWRECORDDETAIL_T> fd)
        {
            //WorkflowRuntime workflowRuntime = null;
            WorkflowInstance instance = null;

            FLOW_FLOWRECORDDETAIL_T entity = new FLOW_FLOWRECORDDETAIL_T();
            #region
            entity.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
            entity.CREATECOMPANYID = submitData.ApprovalUser.CompanyID;
            entity.CREATEDEPARTMENTID = submitData.ApprovalUser.DepartmentID;
            entity.CREATEPOSTID = submitData.ApprovalUser.PostID;
            entity.CREATEUSERID = submitData.ApprovalUser.UserID;
            entity.CREATEUSERNAME = submitData.ApprovalUser.UserName;
            entity.EDITDATE = DateTime.Now;
            entity.CONTENT = submitData.ApprovalContent;
            entity.CHECKSTATE = "9";
            entity.STATECODE = "Cancel";
            entity.FLAG = "1";
            entity.PARENTSTATEID = entity.FLOWRECORDDETAILID;

            entity.CREATEDATE = DateTime.Now;
            entity.EDITDATE = DateTime.Now;
            entity.EDITUSERID = entity.CREATEUSERID;
            entity.EDITUSERNAME = entity.CREATEUSERNAME;
            entity.EDITCOMPANYID = entity.CREATECOMPANYID;
            entity.EDITDEPARTMENTID = entity.CREATEDEPARTMENTID;
            entity.EDITPOSTID = entity.CREATEPOSTID;

            entity.FLOW_FLOWRECORDMASTER_T = fd[0].FLOW_FLOWRECORDMASTER_T;
            entity.FLOW_FLOWRECORDMASTER_T.FLOW_FLOWRECORDDETAIL_T.Add(entity);
            entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "9"; //设为撤销
            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = submitData.ApprovalUser.UserID;
            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = submitData.ApprovalUser.UserName;
            entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now;
            #endregion

            workflowRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(true);
            Tracer.Debug("CancelFlow从持久化库在恢复创建工作流实例ID=" + entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID);
            instance = SMTWorkFlowManage.GetWorkflowInstance(workflowRuntime, entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID);
            instance.Terminate("0");
            FLOW_FLOWRECORDDETAIL_TDAL Dal = new FLOW_FLOWRECORDDETAIL_TDAL();
            entity.FLAG = "1";
            //Dal.AddFlowRecord(entity);
            FLOW_FLOWRECORDDETAIL_TDAL.Add(entity);
            FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);
            fd.Where(detail => detail.FLAG == "0").ToList().ForEach(item =>
            {
                //Dal.Delete(item);
                //Dal.DeleteFlowRecord(item);
                FLOW_FLOWRECORDDETAIL_TDAL.Delete(item);
            });
            dataResult.CheckState = "9";//
            dataResult.FlowResult = FlowResult.SUCCESS;
            return dataResult;
        }

        //下一审核人提交审核时调用方法
        /// <summary>
        /// 固定流程:下一审核人提交审核时调用方法
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="dataResult"></param>
        /// <param name="listDetail"></param>
        /// <returns></returns>
        public DataResult ApprovalFlow(SubmitData submitData, DataResult dataResult, List<FLOW_FLOWRECORDDETAIL_T> listDetail, ref FlowUser user, ref string msg)
        {
            if (submitData.NextApprovalUser == null)
            {
                submitData.NextApprovalUser = new UserInfo();
            }
            ///针对会签，该次审核成功后是否跳转至下一状态
            bool isGotoNextState = true;
            // WorkflowRuntime workflowRuntime = null;
            WorkflowInstance instance = null;
            FLOW_FLOWRECORDDETAIL_T entity = new FLOW_FLOWRECORDDETAIL_T();
            entity.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();

            try
            {
                #region Entity赋值
                List<FLOW_FLOWRECORDDETAIL_T> tmpEntity = listDetail.Where(c => (c.EDITUSERID == submitData.ApprovalUser.UserID || c.AGENTUSERID == submitData.ApprovalUser.UserID) && c.FLAG == "0").ToList();
                if (tmpEntity == null)
                {
                    dataResult.FlowResult = FlowResult.FAIL;
                    dataResult.Err = "没有找到待审核信息 FORMID=" + user.FormID + "\r\n";
                    user.TrackingMessage += "没有找到待审核信息 FORMID=" + user.FormID + "\r\n";

                    return dataResult;
                }
                entity = tmpEntity[0];
                entity.EDITDATE = DateTime.Now;  //审批时间
                if (entity.AGENTUSERID == submitData.ApprovalUser.UserID)
                {
                    entity.AGENTEDITDATE = entity.EDITDATE;  //代理审批时审批时间与代理审批时间到致
                }

                entity.CONTENT = submitData.ApprovalContent;
                entity.CHECKSTATE = ((int)submitData.ApprovalResult).ToString();
                #endregion
                #region backup persisted workflow instanceState
                if (!string.IsNullOrEmpty(entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID))
                {
                    String connStringPersistence = ConfigurationManager.ConnectionStrings["//OracleConnection"].ConnectionString;//Data Source=172.30.50.110;Initial Catalog=WorkflowPersistence;Persist Security Info=True;User ID=sa;Password=fbaz2012;MultipleActiveResultSets=True";
                    string sql = string.Format("select * from instance_state where instance_id='{0}'", entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID);
                    DataTable dt = dao.GetDataTable(sql);
                    DataSet Dataset = new DataSet();
                    Dataset.Tables.Add(dt);
                    user.InstanceState = Dataset;
                }
                #endregion
                //workflowRuntime.StartRuntime();
                user.TrackingMessage += "创建工作流运行时开始 FORMID=" + user.FormID + "\r\n";
                workflowRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(true);
                try
                {
                    instance = SMTWorkFlowManage.GetWorkflowInstance(workflowRuntime, entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID);// workflowRuntime.GetWorkflow(new Guid(tmp[0].FLOW_FLOWRECORDMASTER_T.INSTANCEID));
                    user.TrackingMessage += "FormID=" + submitData.FormID + ";ApprovalFlow2(try)从持久化库[ 完成 ]恢复创建工作流实例ID=" + entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID + "\r\n";
                    Tracer.Debug("审核 FormID=" + user.FormID + " WorkflowInstance ID=" + instance.InstanceId.ToString());

                }
                catch (Exception exGetWorkflowInstance)
                {
                    #region 重新创建新流程，将新流程设置为当前状态。
                    try
                    {
                        user.TrackingMessage += "FormID=" + submitData.FormID + ";从持久化恢复工作流失败 SMTWorkFlowManage.GetWorkflowInstance(" + workflowRuntime.Name + ", " + entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID + ");原因如下:\r\n" + exGetWorkflowInstance.ToString() + ";\r\n下面重新创建新流程，并将新流程设置为当前状态;\r\nGetFlowByModelName:submitData.ApprovalUser.DepartmentID=" + submitData.ApprovalUser.DepartmentID + ";OrgType='" + ((int)submitData.FlowType).ToString() + "'";

                        List<FLOW_MODELFLOWRELATION_T> MODELFLOWRELATION = GetFlowByModelName(entity.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID, entity.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID, submitData.ModelCode, ((int)submitData.FlowType).ToString(), ref  user);

                        FLOW_MODELFLOWRELATION_T flowRelation = MODELFLOWRELATION[0];
                        FLOW_FLOWDEFINE_T flowDefine = flowRelation.FLOW_FLOWDEFINE_T;
                        instance = SMTWorkFlowManage.CreateWorkflowInstance(workflowRuntime, flowDefine.LAYOUT, flowDefine.RULES);
                        user.TrackingMessage += "FormID=" + submitData.FormID + ";ApprovalFlow2(catch)完成重新创建工作流实例ID=" + instance.InstanceId + "\r\n";
                        //StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(workflowRuntime, instance.InstanceId);
                        //ManualWorkflowSchedulerService scheduleService = workflowRuntime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;
                        //scheduleService.RunWorkflow(workflowinstance.InstanceId);

                        //workflowinstance.SetState(entity.STATECODE);

                        //System.Threading.Thread.Sleep(1000); //commented by alan 2012/9/7
                        instance = SMTWorkFlowManage.GetWorkflowInstance(workflowRuntime, instance.InstanceId.ToString());
                        user.TrackingMessage += "FormID=" + submitData.FormID + ";ApprovalFlow2(catch)从持久化库再恢复刚才创建工作流实例ID=" + instance.InstanceId + "\r\n";

                        entity.FLOW_FLOWRECORDMASTER_T.INSTANCEID = instance.InstanceId.ToString();
                        //FLOW_FLOWRECORDDETAIL_TDAL.UpdateMasterINSTANCEID(entity.FLOW_FLOWRECORDMASTER_T);
                        FLOW_FLOWRECORDMASTER_TDAL.UpdateMasterINSTANCEID(entity.FLOW_FLOWRECORDMASTER_T);

                    }
                    catch (Exception exNewInstance)
                    {
                        user.ErrorMsg += "重新创建新流程，将新流程设置为当前状态失败:FormID=" + submitData.FormID + "异常信息：\r\n" + exNewInstance.Message + "\r\n";
                        Tracer.Debug("重新创建新流程，将新流程设置为当前状态失败:FormID=" + submitData.FormID + "FlowBLL->ApprovalFlow2" + exNewInstance.Message);
                        //Tracer.Debug("exNewInstance: -" + submitData.FormID + "--submitDataXML:" + submitData.XML + "-" + exNewInstance.InnerException + exNewInstance.Message);
                        throw new Exception("重新创建新流程，将新流程设置为当前状态失败,请联系管理!");
                    }
                    #endregion
                }
                user.TrackingMessage += "SMTWorkFlowManage.CreateWorkFlowRuntime(true)完成FORMID=" + user.FormID + " \r\n";


                #region 当前状态会签状态处理
                bool currentIsCountersign = false;
                string currentCountersignType = "0";

                FlowUtility.IsCountersign(entity.FLOW_FLOWRECORDMASTER_T.ACTIVEROLE, entity.STATECODE, ref currentIsCountersign, ref currentCountersignType);
                if (currentIsCountersign)
                {
                    user.TrackingMessage += "状态会签状态处理 FORMID=" + user.FormID + "  \r\n";
                    if (currentCountersignType == "1")//一人通过即所有通过，可以跳转至下一状态
                    {
                        isGotoNextState = true;
                    }
                    else
                    {
                        ///该审核是会签的最后的审核人
                        if (entity.FLOW_FLOWRECORDMASTER_T.FLOW_FLOWRECORDDETAIL_T.Count == 1)
                        {
                            isGotoNextState = true;
                        }
                        else
                        {
                            isGotoNextState = false;
                        }
                    }
                    user.TrackingMessage += "状态会签状态处理完成 FORMID=" + user.FormID + " \r\n";
                }
                #endregion
                //不同意状态处理
                if (submitData.ApprovalResult == ApprovalResult.NoPass)
                {
                    user.TrackingMessage += "审核不通过状态处理(开始) FORMID=" + user.FormID + " \r\n";
                    #region
                    instance.Terminate("0");
                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "3"; //设为终审不通过
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = submitData.ApprovalUser.UserID;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = submitData.ApprovalUser.UserName;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now;
                    user.TrackingMessage += "审核不通过【开始更新明细表】!FORMID=" + user.FormID;
                    //user.TrackingMessage += "entity.FLOWRECORDDETAILID" + entity.FLOWRECORDDETAILID + "\r\n";//
                    //user.TrackingMessage += "entity.STATECODE=" + entity.STATECODE + "\r\n";//
                    //user.TrackingMessage += "entity.PARENTSTATEID =" + entity.PARENTSTATEID + "\r\n";//
                    //user.TrackingMessage += "entity.CONTENT=" + entity.CONTENT + "\r\n";//
                    //user.TrackingMessage += "entity.CHECKSTATE=" + entity.CHECKSTATE + "\r\n";//同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8
                    //user.TrackingMessage += "entity.FLAG =" + entity.FLAG + "\r\n";//已审批：1，未审批:0
                    //user.TrackingMessage += " entity.CREATEUSERID =" + entity.CREATEUSERID + "\r\n";//
                    //user.TrackingMessage += "entity.CREATEUSERNAME=" + entity.CREATEUSERNAME + "\r\n";//
                    //user.TrackingMessage += " entity.CREATECOMPANYID=" + entity.CREATECOMPANYID + "\r\n";//
                    //user.TrackingMessage += " entity.CREATEDEPARTMENTID =" + entity.CREATEDEPARTMENTID + "\r\n";//
                    //user.TrackingMessage += "entity.CREATEPOSTID=" + entity.CREATEPOSTID + "\r\n";//
                    //user.TrackingMessage += "entity.CREATEDATE=" + entity.CREATEDATE + "\r\n";//
                    //user.TrackingMessage += " entity.EDITUSERID=" + entity.EDITUSERID + "\r\n";//
                    //user.TrackingMessage += "entity.EDITUSERNAME=" + entity.EDITUSERNAME + "\r\n";//
                    //user.TrackingMessage += " entity.EDITCOMPANYID =" + entity.EDITCOMPANYID + "\r\n";//
                    //user.TrackingMessage += " entity.EDITDEPARTMENTID=" + entity.EDITDEPARTMENTID + "\r\n";//
                    //user.TrackingMessage += "entity.EDITPOSTID=" + entity.EDITPOSTID + "\r\n";//
                    //user.TrackingMessage += "entity.EDITDATE=" + entity.EDITDATE + "\r\n";//
                    //user.TrackingMessage += "entity.AGENTUSERID =" + entity.AGENTUSERID + "\r\n";//
                    //user.TrackingMessage += " entity.AGENTERNAME=" + entity.AGENTERNAME + "\r\n";//
                    //user.TrackingMessage += "entity.AGENTEDITDATE=" + entity.AGENTEDITDATE + "\r\n";//
                    //user.TrackingMessage += "submitData.NextStateCode=" + submitData.NextStateCode + "\r\n";//
                    //user.TrackingMessage += "submitData.NextApprovalUser.UserID=" + submitData.NextApprovalUser.UserID + "\r\n";//
                    //user.TrackingMessage += " entity.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=" + entity.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "\r\n";//


                    UpdateFlowDetailRecord(entity, submitData.NextStateCode, submitData.NextApprovalUser.UserID);
                    user.TrackingMessage += "审核不通过【开始更新主表】!FORMID=" + user.FormID;
                    FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);
                    dataResult.CheckState = "3";//
                    dataResult.FlowResult = FlowResult.END;

                    if (currentIsCountersign)
                    {
                        #region 当前是会签状态，删除未审核记录
                        user.TrackingMessage += "审核不通过【当前是会签状态，删除未审核记录】!FORMID=" + user.FormID;
                        entity.FLOW_FLOWRECORDMASTER_T.FLOW_FLOWRECORDDETAIL_T
                                .Where(detail => detail.FLOWRECORDDETAILID != entity.FLOWRECORDDETAILID && detail.STATECODE == entity.STATECODE && detail.FLAG == "0")
                                .ToList().ForEach(item =>
                                {
                                    item.FLAG = "1";
                                    item.CHECKSTATE = "8";
                                    UpdateFlowRecord2(item);
                                });
                        #endregion
                    }

                    #endregion
                    user.TrackingMessage += "审核通过状态处理(完成) FORMID=" + user.FormID + " \r\n";

                }
                else
                {
                    if (!isGotoNextState)
                    {
                        user.TrackingMessage += "isGotoNextState开始 FORMID=" + user.FormID + " \r\n";
                        #region

                        UpdateFlowRecord2(entity);
                        dataResult.AppState = entity.STATECODE;
                        dataResult.FlowResult = FlowResult.SUCCESS;
                        dataResult.CheckState = "1";
                        #endregion
                        user.TrackingMessage += "isGotoNextState完成 FORMID=" + user.FormID + "\r\n";
                    }
                    else
                    {
                        user.TrackingMessage += "获取下一状态数据开始 FORMID=" + user.FormID + " \r\n";
                        #region 获取下一状态数据
                        List<string> User = new List<string>();
                        User.Add(entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERID);
                        User.Add(submitData.ApprovalUser.UserID);

                        List<string> tmpPostID = new List<string>();
                        tmpPostID.Add(entity.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID);
                        tmpPostID.Add(entity.EDITPOSTID);
                        GetUserByInstance2(entity.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID, workflowRuntime, instance, entity.FLOW_FLOWRECORDMASTER_T.ACTIVEROLE, submitData.XML, entity.STATECODE, User, tmpPostID, submitData.FlowType, ref dataResult, ref user);

                        if (dataResult.FlowResult == FlowResult.FAIL)
                        {
                            return dataResult;
                        }
                        submitData.NextStateCode = dataResult.AppState;
                        if (dataResult.IsCountersign)
                        {
                            #region
                            if (dataResult.FlowResult == FlowResult.Countersign)
                            {
                                if (submitData.DictCounterUser == null || submitData.DictCounterUser.Keys.Count == 0)
                                {
                                    return dataResult;
                                }
                            }
                            else
                            {
                                if (submitData.DictCounterUser == null || submitData.DictCounterUser.Keys.Count == 0)
                                {
                                    submitData.DictCounterUser = dataResult.DictCounterUser;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            if (dataResult.FlowResult == FlowResult.MULTIUSER)
                            {
                                if (submitData.NextApprovalUser == null || (FlowUtility.GetString(submitData.NextApprovalUser.UserID) == "" || FlowUtility.GetString(submitData.NextApprovalUser.UserName) == ""))
                                {
                                    return dataResult;
                                }
                            }
                            else
                            {
                                if (submitData.DictCounterUser == null || submitData.DictCounterUser.Keys.Count == 0)
                                {
                                    submitData.NextApprovalUser = dataResult.UserInfo[0];
                                }
                            }
                            #endregion
                        }

                        #endregion

                        user.TrackingMessage += "获取下一状态数据完成 FORMID=" + user.FormID + "\r\n";


                        user.TrackingMessage += "单据会签情况开始 FORMID=" + user.FormID + "\r\n";
                        #region 对于单会签情况，需要将其他审核人的审核设为会签通过状态

                        if (currentIsCountersign && currentCountersignType == "1")
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.FLOW_FLOWRECORDDETAIL_T
                               .Where(detail => detail.FLOWRECORDDETAILID != entity.FLOWRECORDDETAILID && detail.STATECODE == entity.STATECODE && detail.FLAG == "0")
                               .ToList().ForEach(item =>
                               {
                                   item.FLAG = "1";
                                   item.CHECKSTATE = "7";
                                   UpdateFlowRecord2(item);
                               });
                        }
                        #endregion
                        user.TrackingMessage += "单据会签情况完成 FORMID=" + user.FormID + "\r\n";

                        #region


                        FlowDataType.FlowData FlowData = new FlowDataType.FlowData();
                        FlowData.xml = submitData.XML;
                        //workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e)
                        //{
                        //    instance = null;

                        //};
                        user.TrackingMessage += "处理kpi 开始 FORMID=" + user.FormID + "\r\n";

                        #region 处理kpi时间
                        string KPITime = "";
                        //PerformanceServiceWS.PerformanceServiceClient psc = new PerformanceServiceWS.PerformanceServiceClient();
                        string pscResult = entity.FLOW_FLOWRECORDMASTER_T.KPITIMEXML;
                        //psc.Close();
                        if (!string.IsNullOrEmpty(pscResult))
                        {
                            XElement xe = XElement.Parse(pscResult);
                            Func<XElement, bool> f = (x) =>
                            {
                                XAttribute xid = x.Attribute("id");
                                XAttribute xvalue = x.Attribute("value");
                                if (xid == null || xvalue == null)
                                    return false;
                                else
                                {
                                    if (xid.Value == dataResult.AppState)
                                        return true;
                                    else return false;
                                }
                            };
                            XElement FlowNode = xe.Elements("FlowNode").FirstOrDefault(f);
                            if (FlowNode != null)
                            {
                                KPITime = FlowNode.Attribute("value").Value;
                            }
                        }
                        dataResult.KPITime = KPITime;

                        #endregion

                        user.TrackingMessage += "处理kpi 完成 FORMID=" + user.FormID + "\r\n";

                        if (!dataResult.IsCountersign)
                        {
                            user.TrackingMessage += "非会签 开始 FORMID=" + user.FormID + "\r\n";
                            #region  非会签

                            UserInfo AppUser = submitData.NextApprovalUser;
                            dataResult.UserInfo.Clear();
                            dataResult.UserInfo.Add(AppUser);
                            UserInfo AgentAppUser = GetAgentUserInfo(submitData.ModelCode, AppUser.UserID);//查询是否启用了代理人                            
                            dataResult = AddOrUpdateFlowRecord(workflowRuntime, instance, entity, submitData.NextStateCode, AppUser, AgentAppUser, submitData.SubmitFlag, submitData.FlowType, ref user); //处理流程数据
                            dataResult.AgentUserInfo = AgentAppUser;
                            dataResult.IsCountersign = false;
                            #endregion
                            user.TrackingMessage += "非会签 完成 FORMID=" + user.FormID + "\r\n";
                        }
                        else
                        {
                            #region  会签

                            dataResult.DictCounterUser = submitData.DictCounterUser;
                            Dictionary<UserInfo, UserInfo> dictAgentUserInfo = GetAgentUserInfo2(submitData.ModelCode, submitData.DictCounterUser);
                            dataResult = DoFlowRecord_Approval(workflowRuntime, instance, entity, submitData.NextStateCode, submitData.DictCounterUser, dictAgentUserInfo, submitData.SubmitFlag, submitData.FlowType); //处理流程数据
                            dataResult.DictAgentUserInfo = dictAgentUserInfo;
                            dataResult.IsCountersign = true;


                            #endregion
                        }

                        user.TrackingMessage += "激发流程引擎执行到一下流程 开始 FORMID=" + user.FormID + "\r\n";
                        #region 激发流程引擎执行到一下流程
                        //StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(workflowRuntime, instance.InstanceId);
                        //ManualWorkflowSchedulerService scheduleService = workflowRuntime.GetService(typeof(ManualWorkflowSchedulerService)) as ManualWorkflowSchedulerService;
                        //if (dataResult.AppState == null || dataResult.AppState == "")
                        //{
                        //    scheduleService.RunWorkflow(workflowinstance.InstanceId);
                        //    workflowRuntime.GetService<FlowEvent>().OnDoFlow(instance.InstanceId, FlowData); //激发流程引擎执行到一下流程
                        //    scheduleService.RunWorkflow(workflowinstance.InstanceId);

                        //}
                        //else
                        //{
                        //    string ss = "";
                        //    int n = 0;
                        //    scheduleService.RunWorkflow(workflowinstance.InstanceId);

                        //    workflowinstance.SetState(dataResult.AppState); //流程跳转到指定节点
                        //    //while (true)
                        //    //{
                        //    //    ss += (n++).ToString()+"|" + workflowinstance.CurrentStateName;
                        //    //    string stateName = workflowinstance.CurrentStateName;

                        //    //    if (stateName != null && stateName.ToUpper().IndexOf("START") == -1)
                        //    //    {
                        //    //        break;
                        //    //    }
                        //    //}
                        //}
                        #endregion
                        user.TrackingMessage += "激发流程引擎执行到一下流程 完成 FORMID=" + user.FormID + "\r\n";
                        //dataResult.CanSendMessage = true;

                        user.TrackingMessage += "System.Threading.Thread.Sleep(1000)\r\n";
                        //System.Threading.Thread.Sleep(1000); //Commented by Alan 2012-7-25 ,使用手动ScheduleService运行工作流,此处不需要
                        if (submitData.FlowType == FlowType.Task)
                            dataResult.SubModelCode = FlowUtility.GetSubModelCode(entity.FLOW_FLOWRECORDMASTER_T.ACTIVEROLE, dataResult.AppState); //返回下一子模块代码
                        user.TrackingMessage += "System.Threading.Thread.Sleep(1000)完成\r\n";

                        #endregion
                    }


                }
                dataResult.CurrentIsCountersign = currentIsCountersign;
                dataResult.IsGotoNextState = isGotoNextState;
                return dataResult;
            }
            catch (Exception e)
            {
                user.ErrorMsg += "提交审核时出错!FORMID=" + user.FormID + " 异常信息:" + e.ToString() + "\r\n";
                Tracer.Debug("提交审核时出错!FORMID=" + user.FormID + " 异常信息:" + e.ToString());
                throw new Exception("提交审核时出错,请联系管理员! \r\n FormID=" + user.FormID + "");
            }
            finally
            {

                entity = null;
                instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(workflowRuntime);
            }
        }

        #endregion
        #endregion
    }
}
