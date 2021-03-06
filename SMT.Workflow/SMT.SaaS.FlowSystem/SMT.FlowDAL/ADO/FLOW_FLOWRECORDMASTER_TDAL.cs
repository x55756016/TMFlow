﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Data.Objects.DataClasses;
using SMT.FlowDAL;
using SMT.Foundation.Log;
using SMT.Workflow.Common.Model;
using SMT.Foundation.Core;

namespace SMT.FLOWDAL.ADO
{
    /// <summary>
    /// 流程审批实例表
    /// </summary>
    public class FLOW_FLOWRECORDMASTER_TDAL:BaseFlowDAL
    {
        #region 提莫科技新增
        /// <summary>
        /// 新增：[流程审批实例表]
        /// </summary>
        /// <param name="con">//OracleConnection 连接对象</param>
        /// <param name="model">流程审批实例表</param>
        public static int Add(FLOW_FLOWRECORDMASTER_T model)
        {

            try
            {
                string insSql = "INSERT INTO FLOW_FLOWRECORDMASTER_T (FLOWRECORDMASTERID,INSTANCEID,FLOWSELECTTYPE,MODELCODE,FLOWCODE,FORMID,FLOWTYPE,CHECKSTATE,CREATEUSERID,CREATEUSERNAME,CREATECOMPANYID,CREATEDEPARTMENTID,CREATEPOSTID,CREATEDATE,EDITUSERID,EDITUSERNAME,EDITDATE,ACTIVEROLE,BUSINESSOBJECT,KPITIMEXML) VALUES (@FLOWRECORDMASTERID,@INSTANCEID,@FLOWSELECTTYPE,@MODELCODE,@FLOWCODE,@FORMID,@FLOWTYPE,@CHECKSTATE,@CREATEUSERID,@CREATEUSERNAME,@CREATECOMPANYID,@CREATEDEPARTMENTID,@CREATEPOSTID,@CREATEDATE,@EDITUSERID,@EDITUSERNAME,@EDITDATE,@ACTIVEROLE,@BUSINESSOBJECT,@KPITIMEXML)";
                Parameter[] pageparm =
                {               
                    new Parameter("@FLOWRECORDMASTERID"), 
                    new Parameter("@INSTANCEID"), 
                    new Parameter("@FLOWSELECTTYPE"), 
                    new Parameter("@MODELCODE"), 
                    new Parameter("@FLOWCODE"), 
                    new Parameter("@FORMID"), 
                    new Parameter("@FLOWTYPE"), 
                    new Parameter("@CHECKSTATE"), 
                    new Parameter("@CREATEUSERID"), 
                    new Parameter("@CREATEUSERNAME"), 
                    new Parameter("@CREATECOMPANYID"), 
                    new Parameter("@CREATEDEPARTMENTID"), 
                    new Parameter("@CREATEPOSTID"), 
                    new Parameter("@CREATEDATE"), 
                    new Parameter("@EDITUSERID"), 
                    new Parameter("@EDITUSERNAME"), 
                    new Parameter("@EDITDATE"), 
                    new Parameter("@ACTIVEROLE"), 
                    new Parameter("@BUSINESSOBJECT"), 
                    new Parameter("@KPITIMEXML") 

                };
                pageparm[0].ParameterValue = GetValue(model.FLOWRECORDMASTERID);//
                pageparm[1].ParameterValue = GetValue(model.INSTANCEID);//
                pageparm[2].ParameterValue = GetValue(model.FLOWSELECTTYPE);//0:固定流程，1：自选流程
                pageparm[3].ParameterValue = GetValue(model.MODELCODE);//
                pageparm[4].ParameterValue = GetValue(model.FLOWCODE);//
                pageparm[5].ParameterValue = GetValue(model.FORMID);//
                pageparm[6].ParameterValue = GetValue(model.FLOWTYPE);//0:审批流程，1：任务流程
                pageparm[7].ParameterValue = GetValue(model.CHECKSTATE);//1:审批中，2：审批通过，3审批不通过，5撤销(为与字典保持一致)
                pageparm[8].ParameterValue = GetValue(model.CREATEUSERID);//
                pageparm[9].ParameterValue = GetValue(model.CREATEUSERNAME);//
                pageparm[10].ParameterValue = GetValue(model.CREATECOMPANYID);//
                pageparm[11].ParameterValue = GetValue(model.CREATEDEPARTMENTID);//
                pageparm[12].ParameterValue = GetValue(model.CREATEPOSTID);//
                pageparm[13].ParameterValue = GetValue(model.CREATEDATE);//
                pageparm[14].ParameterValue = GetValue(model.EDITUSERID);//
                pageparm[15].ParameterValue = GetValue(model.EDITUSERNAME);//
                pageparm[16].ParameterValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//
                pageparm[17].ParameterValue = GetValue(model.ACTIVEROLE);//
                pageparm[18].ParameterValue = GetValue(model.BUSINESSOBJECT);//
                pageparm[19].ParameterValue = GetValue(model.KPITIMEXML);//

                int n = dao.ExecuteNonQuery(insSql, pageparm);
                Tracer.Debug("FLOW_FLOWRECORDMASTER_TDAL->Add 新增：[流程审批实例表]成功：FLOWRECORDDETAILID＝" + model.FLOWRECORDMASTERID + ";EDITUSERID=" + model.EDITUSERID + ";CHECKSTATE=" + model.CHECKSTATE + ";时间：" + DateTime.Now.ToString());
                return n;
            }
            catch (Exception ex)
            {
                Tracer.Debug("FLOW_FLOWRECORDMASTER_TDAL->Add 新增：[流程审批实例表]失败：FLOWRECORDDETAILID＝" + model.FLOWRECORDMASTERID + ";EDITUSERID=" + model.EDITUSERID + ";CHECKSTATE=" + model.CHECKSTATE + ";时间：" + DateTime.Now.ToString() + "\r\n异常信息：" + ex.Message);
                throw new Exception("FLOW_FLOWRECORDMASTER_TDAL->Add:" + ex.Message);
            }
        }
        /// <summary>
        /// 更新：[流程审批实例表]
        /// </summary>
        /// <param name="con">//OracleConnection 连接对象</param>
        /// <param name="model">流程审批实例表</param>
        public static int Update( FLOW_FLOWRECORDMASTER_T model)
        {
            try
            {
                string updSql = "UPDATE FLOW_FLOWRECORDMASTER_T SET INSTANCEID=@INSTANCEID,FLOWSELECTTYPE=@FLOWSELECTTYPE,MODELCODE=@MODELCODE,FLOWCODE=@FLOWCODE,FORMID=@FORMID,FLOWTYPE=@FLOWTYPE,CHECKSTATE=@CHECKSTATE,CREATEUSERID=@CREATEUSERID,CREATEUSERNAME=@CREATEUSERNAME,CREATECOMPANYID=@CREATECOMPANYID,CREATEDEPARTMENTID=@CREATEDEPARTMENTID,CREATEPOSTID=@CREATEPOSTID,CREATEDATE=@CREATEDATE,EDITUSERID=@EDITUSERID,EDITUSERNAME=@EDITUSERNAME,EDITDATE=@EDITDATE,ACTIVEROLE=@ACTIVEROLE,BUSINESSOBJECT=@BUSINESSOBJECT,KPITIMEXML=@KPITIMEXML WHERE   FLOWRECORDMASTERID=@FLOWRECORDMASTERID";
                Parameter[] pageparm =
                {               
                    new Parameter("@FLOWRECORDMASTERID"), 
                    new Parameter("@INSTANCEID"), 
                    new Parameter("@FLOWSELECTTYPE"), 
                    new Parameter("@MODELCODE"), 
                    new Parameter("@FLOWCODE"), 
                    new Parameter("@FORMID"), 
                    new Parameter("@FLOWTYPE"), 
                    new Parameter("@CHECKSTATE"), 
                    new Parameter("@CREATEUSERID"), 
                    new Parameter("@CREATEUSERNAME"), 
                    new Parameter("@CREATECOMPANYID"), 
                    new Parameter("@CREATEDEPARTMENTID"), 
                    new Parameter("@CREATEPOSTID"), 
                    new Parameter("@CREATEDATE"), 
                    new Parameter("@EDITUSERID"), 
                    new Parameter("@EDITUSERNAME"), 
                    new Parameter("@EDITDATE"), 
                    new Parameter("@ACTIVEROLE"), 
                    new Parameter("@BUSINESSOBJECT"), 
                    new Parameter("@KPITIMEXML") 

                };
                pageparm[0].ParameterValue = GetValue(model.FLOWRECORDMASTERID);//
                pageparm[1].ParameterValue = GetValue(model.INSTANCEID);//
                pageparm[2].ParameterValue = GetValue(model.FLOWSELECTTYPE);//0:固定流程，1：自选流程
                pageparm[3].ParameterValue = GetValue(model.MODELCODE);//
                pageparm[4].ParameterValue = GetValue(model.FLOWCODE);//
                pageparm[5].ParameterValue = GetValue(model.FORMID);//
                pageparm[6].ParameterValue = GetValue(model.FLOWTYPE);//0:审批流程，1：任务流程
                pageparm[7].ParameterValue = GetValue(model.CHECKSTATE);//1:审批中，2：审批通过，3审批不通过，5撤销(为与字典保持一致)
                pageparm[8].ParameterValue = GetValue(model.CREATEUSERID);//
                pageparm[9].ParameterValue = GetValue(model.CREATEUSERNAME);//
                pageparm[10].ParameterValue = GetValue(model.CREATECOMPANYID);//
                pageparm[11].ParameterValue = GetValue(model.CREATEDEPARTMENTID);//
                pageparm[12].ParameterValue = GetValue(model.CREATEPOSTID);//
                pageparm[13].ParameterValue = GetValue(model.CREATEDATE);//
                pageparm[14].ParameterValue = GetValue(model.EDITUSERID);//
                pageparm[15].ParameterValue = GetValue(model.EDITUSERNAME);//
                pageparm[16].ParameterValue = GetValue(model.EDITDATE);//
                pageparm[17].ParameterValue = GetValue(model.ACTIVEROLE);//
                pageparm[18].ParameterValue = GetValue(model.BUSINESSOBJECT);//
                pageparm[19].ParameterValue = GetValue(model.KPITIMEXML);//

                int n = dao.ExecuteNonQuery(updSql, pageparm);
                Tracer.Debug("FLOW_FLOWRECORDMASTER_TDAL->Update 更新：[流程审批实例表]成功：FLOWRECORDDETAILID＝" + model.FLOWRECORDMASTERID + ";EDITUSERID=" + model.EDITUSERID + ";CHECKSTATE=" + model.CHECKSTATE + ";时间：" + DateTime.Now.ToString());
                return n;

            }
            catch (Exception ex)
            {
                Tracer.Debug("FLOW_FLOWRECORDMASTER_TDAL->Update 更新：[流程审批实例表]失败：FLOWRECORDDETAILID＝" + model.FLOWRECORDMASTERID + ";EDITUSERID=" + model.EDITUSERID + ";CHECKSTATE=" + model.CHECKSTATE + ";时间：" + DateTime.Now.ToString() + "\r\n异常信息：" + ex.Message);

                throw new Exception("FLOW_FLOWRECORDMASTER_TDAL->Update:" + ex.Message);
            }


        }
        /// <summary>
        /// 更新：[流程审批实例表]
        /// </summary>
        /// <param name="con">//OracleConnection 连接对象</param>
        /// <param name="master">流程审批实例表</param>
        public static int UpdateMasterINSTANCEID(FLOW_FLOWRECORDMASTER_T master)
        {
            try
            {
                string updSql = "UPDATE FLOW_FLOWRECORDMASTER_T SET INSTANCEID=@INSTANCEID WHERE   FLOWRECORDMASTERID=@FLOWRECORDMASTERID";
                Parameter[] pageparm =
                {               
                    new Parameter("@FLOWRECORDMASTERID"), 
                    new Parameter("@INSTANCEID") 

                };
                pageparm[0].ParameterValue = master.FLOWRECORDMASTERID;//
                pageparm[1].ParameterValue = master.INSTANCEID;//

                int n = dao.ExecuteNonQuery(updSql, pageparm);
                Tracer.Debug("FLOW_FLOWRECORDMASTER_TDAL->UpdateMasterINSTANCEID 更新：[流程审批实例表]成功：master.FLOWRECORDMASTERID＝" + master.FLOWRECORDMASTERID + ";master.INSTANCEID=" + master.INSTANCEID + ";时间：" + DateTime.Now.ToString());
                return n;
            }
            catch (Exception ex)
            {
                Tracer.Debug("FLOW_FLOWRECORDMASTER_TDAL->UpdateMasterINSTANCEID 更新：[流程审批实例表]失败：master.FLOWRECORDMASTERID＝" + master.FLOWRECORDMASTERID + ";master.INSTANCEID=" + master.INSTANCEID + ";时间：" + DateTime.Now.ToString() + "\r\n异常信息：" + ex.Message);

                throw new Exception("FLOW_FLOWRECORDMASTER_TDAL->UpdateMasterINSTANCEID:" + ex.Message);
            }
        }
        public static List<FLOW_FLOWRECORDMASTER_T> GetFlowRecordBySubmitUserID(string CheckState, string EditUserID)
        {
            List<FLOW_FLOWRECORDMASTER_T> listMaster = new List<FLOW_FLOWRECORDMASTER_T>();
            List<string> listMasterID = new List<string>();

            IDataReader dr = null;
            try
            {
                StringBuilder sbMaster = new StringBuilder();
                sbMaster.Append(@"select FLOWRECORDMASTERID,INSTANCEID,MODELCODE,FLOWCODE,
                                        FORMID,CHECKSTATE,CREATEUSERID,CREATEUSERNAME,CREATECOMPANYID,CREATEDEPARTMENTID,CREATEPOSTID,
                                         CREATEDATE,EDITUSERID,EDITUSERNAME,EDITDATE,FLOWTYPE,FLOWSELECTTYPE
                                         from FLOW_FLOWRECORDMASTER_T where 1=1 ");
                if (!string.IsNullOrEmpty(EditUserID))
                {
                    sbMaster.Append(" and CREATEUSERID='" + EditUserID + "'");
                }

                if (!string.IsNullOrEmpty(CheckState))
                {
                    sbMaster.Append(" and CHECKSTATE='" + CheckState + "'");
                }


                #region

                ////OracleCommand cmd = con.CreateCommand();
                ////cmd.CommandText = sbMaster.ToString();
                //dr = cmd.ExecuteReader();
                dr = dao.ExecuteReader(sbMaster.ToString());
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
                    #region detail
                    string sql = @"select * from FLOW_FLOWRECORDDETAIL_T where FLOWRECORDMASTERID in (" + string.Join(",", listMasterID.ToArray()) + ")";
                    //dr = cmd.ExecuteReader();
                    dr = dao.ExecuteReader(sql);
                    while (dr.Read())
                    {
                        #region detail
                        FLOW_FLOWRECORDDETAIL_T detail = new FLOW_FLOWRECORDDETAIL_T();
                        detail.FLOW_FLOWRECORDMASTER_T = listMaster.FirstOrDefault(m => m.FLOWRECORDMASTERID == dr["FLOWRECORDMASTERID"].ToString());
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
                        #endregion
                    }
                    dr.Close();
                    #endregion
                }

                return listMaster;
                #endregion


            }
            catch (Exception ex)
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
                throw ex;
            }


        }


