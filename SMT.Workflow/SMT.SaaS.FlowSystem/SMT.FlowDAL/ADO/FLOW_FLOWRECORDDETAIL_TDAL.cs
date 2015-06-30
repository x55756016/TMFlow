using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Data.Objects.DataClasses;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Log;
using SMT.FlowDAL;
using SMT.Foundation.Core;

namespace SMT.FLOWDAL.ADO
{
    public class FLOW_FLOWRECORDDETAIL_TDAL:BaseFlowDAL
    {
        #region 向寒咏新增
        /// <summary>
        /// 新增[流程审批明细表]
        /// </summary>
        /// <param name="con">//OracleConnection连接对象</param>
        /// <param name="detail">流程审批明细表</param>
        public static int Add(FLOW_FLOWRECORDDETAIL_T model)
        {
            try
            {
                #region
                string insSql = "INSERT INTO FLOW_FLOWRECORDDETAIL_T (FLOWRECORDDETAILID,FLOWRECORDMASTERID,STATECODE,PARENTSTATEID,CONTENT,CHECKSTATE,FLAG,CREATEUSERID,CREATEUSERNAME,CREATECOMPANYID,CREATEDEPARTMENTID,CREATEPOSTID,CREATEDATE,EDITUSERID,EDITUSERNAME,EDITCOMPANYID,EDITDEPARTMENTID,EDITPOSTID,EDITDATE,AGENTUSERID,AGENTERNAME,AGENTEDITDATE) VALUES (:FLOWRECORDDETAILID,:FLOWRECORDMASTERID,:STATECODE,:PARENTSTATEID,:CONTENT,:CHECKSTATE,:FLAG,:CREATEUSERID,:CREATEUSERNAME,:CREATECOMPANYID,:CREATEDEPARTMENTID,:CREATEPOSTID,:CREATEDATE,:EDITUSERID,:EDITUSERNAME,:EDITCOMPANYID,:EDITDEPARTMENTID,:EDITPOSTID,:EDITDATE,:AGENTUSERID,:AGENTERNAME,:AGENTEDITDATE)";
                Parameter[] pageparm =
                {               
                    new Parameter(":FLOWRECORDDETAILID",OracleType.NVarChar), 
                    new Parameter(":FLOWRECORDMASTERID",OracleType.NVarChar), 
                    new Parameter(":STATECODE",OracleType.NVarChar), 
                    new Parameter(":PARENTSTATEID",OracleType.NVarChar), 
                    new Parameter(":CONTENT",OracleType.NVarChar), 
                    new Parameter(":CHECKSTATE",OracleType.NVarChar), 
                    new Parameter(":FLAG",OracleType.NVarChar), 
                    new Parameter(":CREATEUSERID",OracleType.NVarChar), 
                    new Parameter(":CREATEUSERNAME",OracleType.NVarChar), 
                    new Parameter(":CREATECOMPANYID",OracleType.NVarChar), 
                    new Parameter(":CREATEDEPARTMENTID",OracleType.NVarChar), 
                    new Parameter(":CREATEPOSTID",OracleType.NVarChar), 
                    new Parameter(":CREATEDATE",OracleType.DateTime), 
                    new Parameter(":EDITUSERID",OracleType.NVarChar), 
                    new Parameter(":EDITUSERNAME",OracleType.NVarChar), 
                    new Parameter(":EDITCOMPANYID",OracleType.NVarChar), 
                    new Parameter(":EDITDEPARTMENTID",OracleType.NVarChar), 
                    new Parameter(":EDITPOSTID",OracleType.NVarChar), 
                    new Parameter(":EDITDATE",OracleType.DateTime), 
                    new Parameter(":AGENTUSERID",OracleType.NVarChar), 
                    new Parameter(":AGENTERNAME",OracleType.NVarChar), 
                    new Parameter(":AGENTEDITDATE",OracleType.DateTime) 

                };
                pageparm[0].ParameterValue = GetValue(model.FLOWRECORDDETAILID);//
                pageparm[1].ParameterValue = GetValue( model.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);//
                pageparm[2].ParameterValue = GetValue( model.STATECODE );//
                pageparm[3].ParameterValue = GetValue( model.PARENTSTATEID );//
                pageparm[4].ParameterValue =GetValue(  model.CONTENT);//
                pageparm[5].ParameterValue =GetValue(  model.CHECKSTATE );//同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8
                pageparm[6].ParameterValue = GetValue( model.FLAG );//已审批：1，未审批:0
                pageparm[7].ParameterValue = GetValue( model.CREATEUSERID );//
                pageparm[8].ParameterValue = GetValue( model.CREATEUSERNAME);//
                pageparm[9].ParameterValue = GetValue( model.CREATECOMPANYID);//
                pageparm[10].ParameterValue =GetValue(  model.CREATEDEPARTMENTID );//
                pageparm[11].ParameterValue =GetValue(  model.CREATEPOSTID);//
                pageparm[12].ParameterValue =GetValue(  model.CREATEDATE);//
                pageparm[13].ParameterValue =GetValue(  model.EDITUSERID);//
                pageparm[14].ParameterValue =GetValue(  model.EDITUSERNAME );//
                pageparm[15].ParameterValue =GetValue(  model.EDITCOMPANYID);//
                pageparm[16].ParameterValue =GetValue(  model.EDITDEPARTMENTID);//
                pageparm[17].ParameterValue =GetValue(  model.EDITPOSTID);//
                pageparm[18].ParameterValue =GetValue(  model.EDITDATE);//
                pageparm[19].ParameterValue =GetValue(  model.AGENTUSERID);//
                pageparm[20].ParameterValue = GetValue( model.AGENTERNAME );//
                pageparm[21].ParameterValue =GetValue(  model.AGENTEDITDATE);//

                int n= dao.ExecuteNonQuery(insSql, pageparm);
                Tracer.Debug("FLOW_FLOWRECORDDETAIL_TDAL->Add新增[流程审批明细表]成功：FLOWRECORDDETAILID＝" + model.FLOWRECORDDETAILID + ";FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=" + model.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + ";时间：" + DateTime.Now.ToString());
                return n;

                #endregion
            }
            catch (Exception ex)
            {
                Tracer.Debug("FLOW_FLOWRECORDDETAIL_TDAL->Add新增[流程审批明细表]失败：FLOWRECORDDETAILID＝" + model.FLOWRECORDDETAILID + ";FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=" + model.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + ";时间：" + DateTime.Now.ToString() + "\r\n异常信息：" + ex.Message);
                throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->Add:" + ex.Message);
            }
        }
        /// <summary>
        /// 更新[流程审批明细表]
        /// </summary>
        /// <param name="con">//OracleConnection连接对象</param>
        /// <param name="detail">流程审批明细表</param>
        public static int Update( FLOW_FLOWRECORDDETAIL_T model)
        {
            try
            {
                string updSql = "UPDATE FLOW_FLOWRECORDDETAIL_T SET FLOWRECORDMASTERID=:FLOWRECORDMASTERID,STATECODE=:STATECODE,PARENTSTATEID=:PARENTSTATEID,CONTENT=:CONTENT,CHECKSTATE=:CHECKSTATE,FLAG=:FLAG,CREATEUSERID=:CREATEUSERID,CREATEUSERNAME=:CREATEUSERNAME,CREATECOMPANYID=:CREATECOMPANYID,CREATEDEPARTMENTID=:CREATEDEPARTMENTID,CREATEPOSTID=:CREATEPOSTID,CREATEDATE=:CREATEDATE,EDITUSERID=:EDITUSERID,EDITUSERNAME=:EDITUSERNAME,EDITCOMPANYID=:EDITCOMPANYID,EDITDEPARTMENTID=:EDITDEPARTMENTID,EDITPOSTID=:EDITPOSTID,EDITDATE=:EDITDATE,AGENTUSERID=:AGENTUSERID,AGENTERNAME=:AGENTERNAME,AGENTEDITDATE=:AGENTEDITDATE WHERE   FLOWRECORDDETAILID=:FLOWRECORDDETAILID";
                Parameter[] pageparm =
                {               
                    new Parameter(":FLOWRECORDDETAILID",OracleType.NVarChar), 
                    new Parameter(":FLOWRECORDMASTERID",OracleType.NVarChar), 
                    new Parameter(":STATECODE",OracleType.NVarChar), 
                    new Parameter(":PARENTSTATEID",OracleType.NVarChar), 
                    new Parameter(":CONTENT",OracleType.NVarChar), 
                    new Parameter(":CHECKSTATE",OracleType.NVarChar), 
                    new Parameter(":FLAG",OracleType.NVarChar), 
                    new Parameter(":CREATEUSERID",OracleType.NVarChar), 
                    new Parameter(":CREATEUSERNAME",OracleType.NVarChar), 
                    new Parameter(":CREATECOMPANYID",OracleType.NVarChar), 
                    new Parameter(":CREATEDEPARTMENTID",OracleType.NVarChar), 
                    new Parameter(":CREATEPOSTID",OracleType.NVarChar), 
                    new Parameter(":CREATEDATE",OracleType.DateTime), 
                    new Parameter(":EDITUSERID",OracleType.NVarChar), 
                    new Parameter(":EDITUSERNAME",OracleType.NVarChar), 
                    new Parameter(":EDITCOMPANYID",OracleType.NVarChar), 
                    new Parameter(":EDITDEPARTMENTID",OracleType.NVarChar), 
                    new Parameter(":EDITPOSTID",OracleType.NVarChar), 
                    new Parameter(":EDITDATE",OracleType.DateTime), 
                    new Parameter(":AGENTUSERID",OracleType.NVarChar), 
                    new Parameter(":AGENTERNAME",OracleType.NVarChar), 
                    new Parameter(":AGENTEDITDATE",OracleType.DateTime) 

                };
                pageparm[0].ParameterValue = GetValue( model.FLOWRECORDDETAILID);//
                pageparm[1].ParameterValue = GetValue( model.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID );//
                pageparm[2].ParameterValue =GetValue(  model.STATECODE);//
                pageparm[3].ParameterValue = GetValue( model.PARENTSTATEID );//
                pageparm[4].ParameterValue =GetValue(  model.CONTENT);//
                pageparm[5].ParameterValue = GetValue( model.CHECKSTATE);//同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8
                pageparm[6].ParameterValue =GetValue(  model.FLAG);//已审批：1，未审批:0
                pageparm[7].ParameterValue = GetValue( model.CREATEUSERID );//
                pageparm[8].ParameterValue = GetValue( model.CREATEUSERNAME );//
                pageparm[9].ParameterValue =GetValue(  model.CREATECOMPANYID);//
                pageparm[10].ParameterValue =GetValue(  model.CREATEDEPARTMENTID );//
                pageparm[11].ParameterValue =GetValue(  model.CREATEPOSTID);//
                pageparm[12].ParameterValue = GetValue( model.CREATEDATE);//
                pageparm[13].ParameterValue =GetValue(  model.EDITUSERID);//
                pageparm[14].ParameterValue =GetValue(  model.EDITUSERNAME );//
                pageparm[15].ParameterValue =GetValue(  model.EDITCOMPANYID);//
                pageparm[16].ParameterValue =GetValue(  model.EDITDEPARTMENTID);//
                pageparm[17].ParameterValue =GetValue(  model.EDITPOSTID);//
                pageparm[18].ParameterValue = GetValue( model.EDITDATE);//
                pageparm[19].ParameterValue =GetValue(  model.AGENTUSERID );//
                pageparm[20].ParameterValue =GetValue(  model.AGENTERNAME);//
                pageparm[21].ParameterValue = GetValue(model.AGENTEDITDATE);//

                int n= dao.ExecuteNonQuery(updSql, pageparm);
                Tracer.Debug("FLOW_FLOWRECORDDETAIL_TDAL->Update:更新[流程审批明细表]成功：FLOWRECORDDETAILID＝" + model.FLOWRECORDDETAILID + ";FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=" + model.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + ";时间：" + DateTime.Now.ToString());
                return n;
            }
            catch (Exception ex)
            {
                Tracer.Debug("FLOW_FLOWRECORDDETAIL_TDAL->Update:更新[流程审批明细表]失败：FLOWRECORDDETAILID＝" + model.FLOWRECORDDETAILID + ";FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=" + model.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + ";时间：" + DateTime.Now.ToString() + "\r\n异常信息：" + ex.Message);

                throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->Update:" + ex.Message);
            }
        }
        /// <summary>
        /// 删除[流程审批明细表]
        /// </summary>
        /// <param name="con">//OracleConnection连接对象</param>
        /// <param name="detail">流程审批明细表</param>
        public static int Delete( FLOW_FLOWRECORDDETAIL_T detail)
        {
            try
            {
                string delSql = "DELETE FROM FLOW_FLOWRECORDDETAIL_T  WHERE   FLOWRECORDDETAILID=:FLOWRECORDDETAILID";
                Parameter[] pageparm =
                {               
                    new Parameter(":FLOWRECORDDETAILID",OracleType.NVarChar) 

                };
                pageparm[0].ParameterValue = detail.FLOWRECORDDETAILID;
                return dao.ExecuteNonQuery(delSql, pageparm);
            }
            catch (Exception ex)
            {
                Tracer.Debug("FLOW_FLOWRECORDDETAIL_TDAL->Delete:FLOWRECORDDETAILID＝" + detail.FLOWRECORDDETAILID +";时间：" + DateTime.Now.ToString() + "\r\n异常信息：" + ex.Message);

                throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->Delete:" + ex.Message);
            }
        }
        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="con">//OracleConnection连接对象</param>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>
        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
        /// <returns></returns>
        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowRecord(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<string> FlowType)
        {
            #region sql

            StringBuilder sb = new StringBuilder();
            sb.Append(@"select FLOW_FLOWRECORDDETAIL_T.* from FLOW_FLOWRECORDDETAIL_T
join FLOW_FLOWRECORDMASTER_T
on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID
where 1=1 ");

            if (!string.IsNullOrEmpty(FlowGUID))
            {
                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");

            }
            if (!string.IsNullOrEmpty(Flag))
            {
                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
            }

            if (!string.IsNullOrEmpty(EditUserID))
            {
                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
            }


            if (!string.IsNullOrEmpty(CompanyID))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
            }
            if (!string.IsNullOrEmpty(ModelCode))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
            }
            if (!string.IsNullOrEmpty(FormID))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
            }
            if (!string.IsNullOrEmpty(CheckState))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
            }

            if (FlowType != null)
            {
                if (FlowType.Count == 1)
                {
                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
                }
                else
                {
                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
                    for (int i = 0; i < FlowType.Count - 1; i++)
                    {
                        sb.Append("'" + FlowType[i] + "',");
                    }
                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
                    sb.Append(")");
                }
            }


            #endregion
            List<FLOW_FLOWRECORDDETAIL_T> listDetail = new List<FLOW_FLOWRECORDDETAIL_T>();
            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();
           
                IDataReader dr = null;
                try
                {
                    List<string> listMasterID = new List<string>();                  
                    ////OracleCommand command = con.CreateCommand();
                    //command.CommandText = sb.ToString();
                    //dr = command.ExecuteReader();
                    dr = dao.ExecuteReader(sb.ToString());
                    while (dr.Read())
                    {
                        #region detail
                        FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
                        detail.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();
                        detail.AGENTEDITDATE = dr["AGENTEDITDATE"] == DBNull.Value ? null : (DateTime?)dr["AGENTEDITDATE"];
                        detail.AGENTERNAME = dr["AGENTERNAME"] == DBNull.Value ? null : dr["AGENTERNAME"].ToString();
                        detail.AGENTUSERID = dr["AGENTUSERID"] == DBNull.Value ? null : dr["AGENTUSERID"].ToString();
                        detail.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
                        detail.CONTENT = dr["CONTENT"] == DBNull.Value ? null : dr["CONTENT"].ToString();
                        detail.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                        detail.CREATEDATE = (DateTime)dr["CREATEDATE"];
                        detail.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                        detail.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
                        detail.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                        detail.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                        detail.EDITCOMPANYID = dr["EDITCOMPANYID"] == DBNull.Value ? null : dr["EDITCOMPANYID"].ToString();
                        detail.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                        detail.EDITDEPARTMENTID = dr["EDITDEPARTMENTID"] == DBNull.Value ? null : dr["EDITDEPARTMENTID"].ToString();
                        detail.EDITPOSTID = dr["EDITPOSTID"] == DBNull.Value ? null : dr["EDITPOSTID"].ToString();
                        detail.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                        detail.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                        detail.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
                        detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                        detail.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"] == DBNull.Value ? null : dr["FLOWRECORDDETAILID"].ToString();
                        detail.PARENTSTATEID = dr["PARENTSTATEID"] == DBNull.Value ? null : dr["PARENTSTATEID"].ToString();
                        detail.STATECODE = dr["STATECODE"] == DBNull.Value ? null : dr["STATECODE"].ToString();
                        listDetail.Add(detail);
                        if (!listMasterID.Contains(detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID))
                        {
                            listMasterID.Add("'" + detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "'");
                        }
                        #endregion
                    }
                    dr.Close();

                    #region master
                    if (listMasterID.Count > 0)
                    {
                        //command.CommandText = "select * from FLOW_FLOWRECORDMASTER_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
                        //dr = command.ExecuteReader();
                        string sql = "select * from FLOW_FLOWRECORDMASTER_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
                        dr=dao.ExecuteReader(sql);
                        while (dr.Read())
                        {
                            #region master
                            FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();
                            master.ACTIVEROLE = dr["ACTIVEROLE"] == DBNull.Value ? null : dr["ACTIVEROLE"].ToString();
                            master.BUSINESSOBJECT = dr["BUSINESSOBJECT"] == DBNull.Value ? null : dr["BUSINESSOBJECT"].ToString();
                            master.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
                            master.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                            master.CREATEDATE = (DateTime)dr["CREATEDATE"];
                            master.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                            master.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
                            master.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                            master.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                            master.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                            master.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                            master.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                            master.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
                            master.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                            master.FLOWSELECTTYPE = dr["FLOWSELECTTYPE"] == DBNull.Value ? null : dr["FLOWSELECTTYPE"].ToString();
                            master.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
                            master.FORMID = dr["FORMID"] == DBNull.Value ? null : dr["FORMID"].ToString();
                            master.INSTANCEID = dr["INSTANCEID"] == DBNull.Value ? null : dr["INSTANCEID"].ToString();
                            master.KPITIMEXML = dr["KPITIMEXML"] == DBNull.Value ? null : dr["KPITIMEXML"].ToString();
                            master.MODELCODE = dr["MODELCODE"] == DBNull.Value ? null : dr["MODELCODE"].ToString();
                            master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
                            listMaster.Add(master);
                            #endregion

                        }
                        dr.Close();
                    }
                    #endregion
                
                    listDetail.ForEach(detail =>
                    {
                        FLOW_FLOWRECORDMASTER_T master = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);

                        detail.FLOW_FLOWRECORDMASTER_T = master;
                        master.FLOW_FLOWRECORDDETAIL_T.Add(detail);
                    });

                    return listDetail;

                }
                catch (Exception ex)
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->GetFlowRecord:-" + FormID + "-" + ex.Message + ex.InnerException);
                }

        }
        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="con">//OracleConnection连接对象</param>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>
        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
        /// <returns></returns>      
        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowRecordV(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<string> FlowType)
        {
            #region sql

            StringBuilder sb = new StringBuilder();
            sb.Append(@"select FLOW_FLOWRECORDDETAIL_T.* from FLOW_FLOWRECORDDETAIL_T
join FLOW_FLOWRECORDMASTER_T
on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID
where 1=1 ");

            if (!string.IsNullOrEmpty(FlowGUID))
            {
                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");

            }
            if (!string.IsNullOrEmpty(Flag))
            {
                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
            }

            if (!string.IsNullOrEmpty(EditUserID))
            {
                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
            }


            if (!string.IsNullOrEmpty(CompanyID))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
            }
            if (!string.IsNullOrEmpty(ModelCode))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
            }
            if (!string.IsNullOrEmpty(FormID))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
            }
            if (!string.IsNullOrEmpty(CheckState))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
            }

            if (FlowType != null)
            {
                if (FlowType.Count == 1)
                {
                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
                }
                else
                {
                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
                    for (int i = 0; i < FlowType.Count - 1; i++)
                    {
                        sb.Append("'" + FlowType[i] + "',");
                    }
                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
                    sb.Append(")");
                }
            }


            #endregion
            List<FLOW_FLOWRECORDDETAIL_T> listDetail = new List<FLOW_FLOWRECORDDETAIL_T>();
            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();
            
                IDataReader dr = null;
                try
                {
                    List<string> listDetailID = new List<string>();
                    List<string> listMasterID = new List<string>();
               
                    ////OracleCommand command = con.CreateCommand();
                    //command.CommandText = sb.ToString();
                    //dr = command.ExecuteReader();
                    dr = dao.ExecuteReader(sb.ToString(), null);
                    while (dr.Read())
                    {
                        #region detail
                        FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
                        detail.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();
                        detail.AGENTEDITDATE = dr["AGENTEDITDATE"] == DBNull.Value ? null : (DateTime?)dr["AGENTEDITDATE"];
                        detail.AGENTERNAME = dr["AGENTERNAME"] == DBNull.Value ? null : dr["AGENTERNAME"].ToString();
                        detail.AGENTUSERID = dr["AGENTUSERID"] == DBNull.Value ? null : dr["AGENTUSERID"].ToString();
                        detail.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
                        detail.CONTENT = dr["CONTENT"] == DBNull.Value ? null : dr["CONTENT"].ToString();
                        detail.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                        detail.CREATEDATE = (DateTime)dr["CREATEDATE"];
                        detail.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                        detail.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
                        detail.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                        detail.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                        detail.EDITCOMPANYID = dr["EDITCOMPANYID"] == DBNull.Value ? null : dr["EDITCOMPANYID"].ToString();
                        detail.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                        detail.EDITDEPARTMENTID = dr["EDITDEPARTMENTID"] == DBNull.Value ? null : dr["EDITDEPARTMENTID"].ToString();
                        detail.EDITPOSTID = dr["EDITPOSTID"] == DBNull.Value ? null : dr["EDITPOSTID"].ToString();
                        detail.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                        detail.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                        detail.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
                        detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                        detail.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"] == DBNull.Value ? null : dr["FLOWRECORDDETAILID"].ToString();
                        detail.PARENTSTATEID = dr["PARENTSTATEID"] == DBNull.Value ? null : dr["PARENTSTATEID"].ToString();
                        detail.STATECODE = dr["STATECODE"] == DBNull.Value ? null : dr["STATECODE"].ToString();
                        detail.FLOW_CONSULTATION_T = new EntityCollection<FLOW_CONSULTATION_T>();
                        listDetail.Add(detail);
                        listDetailID.Add("'" + detail.FLOWRECORDDETAILID + "'");
                        if (!listMasterID.Contains(detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID))
                        {
                            listMasterID.Add("'" + detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "'");
                        }


                        #endregion
                    }
                    dr.Close();

                    #region master
                    if (listMasterID.Count > 0)
                    {
                        string sql= @"select CHECKSTATE,CREATECOMPANYID,CREATEDATE,CREATEDEPARTMENTID,CREATEPOSTID,CREATEUSERID,CREATEUSERNAME,
                                               EDITDATE,EDITUSERID,EDITUSERNAME,FLOWCODE,FLOWRECORDMASTERID,FLOWSELECTTYPE,FLOWTYPE,FORMID,INSTANCEID,MODELCODE
                                              from FLOW_FLOWRECORDMASTER_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
                     
                        dr = dao.ExecuteReader(sql);
                        while (dr.Read())
                        {
                            #region master
                            FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();

                            //master.ACTIVEROLE = dr["ACTIVEROLE"] == DBNull.Value ? null : dr["ACTIVEROLE"].ToString();
                            //master.BUSINESSOBJECT = dr["BUSINESSOBJECT"] == DBNull.Value ? null : dr["BUSINESSOBJECT"].ToString();
                            master.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
                            master.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                            master.CREATEDATE = (DateTime)dr["CREATEDATE"];
                            master.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                            master.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
                            master.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                            master.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                            master.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                            master.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                            master.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                            master.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
                            master.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                            master.FLOWSELECTTYPE = dr["FLOWSELECTTYPE"] == DBNull.Value ? null : dr["FLOWSELECTTYPE"].ToString();
                            master.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
                            master.FORMID = dr["FORMID"] == DBNull.Value ? null : dr["FORMID"].ToString();
                            master.INSTANCEID = dr["INSTANCEID"] == DBNull.Value ? null : dr["INSTANCEID"].ToString();
                            //master.KPITIMEXML = dr["KPITIMEXML"] == DBNull.Value ? null : dr["KPITIMEXML"].ToString();
                            master.MODELCODE = dr["MODELCODE"] == DBNull.Value ? null : dr["MODELCODE"].ToString();
                            master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
                            listMaster.Add(master);
                            #endregion

                        }
                        dr.Close();

                    }
                    #endregion
                    #region FLOW_CONSULTATION_T

                    List<FLOW_CONSULTATION_T> listConsultation = new List<FLOW_CONSULTATION_T>();
                    if (!string.IsNullOrEmpty(FormID) && !string.IsNullOrEmpty(ModelCode))
                    {
                        if (listDetailID.Count > 0)
                        {

                            string sql = @"select * from FLOW_CONSULTATION_T where FLOWRECORDDETAILID in (" + string.Join(",", listDetailID.ToArray()) + ")";
                      
                            dr = dao.ExecuteReader(sql);
                            while (dr.Read())
                            {
                                #region FLOW_CONSULTATION_T
                                FLOW_CONSULTATION_T consul = new FLOW_CONSULTATION_T();
                                consul.CONSULTATIONCONTENT = dr["CONSULTATIONCONTENT"] == DBNull.Value ? null : dr["CONSULTATIONCONTENT"].ToString();
                                consul.CONSULTATIONDATE = dr["CONSULTATIONDATE"] == DBNull.Value ? null : (DateTime?)dr["CONSULTATIONDATE"];
                                consul.CONSULTATIONID = dr["CONSULTATIONID"] == DBNull.Value ? null : dr["CONSULTATIONID"].ToString();
                                consul.CONSULTATIONUSERID = dr["CONSULTATIONUSERID"] == DBNull.Value ? null : dr["CONSULTATIONUSERID"].ToString();
                                consul.CONSULTATIONUSERNAME = dr["CONSULTATIONUSERNAME"] == DBNull.Value ? null : dr["CONSULTATIONUSERNAME"].ToString();
                                consul.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
                                consul.FLOW_FLOWRECORDDETAIL_T = new FLOW_FLOWRECORDDETAIL_T();
                                consul.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"].ToString();
                                consul.REPLYCONTENT = dr["REPLYCONTENT"] == DBNull.Value ? null : dr["REPLYCONTENT"].ToString();
                                consul.REPLYDATE = dr["REPLYDATE"] == DBNull.Value ? null : (DateTime?)dr["REPLYDATE"];
                                consul.REPLYUSERID = dr["REPLYUSERID"] == DBNull.Value ? null : dr["REPLYUSERID"].ToString();
                                consul.REPLYUSERNAME = dr["REPLYUSERNAME"] == DBNull.Value ? null : dr["REPLYUSERNAME"].ToString();

                                listConsultation.Add(consul);

                                #endregion

                            }
                            dr.Close();

                        }
                    }
                    #endregion
              
                    listDetail.ForEach(detail =>
                    {
                        #region
                        FLOW_FLOWRECORDMASTER_T master = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);

                        detail.FLOW_FLOWRECORDMASTER_T = master;
                        if (master.FLOW_FLOWRECORDDETAIL_T == null)
                        {
                            master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
                        }
                        master.FLOW_FLOWRECORDDETAIL_T.Add(detail);

                        if (!string.IsNullOrEmpty(FormID) && !string.IsNullOrEmpty(ModelCode))
                        {
                            IEnumerable<FLOW_CONSULTATION_T> iFLOW_CONSULTATION_T = listConsultation.Where(c => c.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID == detail.FLOWRECORDDETAILID);
                            if (iFLOW_CONSULTATION_T.Count() > 0)
                            {
                                detail.FLOW_CONSULTATION_T = new EntityCollection<FLOW_CONSULTATION_T>();
                                foreach (var consultation in iFLOW_CONSULTATION_T)
                                {
                                    consultation.FLOW_FLOWRECORDDETAIL_T = detail;
                                    detail.FLOW_CONSULTATION_T.Add(consultation);
                                }
                            }
                        }
                        #endregion
                    });
                    return listDetail;

                }
                catch (Exception ex)
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->GetFlowRecordV:-" + FormID + "-" + ex.Message + ex.InnerException);
                }

    



        }
        /// <summary>
        /// 获取流程(只返回前100条detail)
        /// </summary>
        /// <param name="con">//OracleConnection连接对象</param>
        /// <param name="FormID">表单ID</param>
        /// <param name="FlowGUID">明细ID</param>
        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="CompanyID">创建公司ID</param>
        /// <param name="EditUserID">操作人</param>
        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
        /// <returns></returns>      
        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowRecordTop(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<string> FlowType)
        {
            #region 旧的sql

//            StringBuilder sb = new StringBuilder();
//            sb.Append(@"select FLOW_FLOWRECORDMASTER_T.CHECKSTATE,FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID,FLOW_FLOWRECORDMASTER_T.CREATEDATE,
//                           FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID,FLOW_FLOWRECORDMASTER_T.CREATEPOSTID,FLOW_FLOWRECORDMASTER_T.CREATEUSERID,
//                           FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME,FLOW_FLOWRECORDMASTER_T.EDITDATE,FLOW_FLOWRECORDMASTER_T.EDITUSERID,
//                           FLOW_FLOWRECORDMASTER_T.EDITUSERNAME,FLOW_FLOWRECORDMASTER_T.FLOWCODE,FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID,
//                           FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE,FLOW_FLOWRECORDMASTER_T.FLOWTYPE,FLOW_FLOWRECORDMASTER_T.FORMID,
//                       FLOW_FLOWRECORDMASTER_T.INSTANCEID,FLOW_FLOWRECORDMASTER_T.MODELCODE 
//                    from FLOW_FLOWRECORDMASTER_T");
//            sb.Append(" where FLOWRECORDMASTERID in (");
//            sb.Append(@" select distinct FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID from FLOW_FLOWRECORDMASTER_T 
//                            join FLOW_FLOWRECORDDETAIL_T
//                            on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID where 1=1");

//            #region detail 条件
//            if (!string.IsNullOrEmpty(FlowGUID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");

//            }
//            if (!string.IsNullOrEmpty(Flag))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
//            }

//            if (!string.IsNullOrEmpty(EditUserID))
//            {
//                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
//            }
//            #endregion

//            #region master 条件

//            if (!string.IsNullOrEmpty(CompanyID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
//            }
//            if (!string.IsNullOrEmpty(ModelCode))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
//            }
//            if (!string.IsNullOrEmpty(FormID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
//            }
//            if (!string.IsNullOrEmpty(CheckState))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
//            }

//            if (FlowType != null)
//            {
//                if (FlowType.Count == 1)
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
//                }
//                else
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
//                    for (int i = 0; i < FlowType.Count - 1; i++)
//                    {
//                        sb.Append("'" + FlowType[i] + "',");
//                    }
//                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
//                    sb.Append(")");
//                }
//            }
//            #endregion

//            sb.Append(")");

//            sb.Append(" and rownum <=20");
//            sb.Append(" order by FLOW_FLOWRECORDMASTER_T.CREATEDATE desc");
            #endregion
            #region 新的sql（根据结果排序）

            StringBuilder sb = new StringBuilder();
            sb.Append(@"select * from ( select FLOW_FLOWRECORDMASTER_T.CHECKSTATE,FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID,FLOW_FLOWRECORDMASTER_T.CREATEDATE,
                           FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID,FLOW_FLOWRECORDMASTER_T.CREATEPOSTID,FLOW_FLOWRECORDMASTER_T.CREATEUSERID,
                           FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME,FLOW_FLOWRECORDMASTER_T.EDITDATE,FLOW_FLOWRECORDMASTER_T.EDITUSERID,
                           FLOW_FLOWRECORDMASTER_T.EDITUSERNAME,FLOW_FLOWRECORDMASTER_T.FLOWCODE,FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID,
                           FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE,FLOW_FLOWRECORDMASTER_T.FLOWTYPE,FLOW_FLOWRECORDMASTER_T.FORMID,
                       FLOW_FLOWRECORDMASTER_T.INSTANCEID,FLOW_FLOWRECORDMASTER_T.MODELCODE 
                    from FLOW_FLOWRECORDMASTER_T");
            sb.Append(" where FLOWRECORDMASTERID in (");
            sb.Append(@" select distinct FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID from FLOW_FLOWRECORDMASTER_T 
                            join FLOW_FLOWRECORDDETAIL_T
                            on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID where 1=1");

            #region detail 条件
            if (!string.IsNullOrEmpty(FlowGUID))
            {
                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");

            }
            if (!string.IsNullOrEmpty(Flag))
            {
                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
            }

            if (!string.IsNullOrEmpty(EditUserID))
            {
                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
            }
            #endregion

            #region master 条件

            if (!string.IsNullOrEmpty(CompanyID))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
            }
            if (!string.IsNullOrEmpty(ModelCode))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
            }
            if (!string.IsNullOrEmpty(FormID))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
            }
            if (!string.IsNullOrEmpty(CheckState))
            {
                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
            }

            if (FlowType != null)
            {
                if (FlowType.Count == 1)
                {
                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
                }
                else
                {
                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
                    for (int i = 0; i < FlowType.Count - 1; i++)
                    {
                        sb.Append("'" + FlowType[i] + "',");
                    }
                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
                    sb.Append(")");
                }
            }
            #endregion

            sb.Append(")");

            sb.Append(" and rownum <=20)");
            sb.Append(" order by CREATEDATE desc");
            #endregion
            List<FLOW_FLOWRECORDDETAIL_T> listDetail = new List<FLOW_FLOWRECORDDETAIL_T>();
            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();          
                IDataReader dr = null;
                try
                {

                    List<string> listMasterID = new List<string>();
                    ////OracleCommand command = con.CreateCommand();
                    //command.CommandText = sb.ToString();
                    //dr = command.ExecuteReader();
                    dr = dao.ExecuteReader(sb.ToString(), null);
                    while (dr.Read())
                    {
                        #region master
                        FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();

                        //master.ACTIVEROLE = dr["ACTIVEROLE"] == DBNull.Value ? null : dr["ACTIVEROLE"].ToString();
                        //master.BUSINESSOBJECT = dr["BUSINESSOBJECT"] == DBNull.Value ? null : dr["BUSINESSOBJECT"].ToString();
                        master.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
                        master.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                        master.CREATEDATE = (DateTime)dr["CREATEDATE"];
                        master.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                        master.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
                        master.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                        master.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                        master.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                        master.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                        master.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                        master.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
                        master.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                        master.FLOWSELECTTYPE = dr["FLOWSELECTTYPE"] == DBNull.Value ? null : dr["FLOWSELECTTYPE"].ToString();
                        master.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
                        master.FORMID = dr["FORMID"] == DBNull.Value ? null : dr["FORMID"].ToString();
                        master.INSTANCEID = dr["INSTANCEID"] == DBNull.Value ? null : dr["INSTANCEID"].ToString();
                        //master.KPITIMEXML = dr["KPITIMEXML"] == DBNull.Value ? null : dr["KPITIMEXML"].ToString();
                        master.MODELCODE = dr["MODELCODE"] == DBNull.Value ? null : dr["MODELCODE"].ToString();
                        master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
                        listMaster.Add(master);
                        listMasterID.Add("'" + master.FLOWRECORDMASTERID + "'");
                        #endregion

                    }
                    dr.Close();
                    if (listMasterID.Count > 0)
                    {
                        string sql = @"select *  from FLOW_FLOWRECORDDETAIL_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
                 
                        dr = dao.ExecuteReader(sql, null);
                        while (dr.Read())
                        {
                            #region detail
                            FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
                            string FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                            detail.FLOW_FLOWRECORDMASTER_T = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == FLOWRECORDMASTERID);
                            detail.FLOW_FLOWRECORDMASTER_T.FLOW_FLOWRECORDDETAIL_T.Add(detail);

                            detail.AGENTEDITDATE = dr["AGENTEDITDATE"] == DBNull.Value ? null : (DateTime?)dr["AGENTEDITDATE"];
                            detail.AGENTERNAME = dr["AGENTERNAME"] == DBNull.Value ? null : dr["AGENTERNAME"].ToString();
                            detail.AGENTUSERID = dr["AGENTUSERID"] == DBNull.Value ? null : dr["AGENTUSERID"].ToString();
                            detail.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
                            detail.CONTENT = dr["CONTENT"] == DBNull.Value ? null : dr["CONTENT"].ToString();
                            detail.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
                            detail.CREATEDATE = (DateTime)dr["CREATEDATE"];
                            detail.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
                            detail.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
                            detail.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
                            detail.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
                            detail.EDITCOMPANYID = dr["EDITCOMPANYID"] == DBNull.Value ? null : dr["EDITCOMPANYID"].ToString();
                            detail.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
                            detail.EDITDEPARTMENTID = dr["EDITDEPARTMENTID"] == DBNull.Value ? null : dr["EDITDEPARTMENTID"].ToString();
                            detail.EDITPOSTID = dr["EDITPOSTID"] == DBNull.Value ? null : dr["EDITPOSTID"].ToString();
                            detail.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
                            detail.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
                            detail.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
                            //detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
                            detail.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"] == DBNull.Value ? null : dr["FLOWRECORDDETAILID"].ToString();
                            detail.PARENTSTATEID = dr["PARENTSTATEID"] == DBNull.Value ? null : dr["PARENTSTATEID"].ToString();
                            detail.STATECODE = dr["STATECODE"] == DBNull.Value ? null : dr["STATECODE"].ToString();
                            detail.FLOW_CONSULTATION_T = new EntityCollection<FLOW_CONSULTATION_T>();
                            listDetail.Add(detail);

                            #endregion
                        }
                        dr.Close();
                    }

                    return listDetail;

                }
                catch (Exception ex)
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }
                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->GetFlowRecordTop:-" + FormID + "-" + ex.Message + ex.InnerException);
                }
        

        }

        /// <summary>
        /// 根据模块代码和用户id查询待审核单据
        /// </summary>
        /// <param name="ModelCode">模块代码</param>
        /// <param name="EditUserID">用户id</param>
        /// <returns></returns>
        public static List<string> GetWaitingApprovalForm(string ModelCode, string EditUserID)
        {
            List<string> fromids = new List<string>();
            string strSelect = @"select a.formid from FLOW_FLOWRECORDMASTER_T a
                                inner join  FLOW_FLOWRECORDDETAIL_T  b on a.flowrecordmasterid=b.flowrecordmasterid
                                where a.modelcode='" + ModelCode + @"'
                                and  b.flag=0
                                and b.edituserid='" + EditUserID + "'";
            DataTable dt = new DataTable();
            dt = dao.GetDataTable(strSelect);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    fromids.Add(dr["formid"].ToString());
                }
            }
            return fromids;

        }
        /// <summary>
        /// 判断是否可能用自选流程或提单人可以撒回流程
        /// string[0]=1 提交人可以撒回流程
        /// string[1]=1 可以用自选流程
        /// </summary>
        /// <param name="modelcode">模块代码</param>
        /// <param name="companyid">公司ID</param>
        public static string[] IsFreeFlowAndIsCancel(string modelcode, string companyid)
        {
            string[] strs=new string[2];
            //OracleConnection con = MicrosoftOracle.CreateOracleConnection(ADOHelper.ConnectionString);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select ");
                sb.AppendLine("(");
                sb.AppendLine("select  count(1) from flow_modeldefine_flowcancle t where MODELCODE='" + modelcode + "' AND COMPANYID='" + companyid + "' ");
                sb.AppendLine(") as IsCancel,");//自选流程
                sb.AppendLine("(");
                sb.AppendLine("select count(1) from flow_modeldefine_freeflow t  where MODELCODE='" + modelcode + "' AND COMPANYID='" + companyid + "' ");
                sb.AppendLine(") as FreeFlow");//提单人可以撒回
                sb.AppendLine("  from dual");
                DataTable dt = dao.GetDataTable(sb.ToString());
                if (dt.Rows.Count > 0)
                {
                    strs[0] = dt.Rows[0]["IsCancel"].ToString();
                    strs[1] = dt.Rows[0]["FreeFlow"].ToString();
                }
                return strs;
            }
            catch (Exception ee)
            {
                Tracer.Debug("判断是否可能用自选流程或提单人可以撒回出错：" + ee.ToString());
                return null;
            }
            finally
            {
                //MicrosoftOracle.Close(con);
            }

        }
        #endregion
        
        #region 旧的
       
