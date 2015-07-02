using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SMT.FlowDAL;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Core;

namespace SMT.FLOWDAL.ADO
{
    /// <summary>
    /// [咨询]
    /// </summary>
    public class FLOW_CONSULTATION_TDAL:BaseFlowDAL
    {
        #region 提莫科技新增
        /// <summary>
        /// 新增[咨询]
        /// </summary>
        /// <param name="con">//OracleConnection</param>
        /// <param name="model">FLOW_CONSULTATION_T</param>
        /// <returns></returns>
        public static int Add(FLOW_CONSULTATION_T model)
        {
            try
            {
                string insSql = "INSERT INTO FLOW_CONSULTATION_T (CONSULTATIONID,FLOWRECORDDETAILID,CONSULTATIONUSERID,CONSULTATIONUSERNAME,CONSULTATIONCONTENT,CONSULTATIONDATE,REPLYUSERID,REPLYUSERNAME,REPLYCONTENT,REPLYDATE,FLAG) VALUES (:CONSULTATIONID,:FLOWRECORDDETAILID,:CONSULTATIONUSERID,:CONSULTATIONUSERNAME,:CONSULTATIONCONTENT,:CONSULTATIONDATE,:REPLYUSERID,:REPLYUSERNAME,:REPLYCONTENT,:REPLYDATE,:FLAG)";
                Parameter[] pageparm =
                {               
                    new Parameter(":CONSULTATIONID",OracleType.NVarChar), 
                    new Parameter(":FLOWRECORDDETAILID",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONUSERID",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONUSERNAME",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONCONTENT",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONDATE",OracleType.DateTime), 
                    new Parameter(":REPLYUSERID",OracleType.NVarChar), 
                    new Parameter(":REPLYUSERNAME",OracleType.NVarChar), 
                    new Parameter(":REPLYCONTENT",OracleType.NVarChar), 
                    new Parameter(":REPLYDATE",OracleType.DateTime), 
                    new Parameter(":FLAG",OracleType.NVarChar) 

                };
                pageparm[0].ParameterValue = GetValue(model.CONSULTATIONID);//
                pageparm[1].ParameterValue = GetValue(model.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID);//
                pageparm[2].ParameterValue = GetValue(model.CONSULTATIONUSERID);//
                pageparm[3].ParameterValue = GetValue(model.CONSULTATIONUSERNAME);//
                pageparm[4].ParameterValue = GetValue(model.CONSULTATIONCONTENT);//
                pageparm[5].ParameterValue = GetValue(model.CONSULTATIONDATE);//
                pageparm[6].ParameterValue = GetValue(model.REPLYUSERID);//
                pageparm[7].ParameterValue = GetValue(model.REPLYUSERNAME);//
                pageparm[8].ParameterValue = GetValue(model.REPLYCONTENT);//
                pageparm[9].ParameterValue = GetValue(model.REPLYDATE);//
                pageparm[10].ParameterValue = GetValue(model.FLAG);//0未回复，1回复

                return dao.ExecuteNonQuery(insSql, pageparm);
            }
            catch (Exception ex)
            {
                throw new Exception("FLOW_CONSULTATION_TDAL_Add:" + ex.Message);
            }
        }
        /// <summary>
        ///  更新: [咨询]
        /// </summary>
        /// <param name="model">FLOW_CONSULTATION_T</param>
        /// <returns></returns>
        public static int Update(FLOW_CONSULTATION_T model)
        {
            try
            {
                model.REPLYDATE = DateTime.Now;
                model.FLAG = "1";
                string updSql = "UPDATE FLOW_CONSULTATION_T SET FLOWRECORDDETAILID=:FLOWRECORDDETAILID,CONSULTATIONUSERID=:CONSULTATIONUSERID,CONSULTATIONUSERNAME=:CONSULTATIONUSERNAME,CONSULTATIONCONTENT=:CONSULTATIONCONTENT,CONSULTATIONDATE=:CONSULTATIONDATE,REPLYUSERID=:REPLYUSERID,REPLYUSERNAME=:REPLYUSERNAME,REPLYCONTENT=:REPLYCONTENT,REPLYDATE=:REPLYDATE,FLAG=:FLAG WHERE   CONSULTATIONID=:CONSULTATIONID";
                Parameter[] pageparm =
                {               
                    new Parameter(":CONSULTATIONID",OracleType.NVarChar), 
                    new Parameter(":FLOWRECORDDETAILID",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONUSERID",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONUSERNAME",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONCONTENT",OracleType.NVarChar), 
                    new Parameter(":CONSULTATIONDATE",OracleType.DateTime), 
                    new Parameter(":REPLYUSERID",OracleType.NVarChar), 
                    new Parameter(":REPLYUSERNAME",OracleType.NVarChar), 
                    new Parameter(":REPLYCONTENT",OracleType.NVarChar), 
                    new Parameter(":REPLYDATE",OracleType.DateTime), 
                    new Parameter(":FLAG",OracleType.NVarChar) 

                };
                pageparm[0].ParameterValue = GetValue(model.CONSULTATIONID);//
                pageparm[1].ParameterValue = GetValue(model.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID);//
                pageparm[2].ParameterValue = GetValue(model.CONSULTATIONUSERID);//
                pageparm[3].ParameterValue = GetValue(model.CONSULTATIONUSERNAME);//
                pageparm[4].ParameterValue = GetValue(model.CONSULTATIONCONTENT);//
                pageparm[5].ParameterValue = GetValue(model.CONSULTATIONDATE);//
                pageparm[6].ParameterValue = GetValue(model.REPLYUSERID);//
                pageparm[7].ParameterValue = GetValue(model.REPLYUSERNAME);//
                pageparm[8].ParameterValue = GetValue(model.REPLYCONTENT);//
                pageparm[9].ParameterValue = GetValue(model.REPLYDATE);//
                pageparm[10].ParameterValue = GetValue(model.FLAG);//0未回复，1回复

                return dao.ExecuteNonQuery(updSql, pageparm);
            }
            catch (Exception ex)
            {
                throw new Exception("FLOW_CONSULTATION_TDAL_Update:" + ex.Message);
            }
        }
        #endregion
//        public static void Add(FLOW_CONSULTATION_T flowConsultation)
//        {
           
//                try
//                {
//                    flowConsultation.CONSULTATIONDATE = DateTime.Now;
//                    string sql = @"insert into FLOW_CONSULTATION_T(CONSULTATIONID,FLOWRECORDDETAILID,CONSULTATIONUSERID,CONSULTATIONCONTENT,
//                                        CONSULTATIONDATE,REPLYUSERID,REPLYCONTENT,REPLYDATE,FLAG,CONSULTATIONUSERNAME,REPLYUSERNAME) 
//                                  values(:pCONSULTATIONID,:pFLOWRECORDDETAILID,:pCONSULTATIONUSERID,:pCONSULTATIONCONTENT,
//                                        :pCONSULTATIONDATE,:pREPLYUSERID,:pREPLYCONTENT,:pREPLYDATE,:pFLAG,:pCONSULTATIONUSERNAME,:pREPLYUSERNAME)";

//                    #region
                   


//                    //OracleCommand cmd = con.CreateCommand();
//                    //cmd.CommandText = sql;

//                    ADOHelper.AddParameter("CONSULTATIONID", flowConsultation.CONSULTATIONID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("FLOWRECORDDETAILID", flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONUSERID", flowConsultation.CONSULTATIONUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONCONTENT", flowConsultation.CONSULTATIONCONTENT, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONDATE", flowConsultation.CONSULTATIONDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYUSERID", flowConsultation.REPLYUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYCONTENT", flowConsultation.REPLYCONTENT, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYDATE", flowConsultation.REPLYDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("FLAG", flowConsultation.FLAG, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONUSERNAME", flowConsultation.CONSULTATIONUSERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYUSERNAME", flowConsultation.REPLYUSERNAME, OracleType.NVarChar, cmd.Parameters);

//                    cmd.ExecuteNonQuery();

                    
//                    #endregion
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("FLOW_CONSULTATION_TDAL_Add:" + ex.Message);
//                }

            
//        }

//        public static void Update(FLOW_CONSULTATION_T flowConsultation)
//        {
//                try
//                {
//                    flowConsultation.REPLYDATE = DateTime.Now;
//                    flowConsultation.FLAG = "1";
//                    //flowConsultation.CONSULTATIONDATE = DateTime.Now;
//                    string sql = @"update FLOW_CONSULTATION_T set FLOWRECORDDETAILID=:pFLOWRECORDDETAILID,CONSULTATIONUSERID=:pCONSULTATIONUSERID,
//                             CONSULTATIONCONTENT=:pCONSULTATIONCONTENT,CONSULTATIONDATE=:pCONSULTATIONDATE,REPLYUSERID=:pREPLYUSERID,
//                             REPLYCONTENT=:pREPLYCONTENT,REPLYDATE=:pREPLYDATE,FLAG=:pFLAG,
//                             CONSULTATIONUSERNAME=:pCONSULTATIONUSERNAME,REPLYUSERNAME=:pREPLYUSERNAME 
//                              where  CONSULTATIONID=:pCONSULTATIONID ";

//                    #region
                   


//                    //OracleCommand cmd = con.CreateCommand();
//                    //cmd.CommandText = sql;


//                    ADOHelper.AddParameter("CONSULTATIONID", flowConsultation.CONSULTATIONID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("FLOWRECORDDETAILID", flowConsultation.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONUSERID", flowConsultation.CONSULTATIONUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONCONTENT", flowConsultation.CONSULTATIONCONTENT, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONDATE", flowConsultation.CONSULTATIONDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYUSERID", flowConsultation.REPLYUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYCONTENT", flowConsultation.REPLYCONTENT, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYDATE", flowConsultation.REPLYDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("FLAG", flowConsultation.FLAG, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONSULTATIONUSERNAME", flowConsultation.CONSULTATIONUSERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("REPLYUSERNAME", flowConsultation.REPLYUSERNAME, OracleType.NVarChar, cmd.Parameters);

//                    cmd.ExecuteNonQuery();

                    
//                    #endregion
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("FLOW_CONSULTATION_TDAL_Update:" + ex.Message);
//                }

            
//        }


    }
}