        public static string IsExistFlowDataByUserID(string UserID, string PostID)
        {

            string message = "";
            string errSQL = "";
            //bool result = false;
            try
            {
                #region  离职人员未处理的单据
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                if (!string.IsNullOrEmpty(PostID))
                {
                    StringBuilder sb = new StringBuilder();
                    //轮到自己审核，但未审批
                    sb.AppendLine("select   d.description,c.createusername,c.createdate,c.formid from ");
                    sb.AppendLine("(");
                    sb.AppendLine(" select t.*,t.rowid from flow_flowrecordmaster_t t where t.flowrecordmasterid in");
                    sb.AppendLine(" (");
                    sb.AppendLine("   select flowrecordmasterid from flow_flowrecorddetail_t  where flag='0' and edituserid='" + UserID + "'  and EDITPOSTID='" + PostID + "'");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" ) c left join  flow_modeldefine_t d  on c.modelcode=d.modelcode order by c.modelcode");
                    dt1 = dao.GetDataTable(sb.ToString());
                    errSQL += sb.ToString()+"\r\n";
                    sb = new StringBuilder();
                    //自己创建的单还在审核中
                    sb.AppendLine("select b.description,a.* from ");
                    sb.AppendLine("(");
                    sb.AppendLine("select d.modelcode,c.editusername,c.createdate,d.formid from ");
                    sb.AppendLine("(");
                    sb.AppendLine(" select t.* from flow_flowrecordmaster_t t where  t.checkstate='1' and t.createuserid='" + UserID + "' and t.CREATEPOSTID='" + PostID + "' ");
                    sb.AppendLine(" ) d left join flow_flowrecorddetail_t c ");
                    sb.AppendLine("on d.flowrecordmasterid=c.flowrecordmasterid and c.checkstate='2' and c.flag='0'");
                    sb.AppendLine(") a   left join  flow_modeldefine_t b on a.modelcode=b.modelcode order by b.modelcode");
                                       
                    dt2 = dao.GetDataTable( sb.ToString());
                    errSQL += sb.ToString() + "\r\n";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    //轮到自己审核，但未审批
                    sb.AppendLine("select   d.description,c.createusername,c.createdate,c.formidfrom ");
                    sb.AppendLine("(");
                    sb.AppendLine(" select t.*,t.rowid from flow_flowrecordmaster_t t where t.flowrecordmasterid in");
                    sb.AppendLine(" (");
                    sb.AppendLine("   select flowrecordmasterid from flow_flowrecorddetail_t  where flag='0' and edituserid='" + UserID + "'");
                    sb.AppendLine(" ) ");
                    sb.AppendLine(" ) c left join  flow_modeldefine_t d  on c.modelcode=d.modelcode order by c.modelcode");
                    dt1 = dao.GetDataTable(sb.ToString());
                    errSQL += sb.ToString() + "\r\n";
                    sb = new StringBuilder();
                    //自己创建的单还在审核中
                    sb.AppendLine("select b.description,a.* from ");
                    sb.AppendLine("(");
                    sb.AppendLine("select d.modelcode,c.editusername,c.createdate,d.formid from ");
                    sb.AppendLine("(");
                    sb.AppendLine(" select t.* from flow_flowrecordmaster_t t where  t.checkstate='1' and t.createuserid='" + UserID + "' ");
                    sb.AppendLine(" ) d left join flow_flowrecorddetail_t c ");
                    sb.AppendLine("on d.flowrecordmasterid=c.flowrecordmasterid and c.checkstate='2' and c.flag='0'");
                    sb.AppendLine(") a   left join  flow_modeldefine_t b on a.modelcode=b.modelcode order by b.modelcode");
                   
                    dt2 = dao.GetDataTable( sb.ToString());
                    errSQL += sb.ToString() + "\r\n";
                }
                if (dt1.Rows.Count > 0)
                {
                    message += "该员工待办的单据，还没有审核的有：\r\n";
                    message += "------------------------------------------\r\n";
                    foreach (DataRow row in dt1.Rows)
                    {
                        message += row["description"].ToString() + "　提单人：" + row["createusername"].ToString() + " 创建时间：" + row["createdate"].ToString() + " Formid=" + row["formid"].ToString() + "\r\n";
                        
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    message += "该员工自己创建的单据，还在审核中的有：\r\n";
                    message += "----------------------------------------------\r\n";
                    foreach (DataRow row in dt2.Rows)
                    {
                        message += row["description"].ToString() + "　审核人：" + row["editusername"].ToString() + " 创建时间：" + row["createdate"].ToString() + "  Formid=" + row["formid"].ToString() + "\r\n";

                    }
                }
                #endregion

                return message;
//                string sql = @"select count(*) from
//                                            (
//                                            select flowrecordmasterid from flow_flowrecorddetail_t  where flag='0' and edituserid='{0}' and editpostid='{1}'
//                                            union all
//                                            select flowrecordmasterid from flow_flowrecordmaster_t  where checkstate='1' and createuserid='{0}' and createpostid='{1}'
//                                            ) t";
//                sql = string.Format(sql, UserID, PostID);
                
//                object obj = dao.ExecuteScalar(con, sql, null);
//                if (obj != null && obj != DBNull.Value)
//                {
//                    if (Convert.ToInt32(obj.ToString()) > 0)
//                    {
//                        result = true;
//                    }
//                }

//                return result;


            }
            catch (Exception ex)
            {
                Tracer.Debug("获取离职人员未处理单据出错 SQL=" + errSQL + "\r\n异常信息：" + ex.ToString());
                throw ex;
            }


        }

        public static FLOW_FLOWRECORDMASTER_T GetFLOW_FLOWRECORDMASTER_T(string masterID)
        {

            IDataReader dr = null;
            try
            {
                string sql = @"select * from FLOW_FLOWRECORDMASTER_T where flowrecordmasterid='{0}'";
                FLOW_FLOWRECORDMASTER_T master = null;
                sql = string.Format(sql, masterID);
                ////OracleCommand command = con.CreateCommand();
                //command.CommandText = sql;
                //dr = command.ExecuteReader();
                dr = dao.ExecuteReader(sql);
                while (dr.Read())
                {
                    #region master
                    master = new FLOW_FLOWRECORDMASTER_T();
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

                    #endregion

                }
                dr.Close();
                return master;
            }
            catch (Exception ex)
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
                throw new Exception("GetFLOW_FLOWRECORDMASTER_T:" + ex.Message + ex.InnerException);
            }


        }
        #endregion


    }


}
