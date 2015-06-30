using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using SMT.FLOWDAL.ADO;
using System.Data;
using SMT.Foundation.Log;
using SMT.Foundation.Core;

namespace SMT.FlowDAL.ADO
{
    /// <summary>
    /// 提供对外接口，让外部操作流程的数据，内部不用
    /// </summary>
    public class OutInterface : BaseFlowDAL
    {
        #region 获取元数据
        /// <summary>
        /// 获取元数据
        /// </summary>
        /// <param name="formid">formid</param>
        /// <returns></returns>
        public static string GetMetadataByFormid(string formid)
        {
            try
            {
                string sql = "select businessobject from FLOW_FLOWRECORDMASTER_T where formid='" + formid + "' order by createdate desc ";

                    try
                    {
                       
                       DataTable  dt= dao.GetDataTable(sql);
                       if (dt != null && dt.Rows.Count > 0)
                       {
                           return dt.Rows[0]["businessobject"].ToString();//取新新的一条
                       }
                       else
                       {
                           return "";
                       }
                    }
                    catch (Exception ex)
                    {
                        Tracer.Debug("获取元数据:GetMetadataByFormid-> nullGetDataTable:异常信息：" + ex.Message);
                        return "";
                    }
                    finally
                    {
                        Tracer.Debug("获取元数据:GetMetadataByFormid-> SQL=" + sql);

                    }
                
            }
            catch (Exception ex)
            {
                Tracer.Debug("获取元数据:GetMetadataByFormid:异常信息：" + ex.Message);
                return "";
            }
        }
        #endregion
        #region 更新元数据
       /// <summary>
        /// 更新元数据
       /// </summary>
        /// <param name="formid">formid</param>
       /// <param name="xml"></param>
       /// <returns></returns>
        public static bool UpdateMetadataByFormid(string formid, string xml)
        {
            try
            {
                string sql = "UPDATE FLOW_FLOWRECORDMASTER_T set BUSINESSOBJECT=:BUSINESSOBJECT where FORMID=:FORMID ";
                string sql2 = "UPDATE T_WF_DOTASK set APPXML=:APPXML where ORDERID=:FORMID ";

                    try
                    {

                        dao.BeginTransaction();
                        #region 审核主表
                        Parameter[] pageparm =
                        { 
                            new Parameter(":FORMID",null), 
                            new Parameter(":BUSINESSOBJECT",null)                   

                        };
                        pageparm[0].ParameterValue = GetValue(formid);//
                        pageparm[1].ParameterValue = GetValue(xml);//
                        int n = dao.ExecuteNonQuery(sql, pageparm);
                        Tracer.Debug("时间：" + DateTime.Now.ToString() + "UpdateMetadataByFormid：【审核主表FLOW_FLOWRECORDMASTER_T】[更新元数据]成功 影响记录数：" + n + ";formid＝" + formid + ";xml=" + xml);
                        #endregion
                        #region 待办任务
                        Parameter[] pageparm2 =
                        { 
                            new Parameter(":FORMID",OracleType.NVarChar), 
                            new Parameter(":APPXML",null)                   

                        };
                        pageparm2[0].ParameterValue = GetValue(formid);//
                        pageparm2[1].ParameterValue = GetValue(xml);//
                        int n2 = dao.ExecuteNonQuery(sql2, pageparm2);
                        Tracer.Debug("时间：" + DateTime.Now.ToString() + "UpdateMetadataByFormid：【待办任务T_WF_DOTASK】[更新元数据]成功 影响记录数：" + n2 + ";formid＝" + formid + ";xml=" + xml);

                        #endregion
                        dao.Commit();
                        if ((n+n2) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        dao.Rollback();
                        Tracer.Debug("更新元数据 UpdateMetadataByFormid 异常信息：" + ex.Message);
                        return false;
                    }
                    finally
                    {
                        Tracer.Debug("更新元数据:UpdateMetadataByFormid-> SQL=" + sql);

                    }
                
            }
            catch (Exception ex)
            {
                Tracer.Debug("更新元数据:UpdateMetadataByFormid:异常信息：" + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