//        public static void Add(FLOW_FLOWRECORDDETAIL_T detail)
//        {
//                try
//                {


//                    string sql = @"insert into FLOW_FLOWRECORDDETAIL_T(FLOWRECORDDETAILID,FLOWRECORDMASTERID,STATECODE,PARENTSTATEID,
//                                        CONTENT,CHECKSTATE,FLAG,CREATEUSERID,CREATEUSERNAME,CREATECOMPANYID,CREATEDEPARTMENTID,CREATEPOSTID,
//                                         CREATEDATE,EDITUSERID,EDITUSERNAME,EDITDATE,AGENTUSERID,AGENTERNAME,AGENTEDITDATE,EDITCOMPANYID,
//                                        EDITDEPARTMENTID,EDITPOSTID) 
//                                  values(:pFLOWRECORDDETAILID,:pFLOWRECORDMASTERID,:pSTATECODE,:pPARENTSTATEID,
//                                        :pCONTENT,:pCHECKSTATE,:pFLAG,:pCREATEUSERID,:pCREATEUSERNAME,:pCREATECOMPANYID,:pCREATEDEPARTMENTID,:pCREATEPOSTID,
//                                         :pCREATEDATE,:pEDITUSERID,:pEDITUSERNAME,:pEDITDATE,:pAGENTUSERID,:pAGENTERNAME,:pAGENTEDITDATE,:pEDITCOMPANYID,
//                                        :pEDITDEPARTMENTID,:pEDITPOSTID)";

                   
//                    //string sql = "insert into FLOW_FLOWRECORDDETAIL_T(FLOWRECORDDETAILID,FLOWRECORDMASTERID,STATECODE,PARENTSTATEID, " +
//                    //                   " CONTENT,CHECKSTATE,FLAG,CREATEUSERID,CREATEUSERNAME,CREATECOMPANYID,CREATEDEPARTMENTID,CREATEPOSTID, " +
//                    //                   "  CREATEDATE,EDITUSERID,EDITUSERNAME,EDITDATE,AGENTUSERID,AGENTERNAME,AGENTEDITDATE,EDITCOMPANYID, "+
//                    //                   " EDITDEPARTMENTID,EDITPOSTID) "+
//                    //             " values('" + detail.FLOWRECORDDETAILID + "','" + detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "','" + detail.STATECODE + "','" + detail.PARENTSTATEID + "', " +
//                    //               "  '" + detail.CONTENT + "',to_date('" + detail.CHECKSTATE + "','yyyy-mm-dd'),'" + detail.FLAG + "','" + detail.CREATEUSERID + "','" + detail.CREATEUSERNAME + "','" + detail.CREATECOMPANYID + "','" + detail.CREATEDEPARTMENTID + "','" + detail.CREATEPOSTID + "', " +
//                    //               "  to_date('" + detail.CREATEDATE + "','yyyy-mm-dd'),'" + detail.EDITUSERID + "','" + detail.EDITUSERNAME + "',to_date('" + detail.EDITDATE + "','yyyy-mm-dd'),'" + detail.AGENTUSERID + "','" + detail.AGENTERNAME + "','" + detail.AGENTEDITDATE + "','" + detail.EDITCOMPANYID + "'," +
//                    //               "   '" + detail.EDITDEPARTMENTID + "','" + detail.EDITPOSTID + "') ";

                                            

