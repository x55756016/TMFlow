﻿/*---------------------------------------------------------------------  
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
    // 注意: 如果更改此处的类名“IService1”，也必须更新 App.config 中对“IService1”的引用。
    public partial class Service
    {
        public static IDAO dao = DALFacoty.CreateDao(ConfigurationManager.ConnectionStrings["WorkFlowConnString"].ToString());

        FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();      
        EnginFlowBLL einginBll = new EnginFlowBLL();
        public FlowUser SUser;
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
                    #region 记录日志
                    FLOW_CONSULTATION_T entity = flowConsultation;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("FLOW_CONSULTATION_T entity= new FLOW_CONSULTATION_T();");//
                    sb.AppendLine("entity.CONSULTATIONID = \"" + entity.CONSULTATIONID + "\";");//
                    sb.AppendLine("entity.FLOWRECORDDETAILID = \"" + (entity.FLOW_FLOWRECORDDETAIL_T == null ? "" : entity.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID) + "\";");
                    sb.AppendLine("entity.CONSULTATIONUSERID = \"" + entity.CONSULTATIONUSERID + "\";");
                    sb.AppendLine("entity.CONSULTATIONUSERNAME = \"" + entity.CONSULTATIONUSERNAME + "\";");
                    sb.AppendLine("entity.CONSULTATIONCONTENT =\"" + entity.CONSULTATIONCONTENT + "\";");
                    sb.AppendLine("entity.CONSULTATIONDATE = \"" + entity.CONSULTATIONDATE + "\";");
                    sb.AppendLine("entity.REPLYUSERID = \"" + entity.REPLYUSERID + "\";");
                    sb.AppendLine("entity.REPLYUSERNAME = \"" + entity.REPLYUSERNAME + "\";");
                    sb.AppendLine("entity.REPLYCONTENT = \"" + entity.REPLYCONTENT + "\";");
                    sb.AppendLine("entity.REPLYDATE = \"" + entity.REPLYDATE + "\";");
                    sb.AppendLine("entity.FLAG = \"" + entity.FLAG + "\";");//0未回复，1回复


                    sb.AppendLine("FLOW_FLOWRECORDDETAIL_T model=new FLOW_FLOWRECORDDETAIL_T();");
                    sb.AppendLine("  model.FLOWRECORDDETAILID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID + "\";");
                    //sb.AppendLine("  model.FLOWRECORDMASTERID=\""+entity.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID+"\";");
                    sb.AppendLine("  model.STATECODE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.STATECODE + "\";");
                    sb.AppendLine("  model.PARENTSTATEID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.PARENTSTATEID + "\";");
                    sb.AppendLine("  model.CONTENT=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CONTENT + "\";");
                    sb.AppendLine("  model.CHECKSTATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CHECKSTATE + "\";//同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8");
                    sb.AppendLine("  model.FLAG=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLAG + "\";//已审批：1，未审批:0");
                    sb.AppendLine("  model.CREATEUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEUSERID + "\";");
                    sb.AppendLine("  model.CREATEUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEUSERNAME + "\";");
                    sb.AppendLine("  model.CREATECOMPANYID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATECOMPANYID + "\";");
                    sb.AppendLine("  model.CREATEDEPARTMENTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEDEPARTMENTID + "\";");
                    sb.AppendLine("  model.CREATEPOSTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEPOSTID + "\";");
                    sb.AppendLine("  model.CREATEDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEDATE + "\";");
                    sb.AppendLine("  model.EDITUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITUSERID + "\";");
                    sb.AppendLine("  model.EDITUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITUSERNAME + "\";");
                    sb.AppendLine("  model.EDITCOMPANYID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITCOMPANYID + "\";");
                    sb.AppendLine("  model.EDITDEPARTMENTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITDEPARTMENTID + "\";");
                    sb.AppendLine("  model.EDITPOSTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITPOSTID + "\";");
                    sb.AppendLine("  model.EDITDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITDATE + "\";");
                    sb.AppendLine("  model.AGENTUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.AGENTUSERID + "\";");
                    sb.AppendLine("  model.AGENTERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.AGENTERNAME + "\";");
                    sb.AppendLine("  model.AGENTEDITDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.AGENTEDITDATE + "\";");
                    sb.AppendLine("  entity.FLOW_FLOWRECORDDETAIL_T = model;");


                    sb.AppendLine("FLOW_FLOWRECORDMASTER_T eentity=new FLOW_FLOWRECORDMASTER_T();");
                    sb.AppendLine("  eentity.FLOWRECORDMASTERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "\";");
                    sb.AppendLine("  eentity.INSTANCEID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.INSTANCEID + "\";");
                    sb.AppendLine("  eentity.FLOWSELECTTYPE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE + "\";//0:固定流程，1：自选流程");
                    sb.AppendLine("  eentity.MODELCODE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.MODELCODE + "\";");
                    sb.AppendLine("  eentity.FLOWCODE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWCODE + "\";");
                    sb.AppendLine("  eentity.FORMID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "\";");
                    sb.AppendLine("  eentity.FLOWTYPE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWTYPE + "\";//0:审批流程，1：任务流程");
                    sb.AppendLine("  eentity.CHECKSTATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CHECKSTATE + "\";//1:审批中，2：审批通过，3审批不通过，5撤销(为与字典保持一致)");
                    sb.AppendLine("  eentity.CREATEUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEUSERID + "\";");
                    sb.AppendLine("  eentity.CREATEUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME + "\";");
                    sb.AppendLine("  eentity.CREATECOMPANYID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID + "\";");
                    sb.AppendLine("  eentity.CREATEDEPARTMENTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID + "\";");
                    sb.AppendLine("  eentity.CREATEPOSTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID + "\";");
                    sb.AppendLine("  eentity.CREATEDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEDATE + "\";");
                    sb.AppendLine("  eentity.EDITUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.EDITUSERID + "\";");
                    sb.AppendLine("  eentity.EDITUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME + "\";");
                    sb.AppendLine("  eentity.EDITDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.EDITDATE + "\";");
                    sb.AppendLine("  eentity.ACTIVEROLE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.ACTIVEROLE + "\";");
                    sb.AppendLine("  eentity.BUSINESSOBJECT=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT + "\";");
                    sb.AppendLine("  eentity.KPITIMEXML=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.KPITIMEXML + "\";");
                    sb.AppendLine("  entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T = eentity;");

                    Tracer.Debug("流程引擎的XML数据"+ "SubimtFlow入口处 进行咨询: FormID=" + submitData.FormID + ";ModelCode=" + submitData.ModelCode + " \r\n FLOW_CONSULTATION_T 实体:\r\n" + sb.ToString());
                    WriteSubmitDate(submitData);
                    #endregion

                    flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T =
                        FlowBLL.GetFLOW_FLOWRECORDMASTER_T( flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);
                    FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    //FlowEngineService.CustomUserMsg[] cs = new FlowEngineService.CustomUserMsg[1];
                    //FlowEngineService.CustomUserMsg cu = new FlowEngineService.CustomUserMsg();                               
                    //cu.FormID = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID;
                    //cu.UserID = flowConsultation.REPLYUSERID;
                    //cs[0] = cu;

                    List<CustomUserMsg> UserAndForm = new List<CustomUserMsg>();
                    CustomUserMsg cu = new CustomUserMsg();
                    cu.FormID = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID;
                    cu.UserID = flowConsultation.REPLYUSERID;
                    UserAndForm.Add(cu);

                    ModelInfo modelinfo = FlowBLL.GetSysCodeByModelCode( submitData.ModelCode);
                    MessageData tmpMessageData = new MessageData("Flow", modelinfo.SysCode,
                        flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID,
                        submitData.ModelCode, modelinfo.ModelName, submitData.FormID, flowConsultation.FLOW_FLOWRECORDDETAIL_T.STATECODE, flowConsultation.FLOW_FLOWRECORDDETAIL_T.CHECKSTATE, "", "", "", "");
                    FlowBLL flowBLL = new FlowBLL();
                    StringBuilder FlowResultXml = flowBLL.BuildMessageData(tmpMessageData);
                    //FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    //log = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);
                    if (!string.IsNullOrEmpty(flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT))
                    {
                        submitData.XML = flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT;
                    }

                    //bool b = FlowEngine.FlowConsultati(cs, "", FlowResultXml.ToString(), submitData.XML);
                    EnginFlowBLL efbll = new EnginFlowBLL();

                    bool b = efbll.FlowConsultati( UserAndForm, "", FlowResultXml.ToString(), submitData.XML);
                    if (!b)
                    {
                        Tracer.Debug("发送咨询失败：FormID" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID
                            + "\n\r submitData.XML:" + submitData.XML);
                    }

                    FlowBLL bll = new FlowBLL();

                    bll.AddConsultation( flowConsultation);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("发送咨询出错: FORMID=" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "异常信息:\r\n" + ex.InnerException + ex.Message);
                    throw ex;
                }
                finally
                {
                }
            }



        }
        public void ReplyConsultation( FLOW_CONSULTATION_T flowConsultation, SubmitData submitData)
        {

            if (strIsFlowEngine.ToLower() == "true")
            {
                try
                {


                    #region 记录日志
                    FLOW_CONSULTATION_T entity = flowConsultation;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("FLOW_CONSULTATION_T entity= new FLOW_CONSULTATION_T();");//
                    sb.AppendLine("entity.CONSULTATIONID = \"" + entity.CONSULTATIONID + "\";");//
                    sb.AppendLine("entity.FLOWRECORDDETAILID = \"" + (entity.FLOW_FLOWRECORDDETAIL_T == null ? "" : entity.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID) + "\";");
                    sb.AppendLine("entity.CONSULTATIONUSERID = \"" + entity.CONSULTATIONUSERID + "\";");
                    sb.AppendLine("entity.CONSULTATIONUSERNAME = \"" + entity.CONSULTATIONUSERNAME + "\";");
                    sb.AppendLine("entity.CONSULTATIONCONTENT =\"" + entity.CONSULTATIONCONTENT + "\";");
                    sb.AppendLine("entity.CONSULTATIONDATE = \"" + entity.CONSULTATIONDATE + "\";");
                    sb.AppendLine("entity.REPLYUSERID = \"" + entity.REPLYUSERID + "\";");
                    sb.AppendLine("entity.REPLYUSERNAME = \"" + entity.REPLYUSERNAME + "\";");
                    sb.AppendLine("entity.REPLYCONTENT = \"" + entity.REPLYCONTENT + "\";");
                    sb.AppendLine("entity.REPLYDATE = \"" + entity.REPLYDATE + "\";");
                    sb.AppendLine("entity.FLAG = \"" + entity.FLAG + "\";");//0未回复，1回复
                    sb.AppendLine("FLOW_FLOWRECORDDETAIL_T model=new FLOW_FLOWRECORDDETAIL_T();");
                    sb.AppendLine("  model.FLOWRECORDDETAILID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID + "\";");
                    //sb.AppendLine("  model.FLOWRECORDMASTERID=\""+entity.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID+"\";");
                    sb.AppendLine("  model.STATECODE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.STATECODE + "\";");
                    sb.AppendLine("  model.PARENTSTATEID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.PARENTSTATEID + "\";");
                    sb.AppendLine("  model.CONTENT=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CONTENT + "\";");
                    sb.AppendLine("  model.CHECKSTATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CHECKSTATE + "\";//同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8");
                    sb.AppendLine("  model.FLAG=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLAG + "\";//已审批：1，未审批:0");
                    sb.AppendLine("  model.CREATEUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEUSERID + "\";");
                    sb.AppendLine("  model.CREATEUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEUSERNAME + "\";");
                    sb.AppendLine("  model.CREATECOMPANYID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATECOMPANYID + "\";");
                    sb.AppendLine("  model.CREATEDEPARTMENTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEDEPARTMENTID + "\";");
                    sb.AppendLine("  model.CREATEPOSTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEPOSTID + "\";");
                    sb.AppendLine("  model.CREATEDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.CREATEDATE + "\";");
                    sb.AppendLine("  model.EDITUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITUSERID + "\";");
                    sb.AppendLine("  model.EDITUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITUSERNAME + "\";");
                    sb.AppendLine("  model.EDITCOMPANYID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITCOMPANYID + "\";");
                    sb.AppendLine("  model.EDITDEPARTMENTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITDEPARTMENTID + "\";");
                    sb.AppendLine("  model.EDITPOSTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITPOSTID + "\";");
                    sb.AppendLine("  model.EDITDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.EDITDATE + "\";");
                    sb.AppendLine("  model.AGENTUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.AGENTUSERID + "\";");
                    sb.AppendLine("  model.AGENTERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.AGENTERNAME + "\";");
                    sb.AppendLine("  model.AGENTEDITDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.AGENTEDITDATE + "\";");
                    sb.AppendLine("  entity.FLOW_FLOWRECORDDETAIL_T = model;");

                    sb.AppendLine("FLOW_FLOWRECORDMASTER_T eentity=new FLOW_FLOWRECORDMASTER_T();");
                    sb.AppendLine("  eentity.FLOWRECORDMASTERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "\";");
                    sb.AppendLine("  eentity.INSTANCEID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.INSTANCEID + "\";");
                    sb.AppendLine("  eentity.FLOWSELECTTYPE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE + "\";//0:固定流程，1：自选流程");
                    sb.AppendLine("  eentity.MODELCODE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.MODELCODE + "\";");
                    sb.AppendLine("  eentity.FLOWCODE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWCODE + "\";");
                    sb.AppendLine("  eentity.FORMID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "\";");
                    sb.AppendLine("  eentity.FLOWTYPE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FLOWTYPE + "\";//0:审批流程，1：任务流程");
                    sb.AppendLine("  eentity.CHECKSTATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CHECKSTATE + "\";//1:审批中，2：审批通过，3审批不通过，5撤销(为与字典保持一致)");
                    sb.AppendLine("  eentity.CREATEUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEUSERID + "\";");
                    sb.AppendLine("  eentity.CREATEUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME + "\";");
                    sb.AppendLine("  eentity.CREATECOMPANYID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID + "\";");
                    sb.AppendLine("  eentity.CREATEDEPARTMENTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID + "\";");
                    sb.AppendLine("  eentity.CREATEPOSTID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEPOSTID + "\";");
                    sb.AppendLine("  eentity.CREATEDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.CREATEDATE + "\";");
                    sb.AppendLine("  eentity.EDITUSERID=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.EDITUSERID + "\";");
                    sb.AppendLine("  eentity.EDITUSERNAME=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.EDITUSERNAME + "\";");
                    sb.AppendLine("  eentity.EDITDATE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.EDITDATE + "\";");
                    sb.AppendLine("  eentity.ACTIVEROLE=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.ACTIVEROLE + "\";");
                    sb.AppendLine("  eentity.BUSINESSOBJECT=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.BUSINESSOBJECT + "\";");
                    sb.AppendLine("  eentity.KPITIMEXML=\"" + entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.KPITIMEXML + "\";");
                    sb.AppendLine("  entity.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T = eentity;");
                    Tracer.Debug("流程引擎的XML数据 SubimtFlow入口处 回复咨询信息: FormID=" + submitData.FormID + ";ModelCode=" + submitData.ModelCode + " \r\n FLOW_CONSULTATION_T 实体:\r\n" + sb.ToString());
                    WriteSubmitDate(submitData);
                    #endregion

                    //FlowEngineService.EngineWcfGlobalFunctionClient FlowEngine = new FlowEngineService.EngineWcfGlobalFunctionClient();
                    EnginFlowBLL FlowEngine = new EnginFlowBLL();
                    //Byte[] Bo = System.Text.UTF8Encoding.UTF8.GetBytes(submitData.XML);
                    //XElement xemeBoObject = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(Bo)));
                    //string strSystemCode = (from item in xemeBoObject.Descendants("Name") select item).FirstOrDefault().Value;


                    ModelInfo modelinfo = FlowBLL.GetSysCodeByModelCode( submitData.ModelCode);
                    // FlowEngine.FlowConsultatiClose(modelinfo.SysCode, flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID, flowConsultation.REPLYUSERID);
                    FlowEngine.FlowConsultatiClose( modelinfo.SysCode, flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID, flowConsultation.REPLYUSERID);

                    FlowBLL bll = new FlowBLL();
                    bll.ReplyConsultation( flowConsultation);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("回复咨询出错，异常信息: FORMID=" + flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOW_FLOWRECORDMASTER_T.FORMID + "异常信息:\r\n" + ex.InnerException + ex.Message);
                    throw ex;
                }
            }

        }
        #endregion
        string[] arr = new string[] { "POSTLEVEL", "TYPEAPPROVAL" };
        /// <summary>
        /// XML数据验证
        /// </summary>
        /// <param name="XML">XML</param>
        /// <returns></returns>
        private bool VerifyXML(string XML)
        {
            try
            {
                StringReader strRdr = new StringReader(XML);
                XDocument xdoc = XDocument.Load(strRdr);
                var items = (from item in xdoc.Descendants("Object") select item).FirstOrDefault();
                if (items.Attribute("Key").Value == string.Empty || items.Attribute("id").Value == string.Empty)
                {
                    return true;
                }
                var model = items.Descendants("Attribute");
                foreach (string item in arr)
                {
                    var dd = from ent in model where ent.Attribute("Name").Value == item select ent;
                    if (dd.FirstOrDefault() != null)
                    {
                        if (dd.FirstOrDefault().Attribute("DataValue").Value == string.Empty)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Tracer.Debug("XML数据验证出错:" + e.ToString());
                return false;
            }
        }

        #region 流程处理

        #region 流程与任务审批
        private void WriteSubmitDate(SubmitData submitData)
        {
            #region 提交信息
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("submitData.FlowSelectType =FlowSelectType." + submitData.FlowSelectType + ";");
            sb.AppendLine("submitData.FormID = \"" + submitData.FormID + "\";");
            sb.AppendLine("submitData.ModelCode = \"" + submitData.ModelCode + "\";");
            sb.AppendLine("submitData.ApprovalUser = new UserInfo();");
            sb.AppendLine("submitData.ApprovalUser.CompanyID = \"" + submitData.ApprovalUser.CompanyID + "\";");
            sb.AppendLine("submitData.ApprovalUser.DepartmentID = \"" + submitData.ApprovalUser.DepartmentID + "\";");
            sb.AppendLine("submitData.ApprovalUser.PostID = \"" + submitData.ApprovalUser.PostID + "\";");
            sb.AppendLine("submitData.ApprovalUser.UserID = \"" + submitData.ApprovalUser.UserID + "\";");
            sb.AppendLine("submitData.ApprovalUser.UserName = \"" + submitData.ApprovalUser.UserName + "\";");
            sb.AppendLine("submitData.NextStateCode = \"" + (submitData.NextStateCode != null ? submitData.NextStateCode : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser = new UserInfo();");
            sb.AppendLine("submitData.NextApprovalUser.CompanyID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.CompanyID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.DepartmentID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.DepartmentID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.PostID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.PostID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.UserID = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.UserID : "空") + "\";");
            sb.AppendLine("submitData.NextApprovalUser.UserName = \"" + (submitData.NextApprovalUser != null ? submitData.NextApprovalUser.UserName : "空") + "\";");
            sb.AppendLine("submitData.SubmitFlag = SubmitFlag." + submitData.SubmitFlag + ";");
            // sb.AppendLine("submitData.XML = \"" + submitData.XML + "\";");
            sb.AppendLine("submitData.FlowType = FlowType." + submitData.FlowType.ToString() + ";");
            sb.AppendLine("submitData.ApprovalResult = ApprovalResult." + submitData.ApprovalResult.ToString() + ";");
            sb.AppendLine("submitData.ApprovalContent = \"" + submitData.ApprovalContent + "\";");            
            if (submitData.DictCounterUser != null)
            {
                if (submitData.DictCounterUser.Count > 0)
                {
                    string name = "";
                    foreach (KeyValuePair<Role_UserType, List<UserInfo>> u in submitData.DictCounterUser)
                    {
                        name += "角色名称：" + u.Key.Remark + "  人数：" + u.Value.Count + "\r\n";
                        foreach (var user in u.Value)
                        {                            
                            name += "姓名：" + user.UserName + " 公司：" + user.CompanyName + "\r\n";
                        }
                        name += "---------------------------------------------------------------\r\n";
                    }
                    sb.AppendLine("FormID=" + submitData.FormID + " 选择会签角色的人员如下:\r\n" + name);
                    
                }
            }

            #region 提交人信息
            sb.AppendLine("submitData.SumbitCompanyID = \"" + submitData.SumbitCompanyID + "\";");
            sb.AppendLine("submitData.SumbitDeparmentID = \"" + submitData.SumbitDeparmentID + "\";");
            sb.AppendLine("submitData.SumbitPostID = \"" + submitData.SumbitPostID + "\";");
            sb.AppendLine("submitData.SumbitUserID = \"" + submitData.SumbitUserID + "\";");
            sb.AppendLine("submitData.SumbitUserName = \"" + submitData.SumbitUserName + "\";");

            #endregion

            Tracer.Debug("提交审核的信息：\r\n" + sb.ToString() + "\r\n");
            Tracer.Debug("流程引擎的XML数据 SubimtFlow入口处:" + submitData.ApprovalContent + " FormID=" + submitData.FormID + ";ModelCode=" + submitData.ModelCode + " \r\n submitData.XML:\r\n" + submitData.XML);
            #endregion
        }

        private void InsertInstanceState(DataRow dr)
        {
            String connStringPersistence = ConfigurationManager.ConnectionStrings["//OracleConnection"].ConnectionString;//Data Source=172.30.50.110;Initial Catalog=WorkflowPersistence;Persist Security Info=True;User ID=sa;Password=fbaz2012;MultipleActiveResultSets=True";
            //OracleConnection con = MsOracle.GetOracleConnection(connStringPersistence);
            try
            {
                string insSql = "INSERT INTO INSTANCE_STATE (INSTANCE_ID,STATE,STATUS,UNLOCKED,BLOCKED,INFO,MODIFIED,OWNER_ID,OWNED_UNTIL,NEXT_TIMER) VALUES (:INSTANCE_ID,:STATE,:STATUS,:UNLOCKED,:BLOCKED,:INFO,:MODIFIED,:OWNER_ID,:OWNED_UNTIL,:NEXT_TIMER)";
                Parameter[] pageparm =
                {               
                    new Parameter(":INSTANCE_ID",null), 
                    new Parameter(":STATE",null), 
                    new Parameter(":STATUS",null), 
                    new Parameter(":UNLOCKED",null), 
                    new Parameter(":BLOCKED",null), 
                    new Parameter(":INFO",null), 
                    new Parameter(":MODIFIED",null), 
                    new Parameter(":OWNER_ID",null), 
                    new Parameter(":OWNED_UNTIL",null), 
                    new Parameter(":NEXT_TIMER",null) 

                };
                pageparm[0].ParameterValue = GetValue(dr["instance_id"]);//
                pageparm[1].ParameterValue =GetValue(dr["state"]);//
                pageparm[2].ParameterValue =GetValue(dr["status"]);//
                pageparm[3].ParameterValue =GetValue(dr["unlocked"]);//
                pageparm[4].ParameterValue =GetValue(dr["blocked"]);//
                pageparm[5].ParameterValue =GetValue(dr["info"]);//
                pageparm[6].ParameterValue =GetValue(dr["modified"]);//
                pageparm[7].ParameterValue =GetValue(dr["owner_id"]);//
                pageparm[8].ParameterValue =GetValue(dr["owned_until"]);//
                pageparm[9].ParameterValue =GetValue(dr["next_timer"]);//

                dao.ExecuteNonQuery(insSql, pageparm);
                Tracer.Debug("恢复完成工作流InstanceState:" + dr["instance_id"]);
            }
            catch (Exception ex)
            {
                Tracer.Debug("恢复完成工作流 失败 InstanceState :" + dr["instance_id"] + " \r\n异常信息：" + ex.Message);
                //throw new Exception("FLOW_CONSULTATION_TDAL_Add:" + ex.Message);//不抛出
            }
        }
        /// <summary>
        /// 发送审批消息（调用引擎服务）
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="dataResult"></param>
        /// <param name="sUser"></param>
        /// <param name="CheckFlowResult"></param>
        /// <param name="Flowbill"></param>
        /// <param name="AppCompanyID">申请公司</param>
        /// <param name="MessageUserID">申请人ID</param>
        /// <param name="MessageUserName">申请人名</param>
        private void SubmitEngineService( SubmitData submitData, DataResult dataResult, ref FlowUser sUser, CheckResult CheckFlowResult, FlowBLL Flowbill, string AppCompanyID, string MessageUserID, string MessageUserName, ref string ErroMessage)
        {
            bool bOK = true;
            #region 发送审批消息（调用引擎服务）
            try
            {
                if (dataResult.FlowResult == FlowResult.SUCCESS || dataResult.FlowResult == FlowResult.END)
                {
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
                                #region 会签
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
                                #region 非会签(多个人时,只取其中一个)
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
                            tmpMessageData = new MessageData("Flow", sUser.SysCode, AppCompanyID, submitData.ModelCode, sUser.ModelName,
                                submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                            FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);

                            sUser.TrackingMessage += "流程引擎的XML数据新增: FormID=" + submitData.FormID + ";SysCode=" + sUser.SysCode + " ModelName=" + sUser.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML;
                            bOK = einginBll.SaveFlowTriggerData(submitData, FlowResultXml.ToString(), submitData.XML, ref sUser, ref ErroMessage);
                            sUser.TrackingMessage += "触发流程返回的结果 bOK＝" + bOK.ToString() + "\r\n";
                            sUser.TrackingMessage += "结束新增 FormID=" + submitData.FormID + " FlowEngine.SaveFlowTriggerData\r\n";
                            if (!bOK)
                            {
                                Tracer.Debug("触发流程返回的结果 bOK＝" + bOK.ToString() + "  FormID=" + submitData.FormID + "\r\nFlowResultXml=" + FlowResultXml.ToString() + "\r\n submitDataXML=" + submitData.XML);
                            }

                            #endregion
                            break;
                        case SubmitFlag.Cancel:
                            #region 撤单
                            tmpMessageData = new MessageData("Flow", sUser.SysCode, AppCompanyID, submitData.ModelCode, sUser.ModelName,
                                submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                            FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                            //bool bCancel = FlowEngine.FlowCancel(FlowResultXml.ToString(), submitData.XML);
                            sUser.TrackingMessage += "流程引擎的XML数据新增: FormID=" + submitData.FormID + ";SysCode=" + sUser.SysCode + " ModelName=" + sUser.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML;

                            bool bCancel = einginBll.FlowCancel( submitData,FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                            if (!bCancel)
                            {
                                Tracer.Debug("触发流程返回的结果 bCancel＝" + bCancel.ToString() + "  FormID=" + submitData.FormID + "\r\nFlowResultXml=" + FlowResultXml.ToString() + "\r\n submitDataXML=" + submitData.XML);
                            }
                            #endregion
                            break;
                        case SubmitFlag.Approval:
                            #region 审核
                            //FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                            einginBll.TaskMsgClose( sUser.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                            if (dataResult.CurrentIsCountersign)
                            {
                                #region 会签
                                if (submitData.ApprovalResult == ApprovalResult.NoPass)
                                {
                                    #region 审核不通过
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
                                        einginBll.TaskMsgClose( sUser.SysCode, submitData.FormID, messageUserID);
                                    }
                                    tmpMessageData = new MessageData("Flow", sUser.SysCode, AppCompanyID, submitData.ModelCode, sUser.ModelName,
                                  submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                    FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                    //bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);                                   

                                    Tracer.Debug("开始审核FlowEngine.SaveFlowTriggerData FormID=" + submitData.FormID + "");
                                    Tracer.Debug("流程引擎的XML数据 审核: FormID=" + submitData.FormID + ";SysCode=" + sUser.SysCode + " ModelName=" + sUser.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML);
                                    bOK = einginBll.SaveFlowTriggerData( submitData,FlowResultXml.ToString(), submitData.XML, ref sUser, ref ErroMessage);
                                    Tracer.Debug("bOK＝" + bOK.ToString());
                                    Tracer.Debug("结束审核FlowEngine.SaveFlowTriggerData FormID=" + submitData.FormID + "");
                                    if (!bOK)
                                    {
                                        Tracer.Debug("FlowEngineService:FormID=" + submitData.FormID + "\r\nFlowResultXml=" + FlowResultXml.ToString() + "\r\n submitDataXML=" + submitData.XML);
                                    }
                                    #endregion
                                }
                                else if (dataResult.IsGotoNextState)
                                {
                                    #region 到下一状态
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
                                        einginBll.TaskMsgClose( sUser.SysCode, submitData.FormID, messageUserID);
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
                                    tmpMessageData = new MessageData("Flow", sUser.SysCode, AppCompanyID, submitData.ModelCode, sUser.ModelName,
                                        submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                    FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                    bOK = einginBll.SaveFlowTriggerData(submitData, FlowResultXml.ToString(), submitData.XML, ref sUser, ref ErroMessage);

                                    if (!bOK)
                                    {
                                        Tracer.Debug("FlowEngineService:FormID=" + submitData.FormID + "\r\nFlowResultXml=" + FlowResultXml.ToString() + "\r\n submitDataXML=" + submitData.XML);
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region 非会签
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
                                tmpMessageData = new MessageData("Flow", sUser.SysCode, AppCompanyID, submitData.ModelCode, sUser.ModelName,
                                    submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                sUser.TrackingMessage += "流程引擎的XML数据新增: FormID=" + submitData.FormID + ";SysCode=" + sUser.SysCode + " ModelName=" + sUser.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML;
                                bOK = einginBll.SaveFlowTriggerData(submitData,FlowResultXml.ToString(), submitData.XML, ref sUser, ref ErroMessage);
                                if (!bOK)
                                {
                                    Tracer.Debug("触发流程返回的结果 bOK＝" + bOK.ToString() + "  FormID=" + submitData.FormID + "\r\nFlowResultXml=" + FlowResultXml.ToString() + "\r\n submitDataXML=" + submitData.XML);
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
                Tracer.Debug("发送审批消息FORMID=" + sUser.FormID + " 异常信息:" + ex.ToString() + "\r\n");
                if (string.IsNullOrEmpty(ErroMessage))
                {
                    throw new Exception("发送审批消息失败,请联系管理员!");
                }
                else
                {
                    throw new Exception(ErroMessage);
                }
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
        /// 获取用户有哪些未处理的单据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<FLOW_FLOWRECORDMASTER_T> GetFlowDataByUserID( string UserID)
        {
            try
            {
                FlowBLL Flow = new FlowBLL();
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
            Tracer.Debug("GetAppUser 启动与工作流程相同类型流程，查询对应节点用户 ModelCode=" + ModelCode + " FlowGUID=" + FlowGUID + " CompanyID=" + CompanyID);
            FlowUser user = SUser;
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
                    List<FLOW_FLOWRECORDDETAIL_T> FlowRecord = FlowBLL.GetFlowInfo( "", FlowGUID, "", "", "", "", "", null);
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
                Tracer.Debug("根据公司ID，模块代码获取配置的流程FlowBLL.GetFlowByModelName:OgrType='0'");

                List<FLOW_MODELFLOWRELATION_T> MODELFLOWRELATION = FlowBLL.GetFlowByModelName( CompanyID, "", ModelCode, "0", ref user);

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
                //ExternalDataExchangeService objService = new ExternalDataExchangeService();
                //workflowRuntime.AddService(objService);
                //objService.AddService(ExternalEvent);
                //TypeProvider typeProvider = new TypeProvider(null);
                //workflowRuntime.AddService(typeProvider);

                ////XmlReader readerxoml = XmlReader.Create(HttpContext.Current.Server.MapPath ("TestFlow.xml"));
                //// instance = workflowRuntime.CreateWorkflow(readerxoml);
                //if (Xoml.RULES == null)
                //    instance = workflowRuntime.CreateWorkflow(readerxoml);
                //else
                //    instance = workflowRuntime.CreateWorkflow(readerxoml, readerule, null);
                //// instance = workflowRuntime.CreateWorkflow(typeof(TestFlow));
                //instance.Start();
                //StateMachineWorkflowInstance workflowinstance = new StateMachineWorkflowInstance(workflowRuntime, instance.InstanceId);

                ////从实例中获取定义
                //if (1 == 2)
                //{
                //    System.Workflow.Activities.StateMachineWorkflowActivity smworkflow = new StateMachineWorkflowActivity();
                //    smworkflow = workflowinstance.StateMachineWorkflow;
                //    RuleDefinitions ruleDefinitions = smworkflow.GetValue(RuleDefinitions.RuleDefinitionsProperty) as RuleDefinitions;
                //    WorkflowMarkupSerializer markupSerializer = new WorkflowMarkupSerializer();

                //    StringBuilder xoml = new StringBuilder();
                //    StringBuilder rule = new StringBuilder();
                //    XmlWriter xmlWriter = XmlWriter.Create(xoml);
                //    XmlWriter ruleWriter = XmlWriter.Create(rule);
                //    markupSerializer.Serialize(xmlWriter, smworkflow);
                //    markupSerializer.Serialize(ruleWriter, ruleDefinitions);
                //    xmlWriter.Close();
                //    ruleWriter.Close();
                //    StringReader readxoml = new StringReader(xoml.ToString());
                //    StringReader readrule = new StringReader(rule.ToString());
                //    XmlReader readerxoml2 = XmlReader.Create(readxoml);
                //    XmlReader readerrule2 = XmlReader.Create(readrule);
                //    WorkflowInstance instance1 = workflowRuntime.CreateWorkflow(readerxoml2, readerrule2, null);
                //    instance1.Start();
                //    StateMachineWorkflowInstance workflowinstance1 = new StateMachineWorkflowInstance(workflowRuntime, instance1.InstanceId);
                //    workflowinstance1.SetState(StateName);
                //}
                //从实例中获取定义并启动新实例

                //跳转到节点StateName
                //workflowinstance.SetState(StateName);

                FlowDataType.FlowData FlowData = new FlowDataType.FlowData();
                FlowData.xml = xml;
                //  FlowData.Flow_FlowRecord_T = null;

                //ExternalEvent.OnDoFlow(instance.InstanceId, FlowData);//激发流程引擎流转到下一状态
                System.Threading.Thread.Sleep(1000);
                PermissionServiceClient WcfPermissionService = new PermissionServiceClient();
                string CurrentStateName = ""; //workflowinstance.CurrentStateName == null ? "End" : //workflowinstance.CurrentStateName; //取得当前状态
                List<UserInfo> listUser = new List<UserInfo>();
                if (CurrentStateName != "End")
                {
                    if (CurrentStateName.Substring(0, 5) == "State")
                    {
                        CurrentStateName = CurrentStateName.Substring(5);
                    }
                    string WFCurrentStateName = new Guid(CurrentStateName).ToString("D");
                    T_SYS_USER[] User = WcfPermissionService.GetSysUserByRole(WFCurrentStateName); //检索本状态（角色）对应用户

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

            FlowBLL bll = new FlowBLL();
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
            Tracer.Debug("GetFlowInfo 查询审批流程 ModelCode=" + ModelCode + " FormID=" + FormID + " EditUserID=" + EditUserID);
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
                    list = FlowBLL.GetFlowInfoV( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
                }
                else
                {
                    Tracer.Debug("GetFlowInfoTop: formID:" + FormID + "--FlowGuid:" + FlowGUID
                        + "--CheckState:" + CheckState + "--Flag:" + Flag + "--ModelCode:" + ModelCode + "--CompanyID:" + CompanyID + "--EditUserID:" + EditUserID);
                    list = FlowBLL.GetFlowInfoTop( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID, FlowTypeList);
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
            Tracer.Debug("GetFlowRecordMaster 获取[流程审批实例]信息 ModelCode=" + ModelCode + " FormID=" + FormID + " EditUserID=" + EditUserID);
            try
            {
                return FlowBLL.GetFlowRecordMaster( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
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
            Tracer.Debug("GetTaskInfo查询任务信息信息 ModelCode=" + ModelCode + " FormID=" + FormID + " EditUserID=" + EditUserID);
            return FlowBLL.GetTaskInfo( FormID, FlowGUID, CheckState, Flag, ModelCode, CompanyID, EditUserID);
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
            Tracer.Debug("根据模块代码和用户id查询待审核单据 ModelCode=" + ModelCode + " EditUserID=" + EditUserID);
            return FlowBLL.GetWaitingApprovalForm(ModelCode, EditUserID);
        }
        #endregion


        public string GetFlowDefine( SubmitData ApprovalData)
        {
            Tracer.Debug("GetFlowDefine获取流程信息 ModelCode=" + ApprovalData.ModelCode + " FormID=" + ApprovalData.FormID);
            FlowBLL Flow = new FlowBLL();
            return Flow.GetFlowDefine( ApprovalData);
        }
        /// <summary>
        /// 根据流程ID获取流程的所有分支
        /// </summary>
        /// <param name="FlowID"></param>
        /// <returns></returns>
        public List<string> GetFlowBranch(string FlowID)
        {
            FlowBLL Flow = new FlowBLL();
            return Flow.GetFlowBranch(FlowID);
        }

        /// <summary>
        /// 对外接口：根据我的单据ID获取记录实体
        /// </summary>
        /// <param name="personalrecordid">我的单据ID</param>
        /// <returns></returns>
        public T_WF_PERSONALRECORD GetPersonalRecordByID(string personalrecordid)
        {
            FlowBLL bll = new FlowBLL();
            //OracleConnection con = ADOHelper.GetOracleConnection();
            return bll.GetPersonalRecordByID( personalrecordid);
        }
        /// <summary>
        /// 判断是否可能用自选流程或提单人可以撒回流程
        /// string[0]=1 提交人可以撒回流程
        /// string[1]=1 可以用自选流程
        /// </summary>
        /// <param name="modelcode">模块代码</param>
        /// <param name="companyid">公司ID</param>       
        public string[] IsFreeFlowAndIsCancel(string modelcode, string companyid)
        {           
            return FLOW_FLOWRECORDDETAIL_TDAL.IsFreeFlowAndIsCancel(modelcode, companyid); 
        }
        /// <summary>
        /// KPI调用
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<FLOW_MODELFLOWRELATION_T> GetModelFlowRelationInfosListBySearch(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount)
        {
            //OracleConnection con = ADOHelper.GetOracleConnection();
            try
            {

                List<FLOW_MODELFLOWRELATION_T> ents = new List<FLOW_MODELFLOWRELATION_T>();
                string sql = " select t.MODELFLOWRELATIONID,t.COMPANYID,t.DEPARTMENTID,t.FLOWCODE,a.DESCRIPTION,t.FLAG from smtwf.FLOW_MODELFLOWRELATION_T t left join smtwf.FLOW_FLOWDEFINE_T a on t.flowcode=a.flowcode";
                if (!string.IsNullOrEmpty(sort))
                {
                    sql += " order by " + sort + "";
                }

                DataTable dt = dao.GetDataTable( sql);
                foreach (DataRow row in dt.Rows)
                {
                    FLOW_MODELFLOWRELATION_T ent = new FLOW_MODELFLOWRELATION_T();
                    ent.MODELFLOWRELATIONID = row["MODELFLOWRELATIONID"] == null ? "" : row["MODELFLOWRELATIONID"].ToString();
                    ent.MODELFLOWRELATIONID = row["COMPANYID"] == null ? "" : row["COMPANYID"].ToString();
                    ent.MODELFLOWRELATIONID = row["DEPARTMENTID"] == null ? "" : row["DEPARTMENTID"].ToString();
                    ent.MODELFLOWRELATIONID = row["FLOWCODE"] == null ? "" : row["FLOWCODE"].ToString();
                    ent.MODELFLOWRELATIONID = row["DESCRIPTION"] == null ? "" : row["DESCRIPTION"].ToString();
                    ent.MODELFLOWRELATIONID = row["FLAG"] == null ? "" : row["FLAG"].ToString();
                    ents.Add(ent);

                }

                IQueryable<FLOW_MODELFLOWRELATION_T> listTemp = ents.AsQueryable();
                int Count = ents.Count;
                pageCount = Count / pageSize + (Count % pageSize > 0 ? 1 : 0);
                var entList = listTemp.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return entList.ToList();

            }
            catch (Exception ex)
            {                
                return null;
                throw (ex);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 获取元数据
        /// </summary>
        /// <param name="formid"></param>
        /// <returns></returns>       
        public string GetMetadataByFormid(string formid)
        {
            return SMT.FlowDAL.ADO.OutInterface.GetMetadataByFormid(formid);
        }
        /// <summary>
        /// 更新元数据
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="xml"></param>
        /// <returns></returns>       
        public bool UpdateMetadataByFormid(string formid, string xml)
        {
            return SMT.FlowDAL.ADO.OutInterface.UpdateMetadataByFormid(formid, xml);
        }

        public static object GetValue(object value)
        {
            return value == null ? DBNull.Value.ToString() : value;
        }
    }
}
