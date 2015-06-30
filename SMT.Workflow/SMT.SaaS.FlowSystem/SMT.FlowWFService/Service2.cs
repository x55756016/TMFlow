﻿/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©  SmtOnline  2011    
	 * 文件名：Service2.cs  
	 * 创建者：LONGKC   
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
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Workflow.Activities;
using System.Workflow.ComponentModel.Compiler;
using System.Threading;
using SMT.WFLib;
using System.Collections.ObjectModel;

using WFTools.Services;
using WFTools.Services.Persistence.Ado;
using System.Configuration;
using WFTools.Services.Tracking.Ado;
using WFTools.Services.Batching.Ado;
using System.Xml;
using System.IO;
using System.Transactions;
using SMT.Foundation.Log;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Activities.Rules;
using System.Xml.Linq;
using System.Diagnostics;
using System.Data.OracleClient;
using SMT.FLOWDAL.ADO;

using System.Data;
using SMT.SaaS.BLLCommonServices.PermissionWS;
using SMT.Workflow.Common.Model;

namespace SMT.FlowWFService
{
    // 注意: 如果更改此处的类名“IService1”，也必须更新 App.config 中对“IService1”的引用。
    public class Service2 : IService2
    {
        EnginFlowBLL einginBll = new EnginFlowBLL();
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
        public void AddConsultation( FLOW_CONSULTATION_T flowConsultation, SubmitData submitData)
        {

            if (strIsFlowEngine.ToLower() == "true")
            {
                try
                {
                    flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T =
                        FlowBLL2.GetFLOW_FLOWRECORDMASTER_T( flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);
                    FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    FlowEngineService.CustomUserMsg[] cs = new FlowEngineService.CustomUserMsg[1];

                    FlowEngineService.CustomUserMsg cu = new FlowEngineService.CustomUserMsg();
                    cu.FormID = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID;
                    cu.UserID = flowConsultation.REPLYUSERID;
                    cs[0] = cu;
                    ModelInfo modelinfo = FlowBLL2.GetSysCodeByModelCode( submitData.ModelCode);
                    MessageData tmpMessageData = new MessageData("Flow", modelinfo.SysCode,
                        flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID,
                        submitData.ModelCode, modelinfo.ModelName, submitData.FormID, flowConsultation.FLOW_FLOWRECORDDETAIL_T.STATECODE, flowConsultation.FLOW_FLOWRECORDDETAIL_T.CHECKSTATE, "", "", "", "");
                    FlowBLL2 flowBLL = new FlowBLL2();
                    StringBuilder FlowResultXml = flowBLL.BuildMessageData(tmpMessageData);
                    //FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    //log = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    if (!string.IsNullOrEmpty(flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT))
                    {
                        submitData.XML = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT;
                    }

                    bool b = FlowEngine.FlowConsultati(cs, "", FlowResultXml.ToString(), submitData.XML);
                    if (!b)
                    {
                        Tracer.Debug("FlowEngineService-FlowConsultati:" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID
                            + "\nsubmitData:" + submitData.XML);
                    }

                    FlowBLL2 bll = new FlowBLL2();

                    bll.AddConsultation( flowConsultation);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("AddConsultation: -" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "-" + ex.InnerException + ex.Message);
                    throw ex;
                }
            }



        }
        public void ReplyConsultation( FLOW_CONSULTATION_T flowConsultation, SubmitData submitData)
        {
            if (strIsFlowEngine.ToLower() == "true")
            {
                try
                {


                    FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    //Byte[] Bo = System.Text.UTF8Encoding.UTF8.GetBytes(submitData.XML);
                    //XElement xemeBoObject = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(Bo)));
                    //string strSystemCode = (from item in xemeBoObject.Descendants("Name") select item).FirstOrDefault().Value;


                    ModelInfo modelinfo = FlowBLL2.GetSysCodeByModelCode( submitData.ModelCode);
                    FlowEngine.FlowConsultatiClose(modelinfo.SysCode, flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID, flowConsultation.REPLYUSERID);

                    FlowBLL2 bll = new FlowBLL2();
                    bll.ReplyConsultation( flowConsultation);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("ReplyConsultation: -" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "-" + ex.InnerException + ex.Message);
                    throw ex;
                }
            }

        }
        #endregion

        #region 流程处理




        #region 流程与任务审批