//                    #region
                   


//                    //OracleCommand cmd = con.CreateCommand();
//                    //cmd.CommandText = sql;


//                    ADOHelper.AddParameter("FLOWRECORDDETAILID", detail.FLOWRECORDDETAILID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("FLOWRECORDMASTERID", detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID, OracleType.VarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("STATECODE", detail.STATECODE, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("PARENTSTATEID", detail.PARENTSTATEID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONTENT", detail.CONTENT, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CHECKSTATE", detail.CHECKSTATE, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("FLAG", detail.FLAG, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEUSERID", detail.CREATEUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEUSERNAME", detail.CREATEUSERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATECOMPANYID", detail.CREATECOMPANYID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEDEPARTMENTID", detail.CREATEDEPARTMENTID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEPOSTID", detail.CREATEPOSTID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEDATE", detail.CREATEDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITUSERID", detail.EDITUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITUSERNAME", detail.EDITUSERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITDATE", detail.EDITDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("AGENTUSERID", detail.AGENTUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("AGENTERNAME", detail.AGENTERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("AGENTEDITDATE", detail.AGENTEDITDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITCOMPANYID", detail.EDITCOMPANYID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITDEPARTMENTID", detail.EDITDEPARTMENTID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITPOSTID", detail.EDITPOSTID, OracleType.NVarChar, cmd.Parameters);
                      


