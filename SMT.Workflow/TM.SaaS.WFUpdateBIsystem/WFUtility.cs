using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections.ObjectModel;
using System.Resources;
using SMT.Foundation.Log;
using TM_SaaS_OA_EFModel;
using SMT.SaaS.OA.BLL;
using SMT.FB.BLL;
using SMT.Foundation.Core;
using System.Configuration;

namespace TM.SaaS.WFUpdateBIsystem
{
    public class WFUtility
    {
        public static IDAO dao = DALFacoty.CreateDao(ConfigurationManager.ConnectionStrings["WorkFlowConnString"].ToString());

        #region 流程更新业务系统
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysType">系统类型</param>
        /// <param name="EntityType">实体类型</param>
        /// <param name="EntityKey">实体主键名</param>
        /// <param name="EntityId">主键ID</param>
        /// <param name="CheckState">审核状态</param>
        /// <param name="message">业务服务的反馈消息</param>
        /// <returns></returns>
        public static bool UpdateFormCheckState(string SysType, string EntityType, string EntityKey, string EntityId, SMT.HRM.BLL.Common.CheckStates CheckState, ref string message, string strXmlParams)
        {
            bool result = false;
            try
            {
                result = InnerUpdateFormCheckState(SysType, EntityType, EntityKey, EntityId, CheckState, ref message, strXmlParams);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = false;
                Tracer.Debug("流程引擎需要回滚，审核异常：模块代码：" + SysType + " 模块id：" + EntityId + " 审核状态:" + CheckState + " 错误消息：" + ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysType">系统类型</param>
        /// <param name="EntityType">实体类型</param>
        /// <param name="EntityKey">实体主键名</param>
        /// <param name="EntityId">主键ID</param>
        /// <param name="CheckState">审核状态</param>
        /// <returns></returns>
        public static bool InnerUpdateFormCheckState(string SysType, string EntityType, string EntityKey, string EntityId, SMT.HRM.BLL.Common.CheckStates CheckState, ref string message, string strXmlParams)
        {
            bool IsResult = false;
            try
            {
                Tracer.Debug("系统名称：" + SysType);
                Tracer.Debug("流程修改审核状态UpdateFormCheckState" + "实体名: " + EntityType);
                Tracer.Debug("表单ID名:" + EntityKey + "表单值：" + EntityId);
                Tracer.Debug("审核状态：" + ((int)CheckState).ToString());
                int i = 0;
                if (SysType == "HR")
                {         
                    i = SMT.HRM.BLL.Utility.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState).ToString());
                    if (i > 0)
                    {
                        IsResult = true;
                    }
                }
                if (SysType == "OA")
                {
                    if (EntityType.ToUpper() == "T_OA_GIFTAPPLYMASTER")
                    {
                        //GSCommonWS.GSCommonServicesClient gsClient = new GSCommonWS.GSCommonServicesClient();
                        //var iresult = gsClient.UpdateCheckState(EntityType, EntityId, ((int)CheckState).ToString(), ref message);
                        //if (iresult > 0)
                        //{
                        //    IsResult = true;
                        //}
                    }
                    else
                    {
                        CommBll<T_OA_AGENTSET> Combll = new CommBll<T_OA_AGENTSET>();
                        i = Combll.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState).ToString());
                        if (i > 0)
                        {
                            IsResult = true;
                        }
                    }
                }
                if (SysType == "FB")
                {
                    //日常管理的状态改动
                    if (EntityType == "T_FB_BORROWAPPLYMASTER" || EntityType == "T_FB_CHARGEAPPLYMASTER" || EntityType == "T_FB_REPAYAPPLYMASTER")
                    {
                        SMT.FBAnalysis.BLL.CommBll<T_FB_BORROWAPPLYMASTER> Combll = new SMT.FBAnalysis.BLL.CommBll<T_FB_BORROWAPPLYMASTER>();
                        i = Combll.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState).ToString());                 
                        if (i > 0)
                        {
                            IsResult = true;
                        }
                    }
                    else
                    {
                        string strMsg = string.Empty;
                        using (FBCommonBLL bll = new FBCommonBLL())
                        {
                            i = bll.UpdateCheckState(EntityType, EntityId, ((int)CheckState).ToString(), ref strMsg);
                        }                        
                        if (i > 0)
                        {
                            IsResult = true;
                        }
                        else
                        {
                            Tracer.Debug(strMsg);
                        }

                    }
                }
                if (SysType == "RM")
                {
                    //OrgClient = new OrganizationWS.OrganizationServiceClient();
                    Tracer.Debug("调用了招聘系统中的：" + EntityType);
                    //RMServicesClient rmClient = new RMServicesClient();
                    //int i = rmClient.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState), strXmlParams);
                    //if (i > 0)
                    //{
                    //    IsResult = true;
                    //}
                }

