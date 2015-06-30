using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Core;

namespace SMT.Workflow.Platform.DAL
{
    public class DefaultMessageDAL : BaseDAL
    {

        public List<T_WF_DEFAULTMESSAGE> GetDefaultMessageList(int pageSize, int pageIndex, string strFilter, string strOrderBy, ref int pageCount)
        {
            try
            {

                
                int number = pageIndex <= 1 ? 1 : (pageIndex - 1) * pageSize + 1;
                string countSql = @"SELECT count(1)  from T_WF_DEFAULTMESSAGE where (1=1)";
                if (!string.IsNullOrWhiteSpace(strFilter))
                {
                    countSql += strFilter + "";
                }
                string sql = @"SELECT * FROM (SELECT A.*, ROWNUM Page FROM (select * from T_WF_DEFAULTMESSAGE 
                                   order by " + strOrderBy + " ) A WHERE (1=1) AND ROWNUM<= " + pageSize * pageIndex + "";
                if (!string.IsNullOrWhiteSpace(strFilter))
                {
                    sql += strFilter + ")";
                }
                else
                {
                    sql += ")";
                }
                sql += "  WHERE  Page >= " + number + "";
                DataTable dt = dao.GetDataTable(sql);
                pageCount = Convert.ToInt32(dao.ExecuteScalar(countSql));
                pageCount = pageCount / pageSize + (pageCount % pageSize > 0 ? 1 : 0);
                return ToList<T_WF_DEFAULTMESSAGE>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                
            }

        }
        public DataTable GetDataTable(string sql)
        {
            try
            {
                
                DataTable dt = dao.GetDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                
            }

        }

        public void ExecuteMessageSql(string sql, Parameter[] pageparm)
        {
            try
            {
                ParameterCollection pras = new ParameterCollection();
                foreach (var item in pageparm)
                {
                    pras.Add(item);
                }
                dao.ExecuteNonQuery(sql, CommandType.Text, pras);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                
            }
        }
    }
}