//                    cmd.ExecuteNonQuery();

                    
//                    #endregion
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL_Add:" + ex.Message);
//                }

            
//        }

//        public static void Update(FLOW_FLOWRECORDDETAIL_T detail)
//        {
//                try
//                {
//                    string sql = @" update FLOW_FLOWRECORDDETAIL_T set FLOWRECORDMASTERID=:pFLOWRECORDMASTERID,STATECODE=:pSTATECODE,PARENTSTATEID=:pPARENTSTATEID,
//                                        CONTENT=:pCONTENT,CHECKSTATE=:pCHECKSTATE,FLAG=:pFLAG,CREATEUSERID=:pCREATEUSERID,CREATEUSERNAME=:pCREATEUSERNAME,
//                                        CREATECOMPANYID=:pCREATECOMPANYID,CREATEDEPARTMENTID=:pCREATEDEPARTMENTID,CREATEPOSTID=:pCREATEPOSTID,
//                                         CREATEDATE=:pCREATEDATE,EDITUSERID=:pEDITUSERID,EDITUSERNAME=:pEDITUSERNAME,EDITDATE=:pEDITDATE,
//                                         AGENTUSERID=:pAGENTUSERID,AGENTERNAME=:pAGENTERNAME,AGENTEDITDATE=:pAGENTEDITDATE,EDITCOMPANYID=:pEDITCOMPANYID,
//                                        EDITDEPARTMENTID=:pEDITDEPARTMENTID,EDITPOSTID=:pEDITPOSTID
//                                     where  FLOWRECORDDETAILID=:pFLOWRECORDDETAILID";
                                 

