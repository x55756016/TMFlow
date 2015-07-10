/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©  SmtOnline  2011    
	 * 文件名：Service2.cs  
	 * 创建者：提莫科技   
	 * 创建日期：2011/12/15 08:51:55   
	 * CLR版本： 4.0.30319.1  
	 * 命名空间：SMT.FlowWFService
	 * 模块名称：
	 * 描　　述： 对原有的Service.cs进行重写，加入了//OracleConnection作为参数，以便作事务处理
* ---------------------------------------------------------------------*/
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
using SMT.SaaS.BLLCommonServices.PermissionWS;
using SMT.Workflow.Common.Model.FlowEngine;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Core;
using SMT.FlowWFService.XmlFlowManager;

namespace SMT.FlowWFService.NewFlow
{
    public partial class Service
    {
        public DataResult SubimtFlow(SubmitData submitData)
        {
            #region 测试用例
            //submitData.FlowSelectType = FlowSelectType.FixedFlow;
            //submitData.FormID = "0671c1d0-07b9-4baa-9a36-349f0c09bd04";
            //submitData.ModelCode = "T_WP_YEARNORMDRAFT";
            //submitData.ApprovalUser = new UserInfo();
            //submitData.ApprovalUser.CompanyID = "721c99d5-4cfb-4cc7-ba43-a05217e25918";
            //submitData.ApprovalUser.DepartmentID = "7fc6b3af-92ec-442b-921e-6a8d13039ce9";
            //submitData.ApprovalUser.PostID = "aa1496a5-840b-4b15-bc3e-36c2aeb14d3b";
            //submitData.ApprovalUser.UserID = "e65cdb8f-26f2-440c-a5f9-977ea29fe4c2";
            //submitData.ApprovalUser.UserName = "黄总";
            //submitData.NextStateCode = "";
            //submitData.NextApprovalUser = new UserInfo();
            //submitData.NextApprovalUser.CompanyID = "721c99d5-4cfb-4cc7-ba43-a05217e25918";
            //submitData.NextApprovalUser.DepartmentID = "e6e9546c-2d58-4f1c-abb6-139df4055114";
            //submitData.NextApprovalUser.PostID = "fdbe688e-8886-42ac-9392-914e4e20960b";
            //submitData.NextApprovalUser.UserID = "7e2fb3cd-ff1a-47b4-91b0-1572e6a3f3e5";
            //submitData.NextApprovalUser.UserName = "王董";
            //submitData.SubmitFlag = SubmitFlag.Approval;
            //submitData.FlowType = FlowType.Approval;
            //submitData.ApprovalResult = ApprovalResult.Pass;
            //submitData.ApprovalContent = "审核通过";
            //submitData.SumbitCompanyID = "";
            //submitData.SumbitDeparmentID = "";
            //submitData.SumbitPostID = "";
            //submitData.SumbitUserID = "";
            //submitData.SumbitUserName = "";


            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            //sb.AppendLine("<System>");
            //sb.AppendLine("  <Name>WP</Name>");
            //sb.AppendLine("  <Version>1.0</Version>");
            //sb.AppendLine("  <System>");
            //sb.AppendLine("    <Function Description=\"\" Address=\"\" FuncName=\"\" Binding=\"\" SplitChar=\"Г\">");
            //sb.AppendLine("      <ParaStr>");
            //sb.AppendLine("        <Para TableName=\"\" Name=\"\" Description=\"\" Value=\"\" />");
            //sb.AppendLine("      </ParaStr>");
            //sb.AppendLine("    </Function>");
            //sb.AppendLine("  </System>");
            //sb.AppendLine("  <MsgOpen>");
            //sb.AppendLine("    <AssemblyName>SMT.WP.UI</AssemblyName>");
            //sb.AppendLine("    <PublicClass>SMT.WP.UI.Class.Utility</PublicClass>");
            //sb.AppendLine("    <ProcessName>CreateFormFromEngine</ProcessName>");
            //sb.AppendLine("    <PageParameter>WP/YearNormDraft/Audit</PageParameter>");
            //sb.AppendLine("    <ApplicationOrder>{DNORMID}</ApplicationOrder>");
            //sb.AppendLine("    <FormTypes>Audit</FormTypes>");
            //sb.AppendLine("  </MsgOpen>");
            //sb.AppendLine("  <Object Name=\"T_WP_YEARNORMDRAFT\" Description=\"公司年度经营指标方案\" Key=\"DNORMID\" id=\"0671c1d0-07b9-4baa-9a36-349f0c09bd04\">");
            //sb.AppendLine("    <Attribute Name=\"DNORMID\" LableResourceID=\"DNORMID\" Description=\"指标主键ID,GUID\" DataType=\"string\" DataValue=\"0671c1d0-07b9-4baa-9a36-349f0c09bd04\" DataText=\"0671c1d0-07b9-4baa-9a36-349f0c09bd04\" />");
            //sb.AppendLine("    <Attribute Name=\"PLANYEAR\" LableResourceID=\"PLANYEAR\" Description=\"计划年度\" DataType=\"string\" DataValue=\"2009\" DataText=\"2009\" />");
            //sb.AppendLine("    <Attribute Name=\"COMPANYID\" LableResourceID=\"COMPANYID\" Description=\"所属公司ID\" DataType=\"string\" DataValue=\"721c99d5-4cfb-4cc7-ba43-a05217e25918\" DataText=\"721c99d5-4cfb-4cc7-ba43-a05217e25918\" />");
            //sb.AppendLine("    <Attribute Name=\"COMPANYNAME\" LableResourceID=\"COMPANYNAME\" Description=\"所属公司名称\" DataType=\"string\" DataValue=\"集团公司\" DataText=\"集团公司\" />");
            //sb.AppendLine("    <Attribute Name=\"APPLYDATE\" LableResourceID=\"APPLYDATE\" Description=\"申请日期\" DataType=\"datetime\" DataValue=\"2012/12/4 14:19:39\" DataText=\"2012/12/4 14:19:39\" />");
            //sb.AppendLine("    <Attribute Name=\"APPLYMAN\" LableResourceID=\"APPLYMAN\" Description=\"申请人\" DataType=\"string\" DataValue=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" DataText=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" />");
            //sb.AppendLine("    <Attribute Name=\"TASKSENDDATE\" LableResourceID=\"TASKSENDDATE\" Description=\"任务发放日期\" DataType=\"datetime\" DataValue=\"0001/1/1 0:00:00\" DataText=\"0001/1/1 0:00:00\" />");
            //sb.AppendLine("    <Attribute Name=\"CHECKSTATES\" LableResourceID=\"CHECKSTATES\" Description=\"审核状态\" DataType=\"string\" DataValue=\"0\" DataText=\"0\" />");
            //sb.AppendLine("    <Attribute Name=\"EDITSTATE\" LableResourceID=\"EDITSTATE\" Description=\"对象编辑状态\" DataType=\"string\" DataValue=\"2\" DataText=\"2\" />");
            //sb.AppendLine("    <Attribute Name=\"OWNERCOMPANYID\" LableResourceID=\"OWNERCOMPANYID\" Description=\"记录所属公司ID\" DataType=\"string\" DataValue=\"721c99d5-4cfb-4cc7-ba43-a05217e25918\" DataText=\"721c99d5-4cfb-4cc7-ba43-a05217e25918\" />");
            //sb.AppendLine("    <Attribute Name=\"OWNERDEPARTMENTID\" LableResourceID=\"OWNERDEPARTMENTID\" Description=\"记录所属部门ID\" DataType=\"string\" DataValue=\"7fc6b3af-92ec-442b-921e-6a8d13039ce9\" DataText=\"7fc6b3af-92ec-442b-921e-6a8d13039ce9\" />");
            //sb.AppendLine("    <Attribute Name=\"OWNERPOSTID\" LableResourceID=\"OWNERPOSTID\" Description=\"记录所属岗位ID\" DataType=\"string\" DataValue=\"aa1496a5-840b-4b15-bc3e-36c2aeb14d3b\" DataText=\"aa1496a5-840b-4b15-bc3e-36c2aeb14d3b\" />");
            //sb.AppendLine("    <Attribute Name=\"OWNERID\" LableResourceID=\"OWNERID\" Description=\"记录所属用户ID\" DataType=\"string\" DataValue=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" DataText=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" />");
            //sb.AppendLine("    <Attribute Name=\"CREATECOMPANYID\" LableResourceID=\"CREATECOMPANYID\" Description=\"创建公司ID\" DataType=\"string\" DataValue=\"721c99d5-4cfb-4cc7-ba43-a05217e25918\" DataText=\"集团公司\" />");
            //sb.AppendLine("    <Attribute Name=\"CREATEDEPARTMENTID\" LableResourceID=\"CREATEDEPARTMENTID\" Description=\"创建部门ID\" DataType=\"string\" DataValue=\"7fc6b3af-92ec-442b-921e-6a8d13039ce9\" DataText=\"总经办\" />");
            //sb.AppendLine("    <Attribute Name=\"CREATEPOSTID\" LableResourceID=\"CREATEPOSTID\" Description=\"创建岗位ID\" DataType=\"string\" DataValue=\"aa1496a5-840b-4b15-bc3e-36c2aeb14d3b\" DataText=\"总经理\" />");
            //sb.AppendLine("    <Attribute Name=\"CREATEUSERID\" LableResourceID=\"CREATEUSERID\" Description=\"创建人ID\" DataType=\"string\" DataValue=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" DataText=\"黄总\" />");
            //sb.AppendLine("    <Attribute Name=\"CREATEDATE\" LableResourceID=\"CREATEDATE\" Description=\"创建时间\" DataType=\"datetime\" DataValue=\"2012/12/4 14:19:39\" DataText=\"2012/12/4 14:19:39\" />");
            //sb.AppendLine("    <Attribute Name=\"UPDATEUSERID\" LableResourceID=\"UPDATEUSERID\" Description=\"修改人ID\" DataType=\"string\" DataValue=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" DataText=\"e65cdb8f-26f2-440c-a5f9-977ea29fe4c2\" />");
            //sb.AppendLine("    <Attribute Name=\"UPDATEDATE\" LableResourceID=\"UPDATEDATE\" Description=\"修改时间\" DataType=\"datetime\" DataValue=\"2012/12/4 14:19:44\" DataText=\"2012/12/4 14:19:44\" />");
            //sb.AppendLine("    <Attribute Name=\"REMARK\" LableResourceID=\"REMARK\" Description=\"备注\" DataType=\"string\" DataValue=\"\" DataText=\"\" />");
            //sb.AppendLine("    <Attribute Name=\"ISMAJORDATA\" LableResourceID=\"ISMAJORDATA\" Description=\"是否主数据\" DataType=\"string\" DataValue=\"1\" DataText=\"1\" />");
            //sb.AppendLine("    <Attribute Name=\"AUXIDNORMID\" LableResourceID=\"AUXIDNORMID\" Description=\"附属的主数据\" DataType=\"string\" DataValue=\"\" DataText=\"\" />");
            //sb.AppendLine("    <Attribute Name=\"RECVERSION\" LableResourceID=\"RECVERSION\" Description=\"版本号\" DataType=\"string\" DataValue=\"1\" DataText=\"1\" />");
            //sb.AppendLine("    <ObjectList Name=\"T_WP_YEARNORMDEFINE\" LableResourceID=\"T_WP_YEARNORMDEFINE\" Description=\"公司年度经营指标定义\" DataText=\"\" />");
            //sb.AppendLine("  </Object>");
            //sb.AppendLine("</System>");


            //submitData.XML = sb.ToString();
            //sb.Clear();

            //SmulatedFlowService sfs = new SmulatedFlowService();
            //sfs.SubimtFlow(submitData);
            //return null;
            #endregion
            SMTWorkFlowManage.ColseWorkFlowRuntime(null);
            DataSet ds = new DataSet();
            WriteSubmitDate(submitData);
            Tracer.Debug("***********************************************开始" + DateTime.Now.ToString() + "***********************************************");
            string returnMsg = "";//暂时没有用,记录执行的顺序
            #region 更新个人缓存
            FlowEngine.TaskCacheReflesh(submitData.ApprovalUser.UserID);
            Tracer.Debug("FormID=" + submitData.FormID + ";更新个人缓存 完成 UserID=" + submitData.ApprovalUser.UserID);
            #endregion
            DateTime dtStart = DateTime.Now;
            DateTime dtEngineStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            DateTime dtCheckData = DateTime.Now;
            DataResult dataResult = new DataResult();
            FlowUser User = new FlowUser();
            //设置2分钟超时时间
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(DateTime.Now.AddMinutes(2).Ticks)))
            {
                try
                {
                    #region 初始化流程所属人的信息
                    Tracer.Debug("进入事务操作 FormID=" + submitData.FormID);
                    User = new FlowUser(submitData.ApprovalUser.CompanyID, submitData.ApprovalUser.UserID, submitData.ModelCode);
                    User.FormID = submitData.FormID;
                    Tracer.Debug("初始化流程所属人的信息 FormID=" + submitData.FormID);
                    SUser = User;
                    Tracer.Debug("SUser = User FormID=" + submitData.FormID);

                    FlowBLL Flowbll = new FlowBLL();
                    string AppCompanyID = "";  //申请公司
                    string MessageUserID = ""; //申请人ID
                    string MessageUserName = ""; //申请人名
                    dataResult.FlowResult = FlowResult.SUCCESS;
                    #endregion

                    #region 检查流程数据是否规范
                    if (!FlowBLL.CheckFlowData(submitData, ref dataResult))
                    {
                        dataResult.FlowResult = FlowResult.FAIL;
                        {
                            return dataResult;
                        }
                    }
                    #region XML进行验证
                    if (!string.IsNullOrEmpty(submitData.XML) && submitData.SubmitFlag == SubmitFlag.New)
                    {//如果是提交并XML不为空是进行验证
                        if (VerifyXML(submitData.XML))
                        {
                            dataResult.Err = "传入的XML不合法！请检查元数据Key值和DataValue值是否有空的";
                            dataResult.FlowResult = FlowResult.FAIL;
                            FlowMonitor.AddFlowMonitor(submitData, User);
                            return dataResult;
                        }
                    }
                    if (string.IsNullOrEmpty(submitData.XML) && submitData.SubmitFlag == SubmitFlag.New)
                    {//如果是提交并XML为空
                        dataResult.Err = "XML元数据不能为空！";
                        dataResult.FlowResult = FlowResult.FAIL;
                        FlowMonitor.AddFlowMonitor(submitData, User);
                        return dataResult;
                    }
                    #endregion
                    #endregion

                    #region 检查是否已提交流程(对数据库操作)
                    submitData.ApprovalResult = submitData.SubmitFlag == SubmitFlag.New ? ApprovalResult.Pass : submitData.ApprovalResult;
                    submitData.FlowSelectType = submitData.FlowSelectType == null ? FlowSelectType.FixedFlow : submitData.FlowSelectType;

                    CheckResult CheckFlowResult = Flowbll.CheckFlowIsApproved(submitData, dataResult);//对数据库操作 
                    dtCheckData = DateTime.Now;
                    dataResult = CheckFlowResult.APPDataResult;
                    if (CheckFlowResult.Flag == 0 && submitData.SubmitFlag == SubmitFlag.New)//已审批：1，未审批:0
                    {
                        dataResult.Err = "该单据已提交，还没有审核，不能再提交！";
                        dataResult.FlowResult = FlowResult.FAIL;
                        return dataResult;
                    }
                    if (CheckFlowResult.fd != null && CheckFlowResult.fd[0].CHECKSTATE == "1" && submitData.SubmitFlag == SubmitFlag.New)//审核中：1
                    {//如果单据还在审核中，不允许再提交单据
                        dataResult.Err = "该单据已在审核中，不能再提交！";
                        dataResult.FlowResult = FlowResult.FAIL;
                        return dataResult;
                    }

                    #endregion

                    #region 新增,撤单,审核
                    dataResult.AppState = submitData.NextStateCode;
                    //提交新流程
                    if (submitData.SubmitFlag == SubmitFlag.New)
                    {
                        #region 新增
                        AppCompanyID = submitData.ApprovalUser.CompanyID;
                        if (submitData.FlowSelectType == FlowSelectType.FreeFlow)
                            //自选流程
                            dataResult = Flowbll.AddFreeFlow(submitData, dataResult, ref User);//对数据库操作
                        else
                        {
                            //固定流程
                            Tracer.Debug("固定流程.Flowbill.AddFlow2");
                            dataResult = Flowbll.AddFlow(submitData, dataResult, ref User);//对数据库操作

                        }
                        #endregion
                    }
                    else if (submitData.SubmitFlag == SubmitFlag.Cancel)
                    {
                        #region 撤单
                        if (!string.IsNullOrEmpty(CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT))
                        {
                            submitData.XML = CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT;
                        }
                        submitData.ApprovalContent = "";
                        dataResult = Flowbll.CancelFlow(submitData, dataResult, CheckFlowResult.fd);
                        dataResult.SubmitFlag = submitData.SubmitFlag;
                        #endregion
                    }

                    //审批流程
                    else
                    {
                        if (CheckFlowResult.fd[0] == null)
                        {
                            dataResult.Err = "FormID =" + submitData.FormID + ";该单据没有审核记录！";
                            dataResult.FlowResult = FlowResult.FAIL;
                            return dataResult;
                        }
                        #region  审核
                        if (!string.IsNullOrEmpty(CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT))
                        {
                            submitData.XML = CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT;
                        }
                        //引擎消息数据
                        AppCompanyID = CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID;
                        MessageUserID = CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.CREATEUSERID;
                        MessageUserName = CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME;
                        submitData.ApprovalUser.CompanyID = CheckFlowResult.fd[0].EDITCOMPANYID;
                        submitData.ApprovalUser.DepartmentID = CheckFlowResult.fd[0].EDITDEPARTMENTID;
                        submitData.ApprovalUser.PostID = CheckFlowResult.fd[0].EDITPOSTID;
                        submitData.FlowSelectType = (FlowSelectType)Convert.ToInt32(CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE);
                        if (submitData.FlowSelectType == FlowSelectType.FreeFlow)
                            dataResult = Flowbll.ApprovalFreeFlow(submitData, dataResult, CheckFlowResult.fd, ref  User);//对数据库操作、对服务操作
                        else
                            dataResult = Flowbll.ApprovalFlow2(submitData, dataResult, CheckFlowResult.fd, ref User, ref returnMsg);
                        #endregion
                    }

                    #endregion

                    #region 如果检出有多个审核人时，返回界面让用户选择一个人进行审核
                    if (dataResult.FlowResult == FlowResult.MULTIUSER)
                    {
                        string users = "FormID =" + submitData.FormID + " 检出有多个审核人\r\n";
                        foreach (var u in dataResult.UserInfo)
                        {
                            //users += "CompanyID      =" + u.CompanyID + "\r\n";
                            //users += "DepartmentID   = " + u.DepartmentID + "\r\n";
                            //users += "PostID         = " + u.PostID + "\r\n";
                            //users += "serID          = " + u.UserID + "\r\n";
                            //users += "UserName       = " + u.UserName + "\r\n";

                            users += "公司ID   = " + u.CompanyID + "\r\n";
                            users += "部门ID   = " + u.DepartmentID + "\r\n";
                            users += "岗位ID   = " + u.PostID + "\r\n";
                            users += "员工ID   = " + u.UserID + "\r\n";

                            users += "公司名称 = " + u.CompanyName + "\r\n";
                            users += "部门名称 = " + u.DepartmentName + "\r\n";
                            users += "岗位名称 = " + u.PostName + "\r\n";
                            users += "员工姓名 = " + u.UserName + "\r\n";
                            users += "------------------------------------\r\n";
                        }
                        Tracer.Debug(users + "返回界面让用户选择一个人审核人");
                        for (int i = 0; i < dataResult.UserInfo.Count; i++)
                        {
                            dataResult.UserInfo[i].Roles = null; //解决 基础连接已经关闭: 连接被意外关闭（WCF深层序列化问题）
                        }

                        return dataResult;
                    }
                    #endregion

                    #region 解决 基础连接已经关闭: 连接被意外关闭（WCF深层序列化问题）
                    for (int i = 0; i < dataResult.UserInfo.Count; i++)
                    {
                        dataResult.UserInfo[i].Roles = null; //解决 基础连接已经关闭: 连接被意外关闭（WCF深层序列化问题）
                    }
                    if (dataResult.DictCounterUser != null)
                    {
                        if (dataResult.DictCounterUser.Count > 0)
                        {
                            string name = "";
                            foreach (KeyValuePair<Role_UserType, List<UserInfo>> u in dataResult.DictCounterUser)
                            {
                                name += "角色名称：" + u.Key.Remark + "  人数：" + u.Value.Count + "\r\n";
                                foreach (var user in u.Value)
                                {
                                    user.Roles = null;//解决 基础连接已经关闭: 连接被意外关闭（WCF深层序列化问题）
                                    name += "姓名：" + user.UserName + " 公司：" + user.CompanyName + "\r\n";
                                }
                                name += "---------------------------------------------------------------\r\n";
                            }
                            Tracer.Debug("FormID=" + User.FormID + " 会签角色下所有的人员 \r\n" + name);
                        }
                    }
                    #endregion

                    #region 发送审批消息（调用引擎服务）
                    Tracer.Debug("FormID=" + User.FormID + " 模块名称：user.ModelCode＝" + User.ModelCode + "; user.ModelName=" + User.ModelName + " ;流程名称＝" + User.FlowName + "(" + User.FlowCode + ")");
                    if (string.IsNullOrEmpty(strIsFlowEngine) || strIsFlowEngine.ToLower() == "true")
                    {
                        SubmitEngineService(submitData, dataResult, ref User, CheckFlowResult, Flowbll, AppCompanyID, MessageUserID, MessageUserName, ref returnMsg);
                    }
                    #endregion

                    #region 修改我的单据状态
                    //修改我的单据状态 新建并且不能不等于会签才更改状态
                    if (submitData.SubmitFlag == SubmitFlag.New && dataResult.FlowResult == FlowResult.SUCCESS)
                    {
                        EnginFlowBLL.AddPersonalRecord(submitData, "1", User, null);
                    }
                    if (dataResult.FlowResult == FlowResult.END)
                    {
                        EnginFlowBLL.AddPersonalRecord(submitData, dataResult.CheckState, User, CheckFlowResult.fd[0].FLOW_FLOWRECORDMASTER_T);
                    }
                    dtEnd = DateTime.Now;
                    dataResult.SubmitFlag = submitData.SubmitFlag;

                    if (dataResult.FlowResult == FlowResult.FAIL)
                    {
                        dataResult.Err = dataResult.Err + " \r\n FormID＝" + User.FormID + "；时间：" + DateTime.Now.ToString();
                        User.ErrorMsg += "=================================================================================\r\n";
                        User.ErrorMsg += dataResult.Err + "\r\n";
                        FlowMonitor.AddFlowMonitor(submitData, User);
                    }
                    #endregion

                    ts.Complete();//提交事务
                    //关闭数据库  
                    Tracer.Debug("执行流程成功：FormID＝" + User.FormID + ";单据所属人：" + User.UserName + "(" + User.UserID + ");公司名称:" + User.CompayName + ";模块名称:" + User.ModelName + " ;流程名称：" + User.FlowName + " (" + User.FlowCode + ") \r\n 返回给业务系统的 dataResult.FlowResult＝" + dataResult.FlowResult.ToString() + "  " + dataResult.Err);
                }
                catch (Exception ex)
                {
                    #region 记录到流程监控表里
                    ts.Dispose();
                    dataResult.RunTime += "---FlowEnd:" + DateTime.Now.ToString();
                    dataResult.FlowResult = FlowResult.FAIL;
                    dataResult.Err = ex.Message;
                    User.ErrorMsg += "=================================================================================\r\n";
                    User.ErrorMsg += "执行流程失败：FormID＝" + User.FormID + ";单据所属人：" + User.UserName + "(" + User.UserID + ");公司名称:" + User.CompayName + ";模块名称:" + User.ModelName + "; 流程名称：" + User.FlowName + " (" + User.FlowCode + "); 异常信息\r\n" + ex.ToString() + "\r\n";
                    FlowMonitor.AddFlowMonitor(submitData, User);

                    #endregion

                    #region restore workflow instanceState
                    if (User.InstanceState != null && User.InstanceState.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in User.InstanceState.Tables[0].Rows)
                        {
                            Tracer.Debug("恢复工作流InstanceState:" + dr["instance_id"]);
                            InsertInstanceState(dr);
                        }
                    }
                    #endregion

                    #region return FAIL result
                    if (dataResult.FlowResult == FlowResult.FAIL)
                    {
                        dataResult.Err = dataResult.Err + " FormID＝" + User.FormID + "；时间：" + DateTime.Now.ToString();
                        Tracer.Debug("失败错误信息：" + dataResult.Err);
                    }
                    Tracer.Debug("执行流程失败：FormID＝" + User.FormID + ";单据所属人：" + User.UserName + "(" + User.UserID + ");公司名称:" + User.CompayName + ";模块名称:" + User.ModelName + "; 流程名称：" + User.FlowName + " (" + User.FlowCode + ");执行踪信息如下:\r\n" + User.TrackingMessage + "\r\n 异常信息\r\n" + ex.ToString());
                    return dataResult;
                    #endregion
                }
                finally
                {
                }
            }
            FlowMonitor.AddInstance(submitData, User);
            return dataResult;
        }
    }
}
