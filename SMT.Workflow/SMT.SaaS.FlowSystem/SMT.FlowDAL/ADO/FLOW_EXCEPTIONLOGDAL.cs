/*---------------------------------------------------------------------  
	 * 版　权：Copyright ©  SmtOnline  2012     
	 * 文件名：FLOW_EXCEPTIONLOGDAL.cs  
	 * 创建者：提莫科技   
	 * 创建日期：2012/9/7 16:32:15   
	 * NET版本： 4.0.30319.225 
	 * 命名空间：SMT.Workflow.Monitoring.DAL 
	 * 模块名称：异常记录日志
	 * 描　　述： 对流程监控产生的日志进行处理
	 * 修改人员：
	 * 修改日期：
	 * 修改内容：
* ---------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using SMT.Workflow.Common.Model;
using SMT.FlowDAL;
using SMT.Foundation.Core;


namespace SMT.FLOWDAL.ADO
{
    /// <summary>
    /// 异常记录日志
    /// </summary>
    public class FLOW_EXCEPTIONLOGDAL : BaseFlowDAL
    {
        /// <summary>
        /// 增加异常记录(以实体传值)
        /// </summary>
        /// <param name="conn">//OracleConnection </param>
        /// <param name="model">FLOW_EXCEPTIONLOG</param>
        /// <returns></returns>
        public int Add(FLOW_EXCEPTIONLOG model)
        {
            try
            {
                string insSql = "INSERT INTO FLOW_EXCEPTIONLOG (ID,FORMID,MODELCODE,CREATEDATE,CREATENAME,SUBMITINFO,LOGINFO,MODELNAME,OWNERID,OWNERNAME,OWNERCOMPANYID,OWNERCOMPANYNAME,OWNERDEPARMENTID,OWNERDEPARMENTNAME,OWNERPOSTID,OWNERPOSTNAME,AUDITSTATE) VALUES (:ID,:FORMID,:MODELCODE,:CREATEDATE,:CREATENAME,:SUBMITINFO,:LOGINFO,:MODELNAME,:OWNERID,:OWNERNAME,:OWNERCOMPANYID,:OWNERCOMPANYNAME,:OWNERDEPARMENTID,:OWNERDEPARMENTNAME,:OWNERPOSTID,:OWNERPOSTNAME,:AUDITSTATE)";
                Parameter[] pageparm =
                {               
                    new Parameter(":ID",null), 
                    new Parameter(":FORMID",null), 
                    new Parameter(":MODELCODE",null), 
                    new Parameter(":CREATEDATE",null), 
                    new Parameter(":CREATENAME",null), 
                    new Parameter(":SUBMITINFO",null), 
                    new Parameter(":LOGINFO",null), 
                    new Parameter(":MODELNAME",null), 
                    new Parameter(":OWNERID",null), 
                    new Parameter(":OWNERNAME",null), 
                    new Parameter(":OWNERCOMPANYID",null), 
                    new Parameter(":OWNERCOMPANYNAME",null), 
                    new Parameter(":OWNERDEPARMENTID",null), 
                    new Parameter(":OWNERDEPARMENTNAME",null), 
                    new Parameter(":OWNERPOSTID",null), 
                    new Parameter(":OWNERPOSTNAME",null), 
                    new Parameter(":AUDITSTATE",null) 

                   

                };
                pageparm[0].ParameterValue = (model.ID);//主键ID
                pageparm[1].ParameterValue = (model.FORMID);//业务ID
                pageparm[2].ParameterValue = (model.MODELCODE);//模块代码
                pageparm[3].ParameterValue = (model.CREATEDATE);//创建日期
                pageparm[4].ParameterValue = (model.CREATENAME);//创建人
                pageparm[5].ParameterValue = (model.SUBMITINFO);//提交信息
                pageparm[6].ParameterValue = (model.LOGINFO);//异常日志信息
                pageparm[7].ParameterValue = (model.MODELNAME);//模块名称
                pageparm[8].ParameterValue = (model.OWNERID);//单据所属人ID
                pageparm[9].ParameterValue = (model.OWNERNAME);//单据所属人姓名
                pageparm[10].ParameterValue = (model.OWNERCOMPANYID);//单据所属人公司ID
                pageparm[11].ParameterValue = (model.OWNERCOMPANYNAME);//单据所属人公司名称
                pageparm[12].ParameterValue = (model.OWNERDEPARMENTID);//单据所属人部门ID
                pageparm[13].ParameterValue = (model.OWNERDEPARMENTNAME);//单据所属人部门名称
                pageparm[14].ParameterValue = (model.OWNERPOSTID);//单据所属人岗位ID
                pageparm[15].ParameterValue = (model.OWNERPOSTNAME);//单据所属人岗位名称
                pageparm[16].ParameterValue = (model.AUDITSTATE);//审核状态;审核通过,审核不通过


                return ExecuteSQL(insSql, pageparm);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        /// <summary>
        /// 运维人员处理异常记录
        /// </summary>
        /// <param name="conn">//OracleConnection</param>
        /// <param name="model">FLOW_EXCEPTIONLOG</param>
        /// <returns></returns>
        public int Update(FLOW_EXCEPTIONLOG model)
        {
            try
            {
                string updSql = "UPDATE FLOW_EXCEPTIONLOG SET STATE=:STATE,UPDATEDATE=:UPDATEDATE,UPDATENAME=:UPDATENAME,REMARK=:REMARK WHERE   ID=:ID";
                Parameter[] pageparm =
                {               
                    new Parameter(":ID",null), 
                    new Parameter(":STATE",null), 
                    new Parameter(":UPDATEDATE",OracleType.DateTime), 
                    new Parameter(":UPDATENAME",null), 
                    new Parameter(":REMARK",null) 
                };
                pageparm[0].ParameterValue = (model.ID);//主键ID
                pageparm[1].ParameterValue = (model.STATE);//状态:未处理;已处理
                pageparm[2].ParameterValue = (DateTime.Now);//处理日期
                pageparm[3].ParameterValue = (model.UPDATENAME);//处理人
                pageparm[4].ParameterValue = (model.REMARK);//备注
                return ExecuteSQL(updSql, pageparm);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