//                    #region
                   


//                    //OracleCommand cmd = con.CreateCommand();
//                    //cmd.CommandText = sql;


//                    ADOHelper.AddParameter("FLOWRECORDDETAILID", detail.FLOWRECORDDETAILID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("FLOWRECORDMASTERID", detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID, OracleType.VarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("STATECODE", detail.STATECODE, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("PARENTSTATEID", detail.PARENTSTATEID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CONTENT", detail.CONTENT, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CHECKSTATE", detail.CHECKSTATE, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("FLAG", detail.FLAG, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEUSERID", detail.CREATEUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEUSERNAME", detail.CREATEUSERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATECOMPANYID", detail.CREATECOMPANYID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEDEPARTMENTID", detail.CREATEDEPARTMENTID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEPOSTID", detail.CREATEPOSTID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("CREATEDATE", detail.CREATEDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITUSERID", detail.EDITUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITUSERNAME", detail.EDITUSERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITDATE", detail.EDITDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("AGENTUSERID", detail.AGENTUSERID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("AGENTERNAME", detail.AGENTERNAME, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("AGENTEDITDATE", detail.AGENTEDITDATE, OracleType.DateTime, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITCOMPANYID", detail.EDITCOMPANYID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITDEPARTMENTID", detail.EDITDEPARTMENTID, OracleType.NVarChar, cmd.Parameters);
//                    ADOHelper.AddParameter("EDITPOSTID", detail.EDITPOSTID, OracleType.NVarChar, cmd.Parameters);

//                    cmd.ExecuteNonQuery();

                    
//                    #endregion
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL_Add:" + ex.Message);
//                }

            
//        }

