/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©  SmtOnline  2011    
	 * 文件名：FlowBLL.cs  
	 * 创建者：提莫科技   
	 * 创建日期：2011/12/15 08:51:55   
	 * CLR版本： 4.0.30319.1  
	 * 命名空间：SMT.FlowWFService
	 * 模块名称：
	 * 描　　述： 对原有的FlowBLL.cs进行优化和简化调整
* ---------------------------------------------------------------------*/
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
        public WorkflowRuntime workflowRuntime = null;
        //   //OracleConnection con = ADOHelper.GetOracleConnection();
        public static IDAO dao = DALFacoty.CreateDao(ConfigurationManager.ConnectionStrings["WorkFlowConnString"].ToString());

        #region 咨询
        public void AddConsultation( FLOW_CONSULTATION_T flowConsultation)
        {
            FLOW_CONSULTATION_TDAL.Add( flowConsultation);
            //FLOWDAL.FLOW_CONSULTATION_TDAL dal = new FLOW_CONSULTATION_TDAL();
            //dal.AddConsultation(flowConsultation);
        }
        public void ReplyConsultation( FLOW_CONSULTATION_T flowConsultation)
        {
            FLOW_CONSULTATION_TDAL.Update( flowConsultation);
            //FLOWDAL.FLOW_CONSULTATION_TDAL dal = new FLOW_CONSULTATION_TDAL();
            //dal.ReplyConsultation(flowConsultation);
        }

        public static FLOW_FLOWRECORDMASTER_T GetFLOW_FLOWRECORDMASTER_T( string masterID)
        {
            return FLOW_FLOWRECORDMASTER_TDAL.GetFLOW_FLOWRECORDMASTER_T( masterID);
        }
        #endregion

        #region 处理审批数据


        /// <summary>
        /// 自选流程使用:流程数据处理(对应SubmitFlow)对数据库操作
        /// </summary>
        /// <param name="workflowRuntime"></param>
        /// <param name="instance"></param>
        /// <param name="entity"></param>
        /// <param name="NextStateCode"></param>
        /// <param name="EditUserId"></param>
        /// <param name="EditUserName"></param>
        /// <param name="SubmitFlag"></param>
        /// <param name="FlowType"></param>
        /// <returns></returns>
        public DataResult DoFlowRecord( WorkflowRuntime workflowRuntime, WorkflowInstance instance, FLOW_FLOWRECORDDETAIL_T entity, string NextStateCode, UserInfo AppUser, UserInfo AgentUser, SubmitFlag SubmitFlag, FlowType FlowType,ref FlowUser fUser)
        {
            DataResult tmpDataResult = new DataResult();
            UserInfo tmpUserInfo = AppUser;

            //tmpUserInfo.UserID = EditUserId;
            //tmpUserInfo.UserName = EditUserName;

            tmpDataResult.UserInfo.Add(tmpUserInfo);
            try
            {

                if (SubmitFlag == SubmitFlag.New)
                {
                    #region 新增流程
                    //添加启动状态

                    entity.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = Guid.NewGuid().ToString();

                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "1";

                    entity.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID = entity.CREATEPOSTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDATE = DateTime.Now;

                    entity.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                    // entity.FLOW_FLOWRECORDMASTER_T .INSTANCEID  = instance.InstanceId.ToString();
                    entity.FLAG = "1";
                    entity.CHECKSTATE = "1";

                    //entity.FlowCode = "TestFlow";  //正式使用时关屏蔽

                    entity.STATECODE = "StartFlow";
                    entity.PARENTSTATEID = entity.FLOWRECORDDETAILID;

                    entity.CREATEDATE = DateTime.Now;
                    entity.EDITDATE = DateTime.Now;
                    entity.EDITUSERID = entity.CREATEUSERID;
                    entity.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.EDITCOMPANYID = entity.CREATECOMPANYID;
                    entity.EDITDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.EDITPOSTID = entity.CREATEPOSTID;

                    #region 引擎自动提交时停留在提交人处

                    if (FlowType == FlowType.Pending)
                    {
                        entity.FLAG = "0";
                        entity.EDITUSERID = AppUser.UserID;
                        entity.EDITUSERNAME = AppUser.UserName;
                        entity.EDITCOMPANYID = AppUser.CompanyID;
                        entity.EDITDEPARTMENTID = AppUser.DepartmentID;
                        entity.EDITPOSTID = AppUser.PostID;
                        FLOW_FLOWRECORDMASTER_TDAL.Add( entity.FLOW_FLOWRECORDMASTER_T);//对数据库操作
                        fUser.NextEditUserID = entity.EDITUSERID;
                        fUser.NextEditUserName = entity.EDITUSERNAME;
                        AddFlowDetailRecord( entity, NextStateCode, AppUser.UserID);
                        tmpDataResult.FlowResult = FlowResult.SUCCESS;
                        tmpDataResult.CheckState = "1";
                        return tmpDataResult;
                    }

                    #endregion

                    FLOW_FLOWRECORDMASTER_TDAL.Add( entity.FLOW_FLOWRECORDMASTER_T);//对数据库操作
                    fUser.NextEditUserID = entity.EDITUSERID;
                    fUser.NextEditUserName = entity.EDITUSERNAME;
                    AddFlowDetailRecord( entity, NextStateCode, AppUser.UserID);



                    FLOW_FLOWRECORDDETAIL_T entity2 = new FLOW_FLOWRECORDDETAIL_T();

                    //添加下一状态
                    entity2.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                    entity2.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                    entity2.STATECODE = NextStateCode == "" ? SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE) : NextStateCode;
                    entity2.PARENTSTATEID = entity.FLOWRECORDDETAILID;//entity.StateCode;

                    //entity2.Content = ".";
                    entity2.FLAG = "0";
                    entity2.CHECKSTATE = "2";
                    entity2.CREATEPOSTID = entity.CREATEPOSTID;
                    entity2.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entity2.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity2.CREATEUSERID = entity.EDITUSERID;
                    entity2.CREATEUSERNAME = entity.EDITUSERNAME;
                    entity2.CREATEDATE = DateTime.Now;
                    entity2.EDITUSERID = AppUser.UserID;
                    entity2.EDITUSERNAME = AppUser.UserName;
                    entity2.EDITCOMPANYID = AppUser.CompanyID;
                    entity2.EDITDEPARTMENTID = AppUser.DepartmentID;
                    entity2.EDITPOSTID = AppUser.PostID;
                    entity2.EDITDATE = DateTime.Now;

                    if (AgentUser != null)  //如果启用了代理，把代理人信息写入
                    {
                        entity2.AGENTUSERID = AgentUser.UserID;
                        entity2.AGENTERNAME = AgentUser.UserName;
                        entity2.AGENTEDITDATE = DateTime.Now;
                    }

                    tmpDataResult.AppState = entity2.STATECODE;

                    if (entity2.STATECODE != "EndFlow")
                    {
                        fUser.NextEditUserID = entity2.EDITUSERID;
                        fUser.NextEditUserName = entity2.EDITUSERNAME;
                        AddFlowDetailRecord( entity2, NextStateCode, AppUser.UserID);//对数据库操作
                        tmpDataResult.FlowResult = FlowResult.SUCCESS;
                        tmpDataResult.CheckState = "1";
                    }
                    else
                    {
                        tmpDataResult.CheckState = "2";
                        tmpDataResult.FlowResult = FlowResult.END;   //如果没有下一处理节点，则返回END
                    }

                    return tmpDataResult;

                    #endregion
                }
                else
                {
                    #region 更新流程
                    //如果NextStateCode为空则自动获取模型中的下一状态，如果不为空则使用传入状态代码，并使用传入下一任务人ID

                    //更新本流程



                    entity = UpdateFlowDetailRecord( entity, NextStateCode, AppUser.UserID);//对数据库操作

                    //添加下一状态
                    FLOW_FLOWRECORDDETAIL_T entity2 = new FLOW_FLOWRECORDDETAIL_T();

                    //添加下一状态
                    entity2.FLOWRECORDDETAILID = Guid.NewGuid().ToString();


                    if (NextStateCode != "")
                    {
                        entity2.STATECODE = NextStateCode;
                        //entity2.EditUserID = EditUserId;
                        //entity2.EditUserName = EditUserName;
                    }
                    else
                    {
                        entity2.STATECODE = SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE);
                        // entity2.EditUserID = entity2.StateCode=="EndFlow" ? "" : "EditUserId"; //根据状态查询权限表中用户ID
                    }

                    if (entity2.STATECODE == "EndFlow")
                    {

                        entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "2"; //设为终审通过
                        if (entity.EDITDATE == entity.AGENTEDITDATE)  //代理审核时
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.AGENTUSERID;
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.AGENTERNAME;
                        }
                        else   //正常审核时
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.EDITUSERID;
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.EDITUSERNAME;
                        }

                        entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now;

                        UpdateFlowDetailRecord( entity, NextStateCode, AppUser.UserID);//对数据库操作
                        FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);//对数据库操作
                    }




                    entity2.PARENTSTATEID = entity.FLOWRECORDDETAILID;// entity.StateCode;

                    //entity2.Content = "";
                    entity2.FLAG = "0";
                    entity2.CHECKSTATE = "2";
                    entity2.CREATEPOSTID = entity.CREATEPOSTID;
                    entity2.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entity2.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;

                    //if (entity.EDITDATE == entity.AGENTEDITDATE) //代理审核时
                    //{
                    //    entity2.CREATEUSERID = entity.AGENTUSERID;
                    //    entity2.CREATEUSERNAME = entity.AGENTERNAME;
                    //}
                    //else   //正常审核时
                    //{
                    entity2.CREATEUSERID = entity.EDITUSERID;
                    entity2.CREATEUSERNAME = entity.EDITUSERNAME;
                    //}

                    entity2.EDITUSERID = AppUser.UserID;
                    entity2.EDITUSERNAME = AppUser.UserName;
                    entity2.EDITCOMPANYID = AppUser.CompanyID;
                    entity2.EDITDEPARTMENTID = AppUser.DepartmentID;
                    entity2.EDITPOSTID = AppUser.PostID;

                    entity2.CREATEDATE = DateTime.Now;

                    entity2.EDITDATE = DateTime.Now;
                    if (AgentUser != null)  //如果启用了代理，把代理人信息写入
                    {
                        entity2.AGENTUSERID = AgentUser.UserID;
                        entity2.AGENTERNAME = AgentUser.UserName;
                        entity2.AGENTEDITDATE = DateTime.Now;
                    }

                    tmpDataResult.AppState = entity2.STATECODE;

                    if (entity2.STATECODE != "EndFlow")
                    {
                        entity2.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                        fUser.NextEditUserID = entity2.EDITUSERID;
                        fUser.NextEditUserName = entity2.EDITUSERNAME;
                        AddFlowDetailRecord( entity2, NextStateCode, AppUser.UserID);//对数据库操作
                        tmpDataResult.FlowResult = FlowResult.SUCCESS;
                        tmpDataResult.CheckState = "1";

                    }
                    else
                    {
                        tmpDataResult.FlowResult = FlowResult.END;   //如果没有下一处理节点，则返回END
                        tmpDataResult.CheckState = "2";
                    }

                    return tmpDataResult;   //如有下一节点，返回SUCCESS

                    #endregion

                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("提交自选流程数据出错,DoFlowRecord异常信息 ：" + ex.ToString());
                throw new Exception("提交自选流程数据出错!请联系管理员!");
                //tmpDataResult.FlowResult = FlowResult.FAIL;
                //tmpDataResult.Err = ex.Message;
                //return tmpDataResult;
            }
        }
        /// <summary>
        /// 非会签是使用
        /// </summary>
        /// <param name="con"></param>
        /// <param name="workflowRuntime"></param>
        /// <param name="instance"></param>
        /// <param name="entity"></param>
        /// <param name="NextStateCode"></param>
        /// <param name="AppUser"></param>
        /// <param name="AgentUser"></param>
        /// <param name="SubmitFlag"></param>
        /// <param name="FlowType"></param>
        /// <returns></returns>
        public DataResult AddOrUpdateFlowRecord( WorkflowRuntime workflowRuntime, WorkflowInstance instance, FLOW_FLOWRECORDDETAIL_T entity, string NextStateCode, UserInfo AppUser, UserInfo AgentUser, SubmitFlag SubmitFlag, FlowType FlowType,ref FlowUser fUser)
        {
            DataResult tmpDataResult = new DataResult();
            UserInfo tmpUserInfo = AppUser;
            tmpDataResult.UserInfo.Add(tmpUserInfo);
            try
            {

                if (SubmitFlag == SubmitFlag.New)
                {
                    #region 新增流程
                    //添加启动状态
                    if (SubmitFlag == FlowWFService.SubmitFlag.New)
                    {
                        entity.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = Guid.NewGuid().ToString();
                    }
                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "1";
                    entity.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID = entity.CREATEPOSTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDATE = DateTime.Now;                    
                    entity.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                    // entity.FLOW_FLOWRECORDMASTER_T .INSTANCEID  = instance.InstanceId.ToString();
                    entity.FLAG = "1";
                    entity.CHECKSTATE = "1";

                    //entity.FlowCode = "TestFlow";  //正式使用时关屏蔽

                    entity.STATECODE = "StartFlow";
                    entity.PARENTSTATEID = entity.FLOWRECORDDETAILID;

                    entity.CREATEDATE = DateTime.Now;
                    entity.EDITDATE = DateTime.Now;
                    entity.EDITUSERID = entity.CREATEUSERID;
                    entity.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.EDITCOMPANYID = entity.CREATECOMPANYID;
                    entity.EDITDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.EDITPOSTID = entity.CREATEPOSTID;

                    #region 引擎自动提交时停留在提交人处

                    if (FlowType == FlowType.Pending)
                    {
                        entity.FLAG = "0";
                        entity.EDITUSERID = AppUser.UserID;
                        entity.EDITUSERNAME = AppUser.UserName;
                        entity.EDITCOMPANYID = AppUser.CompanyID;
                        entity.EDITDEPARTMENTID = AppUser.DepartmentID;
                        entity.EDITPOSTID = AppUser.PostID;
                        fUser.NextEditUserID = entity.EDITUSERID;
                        fUser.NextEditUserName = entity.EDITUSERNAME;
                        FLOW_FLOWRECORDMASTER_TDAL.Add( entity.FLOW_FLOWRECORDMASTER_T);
                        AddFlowDetailRecord( entity, NextStateCode, AppUser.UserID);
                        tmpDataResult.FlowResult = FlowResult.SUCCESS;
                        tmpDataResult.CheckState = "1";
                        return tmpDataResult;
                    }

                    #endregion

                    FLOW_FLOWRECORDMASTER_TDAL.Add( entity.FLOW_FLOWRECORDMASTER_T);
                    fUser.NextEditUserID = entity.EDITUSERID;
                    fUser.NextEditUserName = entity.EDITUSERNAME;
                    AddFlowDetailRecord( entity, NextStateCode, AppUser.UserID);

                    FLOW_FLOWRECORDDETAIL_T entDetail = new FLOW_FLOWRECORDDETAIL_T();

                    //添加下一状态
                    entDetail.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                    entDetail.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                    entDetail.STATECODE = NextStateCode == "" ? SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE) : NextStateCode;
                    entDetail.PARENTSTATEID = entity.FLOWRECORDDETAILID;//entity.StateCode;
                    //entity2.Content = ".";
                    entDetail.FLAG = "0";
                    entDetail.CHECKSTATE = "2";
                    entDetail.CREATEPOSTID = entity.CREATEPOSTID;
                    entDetail.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entDetail.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entDetail.CREATEUSERID = entity.EDITUSERID;
                    entDetail.CREATEUSERNAME = entity.EDITUSERNAME;
                    entDetail.CREATEDATE = DateTime.Now;
                    entDetail.EDITUSERID = AppUser.UserID;
                    entDetail.EDITUSERNAME = AppUser.UserName;
                    entDetail.EDITCOMPANYID = AppUser.CompanyID;
                    entDetail.EDITDEPARTMENTID = AppUser.DepartmentID;
                    entDetail.EDITPOSTID = AppUser.PostID;
                    entDetail.EDITDATE = DateTime.Now;

                    if (AgentUser != null)  //如果启用了代理，把代理人信息写入
                    {
                        entDetail.AGENTUSERID = AgentUser.UserID;
                        entDetail.AGENTERNAME = AgentUser.UserName;
                        entDetail.AGENTEDITDATE = DateTime.Now;
                    }

                    tmpDataResult.AppState = entDetail.STATECODE;

                    if (entDetail.STATECODE != "EndFlow")
                    {
                        fUser.NextEditUserID = entDetail.EDITUSERID;
                        fUser.NextEditUserName = entDetail.EDITUSERNAME;
                        AddFlowDetailRecord( entDetail, NextStateCode, AppUser.UserID);
                        tmpDataResult.FlowResult = FlowResult.SUCCESS;
                        tmpDataResult.CheckState = "1";
                    }
                    else
                    {
                        tmpDataResult.CheckState = "2";
                        tmpDataResult.FlowResult = FlowResult.END;   //如果没有下一处理节点，则返回END
                    }
                    tmpDataResult.IsCountersignComplete = true;
                    return tmpDataResult;

                    #endregion
                }

                else
                {
                    #region 更新流程
                    //如果NextStateCode为空则自动获取模型中的下一状态，如果不为空则使用传入状态代码，并使用传入下一任务人ID
                    //更新本流程
                    entity = UpdateFlowDetailRecord( entity, NextStateCode, AppUser.UserID);
                    string stateCode = "";
                    if (NextStateCode.ToUpper() == "ENDFLOW")
                    {
                        stateCode = NextStateCode;
                    }
                    else
                    {
                        stateCode = string.IsNullOrEmpty(NextStateCode) ? SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE) : NextStateCode;
                    }
                    if (stateCode == "EndFlow")
                    {
                        #region
                        entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "2"; //设为终审通过
                        if (entity.EDITDATE == entity.AGENTEDITDATE)  //代理审核时
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.AGENTUSERID;
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.AGENTERNAME;
                            
                        }
                        else   //正常审核时
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.EDITUSERID;
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.EDITUSERNAME;
                           
                        }

                        entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now;

                        UpdateFlowDetailRecord( entity, NextStateCode, AppUser.UserID);

                        FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);
                        tmpDataResult.AppState = stateCode;
                        tmpDataResult.FlowResult = FlowResult.END;   //如果没有下一处理节点，则返回END
                        tmpDataResult.CheckState = "2";
                        #endregion
                    }
                    else
                    {
                        #region
                        //添加下一状态
                        FLOW_FLOWRECORDDETAIL_T entity2 = new FLOW_FLOWRECORDDETAIL_T();
                        //添加下一状态
                        entity2.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                        if (NextStateCode != "")
                        {
                            entity2.STATECODE = NextStateCode;
                        }
                        else
                        {
                            entity2.STATECODE = stateCode;
                        }
                        entity2.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                        entity2.PARENTSTATEID = entity.FLOWRECORDDETAILID;// entity.StateCode;
                        entity2.FLAG = "0";
                        entity2.CHECKSTATE = "2";
                        entity2.CREATEPOSTID = entity.CREATEPOSTID;
                        entity2.CREATECOMPANYID = entity.CREATECOMPANYID;
                        entity2.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                        entity2.CREATEUSERID = entity.EDITUSERID;
                        entity2.CREATEUSERNAME = entity.EDITUSERNAME;

                        entity2.EDITUSERID = AppUser.UserID;
                        entity2.EDITUSERNAME = AppUser.UserName;
                        entity2.EDITCOMPANYID = AppUser.CompanyID;
                        entity2.EDITDEPARTMENTID = AppUser.DepartmentID;
                        entity2.EDITPOSTID = AppUser.PostID;

                        entity2.CREATEDATE = DateTime.Now;

                        entity2.EDITDATE = DateTime.Now;
                        if (AgentUser != null)  //如果启用了代理，把代理人信息写入
                        {
                            entity2.AGENTUSERID = AgentUser.UserID;
                            entity2.AGENTERNAME = AgentUser.UserName;
                            entity2.AGENTEDITDATE = DateTime.Now;
                        }

                        tmpDataResult.AppState = entity2.STATECODE;
                        fUser.NextEditUserID = entity2.EDITUSERID;
                        fUser.NextEditUserName = entity2.EDITUSERNAME;
                        AddFlowDetailRecord( entity2, NextStateCode, AppUser.UserID);
                        tmpDataResult.FlowResult = FlowResult.SUCCESS;
                        tmpDataResult.CheckState = "1";
                        #endregion
                        #region 更新审核主表的审核人(提莫科技新增)
                        entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "1"; 
                        if (entity.EDITDATE == entity.AGENTEDITDATE)  
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.AGENTUSERID;
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.AGENTERNAME;
                        }
                        else   //正常审核时
                        {
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.EDITUSERID;
                            entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.EDITUSERNAME;
                        }
                        entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now; 
                        FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);
                       
                        #endregion
                    }

                    tmpDataResult.IsCountersignComplete = true;
                    return tmpDataResult;   //如有下一节点，返回SUCCESS

                    #endregion

                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("DoFlowRecord2异常信息 ：" + ex.ToString());
                throw new Exception("DoFlowRecord2:" + ex.InnerException + ex.Message);
            }
        }

        /// <summary>
        /// 会签
        /// </summary>
        /// <param name="workflowRuntime"></param>
        /// <param name="instance"></param>
        /// <param name="entity"></param>
        /// <param name="NextStateCode"></param>
        /// <param name="dictUserInfo"></param>
        /// <param name="dictAgentUserInfo"></param>
        /// <param name="SubmitFlag"></param>
        /// <param name="FlowType"></param>
        /// <returns></returns>

        public DataResult DoFlowRecord_Add( WorkflowRuntime workflowRuntime, WorkflowInstance instance, FLOW_FLOWRECORDDETAIL_T entity, string NextStateCode, Dictionary<FlowRole, List<UserInfo>> dictUserInfo, Dictionary<UserInfo, UserInfo> dictAgentUserInfo, SubmitFlag SubmitFlag, FlowType FlowType)
        {
            DataResult tmpDataResult = new DataResult();
            tmpDataResult.DictCounterUser = dictUserInfo;

            try
            {
                if (SubmitFlag == SubmitFlag.New)
                {

                    #region 添加启动状态

                    entity.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = Guid.NewGuid().ToString();
                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "1";
                    entity.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID = entity.CREATEPOSTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDATE = DateTime.Now;

                    entity.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                    // entity.FLOW_FLOWRECORDMASTER_T .INSTANCEID  = instance.InstanceId.ToString();
                    entity.FLAG = "1";
                    entity.CHECKSTATE = "1";
                    //entity.FlowCode = "TestFlow";  //正式使用时关屏蔽

                    entity.STATECODE = "StartFlow";
                    entity.PARENTSTATEID = entity.FLOWRECORDDETAILID;
                    entity.CREATEDATE = DateTime.Now;
                    entity.EDITDATE = DateTime.Now;
                    entity.EDITUSERID = entity.CREATEUSERID;
                    entity.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.EDITCOMPANYID = entity.CREATECOMPANYID;
                    entity.EDITDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.EDITPOSTID = entity.CREATEPOSTID;
                    FLOW_FLOWRECORDMASTER_TDAL.Add( entity.FLOW_FLOWRECORDMASTER_T);
                    AddFlowRecord2( entity);

                    #endregion
                }
                else
                {
                    #region

                    //entity.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = Guid.NewGuid().ToString();
                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "1";
                    entity.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID = entity.CREATEPOSTID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.CREATEUSERID;
                    entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.FLOW_FLOWRECORDMASTER_T.CREATEDATE = DateTime.Now;

                    entity.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                    // entity.FLOW_FLOWRECORDMASTER_T .INSTANCEID  = instance.InstanceId.ToString();
                    entity.FLAG = "1";
                    entity.CHECKSTATE = "1";
                    //entity.FlowCode = "TestFlow";  //正式使用时关屏蔽

                    entity.STATECODE = "StartFlow";
                    entity.PARENTSTATEID = entity.FLOWRECORDDETAILID;
                    entity.CREATEDATE = DateTime.Now;
                    entity.EDITDATE = DateTime.Now;
                    entity.EDITUSERID = entity.CREATEUSERID;
                    entity.EDITUSERNAME = entity.CREATEUSERNAME;
                    entity.EDITCOMPANYID = entity.CREATECOMPANYID;
                    entity.EDITDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entity.EDITPOSTID = entity.CREATEPOSTID;
                    entity.CHECKSTATE = "6";
                    entity.STATECODE = "ReSubmit";
                    entity.FLAG = "1";
                    AddFlowRecord2( entity);
                    FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);
                    #endregion
                }
                //System.Threading.Thread.Sleep(1000);
                string stateCode = NextStateCode == "" ? SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE) : NextStateCode;
                tmpDataResult.AppState = stateCode;
                if (stateCode != "EndFlow")
                {
                    #region
                    dictUserInfo.Values.ToList().ForEach(users =>
                    {
                        users.ForEach(user =>
                        {
                            #region
                            FLOW_FLOWRECORDDETAIL_T entity2 = new FLOW_FLOWRECORDDETAIL_T();
                            entity2.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                            entity2.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                            entity2.STATECODE = stateCode;
                            entity2.PARENTSTATEID = entity.FLOWRECORDDETAILID;//entity.StateCode;                            
                            entity2.FLAG = "0";
                            entity2.CHECKSTATE = "2";
                            entity2.CREATEPOSTID = entity.CREATEPOSTID;
                            entity2.CREATECOMPANYID = entity.CREATECOMPANYID;
                            entity2.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                            entity2.CREATEUSERID = entity.EDITUSERID;
                            entity2.CREATEUSERNAME = entity.EDITUSERNAME;
                            entity2.CREATEDATE = DateTime.Now;
                            entity2.EDITUSERID = user.UserID;
                            entity2.EDITUSERNAME = user.UserName;
                            entity2.EDITCOMPANYID = user.CompanyID;
                            entity2.EDITDEPARTMENTID = user.DepartmentID;
                            entity2.EDITPOSTID = user.PostID;
                            entity2.EDITDATE = DateTime.Now;
                            if (dictAgentUserInfo.ContainsKey(user))
                            {
                                entity2.AGENTUSERID = dictAgentUserInfo[user].UserID;
                                entity2.AGENTERNAME = dictAgentUserInfo[user].UserName;
                                entity2.AGENTEDITDATE = DateTime.Now;
                            }
                            AddFlowRecord2( entity2);

                            #endregion
                        });
                    });
                    #endregion
                    tmpDataResult.AppState = stateCode;
                    tmpDataResult.FlowResult = FlowResult.SUCCESS;
                    tmpDataResult.CheckState = "1";
                }
                else
                {
                    tmpDataResult.CheckState = "2";
                    tmpDataResult.FlowResult = FlowResult.END;   //如果没有下一处理节点，则返回END
                }
                tmpDataResult.IsCountersignComplete = true;
                return tmpDataResult;


            }
            catch (Exception ex)
            {
                Tracer.Debug("DoFlowRecord_Add异常信息 ：" + ex.ToString());
                throw new Exception("DoFlowRecord_Add:" + ex.InnerException + ex.Message);
                //tmpDataResult.FlowResult = FlowResult.FAIL;
                //tmpDataResult.Err = ex.Message;
                //return tmpDataResult;
            }
        }
        /// <summary>
        /// 回定流程中,会签
        /// </summary>
        /// <param name="con"></param>
        /// <param name="workflowRuntime"></param>
        /// <param name="instance"></param>
        /// <param name="entity"></param>
        /// <param name="NextStateCode"></param>
        /// <param name="dictUserInfo"></param>
        /// <param name="dictAgentUserInfo"></param>
        /// <param name="SubmitFlag"></param>
        /// <param name="FlowType"></param>
        /// <returns></returns>
        public DataResult DoFlowRecord_Approval( WorkflowRuntime workflowRuntime, WorkflowInstance instance, FLOW_FLOWRECORDDETAIL_T entity, string NextStateCode, Dictionary<FlowRole, List<UserInfo>> dictUserInfo, Dictionary<UserInfo, UserInfo> dictAgentUserInfo, SubmitFlag SubmitFlag, FlowType FlowType)
        {
            DataResult tmpDataResult = new DataResult();
            tmpDataResult.DictCounterUser = dictUserInfo;

            try
            {


                #region 更新流程
                //如果NextStateCode为空则自动获取模型中的下一状态，如果不为空则使用传入状态代码，并使用传入下一任务人ID

                //更新本流程



                entity = UpdateFlowRecord2( entity);
                string stateCode = "";
                if (NextStateCode.ToUpper() == "ENDFLOW")
                {
                    stateCode = NextStateCode;
                }
                else
                {
                    stateCode = string.IsNullOrEmpty(NextStateCode) ? SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE) : NextStateCode;
                }
                //string stateCode = NextStateCode == "" ? SMTWorkFlowManage.GetNextState(workflowRuntime, instance, entity.STATECODE) : NextStateCode;
                tmpDataResult.AppState = stateCode;



                if (stateCode == "EndFlow")
                {
                    entity.FLOW_FLOWRECORDMASTER_T.CHECKSTATE = "2"; //设为终审通过
                    if (entity.EDITDATE == entity.AGENTEDITDATE)  //代理审核时
                    {
                        entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.AGENTUSERID;
                        entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.AGENTERNAME;
                    }
                    else   //正常审核时
                    {
                        entity.FLOW_FLOWRECORDMASTER_T.EDITUSERID = entity.EDITUSERID;
                        entity.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME = entity.EDITUSERNAME;
                    }

                    entity.FLOW_FLOWRECORDMASTER_T.EDITDATE = DateTime.Now;

                    UpdateFlowRecord2( entity);
                    FLOW_FLOWRECORDMASTER_TDAL.Update(entity.FLOW_FLOWRECORDMASTER_T);
                    tmpDataResult.FlowResult = FlowResult.END;   //如果没有下一处理节点，则返回END
                    tmpDataResult.CheckState = "2";
                }
                else
                {
                    dictUserInfo.Values.ToList().ForEach(users =>
                    {
                        users.ForEach(user =>
                        {
                            #region
                            //添加下一状态
                            //FLOW_FLOWRECORDDETAIL_T entity2 = new FLOW_FLOWRECORDDETAIL_T();

                            ////添加下一状态
                            //entity2.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                            //entity2.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                            //entity2.PARENTSTATEID = entity.FLOWRECORDDETAILID;// entity.StateCode;

                            ////entity2.Content = "";
                            //entity2.STATECODE = stateCode;
                            //entity2.FLAG = "0";
                            //entity2.CHECKSTATE = "2";
                            //entity2.CREATEPOSTID = entity.CREATEPOSTID;
                            //entity2.CREATECOMPANYID = entity.CREATECOMPANYID;
                            //entity2.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;

                            ////if (entity.EDITDATE == entity.AGENTEDITDATE) //代理审核时
                            ////{
                            ////    entity2.CREATEUSERID = entity.AGENTUSERID;
                            ////    entity2.CREATEUSERNAME = entity.AGENTERNAME;
                            ////}
                            ////else   //正常审核时
                            ////{
                            //entity2.CREATEUSERID = entity.EDITUSERID;
                            //entity2.CREATEUSERNAME = entity.EDITUSERNAME;
                            ////}

                            //entity2.EDITUSERID = user.UserID;
                            //entity2.EDITUSERNAME = user.UserName;
                            //entity2.EDITCOMPANYID = user.CompanyID;
                            //entity2.EDITDEPARTMENTID = user.DepartmentID;
                            //entity2.EDITPOSTID = user.PostID;

                            //entity2.CREATEDATE = DateTime.Now;

                            //entity2.EDITDATE = DateTime.Now;
                            //if (dictAgentUserInfo.ContainsKey(user))
                            //{
                            //    entity2.AGENTUSERID = dictAgentUserInfo[user].UserID;
                            //    entity2.AGENTERNAME = dictAgentUserInfo[user].UserName;
                            //    entity2.AGENTEDITDATE = DateTime.Now;
                            //}
                            //AddFlowRecord2(entity2);

                            #endregion

                            #region
                            FLOW_FLOWRECORDDETAIL_T entity2 = new FLOW_FLOWRECORDDETAIL_T();
                            entity2.FLOWRECORDDETAILID = Guid.NewGuid().ToString();
                            entity2.FLOW_FLOWRECORDMASTER_T = entity.FLOW_FLOWRECORDMASTER_T;
                            entity2.STATECODE = stateCode;
                            entity2.PARENTSTATEID = entity.FLOWRECORDDETAILID;//entity.StateCode;                            
                            entity2.FLAG = "0";
                            entity2.CHECKSTATE = "2";
                            entity2.CREATEPOSTID = entity.CREATEPOSTID;
                            entity2.CREATECOMPANYID = entity.CREATECOMPANYID;
                            entity2.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                            entity2.CREATEUSERID = entity.EDITUSERID;
                            entity2.CREATEUSERNAME = entity.EDITUSERNAME;
                            entity2.CREATEDATE = DateTime.Now;
                            entity2.EDITUSERID = user.UserID;
                            entity2.EDITUSERNAME = user.UserName;
                            entity2.EDITCOMPANYID = user.CompanyID;
                            entity2.EDITDEPARTMENTID = user.DepartmentID;
                            entity2.EDITPOSTID = user.PostID;
                            entity2.EDITDATE = DateTime.Now;
                            if (dictAgentUserInfo.ContainsKey(user))
                            {
                                entity2.AGENTUSERID = dictAgentUserInfo[user].UserID;
                                entity2.AGENTERNAME = dictAgentUserInfo[user].UserName;
                                entity2.AGENTEDITDATE = DateTime.Now;
                            }
                            AddFlowRecord2( entity2);

                            #endregion
                        });
                    });

                    tmpDataResult.AppState = stateCode;
                    tmpDataResult.FlowResult = FlowResult.SUCCESS;
                    tmpDataResult.CheckState = "1";

                }

                tmpDataResult.IsCountersignComplete = true;
                return tmpDataResult;

                #endregion


            }
            catch (Exception ex)
            {
                Tracer.Debug("DoFlowRecord_Approval异常信息 ：" + ex.ToString());
                throw new Exception("DoFlowRecord_Approval:" + ex.InnerException + ex.Message);
                //tmpDataResult.FlowResult = FlowResult.FAIL;
                //tmpDataResult.Err = ex.Message;
                //return tmpDataResult;
            }
        }


        #endregion

        #region 操作流程数据
        /// <summary>
        /// 新增[流程审批明细表]
        /// </summary>
        /// <param name="entity">流程审批明细表</param>
        /// <param name="NextStateCode">下一个状态代码</param>
        /// <param name="EditUserId">编辑用户ID</param>
        void AddFlowDetailRecord( FLOW_FLOWRECORDDETAIL_T entity, string NextStateCode, string EditUserId)
        {
            FLOW_FLOWRECORDDETAIL_TDAL.Add( entity);
        }
        /// <summary>
        /// 新增[流程审批明细表]
        /// </summary>
        /// <param name="entity">流程审批明细表</param>
        void AddFlowRecord2( FLOW_FLOWRECORDDETAIL_T entity)
        {
            FLOW_FLOWRECORDDETAIL_TDAL.Add( entity);
        }
        /// <summary>
        /// 更新[流程审批明细表]
        /// </summary>
        /// <param name="entity">流程审批明细表</param>
        /// <param name="NextStateCode">下一个状态代码</param>
        /// <param name="EditUserId">编辑用户ID</param>
        /// <returns></returns>
        public FLOW_FLOWRECORDDETAIL_T UpdateFlowDetailRecord( FLOW_FLOWRECORDDETAIL_T entity, string NextStateCode, string EditUserId)
        {
            entity.FLAG = "1";
            FLOW_FLOWRECORDDETAIL_TDAL.Update( entity);
            return entity;
        }
        /// <summary>
        /// 更新[流程审批明细表]
        /// </summary>
        /// <param name="entity">流程审批明细表</param>
        /// <returns></returns>
        public FLOW_FLOWRECORDDETAIL_T UpdateFlowRecord2( FLOW_FLOWRECORDDETAIL_T entity)
        {
            FLOW_FLOWRECORDDETAIL_TDAL Dal = new FLOW_FLOWRECORDDETAIL_TDAL();
            entity.FLAG = "1";
            FLOW_FLOWRECORDDETAIL_TDAL.Update( entity);
            return entity;
        }

        #endregion

        #region 查询流程信息

        /// <summary>
        /// 获取流程信息(对数据库操作)
        /// </summary>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>
        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
        /// <returns></returns>       
        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowInfo( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<FlowType> FlowType)
        {

            try
            {
                List<string> FlowTypeList = new List<string>();

                FLOW_FLOWRECORDDETAIL_TDAL Dal = new FLOW_FLOWRECORDDETAIL_TDAL();
                var dt = FLOW_FLOWRECORDDETAIL_TDAL.GetFlowRecord( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowUtility.FlowTypeListToStringList(FlowType));

                if (dt.Count > 0)
                    return dt;
                return null;
            }
            catch (Exception ex)
            {
                Tracer.Debug("获取流程信息GetFlowInfo异常信息(FormID=" + FormID + ";ModelCode=" + ModelCode + ";CompanyID=" + CompanyID + ";EditUserID=" + EditUserID + ")：" + ex.Message);
                throw new Exception("获取流程信息出错,请联系管理员! \r\n FormID=" + FormID + "");
            }


        }
        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>
        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
        /// <returns></returns>       
        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowInfoV( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<FlowType> FlowType)
        {

            try
            {
                var dt = FLOW_FLOWRECORDDETAIL_TDAL.GetFlowRecordV( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowUtility.FlowTypeListToStringList(FlowType));

                if (dt.Count > 0)
                    return dt;
                return null;
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetFlowInfoV异常信息：" + ex.Message);
                throw new Exception("GetFlowInfoV:" + ex.Message);
            }


        }
        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>
        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
        /// <returns></returns>           
        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowInfoTop( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<FlowType> FlowType)
        {
            return FLOW_FLOWRECORDDETAIL_TDAL.GetFlowRecordTop( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowUtility.FlowTypeListToStringList(FlowType));
        }
        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>     
        /// <returns></returns>
        public static List<TaskInfo> GetTaskInfo( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {
            try
            {
                List<FlowType> FlowTypeList = new List<FlowWFService.FlowType>();
                FlowTypeList.Add(FlowType.Task);
                List<FLOW_FLOWRECORDDETAIL_T> FLOWRECORDDETAIList = GetFlowInfo( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
                if (FLOWRECORDDETAIList == null || FLOWRECORDDETAIList.Count == 0)
                    return null;

                string ACTIVEROLE = FLOWRECORDDETAIList[0].FLOW_FLOWRECORDMASTER_T.ACTIVEROLE;
                List<TaskInfo> TaskInfoList = new List<TaskInfo>();
                for (int i = 0; i < FLOWRECORDDETAIList.Count; i++)
                {
                    TaskInfo tmpTaskInfo = new TaskInfo();
                    tmpTaskInfo.FlowInfo = FLOWRECORDDETAIList[i];
                    tmpTaskInfo.SubModelCode = FlowUtility.GetString(FlowUtility.GetSubModelCode(ACTIVEROLE, FLOWRECORDDETAIList[i].STATECODE));
                    TaskInfoList.Add(tmpTaskInfo);
                }

                return TaskInfoList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 获取[流程审批实例]信息
        /// </summary>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>      
        /// <returns></returns>  
        public static List<FLOW_FLOWRECORDMASTER_T> GetFlowRecordMaster( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {
            //FLOW_FLOWRECORDDETAIL_TDAL Dal = new FLOW_FLOWRECORDDETAIL_TDAL();
            //var dt = Dal.GetFlowRecordMaster(FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
            List<FLOW_FLOWRECORDDETAIL_T> listDetail = FLOW_FLOWRECORDDETAIL_TDAL.GetFlowRecord( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, null);
            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();
            listDetail.ForEach(detail =>
            {
                if (listMaster.FirstOrDefault(d => d.FLOWRECORDMASTERID == detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID) == null)
                {
                    listMaster.Add(detail.FLOW_FLOWRECORDMASTER_T);
                }
            });
            return listMaster;
        }

        public static List<FLOW_FLOWRECORDMASTER_T> GetFlowRecordBySubmitUserID(string CheckState, string EditUserID)
        {
            //FLOW_FLOWRECORDDETAIL_TDAL Dal = new FLOW_FLOWRECORDDETAIL_TDAL();
            //var dt = Dal.GetFlowRecordBySubmitUserID( CheckState,EditUserID);
            var dt = FLOW_FLOWRECORDMASTER_TDAL.GetFlowRecordBySubmitUserID(CheckState, EditUserID);
            if (dt.Count > 0)
                return dt;
            return null;
        }

        #endregion

        #region "查询带审核单据"
        /// <summary>
        /// 根据模块代码和用户id查询待审核单据
        /// </summary>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="EditUserID">用户id</param>
        /// <returns></returns>
        public static List<string> GetWaitingApprovalForm(string ModelCode, string EditUserID)
        {
            var dt = FLOW_FLOWRECORDDETAIL_TDAL.GetWaitingApprovalForm(ModelCode, EditUserID);
            if (dt.Count > 0)
                return dt;
            return null;
        }
        #endregion

        #region 通过模块代码查询系统代码
        /// <summary>
        /// 通过模块代码查询系统代码
        /// </summary>
        /// <param name="ModelCode">模块代码</param>
        /// <returns></returns>
        public static ModelInfo GetSysCodeByModelCode( string ModelCode)
        {
            ModelInfo tmpModelInfo = new ModelInfo();
            try
            {
                var dt = FLOW_MODELDEFINE_TDAL.GetModelDefineByCode( ModelCode);
                //Flow_ModelDefine_TDAL Dal = new Flow_ModelDefine_TDAL();
                //var dt = Dal.GetModelDefineByCode(ModelCode);

                if (dt.Count > 0)
                {
                    tmpModelInfo.SysCode = dt[0].SYSTEMCODE;
                    tmpModelInfo.ModelName = dt[0].DESCRIPTION;
                    return tmpModelInfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetSysCodeByModelCode异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                tmpModelInfo = null;
            }

        }

        #endregion



        #region 获取工作流状态

        /// <summary>
        /// 获取工作流状态
        /// </summary>
        /// <param name="workflowRuntime">工作流运行时</param>
        /// <param name="instance">工作流实例</param>
        /// <returns></returns>
        public static string GetState(WorkflowRuntime workflowRuntime, WorkflowInstance instance, string CurrentStateName)
        {
            string StateName = CurrentStateName;

            while (StateName == CurrentStateName)
            {
                //StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(workflowRuntime, instance.InstanceId);
                //StateName = workflowinstance.CurrentStateName;

                StateName = StateName == null ? "EndFlow" : StateName;
            }
            //System.Threading.Thread.Sleep(1 * 1000);
            //ReadOnlyCollection<WorkflowQueueInfo> queueInfoData = instance.GetWorkflowQueueData();
            //if (queueInfoData.Count != 0)
            //{

            //    foreach (WorkflowQueueInfo info in queueInfoData)
            //    {
            //        if (info.QueueName.Equals("SetStateQueue"))
            //        {

            //            StateName = info.SubscribedActivityNames[0];
            //        }
            //    }

            //}
            return StateName;

        }

        #endregion

        #region 通过流程查找审核人

        /// <summary>
        /// 通过流程查找审核人
        /// </summary>
        /// <param name="Xoml"></param>
        /// <param name="Rules"></param>
        /// <param name="xml"></param>
        /// <param name="UserID"></param>
        /// <param name="DataResult"></param>
        //public void GetUserByFlow(string Xoml, string Rules, string xml, string UserID, ref DataResult DataResult)
        //{
        //    WorkflowRuntime WfRuntime = null;
        //    WorkflowInstance Instance = null;
        //    try
        //    {
        //        WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
        //        Instance = SMTWorkFlowManage.CreateWorkflowInstance(WfRuntime, Xoml, Rules);

        //        string strNextState = SMTWorkFlowManage.GetNextStateByEvent(WfRuntime, Instance, "StartFlow", xml);



        //        List<UserInfo> AppUserInfo = GetUserByStateCode(strNextState, UserID);
        //        if (AppUserInfo == null || AppUserInfo.Count == 0)
        //        {
        //            DataResult.Err = "没有找到审核人";
        //            DataResult.FlowResult = FlowResult.FAIL;
        //        }
        //        else if (AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
        //        {
        //            DataResult.FlowResult = FlowResult.MULTIUSER;
        //        }
        //        DataResult.UserInfo = AppUserInfo;


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        Instance = null;
        //        SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime);

        //    }


        //}

        #endregion

        #region 通过流程查找审核人(对应SubmitFlow)

        /// <summary>
        /// 通过流程查找审核人(对应SubmitFlow)
        /// </summary>
        /// <param name="Xoml"></param>
        /// <param name="Rules"></param>
        /// <param name="Layout"></param>
        /// <param name="xml"></param>
        /// <param name="UserID"></param>
        /// <param name="FlowType"></param>
        /// <param name="DataResult"></param>
        public void GetUserByFlow(string companyID, string Xoml, string Rules, string Layout, string xml, string UserID, string PostID, FlowType FlowType, ref DataResult DataResult)
        {
            string Msg = "companyID=" + companyID + "\r\n";
            Msg += "Xoml=" + Xoml + "\r\n";
            Msg += "Rules=" + Rules + "\r\n";
            Msg += "Layout=" + Layout + "\r\n";
            Msg += "xml=" + xml + "\r\n";
            Msg += "UserID=" + UserID + "\r\n";
            Msg += "PostID=" + PostID + "\r\n";
            Msg += "FlowType=" + FlowType + "\r\n";

            WorkflowRuntime WfRuntime = null;
            WorkflowInstance Instance = null;
            string strCurrState = "StartFlow";
            string strNextState = "StartFlow";

            FlowRole RuleName;
            List<UserInfo> AppUserInfo = null;
            try
            {
                WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
                Instance = SMTWorkFlowManage.CreateWorkflowInstance(WfRuntime,Xoml, Rules);
                Tracer.Debug("GetUserByFlow创建工作流实例ID=" + Instance.InstanceId);
                bool iscurruser = true;

                while (iscurruser)
                {
                    strCurrState = strNextState;

                    strNextState = SMTWorkFlowManage.GetFlowNextStepRoles(WfRuntime, Instance, strNextState, xml);

                    if (strNextState == "EndFlow")
                    {
                        strNextState = strCurrState;
                        iscurruser = false;
                    }
                    else
                    {

                        RuleName = FlowUtility.GetRlueName(Layout, strNextState);
                        if (RuleName == null)
                        {
                            DataResult.Err = "没有找到对应角色";
                            DataResult.FlowResult = FlowResult.FAIL;
                            return;
                        }
                        bool isHigher = false;
                        AppUserInfo = TmGetUserByRoleId(RuleName.RoleName, UserID, PostID, ref isHigher);
                        #region 打印审核人
                        string names = "\r\n=======打印审核人A(listRole[0].RoleName=" + RuleName.RoleName + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                        foreach (var user in AppUserInfo)
                        {
                            names += "CompanyID:" + user.CompanyID + "\r\n";
                            names += "DepartmentID:" + user.DepartmentID + "\r\n";
                            names += "PostID:" + user.PostID + "\r\n";
                            names += "UserID:" + user.UserID + "\r\n";
                           

                            names += "CompanyName:" + user.CompanyName + "\r\n";
                            names += "DepartmentName:" + user.DepartmentName + "\r\n";
                            names += "PostName:" + user.PostName + "\r\n";
                            names += "UserName:" + user.UserName + "\r\n";
                            names += "----------------------------------------------------\r\n";
                        }
                        if (!isHigher && RuleName.IsOtherCompany != null)
                        {
                            if (RuleName.IsOtherCompany.Value == true)
                            {
                                names += "是否指定公司：" + RuleName.IsOtherCompany.Value.ToString() + "\r\n";
                                names += "公司的ID：" + RuleName.OtherCompanyID + "\r\n";
                                if (string.IsNullOrEmpty(RuleName.OtherCompanyID))
                                {
                                    names += "Layout=" + Layout + "\r\n";
                                }
                            }
                            else if (RuleName.IsOtherCompany.Value == false)
                            {
                                names += "实际要查找公司的ID:" + companyID + "\r\n";
                                
                            }
                        }

                        Tracer.Debug(names);
                        #endregion
                        #region beyond
                        if (!isHigher && RuleName.IsOtherCompany != null)
                        {
                            if (RuleName.IsOtherCompany.Value == true)
                            {
                                AppUserInfo = AppUserInfo.Where(user => user.CompanyID == RuleName.OtherCompanyID).ToList();
                            }
                            else if (RuleName.IsOtherCompany.Value == false)
                            {
                                AppUserInfo = AppUserInfo.Where(user => user.CompanyID == companyID).ToList();
                            }
                        }
                        #endregion

                        if (AppUserInfo == null || AppUserInfo.Count == 0)
                        {
                            DataResult.Err = "没有找到审核人";
                            DataResult.FlowResult = FlowResult.FAIL;
                            return;
                        }

                        if (AppUserInfo.Where(c => c.UserID == UserID).Count() == 0)
                            iscurruser = false;
                    }
                }


                //if (AppUserInfo == null || AppUserInfo.Count == 0)
                //{
                //    DataResult.Err = "没有找到审核人";
                //    DataResult.FlowResult = FlowResult.FAIL;
                //}
                // else if (AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
                if (AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
                {
                    DataResult.FlowResult = FlowResult.MULTIUSER;
                }
                DataResult.AppState = strNextState;
                DataResult.UserInfo = AppUserInfo;


            }
            catch (Exception ex)
            {
                Tracer.Debug("GetUserByFlow发生异常：" + Msg);
                throw new Exception("GetUserByFlow:" + ex.ToString());
            }
            finally
            {
                strCurrState = null;
                strNextState = null;
                RuleName = null;
                AppUserInfo = null;
                Instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime);

            }


        }

        #endregion

        #region 通过持久化服务查询下一处理人

        /// <summary>
        /// 通过持久化服务查询下一处理人
        /// </summary>
        /// <param name="WfRuntimeClone">持久化运行时</param>
        /// <param name="instanceClone">持久化实例</param>
        /// <param name="xml">条件XML</param>
        /// <param name="CurrentStateName">当前状态代码</param>
        /// <param name="DataResult">操作结果</param>
        public void GetUserByInstance(WorkflowRuntime WfRuntimeClone, WorkflowInstance instanceClone, string xml, string CurrentStateName, string UserID, string PostID, ref DataResult DataResult)
        {
            if (!WfRuntimeClone.IsStarted)
            {
                WfRuntimeClone.StartRuntime();
            }
            WorkflowRuntime WfRuntime = null;
            WorkflowInstance Instance = null;
            try
            {
                WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
                Instance = SMTWorkFlowManage.CloneWorkflowInstance(WfRuntimeClone, instanceClone, WfRuntime);

                string strNextState = SMTWorkFlowManage.GetFlowNextStepRoles(WfRuntime, Instance, CurrentStateName, xml);
                bool isHigher = false;
                List<UserInfo> AppUserInfo = TmGetUserByRoleId(strNextState, UserID, PostID, ref isHigher);
                #region 打印审核人
                string names = "\r\n=======打印审核人D(strNextState" + strNextState + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                foreach (var user in AppUserInfo)
                {
                    names += "CompanyID:" + user.CompanyID + "\r\n";
                    names += "DepartmentID:" + user.DepartmentID + "\r\n";
                    names += "PostID:" + user.PostID + "\r\n";
                    names += "UserID:" + user.UserID + "\r\n";

                    names += "CompanyName:" + user.CompanyName + "\r\n";
                    names += "DepartmentName:" + user.DepartmentName + "\r\n";
                    names += "PostName:" + user.PostName + "\r\n";
                    names += "UserName:" + user.UserName + "\r\n";
                    names += "----------------------------------------------------\r\n";
                }
                #endregion
                if (AppUserInfo == null || AppUserInfo.Count == 0)
                {
                    DataResult.Err = "没有找到审核人";
                    DataResult.FlowResult = FlowResult.FAIL;
                }
                else if (AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
                {
                    DataResult.FlowResult = FlowResult.MULTIUSER;
                }
                DataResult.UserInfo = AppUserInfo;


            }
            catch (Exception ex)
            {
                Tracer.Debug("GetUserByInstance异常信息 ：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                Instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime);

            }


        }

        #endregion

        #region 通过持久化服务查询下一处理人(对应SubmitFlow)

        /// <summary>
        /// 通过持久化服务查询下一处理人(对应SubmitFlow)
        /// </summary>
        /// <param name="WfRuntimeClone">持久化运行时</param>
        /// <param name="instanceClone">持久化实例</param>
        /// <param name="xml">条件XML</param>
        /// <param name="CurrentStateName">当前状态代码</param>
        /// <param name="DataResult">操作结果</param>
        public void GetUserByInstance(string companyID, WorkflowRuntime WfRuntimeClone, WorkflowInstance instanceClone, string Layout, string xml, string CurrentStateName, List<string> UserID, List<string> PostID, FlowType FlowType, ref DataResult DataResult)
        {
            WorkflowRuntime WfRuntime = null;
            WorkflowInstance Instance = null;
            List<UserInfo> AppUserInfo = null;
            string strNextState = CurrentStateName;
            FlowRole RuleName;
            try
            {
                if (!WfRuntimeClone.IsStarted)
                {
                    WfRuntimeClone.StartRuntime();
                }
                WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
                Instance = SMTWorkFlowManage.CloneWorkflowInstance(WfRuntimeClone, instanceClone, WfRuntime);
                bool iscurruser = true;

                while (iscurruser)
                {
                    //   CurrentStateName = strNextState;
                    strNextState = SMTWorkFlowManage.GetFlowNextStepRoles(WfRuntime, Instance, strNextState, xml);
                    //if (FlowType == FlowType.Task && strNextState != "EndFlow")
                    //{
                    //    XmlReader XmlReader;

                    //    StringReader tmpLayout = new StringReader(Layout);
                    //    XmlReader = XmlReader.Create(tmpLayout);
                    //    XElement XElementS = XElement.Load(XmlReader);
                    //    var a = from c in XElementS.Descendants("Activity")
                    //            where c.Attribute("Name").Value == strNextState
                    //            select c.Attribute("RoleName").Value;
                    //    if (a.Count() > 0)
                    //    {
                    //        strNextState = a.First().ToString();
                    //    }
                    //    else
                    //    {
                    //        DataResult.Err = "没有找到对应角色";
                    //        DataResult.FlowResult = FlowResult.FAIL;
                    //        return;
                    //    }

                    //    tmpLayout = null;
                    //    XmlReader = null;
                    //    XElementS = null;
                    //    a = null;
                    //}


                    RuleName = FlowUtility.GetRlueName(Layout, strNextState);



                    if (RuleName == null)
                    {
                        DataResult.Err = "没有找到对应角色";
                        DataResult.FlowResult = FlowResult.FAIL;
                        return;
                    }

                    string tmpPostID = RuleName.UserType == "CREATEUSER" ? PostID[0] : PostID[1];
                    bool isHigher = false;
                    AppUserInfo = TmGetUserByRoleId(RuleName.RoleName, null, tmpPostID, ref isHigher);
                    #region 打印审核人
                    string names = "\r\n=======打印审核人E(RuleName.RoleName=" + RuleName.RoleName + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                    foreach (var user in AppUserInfo)
                    {
                        names += "CompanyID:" + user.CompanyID + "\r\n";
                        names += "DepartmentID:" + user.DepartmentID + "\r\n";
                        names += "PostID:" + user.PostID + "\r\n";
                        names += "UserID:" + user.UserID + "\r\n";
                      
                        names += "CompanyName:" + user.CompanyName + "\r\n";
                        names += "DepartmentName:" + user.DepartmentName + "\r\n";
                        names += "PostName:" + user.PostName + "\r\n";
                        names += "UserName:" + user.UserName + "\r\n";
                        names += "----------------------------------------------------\r\n";
                    }
                    if (!isHigher && RuleName.IsOtherCompany != null)
                    {
                        if (RuleName.IsOtherCompany.Value == true)
                        {
                            names += "是否指定公司：" + RuleName.IsOtherCompany.Value.ToString() + "\r\n";
                            names += "公司的ID：" + RuleName.OtherCompanyID + "\r\n";
                            if (string.IsNullOrEmpty(RuleName.OtherCompanyID))
                            {
                                names += "Layout=" + Layout + "\r\n";
                            }
                        }
                        else if (RuleName.IsOtherCompany.Value == false)
                        {
                            names += "实际要查找公司的ID:" + companyID + "\r\n";
                        }
                    }
                    
                    Tracer.Debug(names);
                    #endregion
                    #region beyond
                    if (!isHigher && RuleName.IsOtherCompany != null)
                    {
                        if (RuleName.IsOtherCompany.Value == true)
                        {
                            AppUserInfo = AppUserInfo.Where(user => user.CompanyID == RuleName.OtherCompanyID).ToList();
                        }
                        else if (RuleName.IsOtherCompany.Value == false)
                        {
                            AppUserInfo = AppUserInfo.Where(user => user.CompanyID == companyID).ToList();
                        }
                    }

                    #endregion
                    if (AppUserInfo == null || AppUserInfo.Count == 0)
                    {
                        DataResult.Err = "没有找到审核人";
                        DataResult.FlowResult = FlowResult.FAIL;
                        return;
                    }
                    if (AppUserInfo.Where(c => c.UserID == UserID[1]).Count() == 0)
                        iscurruser = false;
                }

                if (AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
                {
                    DataResult.FlowResult = FlowResult.MULTIUSER;
                }

                DataResult.AppState = strNextState;
                DataResult.UserInfo = AppUserInfo;


            }
            catch (Exception ex)
            {
                Tracer.Debug("GetUserByInstance异常信息 ：" + ex.ToString());
                throw new Exception("GetUserByInstance:" + ex.Message);
            }
            finally
            {
                strNextState = null;
                AppUserInfo = null;
                RuleName = null;
                Instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime);

            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="WfRuntimeClone"></param>
        /// <param name="instanceClone"></param>
        /// <param name="Layout">从审核主表记录ACTIVEROLE字段获取</param>
        /// <param name="xml"></param>
        /// <param name="CurrentStateName">当前状态</param>
        /// <param name="UserID"></param>
        /// <param name="PostID"></param>
        /// <param name="FlowType"></param>
        /// <param name="DataResult"></param>
        public void GetUserByInstance2(string companyID, WorkflowRuntime WfRuntimeClone, WorkflowInstance instanceClone, string Layout, string xml, string CurrentStateName, List<string> UserID, List<string> PostID, FlowType FlowType, ref DataResult DataResult,ref FlowUser fUser)
        {
            WorkflowRuntime WfRuntime = null;
            WorkflowInstance Instance = null;
            List<UserInfo> AppUserInfo = null;
            string strNextState = CurrentStateName;
            bool IsCountersign = false;
            string CountersignType = "0";
            //Role_UserType RuleName;
            //List<UserInfo> AppUserInfo = null;
            Dictionary<FlowRole, List<UserInfo>> DictCounterUser = null;
            try
            {
                if (!WfRuntimeClone.IsStarted)
                {
                    WfRuntimeClone.StartRuntime();
                }
                WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
                Instance = SMTWorkFlowManage.CloneWorkflowInstance(WfRuntimeClone, instanceClone, WfRuntime);
                bool iscurruser = true;
                int testflag = 0;
                while (iscurruser)
                {
                    testflag++;
                    if (testflag > 10)
                    {
                        throw new Exception("循环处理流程超过10次，请联系系统管理员");
                    }
                    #region


                    strNextState = SMTWorkFlowManage.GetFlowNextStepRoles(WfRuntime, Instance, strNextState, xml);
                    List<FlowRole> listRole = FlowUtility.GetRlueIdFromActivitID(Layout, strNextState, ref IsCountersign, ref CountersignType);
                    if (listRole.Count == 0)
                    {
                        DataResult.Err = "没有找到对应角色";
                        DataResult.FlowResult = FlowResult.FAIL;
                        return;
                    }

                    if (!IsCountersign)
                    {
                        #region
                        string tmpPostID = listRole[0].UserType == "CREATEUSER" ? PostID[0] : PostID[1];
                        bool isHigher = false;
                        AppUserInfo = TmGetUserByRoleId(listRole[0].RoleName, null, tmpPostID, ref isHigher);
                        #region 打印审核人
                        string names = "\r\nFormID=" + fUser.FormID + ";=======打印审核人F(listRole[0].RoleName=" + listRole[0].RoleName + ";审核人数量=" + AppUserInfo.Count + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                        foreach (var user in AppUserInfo)
                        {
                            names += "CompanyID:" + user.CompanyID + "\r\n";
                            names += "DepartmentID:" + user.DepartmentID + "\r\n";
                            names += "PostID:" + user.PostID + "\r\n";
                            names += "UserID:" + user.UserID + "\r\n";

                            names += "CompanyName:" + user.CompanyName + "\r\n";
                            names += "DepartmentName:" + user.DepartmentName + "\r\n";
                            names += "PostName:" + user.PostName + "\r\n";
                            names += "UserName:" + user.UserName + "\r\n";
                            names += "----------------------------------------------------\r\n";
                        }
                        if (!isHigher && listRole[0].IsOtherCompany != null)
                        {
                            if (listRole[0].IsOtherCompany.Value == true)
                            {
                                names += "是否指定公司：" + listRole[0].IsOtherCompany.Value.ToString() + "\r\n";
                                names += "公司的ID：" + listRole[0].OtherCompanyID + "\r\n";
                                if (string.IsNullOrEmpty(listRole[0].OtherCompanyID))
                                {
                                    names += "Layout=" + Layout + "\r\n";
                                }
                            }
                            else if (listRole[0].IsOtherCompany.Value == false)
                            {
                                names += "实际要查找公司的ID:" + companyID + "\r\n";
                            }
                        }
                        fUser.ErrorMsg += names;
                        Tracer.Debug(names);
                        #endregion
                        #region beyond

                        if (!isHigher && strNextState.ToUpper() != "ENDFLOW")
                        {
                            if (listRole[0].IsOtherCompany != null && listRole[0].IsOtherCompany.Value == true)
                            {
                                AppUserInfo = AppUserInfo.Where(user => user.CompanyID == listRole[0].OtherCompanyID).ToList();
                            }
                            else
                            {
                                AppUserInfo = AppUserInfo.Where(user => user.CompanyID == companyID).ToList();
                            }


                        }


                        #endregion
                        if (AppUserInfo == null || AppUserInfo.Count == 0)
                        {
                            DataResult.Err = "当前角色 " + listRole[0].Remark + " 没有找到审核人" ;
                            DataResult.FlowResult = FlowResult.FAIL;
                            return;
                        }


                        if (AppUserInfo.Where(c => c.UserID == UserID[1]).Count() == 0)
                            iscurruser = false;
                        #endregion
                    }
                    else
                    {
                        #region
                        DictCounterUser = new Dictionary<FlowRole, List<UserInfo>>();
                        if (CountersignType == "0")
                        {
                            #region
                            for (int i = 0; i < listRole.Count; i++)
                            {
                                string tmpPostID = listRole[i].UserType == "CREATEUSER" ? PostID[0] : PostID[1];
                                bool isHigher = false;
                                var listuserinfo = TmGetUserByRoleId(listRole[i].RoleName, null, tmpPostID, ref isHigher);
                                #region 打印审核人
                                string names = "\r\n=======打印审核人G(listRole[i].RoleName=" + listRole[0].RoleName + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                                foreach (var user in listuserinfo)
                                {
                                    names += "CompanyID:" + user.CompanyID + "\r\n";
                                    names += "DepartmentID:" + user.DepartmentID + "\r\n";
                                    names += "PostID:" + user.PostID + "\r\n";
                                    names += "UserID:" + user.UserID + "\r\n";
                                   
                                    names += "CompanyName:" + user.CompanyName + "\r\n";
                                    names += "DepartmentName:" + user.DepartmentName + "\r\n";
                                    names += "PostName:" + user.PostName + "\r\n";
                                    names += "UserName:" + user.UserName + "\r\n";
                                    names += "----------------------------------------------------\r\n";
                                }
                                if (!isHigher && listRole[0].IsOtherCompany != null)
                                {
                                    if (listRole[0].IsOtherCompany.Value == true)
                                    {
                                        names += "是否指定公司：" + listRole[0].IsOtherCompany.Value.ToString() + "\r\n";
                                        names += "公司的ID：" + listRole[0].OtherCompanyID + "\r\n";
                                        if (string.IsNullOrEmpty(listRole[0].OtherCompanyID))
                                        {
                                            names += "Layout=" + Layout + "\r\n";
                                        }
                                    }
                                    else if (listRole[0].IsOtherCompany.Value == false)
                                    {
                                        names += "实际要查找公司的ID:" + companyID + "\r\n";
                                    }
                                }
                                fUser.ErrorMsg += names;
                                Tracer.Debug(names);
                                #endregion
                                if (!isHigher)
                                {
                                    if (listRole[i].IsOtherCompany != null && listRole[i].IsOtherCompany.Value == true)
                                    {
                                        listuserinfo = listuserinfo.Where(user => user.CompanyID == listRole[i].OtherCompanyID).ToList();
                                    }
                                    else
                                    {
                                        listuserinfo = listuserinfo.Where(user => user.CompanyID == companyID).ToList();
                                    }
                                }

                                if (listuserinfo == null || listuserinfo.Count == 0)
                                {
                                    DataResult.Err = "角色 " + listRole[i].Remark + "没有找到审核人";
                                    DataResult.FlowResult = FlowResult.FAIL;
                                    return;
                                }
                                DictCounterUser.Add(listRole[i], listuserinfo);
                            }
                            iscurruser = false;
                            #endregion
                        }
                        else
                        {
                            #region
                            string roleNames = "";//所有角色名称
                            iscurruser = false;
                            bool bFlag = false;
                            for (int i = 0; i < listRole.Count; i++)
                            {
                                roleNames += listRole[i].Remark + "、";
                                string tmpPostID = listRole[i].UserType == "CREATEUSER" ? PostID[0] : PostID[1];
                                bool isHigher = false;
                                var listuserinfo = TmGetUserByRoleId(listRole[i].RoleName, null, tmpPostID, ref isHigher);
                                #region 打印审核人
                                string names = "\r\n=======打印审核人H(listRole[0].RoleName=" + listRole[i].RoleName + ";UserID=" + UserID + ";PostID=" + tmpPostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                                foreach (var user in listuserinfo)
                                {
                                    names += "CompanyID:" + user.CompanyID + "\r\n";
                                    names += "DepartmentID:" + user.DepartmentID + "\r\n";
                                    names += "PostID:" + user.PostID + "\r\n";
                                    names += "UserID:" + user.UserID + "\r\n";
                                    

                                    names += "CompanyName:" + user.CompanyName + "\r\n";
                                    names += "DepartmentName:" + user.DepartmentName + "\r\n";
                                    names += "PostName:" + user.PostName + "\r\n";
                                    names += "UserName:" + user.UserName + "\r\n";
                                    names += "----------------------------------------------------\r\n";
                                }
                                if (!isHigher && listRole[0].IsOtherCompany != null)
                                {
                                    if (listRole[0].IsOtherCompany.Value == true)
                                    {
                                        names += "是否指定公司：" + listRole[0].IsOtherCompany.Value.ToString() + "\r\n";
                                        names += "公司的ID：" + listRole[0].OtherCompanyID + "\r\n";
                                        if (string.IsNullOrEmpty(listRole[0].OtherCompanyID))
                                        {
                                            names += "Layout=" + Layout + "\r\n";
                                        }
                                    }
                                    else if (listRole[0].IsOtherCompany.Value == false)
                                    {
                                        names += "实际要查找公司的ID:" + companyID + "\r\n";
                                    }
                                }
                                fUser.ErrorMsg += names;
                                Tracer.Debug(names);
                                #endregion
                                if (!isHigher)
                                {
                                    if (listRole[i].IsOtherCompany != null && listRole[i].IsOtherCompany.Value == true)
                                    {
                                        listuserinfo = listuserinfo.Where(user => user.CompanyID == listRole[i].OtherCompanyID).ToList();
                                    }
                                    else
                                    {
                                        listuserinfo = listuserinfo.Where(user => user.CompanyID == companyID).ToList();
                                    }
                                }
                                if (listuserinfo != null && listuserinfo.Count > 0)
                                {
                                    bFlag = true;
                                    if (listuserinfo.FirstOrDefault(u => u.UserID == UserID[1]) != null)
                                    {
                                        iscurruser = true;
                                        break;
                                    }
                                    //DataResult.Err = "没有找到审核人";
                                    //DataResult.FlowResult = FlowResult.FAIL;
                                    //return;
                                }
                                DictCounterUser.Add(listRole[i], listuserinfo);
                            }
                            if (!bFlag)
                            {
                                DataResult.Err = "当前的角色 " + roleNames + " 没有找到审核人";
                                DataResult.FlowResult = FlowResult.FAIL;
                                return;
                            }
                            #endregion
                        }
                        #endregion
                    }


                    #endregion
                }
                DataResult.IsCountersign = IsCountersign;
                DataResult.AppState = strNextState;
                DataResult.CountersignType = CountersignType;
                if (!IsCountersign)
                {
                    #region
                    if (AppUserInfo != null && AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
                    {
                        DataResult.FlowResult = FlowResult.MULTIUSER;
                    }
                    DataResult.UserInfo = AppUserInfo;
                    #endregion
                }
                else
                {
                    #region
                    if (DataResult.DictCounterUser == null)
                    {
                        DataResult.DictCounterUser = new Dictionary<FlowRole, List<UserInfo>>();
                    }
                    DataResult.DictCounterUser = DictCounterUser;

                    List<FlowRole> listkeys = DictCounterUser.Keys.ToList();
                    for (int i = 0; i < listkeys.Count; i++)
                    {
                        FlowRole key = listkeys[i];
                        if (DictCounterUser[key].Count > 1)
                        {
                            DataResult.FlowResult = FlowResult.Countersign;
                            break;
                        }
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                //throw new Exception("GetUserByInstance2:" + ex.Message);//旧的
                Tracer.Debug("FORMID="+fUser.FormID+";通过实体例查找用户Instance=" + Instance.InstanceId.ToString()+" 异常信息:\r\n" + ex.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                strNextState = null;
                AppUserInfo = null;
                //RuleName = null;
                Instance = null;
                SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime);

            }


        }




        #endregion

        #region 通过状态代码查询下一处理人
        /// <summary>
        /// 通过状态代码查询下一处理人(对服务操作)
        /// </summary>
        /// <param name="RoleId">状态代码(角色ID(RoleName))</param>
        /// <returns></returns>
        private List<UserInfo> TmGetUserByRoleId(string RoleId, string UserID, string PostID, ref bool isHigher)
        {
            try
            {

                string CurrentRoleId = RoleId == "EndFlow" ? "End" : RoleId; //取得当前状态
                List<UserInfo> listUser = new List<UserInfo>();
                if (CurrentRoleId != "End")
                {
                    string WFCurrentRoleId = "";
                    int isDirect = 0;
                    #region 是否是直接上级,隔级上级,部门负责人
                    foreach (Higher c in Enum.GetValues(typeof(Higher)))
                    {
                        if (CurrentRoleId.ToUpper() == c.ToString().ToUpper())
                        {
                            isDirect = (int)c;//== 1 ? true : false;

                            WFCurrentRoleId = CurrentRoleId;
                            isHigher = true;
                        }
                    }
                    #endregion

                    if (WFCurrentRoleId != "")
                    {
                        //PermissionService
                        #region 如果是直接上级,隔级上级,部门负责人
                        Tracer.Debug("UserID=" + UserID + " 开始调用 直接上级,隔级上级,部门负责人：WcfPersonnel.GetEmployeeLeaders(岗位ID=" + PostID + ", isDirect=" + isDirect + ")");

                        List<SMT.HRM.CustomModel.V_EMPLOYEEVIEW> User = new List<SMT.HRM.CustomModel.V_EMPLOYEEVIEW>();
                        using (EmployeeBLL bll = new EmployeeBLL())
                        {
                            User = bll.GetEmployeeLeaders(PostID, isDirect);
                        }

                        //V_EMPLOYEEVIEW[] User = WcfPersonnel.GetEmployeeLeaders(PostID, isDirect);
                        string strtemp = "UserID=" + UserID + " 结束调用 直接上级,隔级上级,部门负责人：WcfPersonnel.GetEmployeeLeaders(岗位ID=" + PostID + ", isDirect=" + isDirect + ")";
                        if (User != null && User.Count > 0)
                        {
                            for (int i = 0; i < User.Count; i++)
                            {
                                UserInfo tmp = new UserInfo();
                                tmp.UserID = User[i].EMPLOYEEID;
                                tmp.UserName = User[i].EMPLOYEECNAME;
                                tmp.CompanyID = User[i].OWNERCOMPANYID;
                                tmp.DepartmentID = User[i].OWNERDEPARTMENTID;
                                tmp.PostID = User[i].OWNERPOSTID;

                                tmp.CompanyName = User[i].COMPANYNAME;
                                tmp.DepartmentName = User[i].DEPARTMENTNAME;
                                tmp.PostName = User[i].POSTNAME;
                                tmp.Roles = new List<TM_SaaS_OA_EFModel.T_SYS_ROLE>();
                                listUser.Add(tmp);  
                                strtemp += "公司ID   = " + User[i].EMPLOYEEID + "\r\n";
                                strtemp += "部门ID   = " + User[i].OWNERDEPARTMENTID + "\r\n";
                                strtemp += "岗位ID   = " + User[i].OWNERPOSTID + "\r\n";
                                strtemp += "员工ID   = " + User[i].EMPLOYEEID + "\r\n";

                                strtemp += "公司名称 = " + User[i].COMPANYNAME + "\r\n";
                                strtemp += "部门名称 = " + User[i].DEPARTMENTNAME + "\r\n";
                                strtemp += "岗位名称 = " + User[i].POSTNAME + "\r\n";
                                strtemp += "员工姓名 = " + User[i].EMPLOYEECNAME + "\r\n";
                            }
                        }
                        #endregion
                        Tracer.Debug(strtemp);
                    }
                    else
                    {
                        #region 根据角色ID查找人
                        Tracer.Debug("UserID=" + UserID + " 开始调用 检索本状态（角色）对应用户：WcfPermissionService.GetFlowUserInfoByRoleID(角色ID=" + WFCurrentRoleId + ")");
                        WFCurrentRoleId = new Guid(CurrentRoleId).ToString("D");
                        
                        try
                        {
                            List<SMT.HRM.CustomModel.Permission.FlowUserInfo> User = new List<HRM.CustomModel.Permission.FlowUserInfo>();//新的接口
                            using (SysUserBLL bll = new SysUserBLL())
                            {
                                User= bll.GetFlowUserInfoByRoleID(WFCurrentRoleId);
                            }

                            string strRole = "UserID=" + UserID + " 结束调用 检索本状态（角色）对应用户：WcfPermissionService.GetFlowUserInfoByRoleID(角色ID=" + WFCurrentRoleId + ")\r\n";

                            if (User != null && User.Count > 0)
                            {
                                for (int i = 0; i < User.Count; i++)
                                {
                                    #region
                                    UserInfo tmp = new UserInfo();
                                    strRole += "公司ID   = " + User[i].CompayID + "\r\n";
                                    strRole += "部门ID   = " + User[i].DepartmentID + "\r\n";
                                    strRole += "岗位ID   = " + User[i].PostID + "\r\n";
                                    strRole += "员工ID   = " + User[i].UserID + "\r\n";

                                    strRole += "公司名称 = " + User[i].CompayName + "\r\n";
                                    strRole += "部门名称 = " + User[i].DepartmentName + "\r\n";
                                    strRole += "岗位名称 = " + User[i].PostName + "\r\n";
                                    strRole += "员工姓名 = " + User[i].EmployeeName + "\r\n";

                                    tmp.UserID = User[i].UserID;
                                    tmp.UserName = User[i].EmployeeName;
                                    tmp.CompanyID = User[i].CompayID;
                                    tmp.DepartmentID = User[i].DepartmentID;
                                    tmp.PostID = User[i].PostID;

                                    tmp.CompanyName = User[i].CompayName;
                                    tmp.DepartmentName = User[i].DepartmentName;
                                    tmp.PostName = User[i].PostName;
                                    tmp.Roles = new List<TM_SaaS_OA_EFModel.T_SYS_ROLE>();
                                    foreach (var role in User[i].Roles)
                                    {
                                        tmp.Roles.Add(role);
                                        strRole += "角色ID   = " + role.ROLEID + "\r\n";
                                        strRole += "角色名称 = " + role.ROLENAME + "\r\n";
                                    }
                                    listUser.Add(tmp);
                                    strRole += "\r\n";
                                    strRole += "==================================================================================\r\n";
                                    #endregion

                                }
                            }
                            Tracer.Debug(strRole);
                        }
                        catch (Exception ex)
                        {
                            Tracer.Debug("权限服务GetSysUserByRole异常信息 角色id：" + WFCurrentRoleId + "" + ex.ToString());
                            throw new Exception("下一审核人为空，请联系公司权限管理员检查角色下的人员,角色id：" + WFCurrentRoleId);
                        }
                        #endregion

                    }

                }
                else
                {
                    //已经到流程结束状态
                    UserInfo tmp = new UserInfo();
                    tmp.UserID = "End";
                    tmp.UserName = "End";

                    listUser.Add(tmp);
                }

                return listUser;
            }
            catch (Exception ex)
            {
                Tracer.Debug("通过状态代码调用权限服务出错查询下一处理人(对服务操作) GetUserByStateCode异常信息 ：" + ex.ToString());
                throw new Exception("调用权限服务出错,请联系管理员!");
                // return null ;
            }
            //finally
            //{
            //    WcfPermissionService.Close();
            //    WcfPermissionService = null;
            //    WcfPersonnel.Close();
            //    WcfPersonnel = null;
            //}


        }

        #endregion

        #region 检查是否已提交流程
        /// <summary>
        /// 检查是否已提交流程
        /// </summary>
        /// <param name="ApprovalData"></param>
        /// <param name="APPDataResult"></param>
        /// <returns></returns>
        public CheckResult CheckFlow( SubmitData ApprovalData, DataResult APPDataResult)
        {

            CheckResult CheckFlowResult = new CheckResult();
            try
            {

                CheckFlowResult.APPDataResult = APPDataResult;
                APPDataResult.RunTime += "---GetFlowInfoStart:" + DateTime.Now.ToString();
                List<FlowType> FlowTypeList = new List<FlowWFService.FlowType>();
                FlowTypeList.Add(ApprovalData.FlowType);
                List<FLOW_FLOWRECORDDETAIL_T> fd = FlowBLL.GetFlowInfo( ApprovalData.FormID, "", "", "0", ApprovalData.ModelCode, "", "", FlowTypeList);
                CheckFlowResult.fd = fd;
                APPDataResult.RunTime += "---GetFlowInfoEnd:" + DateTime.Now.ToString();

                if (ApprovalData.SubmitFlag == SubmitFlag.New)
                {

                    if (fd != null && fd.Count > 0)
                    {
                        CheckFlowResult.APPDataResult.FlowResult = FlowResult.SUCCESS;
                        CheckFlowResult.Flag = 0;
                        UserInfo AppUser = new UserInfo();//下一审核人
                        AppUser.UserID = fd[0].EDITUSERID;
                        AppUser.UserName = fd[0].EDITUSERNAME;
                        AppUser.CompanyID = fd[0].EDITCOMPANYID;
                        AppUser.DepartmentID = fd[0].EDITDEPARTMENTID;
                        AppUser.PostID = fd[0].EDITPOSTID;

                        CheckFlowResult.APPDataResult.UserInfo.Add(AppUser);
                        CheckFlowResult.APPDataResult.AppState = fd[0].STATECODE;

                        return CheckFlowResult;
                    }

                }
                else
                {
                    if (fd == null || fd.Count == 0)
                    {
                        CheckFlowResult.APPDataResult.FlowResult = FlowResult.FAIL;
                        CheckFlowResult.APPDataResult.Err = "没有待审批节点，请检查流程是否已经结束或流程有异常!";
                        CheckFlowResult.Flag = 0;

                        return CheckFlowResult;
                    }
                    else
                    {

                        if (fd.Where(c => c.EDITUSERID == ApprovalData.ApprovalUser.UserID || c.AGENTUSERID == ApprovalData.ApprovalUser.UserID).ToList().Count == 0)
                        {
                            APPDataResult.FlowResult = FlowResult.SUCCESS;
                            CheckFlowResult.Flag = 0;
                            UserInfo AppUser = new UserInfo();
                            AppUser.UserID = fd[0].EDITUSERID;
                            AppUser.UserName = fd[0].EDITUSERNAME;
                            AppUser.CompanyID = fd[0].EDITCOMPANYID;
                            AppUser.DepartmentID = fd[0].EDITDEPARTMENTID;
                            AppUser.PostID = fd[0].EDITPOSTID;

                            CheckFlowResult.APPDataResult.UserInfo.Add(AppUser);
                            CheckFlowResult.APPDataResult.AppState = fd[0].STATECODE;

                            return CheckFlowResult;
                        }

                    }
                }
                CheckFlowResult.Flag = 1;
                return CheckFlowResult;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                CheckFlowResult = null;
            }
        }
        /// <summary>
        /// 检查是否已提交流程(对数据库操作)
        /// </summary>
        /// <param name="ApprovalData"></param>
        /// <param name="APPDataResult"></param>
        /// <returns></returns>
        public CheckResult CheckFlowIsApproved( SubmitData ApprovalData, DataResult APPDataResult)
        {
            CheckResult CheckFlowResult = new CheckResult();
            try
            {

                CheckFlowResult.APPDataResult = APPDataResult;
                APPDataResult.RunTime += "---GetFlowInfoStart:" + DateTime.Now.ToString();
                List<FlowType> FlowTypeList = new List<FlowWFService.FlowType>();
                FlowTypeList.Add(ApprovalData.FlowType);
                List<FLOW_FLOWRECORDDETAIL_T> fd = FlowBLL.GetFlowInfo( ApprovalData.FormID, "", "", "0", ApprovalData.ModelCode, "", "", FlowTypeList);//对数据库操作
                CheckFlowResult.fd = fd;
                APPDataResult.RunTime += "---GetFlowInfoEnd:" + DateTime.Now.ToString();

                if (ApprovalData.SubmitFlag == SubmitFlag.New)
                {
                    #region
                    if (fd != null && fd.Count > 0)
                    {
                        CheckFlowResult.APPDataResult.FlowResult = FlowResult.SUCCESS;
                        CheckFlowResult.Flag = 0;
                        UserInfo AppUser = new UserInfo();//下一审核人
                        AppUser.UserID = fd[0].EDITUSERID;
                        AppUser.UserName = fd[0].EDITUSERNAME;
                        AppUser.CompanyID = fd[0].EDITCOMPANYID;
                        AppUser.DepartmentID = fd[0].EDITDEPARTMENTID;
                        AppUser.PostID = fd[0].EDITPOSTID;

                        CheckFlowResult.APPDataResult.UserInfo.Add(AppUser);
                        CheckFlowResult.APPDataResult.AppState = fd[0].STATECODE;

                        return CheckFlowResult;
                    }
                    #endregion

                }
                else if (ApprovalData.SubmitFlag == SubmitFlag.Cancel)
                {
                }

                else
                {
                    if (fd == null || fd.Count == 0)
                    {
                        CheckFlowResult.APPDataResult.FlowResult = FlowResult.FAIL;
                        CheckFlowResult.APPDataResult.Err = "没有待审批节点，请检查流程是否已经结束或流程有异常!";
                        CheckFlowResult.Flag = 0;

                        return CheckFlowResult;
                    }
                    else
                    {

                        if (fd.Where(c => c.EDITUSERID == ApprovalData.ApprovalUser.UserID || c.AGENTUSERID == ApprovalData.ApprovalUser.UserID).ToList().Count == 0)
                        {
                            APPDataResult.FlowResult = FlowResult.SUCCESS;
                            CheckFlowResult.Flag = 0;
                            UserInfo AppUser = new UserInfo();
                            AppUser.UserID = fd[0].EDITUSERID;
                            AppUser.UserName = fd[0].EDITUSERNAME;
                            AppUser.CompanyID = fd[0].EDITCOMPANYID;
                            AppUser.DepartmentID = fd[0].EDITDEPARTMENTID;
                            AppUser.PostID = fd[0].EDITPOSTID;

                            CheckFlowResult.APPDataResult.UserInfo.Add(AppUser);
                            CheckFlowResult.APPDataResult.AppState = fd[0].STATECODE;

                            return CheckFlowResult;
                        }

                    }


                }
                CheckFlowResult.Flag = 1;
                return CheckFlowResult;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                CheckFlowResult = null;
            }
        }
        #endregion




        #region 查询是否使用代理
        /// <summary>
        /// 查询是否使用代理(对服务操作)
        /// </summary>
        /// <param name="ModelCode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public UserInfo GetAgentUserInfo(string ModelCode, string UserID)
        {
            UserInfo AgentUser = new UserInfo();
            try
            {
                //OAWS.AgentServicesClient oa = new OAWS.AgentServicesClient();
                //OAWS.T_HR_EMPLOYEE AGENTSET = oa.GetQueryAgent("TravelApplication1", "da844654-49c4-4138-ad83-e369cf03af5c");
                //OAWS.T_HR_EMPLOYEE AGENTSET = oa.GetQueryAgent(UserID, ModelCode);
                //if (AGENTSET == null)
                //    return null;

                //AgentUser.UserID = AGENTSET.EMPLOYEEID;//"userid0";
                //AgentUser.UserName = AGENTSET.EMPLOYEECNAME;//"testuser";

                return AgentUser;
            }
            catch (Exception e)
            {
                
                Tracer.Debug("ModelCode=" + ModelCode + ",UserID=" + UserID + " ;查询是否使用代理出错,异常信息:\r\n" + e.ToString());
                throw new Exception("查询是否使用代理出错,请联系管理员!");
            }
            finally
            {
                AgentUser = null;

            }

        }
        /// <summary>
        /// 查找会签角色
        /// </summary>
        /// <param name="ModelCode"></param>
        /// <param name="dictUserInfo"></param>
        /// <returns></returns>
        public Dictionary<UserInfo, UserInfo> GetAgentUserInfo2(string ModelCode, Dictionary<FlowRole, List<UserInfo>> dictUserInfo)
        {
            Dictionary<UserInfo, UserInfo> dict = new Dictionary<UserInfo, UserInfo>();

            try
            {
                dictUserInfo.Values.ToList().ForEach(users =>
                {
                    users.ForEach(user =>
                    {
                        UserInfo AgentUser = new UserInfo();
                        //OAWS.AgentServicesClient oa = new OAWS.AgentServicesClient();
                        ////OAWS.T_HR_EMPLOYEE AGENTSET = oa.GetQueryAgent("TravelApplication1", "da844654-49c4-4138-ad83-e369cf03af5c");
                        //OAWS.T_HR_EMPLOYEE AGENTSET = oa.GetQueryAgent(user.UserID, ModelCode);
                        //if (AGENTSET != null)
                        //{
                        //    AgentUser.UserID = AGENTSET.EMPLOYEEID;//"userid0";
                        //    AgentUser.UserName = AGENTSET.EMPLOYEECNAME;//"testuser";
                        //    dict[user] = AgentUser;
                        //}
                    });
                });


                return dict;
            }
            catch (Exception e)
            {
                Tracer.Debug("ModelCode=" + ModelCode + "查找会签角色出错,异常信息:\r\n" + e.ToString());
                throw new Exception("查找会签角色出错:请联系管理员!");
            }
            finally
            {


            }

        }
        #endregion


        /// <summary>
        /// 构建引擎消息
        /// </summary>
        /// <param name="EngineMessageData"></param>
        /// <returns></returns>
        public StringBuilder BuildMessageData(MessageData EngineMessageData)
        {


            StringBuilder FlowResultXml = new StringBuilder(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            FlowResultXml.Append(Environment.NewLine);
            FlowResultXml.Append(@"    <System>");
            FlowResultXml.Append(Environment.NewLine);
            FlowResultXml.Append(@"       <Name>""" + EngineMessageData.MessageSystemCode + @"""</Name>");
            FlowResultXml.Append(Environment.NewLine);
            FlowResultXml.Append(@"       <SystemCode>""" + EngineMessageData.SystemCode + @"""</SystemCode>");
            FlowResultXml.Append(Environment.NewLine);
            FlowResultXml.Append(@"       <Message>");
            FlowResultXml.Append(@"           <Attribute Name=""CompanyID""  DataValue=""" + EngineMessageData.CompanyID + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""ModelCode""  DataValue=""" + EngineMessageData.ModelCode + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""ModelName""  DataValue=""" + EngineMessageData.ModelName + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""FormID""     DataValue=""" + EngineMessageData.FormID + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""StateCode""  DataValue=""" + EngineMessageData.StateCode + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""CheckState""  DataValue=""" + EngineMessageData.CheckState + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""IsTask""     DataValue=""" + EngineMessageData.IsTask + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""AppUserID""  DataValue=""" + EngineMessageData.AppUserID + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""AppUserName""  DataValue=""" + EngineMessageData.AppUserName + @"""></Attribute>");
            FlowResultXml.Append(@"           <Attribute Name=""OutTime""  DataValue=""" + EngineMessageData.KPITime + @"""></Attribute>");
            FlowResultXml.Append(@"       </Message>");
            FlowResultXml.Append(@"     </System>");

            return FlowResultXml;
        }

        public string GetFlowDefine( SubmitData ApprovalData)
        {
            try
            {
                FlowUser user = new FlowUser(ApprovalData.ApprovalUser.CompanyID, ApprovalData.ApprovalUser.UserID,  ApprovalData.ModelCode);
                user.TrackingMessage += "构建引擎消息FlowBLL.GetFlowDefine.GetFlowByModelName(ApprovalData.ApprovalUser.DepartmentID=" + ApprovalData.ApprovalUser.DepartmentID + ";OrgType='" + ((int)ApprovalData.FlowType).ToString() + ")'";

                List<FLOW_MODELFLOWRELATION_T> MODELFLOWRELATION = GetFlowByModelName( ApprovalData.ApprovalUser.CompanyID, ApprovalData.ApprovalUser.DepartmentID, ApprovalData.ModelCode, ((int)ApprovalData.FlowType).ToString(), ref user);


                if (MODELFLOWRELATION == null || MODELFLOWRELATION.Count == 0)
                {

                    return null;
                }
                return MODELFLOWRELATION.First().FLOW_FLOWDEFINE_T.LAYOUT;
            }
            catch (Exception e)
            {
                Tracer.Debug("构建引擎消息出错;异常信息\r\n" + e.ToString());
                throw e;
            }
        }

        public string IsExistFlowDataByUserID( string UserID, string PostID)
        {
            try
            {


                return FLOW_FLOWRECORDMASTER_TDAL.IsExistFlowDataByUserID( UserID, PostID);
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public List<FLOW_FLOWRECORDMASTER_T> GetFlowDataByUserID( string UserID)
        {
            try
            {

                List<FlowType> FlowTypeList = new List<FlowWFService.FlowType>();
                FlowTypeList.Add(FlowType.Approval);

                //FlowTypeList.Add(2);


                List<FLOW_FLOWRECORDDETAIL_T> fd = FlowBLL.GetFlowInfo( "", "", "", "0", "", "", UserID, FlowTypeList);
                List<FLOW_FLOWRECORDMASTER_T> fd2 = FlowBLL.GetFlowRecordBySubmitUserID("1", UserID);
                if (fd != null)
                {
                    if (fd2 == null)
                        fd2 = new List<FLOW_FLOWRECORDMASTER_T>();
                    foreach (FLOW_FLOWRECORDDETAIL_T item in fd)
                    {

                        fd2.Add(item.FLOW_FLOWRECORDMASTER_T);
                    }
                }

                return fd2;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #region 检查流程参数是否符合规则
        /// <summary>
        /// 检查流程参数是否符合规则
        /// </summary>
        /// <param name="ApprovalData"></param>
        /// <param name="APPDataResult"></param>
        /// <returns></returns>
        public static bool CheckFlowData(SubmitData ApprovalData, ref DataResult APPDataResult)
        {
            try
            {

                if (ApprovalData.FormID == null || ApprovalData.FormID == "")
                {
                    APPDataResult.Err = "业务对象的FORMID为空";
                    return false;
                }

                if (ApprovalData.ModelCode == null || ApprovalData.ModelCode == "")
                {
                    APPDataResult.Err = "模块代码为空";
                    return false;
                }

                //if (ApprovalData.SubmitFlag == null || (ApprovalData.SubmitFlag != SubmitFlag.New && ApprovalData.SubmitFlag != SubmitFlag.Approval))
                //{
                //    APPDataResult.Err = "流程提交标志(SubmitFlag)有误,需要设置成SubmitFlag.New或者SubmitFlag.Approval";
                //    return false;
                //}

                if (ApprovalData.SubmitFlag == null)
                {
                    APPDataResult.Err = "流程提交标志(SubmitFlag)不能为空";
                    return false;
                }

                if (ApprovalData.FlowSelectType == null || (ApprovalData.FlowSelectType != FlowSelectType.FixedFlow && ApprovalData.FlowSelectType != FlowSelectType.FreeFlow))
                {
                    APPDataResult.Err = "流程审批类型设置有误，应设置成FlowSelectType.FixedFlow或FlowSelectType.FreeFlow";
                    return false;
                }

                if (ApprovalData.ApprovalUser == null)
                {
                    APPDataResult.Err = "提交用户信息不能为空";
                    return false;
                }
                else if (ApprovalData.ApprovalUser.CompanyID == null || ApprovalData.ApprovalUser.CompanyID == "")
                {
                    APPDataResult.Err = "提交用户所属公司不能为空";
                    return false;
                }
                else if (ApprovalData.ApprovalUser.DepartmentID == null || ApprovalData.ApprovalUser.DepartmentID == "")
                {
                    APPDataResult.Err = "提交用户所属部门不能为空";
                    return false;
                }
                else if (ApprovalData.ApprovalUser.PostID == null || ApprovalData.ApprovalUser.PostID == "")
                {
                    APPDataResult.Err = "提交用户所属岗位不能为空";
                    return false;
                }
                else if (ApprovalData.ApprovalUser.UserID == null || ApprovalData.ApprovalUser.UserID == "")
                {
                    APPDataResult.Err = "提交用户ID不能为空";
                    return false;
                }
                else if (ApprovalData.ApprovalUser.UserName == null || ApprovalData.ApprovalUser.UserName == "")
                {
                    APPDataResult.Err = "提交用户名称不能为空";
                    return false;
                }

                if (ApprovalData.NextApprovalUser != null)
                {
                    if ((ApprovalData.NextApprovalUser.CompanyID != null && ApprovalData.NextApprovalUser.CompanyID != "")
                        || (ApprovalData.NextApprovalUser.DepartmentID != null && ApprovalData.NextApprovalUser.DepartmentID != "")
                        || (ApprovalData.NextApprovalUser.PostID != null && ApprovalData.NextApprovalUser.PostID != "")
                        || (ApprovalData.NextApprovalUser.UserID != null && ApprovalData.NextApprovalUser.UserID != "")
                        || (ApprovalData.NextApprovalUser.UserName != null && ApprovalData.NextApprovalUser.UserName != ""))
                    {
                        if (ApprovalData.NextStateCode == null || ApprovalData.NextStateCode == "")
                        {
                            //APPDataResult.Err = "设置了下一审核人时下一审核节点代码不能为空";
                            //return false;
                        }
                        else if (ApprovalData.NextApprovalUser.CompanyID == null || ApprovalData.NextApprovalUser.CompanyID == "")
                        {
                            //APPDataResult.Err = "下一审核用户所属公司不能为空";
                            //return false;
                        }
                        else if (ApprovalData.NextApprovalUser.DepartmentID == null || ApprovalData.NextApprovalUser.DepartmentID == "")
                        {
                            //APPDataResult.Err = "下一审核用户所属部门不能为空";
                            //return false;
                        }
                        else if (ApprovalData.NextApprovalUser.PostID == null || ApprovalData.NextApprovalUser.PostID == "")
                        {
                            //APPDataResult.Err = "下一审核用户所属岗位不能为空";
                            //return false;
                        }
                        else if (ApprovalData.NextApprovalUser.UserID == null || ApprovalData.NextApprovalUser.UserID == "")
                        {
                            //APPDataResult.Err = "下一审核用户ID不能为空";
                            //return false;
                        }
                        else if (ApprovalData.NextApprovalUser.UserName == null || ApprovalData.NextApprovalUser.UserName == "")
                        {
                            //APPDataResult.Err = "下一审核用户名称不能为空";
                            //return false;
                        }
                    }
                    else if (ApprovalData.NextStateCode != null && ApprovalData.NextStateCode != "")
                    {
                        //APPDataResult.Err = "未设置下一审核人时，不能设置下一审核节点代码";
                        //return false;
                    }
                }
                else if (ApprovalData.NextStateCode != null && ApprovalData.NextStateCode != "")
                {
                    //APPDataResult.Err = "未设置下一审核人时，不能设置下一审核节点代码";
                    //return false;
                }

                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        #endregion

        #region 2012/05/21 加入 亢
        Dictionary<string, string> lineList = new Dictionary<string, string>();

        /// <summary>
        /// 通过事项类型获取流程路径
        /// </summary>
        /// <param name="typeNumber">事项类型的值</param>
        /// <returns></returns>
        public string GetFlowPathByNumber(string typeNumber)
        {
            #region 找路径
            Dictionary<string, string> TypeName = new Dictionary<string, string>();
            TypeName.Add("41", "重要合同");
            TypeName.Add("461", "集团合同");
            TypeName.Add("35", "重要金额");
            //开始--[一级事项类型>=461]-->财务经理-在线---->副总经理---->助理总裁---->副总裁（刘小强）---->总裁---->结束；

            #endregion
            if (lineList.Count > 0)
            {
                if (lineList.ContainsKey(typeNumber))
                {
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[(.*?)\\]", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.MatchCollection matchs = reg.Matches(lineList[typeNumber].ToString());
                    for (int i = 0; i < matchs.Count; i++)
                    {
                        string value = matchs[i].Groups[1].Value;
                    }
                           
                    return lineList[typeNumber].ToString();
                }
                else
                {
                    return "流程条件出错";
                }
            }
            else
            {
                return "";
            }
        }
        private List<string> GetFlowType(string layoutXml)
        {
            try
            {
                List<string> list = new List<string>();               
                Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(layoutXml);
                XElement xElement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
                var lines = from item in xElement.Descendants("Rule")
                            where item.Attribute("StrStartActive").Value == "StartFlow"
                            select item;
                foreach (var line in lines)
                {
                    string conditions = "", conditionsValue = "", Operate = "";
                    if (line.Element("Conditions") != null)
                    {
                        conditions = "[" + line.Element("Conditions").Element("Condition").Attribute("Description").Value + "";
                        Operate = "" + line.Element("Conditions").Element("Condition").Attribute("Operate").Value + "";
                        conditionsValue = "" + line.Element("Conditions").Element("Condition").Attribute("CompareValue").Value + "]";
                    }
                    var Element = (from item in xElement.Descendants("Activity")
                                   where item.Attribute("Name").Value == line.Attribute("StrEndActive").Value
                                   select item).FirstOrDefault();
                    string path = "开始--" + conditions + Operate + conditionsValue + "-->" + Element.Attribute(XName.Get("Remark")).Value;
                    list = GetActivityPath(xElement, Element.Attribute(XName.Get("Name")).Value, path, list);
                    if (list[0].ToString() == "开始-->流程设计格式不正确")
                    {
                        return list;
                    }
                }
                #region 获取事项类型
                foreach (var li in list)
                {
                    if (li.Contains("事项类型") || li.Contains("审批类型"))
                    {
                        try
                        {
                            //开始--[一级事项类型==127]-->分支机构财务负责人5---->分支机构负责人5---->商务部副经理---->营销总监---->营运中心办公室助理总监---->财务中心办公室总监---->副总经理5---->法务主管---->总经理5---->结束；
                            //开始---->直接上级---->部门负责人--[事项审批类型==35]-->财务经理-在线---->律师---->副总经理--[一级事项类型==42]-->人力资源部负责人---->副总裁（刘小强）---->总裁---->结束；"
                            //开始--[一级事项类型>=461]-->财务经理-在线---->副总经理---->助理总裁---->副总裁（刘小强）---->总裁---->结束；

                            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[(.*?)\\]", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            System.Text.RegularExpressions.MatchCollection matchs = reg.Matches(li);
                            for (int i = 0; i < matchs.Count; ++i)
                            {
                                string charStr = "";
                                #region 比较字符
                                if (matchs[i].Groups[1].Value.IndexOf(">") > 0)
                                {
                                    charStr = ">";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf("<") > 0)
                                {
                                    charStr = "<";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf("==") > 0)
                                {
                                    charStr = "==";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf(">=") > 0)
                                {
                                    charStr = ">=";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf("<=") > 0)
                                {
                                    charStr = "<=";
                                }

                                #endregion
                                if (charStr != "")
                                {
                                    string name = System.Text.RegularExpressions.Regex.Split(matchs[i].Groups[1].Value, charStr)[1];
                                    if (lineList.ContainsKey(name))
                                    {
                                        lineList[name] = lineList[name] + "\r\n" + li;
                                    }
                                    else
                                    {
                                        lineList.Add(name, li);
                                    }
                                }
                            }
                        }
                        catch
                        { }
                    }

                }
                #endregion

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("GetFlowBreach:" + ex.Message);
            }
        }
        public List<string> GetFlowBranch(string FlowID)
        {
           
            try
            {
                List<string> list = new List<string>();
                string Xml = FLOW_FLOWDEFINE_TDAL.GetFlowBranch(FlowID);
                Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(Xml);
                XElement xElement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
                var lines = from item in xElement.Descendants("Rule")
                            where item.Attribute("StrStartActive").Value == "StartFlow"
                            select item;
                foreach (var line in lines)
                {
                    string conditions = "", conditionsValue = "", Operate = "";
                    if (line.Element("Conditions") != null)
                    {
                        conditions = "[" + line.Element("Conditions").Element("Condition").Attribute("Description").Value + "";
                        Operate = "" + line.Element("Conditions").Element("Condition").Attribute("Operate").Value + "";
                        conditionsValue = "" + line.Element("Conditions").Element("Condition").Attribute("CompareValue").Value + "]";
                    }
                    var Element = (from item in xElement.Descendants("Activity")
                                   where item.Attribute("Name").Value == line.Attribute("StrEndActive").Value
                                   select item).FirstOrDefault();
                    string path = "开始--" + conditions + Operate + conditionsValue + "-->" + Element.Attribute(XName.Get("Remark")).Value;
                    list = GetActivityPath(xElement, Element.Attribute(XName.Get("Name")).Value, path, list);
                    if (list[0].ToString() == "开始-->流程设计格式不正确")
                    {
                        return list;
                    }
                }
                #region 获取事项类型
               
                foreach (var li in list)
                {
                    if (li.Contains("事项类型") || li.Contains("审批类型"))
                    {
                        try
                        {
                            //开始--[一级事项类型==127]-->分支机构财务负责人5---->分支机构负责人5---->商务部副经理---->营销总监---->营运中心办公室助理总监---->财务中心办公室总监---->副总经理5---->法务主管---->总经理5---->结束；
                            //开始---->直接上级---->部门负责人--[事项审批类型==35]-->财务经理-在线---->律师---->副总经理--[一级事项类型==42]-->人力资源部负责人---->副总裁（刘小强）---->总裁---->结束；"
                            //开始--[一级事项类型>=461]-->财务经理-在线---->副总经理---->助理总裁---->副总裁（刘小强）---->总裁---->结束；

                             System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[(.*?)\\]", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                            //System.Text.RegularExpressions.Match match = reg.Match(li);
                            //for (int i = 1; i < match.Groups.Count; ++i)
                            //{

                            //    string anme = match[i].Groups[i].Value;
                            //}

                            System.Text.RegularExpressions.MatchCollection matchs = reg.Matches(li);
                            for (int i = 0; i < matchs.Count; i++)
                            {
                                string charStr = "";
                                #region 比较字符
                                if (matchs[i].Groups[1].Value.IndexOf(">") > 0)
                                {
                                    charStr = ">";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf("<") > 0)
                                {
                                    charStr = "<";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf("==") > 0)
                                {
                                    charStr = "==";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf(">=") > 0)
                                {
                                    charStr = ">=";
                                }
                                if (matchs[i].Groups[1].Value.IndexOf("<=") > 0)
                                {
                                    charStr = "<=";
                                }
                                
                                #endregion
                                if (charStr != "")
                                {
                                    //matchs[i].Groups[1].Value  =  一级事项类型==39
                                    string name = System.Text.RegularExpressions.Regex.Split(matchs[i].Groups[1].Value, charStr)[1];                                    
                                    if (lineList.ContainsKey(name))
                                    {
                                        lineList[name] = lineList[name] + "\r\n" + li;
                                    }
                                    else
                                    {
                                        lineList.Add(name, li);
                                    }
                                }
                            }                            
                        }
                        catch
                        { }
                    }

                }
                #endregion
                #region 找路径
                Dictionary<string, string> TypeName = new Dictionary<string, string>();
                TypeName.Add("41", "重要合同41");
                TypeName.Add("39", "集团合同39");
                TypeName.Add("35", "普通合同35");
                TypeName.Add("42", "在线合同");
               Dictionary<string, string> newList = new Dictionary<string, string>();
                //开始--[一级事项类型>=461]-->财务经理-在线---->副总经理---->助理总裁---->副总裁（刘小强）---->总裁---->结束；
                foreach (KeyValuePair<string, string> line in lineList)
                {

                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[(.*?)\\]", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.MatchCollection matchs = reg.Matches(line.Value);
                    for (int i = 0; i < matchs.Count; i++)
                    {
                        string charStr = "";
                        #region 比较字符
                        if (matchs[i].Groups[1].Value.IndexOf(">") > 0)
                        {
                            charStr = ">";
                        }
                        if (matchs[i].Groups[1].Value.IndexOf("<") > 0)
                        {
                            charStr = "<";
                        }
                        if (matchs[i].Groups[1].Value.IndexOf("==") > 0)
                        {
                            charStr = "==";
                        }
                        if (matchs[i].Groups[1].Value.IndexOf(">=") > 0)
                        {
                            charStr = ">=";
                        }
                        if (matchs[i].Groups[1].Value.IndexOf("<=") > 0)
                        {
                            charStr = "<=";
                        }

                        #endregion
                        if (charStr != "")
                        {
                            //matchs[i].Groups[1].Value  =  一级事项类型==39
                            string name = System.Text.RegularExpressions.Regex.Split(matchs[i].Groups[1].Value, charStr)[1];
                            string typeName = name;
                            if (TypeName.ContainsKey(typeName))
                            {
                                typeName = TypeName[typeName];//事项类型名称
                            }
                            string typeAllName = matchs[i].Groups[1].Value.Replace(name, typeName);// 一级事项类型==集团合同
                            if (!newList.ContainsKey(line.Key))
                            {
                                newList.Add(line.Key, line.Value.Replace(matchs[i].Groups[1].Value, typeAllName));
                            }
                            else
                            {
                                newList[line.Key] = newList[line.Key].Replace(matchs[i].Groups[1].Value, typeAllName);
                            }
                        }
                        else
                        {
                            newList.Add(line.Key, line.Value);
                        }
                    }
                }
                #endregion
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("GetFlowBreach:" + ex.Message);
            }
        }

       
        private List<string> GetActivityPath(XElement xElement, string ActivityID, string path, List<string> list)
        {
            string snap = path;//foreach中循环的变量
            var lines = from item in xElement.Descendants("Rule")
                        where item.Attribute("StrStartActive").Value == ActivityID
                        select item;
            foreach (var line in lines)
            {
                string conditions = "", conditionsValue = "", Operate = "";
                if (line.Element("Conditions") != null)
                {
                    conditions = "[" + line.Element("Conditions").Element("Condition").Attribute("Description").Value + "";
                    Operate = "" + line.Element("Conditions").Element("Condition").Attribute("Operate").Value + "";
                    conditionsValue = "" + line.Element("Conditions").Element("Condition").Attribute("CompareValue").Value + "]";
                }
                var Element = (from item in xElement.Descendants("Activity")
                               where item.Attribute("Name").Value == line.Attribute("StrEndActive").Value
                               select item).FirstOrDefault();
                if (line.Attribute("StrEndActive").Value == "EndFlow")
                {
                    if (list.Count() > 0 && list.Count() == 2 && list[0].ToString() == "开始-->流程设计格式不正确")
                    {
                        return list;
                    }
                    else
                    {                        
                        list.Add(snap + "---->结束；");                        
                    }
                    continue;
                }
                else
                {

                    if (snap.IndexOf("-->" + Element.Attribute(XName.Get("Remark")).Value + "--") < 1)
                    {
                        if (lines.Count() > 1)
                        {
                            path = snap + "--" + conditions + Operate + conditionsValue + "-->" + Element.Attribute(XName.Get("Remark")).Value;
                        }
                        else
                        {

                            path += "--" + conditions + Operate + conditionsValue + "-->" + Element.Attribute(XName.Get("Remark")).Value;
                        }
                        GetActivityPath(xElement, Element.Attribute(XName.Get("Name")).Value, path, list);
                    }
                    else
                    {
                        #region 获取事项类型
                        foreach (var li in list)
                        {
                            if (li.Split('>')[0].Contains("事项类型"))
                            {
                                try
                                {
                                    //开始--[一级事项类型==127]-->分支机构财务负责人5---->分支机构负责人5---->商务部副经理---->营销总监---->营运中心办公室助理总监---->财务中心办公室总监---->副总经理5---->法务主管---->总经理5---->结束；
                                    //开始---->直接上级---->部门负责人--[事项审批类型==35]-->财务经理-在线---->律师---->副总经理--[一级事项类型==42]-->人力资源部负责人---->副总裁（刘小强）---->总裁---->结束；"
                                    //开始--[一级事项类型>=461]-->财务经理-在线---->副总经理---->助理总裁---->副总裁（刘小强）---->总裁---->结束；
                                    string x = System.Text.RegularExpressions.Regex.Split(System.Text.RegularExpressions.Regex.Split(li, "]-->")[0], "==")[1];
                                    
                                    // tring str = "我是[001]真心求救的[002]，你能帮帮我吗";
                                    //Pattern pattern = Pattern.compile("\\[(.*?)\\]");
                                    //Matcher matcher = pattern.matcher(str);
                                    //while(matcher.find()){
                                    //    System.out.println(matcher.group(1));
                                    //}

                                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[(.*?)\\]", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                                    //System.Text.RegularExpressions.Match match = reg.Match(li);
                                    //for (int i = 1; i < match.Groups.Count; ++i)
                                    //{

                                    //    string anme = match[i].Groups[i].Value;
                                    //}

                                    System.Text.RegularExpressions.MatchCollection matchs = reg.Matches(li);
                                    for (int i = 0; i < matchs.Count; ++i)
                                    {
                                        string charStr = "";
                                        #region 比较字符
                                        if (matchs[i].Groups[1].Value.IndexOf("==") > 0)
                                        {
                                            charStr = "=="; 
                                        }
                                        if (matchs[i].Groups[1].Value.IndexOf(">=") > 0)
                                        {
                                            charStr = ">=";
                                        }
                                        if (matchs[i].Groups[1].Value.IndexOf("<=") > 0)
                                        {
                                            charStr = "<=";
                                        }
                                        if (matchs[i].Groups[1].Value.IndexOf(">") > 0)
                                        {
                                            charStr = ">";
                                        }
                                        if (matchs[i].Groups[1].Value.IndexOf("<") > 0)
                                        {
                                            charStr = "<";
                                        }
                                        #endregion
                                        if (charStr != "")
                                        {
                                            string name = System.Text.RegularExpressions.Regex.Split(matchs[i].Groups[1].Value, charStr)[1];
                                            if (lineList.ContainsKey(name))
                                            {
                                                lineList[name] = lineList[name] + "\r\n" + li;
                                            }
                                            else
                                            {
                                                lineList.Add(name, li);
                                            }
                                        }
                                    }                                   

                                    //string name = li.Replace("]-->", "|").Split('|')[0].Replace("==", "|").Split('|')[1];
                                    //if (lineList.ContainsKey(name))
                                    //{
                                    //    lineList[name] = lineList[name] + "\r\n" + li;
                                    //}
                                    //else
                                    //{
                                    //    lineList.Add(name, li);
                                    //}
                                }
                                catch
                                { }
                            }

                        }
                        #endregion
                        list.Clear();
                        list.Add("开始-->流程设计格式不正确");
                        list.Add("错误分支：" + snap + "--" + conditions + Operate + conditionsValue + "-->" + Element.Attribute(XName.Get("Remark")).Value + "");
                        return list;
                    }
                }
            }
            return list;
        }
        #endregion

        #region 对外接口获到我的单据实体
        public T_WF_PERSONALRECORD GetPersonalRecordByID(string id)
        {
           return T_WF_PERSONALRECORDDAL.GetPersonalRecordByPersonalrecordid(id);
        }
        #endregion
    }
}