                if (SysType == "WP")
                {
                    //WPServicesWS.WPServicesClient wpClient = new WPServicesClient();
                    //Tracer.Debug("调用了工作计划中的：EntityType:" + EntityType + " EntityKey:" + EntityKey + "\r\n" + " EntityId:" + EntityId + " CheckState:" + CheckState + " URL:" + wpClient.Endpoint.Address);
                    //Tracer.Debug("(int)CheckState):" + ((int)CheckState).ToString());
                    //int i = wpClient.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState), strXmlParams, ref message);
                    //Tracer.Debug("调用工作计划返回结果" + i.ToString() + "\r\n" + message);
                    //if (i > 0)
                    //{
                    //    IsResult = true;
                    //}
                }
                if (SysType == "TM")
                {
                    //OrgClient = new OrganizationWS.OrganizationServiceClient();

                    //TMServicesWS.TMServicesClient tmClient = new TMServicesClient();
                    //Tracer.Debug("调用了培训系统中的：EntityType:" + EntityType + " EntityKey:" + EntityKey + "\r\n" + " EntityId:" + EntityId + " CheckState:" + CheckState + " URL:" + tmClient.Endpoint.Address);

                    //int i = tmClient.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState), strXmlParams);
                    //Tracer.Debug("调用培训系统返回结果" + i.ToString());
                    //if (i > 0)
                    //{
                    //    IsResult = true;
                    //}
                }
                //考试系统
                if (SysType == "EM")
                {
                    //EMServiceWS.EMServicesClient emClient = new EMServiceWS.EMServicesClient();
                    //Tracer.Debug("调用了考试系统中的：EntityType:" + EntityType + " EntityKey:" + EntityKey + "\r\n" + " EntityId:" + EntityId + " CheckState:" + CheckState + " URL:" + emClient.Endpoint.Address + " strXmlParams:" + strXmlParams);

                    //int i = emClient.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState), strXmlParams);
                    //Tracer.Debug("调用考试系统返回结果" + i.ToString());
                    //if (i > 0)
                    //{
                    //    IsResult = true;
                    //}
                }
                if (SysType == "VM")
                {
                    //VMServiceWS.VMServicesClient vmClient = new VMServiceWS.VMServicesClient();
                    //Tracer.Debug("调用了车辆系统中的：EntityType:" + EntityType + " EntityKey:" + EntityKey + "\r\n" + " EntityId:" + EntityId + " CheckState:" + CheckState + " URL:" + vmClient.Endpoint.Address + " strXmlParams:" + strXmlParams);

                    //int i = vmClient.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState), strXmlParams);
                    //Tracer.Debug("调用车辆系统返回结果" + i.ToString());
                    //if (i > 0)
                    //{
                    //    IsResult = true;
                    //}
                }
                if (SysType == "TK")
                {
                    //TKServicesWS.TKServicesClient tkClient = new TKServicesWS.TKServicesClient();
                    //Tracer.Debug("调用了任务系统中的：EntityType:" + EntityType + " EntityKey:" + EntityKey + "\r\n" + " EntityId:" + EntityId + " CheckState:" + CheckState + " URL:" + tkClient.Endpoint.Address + " strXmlParams:" + strXmlParams);

                    //int i = tkClient.UpdateCheckState(EntityType, EntityKey, EntityId, ((int)CheckState), strXmlParams);
                    //Tracer.Debug("调用任务系统返回结果" + i.ToString());
                    //if (i > 0)
                    //{
                    //    IsResult = true;
                    //}
                }else
                {
                    //直接更新单据审核状态
                    string sql = "update " + EntityType + " set checkstate='" + ((int)CheckState).ToString() + "'"
                        + " where " + EntityKey + " ='" + EntityId + "'";
                    i=dao.ExecuteNonQuery(sql);
                    if (i > 0)
                    {
                        IsResult = true;
                    }
                }

                SMT.Foundation.Log.Tracer.Debug("WF流程UpdateCheckState成功： strEntityName：" + EntityType
                + " EntityKeyName:" + EntityKey + " EntityKeyValue：" + EntityId
                + " CheckState:" + CheckState + " 受影响的记录数：" + i.ToString());
            }
            catch (Exception  bex)
            {
                Tracer.Debug("手机版修改审核状态UpdateFormCheckState" + System.DateTime.Now.ToString() + " " + bex.ToString());
                throw bex;
            }
            return IsResult;
        }

        #endregion
    }
}