//        public static void Delete(FLOW_FLOWRECORDDETAIL_T detail)
//        {
//                try
//                {
//                    string sql = @" delete from  FLOW_FLOWRECORDDETAIL_T  where  FLOWRECORDDETAILID=:pFLOWRECORDDETAILID";


//                    #region
                   


//                    //OracleCommand cmd = con.CreateCommand();
//                    //cmd.CommandText = sql;


//                    ADOHelper.AddParameter("FLOWRECORDDETAILID", detail.FLOWRECORDDETAILID, OracleType.NVarChar, cmd.Parameters);                   

//                    cmd.ExecuteNonQuery();

                    
//                    #endregion
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL_Add:" + ex.Message);
//                }

            
//        }
//        /// <summary>
//        /// 获取流程
//        /// </summary>
//        /// <param name="FormID">表单ID</param>
//        /// <param name="FlowGUID">明细ID</param>
//        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
//        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
//        /// <param name="ModelCode">模块代码</param>
//        /// <param name="CompanyID">创建公司ID</param>
//        /// <param name="EditUserID">操作人</param>
//        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
//        /// <returns></returns>
//        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowRecord(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<string> FlowType)
//        {
//            #region sql

//            StringBuilder sb = new StringBuilder();
//            sb.Append(@"select FLOW_FLOWRECORDDETAIL_T.* from FLOW_FLOWRECORDDETAIL_T
//join FLOW_FLOWRECORDMASTER_T
//on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID
//where 1=1 ");

//            if (!string.IsNullOrEmpty(FlowGUID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");

//            }
//            if (!string.IsNullOrEmpty(Flag))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
//            }

//            if (!string.IsNullOrEmpty(EditUserID))
//            {
//                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
//            }


//            if (!string.IsNullOrEmpty(CompanyID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
//            }
//            if (!string.IsNullOrEmpty(ModelCode))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
//            }
//            if (!string.IsNullOrEmpty(FormID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
//            }
//            if (!string.IsNullOrEmpty(CheckState))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
//            }

//            if (FlowType != null)
//            {
//                if (FlowType.Count == 1)
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
//                }
//                else
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
//                    for (int i = 0; i < FlowType.Count - 1; i++)
//                    {
//                        sb.Append("'" + FlowType[i] + "',");
//                    }
//                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
//                    sb.Append(")");
//                }
//            }


//            #endregion
//            List<FLOW_FLOWRECORDDETAIL_T> listDetail = new List<FLOW_FLOWRECORDDETAIL_T>();
//            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();

//                IDataReader dr = null;
//                try
//                {
//                    List<string> listMasterID = new List<string>();
                   
//                    //OracleCommand command = con.CreateCommand();
//                    command.CommandText = sb.ToString();
//                    dr = command.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        #region detail
//                        FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
//                        detail.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();
//                        detail.AGENTEDITDATE = dr["AGENTEDITDATE"] == DBNull.Value ? null : (DateTime?)dr["AGENTEDITDATE"];
//                        detail.AGENTERNAME = dr["AGENTERNAME"] == DBNull.Value ? null : dr["AGENTERNAME"].ToString();
//                        detail.AGENTUSERID = dr["AGENTUSERID"] == DBNull.Value ? null : dr["AGENTUSERID"].ToString();
//                        detail.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
//                        detail.CONTENT = dr["CONTENT"] == DBNull.Value ? null : dr["CONTENT"].ToString();
//                        detail.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
//                        detail.CREATEDATE = (DateTime)dr["CREATEDATE"];
//                        detail.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
//                        detail.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
//                        detail.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
//                        detail.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
//                        detail.EDITCOMPANYID = dr["EDITCOMPANYID"] == DBNull.Value ? null : dr["EDITCOMPANYID"].ToString();
//                        detail.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
//                        detail.EDITDEPARTMENTID = dr["EDITDEPARTMENTID"] == DBNull.Value ? null : dr["EDITDEPARTMENTID"].ToString();
//                        detail.EDITPOSTID = dr["EDITPOSTID"] == DBNull.Value ? null : dr["EDITPOSTID"].ToString();
//                        detail.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
//                        detail.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
//                        detail.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
//                        detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                        detail.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"] == DBNull.Value ? null : dr["FLOWRECORDDETAILID"].ToString();
//                        detail.PARENTSTATEID = dr["PARENTSTATEID"] == DBNull.Value ? null : dr["PARENTSTATEID"].ToString();
//                        detail.STATECODE = dr["STATECODE"] == DBNull.Value ? null : dr["STATECODE"].ToString();
//                        listDetail.Add(detail);
//                        if (!listMasterID.Contains(detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID))
//                        {
//                            listMasterID.Add("'" + detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "'");
//                        }
//                        #endregion
//                    }
//                    dr.Close();

//                    #region master
//                    if (listMasterID.Count > 0)
//                    {
//                        command.CommandText = "select * from FLOW_FLOWRECORDMASTER_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
//                        dr = command.ExecuteReader();
//                        while (dr.Read())
//                        {
//                            #region master
//                            FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();
//                            master.ACTIVEROLE = dr["ACTIVEROLE"] == DBNull.Value ? null : dr["ACTIVEROLE"].ToString();
//                            master.BUSINESSOBJECT = dr["BUSINESSOBJECT"] == DBNull.Value ? null : dr["BUSINESSOBJECT"].ToString();
//                            master.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
//                            master.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
//                            master.CREATEDATE = (DateTime)dr["CREATEDATE"];
//                            master.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
//                            master.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
//                            master.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
//                            master.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
//                            master.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
//                            master.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
//                            master.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
//                            master.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
//                            master.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                            master.FLOWSELECTTYPE = dr["FLOWSELECTTYPE"] == DBNull.Value ? null : dr["FLOWSELECTTYPE"].ToString();
//                            master.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
//                            master.FORMID = dr["FORMID"] == DBNull.Value ? null : dr["FORMID"].ToString();
//                            master.INSTANCEID = dr["INSTANCEID"] == DBNull.Value ? null : dr["INSTANCEID"].ToString();
//                            master.KPITIMEXML = dr["KPITIMEXML"] == DBNull.Value ? null : dr["KPITIMEXML"].ToString();
//                            master.MODELCODE = dr["MODELCODE"] == DBNull.Value ? null : dr["MODELCODE"].ToString();
//                            master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
//                            listMaster.Add(master);
//                            #endregion

//                        }
//                        dr.Close();
//                    }
//                    #endregion
                    
//                    listDetail.ForEach(detail =>
//                    {
//                        FLOW_FLOWRECORDMASTER_T master = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);

//                        detail.FLOW_FLOWRECORDMASTER_T = master;
//                        master.FLOW_FLOWRECORDDETAIL_T.Add(detail);
//                    });

//                    return listDetail;

//                }
//                catch (Exception ex)
//                {
//                    if (dr != null && !dr.IsClosed)
//                    {
//                        dr.Close();
//                    }
//                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->GetFlowRecord:-" + FormID + "-" + ex.Message + ex.InnerException);
//                }

            



//        }
//        /// <summary>
//        /// 获取流程
//        /// </summary>
//        /// <param name="FormID">表单ID</param>
//        /// <param name="FlowGUID">明细ID</param>
//        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
//        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
//        /// <param name="ModelCode">模块代码</param>
//        /// <param name="CompanyID">创建公司ID</param>
//        /// <param name="EditUserID">操作人</param>
//        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
//        /// <returns></returns>      
//        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowRecordV(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<string> FlowType)
//        {
//            #region sql

//            StringBuilder sb = new StringBuilder();
//            sb.Append(@"select FLOW_FLOWRECORDDETAIL_T.* from FLOW_FLOWRECORDDETAIL_T
//join FLOW_FLOWRECORDMASTER_T
//on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID
//where 1=1 ");
            
//            if (!string.IsNullOrEmpty(FlowGUID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");
                
//            }
//            if (!string.IsNullOrEmpty(Flag))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
//            }

//            if (!string.IsNullOrEmpty(EditUserID))
//            {
//                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
//            }

           
//            if (!string.IsNullOrEmpty(CompanyID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
//            }
//            if (!string.IsNullOrEmpty(ModelCode))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
//            }
//            if (!string.IsNullOrEmpty(FormID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
//            }
//            if (!string.IsNullOrEmpty(CheckState))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
//            }

//            if (FlowType != null)
//            {
//                if (FlowType.Count == 1)
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
//                }
//                else
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
//                    for (int i = 0; i < FlowType.Count - 1; i++)
//                    {
//                        sb.Append("'" + FlowType[i] + "',");
//                    }
//                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
//                    sb.Append(")");
//                }
//            }

           
//            #endregion
//            List<FLOW_FLOWRECORDDETAIL_T> listDetail = new List<FLOW_FLOWRECORDDETAIL_T>();
//            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();

