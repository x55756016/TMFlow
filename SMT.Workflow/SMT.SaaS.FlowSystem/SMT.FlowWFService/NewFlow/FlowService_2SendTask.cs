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
        private void SubmitEngineService(SubmitData submitData, DataResult dataResult, ref FlowUser sUser, CheckResult CheckFlowResult, FlowBLL Flowbill, string AppCompanyID, string MessageUserID, string MessageUserName, ref string ErroMessage)
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

                            bool bCancel = einginBll.FlowCancel(submitData, FlowResultXml.ToString(), submitData.XML, ref ErroMessage);
                            if (!bCancel)
                            {
                                Tracer.Debug("触发流程返回的结果 bCancel＝" + bCancel.ToString() + "  FormID=" + submitData.FormID + "\r\nFlowResultXml=" + FlowResultXml.ToString() + "\r\n submitDataXML=" + submitData.XML);
                            }
                            #endregion
                            break;
                        case SubmitFlag.Approval:
                            #region 审核
                            //FlowEngine.TaskMsgClose(modelinfo.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
                            einginBll.TaskMsgClose(sUser.SysCode, submitData.FormID, submitData.ApprovalUser.UserID);
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
                                        einginBll.TaskMsgClose(sUser.SysCode, submitData.FormID, messageUserID);
                                    }
                                    tmpMessageData = new MessageData("Flow", sUser.SysCode, AppCompanyID, submitData.ModelCode, sUser.ModelName,
                                  submitData.FormID, dataResult.AppState, dataResult.CheckState, IsTask, MessageUserID, MessageUserName, dataResult.KPITime);
                                    FlowResultXml = Flowbill.BuildMessageData(tmpMessageData);
                                    //bOK = FlowEngine.SaveFlowTriggerData(FlowResultXml.ToString(), submitData.XML);                                   

                                    Tracer.Debug("开始审核FlowEngine.SaveFlowTriggerData FormID=" + submitData.FormID + "");
                                    Tracer.Debug("流程引擎的XML数据 审核: FormID=" + submitData.FormID + ";SysCode=" + sUser.SysCode + " ModelName=" + sUser.ModelName + "\r\n FlowResultXml= \r\n" + FlowResultXml + "\r\n submitData.XML= \r\n" + submitData.XML);
                                    bOK = einginBll.SaveFlowTriggerData(submitData, FlowResultXml.ToString(), submitData.XML, ref sUser, ref ErroMessage);
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
                                        einginBll.TaskMsgClose(sUser.SysCode, submitData.FormID, messageUserID);
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
                                bOK = einginBll.SaveFlowTriggerData(submitData, FlowResultXml.ToString(), submitData.XML, ref sUser, ref ErroMessage);
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
    }
}
