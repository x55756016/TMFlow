using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SMT.FlowDAL;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Log;

namespace SMT.FLOWDAL.ADO
{
    /// <summary>
    /// [流程模型定义表]
    /// </summary>
    public class FLOW_FLOWDEFINE_TDAL:BaseFlowDAL
    {
        #region 提莫科技新增
        /// <summary>
        /// 流程模型定义
        /// </summary>
        /// <param name="con"></param>
        /// <param name="CompanyID"></param>
        /// <param name="ModelCode"></param>
        /// <returns></returns>
        public List<FLOW_FLOWDEFINE_T> GetFlowByModelName(string CompanyID, string ModelCode)
        {
            List<FLOW_FLOWDEFINE_T> listDefine = new List<FLOW_FLOWDEFINE_T>();
            IDataReader dr = null;
            try
            {
                #region
                string sqlFLOW_MODELFLOWRELATION_T = @"select * from FLOW_MODELFLOWRELATION_T where COMPANYID='" + CompanyID + "' and FLAG='1'";

                string sqlFLOW_FLOWDEFINE_T = @"select * from FLOW_FLOWDEFINE_T where MODELCODE='" + ModelCode + "'";
                string sql = @"select FLOW_FLOWDEFINE_T.* from ("
                    + sqlFLOW_MODELFLOWRELATION_T
                    + ") FLOW_MODELFLOWRELATION_T join ("
                    + sqlFLOW_FLOWDEFINE_T
                    + ") FLOW_FLOWDEFINE_T on FLOW_MODELFLOWRELATION_T.FLOWCODE=FLOW_FLOWDEFINE_T.FLOWCODE";


                ////OracleCommand cmd = con.CreateCommand();
                ////cmd.CommandText = sql;

                //dr = cmd.ExecuteReader();
                Tracer.Debug("FLOW_FLOWDEFINE_TDAL->GetFlowByModelName SQL语句：" + sql);
                dr = dao.ExecuteReader(sql);
                while (dr.Read())
                {
                    #region FLOW_FLOWDEFINE_T
                    FLOW_FLOWDEFINE_T define = new FLOW_FLOWDEFINE_T();
                    define.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                    define.CREATEDATE = (DateTime)dr["CREATEDATE"];
                    define.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                    define.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                    define.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                    define.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                    define.DESCRIPTION = dr["DESCRIPTION"] == DBNull.Value ? null : dr["DESCRIPTION"].ToString();
                    define.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                    define.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                    define.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                    define.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
                    define.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
                    define.FLOWDEFINEID = dr["FLOWDEFINEID"] == DBNull.Value ? null : dr["FLOWDEFINEID"].ToString();
                    define.LAYOUT = dr["LAYOUT"] == DBNull.Value ? null : dr["LAYOUT"].ToString();
                    define.RULES = dr["RULES"] == DBNull.Value ? null : dr["RULES"].ToString();
                    define.XOML = dr["XOML"] == DBNull.Value ? null : dr["XOML"].ToString(); ;
                    listDefine.Add(define);
                    break;
                    #endregion

                }
                dr.Close();
                #endregion
                return listDefine;
            }

            catch (Exception ex)
            {
                #region
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }

                throw new Exception("GetFlowByModelName-->" + ex.Message);
                #endregion
            }

        }

        #endregion

        public static string GetFlowBranch(string FlowID)
        {
            try
            {
                    string result = "";
                    string sql = "select t.layout from flow_flowdefine_t t where t.flowcode='" + FlowID + "'";
              
                    //OracleCommand cmd = new //OracleCommand(sql, con);
                    IDataReader dr = dao.ExecuteReader(sql);
                    while (dr.Read())
                    {
                        result = dr["layout"] == DBNull.Value ? null : dr["layout"].ToString();

                    }
                    return result;
                

            }
            catch (Exception ex)
            {

                throw new Exception("GetFlowBranch-->" + ex.Message);

            }

        }

        //public static List<FLOW_FLOWDEFINE_T> GetFlowByModelName(string CompanyID, string ModelCode)
        //{
        //        List<FLOW_FLOWDEFINE_T> listDefine = new List<FLOW_FLOWDEFINE_T>();
        //        IDataReader dr = null;
        //        try
        //        {
        //            #region
        //            string sqlFLOW_MODELFLOWRELATION_T = @"select * from FLOW_MODELFLOWRELATION_T where COMPANYID='" + CompanyID + "' and FLAG='1'";

        //            string sqlFLOW_FLOWDEFINE_T = @"select * from FLOW_FLOWDEFINE_T where MODELCODE='" + ModelCode + "'";
        //            string sql = @"select FLOW_FLOWDEFINE_T.* from ("
        //                + sqlFLOW_MODELFLOWRELATION_T
        //                + ") FLOW_MODELFLOWRELATION_T join ("
        //                + sqlFLOW_FLOWDEFINE_T
        //                + ") FLOW_FLOWDEFINE_T on FLOW_MODELFLOWRELATION_T.FLOWCODE=FLOW_FLOWDEFINE_T.FLOWCODE";

                   


        //            //OracleCommand cmd = con.CreateCommand();
        //            //cmd.CommandText = sql;

        //            dr = cmd.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                #region FLOW_FLOWDEFINE_T
        //                FLOW_FLOWDEFINE_T define = new FLOW_FLOWDEFINE_T();
        //                define.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
        //                define.CREATEDATE = (DateTime)dr["CREATEDATE"];
        //                define.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
        //                define.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
        //                define.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
        //                define.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
        //                define.DESCRIPTION = dr["DESCRIPTION"] == DBNull.Value ? null : dr["DESCRIPTION"].ToString();
        //                define.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
        //                define.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
        //                define.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
        //                define.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
        //                define.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
        //                define.FLOWDEFINEID = dr["FLOWDEFINEID"] == DBNull.Value ? null : dr["FLOWDEFINEID"].ToString();
        //                define.LAYOUT = dr["LAYOUT"] == DBNull.Value ? null : dr["LAYOUT"].ToString();
        //                define.RULES = dr["RULES"] == DBNull.Value ? null : dr["RULES"].ToString();
        //                define.XOML = dr["XOML"] == DBNull.Value ? null : dr["XOML"].ToString(); ;
        //                listDefine.Add(define);
        //                break;
        //                #endregion

        //            }
        //            dr.Close();

                    
        //            #endregion
        //            return listDefine;
        //        }

        //        catch (Exception ex)
        //        {
        //            #region
        //            if (dr != null && !dr.IsClosed)
        //            {
        //                dr.Close();
        //            }
        //            if (con.State == ConnectionState.Open)
        //            {
                        
        //            }
        //            throw new Exception("GetFlowByModelName-->" + ex.Message);
        //            #endregion
        //        }
            
        //}


    }
}