        public DataResult SubimtFlow(SubmitData submitData)
        {
            #region
            //submitData = new SubmitData();
            //StringBuilder sb2 = new StringBuilder();
            //sb2.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            //sb2.AppendLine("<System>");
            //sb2.AppendLine("  <Name>OA</Name>");
            //sb2.AppendLine("  <Version>1.0</Version>");
            //sb2.AppendLine("  <System>");
            //sb2.AppendLine("    <Function Description=\"事项审批\" Address=\"SmtOAPersonOffice.svc\" FuncName=\"CallApplicationService\" Binding=\"customBinding\" SplitChar=\"Г\">");
            //sb2.AppendLine("      <ParaStr>");
            //sb2.AppendLine("        <Para TableName=\"T_OA_APPROVALINFO\" Name=\"APPROVALID\" Description=\"报批件ID\" Value=\"\"></Para>");
            //sb2.AppendLine("      </ParaStr>");
            //sb2.AppendLine("    </Function>");
            //sb2.AppendLine("  </System>");
            //sb2.AppendLine("  <MsgOpen>");
            //sb2.AppendLine("    <AssemblyName>SMT.SaaS.OA.UI</AssemblyName>");
            //sb2.AppendLine("    <PublicClass>SMT.SaaS.OA.UI.Utility</PublicClass>");
            //sb2.AppendLine("    <ProcessName>CreateFormFromEngine</ProcessName>");
            //sb2.AppendLine("    <PageParameter>SMT.SaaS.OA.UI.UserControls.ApprovalForm_aud</PageParameter>");
            //sb2.AppendLine("    <ApplicationOrder>{APPROVALID}</ApplicationOrder>");
            //sb2.AppendLine("    <FormTypes>Audit</FormTypes>");
            //sb2.AppendLine("  </MsgOpen>");
            //sb2.AppendLine("  <Object Name=\"T_OA_APPROVALINFO\" Description=\"事项审批\" Key=\"APPROVALID\" id=\"ea454559-3810-4fa2-8500-b20c480dd4bd\">");
            //sb2.AppendLine("    <Attribute Name=\"CURRENTEMPLOYEENAME\" LableResourceID=\"CURRENTEMPLOYEENAME\" Description=\"提交者\" DataType=\"string\" DataValue=\"\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"APPROVALID\" LableResourceID=\"APPROVALID\" Description=\"报批件ID\" DataType=\"string\" DataValue=\"ea454559-3810-4fa2-8500-b20c480dd4bd\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"APPROVALCODE\" LableResourceID=\"CODINGMATTERS\" Description=\"事项编码\" DataType=\"string\" DataValue=\"BPJ201205050001\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"APPROVALTITLE\" LableResourceID=\"TITLEMATTERS\" Description=\"事项标题\" DataType=\"string\" DataValue=\"旧流程数据到新流程数据过度测试\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"TEL\" LableResourceID=\"TEL\" Description=\"电话\" DataType=\"string\" DataValue=\"88997744\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"ISCHARGE\" LableResourceID=\"ISCHARGE\" Description=\"是否有费用\" DataType=\"string\" DataValue=\"1\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CHARGEMONEY\" LableResourceID=\"Fees\" Description=\"费用\" DataType=\"decimal\" DataValue=\"500\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CHECKSTATE\" LableResourceID=\"CHECKSTATE\" Description=\"审批状态\" DataType=\"string\" DataValue=\"0\" DataText=\"审核中\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"OWNERID\" LableResourceID=\"OWNERID\" Description=\"所属员工\" DataType=\"string\" DataValue=\"1bc4b78a-d178-4499-b948-16e18a9d73d3\" DataText=\"人事001-人事经理-人力资源部-模拟测试公司\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"OWNERNAME\" LableResourceID=\"OWNERNAME\" Description=\"当事人\" DataType=\"string\" DataValue=\"人事001\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"OWNERCOMPANYID\" LableResourceID=\"OWNERCOMPANYID\" Description=\"所属公司\" DataType=\"string\" DataValue=\"491d6eac-3bae-47ca-8998-89d39fc6faea\" DataText=\"模拟测试公司\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"OWNERDEPARTMENTID\" LableResourceID=\"OWNERDEPARTMENTID\" Description=\"所属部门\" DataType=\"string\" DataValue=\"4b146209-9442-489a-b19d-b70bd6a720d6\" DataText=\"人力资源部\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"OWNERPOSTID\" LableResourceID=\"OWNERPOSTID\" Description=\"所属岗位\" DataType=\"string\" DataValue=\"6d438cb1-92a8-4a07-a2ab-49926c68d1cd\" DataText=\"人事经理\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CREATEUSERID\" LableResourceID=\"CREATEUSERID\" Description=\"创建人\" DataType=\"string\" DataValue=\"1bc4b78a-d178-4499-b948-16e18a9d73d3\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CREATEUSERNAME\" LableResourceID=\"CREATEUSERNAME\" Description=\"创建人\" DataType=\"string\" DataValue=\"人事001\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CREATECOMPANYID\" LableResourceID=\"CREATECOMPANYID\" Description=\"创建公司\" DataType=\"string\" DataValue=\"491d6eac-3bae-47ca-8998-89d39fc6faea\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CREATEDEPARTMENTID\" LableResourceID=\"CREATEDEPARTMENTID\" Description=\"创建人部门\" DataType=\"string\" DataValue=\"4b146209-9442-489a-b19d-b70bd6a720d6\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CREATEPOSTID\" LableResourceID=\"CREATEPOSTID\" Description=\"创建人岗位\" DataType=\"string\" DataValue=\"6d438cb1-92a8-4a07-a2ab-49926c68d1cd\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CREATEDATE\" LableResourceID=\"CREATEDATE\" Description=\"创建日期\" DataType=\"DateTime\" DataValue=\"2012/5/5 15:42:27\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"UPDATEUSERID\" LableResourceID=\"UPDATEUSERID\" Description=\"修改人\" DataType=\"string\" DataValue=\"\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"UPDATEUSERNAME\" LableResourceID=\"UPDATEUSERNAME\" Description=\"修改人名\" DataType=\"string\" DataValue=\"\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"UPDATEDATE\" LableResourceID=\"UPDATEDATE\" Description=\"修改时间\" DataType=\"DateTime\" DataValue=\"\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"CONTENT\" LableResourceID=\"CONTENT\" Description=\"内容\" DataType=\"RTF\" DataValue=\"ea454559-3810-4fa2-8500-b20c480dd4bd\" DataText=\"ea454559-3810-4fa2-8500-b20c480dd4bd\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"AttachMent\" LableResourceID=\"AttachMent\" Description=\"附件\" DataType=\"attachmentlist\" DataValue=\"ea454559-3810-4fa2-8500-b20c480dd4bd\" DataText=\"ea454559-3810-4fa2-8500-b20c480dd4bd\" />");
            //sb2.AppendLine("    <Attribute Name=\"POSTLEVEL\" LableResourceID=\"POSTLEVEL\" Description=\"岗位级别\" DataType=\"string\" DataValue=\"15\" DataText=\"15\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"TYPEAPPROVAL\" LableResourceID=\"TYPEAPPROVAL\" Description=\"事项类型\" DataType=\"string\" DataValue=\"5\" DataText=\"公司制度修改\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"TYPEAPPROVALONE\" LableResourceID=\"TYPEAPPROVALONE\" Description=\"上级事项审批类型\" DataType=\"string\" DataValue=\"0\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"TYPEAPPROVALTWO\" LableResourceID=\"TYPEAPPROVALTWO\" Description=\"上上级事项审批类型\" DataType=\"string\" DataValue=\"\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("    <Attribute Name=\"TYPEAPPROVALTHREE\" LableResourceID=\"TYPEAPPROVALTHREE\" Description=\"上上上级事项审批类型\" DataType=\"string\" DataValue=\"\" DataText=\"\"></Attribute>");
            //sb2.AppendLine("  </Object>");
            //sb2.AppendLine("</System>");

            //submitData.FlowSelectType = FlowSelectType.FixedFlow;
            //submitData.FormID = "ea454559-3810-4fa2-8500-b20c480dd4bd";
            //submitData.ModelCode = "T_OA_APPROVALINFO";
            //submitData.ApprovalUser = new UserInfo();
            //submitData.ApprovalUser.CompanyID = "491d6eac-3bae-47ca-8998-89d39fc6faea";
            //submitData.ApprovalUser.DepartmentID = "4b146209-9442-489a-b19d-b70bd6a720d6";
            //submitData.ApprovalUser.PostID = "6d438cb1-92a8-4a07-a2ab-49926c68d1cd";
            //submitData.ApprovalUser.UserID = "1bc4b78a-d178-4499-b948-16e18a9d73d3";
            //submitData.ApprovalUser.UserName = "人事001";
            //submitData.NextStateCode = "";
            //submitData.NextApprovalUser = new UserInfo();
            //submitData.NextApprovalUser.CompanyID = "";
            //submitData.NextApprovalUser.DepartmentID = "";
            //submitData.NextApprovalUser.PostID = "";
            //submitData.NextApprovalUser.UserID = "";
            //submitData.NextApprovalUser.UserName = "";
            //submitData.SubmitFlag = SubmitFlag.New;
            //submitData.XML = sb2.ToString();
            //submitData.FlowType = FlowType.Approval;
            //submitData.ApprovalResult = ApprovalResult.Pass;
            //submitData.ApprovalContent = "提交";
            #endregion
            //调试外网 st = new 调试外网();
            //return st.SubimtFlow(submitData);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("submitData.FlowSelectType =\"" + submitData.FlowSelectType + "\";");
            sb.AppendLine("submitData.FormID = \"" + submitData.FormID + "\";");
            sb.AppendLine("submitData.ModelCode = \"" + submitData.ModelCode + "\";");
            sb.AppendLine("submitData.ApprovalUser.CompanyID = \"" + submitData.ApprovalUser.CompanyID + "\";");
            sb.AppendLine("submitData.ApprovalUser.DepartmentID = \"" + submitData.ApprovalUser.DepartmentID + "\";");
            sb.AppendLine("submitData.ApprovalUser.PostID = \"" + submitData.ApprovalUser.PostID + "\";");
            sb.AppendLine("submitData.ApprovalUser.UserID = \"" + submitData.ApprovalUser.UserID + "\";");
            sb.AppendLine("submitData.ApprovalUser.UserName = \"" + submitData.ApprovalUser.UserName + "\";");
            sb.AppendLine("submitData.NextStateCode = \"" + (submitData.NextStateCode != null ? submitData.NextStateCode : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser = \"" + submitData.NextApprovalUser + "\";");
            sb.AppendLine("submitData.NextApprovalUser.CompanyID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.CompanyID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.DepartmentID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.DepartmentID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.PostID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.PostID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.UserID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.UserID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.UserName = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.UserName : "空") + "\";");
            sb.AppendLine("submitData.SubmitFlag = \"" + submitData.SubmitFlag + "\";");
            // sb.AppendLine("submitData.XML = \"" + submitData.XML + "\";");
            sb.AppendLine("submitData.FlowType = \"" + submitData.FlowType.ToString() + "\";");
            sb.AppendLine("submitData.ApprovalResult = \"" + submitData.ApprovalResult.ToString() + "\";");
            sb.AppendLine("submitData.ApprovalContent = \"" + submitData.ApprovalContent + "\";");
            Tracer.Debug("提交审核的信息：\r\n" + sb.ToString() + "\r\n");
            Tracer.Debug("流程引擎的XML数据 SubimtFlow入口处:" + submitData.ApprovalContent + " FormID=" + submitData.FormID + ";ModelCode=" + submitData.ModelCode + " \r\n submitData.XML:\r\n" + submitData.XML);
            Tracer.Debug("***********************************************开始" + DateTime.Now.ToString() + "***********************************************");
            string msg = "";

            // MsOracle.BeginTransaction(con);

            DateTime dtStart = DateTime.Now;
            DateTime dtEngineStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            DateTime dtCheckData = DateTime.Now;
            //using (TransactionScope ts = new TransactionScope())
            //设置2分钟超时时间
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(DateTime.Now.AddMinutes(2).Ticks)))
            {
                //OracleConnection con = ADOHelper.GetOracleConnection();
                #region 事务处理

                //System.Threading.Thread.Sleep(60000);
                DataResult dataResult = new DataResult();
                FlowBLL2 Flowbill = new FlowBLL2();
                string AppCompanyID = "";  //申请公司
                string MessageUserID = ""; //申请人ID
                string MessageUserName = ""; //申请人名
                dataResult.FlowResult = FlowResult.SUCCESS;
                try
                {
                    #region 检查流程数据是否规范
                    if (!FlowBLL2.CheckFlowData(submitData, ref dataResult))
                    {
                        dataResult.FlowResult = FlowResult.FAIL;
                        {
                            //ts.Complete();
                            return dataResult;
                        }
                    }
                    #endregion
                    submitData.ApprovalResult = submitData.SubmitFlag == SubmitFlag.New ? ApprovalResult.Pass : submitData.ApprovalResult;
                    submitData.FlowSelectType = submitData.FlowSelectType == null ? FlowSelectType.FixedFlow : submitData.FlowSelectType;
                    #region
                    #region 检查是否已提交流程(对数据库操作)
                    msg += "检查是否已提交流程" + DateTime.Now.ToString() + "\r\n";
                    Tracer.Debug("检查是否已提交流程FormID = " + submitData.FormID);
                    //OracleConnection ocon = ADOHelper.GetOracleConnection();
                    CheckResult CheckFlowResult = Flowbill.CheckFlow2( submitData, dataResult);//对数据库操作                    
                    msg += "检查是否已提交流程完成" + DateTime.Now.ToString() + "\r\n";
                    Tracer.Debug("检查是否已提交流程完成FormID =" + submitData.FormID);
                    dtCheckData = DateTime.Now;
                    dataResult = CheckFlowResult.APPDataResult;
                    if (CheckFlowResult.Flag == 0)//已审批：1，未审批:0
                    {
                        //ts.Complete();
                        //return dataResult;
                    }

                    #endregion
                    dataResult.AppState = submitData.NextStateCode;
                    //提交新流程
                    if (submitData.SubmitFlag == SubmitFlag.New)
                    {
                        #region 新增
                        AppCompanyID = submitData.ApprovalUser.CompanyID;
                        if (submitData.FlowSelectType == FlowSelectType.FreeFlow)
                            //自选流程
                            dataResult = Flowbill.AddFreeFlow( submitData, dataResult);//对数据库操作
                        else
                        {
                            //固定流程
                            msg += "固定流程" + DateTime.Now.ToString() + "\r\n";
                            Tracer.Debug("固定流程.Flowbill.AddFlow2");
                            dataResult = Flowbill.AddFlow2( submitData, dataResult, ref msg);//对数据库操作
                            msg += "固定流程完成" + DateTime.Now.ToString() + "\r\n";
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
                        dataResult = Flowbill.CancelFlow(submitData, dataResult, CheckFlowResult.fd);
                        dataResult.SubmitFlag = submitData.SubmitFlag;
                        #endregion
                    }

                    //审批流程
                    else
                    {
                        msg += "审核" + DateTime.Now.ToString() + "\r\n";
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
                            dataResult = Flowbill.ApprovalFreeFlow( submitData, dataResult, CheckFlowResult.fd);//对数据库操作、对服务操作
                        else
                            dataResult = Flowbill.ApprovalFlow2( submitData, dataResult, CheckFlowResult.fd, ref msg);
                        #endregion
                        msg += "审核完成" + DateTime.Now.ToString() + "\r\n";
                    }

                    #endregion
                    ModelInfo modelinfo = FlowBLL2.GetSysCodeByModelCode( submitData.ModelCode);//对数据库操作
                    if (dataResult.FlowResult == FlowResult.MULTIUSER)
                    {//如果检出有多个审核人时，返回界面让用户选择一个人进行审核
                        string users = "检出有多个审核人\r\n";
                        foreach (var u in dataResult.UserInfo)
                        {
                            users += "CompanyID      =" + u.CompanyID + "\r\n";
                            users += "DepartmentID   = " + u.DepartmentID + "\r\n";
                            users += "PostID         = " + u.PostID + "\r\n";
                            users += "serID          = " + u.UserID + "\r\n";
                            users += "UserName       = " + u.UserName + "\r\n";
                            users += "------------------------------------\r\n";
                        }
                        Tracer.Debug(users + "检出有多个审核人，返回界面让用户选择一个人进行审核 FormID =" + submitData.FormID);
                        return dataResult;
                    }
                    //return dataResult;//测试检查流程用的，不能提交事务
                    Tracer.Debug("模块名称：submitData.ModelCode＝" + submitData.ModelCode + ";modelinfo.ModelName=" + modelinfo.ModelName);
                    //ts.Complete(); //提交事务                           
                    if (string.IsNullOrEmpty(strIsFlowEngine) || strIsFlowEngine.ToLower() == "true")
                    {
                        msg += "发送审批消息（调用引擎服务）" + DateTime.Now.ToString() + "\r\n";
                        #region 发送审批消息（调用引擎服务）
                        Tracer.Debug("开始（调用引擎服务）：SubmitEngineService");
                        SubmitEngineService( submitData, dataResult, modelinfo, CheckFlowResult, Flowbill, AppCompanyID, MessageUserID, MessageUserName, ref msg);
                        Tracer.Debug("结束（调用引擎服务）：SubmitEngineService");
                        #endregion
                        msg += "发送审批消息（调用引擎服务）完成" + DateTime.Now.ToString() + "\r\n";
                    }
                    dtEnd = DateTime.Now;
                    dataResult.SubmitFlag = submitData.SubmitFlag;
                    msg += "开始时间" + dtStart.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "\r\n";
                    msg += "结束时间" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "\r\n";
                    msg += "完成时间" + DateDiff(dtEnd, dtStart);
                    ts.Complete();//提交事务
                    //关闭数据库  
                    Tracer.Debug("执行流程成功：FormID＝" + submitData.FormID);
                    //Tracer.Debug(msg);
                    return dataResult;
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    dataResult.RunTime += "---FlowEnd:" + DateTime.Now.ToString();
                    dataResult.FlowResult = FlowResult.FAIL;
                    // Flowbill              = null;
                    dataResult.Err = ex.Message;
                    // Tracer.Debug("FlowService: -" + submitData.FormID + "--submitDataXML:" + submitData.XML + "-" + ex.InnerException + ex.Message);
                    Tracer.Debug("执行流程失败：FormID＝" + submitData.FormID + " 异常信息：" + ex.Message);
                    return dataResult;
                }
                finally
                {
                    if (Flowbill.workflowRuntime != null && Flowbill.workflowRuntime.IsStarted)
                    {
                        Flowbill.workflowRuntime.Dispose();
                    }
                    dataResult = null;
                    // Tracer.Debug("-------Trace--FormID:" + submitData.FormID + "DateTime: Start:" + dtStart.ToString()  + "  EngineStart:" + dtEngineStart.ToString() + "  End:" + dtEnd.ToString() + "\n");

                }
                #endregion
            }

        }
        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="endTime"></param>
        /// <param name="beginTime"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime endTime, DateTime beginTime)
        {
            string dateDiff = null;

            TimeSpan ts1 = new TimeSpan(endTime.Ticks);
            TimeSpan ts2 = new TimeSpan(beginTime.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天"
                + ts.Hours.ToString() + "小时"
                + ts.Minutes.ToString() + "分钟"
                + ts.Seconds.ToString() + "秒";

            return dateDiff;
        }
        /// <summary>
        /// 发送审批消息（调用引擎服务）
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="dataResult"></param>
        /// <param name="modelinfo"></param>
        /// <param name="CheckFlowResult"></param>
        /// <param name="Flowbill"></param>
        /// <param name="AppCompanyID">申请公司</param>
        /// <param name="MessageUserID">申请人ID</param>
        /// <param name="MessageUserName">申请人名</param>
        private void SubmitEngineService( SubmitData submitData, DataResult dataResult, ModelInfo modelinfo, CheckResult CheckFlowResult, FlowBLL2 Flowbill, string AppCompanyID, string MessageUserID, string MessageUserName, ref string ErroMessage)
        {
            bool bOK = true;
            #region 发送审批消息（调用引擎服务）
            try
            {
                if (dataResult.FlowResult == FlowResult.SUCCESS || dataResult.FlowResult == FlowResult.END)
                {
                    #region 调用引擎服务(原有调用WCF服务)
                    ////EngineWcfGlobalFunctionService.EngineWcfGlobalFunctionClient engineClient = new EngineWcfGlobalFunctionService.EngineWcfGlobalFunctionClient();//旧引擎服务

                    //FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    //string IsTask = dataResult.FlowResult == FlowResult.SUCCESS ? "1" : "0";
                    //MessageData tmpMessageData = null;
                    //StringBuilder FlowResultXml = null;
                    //#region
                    //switch (submitData.SubmitFlag)
                    //{
                    //    case SubmitFlag.New:
                    //        #region 新增
                    //        if (dataResult.IsCountersign)
                    //        {
                    //            #region
                    //            if (dataResult.FlowResult == FlowResult.SUCCESS)
                    //            {
                    //                List<string> listUserID = new List<string>();
                    //                List<string> listUserName = new List<string>();
                    //                if (dataResult.DictCounterUser != null && dataResult.DictCounterUser.Keys.Count > 0)
                    //                {
                    //                    dataResult.DictCounterUser.Values.ToList().ForEach(users =>
                    //                    {
                    //                        users.ForEach(user =>
                    //                        {
                    //                            listUserID.Add(user.UserID);
                    //                            listUserName.Add(user.UserName);
                    //                        });
                    //                    });

                    //                    MessageUserID = string.Join("|", listUserID);
                    //                    MessageUserName = string.Join("|", listUserName);
                    //                }

                    //            }
                    //            if (dataResult.DictAgentUserInfo != null && dataResult.DictAgentUserInfo.Count > 0)
                    //            {
                    //                dataResult.DictAgentUserInfo.Values.ToList().ForEach(user =>
                    //                {
                    //                    MessageUserID += "|" + user.UserID.Trim();
                    //                    MessageUserName += "|" + user.UserName.Trim();

                    //                });
                    //            }
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            #region
                    //            if (dataResult.FlowResult == FlowResult.SUCCESS)
                    //            {
                    //                MessageUserID = dataResult.UserInfo[0].UserID.Trim();
                    //                MessageUserName = dataResult.UserInfo[0].UserName.Trim();
                    //            }
                    //            if (dataResult.AgentUserInfo != null)
                    //            {
                    //                MessageUserID += "|" + dataResult.AgentUserInfo.UserID.Trim();
                    //                MessageUserName += "|" + dataResult.AgentUserInfo.UserName.Trim();
                    //            }
                    //            #endregion
                    //        }
                    //        tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                    //            submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                    //        FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                    //        DateTime s = DateTime.Now;
                    //        Tracer.Debug("开始新增FlowEngine.SaveFlowTriggerData");
                    //        bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //        Tracer.Debug("bOK＝" + bOK.ToString());
                    //        Tracer.Debug("结束新增FlowEngine.SaveFlowTriggerData");
                    //        //bOK = engineClient.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //        DateTime e = DateTime.Now;
                    //        string str = DateDiff(e, s);
                    //        if (!bOK)
                    //        {
                    //            Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                    //        }

                    //        #endregion
                    //        break;
                    //    case SubmitFlag.Cancel:
                    //        #region 撤单
                    //        tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                    //            submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                    //        FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                    //        Tracer.Debug("开始撤单FlowEngine.FlowCancel");
                    //        bool bCancel = FlowEngine.FlowCancel(FlowResultXml.ToString(), submitData.XML);
                    //        Tracer.Debug("bCancel＝" + bCancel.ToString());
                    //        Tracer.Debug("结束撤单FlowEngine.FlowCancel");
                    //        //bool bCancel = engineClient.FlowCancel(FlowResultXml.ToString(), submitData.XML); 
                    //        if (!bCancel)
                    //        {
                    //            Tracer.Debug("FlowEngineService:DateTime:" + DateTime.Now.ToString() + "\n" + "FlowCancel:submitData.XML" + "\n\nFlowResultXml" + FlowResultXml.ToString());
                    //        }
                    //        #endregion
                    //        break;
                    //    case SubmitFlag.Approval:
                    //        Tracer.Debug("开始审核");
                    //        #region 审核

                    //        if (dataResult.CurrentIsCountersign)
                    //        {
                    //            Tracer.Debug("第1次：TaskMsgClose");
                    //            FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                    //            //engineClient.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                    //            //einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);

                    //            #region
                    //            if (submitData.ApprovalResult == ApprovalResult.NoPass)
                    //            {
                    //                #region
                    //                List<string> listMessageUserID = new List<string>();
                    //                CheckFlowResult.fd.Where(detail => detail.EDITUSERID != submitData.ApprovalUser.UserID).ToList().ForEach(item =>
                    //                {
                    //                    listMessageUserID.Add(item.EDITUSERID);
                    //                    if (!string.IsNullOrEmpty(item.AGENTUSERID))
                    //                    {
                    //                        listMessageUserID.Add(item.AGENTUSERID);
                    //                    }
                    //                });
                    //                if (listMessageUserID.Count > 0)
                    //                {
                    //                    string messageUserID = string.Join(",", listMessageUserID);
                    //                    Tracer.Debug("第2次：TaskMsgClose");
                    //                    FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, messageUserID);

                    //                    //engineClient.TaskMsgClose(modelinfo.SysCode, submitData.FormID, messageUserID);
                    //                    //einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, messageUserID);
                    //                }
                    //                tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                    //              submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                    //                FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);

                    //                DateTime s2 = DateTime.Now;
                    //                bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //                //bOK = engineClient.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //                // bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML);
                    //                DateTime e2 = DateTime.Now;
                    //                string str2 = DateDiff(e2, s2);
                    //                if (!bOK)
                    //                {
                    //                    Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                    //                }
                    //                #endregion
                    //            }
                    //            else if (dataResult.IsGotoNextState)
                    //            {
                    //                #region
                    //                List<string> listMessageUserID = new List<string>();
                    //                CheckFlowResult.fd.Where(detail => detail.EDITUSERID != submitData.ApprovalUser.UserID).ToList().ForEach(item =>
                    //                {
                    //                    listMessageUserID.Add(item.EDITUSERID);
                    //                    if (!string.IsNullOrEmpty(item.AGENTUSERID))
                    //                    {
                    //                        listMessageUserID.Add(item.AGENTUSERID);
                    //                    }
                    //                });
                    //                if (listMessageUserID.Count > 0)
                    //                {

                    //                    string messageUserID = string.Join(",", listMessageUserID);
                    //                    Tracer.Debug("第3次：TaskMsgClose");
                    //                    FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, messageUserID);

                    //                    //engineClient.TaskMsgClose(modelinfo.SysCode, submitData.FormID, messageUserID);
                    //                    //einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, messageUserID);
                    //                }
                    //                if (dataResult.IsCountersign)
                    //                {
                    //                    #region
                    //                    if (dataResult.FlowResult == FlowResult.SUCCESS)
                    //                    {

                    //                        List<string> listUserID = new List<string>();
                    //                        List<string> listUserName = new List<string>();
                    //                        if (dataResult.DictCounterUser != null && dataResult.DictCounterUser.Keys.Count > 0)
                    //                        {
                    //                            dataResult.DictCounterUser.Values.ToList().ForEach(users =>
                    //                            {
                    //                                users.ForEach(user =>
                    //                                {
                    //                                    listUserID.Add(user.UserID);
                    //                                    listUserName.Add(user.UserName);
                    //                                });
                    //                            });

                    //                            MessageUserID = string.Join("|", listUserID);
                    //                            MessageUserName = string.Join("|", listUserName);
                    //                        }

                    //                    }
                    //                    if (dataResult.DictAgentUserInfo != null && dataResult.DictAgentUserInfo.Count > 0)
                    //                    {
                    //                        dataResult.DictAgentUserInfo.Values.ToList().ForEach(user =>
                    //                        {
                    //                            MessageUserID += "|" + user.UserID.Trim();
                    //                            MessageUserName += "|" + user.UserName.Trim();

                    //                        });
                    //                    }
                    //                    #endregion
                    //                }
                    //                else
                    //                {
                    //                    #region
                    //                    if (dataResult.FlowResult == FlowResult.SUCCESS)
                    //                    {
                    //                        MessageUserID = dataResult.UserInfo[0].UserID.Trim();
                    //                        MessageUserName = dataResult.UserInfo[0].UserName.Trim();

                    //                    }

                    //                    if (dataResult.AgentUserInfo != null)
                    //                    {
                    //                        MessageUserID += "|" + dataResult.AgentUserInfo.UserID.Trim();
                    //                        MessageUserName += "|" + dataResult.AgentUserInfo.UserName.Trim();
                    //                    }
                    //                    #endregion
                    //                }
                    //                tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                    //                    submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                    //                FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);

                    //                DateTime s3 = DateTime.Now;
                    //                bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //                //bOK = engineClient.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //                //bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML);
                    //                DateTime e3 = DateTime.Now;
                    //                string str3 = DateDiff(e3, s3);
                    //                if (!bOK)
                    //                {
                    //                    Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                    //                }
                    //                #endregion
                    //            }
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            #region
                    //            if (dataResult.IsCountersign)
                    //            {
                    //                #region
                    //                if (dataResult.FlowResult == FlowResult.SUCCESS)
                    //                {

                    //                    List<string> listUserID = new List<string>();
                    //                    List<string> listUserName = new List<string>();
                    //                    if (dataResult.DictCounterUser != null && dataResult.DictCounterUser.Keys.Count > 0)
                    //                    {
                    //                        dataResult.DictCounterUser.Values.ToList().ForEach(users =>
                    //                        {
                    //                            users.ForEach(user =>
                    //                            {
                    //                                listUserID.Add(user.UserID);
                    //                                listUserName.Add(user.UserName);
                    //                            });
                    //                        });

                    //                        MessageUserID = string.Join("|", listUserID);
                    //                        MessageUserName = string.Join("|", listUserName);
                    //                    }

                    //                }
                    //                if (dataResult.DictAgentUserInfo != null && dataResult.DictAgentUserInfo.Count > 0)
                    //                {
                    //                    dataResult.DictAgentUserInfo.Values.ToList().ForEach(user =>
                    //                    {
                    //                        MessageUserID += "|" + user.UserID.Trim();
                    //                        MessageUserName += "|" + user.UserName.Trim();

                    //                    });
                    //                }
                    //                #endregion
                    //            }
                    //            else
                    //            {
                    //                #region
                    //                if (dataResult.FlowResult == FlowResult.SUCCESS)
                    //                {
                    //                    MessageUserID = dataResult.UserInfo[0].UserID.Trim();
                    //                    MessageUserName = dataResult.UserInfo[0].UserName.Trim();

                    //                }

                    //                if (dataResult.AgentUserInfo != null)
                    //                {
                    //                    MessageUserID += "|" + dataResult.AgentUserInfo.UserID.Trim();
                    //                    MessageUserName += "|" + dataResult.AgentUserInfo.UserName.Trim();
                    //                }
                    //                #endregion
                    //            }
                    //            tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                    //                submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                    //            FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);

                    //            DateTime s4 = DateTime.Now;
                    //            Tracer.Debug("开始FlowEngine.SaveFlowTriggerData");
                    //            bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //            Tracer.Debug("bOK＝" + bOK.ToString());
                    //            Tracer.Debug("结束FlowEngine.SaveFlowTriggerData");
                    //            //bOK = engineClient.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    //            //bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML);
                    //            DateTime e4 = DateTime.Now;
                    //            string str4 = DateDiff(e4, s4);
                    //            if (!bOK)
                    //            {
                    //                Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                    //            }
                    //            else
                    //            {
                    //                Tracer.Debug("第4次：TaskMsgClose");
                    //                FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);

                    //                //engineClient.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                    //                //einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                    //            }
                    //            #endregion
                    //        }
                    //        #endregion
                    //        Tracer.Debug("结束审核");
                    //        break;
                    //}
                    //#endregion
                    #endregion 调用引擎服务(原有调用WCF服务)
                    #region 调用引擎服务（调用本地服DLL）
                    string IsTask = dataResult.FlowResult == FlowResult.SUCCESS ? "1" : "0";
                    MessageData tmpMessageData = null;
                    StringBuilder FlowResultXml = null;
                    #region
                    switch (submitData.SubmitFlag)
                    {
                        case SubmitFlag.New:
                            #region 新增
                            if (dataResult.IsCountersign)
                            {
                                #region
                                if (dataResult.FlowResult == FlowResult.SUCCESS)
                                {
                                    List<string> listUserID = new List<string>();
                                    List<string> listUserName = new List<string>();
                                    if (dataResult.DictCounterUser != null && dataResult.DictCounterUser.Keys.Count > 0)
                                    {
                                        dataResult.DictCounterUser.Values.ToList().ForEach(users =>
                                        {
                                            users.ForEach(user =>
                                            {
                                                listUserID.Add(user.UserID);
                                                listUserName.Add(user.UserName);
                                            });
                                        });

                                        MessageUserID = string.Join("|", listUserID);
                                        MessageUserName = string.Join("|", listUserName);
                                    }

                                }
                                if (dataResult.DictAgentUserInfo != null && dataResult.DictAgentUserInfo.Count > 0)
                                {
                                    dataResult.DictAgentUserInfo.Values.ToList().ForEach(user =>
                                    {
                                        MessageUserID += "|" + user.UserID.Trim();
                                        MessageUserName += "|" + user.UserName.Trim();

                                    });
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                if (dataResult.FlowResult == FlowResult.SUCCESS)
                                {
                                    MessageUserID = dataResult.UserInfo[0].UserID.Trim();
                                    MessageUserName = dataResult.UserInfo[0].UserName.Trim();
                                }
                                if (dataResult.AgentUserInfo != null)
                                {
                                    MessageUserID += "|" + dataResult.AgentUserInfo.UserID.Trim();
                                    MessageUserName += "|" + dataResult.AgentUserInfo.UserName.Trim();
                                }
                                #endregion
                            }
                            tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                                submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                            FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);

                            Tracer.Debug("开始新增FlowEngine.SaveFlowTriggerData");
                            Tracer.Debug("流程引擎的XML数据 新增: FormID=" + submitData.FormID + ";SysCode=" + modelinfo.SysCode + " ModelName=" + modelinfo.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML);
                            bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                            Tracer.Debug("bOK＝" + bOK.ToString());
                            Tracer.Debug("结束新增FlowEngine.SaveFlowTriggerData");
                            if (!bOK)
                            {
                                Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                            }

                            #endregion
                            break;
                        case SubmitFlag.Cancel:
                            #region 撤单
                            tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                                submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                            FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                            //bool bCancel = FlowEngine.FlowCancel(FlowResultXml.ToString(), submitData.XML);
                            Tracer.Debug("流程引擎的XML数据 撤单: FormID=" + submitData.FormID + ";SysCode=" + modelinfo.SysCode + " ModelName=" + modelinfo.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML);

                            bool bCancel = einginBll.FlowCancel( FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                            if (!bCancel)
                            {
                                Tracer.Debug("FlowEngineService:DateTime:" + DateTime.Now.ToString() + "\n" + "FlowCancel:submitData.XML" + "\n\nFlowResultXml" + FlowResultXml.ToString());
                            }
                            #endregion
                            break;
                        case SubmitFlag.Approval:
                            #region 审核
                            //FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                            einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                            if (dataResult.CurrentIsCountersign)
                            {
                                #region
                                if (submitData.ApprovalResult == ApprovalResult.NoPass)
                                {
                                    #region
                                    List<string> listMessageUserID = new List<string>();
                                    CheckFlowResult.fd.Where(detail => detail.EDITUSERID != submitData.ApprovalUser.UserID).ToList().ForEach(item =>
                                    {
                                        listMessageUserID.Add(item.EDITUSERID);
                                        if (!string.IsNullOrEmpty(item.AGENTUSERID))
                                        {
                                            listMessageUserID.Add(item.AGENTUSERID);
                                        }
                                    });
                                    if (listMessageUserID.Count > 0)
                                    {
                                        string messageUserID = string.Join(",", listMessageUserID);
                                        //FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, messageUserID);
                                        einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, messageUserID);
                                    }
                                    tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                                  submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                    FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                    //bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);                                   

                                    Tracer.Debug("开始审核FlowEngine.SaveFlowTriggerData");
                                    Tracer.Debug("流程引擎的XML数据 审核: FormID=" + submitData.FormID + ";SysCode=" + modelinfo.SysCode + " ModelName=" + modelinfo.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML);
                                    bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                                    Tracer.Debug("bOK＝" + bOK.ToString());
                                    Tracer.Debug("结束审核FlowEngine.SaveFlowTriggerData");
                                    if (!bOK)
                                    {
                                        Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                                    }
                                    #endregion
                                }
                                else if (dataResult.IsGotoNextState)
                                {
                                    #region
                                    List<string> listMessageUserID = new List<string>();
                                    CheckFlowResult.fd.Where(detail => detail.EDITUSERID != submitData.ApprovalUser.UserID).ToList().ForEach(item =>
                                    {
                                        listMessageUserID.Add(item.EDITUSERID);
                                        if (!string.IsNullOrEmpty(item.AGENTUSERID))
                                        {
                                            listMessageUserID.Add(item.AGENTUSERID);
                                        }
                                    });
                                    if (listMessageUserID.Count > 0)
                                    {
                                        string messageUserID = string.Join(",", listMessageUserID);
                                        // FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, messageUserID);
                                        einginBll.TaskMsgClose( modelinfo.SysCode, submitData.FormID, messageUserID);
                                    }
                                    if (dataResult.IsCountersign)
                                    {
                                        #region
                                        if (dataResult.FlowResult == FlowResult.SUCCESS)
                                        {

                                            List<string> listUserID = new List<string>();
                                            List<string> listUserName = new List<string>();
                                            if (dataResult.DictCounterUser != null && dataResult.DictCounterUser.Keys.Count > 0)
                                            {
                                                dataResult.DictCounterUser.Values.ToList().ForEach(users =>
                                                {
                                                    users.ForEach(user =>
                                                    {
                                                        listUserID.Add(user.UserID);
                                                        listUserName.Add(user.UserName);
                                                    });
                                                });

                                                MessageUserID = string.Join("|", listUserID);
                                                MessageUserName = string.Join("|", listUserName);
                                            }

                                        }
                                        if (dataResult.DictAgentUserInfo != null && dataResult.DictAgentUserInfo.Count > 0)
                                        {
                                            dataResult.DictAgentUserInfo.Values.ToList().ForEach(user =>
                                            {
                                                MessageUserID += "|" + user.UserID.Trim();
                                                MessageUserName += "|" + user.UserName.Trim();

                                            });
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        if (dataResult.FlowResult == FlowResult.SUCCESS)
                                        {
                                            MessageUserID = dataResult.UserInfo[0].UserID.Trim();
                                            MessageUserName = dataResult.UserInfo[0].UserName.Trim();

                                        }

                                        if (dataResult.AgentUserInfo != null)
                                        {
                                            MessageUserID += "|" + dataResult.AgentUserInfo.UserID.Trim();
                                            MessageUserName += "|" + dataResult.AgentUserInfo.UserName.Trim();
                                        }
                                        #endregion
                                    }
                                    tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                                        submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                    FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                    //bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                                    DateTime s3 = DateTime.Now;
                                    bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                                    DateTime e3 = DateTime.Now;
                                    string str3 = DateDiff(e3, s3);
                                    if (!bOK)
                                    {
                                        Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                if (dataResult.IsCountersign)
                                {
                                    #region
                                    if (dataResult.FlowResult == FlowResult.SUCCESS)
                                    {

                                        List<string> listUserID = new List<string>();
                                        List<string> listUserName = new List<string>();
                                        if (dataResult.DictCounterUser != null && dataResult.DictCounterUser.Keys.Count > 0)
                                        {
                                            dataResult.DictCounterUser.Values.ToList().ForEach(users =>
                                            {
                                                users.ForEach(user =>
                                                {
                                                    listUserID.Add(user.UserID);
                                                    listUserName.Add(user.UserName);
                                                });
                                            });

                                            MessageUserID = string.Join("|", listUserID);
                                            MessageUserName = string.Join("|", listUserName);
                                        }

                                    }
                                    if (dataResult.DictAgentUserInfo != null && dataResult.DictAgentUserInfo.Count > 0)
                                    {
                                        dataResult.DictAgentUserInfo.Values.ToList().ForEach(user =>
                                        {
                                            MessageUserID += "|" + user.UserID.Trim();
                                            MessageUserName += "|" + user.UserName.Trim();

                                        });
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region
                                    if (dataResult.FlowResult == FlowResult.SUCCESS)
                                    {
                                        MessageUserID = dataResult.UserInfo[0].UserID.Trim();
                                        MessageUserName = dataResult.UserInfo[0].UserName.Trim();

                                    }

                                    if (dataResult.AgentUserInfo != null)
                                    {
                                        MessageUserID += "|" + dataResult.AgentUserInfo.UserID.Trim();
                                        MessageUserName += "|" + dataResult.AgentUserInfo.UserName.Trim();
                                    }
                                    #endregion
                                }
                                tmpMessageData = new MessageData("Flow", modelinfo.SysCode, AppCompanyID, submitData.ModelCode, modelinfo.ModelName,
                                    submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                // bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                                DateTime s4 = DateTime.Now;
                                bOK = einginBll.SaveFlowTriggerData( FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                                DateTime e4 = DateTime.Now;
                                string str4 = DateDiff(e4, s4);
                                if (!bOK)
                                {
                                    Tracer.Debug("FlowEngineService:FormID-" + submitData.FormID + "--submitDataXML:" + submitData.XML);
                                }
                                #endregion
                            }
                            #endregion
                            break;
                    }
                    #endregion
                    #endregion  调用引擎服务（调用本地服DLL）

                }
            }
            catch (Exception ex)
            {
                //Tracer.Debug("FlowEngineService: -" + "\n\nError:" + ex.InnerException + ex.Message);
                Tracer.Debug("SubmitEngineService"+ex);
                throw new Exception(ErroMessage);
            }
            #endregion
        }
        #endregion


        /// <summary>
        /// 检测用户是否有未处理的单据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string IsExistFlowDataByUserID(string UserID, string PostID)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            try
            {
                FlowBLL2 Flow = new FlowBLL2();
                return Flow.IsExistFlowDataByUserID( UserID, PostID);
            }
            catch (Exception ex)
            {
                Tracer.Debug("IsExistFlowDataByUserID:" + UserID + " Ex:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户有哪些未处理的单据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<FLOW_FLOWRECORDMASTER_T> GetFlowDataByUserID( string UserID)
        {
            try
            {
                FlowBLL2 Flow = new FlowBLL2();
                return Flow.GetFlowDataByUserID( UserID);
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetFlowDataByUserID:" + UserID + " Ex:" + ex.Message);
                throw ex;
            }
        }
        #endregion



        #region 查询下一节点用户

        /// <summary>
        /// 启动与工作流程相同类型流程，查询对应节点用户
        /// </summary>
        /// <param name="CompanyID">公司ID</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="FlowGUID">待审批流GUID，新增时为空或者为StartFlow</param>
        /// <returns></returns>
        public DataResult GetAppUser( string CompanyID, string ModelCode, string FlowGUID, string xml)
        {

            DataResult GetAppUserResult = new DataResult();
            try
            {
                string StateName = null;


                if (FlowGUID == "" || FlowGUID == "StartFlow")
                {
                    StateName = "StartFlow";
                }
                else
                {
                    //根据待审批流程GUID,检索待审批状态节点代码
                    List<FLOW_FLOWRECORDDETAIL_T> FlowRecord = FlowBLL2.GetFlowInfo( "", FlowGUID, "", "", "", "", "", null);
                    if (FlowRecord == null)
                    {
                        GetAppUserResult.Err = "没有待处理的审核";
                        GetAppUserResult.UserInfo = null;
                        return GetAppUserResult;
                    }
                    StateName = FlowRecord[0].STATECODE;
                }

                //根据公司ID，模块代码获取配置的流程
                WorkflowInstance instance = null;
                Tracer.Debug("根据公司ID，模块代码获取配置的流程FlowBLL2.GetFlowByModelName:OgrType='0'");

                List<FLOW_MODELFLOWRELATION_T> MODELFLOWRELATION = FlowBLL2.GetFlowByModelName( CompanyID, "", ModelCode, "0");

                if (MODELFLOWRELATION == null || MODELFLOWRELATION.Count == 0)
                {
                    GetAppUserResult.Err = "没有可使用的流程";
                    GetAppUserResult.UserInfo = null;
                    return GetAppUserResult;
                }
                FLOW_FLOWDEFINE_T Xoml = MODELFLOWRELATION[0].FLOW_FLOWDEFINE_T;

                XmlReader readerxoml, readerule;
                StringReader strXoml = new StringReader(Xoml.XOML);
                StringReader strRules = new StringReader(Xoml.RULES == null ? "" : Xoml.RULES);

                readerxoml = XmlReader.Create(strXoml);
                readerule = XmlReader.Create(strRules);

                WorkflowRuntime workflowRuntime = new WorkflowRuntime();
                workflowRuntime.StartRuntime();

                FlowEvent ExternalEvent = new FlowEvent();
                ExternalDataExchangeService objService = new ExternalDataExchangeService();
                workflowRuntime.AddService(objService);
                objService.AddService(ExternalEvent);
                TypeProvider typeProvider = new TypeProvider(null);
                workflowRuntime.AddService(typeProvider);

                //XmlReader readerxoml = XmlReader.Create(HttpContext.Current.Server.MapPath ("TestFlow.xml"));
                // instance = workflowRuntime.CreateWorkflow(readerxoml);
                if (Xoml.RULES == null)
                    instance = workflowRuntime.CreateWorkflow(readerxoml);
                else
                    instance = workflowRuntime.CreateWorkflow(readerxoml, readerule, null);
                // instance = workflowRuntime.CreateWorkflow(typeof(TestFlow));
                instance.Start();
                StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(workflowRuntime, instance.InstanceId);

                //从实例中获取定义
                if (1 == 2)
                {
                    System.Workflow.Activities.StateMachineWorkflowActivity smworkflow = new StateMachineWorkflowActivity();
                    smworkflow = workflowinstance.StateMachineWorkflow;
                    RuleDefinitions ruleDefinitions = smworkflow.GetValue(RuleDefinitions.RuleDefinitionsProperty) as RuleDefinitions;
                    WorkflowMarkupSerializer markupSerializer = new WorkflowMarkupSerializer();

                    StringBuilder xoml = new StringBuilder();
                    StringBuilder rule = new StringBuilder();
                    XmlWriter xmlWriter = XmlWriter.Create(xoml);
                    XmlWriter ruleWriter = XmlWriter.Create(rule);
                    markupSerializer.Serialize(xmlWriter, smworkflow);
                    markupSerializer.Serialize(ruleWriter, ruleDefinitions);
                    xmlWriter.Close();
                    ruleWriter.Close();
                    StringReader readxoml = new StringReader(xoml.ToString());
                    StringReader readrule = new StringReader(rule.ToString());
                    XmlReader readerxoml2 = XmlReader.Create(readxoml);
                    XmlReader readerrule2 = XmlReader.Create(readrule);
                    WorkflowInstance instance1 = workflowRuntime.CreateWorkflow(readerxoml2, readerrule2, null);
                    instance1.Start();
                    StateMachineWorkflowInstance workflowinstance1 = new StateMachineWorkflowInstance(workflowRuntime, instance1.InstanceId);
                    workflowinstance1.SetState(StateName);
                }
                //从实例中获取定义并启动新实例

                //跳转到节点StateName
                workflowinstance.SetState(StateName);

                FlowDataType.FlowData FlowData = new FlowDataType.FlowData();
                FlowData.xml = xml;
                //  FlowData.Flow_FlowRecord_T = null;

                ExternalEvent.OnDoFlow(instance.InstanceId, FlowData);//激发流程引擎流转到下一状态
                System.Threading.Thread.Sleep(1000);
                PermissionServiceClient WcfPermissionService = new PermissionServiceClient();
                string CurrentStateName = workflowinstance.CurrentStateName == null ? "End" : workflowinstance.CurrentStateName; //取得当前状态
                List<UserInfo> listUser = new List<UserInfo>();
                if (CurrentStateName != "End")
                {
                    if (CurrentStateName.Substring(0, 5) == "State")
                    {
                        CurrentStateName = CurrentStateName.Substring(5);
                    }
                    string WFCurrentStateName = new Guid(CurrentStateName).ToString("D");
                    T_SYS_USER[]  User = WcfPermissionService.GetSysUserByRole(WFCurrentStateName); //检索本状态（角色）对应用户

                    if (User != null)
                        for (int i = 0; i < User.Length; i++)
                        {
                            UserInfo tmp = new UserInfo();
                            tmp.UserID = User[i].EMPLOYEEID;
                            tmp.UserName = User[i].EMPLOYEENAME;
                            listUser.Add(tmp);
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


                GetAppUserResult.UserInfo = listUser.Count > 0 ? listUser : null;

                if (GetAppUserResult.UserInfo == null)
                    GetAppUserResult.Err = "没有找到用户";

                return GetAppUserResult;
                // return listUser;


                //return workflowinstance.CurrentStateName == null ? "End" : workflowinstance.CurrentStateName;
            }
            catch (Exception ex)
            {
                GetAppUserResult.Err = ex.Message;
                GetAppUserResult.UserInfo = null;
                return GetAppUserResult;
            }
        }

        #endregion


        private bool GetUser( string OptFlag, string Xoml, string Rules, string xml, ref DataResult DataResult)
        {
            try
            {
                WorkflowRuntime WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);
                WorkflowInstance Instance = SMTWorkFlowManage.CreateWorkflowInstance(WfRuntime, Xoml, Rules);
                Tracer.Debug("GetUser(try)下根据模型文件创建工作流实例(完成) ID=" + Instance.InstanceId);
                string strNextState = SMTWorkFlowManage.GetNextStateByEvent(WfRuntime, Instance, "StartFlow", xml);


            }
            catch (Exception ex)
            {
            }
            finally
            {
            }

            return true;
        }

        public string UpdateFlow( FLOW_FLOWRECORDDETAIL_T entity)
        {

            FlowBLL2 bll = new FlowBLL2();
            bll.UpdateFlowRecord( entity, "", "");

            return "";
        }

        #region 查询流程信息
        /// <summary>
        /// 查询审批流程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<FLOW_FLOWRECORDDETAIL_T> GetFlowInfo( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {

            List<FLOW_FLOWRECORDDETAIL_T> list = new List<FLOW_FLOWRECORDDETAIL_T>();
            try
            {
                List<FlowType> FlowTypeList = new List<FlowWFService.FlowType>();
                FlowTypeList.Add(FlowType.Approval);
                FlowTypeList.Add(FlowType.Pending);
                //Debug.WriteLine("GetFlowInfo\n");
                //Debug.WriteLine(DateTime.Now.ToString());
                //Debug.WriteLine("\n");
                //有formid和modelcode不对返回数据量作限制，否则只返回前20条master数据
                if (!string.IsNullOrEmpty(FormID) && !string.IsNullOrEmpty(ModelCode))
                {
                    list = FlowBLL2.GetFlowInfoV( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
                }
                else
                {
                    Tracer.Debug("GetFlowInfoTop: formID:" + FormID + "--FlowGuid:" + FlowGUID
                        + "--CheckState:" + CheckState + "--Flag:" + Flag + "--ModelCode:" + ModelCode + "--CompanyID:" + CompanyID + "--EditUserID:" + EditUserID);
                    list = FlowBLL2.GetFlowInfoTop( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
                }

            }
            catch (Exception ex)
            {
                Tracer.Debug("GetFlowInfo异常信息 formid：" + FormID + ":" + ex.ToString());
                //Tracer.Debug("GetFlowInfo: -" + FormID + "-" + ex.InnerException +"\n"+ ex.Message);
                throw ex;

            }
            return list;
        }

        public List<FLOW_FLOWRECORDMASTER_T> GetFlowRecordMaster( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {
            try
            {
                return FlowBLL2.GetFlowRecordMaster( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetFlowRecordMaster: -" + ex.InnerException + ex.Message);
                throw ex;
            }
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
        public List<TaskInfo> GetTaskInfo( string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID)
        {

            return FlowBLL2.GetTaskInfo( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
        }
        #endregion

        #region "查询待审核单据"
        /// <summary>
        /// 根据模块代码和用户id查询待审核单据
        /// </summary>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="EditUserID">用户id</param>
        /// <returns></returns>
        public List<string> GetWaitingApprovalForm(string ModelCode, string EditUserID)
        {
            return FlowBLL2.GetWaitingApprovalForm(ModelCode, EditUserID);
        }
        #endregion

        /// <summary>
        /// 通过机构ID和模块代码查询对应流程代码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //public List<FLOW_MODELFLOWRELATION_T> GetFlowByModel(FLOW_MODELFLOWRELATION_T entity)
        //{
        //    return FlowBLL2.GetFlowByModel(entity);
        //}

        public string GetFlowDefine( SubmitData ApprovalData)
        {

            FlowBLL2 Flow = new FlowBLL2();
            return Flow.GetFlowDefine( ApprovalData);
        }

        /// <summary>
        /// 根据流程ID获取流程的所有分支
        /// </summary>
        /// <param name="FlowID"></param>
        /// <returns></returns>
        public List<string> GetFlowBranch(string FlowID)
        {
            FlowBLL2 Flow = new FlowBLL2();
            return Flow.GetFlowBranch(FlowID);
        }



    }
}