//                IDataReader dr = null;
//                try
//                {
//                    List<string> listDetailID = new List<string>();
//                    List<string> listMasterID = new List<string>();

                   
//                    //OracleCommand command = con.CreateCommand();
//                    command.CommandText = sb.ToString();
//                    dr = command.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        #region detail
//                        FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
//                        detail.FLOW_FLOWRECORDMASTER_T = new FLOW_FLOWRECORDMASTER_T();
//                        detail.AGENTEDITDATE = dr["AGENTEDITDATE"] == DBNull.Value ? null : (DateTime?)dr["AGENTEDITDATE"];
//                        detail.AGENTERNAME = dr["AGENTERNAME"] == DBNull.Value ? null : dr["AGENTERNAME"].ToString();
//                        detail.AGENTUSERID = dr["AGENTUSERID"] == DBNull.Value ? null : dr["AGENTUSERID"].ToString();
//                        detail.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
//                        detail.CONTENT = dr["CONTENT"] == DBNull.Value ? null : dr["CONTENT"].ToString();
//                        detail.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
//                        detail.CREATEDATE = (DateTime)dr["CREATEDATE"];
//                        detail.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
//                        detail.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
//                        detail.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
//                        detail.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
//                        detail.EDITCOMPANYID = dr["EDITCOMPANYID"] == DBNull.Value ? null : dr["EDITCOMPANYID"].ToString();
//                        detail.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
//                        detail.EDITDEPARTMENTID = dr["EDITDEPARTMENTID"] == DBNull.Value ? null : dr["EDITDEPARTMENTID"].ToString();
//                        detail.EDITPOSTID = dr["EDITPOSTID"] == DBNull.Value ? null : dr["EDITPOSTID"].ToString();
//                        detail.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
//                        detail.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
//                        detail.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
//                        detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                        detail.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"] == DBNull.Value ? null : dr["FLOWRECORDDETAILID"].ToString();
//                        detail.PARENTSTATEID = dr["PARENTSTATEID"] == DBNull.Value ? null : dr["PARENTSTATEID"].ToString();
//                        detail.STATECODE = dr["STATECODE"] == DBNull.Value ? null : dr["STATECODE"].ToString();
//                        detail.FLOW_CONSULTATION_T = new EntityCollection<FLOW_CONSULTATION_T>();
//                        listDetail.Add(detail);
//                        listDetailID.Add("'" + detail.FLOWRECORDDETAILID + "'");
//                        if (!listMasterID.Contains(detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID))
//                        {
//                            listMasterID.Add("'" + detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID + "'");
//                        }


//                        #endregion
//                    }
//                    dr.Close();

//                    #region master
//                    if (listMasterID.Count > 0)
//                    {
//                        command.CommandText = @"select CHECKSTATE,CREATECOMPANYID,CREATEDATE,CREATEDEPARTMENTID,CREATEPOSTID,CREATEUSERID,CREATEUSERNAME,
//                                               EDITDATE,EDITUSERID,EDITUSERNAME,FLOWCODE,FLOWRECORDMASTERID,FLOWSELECTTYPE,FLOWTYPE,FORMID,INSTANCEID,MODELCODE
//                                              from FLOW_FLOWRECORDMASTER_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";

//                        dr = command.ExecuteReader();
//                        while (dr.Read())
//                        {
//                            #region master
//                            FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();

//                            //master.ACTIVEROLE = dr["ACTIVEROLE"] == DBNull.Value ? null : dr["ACTIVEROLE"].ToString();
//                            //master.BUSINESSOBJECT = dr["BUSINESSOBJECT"] == DBNull.Value ? null : dr["BUSINESSOBJECT"].ToString();
//                            master.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
//                            master.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
//                            master.CREATEDATE = (DateTime)dr["CREATEDATE"];
//                            master.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
//                            master.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
//                            master.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
//                            master.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
//                            master.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
//                            master.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
//                            master.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
//                            master.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
//                            master.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                            master.FLOWSELECTTYPE = dr["FLOWSELECTTYPE"] == DBNull.Value ? null : dr["FLOWSELECTTYPE"].ToString();
//                            master.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
//                            master.FORMID = dr["FORMID"] == DBNull.Value ? null : dr["FORMID"].ToString();
//                            master.INSTANCEID = dr["INSTANCEID"] == DBNull.Value ? null : dr["INSTANCEID"].ToString();
//                            //master.KPITIMEXML = dr["KPITIMEXML"] == DBNull.Value ? null : dr["KPITIMEXML"].ToString();
//                            master.MODELCODE = dr["MODELCODE"] == DBNull.Value ? null : dr["MODELCODE"].ToString();
//                            master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
//                            listMaster.Add(master);
//                            #endregion

//                        }
//                        dr.Close();

//                    }

//                    //listMaster.ForEach(master =>
//                    //{
//                    //    master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
//                    //    IEnumerable<FLOW_FLOWRECORDDETAIL_T> idetail = listDetail.Where(d => d.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID == master.FLOWRECORDMASTERID);
//                    //    foreach (var detail in idetail)
//                    //    {
//                    //        detail.FLOW_FLOWRECORDMASTER_T = master;
//                    //        master.FLOW_FLOWRECORDDETAIL_T.Add(detail);
//                    //    }
//                    //});
//                    #endregion
//                    #region FLOW_CONSULTATION_T

//                    List<FLOW_CONSULTATION_T> listConsultation = new List<FLOW_CONSULTATION_T>();
//                    if (!string.IsNullOrEmpty(FormID) && !string.IsNullOrEmpty(ModelCode))
//                    {
//                        if (listDetailID.Count > 0)
//                        {

//                            command.CommandText = @"select * from FLOW_CONSULTATION_T where FLOWRECORDDETAILID in (" + string.Join(",", listDetailID.ToArray()) + ")";
//                            dr = command.ExecuteReader();
//                            while (dr.Read())
//                            {
//                                #region FLOW_CONSULTATION_T
//                                FLOW_CONSULTATION_T consul = new FLOW_CONSULTATION_T();
//                                consul.CONSULTATIONCONTENT = dr["CONSULTATIONCONTENT"] == DBNull.Value ? null : dr["CONSULTATIONCONTENT"].ToString();
//                                consul.CONSULTATIONDATE = dr["CONSULTATIONDATE"] == DBNull.Value ? null : (DateTime?)dr["CONSULTATIONDATE"];
//                                consul.CONSULTATIONID = dr["CONSULTATIONID"] == DBNull.Value ? null : dr["CONSULTATIONID"].ToString();
//                                consul.CONSULTATIONUSERID = dr["CONSULTATIONUSERID"] == DBNull.Value ? null : dr["CONSULTATIONUSERID"].ToString();
//                                consul.CONSULTATIONUSERNAME = dr["CONSULTATIONUSERNAME"] == DBNull.Value ? null : dr["CONSULTATIONUSERNAME"].ToString();
//                                consul.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
//                                consul.FLOW_FLOWRECORDDETAIL_T = new FLOW_FLOWRECORDDETAIL_T();
//                                consul.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"].ToString();
//                                consul.REPLYCONTENT = dr["REPLYCONTENT"] == DBNull.Value ? null : dr["REPLYCONTENT"].ToString();
//                                consul.REPLYDATE = dr["REPLYDATE"] == DBNull.Value ? null : (DateTime?)dr["REPLYDATE"];
//                                consul.REPLYUSERID = dr["REPLYUSERID"] == DBNull.Value ? null : dr["REPLYUSERID"].ToString();
//                                consul.REPLYUSERNAME = dr["REPLYUSERNAME"] == DBNull.Value ? null : dr["REPLYUSERNAME"].ToString();

//                                listConsultation.Add(consul);

//                                #endregion

//                            }
//                            dr.Close();

//                        }
//                    }
//                    #endregion
                    
//                    listDetail.ForEach(detail =>
//                    {
//                        #region
//                        FLOW_FLOWRECORDMASTER_T master = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID);

//                        detail.FLOW_FLOWRECORDMASTER_T = master;
//                        if (master.FLOW_FLOWRECORDDETAIL_T == null)
//                        {
//                            master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
//                        }
//                        master.FLOW_FLOWRECORDDETAIL_T.Add(detail);

//                        if (!string.IsNullOrEmpty(FormID) && !string.IsNullOrEmpty(ModelCode))
//                        {
//                            IEnumerable<FLOW_CONSULTATION_T> iFLOW_CONSULTATION_T = listConsultation.Where(c => c.FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID == detail.FLOWRECORDDETAILID);
//                            if (iFLOW_CONSULTATION_T.Count() > 0)
//                            {
//                                detail.FLOW_CONSULTATION_T = new EntityCollection<FLOW_CONSULTATION_T>();
//                                foreach (var consultation in iFLOW_CONSULTATION_T)
//                                {
//                                    consultation.FLOW_FLOWRECORDDETAIL_T = detail;
//                                    detail.FLOW_CONSULTATION_T.Add(consultation);
//                                }
//                            }
//                        }
//                        #endregion


//                    });



//                    return listDetail;

//                }
//                catch (Exception ex)
//                {
//                    if (dr != null && !dr.IsClosed)
//                    {
//                        dr.Close();
//                    }

//                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->GetFlowRecordV:-" + FormID + "-" + ex.Message + ex.InnerException);
//                }

            



//        }
//        /// <summary>
//        /// 获取流程(只返回前100条detail)
//        /// </summary>
//        /// <param name="FormID">表单ID</param>
//        /// <param name="FlowGUID">明细ID</param>
//        /// <param name="CheckState">审批状态(同意：1，不同意:0 ,未处理:2，会签同意7，会签不同意8)</param>
//        /// <param name="Flag">审批状态（已审批：1，未审批:0）</param>
//        /// <param name="ModelCode">模块代码</param>
//        /// <param name="CompanyID">创建公司ID</param>
//        /// <param name="EditUserID">操作人</param>
//        /// <param name="FlowType">流程类型（0:审批流程，1：任务流程）</param>
//        /// <returns></returns>      
//        public static List<FLOW_FLOWRECORDDETAIL_T> GetFlowRecordTop(string FormID, string FlowGUID, string CheckState, string Flag, string ModelCode, string CompanyID, string EditUserID, List<string> FlowType)
//        {
//            #region sql

