using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.Workflow.Common.Model;
using System.Data;
using SMT.Workflow.Common.Model.FlowEngine;
using System.Data.OracleClient;
using SMT.Foundation.Core;
namespace SMT.Workflow.Platform.DAL
{
    public class EngineDAL : BaseDAL
    {
        #region 默认消息增、删、改、查方法
        //默认消息查询
        public IQueryable<T_WF_MESSAGEBODYDEFINE> GetFlowMsgDefine()
        {
            try
            {
                string sql = "SELECT * FROM T_WF_MESSAGEBODYDEFINE";
                
                DataTable dtFlowModelDefine = dao.GetDataTable(sql);
                
                var items = ToList<T_WF_MESSAGEBODYDEFINE>(dtFlowModelDefine);
                return items.AsQueryable();
            }
            catch 
            {
                return null;
            }
        }
        //默认消息条件查询
        public IQueryable<T_WF_MESSAGEBODYDEFINE> GetListFlowMsgDefine(string filterString, int pageIndex, int pageSize, ref int pageCount)
        {
            try
            {
                string sql = string.Empty;
                if (filterString != null && filterString!= "")
                {
                    sql = "SELECT * FROM T_WF_MESSAGEBODYDEFINE where " + filterString;
                }
                else 
                {
                    sql = "SELECT * FROM T_WF_MESSAGEBODYDEFINE";
                }
                
                DataTable dtEngineNews = dao.GetDataTable(sql);
                
                var items = ToList<T_WF_MESSAGEBODYDEFINE>(dtEngineNews).AsQueryable();
                items = Pager<T_WF_MESSAGEBODYDEFINE>(items, pageIndex, pageSize, ref pageCount);
                return items;
            }
            catch
            {
                return null;
            }
        }
        //默认消息查询
        public T_WF_MESSAGEBODYDEFINE GetListFlowMsgBodyDefine(string moduleCode)
        {
            try
            {
                string sql = "SELECT * FROM T_WF_MESSAGEBODYDEFINE where MODELCODE = '" + moduleCode + "'";
                
                DataTable dtFlowModelDefine = dao.GetDataTable(sql);
                
                var items = ToList<T_WF_MESSAGEBODYDEFINE>(dtFlowModelDefine);
                return items[0];
            }
            catch
            {
                return null;
            }
        }
        //添加默认消息
        public bool AddFlowMsgDefine(T_WF_MESSAGEBODYDEFINE FlowMsg)
        {
            try
            {
                int result = 0;

                

                //string sql = "insert into T_WF_MESSAGEBODYDEFINE(DEFINEID, SYSTEMCODE, MODELCODE, MESSAGEBODY, MESSAGEURL, CREATEDATE,COMPANYID,CREATEUSERNAME,CREATEUSERID,MESSAGETYPE)"
                //                                 + "values('" + FlowMsg.DEFINEID + "','" + FlowMsg.SYSTEMCODE + "','" + FlowMsg.MODELCODE + "',"
                //                                        + "'" + FlowMsg.MESSAGEBODY + "','" + FlowMsg.MESSAGEURL + "',to_date('" + FlowMsg.CREATEDATE + "','yyyy-mm-dd hh24:mi:ss'),"
                //                                        + "'" + FlowMsg.COMPANYID + "','" + FlowMsg.CREATEUSERNAME + "','" + FlowMsg.CREATEUSERID + "','" + FlowMsg.MESSAGETYPE + "')";
                //result = dao.ExecuteNonQuery(sql);

                 string insSql = "INSERT INTO T_WF_MESSAGEBODYDEFINE (DEFINEID,COMPANYID,SYSTEMCODE,MODELCODE,MESSAGEBODY,MESSAGEURL,MESSAGETYPE,CREATEDATE,CREATEUSERNAME,CREATEUSERID) VALUES (:DEFINEID,:COMPANYID,:SYSTEMCODE,:MODELCODE,:MESSAGEBODY,:MESSAGEURL,:MESSAGETYPE,:CREATEDATE,:CREATEUSERNAME,:CREATEUSERID)";
            Parameter[] pageparm =
                {  
                    new Parameter(":DEFINEID",GetValue(FlowMsg.DEFINEID)), //默认消息ID 
                    new Parameter(":COMPANYID",GetValue(FlowMsg.COMPANYID)), //公司ID 
                    new Parameter(":SYSTEMCODE",GetValue(FlowMsg.SYSTEMCODE)), //系统代号 
                    new Parameter(":MODELCODE",GetValue(FlowMsg.MODELCODE)), //模块代码 
                    new Parameter(":MESSAGEBODY",GetValue(FlowMsg.MESSAGEBODY)), //消息体 
                    new Parameter(":MESSAGEURL",GetValue(FlowMsg.MESSAGEURL)), //消息链接 
                    new Parameter(":MESSAGETYPE",GetValue(FlowMsg.MESSAGETYPE)), //消息类型 
                    new Parameter(":CREATEDATE",GetValue(FlowMsg.CREATEDATE)), //创建日期 
                    new Parameter(":CREATEUSERNAME",GetValue(FlowMsg.CREATEUSERNAME)), //创建人名称 
                    new Parameter(":CREATEUSERID",GetValue(FlowMsg.CREATEUSERID)) //创建人 

                };
            ParameterCollection pras = new ParameterCollection();
            foreach (var item in pageparm)
            {
                pras.Add(item);
            }
            result = dao.ExecuteNonQuery(insSql, System.Data.CommandType.Text, pras);
                return result > 0 ? true : false;
            }
            catch
            {
                return false;
            }
            finally
            {
                
            }
        }
        //修改默认消息
        public bool UpdateFlowMsgDefine(T_WF_MESSAGEBODYDEFINE FlowMsg)
        {
            try
            {
                int result = 0;
                
                //string sql = "update  T_WF_MESSAGEBODYDEFINE set "
                //                         + "SYSTEMCODE = '" + FlowMsg.SYSTEMCODE + "', "
                //                         + "MODELCODE = '" + FlowMsg.MODELCODE + "', "
                //                         + "MESSAGEBODY = '" + FlowMsg.MESSAGEBODY + "',"
                //                         + "MESSAGEURL = '" + FlowMsg.MESSAGEURL + "' "
                //                         + "where DEFINEID = '" + FlowMsg.DEFINEID + "' ";

                //result = dao.ExecuteNonQuery(sql);

                 string updSql = "UPDATE T_WF_MESSAGEBODYDEFINE SET SYSTEMCODE=:SYSTEMCODE,MODELCODE=:MODELCODE,MESSAGEBODY=:MESSAGEBODY,MESSAGEURL=:MESSAGEURL WHERE   DEFINEID=:DEFINEID";
            Parameter[] pageparm =
                { 
                    new Parameter(":DEFINEID",GetValue(FlowMsg.DEFINEID)), //默认消息ID 
                    new Parameter(":SYSTEMCODE",GetValue(FlowMsg.SYSTEMCODE)), //系统代号 
                    new Parameter(":MODELCODE",GetValue(FlowMsg.MODELCODE)), //模块代码 
                    new Parameter(":MESSAGEBODY",GetValue(FlowMsg.MESSAGEBODY)), //消息体 
                    new Parameter(":MESSAGEURL",GetValue(FlowMsg.MESSAGEURL)) //消息链接 

                };
            ParameterCollection pras = new ParameterCollection();
            foreach (var item in pageparm)
            {
                pras.Add(item);
            }
            result = dao.ExecuteNonQuery(updSql, System.Data.CommandType.Text, pras);
                return result > 0 ? true : false;
            }
            catch
            {
                return false;
            }
            finally
            {
                
            }
        }
        //删除默认消息
        public bool DeleteFlowMsgDefine(List<T_WF_MESSAGEBODYDEFINE> FlowMsglList)
        {
            try
            {
                int result = 0;
                
                dao.BeginTransaction();

                foreach (var item in FlowMsglList)
                {
                    string Sql = "delete from T_WF_MESSAGEBODYDEFINE "
                                  + "where DEFINEID = '" + item.DEFINEID + "' "; 
                    result = dao.ExecuteNonQuery(Sql);
                    if (result == 0)
                    {
                        dao.Rollback();
                        return false;
                    }
                }

                dao.Commit();
                return result > 0 ? true : false;
            }
            catch 
            {
                dao.Rollback();
                return false;
            }
            finally
            {
                
            }
        }
        //删除默认消息
        public bool DeleteFlowMsgsDefine(T_WF_MESSAGEBODYDEFINE FlowMsglList)
        {
            try
            {
                int result = 0;
                
                dao.BeginTransaction();
                string Sql = "delete from T_WF_MESSAGEBODYDEFINE "
                                + "where DEFINEID = '" + FlowMsglList.DEFINEID + "' ";
                result = dao.ExecuteNonQuery(Sql);
                if (result == 0)
                {
                    dao.Rollback();
                    return false;
                }
                dao.Commit();
                return result > 0 ? true : false;
            }
            catch 
            {
                dao.Rollback();
                return false;
            }
            finally
            {
                
            }
        }
        #endregion
    }
}
