/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©  SmtOnline  2012     
	 * 文件名：FLOW_INSTANCE_STATEDAL.cs  
	 * 创建者：LONGKC   
	 * 创建日期：2012/9/10 16:31:33   
	 * NET版本： 4.0.30319.225 
	 * 命名空间：SMT.Workflow.Monitoring.DAL 
	 * 模块名称：流程监控
	 * 描　　述： 流程审核过程中的持久化实例
	 * 修改人员：
	 * 修改日期：
	 * 修改内容：
* ---------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using SMT.FlowDAL;
using SMT.Workflow.Common.Model;
using System.Data;
using SMT.Foundation.Core;

namespace SMT.FLOWDAL.ADO
{
    /// <summary>
    /// [流程审核过程中的持久化实例]
    /// </summary>
   public  class FLOW_INSTANCE_STATEDAL:BaseFlowDAL
    {
      /// <summary>
        /// 得到审核主表最新的一个实体
      /// </summary>
        /// <param name="conn">//OracleConnection</param>
        /// <param name="formid">formid</param>
      /// <returns></returns>
       public FLOW_FLOWRECORDMASTER_T GetFlowerMasterIDByFormid(string formid)
       {
           try
           {
               FLOW_FLOWRECORDMASTER_T model = new FLOW_FLOWRECORDMASTER_T();
               string selSql = "SELECT INSTANCEID,FORMID,EDITUSERID,EDITUSERNAME FROM FLOW_FLOWRECORDMASTER_T WHERE   FORMID=:FORMID   ORDER BY CREATEDATE DESC";
               Parameter[] pageparm =
                {               
                    new Parameter(":FORMID",null) 

                };
               pageparm[0].ParameterValue = formid;
               ParameterCollection pras = new ParameterCollection();
               foreach (var item in pageparm)
               {
                   pras.Add(item);
               }

               DataTable dt = dao.GetDataTable(selSql, CommandType.Text, pras);
               if (dt.Rows.Count > 0)
               {//多次提交单据的时候，取最新的一条数据
                   model.INSTANCEID = dt.Rows[0]["INSTANCEID"].ToString();// 
                   model.FORMID = dt.Rows[0]["FORMID"].ToString();// 
                   model.EDITUSERID = dt.Rows[0]["EDITUSERID"].ToString();// 
                   model.EDITUSERNAME = dt.Rows[0]["EDITUSERNAME"].ToString();// 

               }
               return model;
           }
           catch (Exception e)
           {
               throw new Exception(e.Message, e);
           }

        }       
       /// <summary>
        /// 增加一条数据(以实体传值)
       /// </summary>
        /// <param name="conn">//OracleConnection</param>
        /// <param name="model">FLOW_INSTANCE_STATE</param>
       /// <returns></returns>
       public int Add( FLOW_INSTANCE_STATE model)
        {
            string insSql = "INSERT INTO FLOW_INSTANCE_STATE (INSTANCE_ID,STATE,STATUS,UNLOCKED,BLOCKED,INFO,MODIFIED,OWNER_ID,OWNED_UNTIL,NEXT_TIMER,FORMID,CREATEID,CREATENAME,EDITID,EDITNAME) VALUES (:INSTANCE_ID,:STATE,:STATUS,:UNLOCKED,:BLOCKED,:INFO,:MODIFIED,:OWNER_ID,:OWNED_UNTIL,:NEXT_TIMER,:FORMID,:CREATEID,:CREATENAME,:EDITID,:EDITNAME)";
            Parameter[] pageparm =
                {               
                    new Parameter(":INSTANCE_ID",null), 
                    new Parameter(":STATE",null), 
                    new Parameter(":STATUS",null), 
                    new Parameter(":UNLOCKED",null), 
                    new Parameter(":BLOCKED",null), 
                    new Parameter(":INFO",null), 
                    new Parameter(":MODIFIED",null), 
                    new Parameter(":OWNER_ID",null), 
                    new Parameter(":OWNED_UNTIL",null), 
                    new Parameter(":NEXT_TIMER",null), 
                    new Parameter(":FORMID",null), 
                    new Parameter(":CREATEID",null), 
                    new Parameter(":CREATENAME",null), 
                    new Parameter(":EDITID",null), 
                    new Parameter(":EDITNAME",null) 
                  

                };
            pageparm[0].ParameterValue = GetValue(model.INSTANCE_ID);//
          pageparm[1].ParameterValue = GetValue(model.STATE);//
          pageparm[2].ParameterValue = GetValue(model.STATUS);//
          pageparm[3].ParameterValue = GetValue(model.UNLOCKED);//
          pageparm[4].ParameterValue = GetValue(model.BLOCKED);//
          pageparm[5].ParameterValue = GetValue(model.INFO);//
          pageparm[6].ParameterValue = GetValue(model.MODIFIED);//
          pageparm[7].ParameterValue = GetValue(model.OWNER_ID);//
          pageparm[8].ParameterValue = GetValue(model.OWNED_UNTIL);//
          pageparm[9].ParameterValue = GetValue(model.NEXT_TIMER);//
          pageparm[10].ParameterValue = GetValue(model.FORMID);//
          pageparm[11].ParameterValue = GetValue(model.CREATEID);//创建人ID
          pageparm[12].ParameterValue = GetValue(model.CREATENAME);//创建人姓名
          pageparm[13].ParameterValue = GetValue(model.EDITID);//下一个审核人ID
          pageparm[14].ParameterValue = GetValue(model.EDITNAME);//下一个审核人姓名

          return ExecuteSQL(insSql, pageparm);
        }     
       /// <summary>
       /// 得到流程持久化实例一个对象实体
       /// </summary>
       /// <param name="conn">//OracleConnection</param>
       /// <param name="instance_id">实例ID</param>
       /// <returns></returns>
       public FLOW_INSTANCE_STATE GetInstanceModel(string instance_id)
       {
           FLOW_INSTANCE_STATE model = new FLOW_INSTANCE_STATE();
           string selSql = "SELECT INSTANCE_ID,STATE,STATUS,UNLOCKED,BLOCKED,INFO,MODIFIED,OWNER_ID,OWNED_UNTIL,NEXT_TIMER FROM INSTANCE_STATE WHERE   INSTANCE_ID=:INSTANCE_ID";
           Parameter[] pageparm =
                {               
                    new Parameter(":INSTANCE_ID",null) 

                };
           pageparm[0].ParameterValue = instance_id;
           ParameterCollection pras = new ParameterCollection();
           foreach (var item in pageparm)
           {
               pras.Add(item);
           }

           IDataReader dr = dao.ExecuteReader(selSql, pras);
           if (dr.Read())
           {
               model.INSTANCE_ID = dr["INSTANCE_ID"].ToString();// 
               model.STATE = (byte[])dr["STATE"];//   
               model.STATUS = dr["STATUS"].ToString() != "" ? Convert.ToDecimal(dr["STATUS"]) : 0; //  
               model.UNLOCKED = dr["UNLOCKED"].ToString() != "" ? Convert.ToDecimal(dr["UNLOCKED"]) : 0; //  
               model.BLOCKED = dr["BLOCKED"].ToString() != "" ? Convert.ToDecimal(dr["BLOCKED"]) : 0; //  
               model.INFO = dr["INFO"].ToString();// 
               model.MODIFIED = dr["MODIFIED"].ToString() != "" ? Convert.ToDateTime(dr["MODIFIED"]) : DateTime.Now; //  
               model.OWNER_ID = dr["OWNER_ID"].ToString();// 
               model.OWNED_UNTIL = dr["OWNED_UNTIL"].ToString() != "" ? Convert.ToDateTime(dr["OWNED_UNTIL"]) : DateTime.Now; //  
               model.NEXT_TIMER = dr["NEXT_TIMER"].ToString() != "" ? Convert.ToDateTime(dr["NEXT_TIMER"]) : DateTime.Now; //  
               
           }
           dr.Close();
           
           return model;
       }
    }
}
