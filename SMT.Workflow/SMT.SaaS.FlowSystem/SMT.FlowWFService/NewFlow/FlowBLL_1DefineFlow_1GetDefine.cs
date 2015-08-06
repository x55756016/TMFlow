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
        #region 通过模块名查找使用流程
        /// <summary>
        /// 通过模块名查找使用流程
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="DepartID"></param>
        /// <param name="ModelCode"></param>
        /// <param name="FlowType"></param>
        /// <returns></returns>
        public static List<FLOW_MODELFLOWRELATION_T> GetFlowByModelName(string CompanyID, string DepartID, string ModelCode, string FlowType, ref FlowUser user)
        {
            try
            {
                //Flow_ModelFlowRelation_TDAL Dal = new Flow_ModelFlowRelation_TDAL();
                //以部门查找流程
                Tracer.Debug("以部门查找流程FLOW_MODELFLOWRELATION_TDAL.GetFlowByModelName:OrgType='1' ;CompanyID=" + CompanyID + ";DepartID=" + DepartID + ";FlowType=" + FlowType + "");
                List<FLOW_MODELFLOWRELATION_T> xoml = FLOW_MODELFLOWRELATION_TDAL.GetFlowByModelName(CompanyID, DepartID, ModelCode, FlowType, "1");
                if (xoml != null && xoml.Count > 0) //找到匹配流程返回
                {
                    #region 写日志
                    if (user != null)
                    {
                        if (CompanyID == user.CompayID && DepartID == user.DepartmentID)
                        {
                            Tracer.Debug("FormID=" + user.FormID + ";找到公司[ " + user.CompayName + " ]下部门[ " + user.DepartmentName + " ]的匹配流程返回");
                        }
                    }
                    #endregion
                    return xoml;
                }
                //部门的上级机构(一般是公司)查找流程
                if (user != null)
                {
                    Tracer.Debug("FormID=" + user.FormID + ";没有找到公司[ " + user.CompayName + " ]下部门[ " + user.DepartmentName + " ]的匹配流程返回,继续部门的上级机构查找流程");
                }
                //OrganizationServiceClient Organization = new OrganizationServiceClient();             
                Dictionary<string, string> OrganizationList = new Dictionary<string, string>();
                using (DepartmentBLL bll = new DepartmentBLL())
                {
                    OrganizationList = bll.GetFatherByDepartmentID(DepartID);
                }

                Tracer.Debug("FormID=" + user.FormID + ";继续查找部门的上级机构");
                if (OrganizationList == null || OrganizationList.Count <= 0)
                {
                    string info = "FormID=" + user.FormID + ";没有找到所在部门的上级机构";
                    #region 写日志
                    if (user != null)
                    {
                        if (DepartID == user.DepartmentID)
                        {
                            info = "FormID=" + user.FormID + ";没有找到所在部门[ " + user.DepartmentName + " ]的上一级上级机构";
                            Tracer.Debug(info);
                        }
                    }
                    #endregion
                    throw new Exception("没有找到所在部门[ " + user.DepartmentName + " ]的上一级上级机构");
                }
                foreach (var item in OrganizationList)
                {
                    if (item.Value == "0")
                    {

                        xoml = FLOW_MODELFLOWRELATION_TDAL.GetFlowByModelName(CompanyID, item.Key, ModelCode, FlowType, "0"); //如果上级机构是公司直接查询公司关联流程并返回
                        #region 写日志
                        if (user.CompayID == CompanyID)
                        {
                            Tracer.Debug("FormID=" + user.FormID + ";找到所在部门[ " + user.DepartmentName + " ]的上一级上级机构 [" + user.CompayName + "]");
                        }
                        else
                        {
                            Tracer.Debug("FormID=" + user.FormID + ";找到所在部门[ " + user.DepartmentName + " ]的上一级上级机构 ");
                        }
                        #endregion
                        return xoml;
                    }

                    xoml = FLOW_MODELFLOWRELATION_TDAL.GetFlowByModelName(CompanyID, item.Key, ModelCode, FlowType, "1");

                    if (xoml != null && xoml.Count > 0) //找到匹配流程返回
                    {
                        #region 写日志
                        if (user.CompayID == CompanyID)
                        {
                            Tracer.Debug("FormID=" + user.FormID + ";找到所在部门[ " + user.DepartmentName + " ]的上一级上级机构 [" + user.CompayName + "]");
                        }
                        else
                        {
                            Tracer.Debug("FormID=" + user.FormID + ";找到所在部门[ " + user.DepartmentName + " ]的上一级上级机构 ");
                        }
                        #endregion
                        return xoml;
                    }

                }
                return xoml;

            }
            catch (Exception ex)
            {
                Tracer.Debug("FlowBLL->GetFlowByModelName：" + ex.Message);
                throw new Exception("GetFlowByModelName:" + ex.Message);// return null;
            }

        }

        #endregion
    }
}
