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
        /// <summary>
        /// 获取下一状态数据
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="FlowDefineXML"></param>
        /// <param name="Rules"></param>
        /// <param name="FlowLayoutXML"></param>
        /// <param name="BusinessXml"></param>
        /// <param name="UserID"></param>
        /// <param name="PostID"></param>
        /// <param name="FlowType"></param>
        /// <param name="DataResult"></param>
        /// <param name="user"></param>
        public void TmFlowToNextStep(string companyID, string FlowDefineXML, string Rules, string FlowLayoutXML, string BusinessXml, string UserID, string PostID, FlowType FlowType, ref DataResult DataResult, ref FlowUser user)
        {

            WorkflowRuntime WfRuntime = null;
            WorkflowInstance Instance = null;
            string strCurrActivitID = "StartFlow";
            string strNextActivitID = "StartFlow";
            bool IsCountersign = false;
            string CountersignType = "0";
            //Role_UserType RuleName;
            List<UserInfo> AppUserInfo = null;
            Dictionary<FlowRole, List<UserInfo>> DictCounterUser = null;
            try
            {
                user.TrackingMessage += "创建工作流运行时 SMTWorkFlowManage.CreateWorkFlowRuntime(false)开始\r\n";
                //创建工作流
                WfRuntime = SMTWorkFlowManage.CreateWorkFlowRuntime(false);

                user.TrackingMessage += "创建工作流运行时SMTWorkFlowManage.CreateWorkFlowRuntime(false)完成\r\n";
                Instance = SMTWorkFlowManage.CreateWorkflowInstance(WfRuntime, FlowDefineXML, Rules);



                #region 激发事件到一下状态
                strCurrActivitID = strNextActivitID;
                //user.TrackingMessage += "激发事件到一下状态，并获取状态代码 SMTWorkFlowManage.GetNextStateByEvent(WfRuntime, Instance, strNextState, xml)开始" + Instance.InstanceId.ToString() + "\r\n";

                strNextActivitID = SMTWorkFlowManage.GetFlowNextStepRoles(WfRuntime, Instance, strNextActivitID, BusinessXml);

                //user.TrackingMessage += "激发事件到一下状态，并获取状态代码 SMTWorkFlowManage.GetNextStateByEvent(WfRuntime, Instance, strNextState, xml)完成" + Instance.InstanceId.ToString() + "\r\n";

                if (strNextActivitID == "EndFlow")
                {
                    strNextActivitID = strCurrActivitID;

                }
                else
                {
                    //获取节点对应的角色id
                    List<FlowRole> listRoleID = FlowUtility.GetRlueIdFromActivitID(FlowLayoutXML, strNextActivitID, ref IsCountersign, ref CountersignType);

                    if (listRoleID.Count == 0)
                    {
                        DataResult.Err = "流程:" + user.FlowName + " 没有找到对应角色";
                        DataResult.FlowResult = FlowResult.FAIL;
                        Tracer.Debug("Formid=" + user.FormID + ";活动属性 Name=" + strNextActivitID + ";没有在流程:" + user.FlowName + " Layout中找到对应角色,Layout如下:\r\r" + FlowLayoutXML);
                        return;
                    }
                    if (!IsCountersign)
                    {
                        Tracer.Debug("Formid=" + user.FormID + ";(非会签) 根活动的字符串查找角色状态码(即活动Name属性)StateCode=" + strNextActivitID + " Layout=" + FlowLayoutXML + "");
                        #region 非会签
                        bool isHigher = false;
                        //根据角色找人,如果角色有多个人,只找其中一个
                        AppUserInfo = TmGetUserByRoleId(listRoleID[0].RoleName, UserID, PostID, ref isHigher);

                        #region 打印审核人
                        string names = "\r\n=======FormID=" + user.FormID + " 非会签 根据角色找人,如果角色有多个人,只找其中一个 打印审核人B(listRole[0].RoleName=" + listRoleID[0].RoleName + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                        foreach (var us in AppUserInfo)
                        {
                            names += "CompanyID:" + us.CompanyID + "\r\n";
                            names += "DepartmentID:" + us.DepartmentID + "\r\n";
                            names += "PostID:" + us.PostID + "\r\n";
                            names += "UserID:" + us.UserID + "\r\n";

                            names += "CompanyName:" + us.CompanyName + "\r\n";
                            names += "DepartmentName:" + us.DepartmentName + "\r\n";
                            names += "PostName:" + us.PostName + "\r\n";
                            names += "UserName:" + us.UserName + "\r\n";
                            names += "----------------------------------------------------\r\n";
                        }
                        if (!isHigher && listRoleID[0].IsOtherCompany != null)
                        {
                            if (listRoleID[0].IsOtherCompany.Value == true)
                            {
                                names += "是否指定公司：" + listRoleID[0].IsOtherCompany.Value.ToString() + "\r\n";
                                names += "公司的ID：" + listRoleID[0].OtherCompanyID + "\r\n";
                                if (string.IsNullOrEmpty(listRoleID[0].OtherCompanyID))
                                {
                                    names += "Layout=" + FlowLayoutXML + "\r\n";
                                }
                            }
                            else if (listRoleID[0].IsOtherCompany.Value == false)
                            {
                                names += "实际要查找公司的ID:" + companyID + " " + user.GetCompanyName(companyID) + "\r\n";
                            }
                        }
                        user.ErrorMsg += names;
                        Tracer.Debug(names);
                        #endregion

                        #region beyond

                        if (!isHigher)
                        {
                            if (listRoleID[0].IsOtherCompany != null && listRoleID[0].IsOtherCompany.Value == true)
                            {//指定公司
                                //过滤人
                                AppUserInfo = AppUserInfo.Where(u => u.CompanyID == listRoleID[0].OtherCompanyID).ToList();
                            }
                            else
                            {
                                AppUserInfo = AppUserInfo.Where(u => u.CompanyID == companyID).ToList();
                            }
                        }
                        #endregion

                        if (AppUserInfo == null || AppUserInfo.Count == 0)
                        {
                            DataResult.Err = user.GetCompanyName(companyID) + " " + listRoleID[0].Remark + " 没有找到审核人";
                            DataResult.FlowResult = FlowResult.FAIL;
                            return;
                        }


                        if (AppUserInfo.Where(c => c.UserID == UserID).Count() == 0)
                        {

                        }

                        #endregion
                    }
                    else
                    {
                        Tracer.Debug("Formid=" + user.FormID + ";(会签) 根活动的字符串查找角色状态码(即活动Name属性)StateCode=" + strNextActivitID + " Layout=" + FlowLayoutXML + "");
                        #region 会签
                        DictCounterUser = new Dictionary<FlowRole, List<UserInfo>>();
                        if (CountersignType == "0")
                        {
                            #region 全部审核通过才算通过
                            for (int i = 0; i < listRoleID.Count; i++)
                            {
                                bool isHigher = false;


                                var listuserinfo = TmGetUserByRoleId(listRoleID[i].RoleName, UserID, PostID, ref isHigher);
                                #region 打印审核人
                                string names = "\r\n=======FormID=" + user.FormID + "会签 全部审核通过才算通过  打印审核人C(listRole[0].RoleName=" + listRoleID[i].RoleName + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                                if (listuserinfo != null)
                                {
                                    foreach (var u in listuserinfo)
                                    {
                                        names += "CompanyID:" + u.CompanyID + "\r\n";
                                        names += "DepartmentID:" + u.DepartmentID + "\r\n";
                                        names += "PostID:" + u.PostID + "\r\n";
                                        names += "UserID:" + u.UserID + "\r\n";

                                        names += "CompanyName:" + u.CompanyName + "\r\n";
                                        names += "DepartmentName:" + u.DepartmentName + "\r\n";
                                        names += "PostName:" + u.PostName + "\r\n";
                                        names += "UserName:" + u.UserName + "\r\n";
                                        names += "----------------------------------------------------\r\n";
                                    }
                                }
                                if (!isHigher && listRoleID[i].IsOtherCompany != null)
                                {
                                    if (listRoleID[i].IsOtherCompany.Value == true)
                                    {
                                        names += "是否指定公司：" + listRoleID[i].IsOtherCompany.Value.ToString() + "\r\n";
                                        names += "公司的ID：" + listRoleID[i].OtherCompanyID + "\r\n";
                                        if (string.IsNullOrEmpty(listRoleID[i].OtherCompanyID))
                                        {
                                            names += "Layout=" + FlowLayoutXML + "\r\n";
                                        }
                                    }
                                    else if (listRoleID[i].IsOtherCompany.Value == false)
                                    {
                                        names += "实际要查找公司的ID:" + companyID + "\r\n";
                                    }
                                }
                                user.ErrorMsg += names;
                                Tracer.Debug(names);
                                #endregion
                                if (!isHigher)
                                {
                                    if (listRoleID[i].IsOtherCompany != null && listRoleID[i].IsOtherCompany.Value == true)
                                    {
                                        listuserinfo = listuserinfo.Where(u => u.CompanyID == listRoleID[i].OtherCompanyID).ToList();
                                    }
                                    else
                                    {
                                        listuserinfo = listuserinfo.Where(u => u.CompanyID == companyID).ToList();
                                    }
                                }

                                if (listuserinfo == null || listuserinfo.Count == 0)
                                {
                                    DataResult.Err = user.GetCompanyName(companyID) + " " + listRoleID[i].Remark + " 没有找到审核人";
                                    DataResult.FlowResult = FlowResult.FAIL;
                                    return;
                                }
                                DictCounterUser.Add(listRoleID[i], listuserinfo);
                            }
                            #endregion
                        }
                        else
                        {
                            #region 只有一个审核通过了就算审核通过了
                            bool bFlag = false;//判断是否找到审核人
                            string roles = "";//得到所有的角色
                            user.TrackingMessage += "GetUserByStateCode\r\n";
                            for (int i = 0; i < listRoleID.Count; i++)
                            {
                                roles += listRoleID[i].Remark + "、";
                                #region
                                bool isHigher = false;

                                var listuserinfo = TmGetUserByRoleId(listRoleID[i].RoleName, UserID, PostID, ref isHigher);
                                #region 打印审核人
                                string names = "\r\n=======FormID=" + user.FormID + " 会签 只有一个审核通过了就算审核通过了  打印审核人C(listRole[0].RoleName=" + listRoleID[i].RoleName + ";UserID=" + UserID + ";PostID=" + PostID + ";isHigher=" + isHigher.ToString() + ")=======\r\n";
                                foreach (var u in listuserinfo)
                                {
                                    names += "CompanyID:" + u.CompanyID + "\r\n";
                                    names += "DepartmentID:" + u.DepartmentID + "\r\n";
                                    names += "PostID:" + u.PostID + "\r\n";
                                    names += "UserID:" + u.UserID + "\r\n";

                                    names += "CompanyName:" + u.CompanyName + "\r\n";
                                    names += "DepartmentName:" + u.DepartmentName + "\r\n";
                                    names += "PostName:" + u.PostName + "\r\n";
                                    names += "UserName:" + u.UserName + "\r\n";
                                    names += "----------------------------------------------------\r\n";
                                }
                                if (!isHigher && listRoleID[i].IsOtherCompany != null)
                                {
                                    if (listRoleID[i].IsOtherCompany.Value == true)
                                    {
                                        names += "是否指定公司：" + listRoleID[i].IsOtherCompany.Value.ToString() + "\r\n";
                                        names += "公司的ID：" + listRoleID[i].OtherCompanyID + "\r\n";
                                        if (string.IsNullOrEmpty(listRoleID[i].OtherCompanyID))
                                        {
                                            names += "Layout=" + FlowLayoutXML + "\r\n";
                                        }
                                    }
                                    else if (listRoleID[i].IsOtherCompany.Value == false)
                                    {
                                        names += "实际要查找公司的ID:" + companyID + "\r\n";
                                    }
                                }
                                user.ErrorMsg += names;
                                Tracer.Debug(names);
                                #endregion
                                if (!isHigher)
                                {
                                    if (listRoleID[i].IsOtherCompany != null && listRoleID[i].IsOtherCompany.Value == true)
                                    {
                                        listuserinfo = listuserinfo.Where(u => u.CompanyID == listRoleID[i].OtherCompanyID).ToList();
                                    }
                                    else
                                    {
                                        listuserinfo = listuserinfo.Where(u => u.CompanyID == companyID).ToList();
                                    }
                                }

                                if (listuserinfo != null && listuserinfo.Count > 0)
                                {
                                    bFlag = true;
                                    if (listuserinfo.FirstOrDefault(u => u.UserID == UserID) != null)
                                    {
                                        break;
                                    }
                                }
                                DictCounterUser.Add(listRoleID[i], listuserinfo);
                                #endregion
                            }
                            if (!bFlag)
                            {
                                DataResult.Err = user.GetCompanyName(companyID) + " " + roles + " 没有找到审核人";
                                DataResult.FlowResult = FlowResult.FAIL;
                                return;
                            }
                            user.TrackingMessage += " GetUserByStateCode完成\r\n";
                            //iscurruser = false;
                            #endregion
                        }
                        #endregion
                    }
                }
                #endregion

                DataResult.IsCountersign = IsCountersign;
                DataResult.AppState = strNextActivitID;
                DataResult.CountersignType = CountersignType;
                if (!IsCountersign)
                {
                    #region 检查非会签角色是否有多个审核人
                    Tracer.Debug("FormID=" + user.FormID + " 检查非会签角色的审核人数＝" + AppUserInfo.Count.ToString());
                    if (AppUserInfo.Count > 1) //处理角色对应多个用户,返回用户集给提交人，选择一个处理人
                    {
                        DataResult.FlowResult = FlowResult.MULTIUSER;
                    }

                    DataResult.UserInfo = AppUserInfo;
                    #endregion
                }
                else
                {
                    Tracer.Debug("FormID=" + user.FormID + " 检查会签角色的审核人数＝" + DictCounterUser.Count.ToString());
                    #region 检查会签角色是否有多个审核人,如果有多个审核人,则返回
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
                user.TrackingMessage += " iscurruser完成\r\n";

            }
            catch (Exception ex)
            {
                Tracer.Debug("Formid=" + user.FormID + ";GetUserByFlow2异常信息 ：" + ex.ToString());
                throw new Exception("获取下一状态数据出错,请联系管理员! \r\n FormID=" + user.FormID + "");
            }
            finally
            {
                strCurrActivitID = null;
                strNextActivitID = null;
                //RuleName = null;
                AppUserInfo = null;
                Instance = null;
                user.TrackingMessage += "  SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime)\r\n";
                SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime);
                user.TrackingMessage += "  SMTWorkFlowManage.ColseWorkFlowRuntime(WfRuntime)完成\r\n";

            }

        }
    }
}