//            StringBuilder sb = new StringBuilder();
//            sb.Append(@"select FLOW_FLOWRECORDMASTER_T.CHECKSTATE,FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID,FLOW_FLOWRECORDMASTER_T.CREATEDATE,
//                           FLOW_FLOWRECORDMASTER_T.CREATEDEPARTMENTID,FLOW_FLOWRECORDMASTER_T.CREATEPOSTID,FLOW_FLOWRECORDMASTER_T.CREATEUSERID,
//                           FLOW_FLOWRECORDMASTER_T.CREATEUSERNAME,FLOW_FLOWRECORDMASTER_T.EDITDATE,FLOW_FLOWRECORDMASTER_T.EDITUSERID,
//                           FLOW_FLOWRECORDMASTER_T.EDITUSERNAME,FLOW_FLOWRECORDMASTER_T.FLOWCODE,FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID,
//                           FLOW_FLOWRECORDMASTER_T.FLOWSELECTTYPE,FLOW_FLOWRECORDMASTER_T.FLOWTYPE,FLOW_FLOWRECORDMASTER_T.FORMID,
//                       FLOW_FLOWRECORDMASTER_T.INSTANCEID,FLOW_FLOWRECORDMASTER_T.MODELCODE 
//                    from FLOW_FLOWRECORDMASTER_T");
//            sb.Append(" where FLOWRECORDMASTERID in (");
//            sb.Append(@" select distinct FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID from FLOW_FLOWRECORDMASTER_T 
//                            join FLOW_FLOWRECORDDETAIL_T
//                            on FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID=FLOW_FLOWRECORDDETAIL_T.FLOWRECORDMASTERID where 1=1");

//            #region detail 条件
//            if (!string.IsNullOrEmpty(FlowGUID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLOWRECORDDETAILID='" + FlowGUID + "'");

//            }
//            if (!string.IsNullOrEmpty(Flag))
//            {
//                sb.Append(" and FLOW_FLOWRECORDDETAIL_T.FLAG='" + Flag + "'");
//            }

//            if (!string.IsNullOrEmpty(EditUserID))
//            {
//                sb.Append(" and (FLOW_FLOWRECORDDETAIL_T.EDITUSERID='" + EditUserID + "' or FLOW_FLOWRECORDDETAIL_T.AGENTUSERID='" + EditUserID + "')");
//            }
//            #endregion 

//            #region master 条件

//            if (!string.IsNullOrEmpty(CompanyID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CREATECOMPANYID='" + CompanyID + "'");
//            }
//            if (!string.IsNullOrEmpty(ModelCode))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.MODELCODE='" + ModelCode + "'");
//            }
//            if (!string.IsNullOrEmpty(FormID))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.FORMID='" + FormID + "'");
//            }
//            if (!string.IsNullOrEmpty(CheckState))
//            {
//                sb.Append(" and FLOW_FLOWRECORDMASTER_T.CHECKSTATE='" + CheckState + "'");
//            }

//            if (FlowType != null)
//            {
//                if (FlowType.Count == 1)
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE='" + FlowType[0] + "'");
//                }
//                else
//                {
//                    sb.Append(" and FLOW_FLOWRECORDMASTER_T.FLOWTYPE in (");
//                    for (int i = 0; i < FlowType.Count - 1; i++)
//                    {
//                        sb.Append("'" + FlowType[i] + "',");
//                    }
//                    sb.Append("'" + FlowType[FlowType.Count - 1] + "'");
//                    sb.Append(")");
//                }
//            }
//            #endregion 

//            sb.Append(")");

//            sb.Append(" and rownum <=20");
//            sb.Append(" order by FLOW_FLOWRECORDMASTER_T.CREATEDATE desc");

            
           

            

//            #endregion
//            List<FLOW_FLOWRECORDDETAIL_T> listDetail = new List<FLOW_FLOWRECORDDETAIL_T>();
//            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();

//                IDataReader dr = null;
//                try
//                {
                    
//                    List<string> listMasterID = new List<string>();

                   
//                    //OracleCommand command = con.CreateCommand();
//                    command.CommandText = sb.ToString();
//                    dr = command.ExecuteReader();

//                    while (dr.Read())
//                    {
//                        #region master
//                        FLOW_FLOWRECORDMASTER_T master = new FLOW_FLOWRECORDMASTER_T();

//                        //master.ACTIVEROLE = dr["ACTIVEROLE"] == DBNull.Value ? null : dr["ACTIVEROLE"].ToString();
//                        //master.BUSINESSOBJECT = dr["BUSINESSOBJECT"] == DBNull.Value ? null : dr["BUSINESSOBJECT"].ToString();
//                        master.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
//                        master.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
//                        master.CREATEDATE = (DateTime)dr["CREATEDATE"];
//                        master.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
//                        master.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
//                        master.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
//                        master.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
//                        master.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
//                        master.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
//                        master.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
//                        master.FLOWCODE = dr["FLOWCODE"] == DBNull.Value ? null : dr["FLOWCODE"].ToString();
//                        master.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                        master.FLOWSELECTTYPE = dr["FLOWSELECTTYPE"] == DBNull.Value ? null : dr["FLOWSELECTTYPE"].ToString();
//                        master.FLOWTYPE = dr["FLOWTYPE"] == DBNull.Value ? null : dr["FLOWTYPE"].ToString();
//                        master.FORMID = dr["FORMID"] == DBNull.Value ? null : dr["FORMID"].ToString();
//                        master.INSTANCEID = dr["INSTANCEID"] == DBNull.Value ? null : dr["INSTANCEID"].ToString();
//                        //master.KPITIMEXML = dr["KPITIMEXML"] == DBNull.Value ? null : dr["KPITIMEXML"].ToString();
//                        master.MODELCODE = dr["MODELCODE"] == DBNull.Value ? null : dr["MODELCODE"].ToString();
//                        master.FLOW_FLOWRECORDDETAIL_T = new EntityCollection<FLOW_FLOWRECORDDETAIL_T>();
//                        listMaster.Add(master);
//                        listMasterID.Add("'" + master.FLOWRECORDMASTERID + "'");
//                        #endregion

//                    }
//                    dr.Close();
//                    if (listMasterID.Count > 0)
//                    {
//                        command.CommandText = @"select *  from FLOW_FLOWRECORDDETAIL_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
//                        dr = command.ExecuteReader();

//                        while (dr.Read())
//                        {
//                            #region detail
//                            FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
//                            string FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                            detail.FLOW_FLOWRECORDMASTER_T = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == FLOWRECORDMASTERID);
//                            detail.FLOW_FLOWRECORDMASTER_T.FLOW_FLOWRECORDDETAIL_T.Add(detail);

//                            detail.AGENTEDITDATE = dr["AGENTEDITDATE"] == DBNull.Value ? null : (DateTime?)dr["AGENTEDITDATE"];
//                            detail.AGENTERNAME = dr["AGENTERNAME"] == DBNull.Value ? null : dr["AGENTERNAME"].ToString();
//                            detail.AGENTUSERID = dr["AGENTUSERID"] == DBNull.Value ? null : dr["AGENTUSERID"].ToString();
//                            detail.CHECKSTATE = dr["CHECKSTATE"] == DBNull.Value ? null : dr["CHECKSTATE"].ToString();
//                            detail.CONTENT = dr["CONTENT"] == DBNull.Value ? null : dr["CONTENT"].ToString();
//                            detail.CREATECOMPANYID = dr["CREATECOMPANYID"] == DBNull.Value ? null : dr["CREATECOMPANYID"].ToString();
//                            detail.CREATEDATE = (DateTime)dr["CREATEDATE"];
//                            detail.CREATEDEPARTMENTID = dr["CREATEDEPARTMENTID"] == DBNull.Value ? null : dr["CREATEDEPARTMENTID"].ToString();
//                            detail.CREATEPOSTID = dr["CREATEPOSTID"] == DBNull.Value ? null : dr["CREATEPOSTID"].ToString();
//                            detail.CREATEUSERID = dr["CREATEUSERID"] == DBNull.Value ? null : dr["CREATEUSERID"].ToString();
//                            detail.CREATEUSERNAME = dr["CREATEUSERNAME"] == DBNull.Value ? null : dr["CREATEUSERNAME"].ToString();
//                            detail.EDITCOMPANYID = dr["EDITCOMPANYID"] == DBNull.Value ? null : dr["EDITCOMPANYID"].ToString();
//                            detail.EDITDATE = dr["EDITDATE"] == DBNull.Value ? null : (DateTime?)dr["EDITDATE"];
//                            detail.EDITDEPARTMENTID = dr["EDITDEPARTMENTID"] == DBNull.Value ? null : dr["EDITDEPARTMENTID"].ToString();
//                            detail.EDITPOSTID = dr["EDITPOSTID"] == DBNull.Value ? null : dr["EDITPOSTID"].ToString();
//                            detail.EDITUSERID = dr["EDITUSERID"] == DBNull.Value ? null : dr["EDITUSERID"].ToString();
//                            detail.EDITUSERNAME = dr["EDITUSERNAME"] == DBNull.Value ? null : dr["EDITUSERNAME"].ToString();
//                            detail.FLAG = dr["FLAG"] == DBNull.Value ? null : dr["FLAG"].ToString();
//                            //detail.FLOW_FLOWRECORDMASTER_T.FLOWRECORDMASTERID = dr["FLOWRECORDMASTERID"].ToString();
//                            detail.FLOWRECORDDETAILID = dr["FLOWRECORDDETAILID"] == DBNull.Value ? null : dr["FLOWRECORDDETAILID"].ToString();
//                            detail.PARENTSTATEID = dr["PARENTSTATEID"] == DBNull.Value ? null : dr["PARENTSTATEID"].ToString();
//                            detail.STATECODE = dr["STATECODE"] == DBNull.Value ? null : dr["STATECODE"].ToString();
//                            detail.FLOW_CONSULTATION_T = new EntityCollection<FLOW_CONSULTATION_T>();
//                            listDetail.Add(detail);                           

//                            #endregion
//                        }
//                        dr.Close();
//                    }    

                   
                                  


//                    return listDetail;

//                }
//                catch (Exception ex)
//                {
//                    if (dr != null && !dr.IsClosed)
//                    {
//                        dr.Close();
//                    }

//                    if (con.State == ConnectionState.Open)
//                    {
                        
//                    }
//                    throw new Exception("FLOW_FLOWRECORDDETAIL_TDAL->GetFlowRecordTop:-" + FormID + "-" + ex.Message + ex.InnerException);
//                }

            



//        }
        #endregion
    }
}
